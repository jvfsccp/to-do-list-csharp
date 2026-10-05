using System.ComponentModel;
using System.Globalization;
using ListaTarefas.Models;
using ListaTarefas.Services;

namespace ListaTarefas.Forms;

/// <summary>Cadastro/edição de tarefa. Valida o formato dos campos; as regras de negócio ficam no serviço.</summary>
public partial class TarefaForm : Form
{
    private const string FormatoData = "dd/MM/yyyy";

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
        mskDataInicio.Text = tarefa.DataInicio?.ToString(FormatoData, CultureInfo.InvariantCulture);
        mskDataFim.Text = tarefa.DataFim?.ToString(FormatoData, CultureInfo.InvariantCulture);
    }

    private void btnSalvar_Click(object? sender, EventArgs e)
    {
        // Operador & (sem curto-circuito) para sinalizar todos os campos inválidos de uma vez.
        bool camposValidos = ValidarTitulo()
            & ValidarData(mskDataInicio, out DateTime? dataInicio)
            & ValidarData(mskDataFim, out DateTime? dataFim);

        if (!camposValidos)
        {
            FocarPrimeiroCampoInvalido();
            return;
        }

        var tarefa = new Tarefa
        {
            Id = _tarefaOriginal?.Id ?? 0,
            Titulo = txtTitulo.Text,
            Descricao = txtDescricao.Text,
            DataInicio = dataInicio,
            DataFim = dataFim
        };

        if (!ExecutorSeguro.Executar(this, () => _service.Salvar(tarefa)))
        {
            return;
        }

        TarefaSalva = tarefa;
        DialogResult = DialogResult.OK;
    }

    private void txtTitulo_Validating(object? sender, CancelEventArgs e) => e.Cancel = !ValidarTitulo();

    private void mskDataInicio_Validating(object? sender, CancelEventArgs e) =>
        e.Cancel = !ValidarData(mskDataInicio, out _);

    private void mskDataFim_Validating(object? sender, CancelEventArgs e) =>
        e.Cancel = !ValidarData(mskDataFim, out _);

    private bool ValidarTitulo()
    {
        bool valido = !string.IsNullOrWhiteSpace(txtTitulo.Text);
        errorProvider.SetError(txtTitulo, valido ? string.Empty : "Informe o título da tarefa.");
        return valido;
    }

    /// <summary>Campo vazio é válido (data opcional); preenchido precisa ser uma data real dd/MM/aaaa.</summary>
    private bool ValidarData(MaskedTextBox campo, out DateTime? data)
    {
        data = null;
        errorProvider.SetError(campo, string.Empty);

        if (campo.MaskedTextProvider?.AssignedEditPositionCount == 0)
        {
            return true;
        }

        if (!DateTime.TryParseExact(campo.Text, FormatoData, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out DateTime valor))
        {
            errorProvider.SetError(campo, "Data inválida. Use o formato dd/mm/aaaa.");
            return false;
        }

        data = valor;
        return true;
    }

    private void FocarPrimeiroCampoInvalido()
    {
        foreach (Control campo in new Control[] { txtTitulo, mskDataInicio, mskDataFim })
        {
            if (!string.IsNullOrEmpty(errorProvider.GetError(campo)))
            {
                campo.Focus();
                return;
            }
        }
    }
}
