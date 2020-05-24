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

namespace antico
{
    public partial class MainFrame : Form
    {
        /// <summary>
        /// Variables needed for allowing user to move app window on screen.
        /// </summary>
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        #region Initialize
        /// <summary>
        /// Function for initializing components on Main frame.
        /// </summary>
        public MainFrame()
        {
            InitializeComponent();

            // Load text font.
            PrivateFontCollection anticoFont = new PrivateFontCollection();
            anticoFont.AddFontFile("../../../../[FONTS]/UnicaOne-Regular.ttf");
            anticoLabel.Font = new Font(anticoFont.Families[0], 35, FontStyle.Regular);
        }
        #endregion

        #region Closing form when pressing Exit sign
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

        #region Functions for enabling moving MainFrame on user screen.

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

        #region Making hand cursor when hovering over exit sign.

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
    }
}
