using System.Diagnostics;

namespace ListaTarefas.Infra;

public static class LogErros
{
    public static void Registrar(Exception erro)
    {
        try
        {
            string arquivo = Path.Combine(PastaDeDados.Caminho, "erros.log");
            string registro = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {erro}{Environment.NewLine}{Environment.NewLine}";
            File.AppendAllText(arquivo, registro);
        }
        catch (Exception falhaNoLog) when (falhaNoLog is IOException or UnauthorizedAccessException)
        {
            // O log nunca pode derrubar a aplicação; na falha só resta o Debug.
            Debug.WriteLine(falhaNoLog);
        }
    }
}
