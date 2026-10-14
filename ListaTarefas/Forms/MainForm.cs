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

    private bool MostrarConcluidas => mnuMostrarConcluidas.Checked;

    private void MainForm_Load(object? sender, EventArgs e)
    {
        CarregarLista();
        dgvTarefas.Focus();
    }

    /// <summary>Enter na grade edita a tarefa; os demais atalhos são dos itens de menu.</summary>
    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        if (keyData == Keys.Enter && dgvTarefas.Focused)
        {
            EditarSelecionada();
            return true;
        }

        return base.ProcessCmdKey(ref msg, keyData);
    }

    /// <summary>Recarrega a grade mantendo a seleção na tarefa informada (ou na posição mais próxima).</summary>
    private void CarregarLista(int? selecionarId = null)
    {
        int? idSelecionado = selecionarId ?? TarefaSelecionada?.Id;
        int indiceAnterior = dgvTarefas.CurrentRow?.Index ?? 0;

        IReadOnlyList<Tarefa> tarefas = Array.Empty<Tarefa>();
        ExecutorSeguro.Executar(this, () => tarefas = _service.Listar(MostrarConcluidas));

        dgvTarefas.DataSource = tarefas.ToList();
        SelecionarLinha(idSelecionado, indiceAnterior);
        AtualizarResumo(tarefas);
        AtualizarAcoes();
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

    /// <summary>Habilita/desabilita as ações (menu e barra de ferramentas) conforme a seleção.</summary>
    private void AtualizarAcoes()
    {
        Tarefa? tarefa = TarefaSelecionada;
        bool temSelecao = tarefa is not null;
        bool pendente = temSelecao && !tarefa!.Concluida;

        mnuEditarTarefa.Enabled = tsbEditar.Enabled = temSelecao;
        mnuExcluir.Enabled = tsbExcluir.Enabled = temSelecao;
        mnuConcluir.Enabled = tsbConcluir.Enabled = temSelecao;
        mnuSubir.Enabled = tsbSubir.Enabled = pendente;
        mnuDescer.Enabled = tsbDescer.Enabled = pendente;

        bool reabrir = temSelecao && tarefa!.Concluida;
        mnuConcluir.Text = reabrir ? "&Reabrir tarefa" : "C&oncluir tarefa";
        tsbConcluir.Text = reabrir ? "Reabrir" : "Concluir";
    }

    private void AbrirFormulario(Tarefa? tarefa)
    {
        using var formulario = new TarefaForm(_service, tarefa);
        if (formulario.ShowDialog(this) == DialogResult.OK)
        {
            CarregarLista(formulario.TarefaSalva?.Id);
        }
    }

    private void EditarSelecionada()
    {
        if (TarefaSelecionada is { } tarefa)
        {
            AbrirFormulario(tarefa);
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

    private void NovaTarefa_Click(object? sender, EventArgs e) => AbrirFormulario(null);

    private void EditarTarefa_Click(object? sender, EventArgs e) => EditarSelecionada();

    private void AlternarConclusao_Click(object? sender, EventArgs e)
    {
        if (TarefaSelecionada is { } tarefa)
        {
            AlternarConclusao(tarefa);
        }
    }

    private void ExcluirTarefa_Click(object? sender, EventArgs e)
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

    private void SubirTarefa_Click(object? sender, EventArgs e) => MoverSelecionada(DirecaoMovimento.Cima);

    private void DescerTarefa_Click(object? sender, EventArgs e) => MoverSelecionada(DirecaoMovimento.Baixo);

    /// <summary>Menu e barra de ferramentas têm um botão "mostrar concluídas" cada; mantém os dois em sincronia.</summary>
    private void MostrarConcluidas_Click(object? sender, EventArgs e)
    {
        bool marcado = sender is ToolStripButton botao ? botao.Checked : mnuMostrarConcluidas.Checked;
        mnuMostrarConcluidas.Checked = marcado;
        tsbMostrarConcluidas.Checked = marcado;
        CarregarLista();
    }

    private void Atualizar_Click(object? sender, EventArgs e) => CarregarLista();

    private void mnuSair_Click(object? sender, EventArgs e) => Close();

    private void mnuAtalhos_Click(object? sender, EventArgs e)
    {
        MessageBox.Show(
            this,
            "Ctrl+N\tNova tarefa\n" +
            "F2 / Enter\tEditar tarefa\n" +
            "Ctrl+Enter\tConcluir ou reabrir\n" +
            "Del\tExcluir tarefa\n" +
            "Ctrl+↑ / Ctrl+↓\tMover para cima / para baixo\n" +
            "Ctrl+H\tMostrar ou ocultar concluídas\n" +
            "F5\tAtualizar a lista",
            "Atalhos de teclado", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void mnuSobre_Click(object? sender, EventArgs e)
    {
        MessageBox.Show(
            this,
            "Lista de Tarefas\nAplicação desktop em C# (Windows Forms) com banco de dados SQLite.",
            "Sobre", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

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
            EditarSelecionada();
        }
    }

    private void dgvTarefas_SelectionChanged(object? sender, EventArgs e) => AtualizarAcoes();

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
