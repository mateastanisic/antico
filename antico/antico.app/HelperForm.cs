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
    #region HelperForm
    /// 
    /// <summary>
    /// 
    /// Form for visualization of the model.
    /// 
    /// Every HelperForm has 
    ///     - variables for moving the form (flag dragging, locations dragCursorPoint and dragCursorPoint)
    ///     - exitSign pictureBox (needs to be part of a class so its location is properly put when changing size of the form)
    ///     
    /// 
    /// </summary>
    /// 
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
            this.InitializeComponent();
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
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitSign_Click( object sender, EventArgs e )
        {
            this.Close();
        }
        #endregion

        #endregion

        #region Methods for enabling moving HelperFrame on user screen.
        /// <summary>
        /// Flag the wariable dragging true since user pressed mouse button initiating beggining of moving frame.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HelperForm_MouseDown( object sender, MouseEventArgs e )
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
        private void HelperForm_MouseMove( object sender, MouseEventArgs e )
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
        private void HelperForm_MouseUp( object sender, MouseEventArgs e )
        {
            this.dragging = false;
        }
        #endregion

        #region HOVERING

        #region Making hand cursor when hovering over exit sign.
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
        #endregion

        #region Show ToolTip when hovering over exit sign.
        /// <summary>
        /// Show that pressing exitSign means closing the form.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitSign_MouseHover(object sender, EventArgs e)
        {
            ToolTip tt = new ToolTip();
            tt.SetToolTip(this.exitSign, "close");
        }
        #endregion

        #endregion

        #region Form Size changed.
        /// <summary>
        /// Change location of exit sign if form size is changed
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HelperForm_SizeChanged(object sender, EventArgs e)
        {
            this.exitSign.Location = new System.Drawing.Point(this.Location.X + this.Size.Width - 41, 6);
        }
        #endregion

        #endregion
    }
    #endregion
}
