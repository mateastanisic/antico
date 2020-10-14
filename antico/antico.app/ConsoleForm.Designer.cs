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
            this.heuristicPrintoutTextBox = new System.Windows.Forms.TextBox();
            this.exitSign = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.exitSign)).BeginInit();
            this.SuspendLayout();
            // 
            // heuristicPrintoutTextBox
            // 
            this.heuristicPrintoutTextBox.BackColor = System.Drawing.SystemColors.WindowText;
            this.heuristicPrintoutTextBox.ForeColor = System.Drawing.SystemColors.Control;
            this.heuristicPrintoutTextBox.Location = new System.Drawing.Point(-2, -2);
            this.heuristicPrintoutTextBox.Multiline = true;
            this.heuristicPrintoutTextBox.Name = "heuristicPrintoutTextBox";
            this.heuristicPrintoutTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.heuristicPrintoutTextBox.Size = new System.Drawing.Size(468, 695);
            this.heuristicPrintoutTextBox.TabIndex = 1;
            this.heuristicPrintoutTextBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.heuristicPrintoutTextBox_MouseDown);
            this.heuristicPrintoutTextBox.MouseEnter += new System.EventHandler(this.heuristicPrintoutTextBox_MouseEnter);
            this.heuristicPrintoutTextBox.MouseLeave += new System.EventHandler(this.heuristicPrintoutTextBox_MouseLeave);
            this.heuristicPrintoutTextBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.heuristicPrintoutTextBox_MouseMove);
            this.heuristicPrintoutTextBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.heuristicPrintoutTextBox_MouseUp);
            // 
            // exitSign
            // 
            this.exitSign.BackColor = System.Drawing.Color.Transparent;
            this.exitSign.Image = ((System.Drawing.Image)(resources.GetObject("exitSign.Image")));
            this.exitSign.Location = new System.Drawing.Point(405, 3);
            this.exitSign.Name = "exitSign";
            this.exitSign.Size = new System.Drawing.Size(37, 33);
            this.exitSign.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.exitSign.TabIndex = 8;
            this.exitSign.TabStop = false;
            this.exitSign.MouseClick += new System.Windows.Forms.MouseEventHandler(this.exitSign_MouseClick);
            this.exitSign.MouseEnter += new System.EventHandler(this.exitSign_MouseEnter);
            this.exitSign.MouseLeave += new System.EventHandler(this.exitSign_MouseLeave);
            this.exitSign.MouseHover += new System.EventHandler(this.exitSign_MouseHover);
            // 
            // ConsoleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.WindowText;
            this.ClientSize = new System.Drawing.Size(465, 687);
            this.Controls.Add(this.exitSign);
            this.Controls.Add(this.heuristicPrintoutTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ConsoleForm";
            this.Opacity = 0.95D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "ConsoleForm";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ConsoleForm_KeyUp);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ConsoleForm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ConsoleForm_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ConsoleForm_MouseUp);
            ((System.ComponentModel.ISupportInitialize)(this.exitSign)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox heuristicPrintoutTextBox;
        public System.Windows.Forms.PictureBox exitSign;
    }
}