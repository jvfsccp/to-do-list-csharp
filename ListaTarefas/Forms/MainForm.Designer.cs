namespace ListaTarefas.Forms;

partial class MainForm
{
    private System.ComponentModel.IContainer? components = null;

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
        dgvTarefas = new DataGridView();
        colConcluida = new DataGridViewCheckBoxColumn();
        colTitulo = new DataGridViewTextBoxColumn();
        colDataInicio = new DataGridViewTextBoxColumn();
        colDataFim = new DataGridViewTextBoxColumn();
        colSituacao = new DataGridViewTextBoxColumn();
        painelBotoes = new FlowLayoutPanel();
        btnNova = new Button();
        btnEditar = new Button();
        btnConcluir = new Button();
        btnExcluir = new Button();
        btnSubir = new Button();
        btnDescer = new Button();
        painelTopo = new Panel();
        chkMostrarConcluidas = new CheckBox();
        barraStatus = new StatusStrip();
        lblResumo = new ToolStripStatusLabel();
        ((System.ComponentModel.ISupportInitialize)dgvTarefas).BeginInit();
        painelBotoes.SuspendLayout();
        painelTopo.SuspendLayout();
        barraStatus.SuspendLayout();
        SuspendLayout();

        // dgvTarefas
        dgvTarefas.AllowUserToAddRows = false;
        dgvTarefas.AllowUserToDeleteRows = false;
        dgvTarefas.AllowUserToResizeRows = false;
        dgvTarefas.AutoGenerateColumns = false;
        dgvTarefas.BackgroundColor = SystemColors.Window;
        dgvTarefas.BorderStyle = BorderStyle.None;
        dgvTarefas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dgvTarefas.Columns.AddRange(new DataGridViewColumn[] { colConcluida, colTitulo, colDataInicio, colDataFim, colSituacao });
        dgvTarefas.Dock = DockStyle.Fill;
        dgvTarefas.MultiSelect = false;
        dgvTarefas.ReadOnly = true;
        dgvTarefas.RowHeadersVisible = false;
        dgvTarefas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvTarefas.TabIndex = 0;
        dgvTarefas.CellContentClick += dgvTarefas_CellContentClick;
        dgvTarefas.CellDoubleClick += dgvTarefas_CellDoubleClick;
        dgvTarefas.DataBindingComplete += dgvTarefas_DataBindingComplete;
        dgvTarefas.SelectionChanged += dgvTarefas_SelectionChanged;

        // colConcluida
        colConcluida.DataPropertyName = "Concluida";
        colConcluida.HeaderText = "Feito";
        colConcluida.SortMode = DataGridViewColumnSortMode.NotSortable;
        colConcluida.Width = 55;

        // colTitulo
        colTitulo.DataPropertyName = "Titulo";
        colTitulo.HeaderText = "Título";
        colTitulo.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        colTitulo.SortMode = DataGridViewColumnSortMode.NotSortable;

        // colDataInicio
        colDataInicio.DataPropertyName = "DataInicio";
        colDataInicio.HeaderText = "Início";
        colDataInicio.DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy", NullValue = "", Alignment = DataGridViewContentAlignment.MiddleCenter };
        colDataInicio.SortMode = DataGridViewColumnSortMode.NotSortable;
        colDataInicio.Width = 100;

        // colDataFim
        colDataFim.DataPropertyName = "DataFim";
        colDataFim.HeaderText = "Término";
        colDataFim.DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy", NullValue = "", Alignment = DataGridViewContentAlignment.MiddleCenter };
        colDataFim.SortMode = DataGridViewColumnSortMode.NotSortable;
        colDataFim.Width = 100;

