namespace antico
{
    partial class ConsoleForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConsoleForm));
            this.printoutPanel = new System.Windows.Forms.Panel();
            this.heuristicPrintoutTextBox = new System.Windows.Forms.TextBox();
            this.titlePanel = new System.Windows.Forms.Panel();
            this.titleLabel = new System.Windows.Forms.Label();
            this.anticoIconInTitlePanel = new System.Windows.Forms.PictureBox();
            this.minimizeSign = new System.Windows.Forms.PictureBox();
            this.exitSign = new System.Windows.Forms.PictureBox();
            this.printoutPanel.SuspendLayout();
            this.titlePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.anticoIconInTitlePanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minimizeSign)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.exitSign)).BeginInit();
            this.SuspendLayout();
            // 
            // printoutPanel
            // 
            this.printoutPanel.AutoScroll = true;
            this.printoutPanel.BackColor = System.Drawing.Color.Transparent;
            this.printoutPanel.Controls.Add(this.heuristicPrintoutTextBox);
            this.printoutPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.printoutPanel.Location = new System.Drawing.Point(0, 24);
            this.printoutPanel.Name = "printoutPanel";
            this.printoutPanel.Size = new System.Drawing.Size(465, 663);
            this.printoutPanel.TabIndex = 9;
            // 
            // heuristicPrintoutTextBox
            // 
            this.heuristicPrintoutTextBox.BackColor = System.Drawing.SystemColors.WindowText;
            this.heuristicPrintoutTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.heuristicPrintoutTextBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.heuristicPrintoutTextBox.Font = new System.Drawing.Font("Source Sans Pro", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.heuristicPrintoutTextBox.ForeColor = System.Drawing.SystemColors.Control;
            this.heuristicPrintoutTextBox.Location = new System.Drawing.Point(0, 0);
            this.heuristicPrintoutTextBox.Margin = new System.Windows.Forms.Padding(5);
            this.heuristicPrintoutTextBox.MaximumSize = new System.Drawing.Size(465, 0);
            this.heuristicPrintoutTextBox.MinimumSize = new System.Drawing.Size(435, 663);
            this.heuristicPrintoutTextBox.Multiline = true;
            this.heuristicPrintoutTextBox.Name = "heuristicPrintoutTextBox";
            this.heuristicPrintoutTextBox.ReadOnly = true;
            this.heuristicPrintoutTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.heuristicPrintoutTextBox.Size = new System.Drawing.Size(465, 663);
            this.heuristicPrintoutTextBox.TabIndex = 0;
            // 
            // titlePanel
            // 
            this.titlePanel.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.titlePanel.Controls.Add(this.titleLabel);
            this.titlePanel.Controls.Add(this.anticoIconInTitlePanel);
            this.titlePanel.Controls.Add(this.minimizeSign);
            this.titlePanel.Controls.Add(this.exitSign);
            this.titlePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.titlePanel.Location = new System.Drawing.Point(0, 0);
            this.titlePanel.Name = "titlePanel";
            this.titlePanel.Size = new System.Drawing.Size(465, 24);
            this.titlePanel.TabIndex = 18;
            this.titlePanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TitlePanel_MouseDown);
            this.titlePanel.MouseEnter += new System.EventHandler(this.TitlePanel_MouseEnter);
            this.titlePanel.MouseLeave += new System.EventHandler(this.TitlePanel_MouseLeave);
            this.titlePanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TitlePanel_MouseMove);
            this.titlePanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TitlePanel_MouseUp);
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("Source Sans Pro Semibold", 9.749999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleLabel.Location = new System.Drawing.Point(29, 4);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(53, 17);
            this.titleLabel.TabIndex = 18;
            this.titleLabel.Text = "Console";
            // 
            // anticoIconInTitlePanel
            // 
            this.anticoIconInTitlePanel.BackgroundImage = global::antico.Properties.Resources.app_icon;
            this.anticoIconInTitlePanel.Image = global::antico.Properties.Resources.app_icon;
            this.anticoIconInTitlePanel.Location = new System.Drawing.Point(3, 2);
            this.anticoIconInTitlePanel.Name = "anticoIconInTitlePanel";
            this.anticoIconInTitlePanel.Size = new System.Drawing.Size(20, 20);
            this.anticoIconInTitlePanel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.anticoIconInTitlePanel.TabIndex = 4;
            this.anticoIconInTitlePanel.TabStop = false;
            // 
            // minimizeSign
            // 
            this.minimizeSign.BackColor = System.Drawing.Color.Transparent;
            this.minimizeSign.Image = global::antico.Properties.Resources.minimize_dark;
            this.minimizeSign.Location = new System.Drawing.Point(411, 2);
            this.minimizeSign.Name = "minimizeSign";
            this.minimizeSign.Size = new System.Drawing.Size(20, 20);
            this.minimizeSign.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.minimizeSign.TabIndex = 17;
            this.minimizeSign.TabStop = false;
            this.minimizeSign.MouseClick += new System.Windows.Forms.MouseEventHandler(this.MinimizeSign_MouseClick);
            this.minimizeSign.MouseEnter += new System.EventHandler(this.MinimizeSign_MouseEnter);
            this.minimizeSign.MouseLeave += new System.EventHandler(this.MinimizeSign_MouseLeave);
            this.minimizeSign.MouseHover += new System.EventHandler(this.MinimizeSign_MouseHover);
            // 
            // exitSign
            // 
            this.exitSign.BackColor = System.Drawing.Color.Transparent;
            this.exitSign.Image = global::antico.Properties.Resources.exit;
            this.exitSign.Location = new System.Drawing.Point(437, 2);
            this.exitSign.Name = "exitSign";
            this.exitSign.Size = new System.Drawing.Size(20, 20);
            this.exitSign.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.exitSign.TabIndex = 8;
            this.exitSign.TabStop = false;
            this.exitSign.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ExitSign_MouseClick);
            this.exitSign.MouseEnter += new System.EventHandler(this.ExitSign_MouseEnter);
            this.exitSign.MouseLeave += new System.EventHandler(this.ExitSign_MouseLeave);
            this.exitSign.MouseHover += new System.EventHandler(this.ExitSign_MouseHover);
            // 
            // ConsoleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.WindowText;
            this.ClientSize = new System.Drawing.Size(465, 687);
            this.Controls.Add(this.printoutPanel);
            this.Controls.Add(this.titlePanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ConsoleForm";
            this.Opacity = 0.9D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "ConsoleForm";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ConsoleForm_KeyUp);
            this.printoutPanel.ResumeLayout(false);
            this.printoutPanel.PerformLayout();
            this.titlePanel.ResumeLayout(false);
            this.titlePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.anticoIconInTitlePanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minimizeSign)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.exitSign)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.PictureBox exitSign;
        private System.Windows.Forms.Panel printoutPanel;
        private System.Windows.Forms.TextBox heuristicPrintoutTextBox;
        private System.Windows.Forms.PictureBox minimizeSign;
        private System.Windows.Forms.Panel titlePanel;
        private System.Windows.Forms.PictureBox anticoIconInTitlePanel;
        private System.Windows.Forms.Label titleLabel;
    }
}