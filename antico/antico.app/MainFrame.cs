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
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Text;

namespace antico
{
    #region MainFrame
    ///
    /// <summary>
    /// Main form - main page of the application.
    /// 
    /// Every MainForm has
    ///     - variables for moving the form (flag dragging, locations dragCursorPoint and dragCursorPoint)
    ///     - anticoLabelDesign (title)
    ///     - other forms (formForCreatingNewModel)
    /// 
    /// </summary>
    /// 
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

        #endregion

        #region OPERATIONS

        #region Initialize.
        /// <summary>
        /// Function for initializing components on Main frame.
        /// </summary>
        internal MainFrame()
        {
            this.InitializeComponent();

            // Load text font.
            PrivateFontCollection anticoFont = new PrivateFontCollection();
            anticoFont.AddFontFile("../../../../[FONTS]/UnicaOne-Regular.ttf");
            this.anticoLabel.Font = new Font(anticoFont.Families[0], 35, System.Drawing.FontStyle.Regular);
            this.anticoLabelDesign = this.anticoLabel;
            
        }
        #endregion

        #region Closing form when pressing Exit sign.
        /// <summary>
        /// Closing form when pressing Exit sign.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitPictureBox_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        #endregion

        #region Methods for enabling moving MainFrame on user screen.

        /// <summary>
        /// Flag the wariable dragging true since user pressed mouse button initiating beggining of moving frame.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainFrame_MouseDown(object sender, MouseEventArgs e)
        {
            this.dragging = true;
            this.dragCursorPoint = Cursor.Position;
            this.dragFormPoint = this.Location;
        }

        /// <summary>
        /// If user previously pressed mouse button (if dragging is true), change location of the Form.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainFrame_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.dragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(this.dragCursorPoint));
                this.Location = Point.Add(this.dragFormPoint, new Size(dif));
            }
        }

        /// <summary>
        /// Flag the wariable dragging false since user pressed mouse button initiating end of moving frame.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainFrame_MouseUp(object sender, MouseEventArgs e)
        {
            this.dragging = false;
        }

        #endregion

        #region HOVER

        #region Making hand cursor when hovering picture boxes that are behaving like buttons.

        #region exit sign
        /// <summary>
        /// Hovering start.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitPictureBox_MouseEnter(object sender, EventArgs e)
        {
            this.exitPictureBox.Cursor = Cursors.Hand;
        }

        /// <summary>
        /// Hovering end.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitPictureBox_MouseLeave(object sender, EventArgs e)
        {
            this.exitPictureBox.Cursor = Cursors.Default;
        }
        #endregion

        #region create model
        /// <summary>
        ///  Hovering start.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateNewModelSign_MouseEnter(object sender, EventArgs e)
        {
            this.createNewModelSign.Cursor = Cursors.Hand;
        }

        /// <summary>
        /// Hovering end.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateNewModelSign_MouseLeave(object sender, EventArgs e)
        {
            this.createNewModelSign.Cursor = Cursors.Default;
        }
        #endregion

        #region is this file malicious
        /// <summary>
        ///  Hovering start.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IsThisMaliciousSign_MouseEnter(object sender, EventArgs e)
        {
            this.isThisMaliciousSign.Cursor = Cursors.Hand;
        }

        /// <summary>
        /// Hovering end.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IsThisMaliciousSign_MouseLeave(object sender, EventArgs e)
        {
            this.isThisMaliciousSign.Cursor = Cursors.Default;
        }
        #endregion

        #region about antico
        /// <summary>
        ///  Hovering start.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AboutAnticoSign_MouseEnter(object sender, EventArgs e)
        {
            this.aboutAnticoSign.Cursor = Cursors.Hand;
        }

        /// <summary>
        /// Hovering end.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AboutAnticoSign_MouseLeave(object sender, EventArgs e)
        {
            this.aboutAnticoSign.Cursor = Cursors.Default;
        }
        #endregion

        #endregion

        #region Showing ToolTip when hovering picture boxes that are behaving like buttons.

        #region create model
        /// <summary>
        /// Show what createNewModelSign picture box represents.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateNewModelSign_MouseHover(object sender, EventArgs e)
        {
            ToolTip tt = new ToolTip();
            tt.SetToolTip(this.createNewModelSign, "create new model");
        }
        #endregion

        #region exit sign
        /// <summary>
        /// Show that pressing exitPictureBox means exiting the application.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitPictureBox_MouseHover(object sender, EventArgs e)
        {
            ToolTip tt = new ToolTip();
            tt.SetToolTip(this.exitPictureBox, "exit");
        }
        #endregion

        #region is this file malicious
        /// <summary>
        /// Show what isThisMaliciousSign picture box represents.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IsThisMaliciousSign_MouseHover(object sender, EventArgs e)
        {
            ToolTip tt = new ToolTip();
            tt.SetToolTip(this.isThisMaliciousSign, "check is your file malicious \nNOT YET IMPLEMENTED");
        }
        #endregion

        #region about antico
        /// <summary>
        /// Show what aboutAnticoSign picture box represents.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AboutAnticoSign_MouseHover(object sender, EventArgs e)
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
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateNewModelSign_MouseClick(object sender, MouseEventArgs e)
        {
            // Create form for model creation.
            this.formForCreatingNewModel = new CreateNewModelForm(this);

            // Hide current form.
            this.Visible = false;
            // Show new form.
            this.formForCreatingNewModel.Show();
        }
        #endregion

        #region is this file malicious sign
        /// <summary>
        /// Forward user to form where he can check if his file is malicious.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IsThisMaliciousSign_MouseClick(object sender, MouseEventArgs e)
        {
            string message = "Feature still not implemented!";
            MessageBox.Show(message, "antico responds", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        #endregion

        #region about antico sign
        /// <summary>
        /// Forward user to form where he can learn about antico project.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AboutAnticoSign_MouseClick(object sender, MouseEventArgs e)
        {
            string message = "Feature still not implemented!";
            MessageBox.Show(message, "antico responds", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        #endregion

        #endregion

        #endregion

    }
    #endregion
}
