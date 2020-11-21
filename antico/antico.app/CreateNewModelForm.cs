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
using System.Drawing.Text;
using antico.abcp;
using antico.data;
using Microsoft.Msagl.Drawing;
using System.Threading;
using System.IO;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace antico
{
    #region CreateNewModelForm
    /// 
    /// <summary>
    /// 
    /// Form for creating new model.
    /// 
    /// Every CreateNewModelForm is represented by
    ///     - thread (for multitasking when preforming ABCP)
    ///     - depthColors (fixed dictionary of colors in tree)
    ///     - forbid (flag for controling clicking on some signs while preforming ABCP)
    ///     - consoleFlag (flag for keeping track of state for lookupConsoleFormSign)
    ///     - progressSign (flag for keeping track of state for showSolutionsProgressSign)
    ///     - other forms (main form,helper form for showing tree graph, console form for printing out while preforming ABCP)
    ///     - variables for moving the form (flag dragging, locations dragCursorPoint and dragCursorPoint)
    ///     - variables needed for preforming ABCP
    ///         - allModelsInForm (best models from each run) - reseting after every change of parameters/data and every new "click" on "start" sign
    ///         - bestModelInForm (best solution of all runs) - reseting after every change of parameters/data and every new "click" on "start" sign
    ///         - formParameters (class Parameters)
    ///         - formData (class Data)
    ///     - uploaded model class instance for saving data of model uploaded via file
    ///     
    /// </summary>
    /// 
    public partial class CreateNewModelForm : Form
    {
        #region ATTRIBUTES

        #region Variables for moving the form.
        // Variables needed for allowing user to move app window on screen.
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;
        #endregion

        #region ABCP variables.
        // Main variable for creating a new model.
        private List<ABCP> allModelsInForm;

        // Variable for saving current best model.
        private Chromosome bestModelInForm;

        // Variable for saving custom parameters.
        private Parameters formParameters;

        // Data variable.
        Data formData;
        #endregion

        #region Other forms.
        // Helper form for showing symbolic tree structure of a (best) model.
        private HelperForm formForVisualizationOfModel;
        // Main frame.
        private MainFrame mainForm;
        // Console form.
        private ConsoleForm consoleForm;
        // Progress form.
        public ProgressForm progressForm;
        #endregion

        #region Forbid clicking on form.
        // Variable that is used to forbid user clicking on signs.
        private bool forbid = false;
        #endregion

        #region List of choosen folds.
        // Variable to keep track of folds choosen to be representative of each run.
        List<int> choosenFold;
        #endregion

        #region Thread.
        // For multitasking while searching for the model.
        Thread myThread;
        private Object myLock = new Object();
        #endregion

        #region Dictionary for depth colors in symbolic tree structure.
        // Dictionary for colors.
        private Dictionary<int, string> depthColors = new Dictionary<int, string>()
        {
            { 0 , "#8FD8B4" }, { 1 , "#8FD8C0" }, { 2 , "#8FD8CC" }, { 3 , "#8FD8D8" }, 
            { 4 , "#70DBDB" }, { 5 , "#00CED1" }, { 6 , "#00BDD1" }, { 7 , "#00ACD1" },
            { 8 , "#009BD1" }, { 9 , "#008AD1" }, { 10 , "#0079D1" }, { 11, "#0056D1" },
            { 12 , "#0033D1" }, { 13 , "#0038E4" }, { 14 , "#3333FF" }, { 15 , "#3377FF" },
            { 16 , "#33AAFF" }, { 17 , "#33CCFF" }, { 18 , "#42C0FB" }, { 19 , "#60AFFE" },
            { 20 , "#60AFFE" }, { 21 , "#78B9FE" }, { 22 , "#87C5EE" }, { 23 , "#87CEEB" }, 
            { 24 , "#A9DCF1" }, { 25, "#BBDCE7" }, { 26 , "#BBE7E7" }, { 27 , "#BBE7DC" }, 
            { 28 , "#BBE7D1" }
        };
        #endregion

        #region Uploaded model.
        // Class that represents model uploaded from the file.
        private UploadedModel upload;

        // Flag to know if model was uploaded.
        private bool uploaded;
        #endregion

        #endregion

        #region OPERATIONS

        #region Initialize.
        /// <summary>
        /// Function for initializing components on frame.
        /// </summary>
        public CreateNewModelForm(MainFrame mainFrame)
        {
            InitializeComponent();
            this.DoubleBuffered = true;

            //Application.DoEvents();
            // Set location of the frame.
            this.Location = mainFrame.DesktopLocation;
            this.mainLayout.Parent = this;

            // Load text font.
            PrivateFontCollection anticoFont = new PrivateFontCollection();
            anticoFont.AddFontFile("../../../../[FONTS]/UnicaOne-Regular.ttf");
            anticoLabel.Font = new Font(anticoFont.Families[0], 35, System.Drawing.FontStyle.Regular);

            // Save sent variables.
            this.mainForm = mainFrame;
            this.allModelsInForm = new List<ABCP>();

            // Initialize the console form.
            this.consoleForm = new ConsoleForm();
            this.consoleForm.CreateNewConsoleForm(this);
            this.consoleForm.Visible = false;
            this.consoleForm.VisibleChanged += new EventHandler(this.ConsoleForm_VisibilityChanged);

            // Initialize the progress form.
            this.progressForm = new ProgressForm(mainForm.Location);
            this.progressForm.Visible = false;
            this.progressForm.VisibleChanged += new EventHandler(this.ProgressForm_VisibilityChanged);

            // Set flag to zero since no model was uploaded.
            this.uploaded = false;
        }

        #endregion

        #region Methods for enabling moving CreateNewModelForm on user screen.
        /// <summary>
        /// Flag the wariable dragging true since user pressed mouse button initiating beggining of moving frame.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateNewModelForm_MouseDown(object sender, MouseEventArgs e)
        {
            this.dragging = true;
            this.dragCursorPoint = Cursor.Position;
            this.dragFormPoint = this.Location;
        }

        /// <summary>
        /// Flag the wariable dragging true since user pressed mouse button initiating beggining of moving frame.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PanelForPrintoutLabel_MouseDown(object sender, MouseEventArgs e)
        {
            this.dragging = true;
            this.dragCursorPoint = Cursor.Position;
            this.dragFormPoint = this.Location;
        }

        /// <summary>
        /// Flag the wariable dragging true since user pressed mouse button initiating beggining of moving frame.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrintoutOfAllSolutionsLabel_MouseDown(object sender, MouseEventArgs e)
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
        private void CreateNewModelForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.dragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(this.dragCursorPoint));
                this.Location = Point.Add(this.dragFormPoint, new Size(dif));
            }
        }

        /// <summary>
        /// If user previously pressed mouse button (if dragging is true), change location of the Form.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PanelForPrintoutLabel_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.dragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(this.dragCursorPoint));
                this.Location = Point.Add(this.dragFormPoint, new Size(dif));
            }
        }

        /// <summary>
        /// If user previously pressed mouse button (if dragging is true), change location of the Form.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrintoutOfAllSolutionsLabel_MouseMove(object sender, MouseEventArgs e)
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
        private void CreateNewModelForm_MouseUp(object sender, MouseEventArgs e)
        {
            this.dragging = false;
        }

        /// <summary>
        /// Flag the wariable dragging false since user pressed mouse button initiating end of moving frame.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PanelForPrintoutLabel_MouseUp(object sender, MouseEventArgs e)
        {
            this.dragging = false;
        }

        /// <summary>
        /// Flag the wariable dragging false since user pressed mouse button initiating end of moving frame.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrintoutOfAllSolutionsLabel_MouseUp(object sender, MouseEventArgs e)
        {
            this.dragging = false;
        }
        #endregion

        #region Font AnticoLabel updated after comeback from minimized window.
        /// <summary>
        /// Updat font of Antico label after returning from minimized windows state.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateNewModelForm_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                // Load text font.
                PrivateFontCollection anticoFont = new PrivateFontCollection();
                anticoFont.AddFontFile("../../../../[FONTS]/UnicaOne-Regular.ttf");
                anticoLabel.Font = new Font(anticoFont.Families[0], 35, System.Drawing.FontStyle.Regular);
            }
        }
        #endregion

        #region HOVERING

        #region Making hand cursor when hovering sign.

        #region exit
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
        #endregion

        #region go back 
        /// <summary>
        /// Hovering start.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GoBackSign_MouseEnter(object sender, EventArgs e)
        {
            this.goBackSign.Cursor = Cursors.Hand;
        }

        /// <summary>
        /// Hovering end.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GoBackSign_MouseLeave(object sender, EventArgs e)
        {
            this.goBackSign.Cursor = Cursors.Default;
        }
        #endregion

        #region set parameters 
        /// <summary>
        /// Hovering start.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ParametersSettingsSign_MouseEnter(object sender, EventArgs e)
        {
            this.parametersSettingsSign.Cursor = Cursors.Hand;
        }

        /// <summary>
        /// Hovering end.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ParametersSettingsSign_MouseLeave(object sender, EventArgs e)
        {
            this.parametersSettingsSign.Cursor = Cursors.Hand;
        }
        #endregion

        #region start 
        /// <summary>
        /// Hovering start.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartSign_MouseEnter(object sender, EventArgs e)
        {
            this.startSign.Cursor = Cursors.Hand;
        }

        /// <summary>
        /// Hovering end.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartSign_MouseLeave(object sender, EventArgs e)
        {
            this.startSign.Cursor = Cursors.Default;
        }
        #endregion

        #region visualize 
        /// <summary>
        /// Hovering start if there model exists.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VisualizeSign_MouseEnter(object sender, EventArgs e)
        {
            if (this.visualizeSign.Visible)
            {
                this.visualizeSign.Cursor = Cursors.Hand;
            }
        }

        /// <summary>
        /// Hovering end.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VisualizeSign_MouseLeave(object sender, EventArgs e)
        {
            this.visualizeSign.Cursor = Cursors.Default;
        }
        #endregion

        #region save 
        /// <summary>
        /// Hovering start if there model exists.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveSign_MouseEnter(object sender, EventArgs e)
        {
            if (this.saveSign.Visible)
            {
                this.saveSign.Cursor = Cursors.Hand;
            }
        }

        /// <summary>
        /// Hovering end.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveSign_MouseLeave(object sender, EventArgs e)
        {
            this.saveSign.Cursor = Cursors.Default;
        }
        #endregion

        #region lookup console form
        /// <summary>
        /// Hovering start.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LookupConsoleFormSign_MouseEnter(object sender, EventArgs e)
        {
            // Show hand cursor only if search is ongoing or done. 
            // Also, check out if console is already opened.
            if (!this.consoleForm.Visible && (this.forbid || this.bestModelInForm != null || this.uploaded))
            {
                this.lookupConsoleFormSign.Cursor = Cursors.Hand;
            }
        }

        /// <summary>
        /// Hovering end.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LookupConsoleForSign_MouseLeave(object sender, EventArgs e)
        {
            this.lookupConsoleFormSign.Cursor = Cursors.Default;
        }
        #endregion

        #region upload
        /// <summary>
        /// Hovering start.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UploadSign_MouseEnter(object sender, EventArgs e)
        {
            this.uploadSign.Cursor = Cursors.Hand;
        }

        /// <summary>
        /// Hovering end.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UploadSign_MouseLeave(object sender, EventArgs e)
        {
            this.uploadSign.Cursor = Cursors.Default;
        }
        #endregion

        #region solutions progress
        /// <summary>
        /// Hovering start.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowSolutionsProgressSign_MouseEnter(object sender, EventArgs e)
        {
            // Show hand cursor only if search is ongoing or done. 
            // Also, check out if form is already opened.
            if (!this.progressForm.Visible && (this.forbid || this.bestModelInForm != null || this.uploaded))
            {
                this.showSolutionsProgressSign.Cursor = Cursors.Hand;
            }            
        }

        /// <summary>
        /// Hovering end.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowSolutionsProgressSign_MouseLeave(object sender, EventArgs e)
        {
            this.showSolutionsProgressSign.Cursor = Cursors.Default;
        }
        #endregion

        #endregion

        #region Showing ToolTip when hovering sign.

        #region go back 
        /// <summary>
        /// Show that pressing GoBack means going back to the main application.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GoBackSign_MouseHover(object sender, EventArgs e)
        {
            ToolTip tt = new ToolTip();
            tt.SetToolTip(this.goBackSign, "go back");
        }
        #endregion

        #region minimize 
        /// <summary>
        /// Show that pressing Minimize means minimizing the window.
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

        #region exit 
        /// <summary>
        /// Show that pressing exit means exiting the application.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitSign_MouseHover(object sender, EventArgs e)
        {
            ToolTip tt = new ToolTip();
            tt.SetToolTip(this.exitSign, "exit the application");
        }
        #endregion

        #region set parameters 
        /// <summary>
        /// Show that pressing parametersSettingsSign means showing text boxes for input of parameters.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ParametersSettingsSign_MouseHover(object sender, EventArgs e)
        {
            ToolTip tt = new ToolTip();
            tt.SetToolTip(this.parametersSettingsSign, "set custom parameters");
        }
        #endregion

        #region start 
        /// <summary>
        /// Show that pressing startSign means starting the search for the model.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartSign_MouseHover(object sender, EventArgs e)
        {
            ToolTip tt = new ToolTip();
            tt.SetToolTip(this.startSign, "start");
        }
        #endregion

        #region visualize 
        /// <summary>
        /// Show that pressing visualizeSign means showing tree structure of the best model.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VisualizeSign_MouseHover(object sender, EventArgs e)
        {
            if (this.visualizeSign.Visible)
            {
                ToolTip tt = new ToolTip();
                tt.SetToolTip(this.visualizeSign, "visualize best model");
            }
        }
        #endregion

        #region save 
        /// <summary>
        /// Show that pressing saveSign means saving the model.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveSign_MouseHover(object sender, EventArgs e)
        {
            if (this.saveSign.Visible)
            {
                ToolTip tt = new ToolTip();
                tt.SetToolTip(this.saveSign, "save");
            }
        }
        #endregion

        #region lookup console form 
        /// <summary>
        /// Show that pressing Lookup means showing printouts from the search.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LookupConsoleFormSign_MouseHover(object sender, EventArgs e)
        {
            // Show tool tip only if search is ongoing or done. 
            // Also, check out if console is already opened.
            if (this.consoleForm.Visible == false && (this.forbid || this.bestModelInForm != null || this.uploaded))
            {
                ToolTip tt = new ToolTip();
                tt.SetToolTip(this.lookupConsoleFormSign, "show console");
            }

        }
        #endregion

        #region upload 
        /// <summary>
        /// Show that pressing upload sign means uploading the solution from .dat file.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UploadSign_MouseHover(object sender, EventArgs e)
        {
            ToolTip tt = new ToolTip();
            tt.SetToolTip(this.uploadSign, "upload solution");
        }
        #endregion

        #region solutions progress 
        /// <summary>
        /// Show that pressing show solutions sign means showing progress of solutions over iterations.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowSolutionsProgressSign_MouseHover(object sender, EventArgs e)
        {
            // Show tool tip only if search is ongoing or done. 
            // Also, check out if form is already opened.
            if (this.progressForm.Visible == false && (this.forbid || this.bestModelInForm != null || this.uploaded))
            {
                ToolTip tt = new ToolTip();
                tt.SetToolTip(this.showSolutionsProgressSign, "show solution progress over iterations");
            }
        }
        #endregion

        #endregion

        #endregion

        #region CLICK

        #region exit
        /// <summary>
        /// Exit application. 
        /// Check with dialog with user if he is sure he wants to stop the search for solution if it is ongoing.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitSign_MouseClick(object sender, MouseEventArgs e)
        {
            #region check if another process is already running
            // If process is ongoing, check if user really wants to stop it.
            if (this.forbid)
            {
                string message = "You want to stop the search for the model and exit the application. Are you sure?";
                CustomDialogBox dialog = new CustomDialogBox("Exit?", message, global::antico.Properties.Resources.question, MessageBoxButtons.YesNoCancel);
                var result = dialog.ShowDialog();

                // If canceled, do nothing.
                if (result == DialogResult.No || result == DialogResult.Abort)
                    return;
            }
            #endregion

            // Abort the process if alive.
            if (this.myThread != null)
            {
                this.myThread.Abort();
            }

            // ProgressForm and ConsoleForm also should not exist.
            this.progressForm.shouldExist = false;
            this.consoleForm.shouldExist = false;

            // Exit.
            Application.Exit();
        }
        #endregion

        #region minimize
        /// <summary>
        /// Minimize the form.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MinimizeSign_MouseClick(object sender, MouseEventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

        }
        #endregion

        #region go back
        /// <summary>
        /// Closing form when pressing GoBack sign.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GoBackSign_MouseClick(object sender, EventArgs e)
        {
            #region check if another process is already running
            // If process is ongoing, check if user really wants to stop it.
            if (this.forbid)
            {
                string message = "You want to stop the search for the model. Are you sure?";
                CustomDialogBox dialog = new CustomDialogBox("Stop search?", message, global::antico.Properties.Resources.question, MessageBoxButtons.YesNoCancel);
                var result = dialog.ShowDialog();

                // If canceled, do nothing.
                if (result == DialogResult.No || result == DialogResult.Abort)
                    return;
            }
            #endregion

            // Abort the process if alive.
            if (this.myThread != null)
            {
                this.myThread.Abort();
            }

            // Close the console form.
            this.consoleForm.shouldExist = false;
            this.consoleForm.Close();

            // Close the progress form.
            this.progressForm.shouldExist = false;
            this.progressForm.Close();

            // Close the form.
            this.Close();

            // Update new location of main frame.
            this.mainForm.Location = this.Location;
            // Make main form visible again.
            this.mainForm.Visible = true;

            // Load text font.
            PrivateFontCollection anticoFont = new PrivateFontCollection();
            anticoFont.AddFontFile("../../../../[FONTS]/UnicaOne-Regular.ttf");
            this.mainForm.anticoLabelDesign.Font = new Font(anticoFont.Families[0], 35, System.Drawing.FontStyle.Regular);
        }
        #endregion

        #region set parameters
        /// <summary>
        /// Showing panel for inputing custom parameters.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ParametersSettingsSign_MouseClick(object sender, MouseEventArgs e)
        {
            #region check if another process is already running
            // Check if another process already runnning.
            if (this.forbid)
            {
                string m = "Wait! Model calculation is still in progress.";
                CustomDialogBox dialog = new CustomDialogBox("Warning!", m, global::antico.Properties.Resources.stop, MessageBoxButtons.OK);
                dialog.ShowDialog();
                
                return;
            }
            #endregion

            // Hide printoutOfAllSolutionsLabel and panelForPrintoutLabel if previously added to form.
            if (this.Controls.Contains(this.panelForPrintoutLabel))
            {
                this.panelForPrintoutLabel.Visible = false;
                this.printoutOfAllSolutionsLabel.Visible = false;
            }

            this.ShowParameters();
        }
        #endregion

        #region start 
        /// <summary>
        /// Creating the model. 
        /// Just in case, asks user if he wants to add custom parameters if already didn't.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartSign_MouseClick(object sender, MouseEventArgs e)
        {
            #region check if another process is already running
            // Check if another process already runnning.
            if (this.forbid)
            {
                string m = "Wait! Model calculation is still in progress.";
                CustomDialogBox dialog = new CustomDialogBox("Warning!", m, global::antico.Properties.Resources.stop, MessageBoxButtons.OK);
                dialog.ShowDialog();

                return;
            }
            #endregion

            // New search has started. Flag that no model is uploaded.
            this.uploaded = false;

            // Hide printoutOfAllSolutionsLabel and panelForPrintoutLabel if previously added to form.
            if (this.Controls.Contains(this.panelForPrintoutLabel))
            {
                this.panelForPrintoutLabel.Visible = false;
                this.printoutOfAllSolutionsLabel.Visible = false;
            }

            #region parameters set just now
            // Check if previously clicked on set parameters.
            if (this.mainLayout.Visible)
            {
                #region read parameters from the form
                // Read parameters from the input.
                int ps = Decimal.ToInt32(colonySizeUpDown.Value);
                int maxnoi = Decimal.ToInt32(maxNoOfIterUpDown.Value);
                int maxnonii = Decimal.ToInt32(maxNoOfNotImprovingIterUpDown.Value);
                int nooruns = Decimal.ToInt32(numberOfRunsUpDown.Value);
                int imd = Decimal.ToInt32(initialMaxDepthUpDown.Value);
                int md = Decimal.ToInt32(maxDepthUpDown.Value);
                int lim = Decimal.ToInt32(limitUpDown.Value);
                double a = Decimal.ToDouble(alphaUpDown.Value);
                double prob = Decimal.ToDouble(probabilityUpDown.Value);
                int nof = Decimal.ToInt32(numberOfFoldsUpDown.Value);
                string method = "ramped";

                if (this.growMethodRadioButton.Checked)
                {
                    method = "grow";
                }
                else if (this.fullMethodRadioButton.Checked)
                {
                    method = "full";
                }

                // Name of the database.
                string database;
                if (this.databaseComboBox.SelectedIndex > -1)
                {
                    database = this.databaseComboBox.SelectedItem.ToString().ToLower();
                }
                else
                {
                    CustomDialogBox dialog = new CustomDialogBox("Choose database!", "Please choose database!", global::antico.Properties.Resources.warning, MessageBoxButtons.OK);
                    dialog.ShowDialog();
                    return;
                }

                // Mathematical operators.
                List<string> mathOp = new List<string>();

                if (this.plusSelected.Checked)
                    mathOp.Add("+");
                if (this.minusSelected.Checked)
                    mathOp.Add("-");
                if (this.divisionSelected.Checked)
                    mathOp.Add("/");
                if (this.timesSelected.Checked)
                    mathOp.Add("*");
                if (this.sinSelected.Checked)
                    mathOp.Add("sin");
                if (this.cosSelected.Checked)
                    mathOp.Add("cos");
                if (this.rlogSelected.Checked)
                    mathOp.Add("rlog");
                if (this.expSelected.Checked)
                    mathOp.Add("exp");
                #endregion

                // Hide parameters layout.
                this.mainLayout.Visible = false;

                // Set up parameters.
                this.formParameters = new Parameters(ps, maxnoi, maxnonii, nooruns, imd, md, method, lim, a, prob);

                // Set up data.
                this.formData = new Data(mathOp, database, nof);

                // Do needed changes on form(s) before starting new search.
                this.BeforeWeStart();

                // User already added custom parameters.
                // Threaded search for the best solution.
                this.SearchForTheBestSolution();

                return;
            }
            #endregion

            #region parameters not set
            // Check if there are custom parameters.
            if (this.formParameters == null) 
            {
                string message = "Do you want add custom ABCP parameters?";
                string title = "Artificial bee colony programming parameters";
                CustomDialogBox dialog = new CustomDialogBox(title, message, global::antico.Properties.Resources.question, MessageBoxButtons.YesNoCancel);
                var result = dialog.ShowDialog();

                // If user wants to add custom parameters. 
                if (result == DialogResult.Yes)
                {
                    this.ShowParameters();
                }
                else if (result == DialogResult.No)
                {
                    // Do not add custom parameters. (TESTING MODE)
                    this.formParameters = new Parameters();
                    this.formData = new Data();

                    // Do needed changes on form(s) before starting new search.
                    this.BeforeWeStart();

                    // Threaded search for the best solution.
                    this.SearchForTheBestSolution();
                }
                return;
            }
            #endregion

            #region parameters set before
            // Hide parameters layout.
            this.mainLayout.Visible = false;

            // Check if form Data is set. (Since Parameters are set.)
            if (this.formData == null)
                throw new Exception("[StartSign_MouseClick] Form Data is not initialized.");

            // Do needed changes on form(s) before starting new search.
            this.BeforeWeStart();

            // User already added custom parameters.
            // Threaded search for the best solution.
            this.SearchForTheBestSolution();

            #endregion
        }
        #endregion

        #region visualize
        /// <summary>
        /// Show symbolic tree structure of best model if exists.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VisualizeSign_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.visualizeSign.Visible)
            {
                string m;

                #region check if another process is already running
                // Check if another process already runnning.
                if (this.forbid)
                {
                    m = "Wait! Model calculation is still in progress.";
                    CustomDialogBox dialog = new CustomDialogBox("Warning!", m, global::antico.Properties.Resources.stop, MessageBoxButtons.OK);
                    dialog.ShowDialog();

                    return;
                }
                #endregion

                // Check if model exists.
                if (this.allModelsInForm != null && this.bestModelInForm != null && !this.uploaded)
                {
                    #region select solution
                    // Choices for saving.
                    List<string> modelsList = new List<string>();

                    // Save indices of choices.
                    // First item of tuple is index of run and second one is index of solution in population.
                    List<Tuple<int, int>> modulsChoices = new List<Tuple<int, int>>();

                    double f = (double)((double)(this.bestModelInForm.trainFitness + this.bestModelInForm.testFitness) / 2);
                    f = Math.Round(f, 4);
                    modelsList.Add("{0}[BEST SOLUTION] " + f.ToString());

                    for (var r = 0; r < this.allModelsInForm.Count; r++)
                    {
                        // Best indices of the solutions in the model.
                        int indBest = this.allModelsInForm[r].bestIndex;
                        int indBestTrain = this.allModelsInForm[r].bestTrainIndex;
                        int indBestTest = this.allModelsInForm[r].bestTestIndex;

                        #region best train + test
                        double tempTrainFitness = this.allModelsInForm[r].population[indBest].trainFitness;
                        double tempTestFitness = this.allModelsInForm[r].population[indBest].testFitness;

                        // Best train + test.
                        f = (double)((double)(tempTrainFitness + tempTestFitness) / 2);
                        f = Math.Round(f, 4);
                        modelsList.Add("{" + (r * 3 + 1).ToString() + "} RUN[" + r + "] (best solution) " + f.ToString());

                        // Remember choice.
                        modulsChoices.Add(Tuple.Create(r,indBest));
                        #endregion

                        #region best train
                        if (indBestTrain != indBest)
                        {
                            tempTrainFitness = this.allModelsInForm[r].population[indBestTrain].trainFitness;
                            tempTestFitness = this.allModelsInForm[r].population[indBestTrain].testFitness;

                            f = (double)((double)(tempTrainFitness + tempTestFitness) / 2);
                            f = Math.Round(f, 4);
                            modelsList.Add("{" + (r * 3 + 2).ToString() + "} RUN[" + r + "] (best train solution) " + f.ToString());

                            // Remember choice.
                            modulsChoices.Add(Tuple.Create(r, indBestTrain));
                        }
                        #endregion

                        #region best test
                        if (indBestTest != indBest && indBestTest != indBestTrain)
                        {
                            tempTrainFitness = this.allModelsInForm[r].population[indBestTest].trainFitness;
                            tempTestFitness = this.allModelsInForm[r].population[indBestTest].testFitness;

                            f = (double)((double)(tempTrainFitness + tempTestFitness) / 2);
                            f = Math.Round(f, 4);
                            modelsList.Add("{" + (r * 3 + 3).ToString() + " RUN[" + r + "] (best test solution) " + f.ToString());

                            // Remember choice.
                            modulsChoices.Add(Tuple.Create(r, indBestTest));
                        }
                        #endregion
                    }

                    // Input box.
                    string message = "Please select solution to be vizualized.";
                    string title = "Select solution";
                    var inputBox = new CustomDialogBox(title, message, "visualize", antico.Properties.Resources.question, modelsList, "");
                    var result = inputBox.ShowDialog();

                    // Check if user clicked Cancel or OK.
                    if (result == DialogResult.Abort)
                    {
                        // User has clicked cancel. Do nothing.
                        return;
                    }
                    #endregion

                    // Create a new helper form.
                    this.formForVisualizationOfModel = new HelperForm();
                    this.formForVisualizationOfModel.Text = "Model";
                    this.formForVisualizationOfModel.WindowState = FormWindowState.Maximized;
                    this.formForVisualizationOfModel.exitSign.Location = new System.Drawing.Point(System.Windows.Forms.SystemInformation.PrimaryMonitorMaximizedWindowSize.Width - 41, 6);

                    #region graph
                    // Create a graph object.
                    Graph graph = new Graph("Model");

                    #region picked solution
                    // Check if 0 <-> bestModelInForm.
                    if (inputBox.pickedSolutionIndex == 0)
                    {
                        // Create the graph content from model.
                        this.DrawSymbolicTree(this.bestModelInForm.symbolicTree, ref graph);
                    }
                    else
                    {
                        // Create the graph content from model.
                        this.DrawSymbolicTree(this.allModelsInForm[modulsChoices[inputBox.pickedSolutionIndex-1].Item1].population[modulsChoices[inputBox.pickedSolutionIndex - 1].Item2].symbolicTree, ref graph);
                    }
                    #endregion

                    #endregion

                    #region viewer 
                    // Create a viewer object .
                    Microsoft.Msagl.GraphViewerGdi.GViewer viewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();
                    // Bind the graph to the viewer.
                    viewer.Graph = graph;

                    // Associate the viewer with the form.
                    this.formForVisualizationOfModel.SuspendLayout();
                    viewer.Dock = DockStyle.Fill;
                    viewer.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Viewer_KeyUp);
                    this.formForVisualizationOfModel.Controls.Add(viewer);
                    this.formForVisualizationOfModel.ResumeLayout();
                    #endregion

                    // Show the form.
                    this.formForVisualizationOfModel.ShowDialog();

                    return;
                }
                else if (this.uploaded)
                {

                    // Create a new helper form.
                    this.formForVisualizationOfModel = new HelperForm();
                    this.formForVisualizationOfModel.Text = "Model";
                    this.formForVisualizationOfModel.WindowState = FormWindowState.Maximized;
                    this.formForVisualizationOfModel.exitSign.Location = new System.Drawing.Point(System.Windows.Forms.SystemInformation.PrimaryMonitorMaximizedWindowSize.Width - 41, 6);

                    #region graph
                    // Create a graph object.
                    Graph graph = new Graph("Model");

                    // Draw symbolic tree.
                    this.DrawSymbolicTree(this.upload.model.symbolicTree, ref graph);
                    #endregion

                    #region viewer 
                    // Create a viewer object .
                    Microsoft.Msagl.GraphViewerGdi.GViewer viewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();
                    // Bind the graph to the viewer.
                    viewer.Graph = graph;

                    // Associate the viewer with the form.
                    this.formForVisualizationOfModel.SuspendLayout();
                    viewer.Dock = DockStyle.Fill;
                    viewer.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Viewer_KeyUp);
                    this.formForVisualizationOfModel.Controls.Add(viewer);
                    this.formForVisualizationOfModel.ResumeLayout();
                    #endregion

                    // Show the form.
                    this.formForVisualizationOfModel.ShowDialog();

                    return;
                }

                m = "Model is not yet created!";
                CustomDialogBox dialog2 = new CustomDialogBox("Warning!", m, global::antico.Properties.Resources.error, MessageBoxButtons.OK);
                dialog2.ShowDialog();
            }
        }
        #endregion

        #region save
        /// <summary>
        /// Saving the best model if exists.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveSign_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.saveSign.Visible)
            {
                string m;

                #region check if another process is already running
                // Check if another process already runnning.
                if (this.forbid)
                {
                    m = "Wait! Model calculation is still in progress.";
                    CustomDialogBox dialog = new CustomDialogBox("Warning!", m, global::antico.Properties.Resources.stop, MessageBoxButtons.OK);
                    dialog.ShowDialog();

                    return;
                }
                #endregion

                // Check if model exists.
                if (this.allModelsInForm != null && this.bestModelInForm != null)
                {
                    // User must input name of model.
                    #region name input

                    // Message, title and default value of input box.
                    string message = "Please select solution and input the name of the file to be saved.";
                    string title = "Select solution";
                    string defaultValue = "ABCP_Model_" + Math.Round(this.bestModelInForm.trainFitness, 4).ToString() + "__" + DateTime.Today.Day.ToString() + "_" + DateTime.Today.Month.ToString() + "_" + DateTime.Today.Year.ToString();

                    #region select solution
                    // Choices for saving.
                    List<string> modelsList = new List<string>();

                    // Save indices of choices.
                    // First item of tuple is index of run and second one is index of solution in population.
                    List<Tuple<int, int>> modulsChoices = new List<Tuple<int, int>>();

                    double f = (double)((double)(this.bestModelInForm.trainFitness + this.bestModelInForm.testFitness) / 2);
                    f = Math.Round(f, 4);
                    modelsList.Add("{0}[BEST SOLUTION] " + f.ToString());

                    for (var r = 0; r < this.allModelsInForm.Count; r++)
                    {
                        // Best indices of the solutions in the model.
                        int indBest = this.allModelsInForm[r].bestIndex;
                        int indBestTrain = this.allModelsInForm[r].bestTrainIndex;
                        int indBestTest = this.allModelsInForm[r].bestTestIndex;

                        #region best train + test
                        double tempTrainFitness = this.allModelsInForm[r].population[indBest].trainFitness;
                        double tempTestFitness = this.allModelsInForm[r].population[indBest].testFitness;

                        // Best train + test.
                        f = (double)((double)(tempTrainFitness + tempTestFitness) / 2);
                        f = Math.Round(f, 4);
                        modelsList.Add("{" + (r * 3 + 1).ToString() + "} RUN[" + r + "] (best solution) " + f.ToString());

                        // Remember choice.
                        modulsChoices.Add(Tuple.Create(r, indBest));
                        #endregion

                        #region best train
                        if (indBestTrain != indBest)
                        {
                            tempTrainFitness = this.allModelsInForm[r].population[indBestTrain].trainFitness;
                            tempTestFitness = this.allModelsInForm[r].population[indBestTrain].testFitness;

                            f = (double)((double)(tempTrainFitness + tempTestFitness) / 2);
                            f = Math.Round(f, 4);
                            modelsList.Add("{" + (r * 3 + 2).ToString() + "} RUN[" + r + "] (best train solution) " + f.ToString());

                            // Remember choice.
                            modulsChoices.Add(Tuple.Create(r, indBestTrain));
                        }
                        #endregion

                        #region best test
                        if (indBestTest != indBest && indBestTest != indBestTrain)
                        {
                            tempTrainFitness = this.allModelsInForm[r].population[indBestTest].trainFitness;
                            tempTestFitness = this.allModelsInForm[r].population[indBestTest].testFitness;

                            f = (double)((double)(tempTrainFitness + tempTestFitness) / 2);
                            f = Math.Round(f, 4);
                            modelsList.Add("{" + (r * 3 + 3).ToString() + " RUN[" + r + "] (best test solution) " + f.ToString());

                            // Remember choice.
                            modulsChoices.Add(Tuple.Create(r, indBestTest));
                        }
                        #endregion
                    }

                    // Input box.
                    var inputBox = new CustomDialogBox(title, message, "save", antico.Properties.Resources.question, modelsList, defaultValue);
                    var result = inputBox.ShowDialog();

                    // Check if user clicked Cancel or OK.
                    if (result == DialogResult.Abort)
                    {
                        // User has clicked cancel. Do nothing.
                        return;
                    }
                    #endregion

                    #endregion

                    // Create a hashtable of values that will eventually be serialized.
                    Hashtable addresses = new Hashtable();

                    // If there will be more version, to keep track of it when loading.
                    addresses.Add("SAVEVersion", 2);

                    #region picked solution
                    // Check if 0 <-> bestModelInForm.
                    if (inputBox.pickedSolutionIndex == 0)
                    {
                        addresses.Add("Solution", this.bestModelInForm);
                        bool breakloop = false;

                        // Add choosen run.
                        for (var j = 0; j < this.allModelsInForm.Count; j++)
                        {
                            for (var q = 0; q < modulsChoices.Count; q++)
                            {
                                if ( modulsChoices[q].Item1 == j && this.allModelsInForm[j].population[modulsChoices[q].Item2].trainFitness == this.bestModelInForm.trainFitness)
                                {
                                    // Add choosed run solution.
                                    addresses.Add("ChoosedRun", j + 1);
                                    breakloop = true;
                                    break;
                                }
                            }

                            if (breakloop)
                                break;
                        }
                    }
                    else
                    {
                        addresses.Add("Solution", this.allModelsInForm[modulsChoices[inputBox.pickedSolutionIndex - 1].Item1].population[modulsChoices[inputBox.pickedSolutionIndex - 1].Item2]);

                        // Add choosed run solution.
                        addresses.Add("ChoosedRun", modulsChoices[inputBox.pickedSolutionIndex - 1].Item1 + 1);
                    }
                    #endregion

                    addresses.Add("Parameters", this.formParameters);
                    addresses.Add("DatabaseName", this.formData.databaseName);
                    addresses.Add("FeatureNames", this.formData.featureNames);
                    addresses.Add("NumberOfFolds", this.formData.numberOfFolds);
                    addresses.Add("MathOperators", this.formData.mathOperators);

                    // For progress visualization.
                    addresses.Add("FitnessProgress", this.progressForm.fitnessPoints);
                    addresses.Add("DepthsProgress", this.progressForm.depthPoints);
                    addresses.Add("TPProgress", this.progressForm.tpPoints);
                    addresses.Add("TNProgress", this.progressForm.tnPoints);
                    addresses.Add("FPProgress", this.progressForm.fpPoints);
                    addresses.Add("FNProgress", this.progressForm.fnPoints);
                    addresses.Add("TP_TN_FP_FN_Progress", this.progressForm.accuracyPointsTrain);

                    // For console.
                    addresses.Add("ConsoleOutput", this.consoleForm.printoutTextBox.Text);


                    // Name to be. Path to be.
                    string fileName = @"../../../../[DATA]/saved/" + inputBox.pickedName + ".dat";
                    string fullPath = Path.GetFullPath(fileName);

                    // To serialize the hashtable and its key/value pairs, first open a stream for writing. 
                    FileStream fs = new FileStream(fileName, FileMode.Create);

                    // Construct a BinaryFormatter and use it to serialize the data to the stream.
                    BinaryFormatter formatter = new BinaryFormatter();
                    try
                    {
                        formatter.Serialize(fs, addresses);
                    }
                    catch (SerializationException exception)
                    {
                        Console.WriteLine("Failed to serialize. Reason: " + exception.Message);
                        throw;
                    }
                    finally
                    {
                        fs.Close();
                    }

                    // Notify the user where the file is saved.
                    string success_message = "File is saved on: " + fullPath + " !";
                    CustomDialogBox dialog2 = new CustomDialogBox("Saved", success_message, global::antico.Properties.Resources.new_packet, MessageBoxButtons.OK);
                    dialog2.ShowDialog();
                    return;
                }

                // Model does not exist. Warn user!
                m = "Model is not yet created!";
                CustomDialogBox dialog3 = new CustomDialogBox("Warning!", m, global::antico.Properties.Resources.error, MessageBoxButtons.OK);
                dialog3.ShowDialog();
            }

        }
        #endregion

        #region lookup console form
        /// <summary>
        /// Showing console form on request.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LookupConsoleFormSign_MouseClick(object sender, MouseEventArgs e)
        {
            // Show console form only if search is ongoing or done. 
            // Also, check out if console is already opened.
            if (!this.consoleForm.Visible && (this.forbid || this.bestModelInForm != null) && !this.uploaded)
            {
                // Show form.
                this.consoleForm.Visible = true;
                this.lookupConsoleFormSign.BackgroundImage = antico.Properties.Resources.console_lookup_darker;
            }
            else if (!this.consoleForm.Visible && this.uploaded)
            {
                // If model was uploaded, show form with console output loaded from file.
                this.consoleForm.Visible = true;
                this.consoleForm.printoutTextBox.Text = this.upload.consoleOutput;
                this.lookupConsoleFormSign.BackgroundImage = antico.Properties.Resources.console_lookup_darker;
            }
        }
        #endregion

        #region upload
        /// <summary>
        /// Show dialog for file upload and uupload the solution (from .dat file).
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UploadSign_MouseClick(object sender, MouseEventArgs e)
        {
            #region check if another process is already running
            // Check if another process already runnning.
            if (this.forbid)
            {
                string m = "Wait! Model calculation is still in progress.";
                CustomDialogBox dialog = new CustomDialogBox("Warning!", m, global::antico.Properties.Resources.stop, MessageBoxButtons.OK);
                dialog.ShowDialog();

                return;
            }
            #endregion

            DialogResult result = this.uploadFileDialog.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                string file = this.uploadFileDialog.FileName;
                try
                {
                    // Clean progress form.
                    this.progressForm = new ProgressForm(mainForm.Location);
                    this.progressForm.Visible = false;
                    this.progressForm.VisibleChanged += new EventHandler(this.ProgressForm_VisibilityChanged);

                    // Load file.
                    LoadFile(file);

                    // Set flag on true.
                    this.uploaded = true;

                    // Hide save sign since this model is already saved.
                    this.saveSign.Visible = false;

                    // Show all other signs.
                    this.lookupConsoleFormSign.Visible = true;
                    this.lookupConsoleFormSign.BackgroundImage = antico.Properties.Resources.console_lookup;
                    this.showSolutionsProgressSign.Visible = true;
                    this.showSolutionsProgressSign.BackgroundImage = antico.Properties.Resources.progress_chart;
                    this.visualizeSign.Visible = true;

                    // Hide layout for parameters.
                    this.mainLayout.Visible = false;

                    #region printout to form - parameters, fitness and other
                    // Add printoutOfAllSolutionsLabel to form if it wasn't previously added.
                    if (!this.Controls.Contains(this.panelForPrintoutLabel))
                    {
                        AddControlForPrintOutToForm();
                    }

                    // Print train fitness of the solution.
                    this.printoutOfAllSolutionsLabel.Text = "\r\n Train fitness of solution: " + this.upload.model.trainFitness.ToString() + "\r\n";
                    // Print test fitness of the solution.
                    this.printoutOfAllSolutionsLabel.Text += " Test fitness of solution: " + this.upload.model.testFitness.ToString() + "\r\n\r\n";
                    // Print database name.
                    this.printoutOfAllSolutionsLabel.Text += " Database name: " + this.upload.databaseName + "\r\n";

                    // Version higher than 1 have saved choosenRun.
                    if (this.upload.version > 1)
                    {
                        // Print index of the best run.
                        this.printoutOfAllSolutionsLabel.Text += " Run index: " + (this.upload.choosenRun).ToString() + "\r\n";

                        int numOfIter = this.upload.fitnessPoints[this.upload.choosenRun].Item1.Count;

                        // Print number of iterations.
                        this.printoutOfAllSolutionsLabel.Text += " Number of iterations: " + (numOfIter - 2).ToString() + "\r\n";
                        // Print depth of solution tree.
                        this.printoutOfAllSolutionsLabel.Text += " Depth of the solution tree: " + this.upload.depthPoints[this.upload.choosenRun][numOfIter - 1] + "\r\n";

                        // Print TN, TP, FN, FP of train data.
                        this.printoutOfAllSolutionsLabel.Text += " Number of TN: " + this.upload.accuracyPointsTrain[this.upload.choosenRun].Item1[numOfIter - 1] + "\r\n";
                        this.printoutOfAllSolutionsLabel.Text += " Number of TP: " + this.upload.accuracyPointsTrain[this.upload.choosenRun].Item2[numOfIter - 1] + "\r\n";
                        this.printoutOfAllSolutionsLabel.Text += " Number of FN: " + this.upload.accuracyPointsTrain[this.upload.choosenRun].Item3[numOfIter - 1] + "\r\n";
                        this.printoutOfAllSolutionsLabel.Text += " Number of FP: " + this.upload.accuracyPointsTrain[this.upload.choosenRun].Item4[numOfIter - 1] + "\r\n";

                        // When opening progress form show fitness progress of choosen run.
                        this.progressForm.ShowChart(this.upload.choosenRun, "fitness");
                    }
                    else
                    {
                        // Try to find the run.
                        for (var r = 0; r < this.upload.fitnessPoints.Keys.Count; r++)
                        {
                            int numOfIter = this.upload.fitnessPoints[r + 1].Item1.Count;
                            if (this.upload.fitnessPoints[r + 1].Item1[numOfIter - 1] == this.upload.model.trainFitness)
                            {
                                if (this.upload.fitnessPoints.Keys.Count >= 2)
                                {
                                    // Print index of the best run.
                                    this.printoutOfAllSolutionsLabel.Text += " Run index: " + (r + 1).ToString() + "\r\n";
                                }

                                // Print number of iterations.
                                this.printoutOfAllSolutionsLabel.Text += " Number of iterations: " + (numOfIter - 2).ToString() + "\r\n";
                                // Print depth of solution tree.
                                this.printoutOfAllSolutionsLabel.Text += " Depth of the solution tree: " + this.upload.depthPoints[r + 1][numOfIter - 1] + "\r\n";

                                // Print TN, TP, FN, FP of train data.
                                this.printoutOfAllSolutionsLabel.Text += " Number of TN: " + this.upload.accuracyPointsTrain[r + 1].Item1[numOfIter - 1] + "\r\n";
                                this.printoutOfAllSolutionsLabel.Text += " Number of TP: " + this.upload.accuracyPointsTrain[r + 1].Item2[numOfIter - 1] + "\r\n";
                                this.printoutOfAllSolutionsLabel.Text += " Number of FN: " + this.upload.accuracyPointsTrain[r + 1].Item3[numOfIter - 1] + "\r\n";
                                this.printoutOfAllSolutionsLabel.Text += " Number of FP: " + this.upload.accuracyPointsTrain[r + 1].Item4[numOfIter - 1] + "\r\n";

                                break;
                            }
                        }
                    }

                    // Print parameters.
                    this.printoutOfAllSolutionsLabel.Text += "\r\n Population size: " + this.upload.parameters.populationSize + "\r\n";
                    this.printoutOfAllSolutionsLabel.Text += " Maximal number of iterations: " + this.upload.parameters.maxNumberOfIterations + "\r\n";
                    this.printoutOfAllSolutionsLabel.Text += " Maximal number of not improving iterations: " + this.upload.parameters.maxNumberOfNotImprovingIterations + "\r\n";
                    this.printoutOfAllSolutionsLabel.Text += " Number of runs: " + this.upload.parameters.numberOfRuns + "\r\n";
                    this.printoutOfAllSolutionsLabel.Text += " Limit: " + this.upload.parameters.limit + "\r\n";
                    this.printoutOfAllSolutionsLabel.Text += " Alpha: " + this.upload.parameters.alpha + "\r\n";
                    this.printoutOfAllSolutionsLabel.Text += " Initial maximal depth of solution tree: " + this.upload.parameters.initialMaxDepth + "\r\n";
                    this.printoutOfAllSolutionsLabel.Text += " Maximal depth of solution tree: " + this.upload.parameters.maxDepth + "\r\n";
                    this.printoutOfAllSolutionsLabel.Text += " Probability of choosing non-terminal over terminal in crossover: " + this.upload.parameters.probability + "\r\n";
                    this.printoutOfAllSolutionsLabel.Text += " Method for generating solution trees: " + this.upload.parameters.generatingTreesMethod + "\r\n\r\n";

                    // Print mathematical operators.
                    this.printoutOfAllSolutionsLabel.Text += " Mathematical operations used in search: \r\n";
                    for (var j = 0; j < this.upload.mathOperators.Count; j++)
                    {
                        this.printoutOfAllSolutionsLabel.Text += " " + this.upload.mathOperators[j] + " ";
                    }
                    this.printoutOfAllSolutionsLabel.Text += "\r\n";

                    // Show printout.
                    this.panelForPrintoutLabel.Visible = true;
                    this.printoutOfAllSolutionsLabel.Visible = true;
                    #endregion

                }
                catch (IOException)
                {
                    throw new Exception("[UploadSign_MouseClick] Coudn't load the file.");
                }
            }
        }
        #endregion

        #region solutions progress
        /// <summary>
        /// Showing progress form on request.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowSolutionsProgressSign_MouseClick(object sender, MouseEventArgs e)
        {
            // Show progress form only if previously not opened and if best model in form is not null or
            // if search is still ongoing or if the model was uploaded via file.
            if (this.progressForm.Visible == false && (this.forbid || this.bestModelInForm != null) && !this.uploaded)
            {
                this.progressForm.Visible = true;
                this.showSolutionsProgressSign.BackgroundImage = antico.Properties.Resources.progress_chart_darker;
            }
            else if (!this.progressForm.Visible && this.uploaded)
            {
                // If model was uploaded, show form with console output loaded from file.
                this.progressForm.Visible = true;
                this.showSolutionsProgressSign.BackgroundImage = antico.Properties.Resources.progress_chart_darker;
            }
        }
        #endregion

        #endregion

        #region HELPER METHODS

        #region Event for "KeyUp" on viewer on helper form for visualisation of model.
        /// <summary>
        /// Closing Form for visualization of model.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Viewer_KeyUp(object sender, KeyEventArgs e)
        {
            // If Esc key is pressed, close the helper form.
            if (e.KeyCode == Keys.Escape)
            {
                this.formForVisualizationOfModel.Close();
            }
        }
        #endregion

        #region Draw symbolic tree.
        /// <summary>
        /// Recursive helper method for adding connections between nodes in symbolic regression tree (that represents a model).
        /// Also, coloring the nodes depending on depth of the node in tree.
        /// </summary>
        /// 
        /// <param name="node">Current node. In first iteration it is the root node.</param>
        /// <param name="graph">Reference to the graph in which connections are added.</param>
        private void DrawSymbolicTree(SymbolicTreeNode node, ref Graph graph)
        {
            // Draw connections from the current node to his children, if it has any.
            if (node.children != null)
            {
                for (var i = 0; i < node.children.Count; i++)
                {
                    // Different names based on arity of the node.
                    if (node.arity == 2 && node.children[i].arity == 2)
                    {
                        // Add edge between the current node and his child node.
                        graph.AddEdge("(" + node.index + ")\r\n  " + node.content, "(" + node.children[i].index + ")\r\n  " + node.children[i].content);
                    }
                    else if (node.arity == 2 && node.children[i].arity != 2)
                    {
                        // Add edge between the current node and his child node.
                        graph.AddEdge("(" + node.index + ")\r\n  " + node.content, "(" + node.children[i].index + ")\r\n" + node.children[i].content);
                    }
                    else if (node.arity != 2 && node.children[i].arity == 2)
                    {
                        // Add edge between the current node and his child node.
                        graph.AddEdge("(" + node.index + ")\r\n" + node.content, "(" + node.children[i].index + ")\r\n  " + node.children[i].content);
                    }
                    else
                    {
                        // Add edge between the current node and his child node.
                        graph.AddEdge("(" + node.index + ")\r\n" + node.content, "(" + node.children[i].index + ")\r\n" + node.children[i].content);
                    }

                    // Do the same for child subtree.
                    DrawSymbolicTree(node.children[i], ref graph);
                }
            }

            // Coloring the node.
            int d = node.depth;
            if (d >= this.depthColors.Keys.Count)
            {
                // Check if depth is higher than number of defined colors. 
                // If is, calculate depth modulo number of colors.
                d = d % this.depthColors.Keys.Count;
            }
            System.Drawing.Color c = System.Drawing.ColorTranslator.FromHtml(this.depthColors[d]);
            if (node.arity == 2 )
            {
                graph.FindNode("(" + node.index + ")\r\n  " + node.content).Attr.FillColor = new Microsoft.Msagl.Drawing.Color((byte)c.R, (byte)c.G, (byte)c.B);
            }
            else if (node.arity != 2)
            {
                graph.FindNode("(" + node.index + ")\r\n" + node.content).Attr.FillColor = new Microsoft.Msagl.Drawing.Color((byte)c.R, (byte)c.G, (byte)c.B);
            }
        }
        #endregion

        #region Printout of all solutions.
        /// <summary>
        /// Prints all solutions to TextBox in form.
        /// </summary>
        private void PrintoutAllSolutions()
        {
            // Add printoutOfAllSolutionsLabel to form if it wasn't previously added.
            if (!this.Controls.Contains(this.panelForPrintoutLabel))
            {
                AddControlForPrintOutToForm();
            }

            // Printout on label.
            string input = "\r\n BEST MODEL: \r\n SymbolicTree:  " + this.bestModelInForm.symbolicTree.ToStringInorder();
            if (this.formData.numberOfFolds != 0)
                input += "\r\n real (average) fold fitness: " + this.bestModelInForm.realFoldAccuraccy.ToString();
            input += "\r\n train fitness:  " + this.bestModelInForm.trainFitness.ToString();
            input += "\r\n test fitness:  " + this.bestModelInForm.testFitness.ToString();
            input += "\r\n (train+test) fitness:  " + ((double)(this.bestModelInForm.trainFitness + this.bestModelInForm.testFitness) / 2).ToString() + "\r\n\r\n";

            this.panelForPrintoutLabel.Visible = true;
            this.printoutOfAllSolutionsLabel.Text = input;

            // Printout of all best solutions.
            for (var run = 0; run < this.formParameters.numberOfRuns; run++)
            {
                // Best indices of the solutions in the model.
                int indBest = this.allModelsInForm[run].bestIndex;
                int indBestTrain = this.allModelsInForm[run].bestTrainIndex;
                int indBestTest = this.allModelsInForm[run].bestTestIndex;

                #region best train + test
                double tempTrainFitness = this.allModelsInForm[run].population[indBest].trainFitness;
                double tempTestFitness = this.allModelsInForm[run].population[indBest].testFitness;

                // Best train + test.
                if (this.formData.numberOfFolds == 0)
                    input = "\r\n BEST (TRAIN+TEST) SOLUTION IN RUN:" + (run + 1).ToString() + "\r\n SymbolicTree:  " + this.allModelsInForm[run].population[indBest].symbolicTree.ToStringInorder();
                else
                {
                    input = "\r\n BEST (TRAIN+TEST) SOLUTION IN RUN:" + (run + 1).ToString();
                    input += "\r\n FOLD: " + (choosenFold[run] + 1).ToString() + "  (Number in solution progress window: " + (run * this.formData.numberOfFolds + 1 + choosenFold[run]).ToString() + ")";
                    input += "\r\n real (average) fold fitness: " + this.allModelsInForm[run].population[indBest].realFoldAccuraccy.ToString();
                    input += "\r\n SymbolicTree:  " + this.allModelsInForm[run].population[indBest].symbolicTree.ToStringInorder();
                }

                input += "\r\n train fitness:  " + tempTrainFitness;
                input += "\r\n test fitness:  " + tempTestFitness;
                input += "\r\n (train+test) fitness:  " + ((double)(tempTrainFitness + tempTestFitness) / 2).ToString() + "\r\n";
                #endregion

                #region best train
                if (indBestTrain != indBest)
                {
                    tempTrainFitness = this.allModelsInForm[run].population[indBestTrain].trainFitness;
                    tempTestFitness = this.allModelsInForm[run].population[indBestTrain].testFitness;

                    // Best train.
                    if (this.formData.numberOfFolds == 0)
                        input += "\r\n BEST(TRAIN) SOLUTION IN RUN:" + (run + 1).ToString() + "\r\n SymbolicTree:  " + this.allModelsInForm[run].population[indBestTrain].symbolicTree.ToStringInorder();
                    else
                    {
                        input += "\r\n BEST (TRAIN) SOLUTION IN RUN:" + (run + 1).ToString();
                        input += "\r\n FOLD: " + (choosenFold[run] + 1).ToString() + "  (Number in solution progress window: " + (run * this.formData.numberOfFolds + 1 + choosenFold[run]).ToString() + ")";
                        input += "\r\n real (average) fold fitness: " + this.allModelsInForm[run].population[indBestTrain].realFoldAccuraccy.ToString();
                        input += "\r\n SymbolicTree:  " + this.allModelsInForm[run].population[indBestTrain].symbolicTree.ToStringInorder();
                    }

                    input += "\r\n train fitness:  " + tempTrainFitness;
                    input += "\r\n test fitness:  " + tempTestFitness;
                    input += "\r\n (train+test) fitness:  " + ((double)(tempTrainFitness + tempTestFitness) / 2).ToString() + "\r\n";
                }
                #endregion

                #region best test
                if (indBestTest != indBest && indBestTest != indBestTrain)
                {
                    tempTrainFitness = this.allModelsInForm[run].population[indBestTest].trainFitness;
                    tempTestFitness = this.allModelsInForm[run].population[indBestTest].testFitness;

                    // Best test.
                    if (this.formData.numberOfFolds == 0)
                        input += "\r\n BEST(TEST) SOLUTION IN RUN:" + (run + 1).ToString() + "\r\n SymbolicTree:  " + this.allModelsInForm[run].population[indBestTest].symbolicTree.ToStringInorder();
                    else
                    {
                        input += "\r\n BEST (TEST) SOLUTION IN RUN:" + (run + 1).ToString();
                        input += "\r\n FOLD: " + (choosenFold[run] + 1).ToString() + "  (Number in solution progress window: " + (run * this.formData.numberOfFolds + 1 + choosenFold[run]).ToString() + ")";
                        input += "\r\n real (average) fold fitness: " + this.allModelsInForm[run].population[indBestTest].realFoldAccuraccy.ToString();
                        input += "\r\n SymbolicTree:  " + this.allModelsInForm[run].population[indBestTest].symbolicTree.ToStringInorder();
                    }

                    input += "\r\n train fitness:  " + tempTrainFitness;
                    input += "\r\n test fitness:  " + tempTestFitness;
                    input += "\r\n (train+test) fitness:  " + ((double)(tempTrainFitness + tempTestFitness) / 2).ToString() + "\r\n\r\n";
                }
                #endregion

                // Add to label.
                this.printoutOfAllSolutionsLabel.Text += input;
            }

            this.printoutOfAllSolutionsLabel.Visible = true;
        }
        #endregion

        #region Add panel and label for printout of solutions to form.
        /// <summary>
        /// Adds new panel and label to form for printouts of solutions.
        /// </summary>
        private void AddControlForPrintOutToForm()
        {
            this.printoutOfAllSolutionsLabel = new System.Windows.Forms.Label();
            this.panelForPrintoutLabel = new System.Windows.Forms.Panel();

            this.panelForPrintoutLabel.SuspendLayout();
            // 
            // printoutOfAllSolutionsLabel
            // 
            this.printoutOfAllSolutionsLabel.AutoSize = true;
            this.printoutOfAllSolutionsLabel.Font = new System.Drawing.Font("Source Sans Pro", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.printoutOfAllSolutionsLabel.ForeColor = System.Drawing.SystemColors.Control;
            this.printoutOfAllSolutionsLabel.Location = new System.Drawing.Point(0, 0);
            this.printoutOfAllSolutionsLabel.Margin = new System.Windows.Forms.Padding(5);
            this.printoutOfAllSolutionsLabel.MaximumSize = new System.Drawing.Size(500, 0);
            this.printoutOfAllSolutionsLabel.MinimumSize = new System.Drawing.Size(500, 550);
            this.printoutOfAllSolutionsLabel.Name = "printoutOfAllSolutionsLabel";
            this.printoutOfAllSolutionsLabel.Size = new System.Drawing.Size(500, 550);
            this.printoutOfAllSolutionsLabel.TabIndex = 18;
            this.printoutOfAllSolutionsLabel.Visible = false;
            this.printoutOfAllSolutionsLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PrintoutOfAllSolutionsLabel_MouseDown);
            this.printoutOfAllSolutionsLabel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PrintoutOfAllSolutionsLabel_MouseMove);
            this.printoutOfAllSolutionsLabel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PrintoutOfAllSolutionsLabel_MouseUp);
            // Set up background of the label for the printout of the solutions.
            this.printoutOfAllSolutionsLabel.BackColor = System.Drawing.Color.FromArgb(100, System.Drawing.Color.WhiteSmoke);
            // 
            // panelForPrintoutLabel
            // 
            this.panelForPrintoutLabel.AutoScroll = true;
            this.panelForPrintoutLabel.BackColor = System.Drawing.Color.Transparent;
            this.panelForPrintoutLabel.Controls.Add(this.printoutOfAllSolutionsLabel);
            this.panelForPrintoutLabel.Font = new System.Drawing.Font("Source Sans Pro", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelForPrintoutLabel.Location = new System.Drawing.Point(30, 107);
            this.panelForPrintoutLabel.Name = "panelForPrintoutLabel";
            this.panelForPrintoutLabel.Size = new System.Drawing.Size(520, 550);
            this.panelForPrintoutLabel.TabIndex = 19;
            this.panelForPrintoutLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PanelForPrintoutLabel_MouseDown);
            this.panelForPrintoutLabel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PanelForPrintoutLabel_MouseMove);
            this.panelForPrintoutLabel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PanelForPrintoutLabel_MouseUp);

            this.Controls.Add(this.panelForPrintoutLabel);

            this.panelForPrintoutLabel.ResumeLayout(false);
            this.panelForPrintoutLabel.PerformLayout();
        }
        #endregion

        #region Show parameters to add on form.
        /// <summary>
        /// Show on form parameters that can be choosen.
        /// </summary>
        private void ShowParameters()
        {
            // Default selected item.
            if (this.databaseComboBox.Items.Count > 0)
            {
                this.databaseComboBox.SelectedIndex = 0;
            }

            this.mainLayout.Visible = true;
        }
        #endregion

        #region Before we START.
        /// <summary>
        /// Makes signs for showing console and progress form visible. (Important for first time starting the search.)
        /// Making those form visible to user and also chaging icon of the signs to darker image 
        /// representing that user can't click on them while forms are visible.
        /// </summary>
        private void BeforeWeStart()
        {
            // Progress bar (re)set.
            this.progressBar.Visible = true;
            this.progressBar.Step = 1;
            this.progressBar.Value = 0;
            if (this.formData.numberOfFolds == 0)
            {
                this.progressBar.Maximum = this.formParameters.numberOfRuns * this.formParameters.maxNumberOfIterations;
            }
            else
            {
                this.progressBar.Maximum = this.formParameters.numberOfRuns * this.formParameters.maxNumberOfIterations * this.formData.numberOfFolds;

                // Set name of the drop down menu to "Fold" in progress form.
                this.progressForm.runToolStripMenuItem.Text = "Fold";

                // Initialize list of choosen fold indices.
                choosenFold = new List<int>();
            }

            // Forbid all other cliks.
            this.forbid = true;

            // Hide save and visualize sign.
            this.saveSign.Visible = false;
            this.visualizeSign.Visible = false;

            // Show sign if not visible.
            // Darker lookup console sign.
            this.lookupConsoleFormSign.Visible = true;
            this.lookupConsoleFormSign.BackgroundImage = antico.Properties.Resources.console_lookup_darker;

            // Show sign if not visible.
            // Darker solutions progress sign.
            this.showSolutionsProgressSign.Visible = true;
            this.showSolutionsProgressSign.BackgroundImage = antico.Properties.Resources.progress_chart_darker;

            // Clear textbox in console form.
            this.consoleForm.printoutTextBox.Text = "";

            // Remove old values from the chart.
            this.progressForm.chart.Series[0].Values.Clear();
            this.progressForm.chart.Series[1].Values.Clear();
            this.progressForm.depthsChart.Series[0].Values.Clear();
            this.progressForm.accuracyChart.Series[0].Values.Clear();
            this.progressForm.accuracyChart.Series[1].Values.Clear();
            this.progressForm.accuracyChart.Series[2].Values.Clear();
            this.progressForm.accuracyChart.Series[3].Values.Clear();

            // (Re)Set dictionary of points in progress form.
            this.progressForm.fitnessPoints = new Dictionary<int, Tuple<List<double>, List<double>>>();
            this.progressForm.tpPoints = new Dictionary<int, Tuple<List<int>, List<int>>>();
            this.progressForm.tnPoints = new Dictionary<int, Tuple<List<int>, List<int>>>();
            this.progressForm.fnPoints = new Dictionary<int, Tuple<List<int>, List<int>>>();
            this.progressForm.fpPoints = new Dictionary<int, Tuple<List<int>, List<int>>>();
            this.progressForm.depthPoints = new Dictionary<int, List<int>>();
            this.progressForm.accuracyPointsTrain = new Dictionary<int, Tuple<List<int>, List<int>, List<int>, List<int>>>();

            // Reset options for runs.
            this.progressForm.runToolStripMenuItem.DropDownItems.Clear();
            this.progressForm.selectedRun = -1;

            // Hide all charts.
            this.progressForm.chart.Visible = false;
            this.progressForm.depthsChart.Visible = false;
            this.progressForm.accuracyChart.Visible = false;

            // Open console and progress form if already not opened.
            if (CheckOpened(this.consoleForm.Text))
            {
                this.consoleForm.Show();
            }
            if (CheckOpened(this.progressForm.Text))
            {
                this.progressForm.Show();
            }
            // Show forms.
            this.consoleForm.Visible = true;
            this.progressForm.Visible = true;
        }
        #endregion

        #region Check if form opened.
        /// <summary>
        /// Helper method for checking if specific form is already opened.
        /// </summary>
        /// 
        /// <param name="name"></param>
        /// <returns></returns>
        private bool CheckOpened(string name)
        {
            FormCollection fc = Application.OpenForms;

            foreach (Form frm in fc)
            {
                if (frm.Text == name)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region Load file.
        /// <summary>
        /// Loads file with specific path and name (file) and saves it in UploadedModel class.
        /// </summary>
        /// 
        /// <param name="file">Name of the file to be loaded into instance of UploadedModel class.</param>
        private void LoadFile(string file)
        {
            // Declare the hashtable reference.
            Hashtable addresses = null;

            // Open the file containing the data that you want to deserialize.
            FileStream fs = new FileStream(file, FileMode.Open);
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();

                // Deserialize the hashtable from the file and  
                // assign the reference to the local variable.
                addresses = (Hashtable)formatter.Deserialize(fs);
            }
            catch (SerializationException e)
            {
                string m = "Failed to deserialize. Reason: " + e.Message;
                CustomDialogBox dialog = new CustomDialogBox("Failed", m, global::antico.Properties.Resources.error, MessageBoxButtons.OK);
                dialog.ShowDialog();
                throw new Exception("[LoadFile] Failed to deserialize. Reason: " + e.Message);
            }
            finally
            {
                fs.Close();
            }

            // New uploaded model.
            this.upload = new UploadedModel();

            // Load values from file into UploadedModel class.
            foreach (DictionaryEntry de in addresses)
            {
                this.upload.Load(de.Key.ToString(), de.Value, ref progressForm);
            }

            for (var r = 0; r < this.upload.fitnessPoints.Keys.Count; r++)
            {
                this.progressForm.runToolStripMenuItem.DropDownItems.Add((r + 1).ToString());
            }
        }
        #endregion

        #endregion

        #region ABCP

        #region Search foor best solution (threaded)
        /// <summary>
        /// Threaded (whole) search for best solution with parameters and data previously set.
        /// </summary>
        private void SearchForTheBestSolution()
        {
            // Initialize and printout when done using threads because of animation.
            this.myThread = new Thread(
                () =>
                {
                    try
                    {
                        this.Invoke((MethodInvoker)delegate { this.waitingAnimation.Visible = true; });

                        // Reset class ABCP variables.
                        this.allModelsInForm = new List<ABCP>();
                        this.bestModelInForm = new Chromosome();

                        // Do the job.
                        ABCPRuns();

                        System.Threading.Thread.Sleep(5000);
                        this.Invoke((MethodInvoker)delegate { this.waitingAnimation.Visible = false; });
                    }
                    finally
                    {
                        // Back to normal.
                        this.forbid = false;
                    }

                });
            this.myThread.Start();
        }
        #endregion

        #region ABCP all runs.
        /// <summary>
        /// Preforms all runs of ABCP algorithm.
        /// </summary>
        /// <returns>(Relevant only if numberOfFollds > 0). Fold that was choosen to be the best.</returns>
        private void ABCPRuns()
        {
            for (var run = 0; run < this.formParameters.numberOfRuns; run++)
            {
                // Printout on console.
                string time = Microsoft.VisualBasic.DateAndTime.Now.ToString("MM/dd/yyyy HH:mm");
                this.consoleForm.Invoke((MethodInvoker)delegate { this.consoleForm.printoutTextBox.AppendText("\r\n[" + time + "] RUN: " + run.ToString() + "\r\n"); });

                // Do this run.
                int fold = this.PreformABCP(run);

                if (fold != -1)
                    choosenFold.Add(fold);

                // Printout on console.
                this.Invoke((MethodInvoker)delegate { this.consoleForm.printoutTextBox.AppendText("\r\n********************************************************\r\n"); });
            }  
        }
        #endregion

        #region Preform one run of ABCP.
        /// <summary>
        /// Helper method for preforming (one - with index of current run: 'run') ABCP.
        /// 
        /// 
        /// IF number of folds is 0
        ///     - just preform search for the model and update solution 'bestModelInForm' if this run improved it.
        ///     
        /// IF number of folds is not 0
        ///     - run search for the model for every fold
        ///     - find best model in all folds by looking at (realTrainFitness + realTestFitness) values 
        ///     of 'best', 'bestTrain' and 'bestTest' solutions in each fold
        ///     (those solutions are best on "fake" (folds) train and test datasets).
        ///     - after searches for the best models finishes in all folds, best fold (index of) is known
        ///     - update 'trainFitness' and 'testFitness' of all solutions (including 'best','bestTrain' and 'bestTest')
        ///     in model and add it to global variable for all best models in all runs (allModelsInForm)
        ///     - update solution 'bestModelInForm' if this run improved it
        ///     
        /// 
        /// </summary>
        /// 
        /// <param name="run">Index of the current run.</param>
        /// <returns>(Relevant only if numberOfFollds > 0). Fold that was choosen to be the best.</returns>
        private int PreformABCP(int run)
        {
            int retVal = -1;

            // Check if parameters and data are previously set.
            if (this.formData == null || this.formParameters == null)
                throw new Exception("[PreformABCP] Parameters/Data not set!");

            // Train model in this run depending on number of folds.
            if (this.formData.numberOfFolds == 0)
            {
                // Add new key to dictionaries in progressForm.
                if (!this.progressForm.fitnessPoints.ContainsKey(run + 1))
                    this.progressForm.fitnessPoints.Add(run + 1, Tuple.Create(new List<double>(), new List<double>()));
                if (!this.progressForm.tpPoints.ContainsKey(run + 1))
                    this.progressForm.tpPoints.Add(run + 1, Tuple.Create(new List<int>(), new List<int>()));
                if (!this.progressForm.tnPoints.ContainsKey(run + 1))
                    this.progressForm.tnPoints.Add(run + 1, Tuple.Create(new List<int>(), new List<int>()));
                if (!this.progressForm.fpPoints.ContainsKey(run + 1))
                    this.progressForm.fpPoints.Add(run + 1, Tuple.Create(new List<int>(), new List<int>()));
                if (!this.progressForm.fnPoints.ContainsKey(run + 1))
                    this.progressForm.fnPoints.Add(run + 1, Tuple.Create(new List<int>(), new List<int>()));
                if (!this.progressForm.depthPoints.ContainsKey(run + 1))
                    this.progressForm.depthPoints.Add(run + 1, new List<int>());
                if (!this.progressForm.accuracyPointsTrain.ContainsKey(run + 1))
                    this.progressForm.accuracyPointsTrain.Add(run + 1, Tuple.Create(new List<int>(), new List<int>(), new List<int>(), new List<int>()));

                // Add option to menu for selecting this run.
                this.progressForm.Invoke((MethodInvoker)delegate { this.progressForm.runToolStripMenuItem.DropDownItems.Add((run + 1).ToString()); });

                // Initialization of model.
                this.allModelsInForm.Add(new ABCP(this.formParameters, this.formData, this.formData.trainFeatures, this.formData.testFeatures, this, this.consoleForm.printoutTextBox));

                // Search for best model.
                this.allModelsInForm[run].basicABCP(this, this.consoleForm.printoutTextBox, run + 1, this.progressBar);
            }
            else
            {
                // Train with folds.

                // Variable for saving models from all folds.
                List<ABCP> allFoldModelsInThisRun = new List<ABCP>();

                // Best solution in all folds.
                Chromosome bestInFolds = new Chromosome();

                // Index of the fold with best solution of all folds.
                int bestFoldIndex = 0;

                #region create model for every fold
                // Create model for every fold.
                for (var f = 0; f < this.formData.numberOfFolds; f++)
                {
                    #region add key to dictionaries
                    int dictKey = run * this.formData.numberOfFolds + f + 1;
                    // Add new key to dictionaries in progressForm.
                    if (!this.progressForm.fitnessPoints.ContainsKey(dictKey))
                        this.progressForm.fitnessPoints.Add(dictKey, Tuple.Create(new List<double>(), new List<double>()));
                    if (!this.progressForm.tpPoints.ContainsKey(dictKey))
                        this.progressForm.tpPoints.Add(dictKey, Tuple.Create(new List<int>(), new List<int>()));
                    if (!this.progressForm.tnPoints.ContainsKey(dictKey))
                        this.progressForm.tnPoints.Add(dictKey, Tuple.Create(new List<int>(), new List<int>()));
                    if (!this.progressForm.fpPoints.ContainsKey(dictKey))
                        this.progressForm.fpPoints.Add(dictKey, Tuple.Create(new List<int>(), new List<int>()));
                    if (!this.progressForm.fnPoints.ContainsKey(dictKey))
                        this.progressForm.fnPoints.Add(dictKey, Tuple.Create(new List<int>(), new List<int>()));
                    if (!this.progressForm.depthPoints.ContainsKey(dictKey))
                        this.progressForm.depthPoints.Add(dictKey, new List<int>());
                    if (!this.progressForm.accuracyPointsTrain.ContainsKey(dictKey))
                        this.progressForm.accuracyPointsTrain.Add(dictKey, Tuple.Create(new List<int>(), new List<int>(), new List<int>(), new List<int>()));
                    #endregion

                    // Add option to menu for selecting this run.
                    //this.progressForm.Invoke((MethodInvoker)delegate { this.progressForm.runToolStripMenuItem.DropDownItems.Add("(run-" + (run + 1).ToString() + ") " + (f + 1).ToString()); });
                    this.progressForm.Invoke((MethodInvoker)delegate { this.progressForm.runToolStripMenuItem.DropDownItems.Add(dictKey.ToString()); });

                    // Printout on console.
                    string time = Microsoft.VisualBasic.DateAndTime.Now.ToString("MM/dd/yyyy HH:mm");
                    this.consoleForm.Invoke((MethodInvoker)delegate { this.consoleForm.printoutTextBox.AppendText("\r\n-----------------------------------------------------------------\r\n"); });
                    this.consoleForm.Invoke((MethodInvoker)delegate { this.consoleForm.printoutTextBox.AppendText("\r\n[" + time + "] FOLD: " + f.ToString() + "\r\n"); });

                    // Initialization of model.
                    allFoldModelsInThisRun.Add(new ABCP(this.formParameters, this.formData, this.formData.trainFeaturesFolds[f], this.formData.testFeaturesFolds[f], this, this.consoleForm.printoutTextBox));

                    // Search for best model.
                    allFoldModelsInThisRun[f].qsABCP(this, this.consoleForm.printoutTextBox, dictKey, this.progressBar);

                    // Input for the printout to console.
                    string input = "";

                    #region find best in this fold and update best in all folds if better
                    // Best indices of the solutions in the model.
                    int indBestTrain = allFoldModelsInThisRun[f].bestTrainIndex;

                    // If it's first fold, there is no bestInFolds yet.
                    if (f == 0)
                    {
                        // Set best in folds.
                        bestInFolds = (Chromosome)allFoldModelsInThisRun[f].population[indBestTrain].Clone();

                        // Printout for the console.
                        time = Microsoft.VisualBasic.DateAndTime.Now.ToString("MM/dd/yyyy HH:mm");
                        input = "[" + time + "] {" + f.ToString() + "} BEST: \r\n SymbolicTree:  " + bestInFolds.symbolicTree.ToStringInorder();
                        input += "\r\n train fitness:  " + bestInFolds.trainFitness.ToString();
                        input += "\r\n test fitness:  " + bestInFolds.testFitness.ToString();
                        input += "\r\n (train+test) fitness:  " + ((double)(bestInFolds.trainFitness + bestInFolds.testFitness) / 2).ToString();
                    }
                    else
                    {
                        // Update best in folds if best in this fold is better.
                        if (allFoldModelsInThisRun[f].population[indBestTrain].trainFitness > bestInFolds.trainFitness)
                        {
                            // Update best solution in folds.
                            bestInFolds = (Chromosome)allFoldModelsInThisRun[f].population[indBestTrain].Clone();

                            // Update index of best fold.
                            bestFoldIndex = f;
                        }

                        // Printout for the console.
                        time = Microsoft.VisualBasic.DateAndTime.Now.ToString("MM/dd/yyyy HH:mm");
                        input = "[" + time + "] {" + f.ToString() + "} BEST: \r\n SymbolicTree:  " + allFoldModelsInThisRun[f].population[indBestTrain].symbolicTree.ToStringInorder();
                        input += "\r\n train fitness:  " + allFoldModelsInThisRun[f].population[indBestTrain].trainFitness.ToString();
                        input += "\r\n test fitness:  " + allFoldModelsInThisRun[f].population[indBestTrain].testFitness.ToString();
                        input += "\r\n (train+test) fitness:  " + ((double)(allFoldModelsInThisRun[f].population[indBestTrain].trainFitness + allFoldModelsInThisRun[f].population[indBestTrain].testFitness) / 2).ToString();
                    }
                    #endregion

                    // Printout on console.
                    this.consoleForm.Invoke((MethodInvoker)delegate { this.consoleForm.printoutTextBox.AppendText(input); });
                    this.consoleForm.Invoke((MethodInvoker)delegate { this.consoleForm.printoutTextBox.AppendText("\r\n-----------------------------------------------------------------\r\n"); });
                }
                #endregion

                // Printout for the console.
                string timeDone = Microsoft.VisualBasic.DateAndTime.Now.ToString("MM/dd/yyyy HH:mm");
                string inputDone = "[" + timeDone + "] FOLDS DONE! BEST (in all folds): \r\n SymbolicTree:  " + bestInFolds.symbolicTree.ToStringInorder();
                inputDone += "\r\n train fitness:  " + bestInFolds.trainFitness.ToString();
                inputDone += "\r\n test fitness:  " + bestInFolds.testFitness.ToString();
                inputDone += "\r\n (train+test) fitness:  " + ((double)(bestInFolds.trainFitness + bestInFolds.testFitness) / 2).ToString() + "\r\n\r\n";
                this.consoleForm.Invoke((MethodInvoker)delegate { this.consoleForm.printoutTextBox.AppendText(inputDone); });

                #region update folds accuracy

                // First, calculate (real) fold accuracy.
                double sum_of_fitness = 0;
                for (var f = 0; f < this.formData.numberOfFolds; f++)
                {
                    int indBestTrain = allFoldModelsInThisRun[f].bestTrainIndex;

                    sum_of_fitness += allFoldModelsInThisRun[f].population[indBestTrain].trainFitness;
                }
                sum_of_fitness = (double)(sum_of_fitness / (double)this.formData.numberOfFolds);

                // Set up that accuracy value.
                for (var f = 0; f < this.formData.numberOfFolds; f++)
                {
                    int indBest = allFoldModelsInThisRun[f].bestIndex;
                    int indBestTrain = allFoldModelsInThisRun[f].bestTrainIndex;
                    int indBestTest = allFoldModelsInThisRun[f].bestTestIndex;

                    // Only these updates are needed.
                    allFoldModelsInThisRun[f].population[indBest].realFoldAccuraccy = sum_of_fitness;
                    allFoldModelsInThisRun[f].population[indBestTrain].realFoldAccuraccy = sum_of_fitness;
                    allFoldModelsInThisRun[f].population[indBestTest].realFoldAccuraccy = sum_of_fitness;
                }
                #endregion

                // Add best model to allModelsInForm variable.
                this.allModelsInForm.Add(allFoldModelsInThisRun[bestFoldIndex]);

                retVal = bestFoldIndex;
            }

            #region update best solution in form if this run improved it
            int iBest = this.allModelsInForm[run].bestTrainIndex;

            // Update best solution in this search.
            if (this.bestModelInForm == null)
            {
                this.bestModelInForm = new Chromosome();
                this.bestModelInForm = (Chromosome)this.allModelsInForm[run].population[iBest].Clone();
            }
            else if (this.bestModelInForm.trainFitness < this.allModelsInForm[run].population[iBest].trainFitness)
            {
                this.bestModelInForm = (Chromosome)this.allModelsInForm[run].population[iBest].Clone();
            }

            return retVal;
            #endregion
        }
        #endregion

        #endregion

        #region Changed visibility event.

        #region console
        /// <summary>
        /// Make lookup sign white after "exiting" console.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConsoleForm_VisibilityChanged(object sender, EventArgs e)
        {
            if (this.consoleForm.Visible == false)
            {
                this.lookupConsoleFormSign.BackgroundImage = antico.Properties.Resources.console_lookup;
            }
        }
        #endregion

        #region progress form
        /// <summary>
        /// Make lookup sign white after "exiting" live chart.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProgressForm_VisibilityChanged(object sender, EventArgs e)
        {
            if (this.progressForm.Visible == false)
            {
                this.showSolutionsProgressSign.BackgroundImage = antico.Properties.Resources.progress_chart;
            }

        }
        #endregion

        #region waitingAnimation (model done)
        /// <summary>
        /// Notify user about end of model search.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WaitingAnimation_VisibilityChanged(object sender, EventArgs e)
        {
            // If waitingAnimation is now hiden (-> process is finished), notify user that search of the model is finished.
            if (!this.waitingAnimation.Visible)
            {
                this.Invoke((MethodInvoker)delegate { this.waitingAnimation.Visible = false; });

                // Notify user that model is created.
                CustomDialogBox dialog = new CustomDialogBox("Model created", "Model is created.", global::antico.Properties.Resources.information, MessageBoxButtons.OK);
                dialog.ShowDialog();

                this.PrintoutAllSolutions();

                // Show visualize and save signs.
                this.Invoke((MethodInvoker)delegate { this.saveSign.Visible = true; });
                this.Invoke((MethodInvoker)delegate { this.visualizeSign.Visible = true; });

                // Hide progress bar.
                this.Invoke((MethodInvoker)delegate { this.progressBar.Visible = false; });
            }
        }
        #endregion

        #endregion

        #endregion
    }
    #endregion
}
