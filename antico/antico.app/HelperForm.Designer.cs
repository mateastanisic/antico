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
    partial class HelperForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HelperForm));
            this.exitSign = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.exitSign)).BeginInit();
            this.SuspendLayout();
            // 
            // exitSign
            // 
            this.exitSign.BackColor = System.Drawing.Color.Transparent;
            this.exitSign.Image = ((System.Drawing.Image)(resources.GetObject("exitSign.Image")));
            this.exitSign.Location = new System.Drawing.Point(1171, 5);
            this.exitSign.Name = "exitSign";
            this.exitSign.Size = new System.Drawing.Size(19, 18);
            this.exitSign.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.exitSign.TabIndex = 7;
            this.exitSign.TabStop = false;
            this.exitSign.Click += new System.EventHandler(this.exitSign_Click);
            this.exitSign.MouseEnter += new System.EventHandler(this.exitSign_MouseEnter_1);
            this.exitSign.MouseLeave += new System.EventHandler(this.exitSign_MouseLeave_1);
            this.exitSign.MouseHover += new System.EventHandler(this.exitSign_MouseHover);
            // 
            // HelperForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1196, 848);
            this.Controls.Add(this.exitSign);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "HelperForm";
            this.Opacity = 0.98D;
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HelperForm";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.HelperForm_KeyUp);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HelperForm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.HelperForm_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.HelperForm_MouseUp);
            ((System.ComponentModel.ISupportInitialize)(this.exitSign)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
    }
}