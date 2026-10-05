namespace ListaTarefas.Models;

public class Tarefa
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public DateTime? DataInicio { get; set; }
    public DateTime? DataFim { get; set; }
    public bool Concluida { get; set; }
    public DateTime? DataConclusao { get; set; }
    public int Ordem { get; set; }

    public bool Atrasada => !Concluida && DataFim.HasValue && DataFim.Value.Date < DateTime.Today;

    public string Situacao
    {
        get
        {
            if (Concluida) return "Concluída";
            if (Atrasada) return "Atrasada";
            return DataFim.HasValue ? "No prazo" : "Sem prazo";
        }
    }
}
