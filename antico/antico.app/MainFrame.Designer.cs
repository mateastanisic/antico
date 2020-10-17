//////////////////////////////////////////////////////////////////////////////////////////
// antico --- artificial bee colony programming based malware detection                 //
// Copyright 2020 Matea Stanišić                                                        //
//                                                                                      //
//                                                                                      //
// Matea Stanišić                                                                       //
// mateastanisic@outlook.com                                                            //
// Zagreb, Hrvatska                                                                     //
//////////////////////////////////////////////////////////////////////////////////////////

namespace antico
{
    partial class MainFrame
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainFrame));
            this.anticoLabel = new System.Windows.Forms.Label();
            this.exitPictureBox = new System.Windows.Forms.PictureBox();
            this.createNewModelSign = new System.Windows.Forms.PictureBox();
            this.isThisMaliciousSign = new System.Windows.Forms.PictureBox();
            this.aboutAnticoSign = new System.Windows.Forms.PictureBox();
            this.minimizeSign = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.exitPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.createNewModelSign)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.isThisMaliciousSign)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.aboutAnticoSign)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minimizeSign)).BeginInit();
            this.SuspendLayout();
            // 
            // anticoLabel
            // 
            this.anticoLabel.AutoSize = true;
            this.anticoLabel.BackColor = System.Drawing.Color.Transparent;
            this.anticoLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.anticoLabel.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.anticoLabel.Location = new System.Drawing.Point(432, 19);
            this.anticoLabel.Name = "anticoLabel";
            this.anticoLabel.Size = new System.Drawing.Size(93, 33);
            this.anticoLabel.TabIndex = 5;
            this.anticoLabel.Text = "antico";
            // 
            // exitPictureBox
            // 
            this.exitPictureBox.BackColor = System.Drawing.Color.Transparent;
            this.exitPictureBox.Image = global::antico.Properties.Resources.exit_white;
            this.exitPictureBox.Location = new System.Drawing.Point(952, 21);
            this.exitPictureBox.Name = "exitPictureBox";
            this.exitPictureBox.Size = new System.Drawing.Size(33, 31);
            this.exitPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.exitPictureBox.TabIndex = 6;
            this.exitPictureBox.TabStop = false;
            this.exitPictureBox.Click += new System.EventHandler(this.ExitPictureBox_Click);
            this.exitPictureBox.MouseEnter += new System.EventHandler(this.ExitPictureBox_MouseEnter);
            this.exitPictureBox.MouseLeave += new System.EventHandler(this.ExitPictureBox_MouseLeave);
            this.exitPictureBox.MouseHover += new System.EventHandler(this.ExitPictureBox_MouseHover);
            // 
            // createNewModelSign
            // 
            this.createNewModelSign.BackColor = System.Drawing.Color.Transparent;
            this.createNewModelSign.BackgroundImage = global::antico.Properties.Resources.create_new_white;
            this.createNewModelSign.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.createNewModelSign.Location = new System.Drawing.Point(120, 136);
            this.createNewModelSign.Name = "createNewModelSign";
            this.createNewModelSign.Size = new System.Drawing.Size(200, 200);
            this.createNewModelSign.TabIndex = 13;
            this.createNewModelSign.TabStop = false;
            this.createNewModelSign.MouseClick += new System.Windows.Forms.MouseEventHandler(this.CreateNewModelSign_MouseClick);
            this.createNewModelSign.MouseEnter += new System.EventHandler(this.CreateNewModelSign_MouseEnter);
            this.createNewModelSign.MouseLeave += new System.EventHandler(this.CreateNewModelSign_MouseLeave);
            this.createNewModelSign.MouseHover += new System.EventHandler(this.CreateNewModelSign_MouseHover);
            // 
            // isThisMaliciousSign
            // 
            this.isThisMaliciousSign.BackColor = System.Drawing.Color.Transparent;
            this.isThisMaliciousSign.BackgroundImage = global::antico.Properties.Resources.no_response_white;
            this.isThisMaliciousSign.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.isThisMaliciousSign.Location = new System.Drawing.Point(406, 136);
            this.isThisMaliciousSign.Name = "isThisMaliciousSign";
            this.isThisMaliciousSign.Size = new System.Drawing.Size(200, 200);
            this.isThisMaliciousSign.TabIndex = 14;
            this.isThisMaliciousSign.TabStop = false;
            this.isThisMaliciousSign.MouseClick += new System.Windows.Forms.MouseEventHandler(this.IsThisMaliciousSign_MouseClick);
            this.isThisMaliciousSign.MouseEnter += new System.EventHandler(this.IsThisMaliciousSign_MouseEnter);
            this.isThisMaliciousSign.MouseLeave += new System.EventHandler(this.IsThisMaliciousSign_MouseLeave);
            this.isThisMaliciousSign.MouseHover += new System.EventHandler(this.IsThisMaliciousSign_MouseHover);
            // 
            // aboutAnticoSign
            // 
            this.aboutAnticoSign.BackColor = System.Drawing.Color.Transparent;
            this.aboutAnticoSign.BackgroundImage = global::antico.Properties.Resources.calculating_save;
            this.aboutAnticoSign.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.aboutAnticoSign.Location = new System.Drawing.Point(692, 136);
            this.aboutAnticoSign.Name = "aboutAnticoSign";
            this.aboutAnticoSign.Size = new System.Drawing.Size(200, 200);
            this.aboutAnticoSign.TabIndex = 15;
            this.aboutAnticoSign.TabStop = false;
            this.aboutAnticoSign.MouseClick += new System.Windows.Forms.MouseEventHandler(this.AboutAnticoSign_MouseClick);
            this.aboutAnticoSign.MouseEnter += new System.EventHandler(this.AboutAnticoSign_MouseEnter);
            this.aboutAnticoSign.MouseLeave += new System.EventHandler(this.AboutAnticoSign_MouseLeave);
            this.aboutAnticoSign.MouseHover += new System.EventHandler(this.AboutAnticoSign_MouseHover);
            // 
            // minimizeSign
            // 
            this.minimizeSign.BackColor = System.Drawing.Color.Transparent;
            this.minimizeSign.Image = global::antico.Properties.Resources.minimizee;
            this.minimizeSign.Location = new System.Drawing.Point(905, 21);
            this.minimizeSign.Name = "minimizeSign";
            this.minimizeSign.Size = new System.Drawing.Size(33, 31);
            this.minimizeSign.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.minimizeSign.TabIndex = 16;
            this.minimizeSign.TabStop = false;
            this.minimizeSign.MouseClick += new System.Windows.Forms.MouseEventHandler(this.MinimizeSign_MouseClick);
            this.minimizeSign.MouseEnter += new System.EventHandler(this.MinimizeSign_MouseEnter);
            this.minimizeSign.MouseLeave += new System.EventHandler(this.MinimizeSign_MouseLeave);
            this.minimizeSign.MouseHover += new System.EventHandler(this.MinimizeSign_MouseHover);
            // 
            // MainFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::antico.Properties.Resources.images;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1012, 687);
            this.Controls.Add(this.exitPictureBox);
            this.Controls.Add(this.minimizeSign);
            this.Controls.Add(this.aboutAnticoSign);
            this.Controls.Add(this.isThisMaliciousSign);
            this.Controls.Add(this.createNewModelSign);
            this.Controls.Add(this.anticoLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainFrame";
            this.Opacity = 0.98D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "antico";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainFrame_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainFrame_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MainFrame_MouseUp);
            ((System.ComponentModel.ISupportInitialize)(this.exitPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.createNewModelSign)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.isThisMaliciousSign)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.aboutAnticoSign)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minimizeSign)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label anticoLabel;
        private System.Windows.Forms.PictureBox exitPictureBox;
        private System.Windows.Forms.PictureBox createNewModelSign;
        private System.Windows.Forms.PictureBox isThisMaliciousSign;
        private System.Windows.Forms.PictureBox aboutAnticoSign;
        private System.Windows.Forms.PictureBox minimizeSign;
    }
}

