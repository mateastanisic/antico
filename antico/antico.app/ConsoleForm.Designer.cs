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
            this.exitSign = new System.Windows.Forms.PictureBox();
            this.printoutPanel = new System.Windows.Forms.Panel();
            this.heuristicPrintoutTextBox = new System.Windows.Forms.TextBox();
            this.cornerTopPictureBox = new System.Windows.Forms.PictureBox();
            this.cornerBottomPictureBox = new System.Windows.Forms.PictureBox();
            this.cornerLeftPictureBox = new System.Windows.Forms.PictureBox();
            this.cornerRightPictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.exitSign)).BeginInit();
            this.printoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cornerTopPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cornerBottomPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cornerLeftPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cornerRightPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // exitSign
            // 
            this.exitSign.BackColor = System.Drawing.Color.Transparent;
            this.exitSign.Image = ((System.Drawing.Image)(resources.GetObject("exitSign.Image")));
            this.exitSign.Location = new System.Drawing.Point(372, 6);
            this.exitSign.Name = "exitSign";
            this.exitSign.Size = new System.Drawing.Size(37, 33);
            this.exitSign.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.exitSign.TabIndex = 8;
            this.exitSign.TabStop = false;
            this.exitSign.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ExitSign_MouseClick);
            this.exitSign.MouseEnter += new System.EventHandler(this.ExitSign_MouseEnter);
            this.exitSign.MouseLeave += new System.EventHandler(this.ExitSign_MouseLeave);
            this.exitSign.MouseHover += new System.EventHandler(this.ExitSign_MouseHover);
            // 
            // printoutPanel
            // 
            this.printoutPanel.AutoScroll = true;
            this.printoutPanel.BackColor = System.Drawing.Color.Transparent;
            this.printoutPanel.Controls.Add(this.exitSign);
            this.printoutPanel.Controls.Add(this.heuristicPrintoutTextBox);
            this.printoutPanel.Location = new System.Drawing.Point(12, 10);
            this.printoutPanel.Name = "printoutPanel";
            this.printoutPanel.Size = new System.Drawing.Size(441, 667);
            this.printoutPanel.TabIndex = 9;
            // 
            // heuristicPrintoutTextBox
            // 
            this.heuristicPrintoutTextBox.BackColor = System.Drawing.SystemColors.WindowText;
            this.heuristicPrintoutTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.heuristicPrintoutTextBox.Font = new System.Drawing.Font("Source Sans Pro", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.heuristicPrintoutTextBox.ForeColor = System.Drawing.SystemColors.Control;
            this.heuristicPrintoutTextBox.Location = new System.Drawing.Point(0, 0);
            this.heuristicPrintoutTextBox.Margin = new System.Windows.Forms.Padding(5);
            this.heuristicPrintoutTextBox.MaximumSize = new System.Drawing.Size(441, 0);
            this.heuristicPrintoutTextBox.MinimumSize = new System.Drawing.Size(435, 663);
            this.heuristicPrintoutTextBox.Multiline = true;
            this.heuristicPrintoutTextBox.Name = "heuristicPrintoutTextBox";
            this.heuristicPrintoutTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.heuristicPrintoutTextBox.Size = new System.Drawing.Size(441, 663);
            this.heuristicPrintoutTextBox.TabIndex = 0;
            this.heuristicPrintoutTextBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HeuristicPrintoutTextBox_MouseDown);
            this.heuristicPrintoutTextBox.MouseEnter += new System.EventHandler(this.HeuristicPrintoutTextBox_MouseEnter);
            this.heuristicPrintoutTextBox.MouseLeave += new System.EventHandler(this.HeuristicPrintoutTextBox_MouseLeave);
            this.heuristicPrintoutTextBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.HeuristicPrintoutTextBox_MouseMove);
            this.heuristicPrintoutTextBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.HeuristicPrintoutTextBox_MouseUp);
            // 
            // cornerTopPictureBox
            // 
            this.cornerTopPictureBox.BackColor = System.Drawing.SystemColors.Menu;
            this.cornerTopPictureBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.cornerTopPictureBox.Location = new System.Drawing.Point(0, 0);
            this.cornerTopPictureBox.Name = "cornerTopPictureBox";
            this.cornerTopPictureBox.Size = new System.Drawing.Size(465, 10);
            this.cornerTopPictureBox.TabIndex = 10;
            this.cornerTopPictureBox.TabStop = false;
            // 
            // cornerBottomPictureBox
            // 
            this.cornerBottomPictureBox.BackColor = System.Drawing.SystemColors.Menu;
            this.cornerBottomPictureBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.cornerBottomPictureBox.Location = new System.Drawing.Point(0, 677);
            this.cornerBottomPictureBox.Name = "cornerBottomPictureBox";
            this.cornerBottomPictureBox.Size = new System.Drawing.Size(465, 10);
            this.cornerBottomPictureBox.TabIndex = 11;
            this.cornerBottomPictureBox.TabStop = false;
            // 
            // cornerLeftPictureBox
            // 
            this.cornerLeftPictureBox.BackColor = System.Drawing.SystemColors.Menu;
            this.cornerLeftPictureBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.cornerLeftPictureBox.Location = new System.Drawing.Point(0, 10);
            this.cornerLeftPictureBox.Name = "cornerLeftPictureBox";
            this.cornerLeftPictureBox.Size = new System.Drawing.Size(10, 667);
            this.cornerLeftPictureBox.TabIndex = 12;
            this.cornerLeftPictureBox.TabStop = false;
            // 
            // cornerRightPictureBox
            // 
            this.cornerRightPictureBox.BackColor = System.Drawing.SystemColors.Menu;
            this.cornerRightPictureBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.cornerRightPictureBox.Location = new System.Drawing.Point(455, 10);
            this.cornerRightPictureBox.Name = "cornerRightPictureBox";
            this.cornerRightPictureBox.Size = new System.Drawing.Size(10, 667);
            this.cornerRightPictureBox.TabIndex = 13;
            this.cornerRightPictureBox.TabStop = false;
            // 
            // ConsoleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.WindowText;
            this.ClientSize = new System.Drawing.Size(465, 687);
            this.Controls.Add(this.cornerRightPictureBox);
            this.Controls.Add(this.cornerLeftPictureBox);
            this.Controls.Add(this.cornerBottomPictureBox);
            this.Controls.Add(this.cornerTopPictureBox);
            this.Controls.Add(this.printoutPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ConsoleForm";
            this.Opacity = 0.90D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "ConsoleForm";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ConsoleForm_KeyUp);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ConsoleForm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ConsoleForm_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ConsoleForm_MouseUp);
            ((System.ComponentModel.ISupportInitialize)(this.exitSign)).EndInit();
            this.printoutPanel.ResumeLayout(false);
            this.printoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cornerTopPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cornerBottomPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cornerLeftPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cornerRightPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.PictureBox exitSign;
        private System.Windows.Forms.Panel printoutPanel;
        private System.Windows.Forms.PictureBox cornerTopPictureBox;
        private System.Windows.Forms.PictureBox cornerBottomPictureBox;
        private System.Windows.Forms.PictureBox cornerLeftPictureBox;
        private System.Windows.Forms.PictureBox cornerRightPictureBox;
        private System.Windows.Forms.TextBox heuristicPrintoutTextBox;
    }
}