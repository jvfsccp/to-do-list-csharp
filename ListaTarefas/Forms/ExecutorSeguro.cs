using ListaTarefas.Exceptions;

namespace ListaTarefas.Forms;

/// <summary>Executa uma ação do serviço e mostra ao usuário uma mensagem amigável se ela falhar.</summary>
internal static class ExecutorSeguro
{
    /// <returns><c>true</c> se a ação terminou sem erro.</returns>
    public static bool Executar(IWin32Window dono, Action acao)
    {
        try
        {
            acao();
            return true;
        }
        catch (RegraNegocioException erro)
        {
            MessageBox.Show(dono, erro.Message, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        catch (AcessoDadosException erro)
        {
            MessageBox.Show(dono, erro.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        return false;
    }
}
