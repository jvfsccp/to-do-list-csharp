using ListaTarefas.Models;
using ListaTarefas.Services;

namespace ListaTarefas.Forms;

/// <summary>Tela inicial: lista as tarefas e dispara as ações. Não contém SQL nem regras de negócio.</summary>
public partial class MainForm : Form
{
    private readonly TarefaService _service;
    private readonly Font _fonteConcluida;

    public MainForm(TarefaService service)
    {
        _service = service;

        InitializeComponent();

        _fonteConcluida = new Font(dgvTarefas.Font, FontStyle.Strikeout);
        FormClosed += (_, _) => _fonteConcluida.Dispose();
    }

    private Tarefa? TarefaSelecionada => dgvTarefas.CurrentRow?.DataBoundItem as Tarefa;

    private void MainForm_Load(object? sender, EventArgs e)
    {
        CarregarLista();
        dgvTarefas.Focus();
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        switch (keyData)
        {
            case Keys.Control | Keys.N:
                btnNova.PerformClick();
                return true;
            case Keys.F2:
            case Keys.Enter when dgvTarefas.Focused:
                btnEditar.PerformClick();
                return true;
            case Keys.Control | Keys.Enter:
                btnConcluir.PerformClick();
                return true;
            case Keys.Delete:
                btnExcluir.PerformClick();
                return true;
            case Keys.Control | Keys.Up:
                btnSubir.PerformClick();
                return true;
            case Keys.Control | Keys.Down:
                btnDescer.PerformClick();
                return true;
            default:
                return base.ProcessCmdKey(ref msg, keyData);
        }
    }

    /// <summary>Recarrega a grade mantendo a seleção na tarefa informada (ou na posição mais próxima).</summary>
    private void CarregarLista(int? selecionarId = null)
    {
        int? idSelecionado = selecionarId ?? TarefaSelecionada?.Id;
        int indiceAnterior = dgvTarefas.CurrentRow?.Index ?? 0;

        IReadOnlyList<Tarefa> tarefas = Array.Empty<Tarefa>();
        ExecutorSeguro.Executar(this, () => tarefas = _service.Listar(chkMostrarConcluidas.Checked));

        dgvTarefas.DataSource = tarefas.ToList();
        SelecionarLinha(idSelecionado, indiceAnterior);
        AtualizarResumo(tarefas);
        AtualizarBotoes();
    }

    private void SelecionarLinha(int? id, int indiceAlternativo)
    {
        if (dgvTarefas.Rows.Count == 0)
        {
            return;
        }

        DataGridViewRow? linha = dgvTarefas.Rows
            .Cast<DataGridViewRow>()
            .FirstOrDefault(l => id.HasValue && (l.DataBoundItem as Tarefa)?.Id == id.Value);

        linha ??= dgvTarefas.Rows[Math.Min(indiceAlternativo, dgvTarefas.Rows.Count - 1)];

        dgvTarefas.CurrentCell = linha.Cells[colTitulo.Index];
        linha.Selected = true;
    }

    private void AtualizarResumo(IReadOnlyList<Tarefa> tarefas)
    {
        int pendentes = tarefas.Count(t => !t.Concluida);
        int atrasadas = tarefas.Count(t => t.Atrasada);
        lblResumo.Text = $"{pendentes} pendente(s) · {atrasadas} atrasada(s)";
    }

    private void AtualizarBotoes()
    {
        Tarefa? tarefa = TarefaSelecionada;
        bool temSelecao = tarefa is not null;
        bool pendente = temSelecao && !tarefa!.Concluida;

        btnEditar.Enabled = temSelecao;
        btnExcluir.Enabled = temSelecao;
        btnConcluir.Enabled = temSelecao;
        btnSubir.Enabled = pendente;
        btnDescer.Enabled = pendente;
        btnConcluir.Text = temSelecao && tarefa!.Concluida ? "&Reabrir (Ctrl+Enter)" : "Con&cluir (Ctrl+Enter)";
    }

