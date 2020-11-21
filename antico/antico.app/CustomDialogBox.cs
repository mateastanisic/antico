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
using System.Drawing;
using System.Windows.Forms;

namespace antico
{
    /// 
    /// <summary>
    /// CustomDialogBox Form is class for creating dialog frames of custom kinds.
    /// 
    /// Every CustomDialogBox has 
    ///     - variables for moving the form (flag dragging, locations dragCursorPoint and dragCursorPoint)
    ///     - version (represents version of custom dialog box)
    ///     - results (which button is clicked, index of the picked solution and name of the file to be)
    ///     
    /// </summary>
    /// 
    public partial class CustomDialogBox : Form
    {
        #region ATTRIBUTES

        #region Variables for moving the form.
        // Variables needed for allowing user to move app window on screen.
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;
        #endregion

        #region version
        // Variable that represents version of the Custom Dialog Box. 
        //  1 - basic (YES/NO/OK/CANCEL)
        //  2 - VISUALIZE
        //  3 - SAVE
        private int customVersion;
        #endregion

        #region results
        // Variabls for keeping track of result (after clicking some button).

        // To know which button is clicked.
        public string buttonClicked;

        // To know which item in comboBox of solution is picked.
        public int pickedSolutionIndex;

        // To know the name of the file to be.
        public string pickedName;
        #endregion

        #endregion

        #region OPERATIONS

        #region Initialize.
        /// <summary>
        /// Besic initialization.
        /// </summary>
        public CustomDialogBox()
        {
            InitializeComponent();
        }

        /// <summary>
        /// "Basic" version of custom dialog. - OK/YES/NO/CANCEL buttons.
        /// </summary>
        /// 
        /// <param name="title">Title of the dialog box.</param>
        /// <param name="message">Message of the dialog box.</param>
        /// <param name="icon">Icon image to be shown.</param>
        /// <param name="button">Buttons to be shown.</param>
        public CustomDialogBox(string title, string message, Image icon, MessageBoxButtons button)
        {
            InitializeComponent();

            // Set up custom version.
            this.customVersion = 1;

            // Hide unnecessary controls.
            this.saveOrVisualizeLabel.Visible = false;
            this.modelListComboBox.Visible = false;
            this.nameOfTheModelLabel.Visible = false;
            this.nameTextBox.Visible = false;

            // Set up title.
            this.titleLabel.Text = title;

            // Set up message.
            this.messageLabel.Text = message;
            this.messageLabel.Location = new Point(102,75);
            this.messageLabel.Size = new Size(293, 80);

            #region dialog when informing user that file is saved on specific location
            // TODO: hardcoded title
            if (title == "Saved")
            {
                this.messageLabel.Location = new Point(102, 58);
            }
            #endregion

            #region icon
            // Set up icon.
            this.customDialogBoxIcon.Image = icon;
            this.customDialogBoxIcon.Location = new Point(39,58);

            // Play sound.
            if (icon == global::antico.Properties.Resources.stop)
            {
                System.Media.SystemSounds.Hand.Play();
            }

            if (icon == global::antico.Properties.Resources.warning || icon == global::antico.Properties.Resources.error)
            {
                System.Media.SystemSounds.Beep.Play();
            }
            #endregion

            #region buttons
            // Make needed buttons visible/not visible based on parameter button.
            if (button == MessageBoxButtons.YesNoCancel)
            {
                // Set up YES button.
                this.yesButton.Visible = true;
                this.yesButton.Text = "YES";

                // Set up NO button.
                this.noOkButton.Visible = true;
                this.noOkButton.Text = "NO";

                // Set up CANCEL button.
                this.okCancleButton.Visible = true;
                this.okCancleButton.Text = "CANCEL";
            }
            else if (button == MessageBoxButtons.OKCancel)
            {
                // Hide YES button.
                this.yesButton.Visible = false;

                // Set up OK button.
                this.noOkButton.Visible = true;
                this.noOkButton.Text = "OK";

                // Set up CANCEL button.
                this.okCancleButton.Visible = true;
                this.okCancleButton.Text = "CANCEL";
            }
            else if (button == MessageBoxButtons.OK)
            {
                // Hide YES button.
                this.yesButton.Visible = false;

                // Set up OK button.
                this.noOkButton.Visible = false;

                // Set up CANCEL button.
                this.okCancleButton.Visible = true;
                this.okCancleButton.Text = "OK";
            }
            #endregion
        }

