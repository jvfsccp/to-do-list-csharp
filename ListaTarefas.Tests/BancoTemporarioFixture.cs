using ListaTarefas.Data;
using ListaTarefas.Services;

namespace ListaTarefas.Tests;

/// <summary>Cria um banco SQLite novo (arquivo temporário) para cada teste.</summary>
public abstract class BancoTemporarioFixture : IDisposable
{
    private readonly string _arquivo = Path.Combine(Path.GetTempPath(), $"tarefas-teste-{Guid.NewGuid():N}.db");

    protected BancoTemporarioFixture()
    {
        // Pooling desligado para o arquivo ser liberado assim que a conexão fecha.
        var conexaoFactory = new ConexaoFactory($"Data Source={_arquivo};Pooling=False");
        new BancoInicializador(conexaoFactory).Inicializar();

        Repositorio = new TarefaRepository(conexaoFactory);
        Servico = new TarefaService(Repositorio);
    }

    protected TarefaRepository Repositorio { get; }

    protected TarefaService Servico { get; }

    public void Dispose()
    {
        if (File.Exists(_arquivo))
        {
            File.Delete(_arquivo);
        }

        GC.SuppressFinalize(this);
    }
}
