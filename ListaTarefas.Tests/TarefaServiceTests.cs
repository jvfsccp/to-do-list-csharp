using ListaTarefas.Exceptions;
using ListaTarefas.Models;

namespace ListaTarefas.Tests;

public class TarefaServiceTests : BancoTemporarioFixture
{
    private Tarefa Criar(string titulo, DateTime? inicio = null, DateTime? fim = null)
    {
        var tarefa = new Tarefa { Titulo = titulo, DataInicio = inicio, DataFim = fim };
        Servico.Salvar(tarefa);
        return tarefa;
    }

    private string[] Titulos(bool incluirConcluidas = false) =>
        Servico.Listar(incluirConcluidas).Select(t => t.Titulo).ToArray();

    [Fact]
    public void Salvar_NovaTarefa_GeraIdEVaiParaOFimDaLista()
    {
        Tarefa a = Criar("A");
        Tarefa b = Criar("B");

        Assert.True(a.Id > 0);
        Assert.True(b.Id > a.Id);
        Assert.Equal(new[] { "A", "B" }, Titulos());
    }

    [Fact]
    public void Salvar_GravaEDevolveDatasEDescricao()
    {
        var tarefa = new Tarefa
        {
            Titulo = "  Estudar  ",
            Descricao = "Capítulo 3",
            DataInicio = new DateTime(2026, 10, 1),
            DataFim = new DateTime(2026, 10, 9)
        };
        Servico.Salvar(tarefa);

        Tarefa lida = Assert.Single(Servico.Listar(false));
        Assert.Equal("Estudar", lida.Titulo);
        Assert.Equal("Capítulo 3", lida.Descricao);
        Assert.Equal(new DateTime(2026, 10, 1), lida.DataInicio);
        Assert.Equal(new DateTime(2026, 10, 9), lida.DataFim);
    }

    [Fact]
    public void Salvar_SemDatas_GuardaNulos()
    {
        Criar("Sem datas");

        Tarefa lida = Assert.Single(Servico.Listar(false));
        Assert.Null(lida.DataInicio);
        Assert.Null(lida.DataFim);
        Assert.Null(lida.Descricao);
    }

    [Fact]
    public void Salvar_TarefaExistente_Atualiza()
    {
        Tarefa tarefa = Criar("Antigo");
        tarefa.Titulo = "Novo";
        tarefa.DataFim = new DateTime(2026, 12, 31);
        Servico.Salvar(tarefa);

        Tarefa lida = Assert.Single(Servico.Listar(false));
        Assert.Equal("Novo", lida.Titulo);
        Assert.Equal(new DateTime(2026, 12, 31), lida.DataFim);
    }

