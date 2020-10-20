using Microsoft.Msagl.Drawing;
using System.Windows.Forms;

namespace antico
{
    partial class CreateNewModelForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.DoubleBuffered = true;

            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreateNewModelForm));
            this.goBackSign = new System.Windows.Forms.PictureBox();
            this.anticoLabel = new System.Windows.Forms.Label();
            this.parametersSettingsSign = new System.Windows.Forms.PictureBox();
            this.startSign = new System.Windows.Forms.PictureBox();
            this.saveSign = new System.Windows.Forms.PictureBox();
            this.visualizeSign = new System.Windows.Forms.PictureBox();
            this.waitingAnimation = new System.Windows.Forms.PictureBox();
            this.databaseRadioLayout = new System.Windows.Forms.FlowLayoutPanel();
            this.databaseLabel = new System.Windows.Forms.Label();
            this.databaseComboBox = new System.Windows.Forms.ComboBox();
            this.mainLayout = new System.Windows.Forms.FlowLayoutPanel();
            this.nonTerminalSelectionLayout = new System.Windows.Forms.FlowLayoutPanel();
            this.nonTerminalsLabel = new System.Windows.Forms.Label();
            this.plusSelected = new System.Windows.Forms.CheckBox();
            this.minusSelected = new System.Windows.Forms.CheckBox();
            this.timesSelected = new System.Windows.Forms.CheckBox();
            this.divisionSelected = new System.Windows.Forms.CheckBox();
            this.sinSelected = new System.Windows.Forms.CheckBox();
            this.cosSelected = new System.Windows.Forms.CheckBox();
            this.expSelected = new System.Windows.Forms.CheckBox();
            this.rlogSelected = new System.Windows.Forms.CheckBox();
            this.colonySizeLayout = new System.Windows.Forms.FlowLayoutPanel();
            this.colonySize = new System.Windows.Forms.Label();
            this.colonySizeUpDown = new System.Windows.Forms.NumericUpDown();
            this.maximalNumberOfIterationsLayout = new System.Windows.Forms.FlowLayoutPanel();
            this.maximalNumberOfIterationsLabel = new System.Windows.Forms.Label();
            this.maxNoOfIterUpDown = new System.Windows.Forms.NumericUpDown();
            this.maxNoOfNotImprovingIterationsLayout = new System.Windows.Forms.FlowLayoutPanel();
            this.maxNoOfNotImprovingIterationsLabel = new System.Windows.Forms.Label();
            this.maxNoOfNotImprovingIterUpDown = new System.Windows.Forms.NumericUpDown();
            this.limitLayout = new System.Windows.Forms.FlowLayoutPanel();
            this.limitLabel = new System.Windows.Forms.Label();
            this.limitUpDown = new System.Windows.Forms.NumericUpDown();
            this.initialMaxDepthLayout = new System.Windows.Forms.FlowLayoutPanel();
            this.initialMaxDepthLabel = new System.Windows.Forms.Label();
            this.initialMaxDepthUpDown = new System.Windows.Forms.NumericUpDown();
            this.maxDepthLayout = new System.Windows.Forms.FlowLayoutPanel();
            this.maxDepthLabel = new System.Windows.Forms.Label();
            this.maxDepthUpDown = new System.Windows.Forms.NumericUpDown();
            this.alphaLayout = new System.Windows.Forms.FlowLayoutPanel();
            this.alphaLabel = new System.Windows.Forms.Label();
            this.alphaUpDown = new System.Windows.Forms.NumericUpDown();
            this.probabilityLayout = new System.Windows.Forms.FlowLayoutPanel();
            this.probabilityLabel = new System.Windows.Forms.Label();
            this.probabilityUpDown = new System.Windows.Forms.NumericUpDown();
            this.generatingTreeMethodLayout = new System.Windows.Forms.FlowLayoutPanel();
            this.generatingTreeMethod = new System.Windows.Forms.Label();
            this.rampedMethodRadioButton = new System.Windows.Forms.RadioButton();
            this.growMethodRadioButton = new System.Windows.Forms.RadioButton();
            this.fullMethodRadioButton = new System.Windows.Forms.RadioButton();
            this.numberOfRunsLayout = new System.Windows.Forms.FlowLayoutPanel();
            this.numberOfRunsLabel = new System.Windows.Forms.Label();
            this.numberOfRunsUpDown = new System.Windows.Forms.NumericUpDown();
            this.lookupConsoleFormSign = new System.Windows.Forms.PictureBox();
            this.minimizeSign = new System.Windows.Forms.PictureBox();
            this.exitSign = new System.Windows.Forms.PictureBox();
            this.uploadSign = new System.Windows.Forms.PictureBox();
            this.showSolutionsProgressSign = new System.Windows.Forms.PictureBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.goBackSign)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.parametersSettingsSign)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.startSign)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.saveSign)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.visualizeSign)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.waitingAnimation)).BeginInit();
            this.databaseRadioLayout.SuspendLayout();
            this.mainLayout.SuspendLayout();
            this.nonTerminalSelectionLayout.SuspendLayout();
            this.colonySizeLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.colonySizeUpDown)).BeginInit();
            this.maximalNumberOfIterationsLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maxNoOfIterUpDown)).BeginInit();
            this.maxNoOfNotImprovingIterationsLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maxNoOfNotImprovingIterUpDown)).BeginInit();
            this.limitLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.limitUpDown)).BeginInit();
            this.initialMaxDepthLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.initialMaxDepthUpDown)).BeginInit();
            this.maxDepthLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maxDepthUpDown)).BeginInit();
            this.alphaLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.alphaUpDown)).BeginInit();
            this.probabilityLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.probabilityUpDown)).BeginInit();
            this.generatingTreeMethodLayout.SuspendLayout();
            this.numberOfRunsLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfRunsUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookupConsoleFormSign)).BeginInit();
            
            ((System.ComponentModel.ISupportInitialize)(this.minimizeSign)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.exitSign)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uploadSign)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.showSolutionsProgressSign)).BeginInit();
            this.SuspendLayout();
            // 
            // goBackSign
            // 
            this.goBackSign.BackColor = System.Drawing.Color.Transparent;
            this.goBackSign.Image = global::antico.Properties.Resources.back_white;
            this.goBackSign.Location = new System.Drawing.Point(752, 30);
            this.goBackSign.Name = "goBackSign";
            this.goBackSign.Size = new System.Drawing.Size(70, 70);
            this.goBackSign.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.goBackSign.TabIndex = 7;
            this.goBackSign.TabStop = false;
            this.goBackSign.MouseClick += new System.Windows.Forms.MouseEventHandler(this.GoBackSign_MouseClick);
            this.goBackSign.MouseEnter += new System.EventHandler(this.GoBackSign_MouseEnter);
            this.goBackSign.MouseLeave += new System.EventHandler(this.GoBackSign_MouseLeave);
            this.goBackSign.MouseHover += new System.EventHandler(this.GoBackSign_MouseHover);
            // 
            // anticoLabel
            // 
            this.anticoLabel.AutoSize = true;
            this.anticoLabel.BackColor = System.Drawing.Color.Transparent;
            this.anticoLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.anticoLabel.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.anticoLabel.Location = new System.Drawing.Point(601, 38);
            this.anticoLabel.Name = "anticoLabel";
            this.anticoLabel.Size = new System.Drawing.Size(93, 33);
            this.anticoLabel.TabIndex = 8;
            this.anticoLabel.Text = "antico";
            // 
            // parametersSettingsSign
            // 
            this.parametersSettingsSign.BackColor = System.Drawing.Color.Transparent;
            this.parametersSettingsSign.BackgroundImage = global::antico.Properties.Resources.settings_white;
            this.parametersSettingsSign.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.parametersSettingsSign.Location = new System.Drawing.Point(155, 21);
            this.parametersSettingsSign.Name = "parametersSettingsSign";
            this.parametersSettingsSign.Size = new System.Drawing.Size(50, 50);
            this.parametersSettingsSign.TabIndex = 9;
            this.parametersSettingsSign.TabStop = false;
            this.parametersSettingsSign.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ParametersSettingsSign_MouseClick);
            this.parametersSettingsSign.MouseEnter += new System.EventHandler(this.ParametersSettingsSign_MouseEnter);
            this.parametersSettingsSign.MouseLeave += new System.EventHandler(this.ParametersSettingsSign_MouseLeave);
            this.parametersSettingsSign.MouseHover += new System.EventHandler(this.ParametersSettingsSign_MouseHover);
            // 
            // startSign
            // 
            this.startSign.BackColor = System.Drawing.Color.Transparent;
            this.startSign.BackgroundImage = global::antico.Properties.Resources.start_white;
            this.startSign.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.startSign.Location = new System.Drawing.Point(30, 17);
            this.startSign.Name = "startSign";
            this.startSign.Size = new System.Drawing.Size(60, 60);
            this.startSign.TabIndex = 10;
            this.startSign.TabStop = false;
            this.startSign.MouseClick += new System.Windows.Forms.MouseEventHandler(this.StartSign_MouseClick);
            this.startSign.MouseEnter += new System.EventHandler(this.StartSign_MouseEnter);
            this.startSign.MouseLeave += new System.EventHandler(this.StartSign_MouseLeave);
            this.startSign.MouseHover += new System.EventHandler(this.StartSign_MouseHover);
            // 
            // saveSign
            // 
            this.saveSign.BackColor = System.Drawing.Color.Transparent;
            this.saveSign.BackgroundImage = global::antico.Properties.Resources.save_white;
            this.saveSign.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.saveSign.Location = new System.Drawing.Point(462, 22);
            this.saveSign.Name = "saveSign";
            this.saveSign.Size = new System.Drawing.Size(45, 45);
            this.saveSign.TabIndex = 11;
            this.saveSign.TabStop = false;
            this.saveSign.Visible = false;
            this.saveSign.MouseClick += new System.Windows.Forms.MouseEventHandler(this.SaveSign_MouseClick);
            this.saveSign.MouseEnter += new System.EventHandler(this.SaveSign_MouseEnter);
            this.saveSign.MouseLeave += new System.EventHandler(this.SaveSign_MouseLeave);
            this.saveSign.MouseHover += new System.EventHandler(this.SaveSign_MouseHover);
            // 
            // visualizeSign
            // 
            this.visualizeSign.BackColor = System.Drawing.Color.Transparent;
            this.visualizeSign.BackgroundImage = global::antico.Properties.Resources.visual_white;
            this.visualizeSign.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.visualizeSign.Location = new System.Drawing.Point(384, 18);
            this.visualizeSign.Name = "visualizeSign";
            this.visualizeSign.Size = new System.Drawing.Size(55, 55);
            this.visualizeSign.TabIndex = 12;
            this.visualizeSign.TabStop = false;
            this.visualizeSign.Visible = false;
            this.visualizeSign.MouseClick += new System.Windows.Forms.MouseEventHandler(this.VisualizeSign_MouseClick);
            this.visualizeSign.MouseEnter += new System.EventHandler(this.VisualizeSign_MouseEnter);
            this.visualizeSign.MouseLeave += new System.EventHandler(this.VisualizeSign_MouseLeave);
            this.visualizeSign.MouseHover += new System.EventHandler(this.VisualizeSign_MouseHover);
            // 
            // waitingAnimation
            // 
            this.waitingAnimation.BackColor = System.Drawing.Color.Transparent;
            this.waitingAnimation.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.waitingAnimation.Image = global::antico.Properties.Resources.wait;
            this.waitingAnimation.Location = new System.Drawing.Point(371, 160);
            this.waitingAnimation.Name = "waitingAnimation";
            this.waitingAnimation.Size = new System.Drawing.Size(270, 270);
            this.waitingAnimation.TabIndex = 14;
            this.waitingAnimation.TabStop = false;
            this.waitingAnimation.Visible = false;
            // Trigger on waitingAnimation visibilitx changed. (For end of model search.)
            this.waitingAnimation.VisibleChanged += new System.EventHandler(this.WaitingAnimation_VisibilityChanged);
            // 
            // databaseRadioLayout
            // 
            this.databaseRadioLayout.BackColor = System.Drawing.Color.Transparent;
            this.databaseRadioLayout.Controls.Add(this.databaseLabel);
            this.databaseRadioLayout.Controls.Add(this.databaseComboBox);
            this.databaseRadioLayout.Location = new System.Drawing.Point(3, 3);
            this.databaseRadioLayout.Name = "databaseRadioLayout";
            this.databaseRadioLayout.Size = new System.Drawing.Size(497, 40);
            this.databaseRadioLayout.TabIndex = 15;
            // 
            // databaseLabel
            // 
            this.databaseLabel.AutoSize = true;
            this.databaseLabel.Font = new System.Drawing.Font("Source Sans Pro", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.databaseLabel.ForeColor = System.Drawing.SystemColors.Control;
            this.databaseLabel.Location = new System.Drawing.Point(3, 0);
            this.databaseLabel.Name = "databaseLabel";
            this.databaseLabel.Size = new System.Drawing.Size(67, 17);
            this.databaseLabel.TabIndex = 17;
            this.databaseLabel.Text = "DATABASE:\r\n";
            // 
            // databaseComboBox
            // 
            this.databaseComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.databaseComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.databaseComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.databaseComboBox.Font = new System.Drawing.Font("Source Sans Pro", 9.749999F);
            this.databaseComboBox.FormattingEnabled = true;
            this.databaseComboBox.Items.AddRange(new object[] {
            "ClaMP",
            "ember"});
            this.databaseComboBox.Location = new System.Drawing.Point(76, 3);
            this.databaseComboBox.Name = "databaseComboBox";
            this.databaseComboBox.Size = new System.Drawing.Size(133, 25);
            this.databaseComboBox.TabIndex = 20;
            // 
            // mainLayout
            // 
            this.mainLayout.BackColor = System.Drawing.Color.Transparent;
            this.mainLayout.Controls.Add(this.databaseRadioLayout);
            this.mainLayout.Controls.Add(this.nonTerminalSelectionLayout);
            this.mainLayout.Controls.Add(this.colonySizeLayout);
            this.mainLayout.Controls.Add(this.maximalNumberOfIterationsLayout);
            this.mainLayout.Controls.Add(this.maxNoOfNotImprovingIterationsLayout);
            this.mainLayout.Controls.Add(this.limitLayout);
            this.mainLayout.Controls.Add(this.initialMaxDepthLayout);
            this.mainLayout.Controls.Add(this.maxDepthLayout);
            this.mainLayout.Controls.Add(this.alphaLayout);
            this.mainLayout.Controls.Add(this.probabilityLayout);
            this.mainLayout.Controls.Add(this.generatingTreeMethodLayout);
            this.mainLayout.Controls.Add(this.numberOfRunsLayout);
            this.mainLayout.Location = new System.Drawing.Point(30, 107);
            this.mainLayout.Name = "mainLayout";
            this.mainLayout.Size = new System.Drawing.Size(500, 550);
            this.mainLayout.TabIndex = 16;
            this.mainLayout.Visible = false;
            // 
            // nonTerminalSelectionLayout
            // 
            this.nonTerminalSelectionLayout.BackColor = System.Drawing.Color.Transparent;
            this.nonTerminalSelectionLayout.Controls.Add(this.nonTerminalsLabel);
            this.nonTerminalSelectionLayout.Controls.Add(this.plusSelected);
            this.nonTerminalSelectionLayout.Controls.Add(this.minusSelected);
            this.nonTerminalSelectionLayout.Controls.Add(this.timesSelected);
            this.nonTerminalSelectionLayout.Controls.Add(this.divisionSelected);
            this.nonTerminalSelectionLayout.Controls.Add(this.sinSelected);
            this.nonTerminalSelectionLayout.Controls.Add(this.cosSelected);
            this.nonTerminalSelectionLayout.Controls.Add(this.expSelected);
            this.nonTerminalSelectionLayout.Controls.Add(this.rlogSelected);
            this.nonTerminalSelectionLayout.Location = new System.Drawing.Point(3, 49);
            this.nonTerminalSelectionLayout.Name = "nonTerminalSelectionLayout";
            this.nonTerminalSelectionLayout.Size = new System.Drawing.Size(493, 40);
            this.nonTerminalSelectionLayout.TabIndex = 18;
            // 
            // nonTerminalsLabel
            // 
            this.nonTerminalsLabel.AutoSize = true;
            this.nonTerminalsLabel.Font = new System.Drawing.Font("Source Sans Pro", 9.749999F);
            this.nonTerminalsLabel.ForeColor = System.Drawing.SystemColors.Control;
            this.nonTerminalsLabel.Location = new System.Drawing.Point(3, 0);
            this.nonTerminalsLabel.Name = "nonTerminalsLabel";
            this.nonTerminalsLabel.Size = new System.Drawing.Size(100, 17);
            this.nonTerminalsLabel.TabIndex = 17;
            this.nonTerminalsLabel.Text = "NON-TERMINALS:";
            // 
            // plusSelected
            // 
            this.plusSelected.AutoSize = true;
            this.plusSelected.Checked = true;
            this.plusSelected.CheckState = System.Windows.Forms.CheckState.Checked;
            this.plusSelected.Font = new System.Drawing.Font("Source Sans Pro", 9.749999F);
            this.plusSelected.ForeColor = System.Drawing.SystemColors.Control;
            this.plusSelected.Location = new System.Drawing.Point(109, 3);
            this.plusSelected.Name = "plusSelected";
            this.plusSelected.Size = new System.Drawing.Size(33, 21);
            this.plusSelected.TabIndex = 18;
            this.plusSelected.Text = "+";
            this.plusSelected.UseVisualStyleBackColor = true;
            // 
            // minusSelected
            // 
            this.minusSelected.AutoSize = true;
            this.minusSelected.Checked = true;
            this.minusSelected.CheckState = System.Windows.Forms.CheckState.Checked;
            this.minusSelected.Font = new System.Drawing.Font("Source Sans Pro", 9.749999F);
            this.minusSelected.ForeColor = System.Drawing.SystemColors.Control;
            this.minusSelected.Location = new System.Drawing.Point(148, 3);
            this.minusSelected.Name = "minusSelected";
            this.minusSelected.Size = new System.Drawing.Size(31, 21);
            this.minusSelected.TabIndex = 19;
            this.minusSelected.Text = "-";
            this.minusSelected.UseVisualStyleBackColor = true;
            // 
            // timesSelected
            // 
            this.timesSelected.AutoSize = true;
            this.timesSelected.Checked = true;
            this.timesSelected.CheckState = System.Windows.Forms.CheckState.Checked;
            this.timesSelected.Font = new System.Drawing.Font("Source Sans Pro", 9.749999F);
            this.timesSelected.ForeColor = System.Drawing.SystemColors.Control;
            this.timesSelected.Location = new System.Drawing.Point(185, 3);
            this.timesSelected.Name = "timesSelected";
            this.timesSelected.Size = new System.Drawing.Size(32, 21);
            this.timesSelected.TabIndex = 20;
            this.timesSelected.Text = "*";
            this.timesSelected.UseVisualStyleBackColor = true;
            // 
            // divisionSelected
            // 
            this.divisionSelected.AutoSize = true;
            this.divisionSelected.Checked = true;
            this.divisionSelected.CheckState = System.Windows.Forms.CheckState.Checked;
            this.divisionSelected.Font = new System.Drawing.Font("Source Sans Pro", 9.749999F);
            this.divisionSelected.ForeColor = System.Drawing.SystemColors.Control;
            this.divisionSelected.Location = new System.Drawing.Point(223, 3);
            this.divisionSelected.Name = "divisionSelected";
            this.divisionSelected.Size = new System.Drawing.Size(32, 21);
            this.divisionSelected.TabIndex = 21;
            this.divisionSelected.Text = "/";
            this.divisionSelected.UseVisualStyleBackColor = true;
            // 
            // sinSelected
            // 
            this.sinSelected.AutoSize = true;
            this.sinSelected.Checked = true;
            this.sinSelected.CheckState = System.Windows.Forms.CheckState.Checked;
            this.sinSelected.Font = new System.Drawing.Font("Source Sans Pro", 9.749999F);
            this.sinSelected.ForeColor = System.Drawing.SystemColors.Control;
            this.sinSelected.Location = new System.Drawing.Point(261, 3);
            this.sinSelected.Name = "sinSelected";
            this.sinSelected.Size = new System.Drawing.Size(42, 21);
            this.sinSelected.TabIndex = 22;
            this.sinSelected.Text = "sin";
            this.sinSelected.UseVisualStyleBackColor = true;
            // 
            // cosSelected
            // 
            this.cosSelected.AutoSize = true;
            this.cosSelected.Checked = true;
            this.cosSelected.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cosSelected.Font = new System.Drawing.Font("Source Sans Pro", 9.749999F);
            this.cosSelected.ForeColor = System.Drawing.SystemColors.Control;
            this.cosSelected.Location = new System.Drawing.Point(309, 3);
            this.cosSelected.Name = "cosSelected";
            this.cosSelected.Size = new System.Drawing.Size(45, 21);
            this.cosSelected.TabIndex = 23;
            this.cosSelected.Text = "cos";
            this.cosSelected.UseVisualStyleBackColor = true;
            // 
            // expSelected
            // 
            this.expSelected.AutoSize = true;
            this.expSelected.Checked = true;
            this.expSelected.CheckState = System.Windows.Forms.CheckState.Checked;
            this.expSelected.Font = new System.Drawing.Font("Source Sans Pro", 9.749999F);
            this.expSelected.ForeColor = System.Drawing.SystemColors.Control;
            this.expSelected.Location = new System.Drawing.Point(360, 3);
            this.expSelected.Name = "expSelected";
            this.expSelected.Size = new System.Drawing.Size(46, 21);
            this.expSelected.TabIndex = 24;
            this.expSelected.Text = "exp";
            this.expSelected.UseVisualStyleBackColor = true;
            // 
            // rlogSelected
            // 
            this.rlogSelected.AutoSize = true;
            this.rlogSelected.Checked = true;
            this.rlogSelected.CheckState = System.Windows.Forms.CheckState.Checked;
            this.rlogSelected.Font = new System.Drawing.Font("Source Sans Pro", 9.749999F);
            this.rlogSelected.ForeColor = System.Drawing.SystemColors.Control;
            this.rlogSelected.Location = new System.Drawing.Point(412, 3);
            this.rlogSelected.Name = "rlogSelected";
            this.rlogSelected.Size = new System.Drawing.Size(49, 21);
            this.rlogSelected.TabIndex = 25;
            this.rlogSelected.Text = "rlog";
            this.rlogSelected.UseVisualStyleBackColor = true;
            // 
            // colonySizeLayout
            // 
            this.colonySizeLayout.BackColor = System.Drawing.Color.Transparent;
            this.colonySizeLayout.Controls.Add(this.colonySize);
            this.colonySizeLayout.Controls.Add(this.colonySizeUpDown);
            this.colonySizeLayout.Location = new System.Drawing.Point(3, 95);
            this.colonySizeLayout.Name = "colonySizeLayout";
            this.colonySizeLayout.Size = new System.Drawing.Size(497, 40);
            this.colonySizeLayout.TabIndex = 18;
            // 
            // colonySize
            // 
            this.colonySize.AutoSize = true;
            this.colonySize.Font = new System.Drawing.Font("Source Sans Pro", 9.749999F);
            this.colonySize.ForeColor = System.Drawing.SystemColors.Control;
            this.colonySize.Location = new System.Drawing.Point(3, 0);
            this.colonySize.Name = "colonySize";
            this.colonySize.Size = new System.Drawing.Size(83, 17);
            this.colonySize.TabIndex = 17;
            this.colonySize.Text = "COLONY SIZE:";
            // 
            // colonySizeUpDown
            // 
            this.colonySizeUpDown.Font = new System.Drawing.Font("Source Sans Pro", 9.749999F);
            this.colonySizeUpDown.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.colonySizeUpDown.Location = new System.Drawing.Point(92, 3);
            this.colonySizeUpDown.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.colonySizeUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.colonySizeUpDown.Name = "colonySizeUpDown";
            this.colonySizeUpDown.Size = new System.Drawing.Size(60, 24);
            this.colonySizeUpDown.TabIndex = 18;
            this.colonySizeUpDown.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // maximalNumberOfIterationsLayout
            // 
            this.maximalNumberOfIterationsLayout.BackColor = System.Drawing.Color.Transparent;
            this.maximalNumberOfIterationsLayout.Controls.Add(this.maximalNumberOfIterationsLabel);
            this.maximalNumberOfIterationsLayout.Controls.Add(this.maxNoOfIterUpDown);
            this.maximalNumberOfIterationsLayout.Location = new System.Drawing.Point(3, 141);
            this.maximalNumberOfIterationsLayout.Name = "maximalNumberOfIterationsLayout";
            this.maximalNumberOfIterationsLayout.Size = new System.Drawing.Size(497, 40);
            this.maximalNumberOfIterationsLayout.TabIndex = 19;
            // 
            // maximalNumberOfIterationsLabel
            // 
            this.maximalNumberOfIterationsLabel.AutoSize = true;
            this.maximalNumberOfIterationsLabel.Font = new System.Drawing.Font("Source Sans Pro", 9.749999F);
            this.maximalNumberOfIterationsLabel.ForeColor = System.Drawing.SystemColors.Control;
            this.maximalNumberOfIterationsLabel.Location = new System.Drawing.Point(3, 0);
            this.maximalNumberOfIterationsLabel.Name = "maximalNumberOfIterationsLabel";
            this.maximalNumberOfIterationsLabel.Size = new System.Drawing.Size(194, 17);
            this.maximalNumberOfIterationsLabel.TabIndex = 17;
            this.maximalNumberOfIterationsLabel.Text = "MAXIMAL NUMBER OF ITERATIONS:";
            // 
            // maxNoOfIterUpDown
            // 
            this.maxNoOfIterUpDown.Font = new System.Drawing.Font("Source Sans Pro", 9.749999F);
            this.maxNoOfIterUpDown.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.maxNoOfIterUpDown.Location = new System.Drawing.Point(203, 3);
            this.maxNoOfIterUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.maxNoOfIterUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.maxNoOfIterUpDown.Name = "maxNoOfIterUpDown";
            this.maxNoOfIterUpDown.Size = new System.Drawing.Size(60, 24);
            this.maxNoOfIterUpDown.TabIndex = 18;
            this.maxNoOfIterUpDown.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            // 
            // maxNoOfNotImprovingIterationsLayout
            // 
            this.maxNoOfNotImprovingIterationsLayout.BackColor = System.Drawing.Color.Transparent;
            this.maxNoOfNotImprovingIterationsLayout.Controls.Add(this.maxNoOfNotImprovingIterationsLabel);
            this.maxNoOfNotImprovingIterationsLayout.Controls.Add(this.maxNoOfNotImprovingIterUpDown);
            this.maxNoOfNotImprovingIterationsLayout.Location = new System.Drawing.Point(3, 187);
            this.maxNoOfNotImprovingIterationsLayout.Name = "maxNoOfNotImprovingIterationsLayout";
            this.maxNoOfNotImprovingIterationsLayout.Size = new System.Drawing.Size(497, 40);
            this.maxNoOfNotImprovingIterationsLayout.TabIndex = 20;
            // 
            // maxNoOfNotImprovingIterationsLabel
            // 
            this.maxNoOfNotImprovingIterationsLabel.AutoSize = true;
            this.maxNoOfNotImprovingIterationsLabel.Font = new System.Drawing.Font("Source Sans Pro", 9.749999F);
            this.maxNoOfNotImprovingIterationsLabel.ForeColor = System.Drawing.SystemColors.Control;
            this.maxNoOfNotImprovingIterationsLabel.Location = new System.Drawing.Point(3, 0);
            this.maxNoOfNotImprovingIterationsLabel.Name = "maxNoOfNotImprovingIterationsLabel";
            this.maxNoOfNotImprovingIterationsLabel.Size = new System.Drawing.Size(285, 17);
            this.maxNoOfNotImprovingIterationsLabel.TabIndex = 17;
            this.maxNoOfNotImprovingIterationsLabel.Text = "MAXIMAL NUMBER OF NOT IMPROVING ITERATIONS:";
            // 
            // maxNoOfNotImprovingIterUpDown
            // 
            this.maxNoOfNotImprovingIterUpDown.Font = new System.Drawing.Font("Source Sans Pro", 9.749999F);
            this.maxNoOfNotImprovingIterUpDown.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.maxNoOfNotImprovingIterUpDown.Location = new System.Drawing.Point(294, 3);
            this.maxNoOfNotImprovingIterUpDown.Maximum = new decimal(new int[] {
            250,
            0,
            0,
            0});
            this.maxNoOfNotImprovingIterUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.maxNoOfNotImprovingIterUpDown.Name = "maxNoOfNotImprovingIterUpDown";
            this.maxNoOfNotImprovingIterUpDown.Size = new System.Drawing.Size(60, 24);
            this.maxNoOfNotImprovingIterUpDown.TabIndex = 18;
            this.maxNoOfNotImprovingIterUpDown.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
            // 
            // limitLayout
            // 
            this.limitLayout.BackColor = System.Drawing.Color.Transparent;
            this.limitLayout.Controls.Add(this.limitLabel);
            this.limitLayout.Controls.Add(this.limitUpDown);
            this.limitLayout.Location = new System.Drawing.Point(3, 233);
            this.limitLayout.Name = "limitLayout";
            this.limitLayout.Size = new System.Drawing.Size(497, 40);
            this.limitLayout.TabIndex = 21;
            // 
            // limitLabel
            // 
            this.limitLabel.AutoSize = true;
            this.limitLabel.Font = new System.Drawing.Font("Source Sans Pro", 9.749999F);
            this.limitLabel.ForeColor = System.Drawing.SystemColors.Control;
            this.limitLabel.Location = new System.Drawing.Point(3, 0);
            this.limitLabel.Name = "limitLabel";
            this.limitLabel.Size = new System.Drawing.Size(39, 17);
            this.limitLabel.TabIndex = 17;
            this.limitLabel.Text = "LIMIT:";
            // 
            // limitUpDown
            // 
            this.limitUpDown.Font = new System.Drawing.Font("Source Sans Pro", 9.749999F);
            this.limitUpDown.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.limitUpDown.Location = new System.Drawing.Point(48, 3);
            this.limitUpDown.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.limitUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.limitUpDown.Name = "limitUpDown";
            this.limitUpDown.Size = new System.Drawing.Size(60, 24);
            this.limitUpDown.TabIndex = 18;
            this.limitUpDown.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // initialMaxDepthLayout
            // 
            this.initialMaxDepthLayout.BackColor = System.Drawing.Color.Transparent;
            this.initialMaxDepthLayout.Controls.Add(this.initialMaxDepthLabel);
            this.initialMaxDepthLayout.Controls.Add(this.initialMaxDepthUpDown);
            this.initialMaxDepthLayout.Location = new System.Drawing.Point(3, 279);
            this.initialMaxDepthLayout.Name = "initialMaxDepthLayout";
            this.initialMaxDepthLayout.Size = new System.Drawing.Size(497, 40);
            this.initialMaxDepthLayout.TabIndex = 19;
            // 
            // initialMaxDepthLabel
            // 
            this.initialMaxDepthLabel.AutoSize = true;
            this.initialMaxDepthLabel.Font = new System.Drawing.Font("Source Sans Pro", 9.749999F);
            this.initialMaxDepthLabel.ForeColor = System.Drawing.SystemColors.Control;
            this.initialMaxDepthLabel.Location = new System.Drawing.Point(3, 0);
            this.initialMaxDepthLabel.Name = "initialMaxDepthLabel";
            this.initialMaxDepthLabel.Size = new System.Drawing.Size(139, 17);
            this.initialMaxDepthLabel.TabIndex = 17;
            this.initialMaxDepthLabel.Text = "INITIAL MAXIMAL DEPTH:";
            // 
            // initialMaxDepthUpDown
            // 
            this.initialMaxDepthUpDown.Font = new System.Drawing.Font("Source Sans Pro", 9.749999F);
            this.initialMaxDepthUpDown.Location = new System.Drawing.Point(148, 3);
            this.initialMaxDepthUpDown.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.initialMaxDepthUpDown.Name = "initialMaxDepthUpDown";
            this.initialMaxDepthUpDown.Size = new System.Drawing.Size(60, 24);
            this.initialMaxDepthUpDown.TabIndex = 18;
            this.initialMaxDepthUpDown.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
            // 
            // maxDepthLayout
            // 
            this.maxDepthLayout.BackColor = System.Drawing.Color.Transparent;
            this.maxDepthLayout.Controls.Add(this.maxDepthLabel);
            this.maxDepthLayout.Controls.Add(this.maxDepthUpDown);
            this.maxDepthLayout.Location = new System.Drawing.Point(3, 325);
            this.maxDepthLayout.Name = "maxDepthLayout";
            this.maxDepthLayout.Size = new System.Drawing.Size(497, 40);
            this.maxDepthLayout.TabIndex = 20;
            // 
            // maxDepthLabel
            // 
            this.maxDepthLabel.AutoSize = true;
            this.maxDepthLabel.Font = new System.Drawing.Font("Source Sans Pro", 9.749999F);
            this.maxDepthLabel.ForeColor = System.Drawing.SystemColors.Control;
            this.maxDepthLabel.Location = new System.Drawing.Point(3, 0);
            this.maxDepthLabel.Name = "maxDepthLabel";
            this.maxDepthLabel.Size = new System.Drawing.Size(99, 17);
            this.maxDepthLabel.TabIndex = 17;
            this.maxDepthLabel.Text = "MAXIMAL DEPTH:";
            // 
            // maxDepthUpDown
            // 
            this.maxDepthUpDown.Font = new System.Drawing.Font("Source Sans Pro", 9.749999F);
            this.maxDepthUpDown.Location = new System.Drawing.Point(108, 3);
            this.maxDepthUpDown.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.maxDepthUpDown.Name = "maxDepthUpDown";
            this.maxDepthUpDown.Size = new System.Drawing.Size(60, 24);
            this.maxDepthUpDown.TabIndex = 18;
            this.maxDepthUpDown.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            // 
            // alphaLayout
            // 
            this.alphaLayout.BackColor = System.Drawing.Color.Transparent;
            this.alphaLayout.Controls.Add(this.alphaLabel);
            this.alphaLayout.Controls.Add(this.alphaUpDown);
            this.alphaLayout.Location = new System.Drawing.Point(3, 371);
            this.alphaLayout.Name = "alphaLayout";
            this.alphaLayout.Size = new System.Drawing.Size(497, 40);
            this.alphaLayout.TabIndex = 22;
            // 
            // alphaLabel
            // 
            this.alphaLabel.AutoSize = true;
            this.alphaLabel.Font = new System.Drawing.Font("Source Sans Pro", 9.749999F);
            this.alphaLabel.ForeColor = System.Drawing.SystemColors.Control;
            this.alphaLabel.Location = new System.Drawing.Point(3, 0);
            this.alphaLabel.Name = "alphaLabel";
            this.alphaLabel.Size = new System.Drawing.Size(46, 17);
            this.alphaLabel.TabIndex = 17;
            this.alphaLabel.Text = "ALPHA:";
            // 
            // alphaUpDown
            // 
            this.alphaUpDown.DecimalPlaces = 2;
            this.alphaUpDown.Font = new System.Drawing.Font("Source Sans Pro", 9.749999F);
            this.alphaUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.alphaUpDown.Location = new System.Drawing.Point(55, 3);
            this.alphaUpDown.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            131072});
            this.alphaUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.alphaUpDown.Name = "alphaUpDown";
            this.alphaUpDown.Size = new System.Drawing.Size(60, 24);
            this.alphaUpDown.TabIndex = 18;
            this.alphaUpDown.Value = new decimal(new int[] {
            9,
            0,
            0,
            65536});
            // 
            // probabilityLayout
            // 
            this.probabilityLayout.BackColor = System.Drawing.Color.Transparent;
            this.probabilityLayout.Controls.Add(this.probabilityLabel);
            this.probabilityLayout.Controls.Add(this.probabilityUpDown);
            this.probabilityLayout.Location = new System.Drawing.Point(3, 417);
            this.probabilityLayout.Name = "probabilityLayout";
            this.probabilityLayout.Size = new System.Drawing.Size(493, 40);
            this.probabilityLayout.TabIndex = 23;
            // 
            // probabilityLabel
            // 
            this.probabilityLabel.AutoSize = true;
            this.probabilityLabel.Font = new System.Drawing.Font("Source Sans Pro", 9.749999F);
            this.probabilityLabel.ForeColor = System.Drawing.SystemColors.Control;
            this.probabilityLabel.Location = new System.Drawing.Point(3, 0);
            this.probabilityLabel.Name = "probabilityLabel";
            this.probabilityLabel.Size = new System.Drawing.Size(82, 17);
            this.probabilityLabel.TabIndex = 17;
            this.probabilityLabel.Text = "PROBABILITY:";
            // 
            // probabilityUpDown
            // 
            this.probabilityUpDown.DecimalPlaces = 2;
            this.probabilityUpDown.Font = new System.Drawing.Font("Source Sans Pro", 9.749999F);
            this.probabilityUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.probabilityUpDown.Location = new System.Drawing.Point(91, 3);
            this.probabilityUpDown.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            131072});
            this.probabilityUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.probabilityUpDown.Name = "probabilityUpDown";
            this.probabilityUpDown.Size = new System.Drawing.Size(60, 24);
            this.probabilityUpDown.TabIndex = 18;
            this.probabilityUpDown.Value = new decimal(new int[] {
            9,
            0,
            0,
            65536});
            // 
            // generatingTreeMethodLayout
            // 
            this.generatingTreeMethodLayout.BackColor = System.Drawing.Color.Transparent;
            this.generatingTreeMethodLayout.Controls.Add(this.generatingTreeMethod);
            this.generatingTreeMethodLayout.Controls.Add(this.rampedMethodRadioButton);
            this.generatingTreeMethodLayout.Controls.Add(this.growMethodRadioButton);
            this.generatingTreeMethodLayout.Controls.Add(this.fullMethodRadioButton);
            this.generatingTreeMethodLayout.Location = new System.Drawing.Point(3, 463);
            this.generatingTreeMethodLayout.Name = "generatingTreeMethodLayout";
            this.generatingTreeMethodLayout.Size = new System.Drawing.Size(493, 40);
            this.generatingTreeMethodLayout.TabIndex = 18;
            // 
            // generatingTreeMethod
            // 
            this.generatingTreeMethod.AutoSize = true;
            this.generatingTreeMethod.Font = new System.Drawing.Font("Source Sans Pro", 9.749999F);
            this.generatingTreeMethod.ForeColor = System.Drawing.SystemColors.Control;
            this.generatingTreeMethod.Location = new System.Drawing.Point(3, 0);
            this.generatingTreeMethod.Name = "generatingTreeMethod";
            this.generatingTreeMethod.Size = new System.Drawing.Size(162, 17);
            this.generatingTreeMethod.TabIndex = 17;
            this.generatingTreeMethod.Text = "GENERATING TREE METHOD:";
            // 
            // rampedMethodRadioButton
            // 
            this.rampedMethodRadioButton.AutoSize = true;
            this.rampedMethodRadioButton.Checked = true;
            this.rampedMethodRadioButton.Font = new System.Drawing.Font("Source Sans Pro", 9.749999F);
            this.rampedMethodRadioButton.ForeColor = System.Drawing.SystemColors.Control;
            this.rampedMethodRadioButton.Location = new System.Drawing.Point(171, 3);
            this.rampedMethodRadioButton.Name = "rampedMethodRadioButton";
            this.rampedMethodRadioButton.Size = new System.Drawing.Size(69, 21);
            this.rampedMethodRadioButton.TabIndex = 0;
            this.rampedMethodRadioButton.TabStop = true;
            this.rampedMethodRadioButton.Text = "ramped";
            this.rampedMethodRadioButton.UseVisualStyleBackColor = true;
            // 
            // growMethodRadioButton
            // 
            this.growMethodRadioButton.AutoSize = true;
            this.growMethodRadioButton.Font = new System.Drawing.Font("Source Sans Pro", 9.749999F);
            this.growMethodRadioButton.ForeColor = System.Drawing.SystemColors.Control;
            this.growMethodRadioButton.Location = new System.Drawing.Point(246, 3);
            this.growMethodRadioButton.Name = "growMethodRadioButton";
            this.growMethodRadioButton.Size = new System.Drawing.Size(54, 21);
            this.growMethodRadioButton.TabIndex = 1;
            this.growMethodRadioButton.Text = "grow";
            this.growMethodRadioButton.UseVisualStyleBackColor = true;
            // 
            // fullMethodRadioButton
            // 
            this.fullMethodRadioButton.AutoSize = true;
            this.fullMethodRadioButton.Font = new System.Drawing.Font("Source Sans Pro", 9.749999F);
            this.fullMethodRadioButton.ForeColor = System.Drawing.SystemColors.Control;
            this.fullMethodRadioButton.Location = new System.Drawing.Point(306, 3);
            this.fullMethodRadioButton.Name = "fullMethodRadioButton";
            this.fullMethodRadioButton.Size = new System.Drawing.Size(43, 21);
            this.fullMethodRadioButton.TabIndex = 18;
            this.fullMethodRadioButton.Text = "full";
            this.fullMethodRadioButton.UseVisualStyleBackColor = true;
            // 
            // numberOfRunsLayout
            // 
            this.numberOfRunsLayout.BackColor = System.Drawing.Color.Transparent;
            this.numberOfRunsLayout.Controls.Add(this.numberOfRunsLabel);
            this.numberOfRunsLayout.Controls.Add(this.numberOfRunsUpDown);
            this.numberOfRunsLayout.Location = new System.Drawing.Point(3, 509);
            this.numberOfRunsLayout.Name = "numberOfRunsLayout";
            this.numberOfRunsLayout.Size = new System.Drawing.Size(493, 40);
            this.numberOfRunsLayout.TabIndex = 20;
            // 
            // numberOfRunsLabel
            // 
            this.numberOfRunsLabel.AutoSize = true;
            this.numberOfRunsLabel.Font = new System.Drawing.Font("Source Sans Pro", 9.749999F);
            this.numberOfRunsLabel.ForeColor = System.Drawing.SystemColors.Control;
            this.numberOfRunsLabel.Location = new System.Drawing.Point(3, 0);
            this.numberOfRunsLabel.Name = "numberOfRunsLabel";
            this.numberOfRunsLabel.Size = new System.Drawing.Size(109, 17);
            this.numberOfRunsLabel.TabIndex = 17;
            this.numberOfRunsLabel.Text = "NUMBER OF RUNS:";
            // 
            // numberOfRunsUpDown
            // 
            this.numberOfRunsUpDown.Font = new System.Drawing.Font("Source Sans Pro", 9.749999F);
            this.numberOfRunsUpDown.Location = new System.Drawing.Point(118, 3);
            this.numberOfRunsUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numberOfRunsUpDown.Name = "numberOfRunsUpDown";
            this.numberOfRunsUpDown.Size = new System.Drawing.Size(60, 24);
            this.numberOfRunsUpDown.TabIndex = 18;
            this.numberOfRunsUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lookupConsoleFormSign
            // 
            this.lookupConsoleFormSign.BackColor = System.Drawing.Color.Transparent;
            this.lookupConsoleFormSign.BackgroundImage = global::antico.Properties.Resources.console_lookup_darker;
            this.lookupConsoleFormSign.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.lookupConsoleFormSign.Location = new System.Drawing.Point(228, 23);
            this.lookupConsoleFormSign.Name = "lookupConsoleFormSign";
            this.lookupConsoleFormSign.Size = new System.Drawing.Size(50, 50);
            this.lookupConsoleFormSign.TabIndex = 17;
            this.lookupConsoleFormSign.TabStop = false;
            this.lookupConsoleFormSign.Visible = false;
            this.lookupConsoleFormSign.MouseClick += new System.Windows.Forms.MouseEventHandler(this.LookupConsoleFormSign_MouseClick);
            this.lookupConsoleFormSign.MouseEnter += new System.EventHandler(this.LookupConsoleFormSign_MouseEnter);
            this.lookupConsoleFormSign.MouseLeave += new System.EventHandler(this.LookupConsoleForSign_MouseLeave);
            this.lookupConsoleFormSign.MouseHover += new System.EventHandler(this.LookupConsoleFormSign_MouseHover);
            // 
            // minimizeSign
            // 
            this.minimizeSign.BackColor = System.Drawing.Color.Transparent;
            this.minimizeSign.Image = global::antico.Properties.Resources.minimizee;
            this.minimizeSign.Location = new System.Drawing.Point(832, 30);
            this.minimizeSign.Name = "minimizeSign";
            this.minimizeSign.Size = new System.Drawing.Size(70, 70);
            this.minimizeSign.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.minimizeSign.TabIndex = 23;
            this.minimizeSign.TabStop = false;
            this.minimizeSign.MouseClick += new System.Windows.Forms.MouseEventHandler(this.MinimizeSign_MouseClick);
            this.minimizeSign.MouseEnter += new System.EventHandler(this.MinimizeSign_MouseEnter);
            this.minimizeSign.MouseLeave += new System.EventHandler(this.MinimizeSign_MouseLeave);
            this.minimizeSign.MouseHover += new System.EventHandler(this.MinimizeSign_MouseHover);
            // 
            // exitSign
            // 
            this.exitSign.BackColor = System.Drawing.Color.Transparent;
            this.exitSign.Image = global::antico.Properties.Resources.exit_white;
            this.exitSign.Location = new System.Drawing.Point(912, 30);
            this.exitSign.Name = "exitSign";
            this.exitSign.Size = new System.Drawing.Size(70, 70);
            this.exitSign.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.exitSign.TabIndex = 24;
            this.exitSign.TabStop = false;
            this.exitSign.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ExitSign_MouseClick);
            this.exitSign.MouseEnter += new System.EventHandler(this.ExitSign_MouseEnter);
            this.exitSign.MouseLeave += new System.EventHandler(this.ExitSign_MouseLeave);
            this.exitSign.MouseHover += new System.EventHandler(this.ExitSign_MouseHover);
            // 
            // uploadSign
            // 
            this.uploadSign.BackColor = System.Drawing.Color.Transparent;
            this.uploadSign.BackgroundImage = global::antico.Properties.Resources.upload;
            this.uploadSign.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.uploadSign.Location = new System.Drawing.Point(90, 17);
            this.uploadSign.Name = "uploadSign";
            this.uploadSign.Size = new System.Drawing.Size(50, 50);
            this.uploadSign.TabIndex = 25;
            this.uploadSign.TabStop = false;
            this.uploadSign.MouseClick += new System.Windows.Forms.MouseEventHandler(this.UploadSign_MouseClick);
            this.uploadSign.MouseEnter += new System.EventHandler(this.UploadSign_MouseEnter);
            this.uploadSign.MouseLeave += new System.EventHandler(this.UploadSign_MouseLeave);
            this.uploadSign.MouseHover += new System.EventHandler(this.UploadSign_MouseHover);
            // 
            // showSolutionsProgressSign
            // 
            this.showSolutionsProgressSign.BackColor = System.Drawing.Color.Transparent;
            this.showSolutionsProgressSign.BackgroundImage = global::antico.Properties.Resources.progress_chart_darker;
            this.showSolutionsProgressSign.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.showSolutionsProgressSign.Location = new System.Drawing.Point(304, 18);
            this.showSolutionsProgressSign.Name = "showSolutionsProgressSign";
            this.showSolutionsProgressSign.Size = new System.Drawing.Size(55, 55);
            this.showSolutionsProgressSign.TabIndex = 26;
            this.showSolutionsProgressSign.TabStop = false;
            this.showSolutionsProgressSign.Visible = false;
            this.showSolutionsProgressSign.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ShowSolutionsProgressSign_MouseClick);
            this.showSolutionsProgressSign.MouseEnter += new System.EventHandler(this.ShowSolutionsProgressSign_MouseEnter);
            this.showSolutionsProgressSign.MouseLeave += new System.EventHandler(this.ShowSolutionsProgressSign_MouseLeave);
            this.showSolutionsProgressSign.MouseHover += new System.EventHandler(this.ShowSolutionsProgressSign_MouseHover);
            // 
            // progressBar
            // 
            this.progressBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar.Location = new System.Drawing.Point(0, 677);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(1012, 10);
            this.progressBar.TabIndex = 27;
            this.progressBar.Visible = false;
            // 
            // CreateNewModelForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::antico.Properties.Resources.images;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1012, 687);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.mainLayout);
            this.Controls.Add(this.showSolutionsProgressSign);
            this.Controls.Add(this.uploadSign);
            this.Controls.Add(this.exitSign);
            this.Controls.Add(this.minimizeSign);
            this.Controls.Add(this.lookupConsoleFormSign);
            this.Controls.Add(this.visualizeSign);
            this.Controls.Add(this.saveSign);
            this.Controls.Add(this.startSign);
            this.Controls.Add(this.parametersSettingsSign);
            this.Controls.Add(this.anticoLabel);
            this.Controls.Add(this.goBackSign);
            this.Controls.Add(this.waitingAnimation);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CreateNewModelForm";
            this.Opacity = 0.98D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "CreateNewModelForm";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CreateNewModelForm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.CreateNewModelForm_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.CreateNewModelForm_MouseUp);
            this.Resize += new System.EventHandler(this.CreateNewModelForm_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.goBackSign)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.parametersSettingsSign)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.startSign)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.saveSign)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.visualizeSign)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.waitingAnimation)).EndInit();
            this.databaseRadioLayout.ResumeLayout(false);
            this.databaseRadioLayout.PerformLayout();
            this.mainLayout.ResumeLayout(false);
            this.nonTerminalSelectionLayout.ResumeLayout(false);
            this.nonTerminalSelectionLayout.PerformLayout();
            this.colonySizeLayout.ResumeLayout(false);
            this.colonySizeLayout.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.colonySizeUpDown)).EndInit();
            this.maximalNumberOfIterationsLayout.ResumeLayout(false);
            this.maximalNumberOfIterationsLayout.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maxNoOfIterUpDown)).EndInit();
            this.maxNoOfNotImprovingIterationsLayout.ResumeLayout(false);
            this.maxNoOfNotImprovingIterationsLayout.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maxNoOfNotImprovingIterUpDown)).EndInit();
            this.limitLayout.ResumeLayout(false);
            this.limitLayout.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.limitUpDown)).EndInit();
            this.initialMaxDepthLayout.ResumeLayout(false);
            this.initialMaxDepthLayout.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.initialMaxDepthUpDown)).EndInit();
            this.maxDepthLayout.ResumeLayout(false);
            this.maxDepthLayout.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maxDepthUpDown)).EndInit();
            this.alphaLayout.ResumeLayout(false);
            this.alphaLayout.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.alphaUpDown)).EndInit();
            this.probabilityLayout.ResumeLayout(false);
            this.probabilityLayout.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.probabilityUpDown)).EndInit();
            this.generatingTreeMethodLayout.ResumeLayout(false);
            this.generatingTreeMethodLayout.PerformLayout();
            this.numberOfRunsLayout.ResumeLayout(false);
            this.numberOfRunsLayout.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfRunsUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookupConsoleFormSign)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minimizeSign)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.exitSign)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uploadSign)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.showSolutionsProgressSign)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox goBackSign;
        private System.Windows.Forms.Label anticoLabel;
        private System.Windows.Forms.PictureBox parametersSettingsSign;
        private System.Windows.Forms.PictureBox startSign;
        private System.Windows.Forms.PictureBox saveSign;
        private System.Windows.Forms.PictureBox visualizeSign;
        private System.Windows.Forms.PictureBox waitingAnimation;
        private System.Windows.Forms.FlowLayoutPanel databaseRadioLayout;
        private System.Windows.Forms.FlowLayoutPanel mainLayout;
        private System.Windows.Forms.Label databaseLabel;
        private System.Windows.Forms.FlowLayoutPanel nonTerminalSelectionLayout;
        private System.Windows.Forms.Label nonTerminalsLabel;
        private System.Windows.Forms.CheckBox plusSelected;
        private System.Windows.Forms.CheckBox minusSelected;
        private System.Windows.Forms.CheckBox timesSelected;
        private System.Windows.Forms.CheckBox divisionSelected;
        private System.Windows.Forms.CheckBox sinSelected;
        private System.Windows.Forms.CheckBox cosSelected;
        private System.Windows.Forms.CheckBox expSelected;
        private System.Windows.Forms.CheckBox rlogSelected;
        private System.Windows.Forms.FlowLayoutPanel colonySizeLayout;
        private System.Windows.Forms.Label colonySize;
        private System.Windows.Forms.NumericUpDown colonySizeUpDown;
        private System.Windows.Forms.FlowLayoutPanel maximalNumberOfIterationsLayout;
        private System.Windows.Forms.Label maximalNumberOfIterationsLabel;
        private System.Windows.Forms.NumericUpDown maxNoOfIterUpDown;
        private System.Windows.Forms.FlowLayoutPanel maxNoOfNotImprovingIterationsLayout;
        private System.Windows.Forms.Label maxNoOfNotImprovingIterationsLabel;
        private System.Windows.Forms.NumericUpDown maxNoOfNotImprovingIterUpDown;
        private System.Windows.Forms.FlowLayoutPanel initialMaxDepthLayout;
        private System.Windows.Forms.Label initialMaxDepthLabel;
        private System.Windows.Forms.NumericUpDown initialMaxDepthUpDown;
        private System.Windows.Forms.FlowLayoutPanel maxDepthLayout;
        private System.Windows.Forms.Label maxDepthLabel;
        private System.Windows.Forms.NumericUpDown maxDepthUpDown;
        private System.Windows.Forms.FlowLayoutPanel limitLayout;
        private System.Windows.Forms.Label limitLabel;
        private System.Windows.Forms.NumericUpDown limitUpDown;
        private System.Windows.Forms.FlowLayoutPanel alphaLayout;
        private System.Windows.Forms.Label alphaLabel;
        private System.Windows.Forms.NumericUpDown alphaUpDown;
        private System.Windows.Forms.FlowLayoutPanel probabilityLayout;
        private System.Windows.Forms.Label probabilityLabel;
        private System.Windows.Forms.NumericUpDown probabilityUpDown;
        private System.Windows.Forms.FlowLayoutPanel generatingTreeMethodLayout;
        private System.Windows.Forms.Label generatingTreeMethod;
        private System.Windows.Forms.RadioButton rampedMethodRadioButton;
        private System.Windows.Forms.RadioButton growMethodRadioButton;
        private System.Windows.Forms.RadioButton fullMethodRadioButton;
        private System.Windows.Forms.FlowLayoutPanel numberOfRunsLayout;
        private System.Windows.Forms.Label numberOfRunsLabel;
        private System.Windows.Forms.NumericUpDown numberOfRunsUpDown;
        private System.Windows.Forms.PictureBox lookupConsoleFormSign;
        private System.Windows.Forms.Label printoutOfAllSolutionsLabel;
        private System.Windows.Forms.Panel panelForPrintoutLabel;
        private System.Windows.Forms.ComboBox databaseComboBox;
        private PictureBox minimizeSign;
        private PictureBox exitSign;
        private PictureBox uploadSign;
        private PictureBox showSolutionsProgressSign;
        private ProgressBar progressBar;
    }
}