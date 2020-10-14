using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace antico
{
    public partial class ConsoleForm : Form
    {
        #region ATTRIBUTES

        #region Printout text box.
        // Variable for printing out the current status of search.
        public TextBox printoutTestBox;
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
        public ConsoleForm()
        {
            InitializeComponent();
            printoutTestBox = new TextBox();
            printoutTestBox = heuristicPrintoutTextBox;
        }

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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitSign_MouseClick(object sender, EventArgs e)
        {
            this.Visible = false;
        }
        #endregion

        #endregion

        #region Methods for enabling moving ConsoleForm on user screen.
        /// <summary>
        /// Flag the wariable dragging true since user pressed mouse button initiating beggining of moving frame.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConsoleForm_MouseDown(object sender, MouseEventArgs e)
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
        private void ConsoleForm_MouseMove(object sender, MouseEventArgs e)
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
        private void ConsoleForm_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        /// <summary>
        /// Flag the wariable dragging true since user pressed mouse button initiating beggining of moving frame.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void heuristicPrintoutTextBox_MouseDown(object sender, MouseEventArgs e)
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
        private void heuristicPrintoutTextBox_MouseMove(object sender, MouseEventArgs e)
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
        private void heuristicPrintoutTextBox_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }
        #endregion

        #region HOVERING
        /// <summary>
        /// Show that pressing ExitSign means exiting the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitSign_MouseHover(object sender, EventArgs e)
        {
            ToolTip tt = new ToolTip();
            tt.SetToolTip(this.exitSign, "exit");
        }

        /// <summary>
        /// Hovering start.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitSign_MouseEnter(object sender, EventArgs e)
        {
            exitSign.Cursor = Cursors.Hand;
        }

        /// <summary>
        /// Hovering end.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitSign_MouseLeave(object sender, EventArgs e)
        {
            exitSign.Cursor = Cursors.Default;
        }

        /// <summary>
        /// Hovering start.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void heuristicPrintoutTextBox_MouseEnter(object sender, EventArgs e)
        {
            heuristicPrintoutTextBox.Cursor = Cursors.Hand;
        }

        /// <summary>
        /// Hovering end.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void heuristicPrintoutTextBox_MouseLeave(object sender, EventArgs e)
        {
            heuristicPrintoutTextBox.Cursor = Cursors.Default;
        }

        #endregion

        #endregion

    }
}
