using ListaTarefas.Data;
using ListaTarefas.Exceptions;
using ListaTarefas.Forms;
using ListaTarefas.Infra;
using ListaTarefas.Services;

namespace ListaTarefas;

internal static class Program
{
    private const string TituloAplicacao = "Lista de Tarefas";

    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
        Application.ThreadException += (_, e) => TratarErroInesperado(e.Exception);

        TarefaService service;
        try
        {
            service = MontarServicos();
        }
        catch (Exception erro) when (erro is AcessoDadosException or IOException or UnauthorizedAccessException)
        {
            LogErros.Registrar(erro);
            string mensagem = erro is AcessoDadosException
                ? erro.Message
                : "Não foi possível acessar a pasta de dados da aplicação.";
            MessageBox.Show(mensagem, TituloAplicacao, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        Application.Run(new MainForm(service));
    }

    /// <summary>Raiz de composição: único lugar que conhece todas as camadas.</summary>
    private static TarefaService MontarServicos()
    {
        PastaDeDados.Inicializar();

        var conexaoFactory = new ConexaoFactory();
        new BancoInicializador(conexaoFactory).Inicializar();

        return new TarefaService(new TarefaRepository(conexaoFactory));
    }

    private static void TratarErroInesperado(Exception erro)
    {
        LogErros.Registrar(erro);
        MessageBox.Show(
            "Ocorreu um erro inesperado. Os detalhes foram gravados no arquivo de log.",
            TituloAplicacao, MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
