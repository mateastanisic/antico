//////////////////////////////////////////////////////////////////////////////////////////
// antico --- artificial bee colony programming based malware detection                 //
// Copyright 2020 Matea Stanišić                                                        //
//                                                                                      //
//                                                                                      //
// Matea Stanišić                                                                       //
// mateastanisic@outlook.com                                                            //
// Zagreb, Hrvatska                                                                     //
//////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using antico.abcp;
using antico.data;
using Microsoft.Msagl.Drawing;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace antico
{
    public partial class MainFrame : Form
    {
        #region ATTRIBUTES

        #region Variables for moving the form.
        // Variables needed for allowing user to move app window on screen.
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;
        #endregion

        #region Variables for design.
        // Variable for antico label needed after comeback from CreateNewModelForm.
        internal System.Windows.Forms.Label anticoLabelDesign;
        #endregion

        #region Other forms.
        // Form for creating new model.
        private CreateNewModelForm formForCreatingNewModel;
        #endregion

        #region Dictionary for depth colors in symbolic tree structure.
        // Dictionary for colors.
        private Dictionary<int, string> depthColors = new Dictionary<int, string>()
        {
            { 0, "#8FD8D8" }, { 1, "#70DBDB" }, { 2, "#00CED1" }, { 3, "#39B7CD" },
            { 4, "#0099CC" }, { 5, "#33A1DE" }, { 6, "#42C0FB" }, { 7, "#87CEEB" },
            { 8, "#BFEFFF" }, { 9, "#9BC4E2" }, { 10, "#739AC5" }, { 11, "#60AFFE" },
            { 12, "#1E90FF" }, { 13, "#0276FD" }, { 14, "#436EEE" }, { 15, "#3333FF" }
        };
        #endregion

        #endregion

        #region OPERATIONS

        #region Initialize.
        /// <summary>
        /// Function for initializing components on Main frame.
        /// </summary>
        internal MainFrame()
        {
            InitializeComponent();

            // Load text font.
            PrivateFontCollection anticoFont = new PrivateFontCollection();
            anticoFont.AddFontFile("../../../../[FONTS]/UnicaOne-Regular.ttf");
            anticoLabel.Font = new Font(anticoFont.Families[0], 35, System.Drawing.FontStyle.Regular);
            anticoLabelDesign = anticoLabel;
            
        }
        #endregion

        #region Closing form when pressing Exit sign.
        /// <summary>
        /// Closing form when pressing Exit sign.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitPictureBox_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        #endregion

        #region Methods for enabling moving MainFrame on user screen.

        /// <summary>
        /// Flag the wariable dragging true since user pressed mouse button initiating beggining of moving frame.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainFrame_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = this.Location;
        }

        /// <summary>
        /// If user previously pressed mouse button (if dragging is true), change location of the Form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainFrame_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(dif));
            }
        }

        /// <summary>
        /// Flag the wariable dragging false since user pressed mouse button initiating end of moving frame.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainFrame_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        #endregion

        #region HOVER

        #region Making hand cursor when hovering picture boxes that are behaving like buttons.

        #region exit sign
        /// <summary>
        /// Hovering start.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitPictureBox_MouseEnter(object sender, EventArgs e)
        {
            exitPictureBox.Cursor = Cursors.Hand;
        }

        /// <summary>
        /// Hovering end.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitPictureBox_MouseLeave(object sender, EventArgs e)
        {
            exitPictureBox.Cursor = Cursors.Default;
        }
        #endregion

        #region create model
        /// <summary>
        ///  Hovering start.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void createNewModelSign_MouseEnter(object sender, EventArgs e)
        {
            createNewModelSign.Cursor = Cursors.Hand;
        }

        /// <summary>
        /// Hovering end.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void createNewModelSign_MouseLeave(object sender, EventArgs e)
        {
            createNewModelSign.Cursor = Cursors.Default;
        }
        #endregion

        #region is this file malicious
        /// <summary>
        ///  Hovering start.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void isThisMaliciousSign_MouseEnter(object sender, EventArgs e)
        {
            isThisMaliciousSign.Cursor = Cursors.Hand;
        }

        /// <summary>
        /// Hovering end.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void isThisMaliciousSign_MouseLeave(object sender, EventArgs e)
        {
            isThisMaliciousSign.Cursor = Cursors.Default;
        }
        #endregion

        #region about antico
        /// <summary>
        ///  Hovering start.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutAnticoSign_MouseEnter(object sender, EventArgs e)
        {
            aboutAnticoSign.Cursor = Cursors.Hand;
        }

        /// <summary>
        /// Hovering end.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutAnticoSign_MouseLeave(object sender, EventArgs e)
        {
            aboutAnticoSign.Cursor = Cursors.Default;
        }
        #endregion

        #endregion

        #region Showing ToolTip when hovering picture boxes that are behaving like buttons.

        #region create model
        /// <summary>
        /// Show what createNewModelSign picture box represents.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void createNewModelSign_MouseHover(object sender, EventArgs e)
        {
            ToolTip tt = new ToolTip();
            tt.SetToolTip(this.createNewModelSign, "create new model");
        }
        #endregion

        #region exit sign
        /// <summary>
        /// Show that pressing exitPictureBox means exiting the application.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitPictureBox_MouseHover(object sender, EventArgs e)
        {
            ToolTip tt = new ToolTip();
            tt.SetToolTip(this.exitPictureBox, "exit");
        }
        #endregion

        #region is this file malicious
        /// <summary>
        /// Show what isThisMaliciousSign picture box represents.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void isThisMaliciousSign_MouseHover(object sender, EventArgs e)
        {
            ToolTip tt = new ToolTip();
            tt.SetToolTip(this.isThisMaliciousSign, "check is your file malicious \nNOT YET IMPLEMENTED");
        }
        #endregion

        #region about antico
        /// <summary>
        /// Show what aboutAnticoSign picture box represents.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutAnticoSign_MouseHover(object sender, EventArgs e)
        {
            ToolTip tt = new ToolTip();
            tt.SetToolTip(this.aboutAnticoSign, "about antico \nNOT YET IMPLEMENTED");
        }
        #endregion

        #endregion

        #endregion

        #region CLICK

        #region new model sign
        /// <summary>
        /// Forward user to createNewModel form when clicking on createNewModelSign.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void createNewModelSign_MouseClick(object sender, MouseEventArgs e)
        {
            // Create form for model creation.
            formForCreatingNewModel = new CreateNewModelForm(this);

            // Hide current form.
            this.Visible = false;
            // Show new form.
            formForCreatingNewModel.Show();
        }
        #endregion

        #region is this file malicious sign
        /// <summary>
        /// Forward user to form where he can check if his file is malicious.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void isThisMaliciousSign_MouseClick(object sender, MouseEventArgs e)
        {
            string message = "Feature still not implemented!";
            string title = "antico responds";
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Error);
        }
        #endregion

        #region about antico sign
        /// <summary>
        /// Forward user to form where he can learn about antico project.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutAnticoSign_MouseClick(object sender, MouseEventArgs e)
        {
            string message = "Feature still not implemented!";
            string title = "antico responds";
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Error);
        }
        #endregion

        #endregion

        #endregion

    }
}
