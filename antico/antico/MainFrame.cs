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

namespace antico
{
    public partial class MainFrame : Form
    {
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        public MainFrame()
        {
            InitializeComponent();
        }

        private void MainFrame_Load(object sender, EventArgs e)
        {
            PrivateFontCollection anticoFont = new PrivateFontCollection();
            anticoFont.AddFontFile("D:/UnicaOne-Regular.ttf");
            anticoLabel.Font = new Font(anticoFont.Families[0], 35, FontStyle.Regular);
        }

        private void exitPictureBox_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void MainFrame_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = this.Location;
        }

        private void MainFrame_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(dif));
            }
        }

        private void MainFrame_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        private void exitPictureBox_MouseEnter(object sender, EventArgs e)
        {
            exitPictureBox.Cursor = Cursors.Hand;
        }

        private void exitPictureBox_MouseLeave(object sender, EventArgs e)
        {
            exitPictureBox.Cursor = Cursors.Default;
        }
    }
}
