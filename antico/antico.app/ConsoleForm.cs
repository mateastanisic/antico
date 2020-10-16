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

namespace antico
{
    #region ConsoleForm
    /// 
    /// <summary>
    /// Form for printouts during the search for the model.
    /// 
    /// Every ConsoleForm has
    ///     - printoutTextBox (label for printing out the current status of search - needs to be a part of class so it text can be added from other classes)
    ///     - variables for moving the form (flag dragging, locations dragCursorPoint and dragCursorPoint)
    ///     
    /// </summary>
    /// 
    public partial class ConsoleForm : Form
    {
        #region ATTRIBUTES

        #region Printout text box.
        // Variable for printing out the current status of search.
        public TextBox printoutTextBox;
        #endregion

        #region Variables for moving the form.
        // Variables needed for allowing user to move app window on screen.
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;
        #endregion

        #endregion

        #region OPERATIONS

        #region Initialize.
        /// <summary>
        /// Initialization.
        /// </summary>
        public ConsoleForm()
        {
            this.InitializeComponent();
            this.printoutTextBox = new TextBox();
            this.printoutTextBox = this.heuristicPrintoutTextBox;
        }

        /// <summary>
        /// Set up loacation of the form.
        /// </summary>
        /// 
        /// <param name="newModelForm"></param>
        public void CreateNewConsoleForm(CreateNewModelForm newModelForm) 
        {
            Application.DoEvents();
            // Set location of the frame.
            this.Location = newModelForm.DesktopLocation;
        }
        #endregion

        #region Closing form.

        #region Esc key.
        /// <summary>
        /// Frame is closed when pressed Esc key.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConsoleForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Visible = false;
            }
        }
        #endregion

        #region Exit sign.
        /// <summary>
        /// Closing form when pressing Exit sign.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitSign_MouseClick(object sender, EventArgs e)
        {
            this.Visible = false;
        }
        #endregion

        #endregion

        #region Methods for enabling moving ConsoleForm on user screen.
        /// <summary>
        /// Flag the wariable dragging true since user pressed mouse button initiating beggining of moving frame.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConsoleForm_MouseDown(object sender, MouseEventArgs e)
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
        private void ConsoleForm_MouseMove(object sender, MouseEventArgs e)
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
        private void ConsoleForm_MouseUp(object sender, MouseEventArgs e)
        {
            this.dragging = false;
        }

        /// <summary>
        /// Flag the wariable dragging true since user pressed mouse button initiating beggining of moving frame.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HeuristicPrintoutTextBox_MouseDown(object sender, MouseEventArgs e)
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
        private void HeuristicPrintoutTextBox_MouseMove(object sender, MouseEventArgs e)
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
        private void HeuristicPrintoutTextBox_MouseUp(object sender, MouseEventArgs e)
        {
            this.dragging = false;
        }
        #endregion

        #region HOVERING
        /// <summary>
        /// Show that pressing ExitSign means exiting the form.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitSign_MouseHover(object sender, EventArgs e)
        {
            ToolTip tt = new ToolTip();
            tt.SetToolTip(this.exitSign, "exit");
        }

        /// <summary>
        /// Hovering start.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitSign_MouseEnter(object sender, EventArgs e)
        {
            this.exitSign.Cursor = Cursors.Hand;
        }

        /// <summary>
        /// Hovering end.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitSign_MouseLeave(object sender, EventArgs e)
        {
            this.exitSign.Cursor = Cursors.Default;
        }

        /// <summary>
        /// Hovering start.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HeuristicPrintoutTextBox_MouseEnter(object sender, EventArgs e)
        {
            this.heuristicPrintoutTextBox.Cursor = Cursors.Hand;
        }

        /// <summary>
        /// Hovering end.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HeuristicPrintoutTextBox_MouseLeave(object sender, EventArgs e)
        {
            this.heuristicPrintoutTextBox.Cursor = Cursors.Default;
        }
        #endregion

        #endregion
    }
    #endregion
}
