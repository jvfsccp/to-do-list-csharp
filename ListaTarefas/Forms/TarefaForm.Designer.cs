#nullable disable
namespace ListaTarefas.Forms;

partial class TarefaForm
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null)
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        errorProvider = new ErrorProvider(components);
        tabela = new TableLayoutPanel();
        lblTitulo = new Label();
        txtTitulo = new TextBox();
        lblDescricao = new Label();
        txtDescricao = new TextBox();
        lblDataInicio = new Label();
        dtpDataInicio = new DateTimePicker();
        lblDataFim = new Label();
        dtpDataFim = new DateTimePicker();
        painelBotoes = new FlowLayoutPanel();
        btnSalvar = new Button();
        btnCancelar = new Button();
        ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
        tabela.SuspendLayout();
        painelBotoes.SuspendLayout();
        SuspendLayout();

        // nomes dos controles
        tabela.Name = "tabela";
        lblTitulo.Name = "lblTitulo";
        txtTitulo.Name = "txtTitulo";
        lblDescricao.Name = "lblDescricao";
        txtDescricao.Name = "txtDescricao";
        lblDataInicio.Name = "lblDataInicio";
        dtpDataInicio.Name = "dtpDataInicio";
        lblDataFim.Name = "lblDataFim";
        dtpDataFim.Name = "dtpDataFim";
        painelBotoes.Name = "painelBotoes";
        btnSalvar.Name = "btnSalvar";
        btnCancelar.Name = "btnCancelar";

        // errorProvider
        errorProvider.ContainerControl = this;
        errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;

        // tabela
        tabela.ColumnCount = 2;
        tabela.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        tabela.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tabela.RowCount = 5;
        tabela.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        tabela.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tabela.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        tabela.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        tabela.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        tabela.Dock = DockStyle.Fill;
        tabela.Padding = new Padding(12);
        tabela.Controls.Add(lblTitulo, 0, 0);
        tabela.Controls.Add(txtTitulo, 1, 0);
        tabela.Controls.Add(lblDescricao, 0, 1);
        tabela.Controls.Add(txtDescricao, 1, 1);
        tabela.Controls.Add(lblDataInicio, 0, 2);
        tabela.Controls.Add(dtpDataInicio, 1, 2);
        tabela.Controls.Add(lblDataFim, 0, 3);
        tabela.Controls.Add(dtpDataFim, 1, 3);
        tabela.Controls.Add(painelBotoes, 0, 4);
        tabela.SetColumnSpan(painelBotoes, 2);
        tabela.TabIndex = 0;

        // lblTitulo
        lblTitulo.Text = "&Título:";
        lblTitulo.AutoSize = true;
        lblTitulo.Anchor = AnchorStyles.Left;
        lblTitulo.TabIndex = 0;

        // txtTitulo
        txtTitulo.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        txtTitulo.TabIndex = 1;
        txtTitulo.Validating += txtTitulo_Validating;

        // lblDescricao
        lblDescricao.Text = "&Descrição:";
        lblDescricao.AutoSize = true;
        lblDescricao.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        lblDescricao.Margin = new Padding(3, 6, 3, 0);
        lblDescricao.TabIndex = 2;

        // txtDescricao
        txtDescricao.Dock = DockStyle.Fill;
        txtDescricao.Multiline = true;
        txtDescricao.AcceptsReturn = true;
        txtDescricao.ScrollBars = ScrollBars.Vertical;
        txtDescricao.TabIndex = 3;

        // lblDataInicio
        lblDataInicio.Text = "Data de &início:";
        lblDataInicio.AutoSize = true;
        lblDataInicio.Anchor = AnchorStyles.Left;
        lblDataInicio.TabIndex = 4;

        // dtpDataInicio (a caixa de seleção à esquerda torna a data opcional)
        dtpDataInicio.Format = DateTimePickerFormat.Custom;
        dtpDataInicio.CustomFormat = "dd/MM/yyyy";
        dtpDataInicio.ShowCheckBox = true;
        dtpDataInicio.Checked = false;
        dtpDataInicio.Anchor = AnchorStyles.Left;
        dtpDataInicio.Width = 140;
        dtpDataInicio.TabIndex = 5;

        // lblDataFim
        lblDataFim.Text = "Data de &término:";
        lblDataFim.AutoSize = true;
        lblDataFim.Anchor = AnchorStyles.Left;
        lblDataFim.TabIndex = 6;

        // dtpDataFim
        dtpDataFim.Format = DateTimePickerFormat.Custom;
        dtpDataFim.CustomFormat = "dd/MM/yyyy";
        dtpDataFim.ShowCheckBox = true;
        dtpDataFim.Checked = false;
        dtpDataFim.Anchor = AnchorStyles.Left;
        dtpDataFim.Width = 140;
        dtpDataFim.TabIndex = 7;

        // painelBotoes
        painelBotoes.AutoSize = true;
        painelBotoes.Dock = DockStyle.Fill;
        painelBotoes.FlowDirection = FlowDirection.RightToLeft;
        painelBotoes.Controls.Add(btnSalvar);
        painelBotoes.Controls.Add(btnCancelar);
        painelBotoes.TabIndex = 8;

        // btnSalvar
        btnSalvar.Text = "&Salvar";
        btnSalvar.Size = new Size(90, 30);
        btnSalvar.TabIndex = 8;
        btnSalvar.Click += btnSalvar_Click;

        // btnCancelar
        btnCancelar.Text = "&Cancelar";
        btnCancelar.Size = new Size(90, 30);
        btnCancelar.CausesValidation = false;
        btnCancelar.DialogResult = DialogResult.Cancel;
        btnCancelar.TabIndex = 9;

        // TarefaForm
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        AutoValidate = AutoValidate.EnableAllowFocusChange;
        AcceptButton = btnSalvar;
        CancelButton = btnCancelar;
        ClientSize = new Size(480, 340);
        Controls.Add(tabela);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Name = "TarefaForm";
        Text = "Tarefa";

        ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
        tabela.ResumeLayout(false);
        tabela.PerformLayout();
        painelBotoes.ResumeLayout(false);
        ResumeLayout(false);
    }

    private ErrorProvider errorProvider;
    private TableLayoutPanel tabela;
    private Label lblTitulo;
    private TextBox txtTitulo;
    private Label lblDescricao;
    private TextBox txtDescricao;
    private Label lblDataInicio;
    private DateTimePicker dtpDataInicio;
    private Label lblDataFim;
    private DateTimePicker dtpDataFim;
    private FlowLayoutPanel painelBotoes;
    private Button btnSalvar;
    private Button btnCancelar;
}
