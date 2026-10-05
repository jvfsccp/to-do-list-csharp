using System.Globalization;
using ListaTarefas.Exceptions;
using ListaTarefas.Infra;
using ListaTarefas.Models;
using Microsoft.Data.Sqlite;

namespace ListaTarefas.Data;

public sealed class TarefaRepository : ITarefaRepository
{
    private const string FormatoData = "yyyy-MM-dd";
    private const string FormatoDataHora = "yyyy-MM-dd HH:mm:ss";

    private const string SqlListar = @"
        SELECT id, titulo, descricao, data_inicio, data_fim, concluida, data_conclusao, ordem
        FROM tarefas
        WHERE concluida = 0 OR @incluirConcluidas = 1
        ORDER BY concluida, ordem";

    private const string SqlInserir = @"
        INSERT INTO tarefas (titulo, descricao, data_inicio, data_fim, ordem)
        VALUES (@titulo, @descricao, @dataInicio, @dataFim,
                (SELECT COALESCE(MAX(ordem), 0) + 1 FROM tarefas))
        RETURNING id";

    private const string SqlAtualizar = @"
        UPDATE tarefas
        SET titulo = @titulo, descricao = @descricao, data_inicio = @dataInicio, data_fim = @dataFim
        WHERE id = @id";

    private const string SqlDefinirConclusao = @"
        UPDATE tarefas SET concluida = @concluida, data_conclusao = @dataConclusao WHERE id = @id";

    private const string SqlExcluir = "DELETE FROM tarefas WHERE id = @id";

    private const string SqlOrdemPendente = "SELECT ordem FROM tarefas WHERE id = @id AND concluida = 0";

    private const string SqlVizinhaAcima = @"
        SELECT id, ordem FROM tarefas
        WHERE concluida = 0 AND ordem < @ordem
        ORDER BY ordem DESC LIMIT 1";

    private const string SqlVizinhaAbaixo = @"
        SELECT id, ordem FROM tarefas
        WHERE concluida = 0 AND ordem > @ordem
        ORDER BY ordem ASC LIMIT 1";

    private const string SqlAtualizarOrdem = "UPDATE tarefas SET ordem = @ordem WHERE id = @id";

    private readonly ConexaoFactory _conexaoFactory;

    public TarefaRepository(ConexaoFactory conexaoFactory)
    {
        _conexaoFactory = conexaoFactory;
    }

    public IReadOnlyList<Tarefa> Listar(bool incluirConcluidas)
    {
        return Consultar(conexao =>
        {
            using SqliteCommand comando = CriarComando(conexao, SqlListar);
            comando.Parameters.AddWithValue("@incluirConcluidas", incluirConcluidas ? 1 : 0);

            using SqliteDataReader leitor = comando.ExecuteReader();
            var tarefas = new List<Tarefa>();
            while (leitor.Read())
            {
                tarefas.Add(MapearTarefa(leitor));
            }

            return (IReadOnlyList<Tarefa>)tarefas;
        }, "Não foi possível carregar as tarefas.");
    }

    public int Inserir(Tarefa tarefa)
    {
        return Consultar(conexao =>
        {
            using SqliteCommand comando = CriarComando(conexao, SqlInserir);
            comando.Parameters.AddWithValue("@titulo", tarefa.Titulo);
            comando.Parameters.AddWithValue("@descricao", ParaBancoTexto(tarefa.Descricao));
            comando.Parameters.AddWithValue("@dataInicio", ParaBancoData(tarefa.DataInicio));
            comando.Parameters.AddWithValue("@dataFim", ParaBancoData(tarefa.DataFim));

            return Convert.ToInt32(comando.ExecuteScalar(), CultureInfo.InvariantCulture);
        }, "Não foi possível salvar a tarefa.");
    }

    public void Atualizar(Tarefa tarefa)
    {
        Executar(conexao =>
        {
            using SqliteCommand comando = CriarComando(conexao, SqlAtualizar);
            comando.Parameters.AddWithValue("@id", tarefa.Id);
            comando.Parameters.AddWithValue("@titulo", tarefa.Titulo);
            comando.Parameters.AddWithValue("@descricao", ParaBancoTexto(tarefa.Descricao));
            comando.Parameters.AddWithValue("@dataInicio", ParaBancoData(tarefa.DataInicio));
            comando.Parameters.AddWithValue("@dataFim", ParaBancoData(tarefa.DataFim));

            ExigirLinhaAfetada(comando.ExecuteNonQuery());
        }, "Não foi possível atualizar a tarefa.");
    }

    public void DefinirConclusao(int id, bool concluida, DateTime? dataConclusao)
    {
        Executar(conexao =>
        {
            using SqliteCommand comando = CriarComando(conexao, SqlDefinirConclusao);
            comando.Parameters.AddWithValue("@id", id);
            comando.Parameters.AddWithValue("@concluida", concluida ? 1 : 0);
            comando.Parameters.AddWithValue("@dataConclusao", ParaBancoDataHora(dataConclusao));

            ExigirLinhaAfetada(comando.ExecuteNonQuery());
        }, "Não foi possível alterar a situação da tarefa.");
    }

