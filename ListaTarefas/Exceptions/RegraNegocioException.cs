namespace ListaTarefas.Exceptions;

/// <summary>Violação de uma regra de negócio; a mensagem já é própria para o usuário.</summary>
public class RegraNegocioException : Exception
{
    public RegraNegocioException(string mensagem) : base(mensagem)
    {
    }
}