    private void AbrirFormulario(Tarefa? tarefa)
    {
        using var formulario = new TarefaForm(_service, tarefa);
        if (formulario.ShowDialog(this) == DialogResult.OK)
        {
            CarregarLista(formulario.TarefaSalva?.Id);
        }
    }

    private void AlternarConclusao(Tarefa tarefa)
    {
        if (ExecutorSeguro.Executar(this, () => _service.AlterarConclusao(tarefa.Id, !tarefa.Concluida)))
        {
            CarregarLista(tarefa.Id);
        }
    }

    private void MoverSelecionada(DirecaoMovimento direcao)
    {
        Tarefa? tarefa = TarefaSelecionada;
        if (tarefa is null)
        {
            return;
        }

        if (ExecutorSeguro.Executar(this, () => _service.Mover(tarefa.Id, direcao)))
        {
            CarregarLista(tarefa.Id);
        }
    }

    private void btnNova_Click(object? sender, EventArgs e) => AbrirFormulario(null);

    private void btnEditar_Click(object? sender, EventArgs e)
    {
        if (TarefaSelecionada is { } tarefa)
        {
            AbrirFormulario(tarefa);
        }
    }

    private void btnConcluir_Click(object? sender, EventArgs e)
    {
        if (TarefaSelecionada is { } tarefa)
        {
            AlternarConclusao(tarefa);
        }
    }

    private void btnExcluir_Click(object? sender, EventArgs e)
    {
        if (TarefaSelecionada is not { } tarefa)
        {
            return;
        }

        DialogResult resposta = MessageBox.Show(
            this, $"Excluir a tarefa \"{tarefa.Titulo}\"?", "Confirmar exclusão",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

        if (resposta == DialogResult.Yes
            && ExecutorSeguro.Executar(this, () => _service.Excluir(tarefa.Id)))
        {
            CarregarLista();
        }
    }

    private void btnSubir_Click(object? sender, EventArgs e) => MoverSelecionada(DirecaoMovimento.Cima);

    private void btnDescer_Click(object? sender, EventArgs e) => MoverSelecionada(DirecaoMovimento.Baixo);

    private void chkMostrarConcluidas_CheckedChanged(object? sender, EventArgs e) => CarregarLista();

    private void dgvTarefas_CellContentClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0 && e.ColumnIndex == colConcluida.Index
            && dgvTarefas.Rows[e.RowIndex].DataBoundItem is Tarefa tarefa)
        {
            AlternarConclusao(tarefa);
        }
    }

    private void dgvTarefas_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0 && e.ColumnIndex != colConcluida.Index)
        {
            btnEditar.PerformClick();
        }
    }

    private void dgvTarefas_SelectionChanged(object? sender, EventArgs e) => AtualizarBotoes();

    /// <summary>Destaca as tarefas atrasadas (vermelho) e risca as concluídas (cinza).</summary>
    private void dgvTarefas_DataBindingComplete(object? sender, DataGridViewBindingCompleteEventArgs e)
    {
        foreach (DataGridViewRow linha in dgvTarefas.Rows)
        {
            if (linha.DataBoundItem is not Tarefa tarefa)
            {
                continue;
            }

            linha.Cells[colTitulo.Index].ToolTipText = tarefa.Descricao ?? string.Empty;

            if (tarefa.Concluida)
            {
                linha.DefaultCellStyle.ForeColor = Color.Gray;
                linha.DefaultCellStyle.Font = _fonteConcluida;
            }
            else if (tarefa.Atrasada)
            {
                linha.DefaultCellStyle.BackColor = Color.MistyRose;
                linha.DefaultCellStyle.ForeColor = Color.Firebrick;
                linha.DefaultCellStyle.SelectionBackColor = Color.Firebrick;
                linha.DefaultCellStyle.SelectionForeColor = Color.White;
            }
        }
    }
}
