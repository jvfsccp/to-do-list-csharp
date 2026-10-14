using System.ComponentModel;
using ListaTarefas.Models;
using ListaTarefas.Services;

namespace ListaTarefas.Forms;

/// <summary>Cadastro/edição de tarefa. Valida o formato dos campos; as regras de negócio ficam no serviço.</summary>
public partial class TarefaForm : Form
{
    private readonly TarefaService _service;
    private readonly Tarefa? _tarefaOriginal;

    /// <summary>Tarefa gravada, disponível quando o formulário fecha com DialogResult.OK.</summary>
    public Tarefa? TarefaSalva { get; private set; }

    public TarefaForm(TarefaService service, Tarefa? tarefa = null)
    {
        _service = service;
        _tarefaOriginal = tarefa;

        InitializeComponent();

        txtTitulo.MaxLength = TarefaService.TamanhoMaximoTitulo;
        txtDescricao.MaxLength = TarefaService.TamanhoMaximoDescricao;
        Text = tarefa is null ? "Nova tarefa" : "Editar tarefa";
        ActiveControl = txtTitulo;

        if (tarefa is not null)
        {
            PreencherCampos(tarefa);
        }
    }

    private void PreencherCampos(Tarefa tarefa)
    {
        txtTitulo.Text = tarefa.Titulo;
        txtDescricao.Text = tarefa.Descricao;
        DefinirData(dtpDataInicio, tarefa.DataInicio);
        DefinirData(dtpDataFim, tarefa.DataFim);
    }

    /// <summary>Data opcional: a caixa de seleção do picker desmarcada significa "sem data".</summary>
    private static void DefinirData(DateTimePicker campo, DateTime? data)
    {
        if (data.HasValue)
        {
            campo.Value = data.Value;
            campo.Checked = true;
        }
        else
        {
            campo.Checked = false;
        }
    }

    private static DateTime? LerData(DateTimePicker campo) => campo.Checked ? campo.Value.Date : null;

    private void btnSalvar_Click(object? sender, EventArgs e)
    {
        if (!ValidarTitulo())
        {
            txtTitulo.Focus();
            return;
        }

        var tarefa = new Tarefa
        {
            Id = _tarefaOriginal?.Id ?? 0,
            Titulo = txtTitulo.Text,
            Descricao = txtDescricao.Text,
            DataInicio = LerData(dtpDataInicio),
            DataFim = LerData(dtpDataFim)
        };

        if (!ExecutorSeguro.Executar(this, () => _service.Salvar(tarefa)))
        {
            return;
        }

        TarefaSalva = tarefa;
        DialogResult = DialogResult.OK;
    }

    private void txtTitulo_Validating(object? sender, CancelEventArgs e) => e.Cancel = !ValidarTitulo();

    private bool ValidarTitulo()
    {
        bool valido = !string.IsNullOrWhiteSpace(txtTitulo.Text);
        errorProvider.SetError(txtTitulo, valido ? string.Empty : "Informe o título da tarefa.");
        return valido;
    }
}
