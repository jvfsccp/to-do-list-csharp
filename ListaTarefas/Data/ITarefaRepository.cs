using ListaTarefas.Models;

namespace ListaTarefas.Data;

public interface ITarefaRepository
{
    IReadOnlyList<Tarefa> Listar(bool incluirConcluidas);

    /// <summary>Insere no fim da lista e devolve o id gerado.</summary>
    int Inserir(Tarefa tarefa);

    void Atualizar(Tarefa tarefa);

    void DefinirConclusao(int id, bool concluida, DateTime? dataConclusao);

    void Excluir(int id);

    /// <summary>Troca a posição da tarefa pendente com a vizinha na direção indicada.</summary>
    void Mover(int id, DirecaoMovimento direcao);
}
