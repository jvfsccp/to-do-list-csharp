#nullable disable
namespace ListaTarefas.Forms;

partial class MainForm
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
        dgvTarefas = new DataGridView();
        colConcluida = new DataGridViewCheckBoxColumn();
        colTitulo = new DataGridViewTextBoxColumn();
        colDataInicio = new DataGridViewTextBoxColumn();
        colDataFim = new DataGridViewTextBoxColumn();
        colSituacao = new DataGridViewTextBoxColumn();

        menuPrincipal = new MenuStrip();
        mnuArquivo = new ToolStripMenuItem();
        mnuNovaTarefa = new ToolStripMenuItem();
        mnuSeparadorArquivo = new ToolStripSeparator();
        mnuSair = new ToolStripMenuItem();
        mnuEditar = new ToolStripMenuItem();
        mnuEditarTarefa = new ToolStripMenuItem();
        mnuConcluir = new ToolStripMenuItem();
        mnuExcluir = new ToolStripMenuItem();
        mnuSeparadorEditar = new ToolStripSeparator();
        mnuSubir = new ToolStripMenuItem();
        mnuDescer = new ToolStripMenuItem();
        mnuVisualizar = new ToolStripMenuItem();
        mnuMostrarConcluidas = new ToolStripMenuItem();
        mnuAtualizar = new ToolStripMenuItem();
        mnuAjuda = new ToolStripMenuItem();
        mnuAtalhos = new ToolStripMenuItem();
        mnuSobre = new ToolStripMenuItem();

        barraFerramentas = new ToolStrip();
        tsbNova = new ToolStripButton();
        tsbEditar = new ToolStripButton();
        tsbConcluir = new ToolStripButton();
        tsbExcluir = new ToolStripButton();
        tssAcoes = new ToolStripSeparator();
        tsbSubir = new ToolStripButton();
        tsbDescer = new ToolStripButton();
        tssOrdem = new ToolStripSeparator();
        tsbMostrarConcluidas = new ToolStripButton();

        barraStatus = new StatusStrip();
        lblResumo = new ToolStripStatusLabel();

        ((System.ComponentModel.ISupportInitialize)dgvTarefas).BeginInit();
        menuPrincipal.SuspendLayout();
        barraFerramentas.SuspendLayout();
        barraStatus.SuspendLayout();
        SuspendLayout();

        // nomes dos controles
        dgvTarefas.Name = "dgvTarefas";
        colConcluida.Name = "colConcluida";
        colTitulo.Name = "colTitulo";
        colDataInicio.Name = "colDataInicio";
        colDataFim.Name = "colDataFim";
        colSituacao.Name = "colSituacao";
        menuPrincipal.Name = "menuPrincipal";
        mnuArquivo.Name = "mnuArquivo";
        mnuNovaTarefa.Name = "mnuNovaTarefa";
        mnuSeparadorArquivo.Name = "mnuSeparadorArquivo";
        mnuSair.Name = "mnuSair";
        mnuEditar.Name = "mnuEditar";
        mnuEditarTarefa.Name = "mnuEditarTarefa";
        mnuConcluir.Name = "mnuConcluir";
        mnuExcluir.Name = "mnuExcluir";
        mnuSeparadorEditar.Name = "mnuSeparadorEditar";
        mnuSubir.Name = "mnuSubir";
        mnuDescer.Name = "mnuDescer";
        mnuVisualizar.Name = "mnuVisualizar";
        mnuMostrarConcluidas.Name = "mnuMostrarConcluidas";
        mnuAtualizar.Name = "mnuAtualizar";
        mnuAjuda.Name = "mnuAjuda";
        mnuAtalhos.Name = "mnuAtalhos";
        mnuSobre.Name = "mnuSobre";
        barraFerramentas.Name = "barraFerramentas";
        tsbNova.Name = "tsbNova";
        tsbEditar.Name = "tsbEditar";
        tsbConcluir.Name = "tsbConcluir";
        tsbExcluir.Name = "tsbExcluir";
        tssAcoes.Name = "tssAcoes";
        tsbSubir.Name = "tsbSubir";
        tsbDescer.Name = "tsbDescer";
        tssOrdem.Name = "tssOrdem";
        tsbMostrarConcluidas.Name = "tsbMostrarConcluidas";
        barraStatus.Name = "barraStatus";
        lblResumo.Name = "lblResumo";

        // menuPrincipal
        menuPrincipal.Items.AddRange(new ToolStripItem[] { mnuArquivo, mnuEditar, mnuVisualizar, mnuAjuda });
        menuPrincipal.Dock = DockStyle.Top;
        menuPrincipal.TabIndex = 3;

        // mnuArquivo
        mnuArquivo.Text = "&Arquivo";
        mnuArquivo.DropDownItems.AddRange(new ToolStripItem[] { mnuNovaTarefa, mnuSeparadorArquivo, mnuSair });

        mnuNovaTarefa.Text = "&Nova tarefa";
        mnuNovaTarefa.ShortcutKeys = Keys.Control | Keys.N;
        mnuNovaTarefa.Click += NovaTarefa_Click;

        mnuSair.Text = "&Sair";
        mnuSair.ShortcutKeyDisplayString = "Alt+F4";
        mnuSair.Click += mnuSair_Click;

        // mnuEditar
        mnuEditar.Text = "&Editar";
        mnuEditar.DropDownItems.AddRange(new ToolStripItem[]
            { mnuEditarTarefa, mnuConcluir, mnuExcluir, mnuSeparadorEditar, mnuSubir, mnuDescer });

        mnuEditarTarefa.Text = "&Editar tarefa";
        mnuEditarTarefa.ShortcutKeys = Keys.F2;
        mnuEditarTarefa.Click += EditarTarefa_Click;

        mnuConcluir.Text = "C&oncluir tarefa";
        mnuConcluir.ShortcutKeys = Keys.Control | Keys.Enter;
        mnuConcluir.Click += AlternarConclusao_Click;

        mnuExcluir.Text = "E&xcluir tarefa";
        mnuExcluir.ShortcutKeys = Keys.Delete;
        mnuExcluir.Click += ExcluirTarefa_Click;

        mnuSubir.Text = "Mover para &cima";
        mnuSubir.ShortcutKeys = Keys.Control | Keys.Up;
        mnuSubir.Click += SubirTarefa_Click;

        mnuDescer.Text = "Mover para &baixo";
        mnuDescer.ShortcutKeys = Keys.Control | Keys.Down;
        mnuDescer.Click += DescerTarefa_Click;

        // mnuVisualizar
        mnuVisualizar.Text = "&Visualizar";
        mnuVisualizar.DropDownItems.AddRange(new ToolStripItem[] { mnuMostrarConcluidas, mnuAtualizar });

        mnuMostrarConcluidas.Text = "&Mostrar tarefas concluídas";
        mnuMostrarConcluidas.CheckOnClick = true;
        mnuMostrarConcluidas.ShortcutKeys = Keys.Control | Keys.H;
        mnuMostrarConcluidas.Click += MostrarConcluidas_Click;

        mnuAtualizar.Text = "&Atualizar";
        mnuAtualizar.ShortcutKeys = Keys.F5;
        mnuAtualizar.Click += Atualizar_Click;

        // mnuAjuda
        mnuAjuda.Text = "Aj&uda";
        mnuAjuda.DropDownItems.AddRange(new ToolStripItem[] { mnuAtalhos, mnuSobre });

        mnuAtalhos.Text = "&Atalhos de teclado";
        mnuAtalhos.Click += mnuAtalhos_Click;

        mnuSobre.Text = "&Sobre";
        mnuSobre.Click += mnuSobre_Click;

        // barraFerramentas
        barraFerramentas.GripStyle = ToolStripGripStyle.Hidden;
        barraFerramentas.Dock = DockStyle.Top;
        barraFerramentas.Padding = new Padding(6, 2, 6, 2);
        barraFerramentas.Items.AddRange(new ToolStripItem[]
            { tsbNova, tsbEditar, tsbConcluir, tsbExcluir, tssAcoes, tsbSubir, tsbDescer, tssOrdem, tsbMostrarConcluidas });
        barraFerramentas.TabIndex = 2;

        tsbNova.Text = "Nova";
        tsbNova.ToolTipText = "Nova tarefa (Ctrl+N)";
        tsbNova.DisplayStyle = ToolStripItemDisplayStyle.Text;
        tsbNova.Click += NovaTarefa_Click;

        tsbEditar.Text = "Editar";
        tsbEditar.ToolTipText = "Editar tarefa (F2)";
        tsbEditar.DisplayStyle = ToolStripItemDisplayStyle.Text;
        tsbEditar.Click += EditarTarefa_Click;

        tsbConcluir.Text = "Concluir";
        tsbConcluir.ToolTipText = "Concluir ou reabrir (Ctrl+Enter)";
        tsbConcluir.DisplayStyle = ToolStripItemDisplayStyle.Text;
        tsbConcluir.Click += AlternarConclusao_Click;

        tsbExcluir.Text = "Excluir";
        tsbExcluir.ToolTipText = "Excluir tarefa (Del)";
        tsbExcluir.DisplayStyle = ToolStripItemDisplayStyle.Text;
        tsbExcluir.Click += ExcluirTarefa_Click;

        tsbSubir.Text = "Subir";
        tsbSubir.ToolTipText = "Mover para cima (Ctrl+↑)";
        tsbSubir.DisplayStyle = ToolStripItemDisplayStyle.Text;
        tsbSubir.Click += SubirTarefa_Click;

        tsbDescer.Text = "Descer";
        tsbDescer.ToolTipText = "Mover para baixo (Ctrl+↓)";
        tsbDescer.DisplayStyle = ToolStripItemDisplayStyle.Text;
        tsbDescer.Click += DescerTarefa_Click;

        tsbMostrarConcluidas.Text = "Mostrar concluídas";
        tsbMostrarConcluidas.ToolTipText = "Mostrar ou ocultar tarefas concluídas (Ctrl+H)";
        tsbMostrarConcluidas.CheckOnClick = true;
        tsbMostrarConcluidas.DisplayStyle = ToolStripItemDisplayStyle.Text;
        tsbMostrarConcluidas.Click += MostrarConcluidas_Click;

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

        // barraStatus
        barraStatus.Items.Add(lblResumo);
        barraStatus.TabStop = false;

        // MainForm
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        ClientSize = new Size(900, 520);
        Controls.Add(dgvTarefas);
        Controls.Add(barraStatus);
        Controls.Add(barraFerramentas);
        Controls.Add(menuPrincipal);
        MainMenuStrip = menuPrincipal;
        MinimumSize = new Size(800, 400);
        StartPosition = FormStartPosition.CenterScreen;
        Name = "MainForm";
        Text = "Lista de Tarefas";
        Load += MainForm_Load;

        ((System.ComponentModel.ISupportInitialize)dgvTarefas).EndInit();
        menuPrincipal.ResumeLayout(false);
        menuPrincipal.PerformLayout();
        barraFerramentas.ResumeLayout(false);
        barraFerramentas.PerformLayout();
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

    private MenuStrip menuPrincipal;
    private ToolStripMenuItem mnuArquivo;
    private ToolStripMenuItem mnuNovaTarefa;
    private ToolStripSeparator mnuSeparadorArquivo;
    private ToolStripMenuItem mnuSair;
    private ToolStripMenuItem mnuEditar;
    private ToolStripMenuItem mnuEditarTarefa;
    private ToolStripMenuItem mnuConcluir;
    private ToolStripMenuItem mnuExcluir;
    private ToolStripSeparator mnuSeparadorEditar;
    private ToolStripMenuItem mnuSubir;
    private ToolStripMenuItem mnuDescer;
    private ToolStripMenuItem mnuVisualizar;
    private ToolStripMenuItem mnuMostrarConcluidas;
    private ToolStripMenuItem mnuAtualizar;
    private ToolStripMenuItem mnuAjuda;
    private ToolStripMenuItem mnuAtalhos;
    private ToolStripMenuItem mnuSobre;

    private ToolStrip barraFerramentas;
    private ToolStripButton tsbNova;
    private ToolStripButton tsbEditar;
    private ToolStripButton tsbConcluir;
    private ToolStripButton tsbExcluir;
    private ToolStripSeparator tssAcoes;
    private ToolStripButton tsbSubir;
    private ToolStripButton tsbDescer;
    private ToolStripSeparator tssOrdem;
    private ToolStripButton tsbMostrarConcluidas;

    private StatusStrip barraStatus;
    private ToolStripStatusLabel lblResumo;
}
