namespace antico
{
    partial class CustomDialogBox
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomDialogBox));
            this.titlePanel = new System.Windows.Forms.Panel();
            this.minimizeSign = new System.Windows.Forms.PictureBox();
            this.exitSign = new System.Windows.Forms.PictureBox();
            this.anticoIconInTitlePanel = new System.Windows.Forms.PictureBox();
            this.titleLabel = new System.Windows.Forms.Label();
            this.okCancleButton = new System.Windows.Forms.Button();
            this.noOkButton = new System.Windows.Forms.Button();
            this.yesButton = new System.Windows.Forms.Button();
            this.saveOrVisualizeLabel = new System.Windows.Forms.Label();
            this.modelListComboBox = new System.Windows.Forms.ComboBox();
            this.nameOfTheModelLabel = new System.Windows.Forms.Label();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.customDialogBoxIcon = new System.Windows.Forms.PictureBox();
            this.messageLabel = new System.Windows.Forms.TextBox();
            this.titlePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.minimizeSign)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.exitSign)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.anticoIconInTitlePanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.customDialogBoxIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // titlePanel
            // 
            this.titlePanel.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.titlePanel.Controls.Add(this.minimizeSign);
            this.titlePanel.Controls.Add(this.exitSign);
            this.titlePanel.Controls.Add(this.anticoIconInTitlePanel);
            this.titlePanel.Controls.Add(this.titleLabel);
            this.titlePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.titlePanel.Location = new System.Drawing.Point(0, 0);
            this.titlePanel.Name = "titlePanel";
            this.titlePanel.Size = new System.Drawing.Size(433, 28);
            this.titlePanel.TabIndex = 0;
            this.titlePanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TitlePanel_MouseDown);
            this.titlePanel.MouseEnter += new System.EventHandler(this.TitlePanel_MouseEnter);
            this.titlePanel.MouseLeave += new System.EventHandler(this.TitlePanel_MouseLeave);
            this.titlePanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TitlePanel_MouseMove);
            this.titlePanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TitlePanel_MouseUp);
            // 
            // minimizeSign
            // 
            this.minimizeSign.BackColor = System.Drawing.Color.Transparent;
            this.minimizeSign.Image = global::antico.Properties.Resources.minimize_dark;
            this.minimizeSign.Location = new System.Drawing.Point(375, 4);
            this.minimizeSign.Name = "minimizeSign";
            this.minimizeSign.Size = new System.Drawing.Size(20, 20);
            this.minimizeSign.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.minimizeSign.TabIndex = 19;
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
            this.exitSign.Location = new System.Drawing.Point(401, 4);
            this.exitSign.Name = "exitSign";
            this.exitSign.Size = new System.Drawing.Size(20, 20);
            this.exitSign.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.exitSign.TabIndex = 18;
            this.exitSign.TabStop = false;
            this.exitSign.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ExitSign_MouseClick);
            this.exitSign.MouseEnter += new System.EventHandler(this.ExitSign_MouseEnter);
            this.exitSign.MouseLeave += new System.EventHandler(this.ExitSign_MouseLeave);
            this.exitSign.MouseHover += new System.EventHandler(this.ExitSign_MouseHover);
            // 
            // anticoIconInTitlePanel
            // 
            this.anticoIconInTitlePanel.BackgroundImage = global::antico.Properties.Resources.app_icon;
            this.anticoIconInTitlePanel.Image = global::antico.Properties.Resources.app_icon;
            this.anticoIconInTitlePanel.Location = new System.Drawing.Point(10, 4);
            this.anticoIconInTitlePanel.Name = "anticoIconInTitlePanel";
            this.anticoIconInTitlePanel.Size = new System.Drawing.Size(20, 20);
            this.anticoIconInTitlePanel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.anticoIconInTitlePanel.TabIndex = 3;
            this.anticoIconInTitlePanel.TabStop = false;
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("Source Sans Pro Semibold", 9.749999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleLabel.Location = new System.Drawing.Point(36, 6);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(151, 17);
            this.titleLabel.TabIndex = 2;
            this.titleLabel.Text = "Title of CustomDialogBox";
            // 
            // okCancleButton
            // 
            this.okCancleButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.okCancleButton.Font = new System.Drawing.Font("Source Sans Pro", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.okCancleButton.Location = new System.Drawing.Point(296, 132);
            this.okCancleButton.Name = "okCancleButton";
            this.okCancleButton.Size = new System.Drawing.Size(101, 23);
            this.okCancleButton.TabIndex = 1;
            this.okCancleButton.Text = "OK/CANCLE";
            this.okCancleButton.UseVisualStyleBackColor = true;
            this.okCancleButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.OkCancleButton_MouseClick);
            this.okCancleButton.MouseEnter += new System.EventHandler(this.OkCancleButton_MouseEnter);
            this.okCancleButton.MouseLeave += new System.EventHandler(this.OkCancleButton_MouseLeave);
            // 
            // noOkButton
            // 
            this.noOkButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.noOkButton.Font = new System.Drawing.Font("Source Sans Pro", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.noOkButton.Location = new System.Drawing.Point(166, 132);
            this.noOkButton.Name = "noOkButton";
            this.noOkButton.Size = new System.Drawing.Size(101, 23);
            this.noOkButton.TabIndex = 2;
            this.noOkButton.Text = "NO/OK";
            this.noOkButton.UseVisualStyleBackColor = true;
            this.noOkButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.NoOkButton_MouseClick);
            this.noOkButton.MouseEnter += new System.EventHandler(this.NoOkButton_MouseEnter);
            this.noOkButton.MouseLeave += new System.EventHandler(this.NoOkButton_MouseLeave);
            // 
            // yesButton
            // 
            this.yesButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.yesButton.Font = new System.Drawing.Font("Source Sans Pro", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.yesButton.Location = new System.Drawing.Point(39, 132);
            this.yesButton.Name = "yesButton";
            this.yesButton.Size = new System.Drawing.Size(101, 23);
            this.yesButton.TabIndex = 3;
            this.yesButton.Text = "YES";
            this.yesButton.UseVisualStyleBackColor = true;
            this.yesButton.Visible = false;
            this.yesButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.YesButton_MouseClick);
            this.yesButton.MouseEnter += new System.EventHandler(this.YesButton_MouseEnter);
            this.yesButton.MouseLeave += new System.EventHandler(this.YesButton_MouseLeave);
            // 
            // saveOrVisualizeLabel
            // 
            this.saveOrVisualizeLabel.AutoSize = true;
            this.saveOrVisualizeLabel.Font = new System.Drawing.Font("Source Sans Pro", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.saveOrVisualizeLabel.Location = new System.Drawing.Point(97, 72);
            this.saveOrVisualizeLabel.Name = "saveOrVisualizeLabel";
            this.saveOrVisualizeLabel.Size = new System.Drawing.Size(80, 17);
            this.saveOrVisualizeLabel.TabIndex = 6;
            this.saveOrVisualizeLabel.Text = "Select model:";
            // 
            // modelListComboBox
            // 
            this.modelListComboBox.BackColor = System.Drawing.SystemColors.Control;
            this.modelListComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.modelListComboBox.FormattingEnabled = true;
            this.modelListComboBox.Location = new System.Drawing.Point(183, 71);
            this.modelListComboBox.Name = "modelListComboBox";
            this.modelListComboBox.Size = new System.Drawing.Size(214, 21);
            this.modelListComboBox.TabIndex = 7;
            this.modelListComboBox.SelectedIndexChanged += new System.EventHandler(this.ModelListComboBox_SelectedIndexChanged);
            // 
            // nameOfTheModelLabel
            // 
            this.nameOfTheModelLabel.AutoSize = true;
            this.nameOfTheModelLabel.Font = new System.Drawing.Font("Source Sans Pro", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nameOfTheModelLabel.Location = new System.Drawing.Point(97, 97);
            this.nameOfTheModelLabel.Name = "nameOfTheModelLabel";
            this.nameOfTheModelLabel.Size = new System.Drawing.Size(43, 17);
            this.nameOfTheModelLabel.TabIndex = 8;
            this.nameOfTheModelLabel.Text = "Name:";
            // 
            // nameTextBox
            // 
            this.nameTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.nameTextBox.Location = new System.Drawing.Point(145, 96);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(252, 20);
            this.nameTextBox.TabIndex = 9;
            // 
            // customDialogBoxIcon
            // 
            this.customDialogBoxIcon.Image = global::antico.Properties.Resources.error;
            this.customDialogBoxIcon.Location = new System.Drawing.Point(39, 68);
            this.customDialogBoxIcon.Name = "customDialogBoxIcon";
            this.customDialogBoxIcon.Size = new System.Drawing.Size(50, 50);
            this.customDialogBoxIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.customDialogBoxIcon.TabIndex = 5;
            this.customDialogBoxIcon.TabStop = false;
            // 
            // messageLabel
            // 
            this.messageLabel.BackColor = System.Drawing.SystemColors.Control;
            this.messageLabel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.messageLabel.Font = new System.Drawing.Font("Source Sans Pro", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.messageLabel.Location = new System.Drawing.Point(39, 42);
            this.messageLabel.Multiline = true;
            this.messageLabel.Name = "messageLabel";
            this.messageLabel.Size = new System.Drawing.Size(356, 20);
            this.messageLabel.TabIndex = 10;
            this.messageLabel.Text = "Message of the CustomDialogBox!";
            // 
            // CustomDialogBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(433, 183);
            this.ControlBox = false;
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.nameOfTheModelLabel);
            this.Controls.Add(this.modelListComboBox);
            this.Controls.Add(this.saveOrVisualizeLabel);
            this.Controls.Add(this.customDialogBoxIcon);
            this.Controls.Add(this.yesButton);
            this.Controls.Add(this.noOkButton);
            this.Controls.Add(this.okCancleButton);
            this.Controls.Add(this.titlePanel);
            this.Controls.Add(this.messageLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CustomDialogBox";
            this.Opacity = 0.98D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CustomDialogBox";
            this.titlePanel.ResumeLayout(false);
            this.titlePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.minimizeSign)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.exitSign)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.anticoIconInTitlePanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.customDialogBoxIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel titlePanel;
        private System.Windows.Forms.Button okCancleButton;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Button noOkButton;
        private System.Windows.Forms.Button yesButton;
        private System.Windows.Forms.PictureBox anticoIconInTitlePanel;
        private System.Windows.Forms.PictureBox customDialogBoxIcon;
        private System.Windows.Forms.Label saveOrVisualizeLabel;
        private System.Windows.Forms.ComboBox modelListComboBox;
        private System.Windows.Forms.Label nameOfTheModelLabel;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.PictureBox minimizeSign;
        public System.Windows.Forms.PictureBox exitSign;
        private System.Windows.Forms.TextBox messageLabel;
    }
}