using ListaTarefas.Exceptions;
using ListaTarefas.Infra;
using Microsoft.Data.Sqlite;

namespace ListaTarefas.Data;

/// <summary>Cria o arquivo e a tabela no primeiro uso (espelha database/schema.sql).</summary>
public sealed class BancoInicializador
{
    private const string SqlCriarTabela = @"
        CREATE TABLE IF NOT EXISTS tarefas (
            id             INTEGER PRIMARY KEY AUTOINCREMENT,
            titulo         TEXT    NOT NULL,
            descricao      TEXT    NULL,
            data_inicio    TEXT    NULL,
            data_fim       TEXT    NULL,
            concluida      INTEGER NOT NULL DEFAULT 0 CHECK (concluida IN (0, 1)),
            data_conclusao TEXT    NULL,
            ordem          INTEGER NOT NULL
        );";

    private readonly ConexaoFactory _conexaoFactory;

    public BancoInicializador(ConexaoFactory conexaoFactory)
    {
        _conexaoFactory = conexaoFactory;
    }

    public void Inicializar()
    {
        try
        {
            using SqliteConnection conexao = _conexaoFactory.CriarConexao();
            conexao.Open();

            using SqliteCommand comando = conexao.CreateCommand();
            comando.CommandText = SqlCriarTabela;
            comando.ExecuteNonQuery();
        }
        catch (SqliteException erro)
        {
            LogErros.Registrar(erro);
            throw new AcessoDadosException("Não foi possível abrir ou criar o banco de dados.", erro);
        }
    }
}
