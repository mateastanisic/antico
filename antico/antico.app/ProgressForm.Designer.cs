namespace antico
{
    partial class ProgressForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProgressForm));
            this.exitSign = new System.Windows.Forms.PictureBox();
            this.minimizeSign = new System.Windows.Forms.PictureBox();
            this.anticoIconInTitlePanel = new System.Windows.Forms.PictureBox();
            this.PanelForMoving = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.exitSign)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minimizeSign)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.anticoIconInTitlePanel)).BeginInit();
            this.SuspendLayout();
            // 
            // exitSign
            // 
            this.exitSign.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.exitSign.Image = global::antico.Properties.Resources.exit;
            this.exitSign.Location = new System.Drawing.Point(984, 5);
            this.exitSign.Name = "exitSign";
            this.exitSign.Size = new System.Drawing.Size(20, 20);
            this.exitSign.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.exitSign.TabIndex = 18;
            this.exitSign.TabStop = false;
            this.exitSign.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ExitSign_MouseClick);
            this.exitSign.MouseEnter += new System.EventHandler(this.ExitSign_MouseEnter);
            this.exitSign.MouseLeave += new System.EventHandler(this.ExitSign_MouseLeave);
            // 
            // minimizeSign
            // 
            this.minimizeSign.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.minimizeSign.Image = global::antico.Properties.Resources.minimize_dark;
            this.minimizeSign.Location = new System.Drawing.Point(956, 5);
            this.minimizeSign.Name = "minimizeSign";
            this.minimizeSign.Size = new System.Drawing.Size(20, 20);
            this.minimizeSign.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.minimizeSign.TabIndex = 19;
            this.minimizeSign.TabStop = false;
            this.minimizeSign.MouseClick += new System.Windows.Forms.MouseEventHandler(this.MinimizeSign_MouseClick);
            this.minimizeSign.MouseEnter += new System.EventHandler(this.MinimizeSign_MouseEnter);
            this.minimizeSign.MouseLeave += new System.EventHandler(this.MinimizeSign_MouseLeave);
            // 
            // anticoIconInTitlePanel
            // 
            this.anticoIconInTitlePanel.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.anticoIconInTitlePanel.BackgroundImage = global::antico.Properties.Resources.app_icon;
            this.anticoIconInTitlePanel.Image = global::antico.Properties.Resources.app_icon;
            this.anticoIconInTitlePanel.Location = new System.Drawing.Point(6, 5);
            this.anticoIconInTitlePanel.Name = "anticoIconInTitlePanel";
            this.anticoIconInTitlePanel.Size = new System.Drawing.Size(20, 20);
            this.anticoIconInTitlePanel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.anticoIconInTitlePanel.TabIndex = 22;
            this.anticoIconInTitlePanel.TabStop = false;
            // 
            // PanelForMoving
            // 
            this.PanelForMoving.Dock = System.Windows.Forms.DockStyle.Top;
            this.PanelForMoving.Location = new System.Drawing.Point(0, 0);
            this.PanelForMoving.Name = "PanelForMoving";
            this.PanelForMoving.Size = new System.Drawing.Size(1012, 44);
            this.PanelForMoving.TabIndex = 23;
            this.PanelForMoving.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PanelForMoving_MouseDown);
            this.PanelForMoving.MouseEnter += new System.EventHandler(this.PanelForMoving_MouseEnter);
            this.PanelForMoving.MouseLeave += new System.EventHandler(this.PanelForMoving_MouseLeave);
            this.PanelForMoving.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PanelForMoving_MouseMove);
            this.PanelForMoving.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PanelForMoving_MouseUp);
            // 
            // ProgressForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1012, 687);
            this.Controls.Add(this.anticoIconInTitlePanel);
            this.Controls.Add(this.exitSign);
            this.Controls.Add(this.minimizeSign);
            this.Controls.Add(this.PanelForMoving);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(450, 300);
            this.Name = "ProgressForm";
            this.Opacity = 0.95D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "ProgressForm";
            this.SizeChanged += new System.EventHandler(this.ProgressForm_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.exitSign)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minimizeSign)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.anticoIconInTitlePanel)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        public System.Windows.Forms.PictureBox exitSign;
        private System.Windows.Forms.PictureBox minimizeSign;
        private System.Windows.Forms.PictureBox anticoIconInTitlePanel;
        private System.Windows.Forms.Panel PanelForMoving;
    }
}