        // colSituacao
        colSituacao.DataPropertyName = "Situacao";
        colSituacao.HeaderText = "Situação";
        colSituacao.DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter };
        colSituacao.SortMode = DataGridViewColumnSortMode.NotSortable;
        colSituacao.Width = 110;

        // painelBotoes
        painelBotoes.AutoSize = true;
        painelBotoes.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        painelBotoes.Dock = DockStyle.Bottom;
        painelBotoes.Padding = new Padding(8, 6, 8, 6);
        painelBotoes.Controls.Add(btnNova);
        painelBotoes.Controls.Add(btnEditar);
        painelBotoes.Controls.Add(btnConcluir);
        painelBotoes.Controls.Add(btnExcluir);
        painelBotoes.Controls.Add(btnSubir);
        painelBotoes.Controls.Add(btnDescer);
        painelBotoes.TabIndex = 2;

        // btnNova
        btnNova.Text = "&Nova (Ctrl+N)";
        btnNova.Size = new Size(125, 32);
        btnNova.TabIndex = 2;
        btnNova.Click += btnNova_Click;

        // btnEditar
        btnEditar.Text = "&Editar (F2)";
        btnEditar.Size = new Size(110, 32);
        btnEditar.TabIndex = 3;
        btnEditar.Click += btnEditar_Click;

        // btnConcluir
        btnConcluir.Text = "Con&cluir (Ctrl+Enter)";
        btnConcluir.Size = new Size(160, 32);
        btnConcluir.TabIndex = 4;
        btnConcluir.Click += btnConcluir_Click;

        // btnExcluir
        btnExcluir.Text = "E&xcluir (Del)";
        btnExcluir.Size = new Size(110, 32);
        btnExcluir.TabIndex = 5;
        btnExcluir.Click += btnExcluir_Click;

        // btnSubir
        btnSubir.Text = "&Subir (Ctrl+↑)";
        btnSubir.Size = new Size(125, 32);
        btnSubir.TabIndex = 6;
        btnSubir.Click += btnSubir_Click;

        // btnDescer
        btnDescer.Text = "&Descer (Ctrl+↓)";
        btnDescer.Size = new Size(130, 32);
        btnDescer.TabIndex = 7;
        btnDescer.Click += btnDescer_Click;

        // painelTopo
        painelTopo.Dock = DockStyle.Top;
        painelTopo.Height = 36;
        painelTopo.Controls.Add(chkMostrarConcluidas);
        painelTopo.TabIndex = 1;

        // chkMostrarConcluidas
        chkMostrarConcluidas.AutoSize = true;
        chkMostrarConcluidas.Location = new Point(12, 8);
        chkMostrarConcluidas.Text = "Mostrar c&oncluídas";
        chkMostrarConcluidas.TabIndex = 1;
        chkMostrarConcluidas.CheckedChanged += chkMostrarConcluidas_CheckedChanged;

        // barraStatus
        barraStatus.Items.Add(lblResumo);
        barraStatus.TabStop = false;

        // MainForm
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        ClientSize = new Size(900, 520);
        Controls.Add(dgvTarefas);
        Controls.Add(painelBotoes);
        Controls.Add(painelTopo);
        Controls.Add(barraStatus);
        MinimumSize = new Size(900, 400);
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Lista de Tarefas";
        Load += MainForm_Load;

        ((System.ComponentModel.ISupportInitialize)dgvTarefas).EndInit();
        painelBotoes.ResumeLayout(false);
        painelTopo.ResumeLayout(false);
        painelTopo.PerformLayout();
        barraStatus.ResumeLayout(false);
        barraStatus.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    private DataGridView dgvTarefas;
    private DataGridViewCheckBoxColumn colConcluida;
    private DataGridViewTextBoxColumn colTitulo;
    private DataGridViewTextBoxColumn colDataInicio;
    private DataGridViewTextBoxColumn colDataFim;
    private DataGridViewTextBoxColumn colSituacao;
    private FlowLayoutPanel painelBotoes;
    private Button btnNova;
    private Button btnEditar;
    private Button btnConcluir;
    private Button btnExcluir;
    private Button btnSubir;
    private Button btnDescer;
    private Panel painelTopo;
    private CheckBox chkMostrarConcluidas;
    private StatusStrip barraStatus;
    private ToolStripStatusLabel lblResumo;
}
