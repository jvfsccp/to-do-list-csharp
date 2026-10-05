namespace ListaTarefas.Infra;

/// <summary>Pasta do usuário (%LocalAppData%\ListaTarefas) onde ficam o banco e o log.</summary>
public static class PastaDeDados
{
    public static string Caminho { get; } = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "ListaTarefas");

    /// <summary>Cria a pasta e a expõe como |DataDirectory| para a connection string.</summary>
    public static void Inicializar()
    {
        Directory.CreateDirectory(Caminho);
        AppDomain.CurrentDomain.SetData("DataDirectory", Caminho);
    }
}
