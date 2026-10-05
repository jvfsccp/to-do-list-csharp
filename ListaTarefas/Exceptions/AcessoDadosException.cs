namespace ListaTarefas.Exceptions;

/// <summary>
/// Falha de acesso ao banco ou à configuração. A mensagem é amigável; o erro técnico
/// original fica em <see cref="Exception.InnerException"/> e no arquivo de log.
/// </summary>
public class AcessoDadosException : Exception
{
    public AcessoDadosException(string mensagem, Exception? causa = null) : base(mensagem, causa)
    {
    }
}
