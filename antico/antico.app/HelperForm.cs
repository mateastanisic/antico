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

namespace antico
{
    public partial class HelperForm : Form
    {
        #region ATTRIBUTES

        #region Variables for moving the form.
        // Variables needed for allowing user to move app window on screen.
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;
        #endregion

        #region Exit sign picture box.
        // Variable for exit sign picture box. Needs to be part of a class so its location is properly put when changing size of the form.
        public System.Windows.Forms.PictureBox exitSign;
        #endregion

        #endregion

        #region OPERATIONS

        #region Initialize.
        /// <summary>
        /// Function for initializing components on Helper frame.
        /// </summary>
        public HelperForm()
        {
            InitializeComponent();
        }
        #endregion

        #region Closing form.
        #region Esc key.
        /// <summary>
        /// Frame is closed when pressed Esc key.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HelperForm_KeyUp( object sender, KeyEventArgs e )
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
        #endregion

        #region Exit sign.
        /// <summary>
        /// Closing form when pressing Exit sign.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitSign_Click( object sender, EventArgs e )
        {
            this.Close();
        }
        #endregion

        #endregion

        #region Methods for enabling moving HelperFrame on user screen.
        /// <summary>
        /// Flag the wariable dragging true since user pressed mouse button initiating beggining of moving frame.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HelperForm_MouseDown( object sender, MouseEventArgs e )
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
        private void HelperForm_MouseMove( object sender, MouseEventArgs e )
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
        private void HelperForm_MouseUp( object sender, MouseEventArgs e )
        {
            dragging = false;
        }
        #endregion

        #region HOVERING

        #region Making hand cursor when hovering over exit sign.
        /// <summary>
        /// Hovering start.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitSign_MouseEnter_1( object sender, EventArgs e )
        {
            exitSign.Cursor = Cursors.Hand;
        }

        /// <summary>
        /// Hovering end.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitSign_MouseLeave_1( object sender, EventArgs e )
        {
            exitSign.Cursor = Cursors.Default;
        }
        #endregion

        #region Show ToolTip when hovering over exit sign.
        /// <summary>
        /// Show that pressing exitSign means closing the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitSign_MouseHover( object sender, EventArgs e )
        {
            ToolTip tt = new ToolTip();
            tt.SetToolTip(this.exitSign, "close");
        }
        #endregion

        #endregion

        #endregion

    }
}
