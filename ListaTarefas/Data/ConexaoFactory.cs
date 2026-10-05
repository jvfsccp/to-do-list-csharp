using ListaTarefas.Exceptions;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace ListaTarefas.Data;

/// <summary>Cria conexões a partir da connection string do appsettings.json.</summary>
public sealed class ConexaoFactory
{
    private const string NomeConnectionString = "TarefasDb";

    private readonly string _stringConexao;

    public ConexaoFactory()
    {
        try
        {
            IConfigurationRoot configuracao = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            _stringConexao = configuracao.GetConnectionString(NomeConnectionString)
                ?? throw new InvalidOperationException($"Connection string '{NomeConnectionString}' não encontrada.");
        }
        catch (Exception erro) when (erro is FileNotFoundException or InvalidOperationException or InvalidDataException)
        {
            throw new AcessoDadosException(
                "Não foi possível ler o arquivo de configuração (appsettings.json).", erro);
        }
    }

    /// <summary>Devolve uma conexão fechada; quem chama abre e descarta com <c>using</c>.</summary>
    public SqliteConnection CriarConexao() => new(_stringConexao);
}