        /// <summary>
        /// "Advanced" version of custom dialog box. - SAVE/VISUALIZE MODEL 
        /// </summary>
        /// 
        /// <param name="title">Title of the dialog box.</param>
        /// <param name="message">Message of the dialog box.</param>
        /// <param name="version">String that represents version of the dialog box. ("save"/"visualize")</param>
        /// <param name="icon">Icon image to be shown.</param>
        /// <param name="comboBoxItems">List of strings that represent combo box names.</param>
        /// <param name="defaultName">String for default name of file to be saved.</param>
        public CustomDialogBox(string title, string message, string version, Image icon, List<string> comboBoxItems, string defaultName)
        {
            InitializeComponent();

            // Check that comboBoxItems is not empty.
            if (comboBoxItems.Count == 0)
                throw new Exception("[CustomDialogBox advanced constructor] ComboBoxItems is not set.");

            // Set up title.
            this.titleLabel.Text = title;

            if (version == "visualize")
            {
                // Set up custom version.
                this.customVersion = 2;

                // Hide unnecessary controls.
                this.nameOfTheModelLabel.Visible = false;
                this.nameTextBox.Visible = false;

                #region message
                // Set up message.
                this.messageLabel.Text = message;
                this.messageLabel.Location = new Point(41, 41);
                this.messageLabel.Size = new Size(358, 20);
                #endregion

                #region icon
                // Set up icon.
                this.customDialogBoxIcon.Image = icon;
                this.customDialogBoxIcon.Location = new Point(39, 68);
                #endregion

                #region buttons
                // Hide YES button.
                this.yesButton.Visible = false;

                // Set up VISUALIZE button.
                this.noOkButton.Visible = true;
                this.noOkButton.Text = "VISUALIZE";

                // Set up CANCEL button.
                this.okCancleButton.Visible = true;
                this.okCancleButton.Text = "CANCEL";
                #endregion

                #region model selection
                // Show and set up label.
                this.saveOrVisualizeLabel.Visible = true;
                this.saveOrVisualizeLabel.Location = new Point(97, 72);

                // Show and set up comboBox.
                this.modelListComboBox.Visible = true;
                this.modelListComboBox.Location = new Point(97, 92);
                this.modelListComboBox.Size = new Size(294, 21);
                this.modelListComboBox.Items.Clear();

                // Add items.
                for (var i = 0; i < comboBoxItems.Count; i++)
                {
                    this.modelListComboBox.Items.Add(comboBoxItems[i]);
                }

                this.modelListComboBox.SelectedIndex = 0;
                #endregion
            }
            else if (version == "save")
            {
                // Set up custom version.
                this.customVersion = 2;

                #region message
                // Set up message.
                this.messageLabel.Text = message;
                this.messageLabel.Location = new Point(41, 41);
                #endregion

                #region icon
                // Set up icon.
                this.customDialogBoxIcon.Image = icon;
                this.customDialogBoxIcon.Location = new Point(39, 68);
                #endregion

                #region buttons
                // Hide YES button.
                this.yesButton.Visible = false;

                // Set up VISUALIZE button.
                this.noOkButton.Visible = true;
                this.noOkButton.Text = "SAVE";

                // Set up CANCEL button.
                this.okCancleButton.Visible = true;
                this.okCancleButton.Text = "CANCEL";
                #endregion

                #region model selection
                // Show and set up label.
                this.saveOrVisualizeLabel.Visible = true;
                this.saveOrVisualizeLabel.Location = new Point(97, 72);

                // Show and set up comboBox.
                this.modelListComboBox.Visible = true;
                this.modelListComboBox.Location = new Point(181, 71);
                this.modelListComboBox.Size = new Size(216, 21);
                this.modelListComboBox.Items.Clear();

                // Add items.
                for (var i = 0; i < comboBoxItems.Count; i++)
                {
                    this.modelListComboBox.Items.Add(comboBoxItems[i]);
                }

                this.modelListComboBox.SelectedIndex = 0;
                #endregion

                #region name of the file
                // Show and set up label.
                this.nameOfTheModelLabel.Visible = true;
                this.nameOfTheModelLabel.Location = new Point(97, 97);

                // Show and set up default name of the file.
                this.nameTextBox.Visible = true;
                this.nameTextBox.Location = new Point(145, 96);
                this.nameTextBox.Text = defaultName;
                #endregion
            }
            else
            {
                throw new Exception("[CustomDialogBox advance constructor] Version '" + version + "' is not supported.");
            }
        }
        #endregion