    [Fact]
    public void Salvar_TarefaInexistente_LancaRegraNegocio()
    {
        var fantasma = new Tarefa { Id = 999, Titulo = "Fantasma" };

        Assert.Throws<RegraNegocioException>(() => Servico.Salvar(fantasma));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Salvar_TituloVazio_LancaRegraNegocio(string titulo)
    {
        Assert.Throws<RegraNegocioException>(() => Servico.Salvar(new Tarefa { Titulo = titulo }));
    }

    [Fact]
    public void Salvar_TituloMuitoLongo_LancaRegraNegocio()
    {
        var tarefa = new Tarefa { Titulo = new string('x', 101) };

        Assert.Throws<RegraNegocioException>(() => Servico.Salvar(tarefa));
    }

    [Fact]
    public void Salvar_FimAnteriorAoInicio_LancaRegraNegocio()
    {
        var tarefa = new Tarefa
        {
            Titulo = "Datas trocadas",
            DataInicio = new DateTime(2026, 10, 10),
            DataFim = new DateTime(2026, 10, 9)
        };

        Assert.Throws<RegraNegocioException>(() => Servico.Salvar(tarefa));
        Assert.Empty(Servico.Listar(true));
    }

    [Fact]
    public void Salvar_TextoComAspasESql_EGravadoLiteralmente()
    {
        const string malicioso = "x'); DROP TABLE tarefas;--";
        Criar(malicioso);
        Criar("Depois");

        Assert.Equal(new[] { malicioso, "Depois" }, Titulos());
    }

    [Fact]
    public void AlterarConclusao_EscondeDaListaPadraoEGravaData()
    {
        Tarefa a = Criar("A");
        Criar("B");

        Servico.AlterarConclusao(a.Id, true);

        Assert.Equal(new[] { "B" }, Titulos());
        Tarefa concluida = Assert.Single(Servico.Listar(true), t => t.Concluida);
        Assert.Equal("A", concluida.Titulo);
        Assert.NotNull(concluida.DataConclusao);
        Assert.Equal(DateTime.Today, concluida.DataConclusao!.Value.Date);
    }

    [Fact]
    public void AlterarConclusao_Reabrir_LimpaDataEVoltaParaLista()
    {
        Tarefa a = Criar("A");
        Servico.AlterarConclusao(a.Id, true);
        Servico.AlterarConclusao(a.Id, false);

        Tarefa lida = Assert.Single(Servico.Listar(false));
        Assert.False(lida.Concluida);
        Assert.Null(lida.DataConclusao);
    }

    [Fact]
    public void Listar_ComConcluidas_MostraPendentesAntes()
    {
        Tarefa a = Criar("A");
        Criar("B");
        Servico.AlterarConclusao(a.Id, true);

        Assert.Equal(new[] { "B", "A" }, Titulos(incluirConcluidas: true));
    }

    [Fact]
    public void Excluir_RemoveATarefa()
    {
        Tarefa a = Criar("A");
        Criar("B");

        Servico.Excluir(a.Id);

        Assert.Equal(new[] { "B" }, Titulos(true));
    }

    [Fact]
    public void Excluir_Inexistente_LancaRegraNegocio()
    {
        Assert.Throws<RegraNegocioException>(() => Servico.Excluir(999));
    }

    [Fact]
    public void Mover_Cima_TrocaComAAnterior()
    {
        Criar("A");
        Criar("B");
        Tarefa c = Criar("C");

        Servico.Mover(c.Id, DirecaoMovimento.Cima);

        Assert.Equal(new[] { "A", "C", "B" }, Titulos());
    }

    [Fact]
    public void Mover_Baixo_TrocaComAPosterior()
    {
        Tarefa a = Criar("A");
        Criar("B");
        Criar("C");

        Servico.Mover(a.Id, DirecaoMovimento.Baixo);

        Assert.Equal(new[] { "B", "A", "C" }, Titulos());
    }

    [Fact]
    public void Mover_NaPrimeiraPosicaoParaCima_NaoAltera()
    {
        Tarefa a = Criar("A");
        Criar("B");

        Servico.Mover(a.Id, DirecaoMovimento.Cima);

        Assert.Equal(new[] { "A", "B" }, Titulos());
    }

    [Fact]
    public void Mover_NaUltimaPosicaoParaBaixo_NaoAltera()
    {
        Criar("A");
        Tarefa b = Criar("B");

        Servico.Mover(b.Id, DirecaoMovimento.Baixo);

        Assert.Equal(new[] { "A", "B" }, Titulos());
    }

    [Fact]
    public void Mover_IgnoraTarefasConcluidasNoMeio()
    {
        Criar("A");
        Tarefa b = Criar("B");
        Tarefa c = Criar("C");
        Servico.AlterarConclusao(b.Id, true);

        Servico.Mover(c.Id, DirecaoMovimento.Cima);

        Assert.Equal(new[] { "C", "A" }, Titulos());
    }

    [Fact]
    public void Mover_TarefaConcluida_NaoAltera()
    {
        Tarefa a = Criar("A");
        Criar("B");
        Servico.AlterarConclusao(a.Id, true);

        Servico.Mover(a.Id, DirecaoMovimento.Baixo);

        Assert.Equal(new[] { "B", "A" }, Titulos(true));
    }

    [Fact]
    public void NovaTarefa_AposExcluirUltima_ContinuaNoFim()
    {
        Criar("A");
        Tarefa b = Criar("B");
        Servico.Excluir(b.Id);
        Criar("C");

        Assert.Equal(new[] { "A", "C" }, Titulos());
    }

    [Fact]
    public void Repositorio_BancoInacessivel_LancaAcessoDados()
    {
        string caminhoInvalido = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"), "nao-existe", "x.db");
        var repositorio = new Data.TarefaRepository(new Data.ConexaoFactory($"Data Source={caminhoInvalido};Pooling=False"));

        var erro = Assert.Throws<AcessoDadosException>(() => repositorio.Listar(false));
        Assert.IsType<Microsoft.Data.Sqlite.SqliteException>(erro.InnerException);
        Assert.DoesNotContain("SqliteException", erro.Message);
    }
}

public class TarefaModeloTests
{
    [Fact]
    public void Atrasada_SoQuandoFimPassouENaoConcluida()
    {
        DateTime ontem = DateTime.Today.AddDays(-1);

        Assert.True(new Tarefa { DataFim = ontem }.Atrasada);
        Assert.False(new Tarefa { DataFim = ontem, Concluida = true }.Atrasada);
        Assert.False(new Tarefa { DataFim = DateTime.Today }.Atrasada);
        Assert.False(new Tarefa { DataFim = null }.Atrasada);
    }

    [Theory]
    [InlineData(-1, false, "Atrasada")]
    [InlineData(3, false, "No prazo")]
    [InlineData(-1, true, "Concluída")]
    public void Situacao_RefleteOEstado(int diasParaOFim, bool concluida, string esperado)
    {
        var tarefa = new Tarefa { DataFim = DateTime.Today.AddDays(diasParaOFim), Concluida = concluida };

        Assert.Equal(esperado, tarefa.Situacao);
    }

    [Fact]
    public void Situacao_SemData_SemPrazo()
    {
        Assert.Equal("Sem prazo", new Tarefa().Situacao);
    }
}
