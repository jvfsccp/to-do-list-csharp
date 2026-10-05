using ListaTarefas.Data;
using ListaTarefas.Exceptions;
using ListaTarefas.Models;

namespace ListaTarefas.Services;

/// <summary>Regras de negócio das tarefas. Não conhece Windows Forms nem SQL.</summary>
public sealed class TarefaService
{
    public const int TamanhoMaximoTitulo = 100;
    public const int TamanhoMaximoDescricao = 500;

    private readonly ITarefaRepository _repositorio;

    public TarefaService(ITarefaRepository repositorio)
    {
        _repositorio = repositorio;
    }

    public IReadOnlyList<Tarefa> Listar(bool incluirConcluidas) => _repositorio.Listar(incluirConcluidas);

    /// <summary>Insere (Id == 0) ou atualiza a tarefa depois de validar as regras de negócio.</summary>
    public void Salvar(Tarefa tarefa)
    {
        Validar(tarefa);

        tarefa.Titulo = tarefa.Titulo.Trim();
        tarefa.Descricao = string.IsNullOrWhiteSpace(tarefa.Descricao) ? null : tarefa.Descricao.Trim();

        if (tarefa.Id == 0)
        {
            tarefa.Id = _repositorio.Inserir(tarefa);
        }
        else
        {
            _repositorio.Atualizar(tarefa);
        }
    }

    /// <summary>Marca como concluída (gravando a data) ou reabre a tarefa.</summary>
    public void AlterarConclusao(int id, bool concluida)
    {
        _repositorio.DefinirConclusao(id, concluida, concluida ? DateTime.Now : null);
    }

    public void Excluir(int id) => _repositorio.Excluir(id);

    public void Mover(int id, DirecaoMovimento direcao) => _repositorio.Mover(id, direcao);

    private static void Validar(Tarefa tarefa)
    {
        if (string.IsNullOrWhiteSpace(tarefa.Titulo))
        {
            throw new RegraNegocioException("Informe o título da tarefa.");
        }

        if (tarefa.Titulo.Trim().Length > TamanhoMaximoTitulo)
        {
            throw new RegraNegocioException($"O título pode ter no máximo {TamanhoMaximoTitulo} caracteres.");
        }

        if (tarefa.Descricao is not null && tarefa.Descricao.Trim().Length > TamanhoMaximoDescricao)
        {
            throw new RegraNegocioException($"A descrição pode ter no máximo {TamanhoMaximoDescricao} caracteres.");
        }

        if (tarefa.DataInicio.HasValue && tarefa.DataFim.HasValue
            && tarefa.DataFim.Value.Date < tarefa.DataInicio.Value.Date)
        {
            throw new RegraNegocioException("A data de término não pode ser anterior à data de início.");
        }
    }
}