        #region Methods for enabling moving ConsoleForm on user screen.

        /// <summary>
        /// Flag the wariable dragging false since user pressed mouse button initiating end of moving frame.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TitlePanel_MouseUp(object sender, MouseEventArgs e)
        {
            this.dragging = false;
        }

        /// <summary>
        /// Flag the wariable dragging true since user pressed mouse button initiating beggining of moving frame.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TitlePanel_MouseDown(object sender, MouseEventArgs e)
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
        private void TitlePanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.dragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(this.dragCursorPoint));
                this.Location = Point.Add(this.dragFormPoint, new Size(dif));
            }
        }

        #endregion

        #region HOVERING

        #region exit
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
        #endregion

        #region title panel
        /// <summary>
        /// Hovering start.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TitlePanel_MouseEnter(object sender, EventArgs e)
        {
            this.titlePanel.Cursor = Cursors.Hand;
        }

        /// <summary>
        /// Hovering end.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TitlePanel_MouseLeave(object sender, EventArgs e)
        {
            this.titlePanel.Cursor = Cursors.Default;
        }
        #endregion

        #region minimize
        /// <summary>
        /// Hovering start.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MinimizeSign_MouseEnter(object sender, EventArgs e)
        {
            this.minimizeSign.Cursor = Cursors.Hand;
        }

        /// <summary>
        /// Hovering end.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MinimizeSign_MouseLeave(object sender, EventArgs e)
        {
            this.minimizeSign.Cursor = Cursors.Default;
        }

        /// <summary>
        /// Show that pressing minimizingSign means minimizing the form.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MinimizeSign_MouseHover(object sender, EventArgs e)
        {
            ToolTip tt = new ToolTip();
            tt.SetToolTip(this.minimizeSign, "minimize");
        }
        #endregion

        #region yes button
        /// <summary>
        /// Hovering start.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void YesButton_MouseEnter(object sender, EventArgs e)
        {
            if (this.yesButton.Visible)
            {
                this.yesButton.Cursor = Cursors.Hand;
            }
        }

        /// <summary>
        /// Hovering end.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void YesButton_MouseLeave(object sender, EventArgs e)
        {
            this.yesButton.Cursor = Cursors.Default;
        }
        #endregion

        #region NO / OK / SAVE /VISUALIZE
        /// <summary>
        /// Hovering start.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NoOkButton_MouseEnter(object sender, EventArgs e)
        {
            if (this.noOkButton.Visible)
            {
                this.noOkButton.Cursor = Cursors.Hand;
            }
        }

        /// <summary>
        /// Hovering end.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NoOkButton_MouseLeave(object sender, EventArgs e)
        {
            this.noOkButton.Cursor = Cursors.Default;
        }
        #endregion

        #region OK / CANCEL
        /// <summary>
        /// Hovering start.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OkCancleButton_MouseEnter(object sender, EventArgs e)
        {
            if (okCancleButton.Visible)
            {
                this.okCancleButton.Cursor = Cursors.Hand;
            }
        }

        /// <summary>
        /// Hovering end.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OkCancleButton_MouseLeave(object sender, EventArgs e)
        {
            this.okCancleButton.Cursor = Cursors.Default;

        }
        #endregion

        #endregion

        #region CLICKING

        #region exit

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
                // Save information about the abort.
                this.buttonClicked = "ABORT";

                // Save dialog result.
                this.DialogResult = DialogResult.Abort;

                // Close the form.
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
        private void ExitSign_MouseClick(object sender, EventArgs e)
        {
            // Save information about the abort.
            this.buttonClicked = "ABORT";

            // Save dialog result.
            this.DialogResult = DialogResult.Abort;

            // Close the form.
            this.Close();
        }
        #endregion

        #endregion

        #region minimize
        /// <summary>
        /// Minimizing form when pressing minimize sign.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MinimizeSign_MouseClick(object sender, MouseEventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        #endregion

        #region YES
        /// <summary>
        /// Yes button is clicked. Save that and close the form.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void YesButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.yesButton.Visible)
            {
                // Version should be 1.
                if (this.customVersion != 1)
                    throw new Exception("[YesButton_MouseClick] Version (" + customVersion + ") should be 1!");

                // Save dialog result.
                this.DialogResult = DialogResult.Yes;

                // Close form.
                this.Close();
            }
        }
        #endregion

        #region NO / OK / SAVE / VISUALIZE
        /// <summary>
        ///  NO / OK / SAVE / VISUALIZE button is clicked. Save that and close the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NoOkButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.customVersion == 1)
            {
                // This button must be NO or OK.
                if (this.noOkButton.Text != "NO" && this.noOkButton.Text != "OK")
                    throw new Exception("[] Version is 1, but button is " + this.noOkButton.Text + "!");

                if (this.noOkButton.Text == "NO")
                {
                    // Save dialog result.
                    this.DialogResult = DialogResult.No;
                }
                else if (this.noOkButton.Text == "OK")
                {
                    // Save dialog result.
                    this.DialogResult = DialogResult.OK;
                }

                // Close the form.
                this.Close();
            }
            else
            {
                // This button must be VISUALIZE or SAVE.
                if (this.noOkButton.Text != "SAVE" && this.noOkButton.Text != "VISUALIZE")
                    throw new Exception("[] Version is " + customVersion + ", but button is " + this.noOkButton.Text + "!");

                // Save dialog result.
                this.DialogResult = DialogResult.Yes;

                // Save information about the picked solution.
                this.pickedSolutionIndex = this.modelListComboBox.SelectedIndex;

                // Save information about the picked name.
                if (this.nameTextBox.Visible)
                {
                    this.pickedName = this.nameTextBox.Text;
                }

                // Close the form.
                this.Close();
            }
        }
        #endregion

        #region OK / CANCEL
        /// <summary>
        /// CANCEL or OK button is clicked. Save that and close the form.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OkCancleButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.okCancleButton.Text == "CANCEL")
            {
                // Save dialog result.
                this.DialogResult = DialogResult.Abort;

                // Close the form.
                this.Close();
            }
            else
            {
                // Save dialog result.
                this.DialogResult = DialogResult.OK;

                // Close the form.
                this.Close();
            }
        }

        #endregion

        #endregion

        #region Selected value changed.
        /// <summary>
        /// Change default name of the file to be saved if solution from combo box is changed.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ModelListComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.modelListComboBox.Visible && this.nameTextBox.Visible)
            {
                string cleanName = this.modelListComboBox.SelectedItem.ToString().Replace("(", "").Replace(")", "").Replace("{", "").Replace("}", "").Replace("[", "").Replace("]", "").Replace(" ","_");
                cleanName += "__" + DateTime.Today.Day.ToString() + "_" + DateTime.Today.Month.ToString() + "_" + DateTime.Today.Year.ToString();
                this.nameTextBox.Text = cleanName;
            }
        }
        #endregion

        #endregion
    }
}