    public void Excluir(int id)
    {
        Executar(conexao =>
        {
            using SqliteCommand comando = CriarComando(conexao, SqlExcluir);
            comando.Parameters.AddWithValue("@id", id);

            ExigirLinhaAfetada(comando.ExecuteNonQuery());
        }, "Não foi possível excluir a tarefa.");
    }

    public void Mover(int id, DirecaoMovimento direcao)
    {
        Executar(conexao =>
        {
            // Os dois UPDATEs da troca são interdependentes: ou ocorrem juntos ou nenhum.
            // Sem Commit explícito, o Dispose da transação faz o rollback.
            using SqliteTransaction transacao = conexao.BeginTransaction();

            using SqliteCommand lerOrdem = CriarComando(conexao, SqlOrdemPendente, transacao);
            lerOrdem.Parameters.AddWithValue("@id", id);
            object? resultadoOrdem = lerOrdem.ExecuteScalar();
            if (resultadoOrdem is null)
            {
                return; // tarefa inexistente ou já concluída: não participa da ordenação
            }

            int ordemAtual = Convert.ToInt32(resultadoOrdem, CultureInfo.InvariantCulture);

            string sqlVizinha = direcao == DirecaoMovimento.Cima ? SqlVizinhaAcima : SqlVizinhaAbaixo;
            using SqliteCommand lerVizinha = CriarComando(conexao, sqlVizinha, transacao);
            lerVizinha.Parameters.AddWithValue("@ordem", ordemAtual);

            int idVizinha;
            int ordemVizinha;
            using (SqliteDataReader leitor = lerVizinha.ExecuteReader())
            {
                if (!leitor.Read())
                {
                    return; // já está na primeira/última posição
                }

                idVizinha = leitor.GetInt32(0);
                ordemVizinha = leitor.GetInt32(1);
            }

            AtualizarOrdem(conexao, transacao, id, ordemVizinha);
            AtualizarOrdem(conexao, transacao, idVizinha, ordemAtual);
            transacao.Commit();
        }, "Não foi possível alterar a ordem da tarefa.");
    }

    private static void AtualizarOrdem(SqliteConnection conexao, SqliteTransaction transacao, int id, int ordem)
    {
        using SqliteCommand comando = CriarComando(conexao, SqlAtualizarOrdem, transacao);
        comando.Parameters.AddWithValue("@id", id);
        comando.Parameters.AddWithValue("@ordem", ordem);
        comando.ExecuteNonQuery();
    }

    /// <summary>Abre a conexão, executa a operação e converte SqliteException em mensagem amigável.</summary>
    private T Consultar<T>(Func<SqliteConnection, T> operacao, string mensagemErro)
    {
        try
        {
            using SqliteConnection conexao = _conexaoFactory.CriarConexao();
            conexao.Open();
            return operacao(conexao);
        }
        catch (SqliteException erro)
        {
            LogErros.Registrar(erro);
            throw new AcessoDadosException(mensagemErro, erro);
        }
    }

    private void Executar(Action<SqliteConnection> operacao, string mensagemErro)
    {
        Consultar<object?>(conexao =>
        {
            operacao(conexao);
            return null;
        }, mensagemErro);
    }

    private static SqliteCommand CriarComando(SqliteConnection conexao, string sql, SqliteTransaction? transacao = null)
    {
        SqliteCommand comando = conexao.CreateCommand();
        comando.CommandText = sql;
        comando.Transaction = transacao;
        return comando;
    }

    private static void ExigirLinhaAfetada(int linhasAfetadas)
    {
        if (linhasAfetadas == 0)
        {
            throw new RegraNegocioException("A tarefa não foi encontrada. Ela pode ter sido excluída.");
        }
    }

    private static Tarefa MapearTarefa(SqliteDataReader leitor) => new()
    {
        Id = leitor.GetInt32(0),
        Titulo = leitor.GetString(1),
        Descricao = leitor.IsDBNull(2) ? null : leitor.GetString(2),
        DataInicio = LerData(leitor, 3, FormatoData),
        DataFim = LerData(leitor, 4, FormatoData),
        Concluida = leitor.GetInt32(5) == 1,
        DataConclusao = LerData(leitor, 6, FormatoDataHora),
        Ordem = leitor.GetInt32(7)
    };

    private static DateTime? LerData(SqliteDataReader leitor, int coluna, string formato) =>
        leitor.IsDBNull(coluna)
            ? null
            : DateTime.ParseExact(leitor.GetString(coluna), formato, CultureInfo.InvariantCulture);

    private static object ParaBancoTexto(string? texto) =>
        string.IsNullOrWhiteSpace(texto) ? DBNull.Value : texto;

    private static object ParaBancoData(DateTime? data) =>
        data.HasValue ? data.Value.ToString(FormatoData, CultureInfo.InvariantCulture) : DBNull.Value;

    private static object ParaBancoDataHora(DateTime? data) =>
        data.HasValue ? data.Value.ToString(FormatoDataHora, CultureInfo.InvariantCulture) : DBNull.Value;
}
