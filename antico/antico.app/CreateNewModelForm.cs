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
    ///     - other forms (main form,helper form for showing tree graph, console form for printing out while preforming ABCP)
    ///     - variables for moving the form (flag dragging, locations dragCursorPoint and dragCursorPoint)
    ///     - variables needed for preforming ABCP
    ///         - allModelsInForm (best models from each run) - reseting after every change of parameters/data and every new "click" on "start" sign
    ///         - bestModelInForm (best solution of all runs) - reseting after every change of parameters/data and every new "click" on "start" sign
    ///         - formParameters (class Parameters)
    ///         - formData (class Data)
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
        #endregion

        #region Forbid clicking on form.
        // Variable that is used to forbid user clicking on signs.
        bool forbid = false;
        #endregion

        #region Thread.
        // For multitasking while searching for the model.
        Thread myThread;
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
        /// Function for initializing components on frame.
        /// </summary>
        public CreateNewModelForm(MainFrame mainFrame)
        {
            InitializeComponent();
            Application.DoEvents();
            // Set location of the frame.
            this.Location = mainFrame.DesktopLocation;

            // Load text font.
            PrivateFontCollection anticoFont = new PrivateFontCollection();
            anticoFont.AddFontFile("../../../../[FONTS]/UnicaOne-Regular.ttf");
            anticoLabel.Font = new Font(anticoFont.Families[0], 35, System.Drawing.FontStyle.Regular);

            // Save sent variables.
            this.mainForm = mainFrame;
            this.allModelsInForm = new List<ABCP>();

            // Set up background of the label for the printout of the solutions.
            this.printoutOfAllSolutionsLabel.BackColor = System.Drawing.Color.FromArgb(100, System.Drawing.Color.WhiteSmoke);

            // Initialize the console form.
            this.consoleForm = new ConsoleForm();
            this.consoleForm.CreateNewConsoleForm(this);
            this.consoleForm.Show();
            this.consoleForm.Visible = false;
            this.consoleForm.VisibleChanged += new EventHandler(this.ConsoleForm_VisibilityChanged);

            // Trigger on waitingAnimation visibilitx changed. (For end of model search.)
            this.waitingAnimation.VisibleChanged += new EventHandler(this.WaitingAnimation_VisibilityChanged);
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

        #region HOVERING

        #region Making hand cursor when hovering sign.

        #region go back sign
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

        #region set parameters sign
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

        #region start sign
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

        #region visualize sign
        /// <summary>
        /// Hovering start if there model exists.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VisualizeSign_MouseEnter(object sender, EventArgs e)
        {
            if (this.allModelsInForm != null && this.bestModelInForm != null)
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

        #region save sign
        /// <summary>
        /// Hovering start if there model exists.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveSign_MouseEnter(object sender, EventArgs e)
        {
            if (this.allModelsInForm != null && this.bestModelInForm != null)
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

        #region save parameters sign
        /// <summary>
        /// Hovering starts.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveParametersSign_MouseEnter(object sender, EventArgs e)
        {
            this.saveParametersSign.Cursor = Cursors.Hand;
        }

        /// <summary>
        /// Hovering end.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveParametersSign_MouseLeave(object sender, EventArgs e)
        {
            this.saveParametersSign.Cursor = Cursors.Default;
        }
        #endregion

        #region lookup console form
        /// <summary>
        /// Hovering start.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LookupConsoleFormPictureBox_MouseEnter(object sender, EventArgs e)
        {
            // Show hand cursor only if search is ongoing or done. 
            // Also, check out if console is already opened.
            if (!this.consoleForm.Visible && (this.forbid || this.bestModelInForm != null))
            {
                this.lookupConsoleFormPictureBox.Cursor = Cursors.Hand;
            }
        }

        /// <summary>
        /// Hovering end.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LookupConsoleFormPictureBox_MouseLeave(object sender, EventArgs e)
        {
            this.lookupConsoleFormPictureBox.Cursor = Cursors.Default;
        }
        #endregion

        #endregion

        #region Showing ToolTip when hovering sign.

        #region go back sign
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

        #region set parameters sign
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

        #region start sign
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

        #region visualize sign
        /// <summary>
        /// Show that pressing visualizeSign means showing tree structure of the best model.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VisualizeSign_MouseHover(object sender, EventArgs e)
        {
            ToolTip tt = new ToolTip();
            tt.SetToolTip(this.visualizeSign, "visualize best model");
        }
        #endregion

        #region save sign
        /// <summary>
        /// Show that pressing saveSign means saving the model.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveSign_MouseHover(object sender, EventArgs e)
        {
            ToolTip tt = new ToolTip();
            tt.SetToolTip(this.saveSign, "save");
        }
        #endregion

        #region save parameters sign
        /// <summary>
        /// Show that pressing saveParametersSign means saving the custom parameters.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveParametersSign_MouseHover(object sender, EventArgs e)
        {
            ToolTip tt = new ToolTip();
            tt.SetToolTip(this.saveParametersSign, "save custom parameters \nand start creating new model");
        }
        #endregion

        #region lookup console form
        /// <summary>
        /// Show that pressing Lookup means showing printouts from the search.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LookupConsoleFormPictureBox_MouseHover(object sender, EventArgs e)
        {
            // Show tool tip only if search is ongoing or done. 
            // Also, check out if console is already opened.
            if (!this.consoleForm.Visible && (this.forbid || this.bestModelInForm != null))
            {
                ToolTip tt = new ToolTip();
                tt.SetToolTip(this.lookupConsoleFormPictureBox, "show console");
            }

        }
        #endregion

        #endregion

        #endregion

        #region CLICK

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
                DialogResult result = MessageBox.Show(message, "Stop search?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

                // If canceled, do nothing.
                if (result == DialogResult.No)
                    return;
            }
            #endregion

            // Abort the process if alive.
            if (this.myThread != null)
            {
                this.myThread.Abort();
            }

            // Close the console form.
            this.consoleForm.Close();

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
                string t = "Warning!";
                MessageBox.Show(m, t, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            #endregion

            // Just in case, remove text box for printout.
            this.printoutOfAllSolutionsLabel.Visible = false;

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
                MessageBox.Show(m, "Wait!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            #endregion

            // Just in case, hide custom parameters layout.
            this.mainLayout.Visible = false;
            // Just in case, hide label with printouts.
            this.printoutOfAllSolutionsLabel.Visible = false;

            // Check if there are custom parameters.
            if (this.formParameters == null) 
            {
                string message = "Do you want add custom ABCP parameters?";
                string title = "Artificial bee colony programming parameters";
                DialogResult result = MessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                // If user wants to add custom parameters. 
                if (result == DialogResult.Yes)
                {
                    this.ShowParameters();
                }
                else
                {
                    // Do not add custom parameters.

                    // Forbid all other cliks.
                    this.forbid = true;

                    #region console form
                    // Just in case, clear textbox in console form.
                    this.consoleForm.printoutTextBox.Text = "";
                    // Show form.
                    this.consoleForm.Visible = true;
                    #endregion

                    #region search for model
                    // Initialize and printout when done using threads because of animation.
                    this.myThread = new Thread(
                        () =>
                        {
                            try
                            {
                                this.Invoke((MethodInvoker)delegate { this.waitingAnimation.Visible = true; });

                                // Reset class ABCP variables.
                                this.formParameters = new Parameters();
                                this.formData = new Data();
                                this.allModelsInForm = new List<ABCP>();
                                this.bestModelInForm = new Chromosome();

                                // Do the jobs.
                                for (var run = 0; run < this.formParameters.numberOfRuns; run++)
                                {
                                    // Printout on console.
                                    string time = Microsoft.VisualBasic.DateAndTime.Now.ToString("MM/dd/yyyy HH:mm");
                                    this.consoleForm.Invoke((MethodInvoker)delegate { this.consoleForm.printoutTextBox.AppendText("\r\n[" + time + "] RUN: " + run.ToString() + "\r\n"); });

                                    this.PreformABCP(run);

                                    // Printout on console.
                                    this.Invoke((MethodInvoker)delegate { this.consoleForm.printoutTextBox.AppendText("\r\n********************************************************\r\n"); });
                                }

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
                    #endregion
                }
                return;
            }

            // Check if form Data is set. (Since Parameters are set.)
            if (this.formData == null)
                throw new Exception("[StartSign_MouseClick] Form Data is not initialized.");

            // Forbid all other cliks.
            this.forbid = true;

            #region console form
            // Just in case, clear textbox in console form.
            this.consoleForm.printoutTextBox.Text = "";
            // Show form.
            this.consoleForm.Visible = true;
            #endregion

            #region search for model
            // User already added custom parameters.
            // Initialize and printout when done using threads because of animation.
            this.myThread = new Thread(
                () =>
                {
                    try
                    {
                        this.Invoke((MethodInvoker)delegate { this.waitingAnimation.Visible = true; });

                        // Reset the class ABCP variables.
                        this.allModelsInForm = new List<ABCP>();
                        this.bestModelInForm = new Chromosome();

                        // Do the jobs.
                        for (var run = 0; run < this.formParameters.numberOfRuns; run++)
                        {
                            // Printout on console.
                            string time = Microsoft.VisualBasic.DateAndTime.Now.ToString("MM/dd/yyyy HH:mm");
                            this.consoleForm.Invoke((MethodInvoker)delegate { this.consoleForm.printoutTextBox.AppendText("\r\n[" + time + "] RUN: " + run.ToString() + "\r\n"); });

                            this.PreformABCP(run);

                            // Printout on console.
                            this.Invoke((MethodInvoker)delegate { this.consoleForm.printoutTextBox.AppendText("\r\n********************************************************\r\n"); });
                        }

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
            #region check if another process is already running
            // Check if another process already runnning.
            if (this.forbid)
            {
                string m = "Wait! Model calculation is still in progress.";
                MessageBox.Show(m, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            #endregion

            // Check if model exists.
            if (this.allModelsInForm != null && this.bestModelInForm != null)
            {
                // Create a new helper form.
                this.formForVisualizationOfModel = new HelperForm();
                this.formForVisualizationOfModel.Text = "Model";
                this.formForVisualizationOfModel.WindowState = FormWindowState.Maximized;
                this.formForVisualizationOfModel.exitSign.Location = new System.Drawing.Point(System.Windows.Forms.SystemInformation.PrimaryMonitorMaximizedWindowSize.Width - 41, 6);

                #region graph
                // Create a graph object.
                Graph graph = new Graph("Model");
                // Create the graph content from model.
                this.DrawSymbolicTree(this.bestModelInForm.symbolicTree, ref graph);
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

            string warning_message = "Model is not yet created!";
            MessageBox.Show(warning_message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            #region check if another process is already running
            // Check if another process already runnning.
            if (this.forbid)
            {
                string m = "Wait! Model calculation is still in progress.";
                MessageBox.Show(m, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            #endregion

            // Check if model exists.
            if (this.allModelsInForm != null && this.bestModelInForm != null)
            {
                // User must input name of model.
                #region name input

                // Message, title and default value of input box.
                string message = "Please input the name of the file to be saved.";
                string title = "Name of the file";
                string defaultValue = "ABCP_Model_" + this.bestModelInForm.trainFitness.ToString() + "__" + DateTime.Today.Day.ToString() + "_" + DateTime.Today.Month.ToString() + "_" + DateTime.Today.Year.ToString();

                // Input box.
                var inputBox = Microsoft.VisualBasic.Interaction.InputBox(message, title, defaultValue);

                // Check if user clicked Cancel or OK.
                if (inputBox.ToString() == "")
                {
                    // User has clicked cancel. Do nothing.
                    return;
                }
                #endregion

                // Create a hashtable of values that will eventually be serialized.
                Hashtable addresses = new Hashtable();
                addresses.Add("BestModel", this.bestModelInForm);
                addresses.Add("Parameters", this.formParameters);

                // Name to be. Path to be.
                string fileName = @"../../../../[DATA]/saved/" + inputBox.ToString() + ".dat";
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
                MessageBox.Show(success_message, "Saving successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Model does not exist. Warn user!
            string warning_message = "Model is not yet created!";
            MessageBox.Show(warning_message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        #endregion

        #region save parameters and start abcp
        /// <summary>
        /// Saving the parameters and starting abcp.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveParametersSign_Click(object sender, EventArgs e)
        {
            #region check if another process is already running
            // Check if another process already runnning.
            if (this.forbid)
            {
                string m = "Wait! Model calculation is still in progress.";
                MessageBox.Show(m, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            #endregion

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
                MessageBox.Show("Please choose database!", "Choose database!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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

            // Just in case, remove text box for printout.
            this.printoutOfAllSolutionsLabel.Visible = false;

            // Hide parameters layout.
            this.mainLayout.Visible = false;

            // Set up parameters.
            this.formParameters = new Parameters(ps, maxnoi, maxnonii, nooruns, imd, md, method, lim, a, prob);

            // Set up data.
            this.formData = new Data(mathOp, database);

            // Forbid all other cliks.
            this.forbid = true;

            #region console form
            // Just in case, clear textbox in console form.
            this.consoleForm.printoutTextBox.Text = "";
            // Show form.
            this.consoleForm.Visible = true;
            #endregion

            // User already added custom parameters.
            // Initialize and printout when done using threads because of animation.
            this.myThread = new Thread(
                () =>
                {
                    try
                    {
                        this.Invoke((MethodInvoker)delegate { this.waitingAnimation.Visible = true; });

                        // Reset the variables.
                        this.allModelsInForm = new List<ABCP>();
                        this.bestModelInForm = new Chromosome();

                        #region do the jobs
                        // Do the jobs.
                        for (var run = 0; run < this.formParameters.numberOfRuns; run++)
                        {
                            // Printout on console.
                            string time = Microsoft.VisualBasic.DateAndTime.Now.ToString("MM/dd/yyyy HH:mm");
                            this.consoleForm.Invoke((MethodInvoker)delegate { this.consoleForm.printoutTextBox.AppendText("\r\n[" + time + "] RUN: " + run.ToString() + "\r\n"); });

                            this.PreformABCP(run);

                            // Printout on console.
                            this.Invoke((MethodInvoker)delegate { this.consoleForm.printoutTextBox.AppendText("\r\n********************************************************\r\n"); });
                        }
                        #endregion

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

        #region lookup console form
        /// <summary>
        /// Showing form on request.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LookupConsoleFormPictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            // Show console form only if search is ongoing or done. 
            // Also, check out if console is already opened.
            if (!this.consoleForm.Visible && (this.forbid || this.bestModelInForm != null))
            {
                // Show form.
                this.consoleForm.Visible = true;
                this.lookupConsoleFormPictureBox.Visible = false;
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
            System.Drawing.Color c = System.Drawing.ColorTranslator.FromHtml(this.depthColors[node.depth]);
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
            // Printout of all best solutions.
            for (var run = 0; run < this.formParameters.numberOfRuns; run++)
            {
                // Best train + test.
                string input = "\r\n BEST SOLUTION IN RUN:" + run + "\r\n SymbolicTree:  " + this.allModelsInForm[run].best.symbolicTree.ToStringInorder();
                input += "\r\n train fitness:  " + this.allModelsInForm[run].best.trainFitness.ToString();
                input += "\r\n test fitness:  " + this.allModelsInForm[run].best.testFitness.ToString();
                input += "\r\n (train+test) fitness:  " + ((double)(this.allModelsInForm[run].best.trainFitness + this.allModelsInForm[run].best.testFitness) / 2).ToString() + "\r\n";

                // Best train.
                input += "\r\n BEST(TRAIN) SOLUTION IN RUN:" + run + "\r\n SymbolicTree:  " + this.allModelsInForm[run].bestTrain.symbolicTree.ToStringInorder();
                input += "\r\n train fitness:  " + this.allModelsInForm[run].bestTrain.trainFitness.ToString();
                input += "\r\n test fitness:  " + this.allModelsInForm[run].bestTrain.testFitness.ToString();
                input += "\r\n (train+test) fitness:  " + ((double)(this.allModelsInForm[run].bestTrain.trainFitness + this.allModelsInForm[run].bestTrain.testFitness) / 2).ToString() + "\r\n";

                // Best test.
                input += "\r\n BEST(TEST) SOLUTION IN RUN:" + run + "\r\n SymbolicTree:  " + this.allModelsInForm[run].bestTest.symbolicTree.ToStringInorder();
                input += "\r\n train fitness:  " + this.allModelsInForm[run].bestTest.trainFitness.ToString();
                input += "\r\n test fitness:  " + this.allModelsInForm[run].bestTest.testFitness.ToString();
                input += "\r\n (train+test) fitness:  " + ((double)(this.allModelsInForm[run].bestTest.trainFitness + this.allModelsInForm[run].bestTest.testFitness) / 2).ToString() + "\r\n";

                // Add to label.
                this.Invoke((MethodInvoker)delegate { this.printoutOfAllSolutionsLabel.Text += input; });
            }
        }
        #endregion

        #region Show parameters to add on form.
        /// <summary>
        /// Show on form parameters that can be choosen.
        /// </summary>
        private void ShowParameters()
        {
            this.mainLayout.Visible = true;
        }
        #endregion

        #region Changed visibility of console form.
        /// <summary>
        /// Make lookup sign visible after "exiting" console.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConsoleForm_VisibilityChanged(object sender, EventArgs e)
        {
            if (this.consoleForm.Visible == false && this.allModelsInForm != null )
            {
                this.lookupConsoleFormPictureBox.Visible = true;
            }
        }
        #endregion

        #region Changed visibility of waitingAnimation.
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
                MessageBox.Show("Model is created.", "Model is created.", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Printout on label.
                string input = "\r\n BEST MODEL: \r\n SymbolicTree:  " + this.bestModelInForm.symbolicTree.ToStringInorder();
                input += "\r\n train fitness:  " + this.bestModelInForm.trainFitness.ToString();
                input += "\r\n test fitness:  " + this.bestModelInForm.testFitness.ToString();
                input += "\r\n (train+test) fitness:  " + ((double)(this.bestModelInForm.trainFitness + this.bestModelInForm.testFitness) / 2).ToString() + "\r\n\r\n";

                this.Invoke((MethodInvoker)delegate { this.printoutOfAllSolutionsLabel.Text = ""; });
                this.Invoke((MethodInvoker)delegate { this.printoutOfAllSolutionsLabel.Visible = true; });
                this.Invoke((MethodInvoker)delegate { this.printoutOfAllSolutionsLabel.Text += input; });
                PrintoutAllSolutions();
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
        private void PreformABCP(int run)
        {
            // Check if parameters and data are previously set.
            if (this.formData == null || this.formParameters == null)
                throw new Exception("[PreformABCP] Parameters/Data not set!");

            // Train model in this run depending on number of folds.
            if (this.formData.numberOfFolds == 0)
            {
                // Initialization of model.
                this.allModelsInForm.Add(new ABCP(this.formParameters, this.formData, this.formData.trainFeatures, this.formData.testFeatures, this, this.consoleForm.printoutTextBox));

                // Search for best model.
                this.allModelsInForm[run].ABCProgramming(this, this.consoleForm.printoutTextBox);
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
                    // Printout on console.
                    string time = Microsoft.VisualBasic.DateAndTime.Now.ToString("MM/dd/yyyy HH:mm");
                    this.consoleForm.Invoke((MethodInvoker)delegate { this.consoleForm.printoutTextBox.AppendText("\r\n-----------------------------------------------------------------\r\n"); });
                    this.consoleForm.Invoke((MethodInvoker)delegate { this.consoleForm.printoutTextBox.AppendText("\r\n[" + time + "] FOLD: " + f.ToString() + "\r\n"); });

                    // Initialization of model.
                    allFoldModelsInThisRun.Add(new ABCP(this.formParameters, this.formData, this.formData.trainFeaturesFolds[f], this.formData.testFeaturesFolds[f], this, this.consoleForm.printoutTextBox));

                    // Search for best model.
                    allFoldModelsInThisRun[f].ABCProgramming(this, this.consoleForm.printoutTextBox);

                    // Input for the printout to console.
                    string input = "";

                    #region find best in this fold and update best in all folds if better
                    // Fitness values. (of whole train dataset - in "normal" models (models with 0 folds) this is equivalent of trainFitness)
                    double realTrainFitnessBestSolution = ((double)(allFoldModelsInThisRun[f].best.trainFitness + allFoldModelsInThisRun[f].best.testFitness) / 2);
                    double realTrainFitnessBestTrainSolution = ((double)(allFoldModelsInThisRun[f].bestTrain.trainFitness + allFoldModelsInThisRun[f].bestTrain.testFitness) / 2);
                    double realTrainFitnessBestTestSolution = ((double)(allFoldModelsInThisRun[f].bestTest.trainFitness + allFoldModelsInThisRun[f].bestTest.testFitness) / 2);

                    double realTestFitnessBestSolution = allFoldModelsInThisRun[f].best.CalculateFitness(this.formData.testFeatures);
                    double realTestFitnessBestTrainSolution = allFoldModelsInThisRun[f].bestTrain.CalculateFitness(this.formData.testFeatures);
                    double realTestFitnessBestTestSolution = allFoldModelsInThisRun[f].bestTest.CalculateFitness(this.formData.testFeatures);

                    // If it's first fold, there is no bestInFolds yet.
                    if (f == 0)
                    {
                        // Assume 'best' has highest fitness on train dataset.
                        double highest = realTrainFitnessBestSolution + realTestFitnessBestSolution;

                        // Update best in folds.
                        bestInFolds = (Chromosome)allFoldModelsInThisRun[f].best.Clone();
                        bestInFolds.trainFitness = realTrainFitnessBestSolution;
                        bestInFolds.testFitness = realTestFitnessBestSolution;

                        // Check if 'bestTrain' is better.
                        if (realTrainFitnessBestTrainSolution + realTestFitnessBestTrainSolution > highest)
                        {
                            // Update highest.
                            highest = realTrainFitnessBestTrainSolution + realTestFitnessBestTrainSolution;

                            // Update best in folds.
                            bestInFolds = (Chromosome)allFoldModelsInThisRun[f].bestTrain.Clone();
                            bestInFolds.trainFitness = realTrainFitnessBestTrainSolution;
                            bestInFolds.testFitness = realTestFitnessBestTrainSolution;
                        }

                        // Check if 'bestTest' is better.
                        if (realTrainFitnessBestTestSolution + realTestFitnessBestTestSolution > highest)
                        {
                            // Update highest.
                            highest = realTrainFitnessBestTestSolution + realTestFitnessBestTestSolution;

                            // Update best in folds.
                            bestInFolds = (Chromosome)allFoldModelsInThisRun[f].bestTest.Clone();
                            bestInFolds.trainFitness = realTrainFitnessBestTestSolution;
                            bestInFolds.testFitness = realTestFitnessBestTestSolution;
                        }

                        // Printout for the console.
                        time = Microsoft.VisualBasic.DateAndTime.Now.ToString("MM/dd/yyyy HH:mm");
                        input = "[" + time + "] {" + f.ToString() + "} BEST: \r\n SymbolicTree:  " + bestInFolds.symbolicTree.ToStringInorder();
                        input += "\r\n train fitness:  " + bestInFolds.trainFitness.ToString();
                        input += "\r\n test fitness:  " + bestInFolds.testFitness.ToString();
                        input += "\r\n (train+test) fitness:  " + ((double)(bestInFolds.trainFitness + bestInFolds.testFitness) / 2).ToString();
                    }
                    else
                    {
                        // Assume 'best' has highest fitness on train dataset.
                        double highest = realTrainFitnessBestSolution + realTestFitnessBestSolution;
                        Chromosome bestInThisFold = (Chromosome)allFoldModelsInThisRun[f].best.Clone();

                        // Check if 'bestTrain' is better.
                        if (realTrainFitnessBestTrainSolution + realTestFitnessBestTrainSolution > highest)
                        {
                            // Update highest.
                            highest = realTrainFitnessBestTrainSolution + realTestFitnessBestTrainSolution;

                            // Update best in this fold.
                            bestInThisFold = (Chromosome)allFoldModelsInThisRun[f].bestTrain.Clone();
                            bestInThisFold.trainFitness = realTrainFitnessBestTrainSolution;
                            bestInThisFold.testFitness = realTestFitnessBestTrainSolution;
                        }

                        // Check if 'bestTest' is better.
                        if (realTrainFitnessBestTestSolution + realTestFitnessBestTestSolution > highest)
                        {
                            // Update highest.
                            highest = realTrainFitnessBestTestSolution + realTestFitnessBestTestSolution;

                            // Update best in this fold.
                            bestInThisFold = (Chromosome)allFoldModelsInThisRun[f].bestTest.Clone();
                            bestInThisFold.trainFitness = realTrainFitnessBestTestSolution;
                            bestInThisFold.testFitness = realTestFitnessBestTestSolution;
                        }

                        // Now chck if highest fitness in this fold is better than overall fitness in all folds.
                        if (highest > (bestInFolds.trainFitness + bestInFolds.testFitness) )
                        {
                            // Update best solution in folds.
                            bestInFolds = (Chromosome)bestInThisFold.Clone();

                            // Update index of best fold.
                            bestFoldIndex = f;
                        }

                        // Printout for the console.
                        time = Microsoft.VisualBasic.DateAndTime.Now.ToString("MM/dd/yyyy HH:mm");
                        input = "[" + time + "] {" + f.ToString() + "} BEST: \r\n SymbolicTree:  " + bestInThisFold.symbolicTree.ToStringInorder();
                        input += "\r\n train fitness:  " + bestInThisFold.trainFitness.ToString();
                        input += "\r\n test fitness:  " + bestInThisFold.testFitness.ToString();
                        input += "\r\n (train+test) fitness:  " + ((double)(bestInThisFold.trainFitness + bestInThisFold.testFitness) / 2).ToString();
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

                #region update trainFitness and testFitness of all chromosomes in choosen model
                // Update trainFitness and testFitness in population in choosen model before adding it in allModelsInForm.

                // Best Solution.
                allFoldModelsInThisRun[bestFoldIndex].best.trainFitness = (double)(((double)(allFoldModelsInThisRun[bestFoldIndex].best.trainFitness + allFoldModelsInThisRun[bestFoldIndex].best.testFitness)) / 2);
                allFoldModelsInThisRun[bestFoldIndex].best.testFitness = allFoldModelsInThisRun[bestFoldIndex].best.CalculateFitness(this.formData.testFeatures);

                // Best Train Solution.
                allFoldModelsInThisRun[bestFoldIndex].bestTrain.trainFitness = (double)(((double)(allFoldModelsInThisRun[bestFoldIndex].bestTrain.trainFitness + allFoldModelsInThisRun[bestFoldIndex].bestTrain.testFitness)) / 2);
                allFoldModelsInThisRun[bestFoldIndex].bestTrain.testFitness = allFoldModelsInThisRun[bestFoldIndex].bestTrain.CalculateFitness(this.formData.testFeatures);

                // Best Test Solution.
                allFoldModelsInThisRun[bestFoldIndex].bestTest.trainFitness = (double)(((double)(allFoldModelsInThisRun[bestFoldIndex].bestTest.trainFitness + allFoldModelsInThisRun[bestFoldIndex].bestTest.testFitness)) / 2);
                allFoldModelsInThisRun[bestFoldIndex].bestTest.testFitness = allFoldModelsInThisRun[bestFoldIndex].bestTest.CalculateFitness(this.formData.testFeatures);

                // Whole population.
                for (var s = 0; s < allFoldModelsInThisRun[bestFoldIndex].population.populationSize; s++)
                {
                    allFoldModelsInThisRun[bestFoldIndex].population[s].trainFitness = (double)(((double)(allFoldModelsInThisRun[bestFoldIndex].population[s].trainFitness + allFoldModelsInThisRun[bestFoldIndex].population[s].testFitness)) / 2);
                    allFoldModelsInThisRun[bestFoldIndex].population[s].testFitness = allFoldModelsInThisRun[bestFoldIndex].population[s].CalculateFitness(this.formData.testFeatures);
                }

                allFoldModelsInThisRun[bestFoldIndex].train = this.formData.trainFeatures;
                allFoldModelsInThisRun[bestFoldIndex].test = this.formData.testFeatures;
                #endregion

                // Add best model to allModelsInForm variable.
                this.allModelsInForm.Add(allFoldModelsInThisRun[bestFoldIndex]);
            }

            #region update best solution in form if this run improved it
            // Update best solution in this search.
            if (this.bestModelInForm == null)
            {
                this.bestModelInForm = new Chromosome();
                this.bestModelInForm = (Chromosome)this.allModelsInForm[run].best.Clone();
            }
            else if (this.bestModelInForm.trainFitness + this.bestModelInForm.testFitness < this.allModelsInForm[run].best.trainFitness + this.allModelsInForm[run].best.testFitness)
            {
                this.bestModelInForm = (Chromosome)this.allModelsInForm[run].best.Clone();
            }
            #endregion
        }
        #endregion

        #endregion

        #endregion

    }
    #endregion
}
