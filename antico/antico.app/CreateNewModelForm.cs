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
using antico.abcp;
using antico.data;
using Microsoft.Msagl.Drawing;
using System.Threading;
using System.IO;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using Microsoft.VisualBasic;

namespace antico
{
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
        private List<ABCP> _models;

        // Variable for saving custom parameters.
        private Parameters _parameters;

        // Variable for saving current best model.
        private Chromosome _best;
        #endregion

        #region Data.
        // Data variable.
        Data _data;
        #endregion

        #region Other forms.
        // Helper form for showing symbolic tree structure of a (best) model.
        private HelperForm formForVisualizationOfModel;
        // Main frame.
        private MainFrame _mainFrame;
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
            _mainFrame = mainFrame;
            _models = new List<ABCP>();

            // Set up background of the label for the printout of the solutions.
            this.printoutOfAllSolutionsLabel.BackColor = System.Drawing.Color.FromArgb(100, System.Drawing.Color.WhiteSmoke);

            // Initialize the console form.
            consoleForm = new ConsoleForm();
            consoleForm.CreateNewConsoleForm(this);
            consoleForm.Show();
            consoleForm.Visible = false;
            consoleForm.VisibleChanged += new EventHandler(this.ConsoleForm_VisibilityChanged);

            // Trigger on waitingAnimation visibilitx changed. (For end of model search.)
            waitingAnimation.VisibleChanged += new EventHandler(this.WaitingAnimation_VisibilityChanged);
        }

        #endregion

        #region Methods for enabling moving CreateNewModelForm on user screen.
        /// <summary>
        /// Flag the wariable dragging true since user pressed mouse button initiating beggining of moving frame.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateNewModelForm_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = this.Location;
        }

        /// <summary>
        /// Flag the wariable dragging true since user pressed mouse button initiating beggining of moving frame.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panelForPrintoutLabel_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = this.Location;
        }

        /// <summary>
        /// Flag the wariable dragging true since user pressed mouse button initiating beggining of moving frame.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void printoutOfAllSolutionsLabel_MouseDown(object sender, MouseEventArgs e)
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
        private void CreateNewModelForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(dif));
            }
        }

        /// <summary>
        /// If user previously pressed mouse button (if dragging is true), change location of the Form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panelForPrintoutLabel_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(dif));
            }
        }

        /// <summary>
        /// If user previously pressed mouse button (if dragging is true), change location of the Form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void printoutOfAllSolutionsLabel_MouseMove(object sender, MouseEventArgs e)
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
        private void CreateNewModelForm_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        /// <summary>
        /// Flag the wariable dragging false since user pressed mouse button initiating end of moving frame.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panelForPrintoutLabel_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        /// <summary>
        /// Flag the wariable dragging false since user pressed mouse button initiating end of moving frame.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void printoutOfAllSolutionsLabel_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }
        #endregion

        #region HOVERING

        #region Making hand cursor when hovering sign.

        #region go back sign
        /// <summary>
        /// Hovering start.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void goBackSign_MouseEnter(object sender, EventArgs e)
        {
            goBackSign.Cursor = Cursors.Hand;
        }

        /// <summary>
        /// Hovering end.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void goBackSign_MouseLeave(object sender, EventArgs e)
        {
            goBackSign.Cursor = Cursors.Default;
        }
        #endregion

        #region set parameters sign
        /// <summary>
        /// Hovering start.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void parametersSettingsSign_MouseEnter(object sender, EventArgs e)
        {
            parametersSettingsSign.Cursor = Cursors.Hand;
        }

        /// <summary>
        /// Hovering end.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void parametersSettingsSign_MouseLeave(object sender, EventArgs e)
        {
            parametersSettingsSign.Cursor = Cursors.Hand;
        }
        #endregion

        #region start sign
        /// <summary>
        /// Hovering start.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void startSign_MouseEnter(object sender, EventArgs e)
        {
            startSign.Cursor = Cursors.Hand;
        }

        /// <summary>
        /// Hovering end.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void startSign_MouseLeave(object sender, EventArgs e)
        {
            startSign.Cursor = Cursors.Default;
        }
        #endregion

        #region visualize sign
        /// <summary>
        /// Hovering start if there model exists.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void visualizeSign_MouseEnter(object sender, EventArgs e)
        {
            if (_models != null && _best != null)
            {
                visualizeSign.Cursor = Cursors.Hand;
            }
        }

        /// <summary>
        /// Hovering end.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void visualizeSign_MouseLeave(object sender, EventArgs e)
        {
            visualizeSign.Cursor = Cursors.Default;
        }
        #endregion

        #region save sign
        /// <summary>
        /// Hovering start if there model exists.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveSign_MouseEnter(object sender, EventArgs e)
        {
            if (_models != null && _best != null)
            {
                saveSign.Cursor = Cursors.Hand;
            }
        }

        /// <summary>
        /// Hovering end.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveSign_MouseLeave(object sender, EventArgs e)
        {
            saveSign.Cursor = Cursors.Default;
        }
        #endregion

        #region save parameters sign
        /// <summary>
        /// Hovering starts.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveParametersSign_MouseEnter(object sender, EventArgs e)
        {
            saveParametersSign.Cursor = Cursors.Hand;
        }

        /// <summary>
        /// Hovering end.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveParametersSign_MouseLeave(object sender, EventArgs e)
        {
            saveParametersSign.Cursor = Cursors.Default;
        }
        #endregion

        #region lookup console form
        /// <summary>
        /// Hovering start.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lookupConsoleFormPictureBox_MouseEnter(object sender, EventArgs e)
        {
            // Show hand cursor only if search is ongoing or done. 
            // Also, check out if console is already opened.
            if (!consoleForm.Visible && (forbid || _best != null))
            {
                lookupConsoleFormPictureBox.Cursor = Cursors.Hand;
            }
        }

        /// <summary>
        /// Hovering end.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lookupConsoleFormPictureBox_MouseLeave(object sender, EventArgs e)
        {
            lookupConsoleFormPictureBox.Cursor = Cursors.Default;
        }
        #endregion

        #endregion

        #region Showing ToolTip when hovering sign.

        #region go back sign
        /// <summary>
        /// Show that pressing GoBack means going back to the main application.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void goBackSign_MouseHover(object sender, EventArgs e)
        {
            ToolTip tt = new ToolTip();
            tt.SetToolTip(this.goBackSign, "go back");
        }
        #endregion

        #region set parameters sign
        /// <summary>
        /// Show that pressing parametersSettingsSign means showing text boxes for input of parameters.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void parametersSettingsSign_MouseHover(object sender, EventArgs e)
        {
            ToolTip tt = new ToolTip();
            tt.SetToolTip(this.parametersSettingsSign, "set custom parameters");
        }
        #endregion

        #region start sign
        /// <summary>
        /// Show that pressing startSign means starting the search for the model.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void startSign_MouseHover(object sender, EventArgs e)
        {
            ToolTip tt = new ToolTip();
            tt.SetToolTip(this.startSign, "start");
        }
        #endregion

        #region visualize sign
        /// <summary>
        /// Show that pressing visualizeSign means showing tree structure of the best model.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void visualizeSign_MouseHover(object sender, EventArgs e)
        {
            ToolTip tt = new ToolTip();
            tt.SetToolTip(this.visualizeSign, "visualize best model");
        }
        #endregion

        #region save sign
        /// <summary>
        /// Show that pressing saveSign means saving the model.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveSign_MouseHover(object sender, EventArgs e)
        {
            ToolTip tt = new ToolTip();
            tt.SetToolTip(this.saveSign, "save");
        }
        #endregion

        #region save parameters sign
        /// <summary>
        /// Show that pressing saveParametersSign means saving the custom parameters.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveParametersSign_MouseHover(object sender, EventArgs e)
        {
            ToolTip tt = new ToolTip();
            tt.SetToolTip(this.saveParametersSign, "save custom parameters \nand start creating new model");
        }
        #endregion

        #region lookup console form
        /// <summary>
        /// Show that pressing Lookup means showing printouts from the search.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lookupConsoleFormPictureBox_MouseHover(object sender, EventArgs e)
        {
            // Show tool tip only if search is ongoing or done. 
            // Also, check out if console is already opened.
            if (!consoleForm.Visible && (forbid || _best != null))
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void goBackSign_MouseClick(object sender, EventArgs e)
        {
            #region check if another process is already running
            // If process is ongoing, check if user really wants to stop it.
            if (forbid)
            {
                string message = "You want to stop the search for the model. Are you sure?";
                string title = "Stop search?";
                DialogResult result = MessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

                // If canceled, do nothing.
                if (result == DialogResult.No)
                {
                    return;
                }
            }
            #endregion

            // Abort the process if alive.
            if (myThread != null)
            {
                myThread.Abort();
            }

            // Close the console form.
            consoleForm.Close();

            // Close the form.
            this.Close();

            // Update new location of main frame.
            _mainFrame.Location = this.Location;
            // Make main form visible again.
            _mainFrame.Visible = true;

            // Load text font.
            PrivateFontCollection anticoFont = new PrivateFontCollection();
            anticoFont.AddFontFile("../../../../[FONTS]/UnicaOne-Regular.ttf");
            _mainFrame.anticoLabelDesign.Font = new Font(anticoFont.Families[0], 35, System.Drawing.FontStyle.Regular);
        }
        #endregion

        #region set parameters
        /// <summary>
        /// Showing panel for inputing custom parameters.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void parametersSettingsSign_MouseClick(object sender, MouseEventArgs e)
        {
            #region check if another process is already running
            // Check if another process already runnning.
            if (forbid)
            {
                string m = "Wait! Model calculation is still in progress.";
                string t = "Warning!";
                MessageBox.Show(m, t, MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }
            #endregion

            // Just in case, remove text box for printout.
            printoutOfAllSolutionsLabel.Visible = false;

            showParameters();
        }

        #endregion

        #region start 
        /// <summary>
        /// Creating the model. 
        /// Just in case, asks user if he wants to add custom parameters if already didn't.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void startSign_MouseClick(object sender, MouseEventArgs e)
        {

            #region check if another process is already running
            // Check if another process already runnning.
            if (forbid)
            {
                string m = "Wait! Model calculation is still in progress.";
                string t = "Warning!";
                MessageBox.Show(m, t, MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }
            #endregion

            // Just in case, hide custom parameters layout.
            mainLayout.Visible = false;
            // Just in case, hide label with printouts.
            printoutOfAllSolutionsLabel.Visible = false;

            // Check if there are custom parameters.
            if (_parameters == null)
            {
                string message = "Do you want add custom ABCP parameters?";
                string title = "Artificial bee colony programming parameters";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Question);

                // If user wants to add custom parameters. 
                if (result == DialogResult.Yes)
                {
                    showParameters();
                }
                else
                {
                    // Forbid all other cliks.
                    forbid = true;

                    #region console form
                    // Just in case, clear textbox in console form.
                    consoleForm.printoutTestBox.Text = "";
                    // Show form.
                    consoleForm.Visible = true;
                    #endregion

                    #region search for model
                    // Initialize and printout when done using threads because of animation.
                    myThread = new Thread(
                        () =>
                        {
                            try
                            {
                                this.Invoke((MethodInvoker)delegate { waitingAnimation.Visible = true; });

                                // Set up parameters
                                _parameters = new Parameters();
                                _models = new List<ABCP>();
                                _best = new Chromosome();

                                // Do the jobs.
                                for (var run = 0; run < _parameters.numberOfRuns; run++)
                                {
                                    // Printout on console.
                                    string time = Microsoft.VisualBasic.DateAndTime.Now.ToString("MM/dd/yyyy HH:mm");
                                    consoleForm.Invoke((MethodInvoker)delegate { consoleForm.printoutTestBox.AppendText("\r\n[" + time + "] RUN: " + run.ToString() + "\r\n"); });

                                    _models.Add(new ABCP(this, consoleForm.printoutTestBox));
                                    _models[run].ABCProgramming(this, consoleForm.printoutTestBox);

                                    if (_best == null)
                                    {
                                        _best = new Chromosome();
                                        _best = _models[run].best.Clone();
                                    }
                                    else if (_best.fitness < _models[run].best.fitness)
                                    {
                                        _best = _models[run].best.Clone();
                                    }

                                    // Printout on console.
                                    this.Invoke((MethodInvoker)delegate { consoleForm.printoutTestBox.AppendText("\r\n****************************************************************************************"); });
                                }

                                System.Threading.Thread.Sleep(5000);
                                this.Invoke((MethodInvoker)delegate { waitingAnimation.Visible = false; });
                            }
                            finally
                            {
                                // Back to normal.
                                forbid = false;
                            }

                        });
                    myThread.Start();
                    #endregion
                }
                return;
            }

            // Forbid all other cliks.
            forbid = true;

            #region console form
            // Just in case, clear textbox in console form.
            consoleForm.printoutTestBox.Text = "";
            // Show form.
            consoleForm.Visible = true;
            #endregion

            // User already added custom parameters.
            // Initialize and printout when done using threads because of animation.
            myThread = new Thread(
                () =>
                {
                    try
                    {
                        this.Invoke((MethodInvoker)delegate { waitingAnimation.Visible = true; });

                        // Reset the variables.
                        _models = new List<ABCP>();
                        _best = new Chromosome();

                        // Do the jobs.
                        for (var run = 0; run < _parameters.numberOfRuns; run++)
                        {
                            // Printout on console.
                            string time = Microsoft.VisualBasic.DateAndTime.Now.ToString("MM/dd/yyyy HH:mm");
                            consoleForm.Invoke((MethodInvoker)delegate { consoleForm.printoutTestBox.AppendText("\r\n[" + time + "] RUN: " + run.ToString() + "\r\n"); });

                            _models.Add(new ABCP(_parameters, _data, this, consoleForm.printoutTestBox));
                            _models[run].ABCProgramming(this, consoleForm.printoutTestBox);

                            if (_best == null)
                            {
                                _best = new Chromosome();
                                _best = _models[run].best.Clone();
                            }
                            else if (_best.fitness < _models[run].best.fitness)
                            {
                                _best = _models[run].best.Clone();
                            }

                            // Printout on console.
                            this.Invoke((MethodInvoker)delegate { consoleForm.printoutTestBox.AppendText("\r\n****************************************************************************************"); });
                        }

                        System.Threading.Thread.Sleep(5000);
                        this.Invoke((MethodInvoker)delegate { waitingAnimation.Visible = false; });
                    }
                    finally
                    {
                        // Back to normal.
                        forbid = false;

                    }
                });
            myThread.Start();

        }
        #endregion

        #region visualize
        /// <summary>
        /// Show symbolic tree structure of best model if exists.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void visualizeSign_MouseClick(object sender, MouseEventArgs e)
        {
            #region check if another process is already running
            // Check if another process already runnning.
            if (forbid)
            {
                string m = "Wait! Model calculation is still in progress.";
                string t = "Warning!";
                MessageBox.Show(m, t, MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }
            #endregion

            // Check if model exists.
            if (_models != null && _best != null)
            {
                // Create a new helper form.
                formForVisualizationOfModel = new HelperForm();
                formForVisualizationOfModel.Text = "Model";
                formForVisualizationOfModel.WindowState = FormWindowState.Maximized;
                formForVisualizationOfModel.exitSign.Location = new System.Drawing.Point(System.Windows.Forms.SystemInformation.PrimaryMonitorMaximizedWindowSize.Width - 41, 6);

                #region graph
                // Create a graph object.
                Graph graph = new Graph("Model");
                // Create the graph content from model.
                DrawSymbolicTree(_best.symbolicTree, ref graph);
                #endregion

                #region viewer 
                // Create a viewer object .
                Microsoft.Msagl.GraphViewerGdi.GViewer viewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();
                // Bind the graph to the viewer.
                viewer.Graph = graph;

                // Associate the viewer with the form.
                formForVisualizationOfModel.SuspendLayout();
                viewer.Dock = DockStyle.Fill;
                viewer.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Viewer_KeyUp);
                formForVisualizationOfModel.Controls.Add(viewer);
                formForVisualizationOfModel.ResumeLayout();
                #endregion

                // Show the form.
                formForVisualizationOfModel.ShowDialog();

                return;
            }

            string warning_message = "Model is not yet created!";
            string warning_title = "Warning";
            MessageBoxButtons warning_buttons = MessageBoxButtons.OK;
            DialogResult warning_result = MessageBox.Show(warning_message, warning_title, warning_buttons, MessageBoxIcon.Warning);
        }
        #endregion

        #region save
        /// <summary>
        /// Saving the best model if exists.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveSign_MouseClick(object sender, MouseEventArgs e)
        {
            #region check if another process is already running
            // Check if another process already runnning.
            if (forbid)
            {
                string m = "Wait! Model calculation is still in progress.";
                string t = "Warning!";
                MessageBox.Show(m, t, MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }
            #endregion

            // Check if model exists.
            if (_models != null && _best != null)
            {
                // User must input name of model.
                #region name input

                // Message, title and default value of input box.
                string message = "Please input the name of the file to be saved.";
                string title = "Name of the file";
                string defaultValue = "ABCP_Model_" + _best.fitness.ToString() + "__" + DateTime.Today.Day.ToString() + "_" + DateTime.Today.Month.ToString() + "_" + DateTime.Today.Year.ToString() ;

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
                addresses.Add("BestModel", _best);
                addresses.Add("Parameters", _parameters);

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
                string success_title = "Saving successful.";
                DialogResult result = MessageBox.Show(success_message, success_title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Model does not exist. Warn user!
            string warning_message = "Model is not yet created!";
            string warning_title = "Warning";
            MessageBoxButtons warning_buttons = MessageBoxButtons.OK;
            DialogResult warning_result = MessageBox.Show(warning_message, warning_title, warning_buttons, MessageBoxIcon.Warning);
        }
        #endregion

        #region save parameters and start abcp
        /// <summary>
        /// Saving the parameters and starting abcp.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveParametersSign_Click(object sender, EventArgs e)
        {
            #region check if another process is already running
            // Check if another process already runnning.
            if (forbid)
            {
                string m = "Wait! Model calculation is still in progress.";
                string t = "Warning!";
                MessageBox.Show(m, t, MessageBoxButtons.OK, MessageBoxIcon.Warning);

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

            if (growMethodRadioButton.Checked)
            {
                method = "grow";
            }
            else if (fullMethodRadioButton.Checked)
            {
                method = "full";
            }

            // Name of the database.
            string database;
            if (databaseComboBox.SelectedIndex > -1)
            {
                database = databaseComboBox.SelectedItem.ToString().ToLower();
            }
            else
            {
                MessageBox.Show("Please choose database!", "Choose database!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            // Mathematical operators.
            List<string> mathOp = new List<string>();

            if (plusSelected.Checked)
                mathOp.Add("+");
            if (minusSelected.Checked)
                mathOp.Add("-");
            if (divisionSelected.Checked)
                mathOp.Add("/");
            if (timesSelected.Checked)
                mathOp.Add("*");
            if (sinSelected.Checked)
                mathOp.Add("sin");
            if (cosSelected.Checked)
                mathOp.Add("cos");
            if (rlogSelected.Checked)
                mathOp.Add("rlog");
            if (expSelected.Checked)
                mathOp.Add("exp");
            #endregion

            // Just in case, remove text box for printout.
            printoutOfAllSolutionsLabel.Visible = false;

            // Hide parameters layout.
            mainLayout.Visible = false;

            // Set up parameters.
            _parameters = new Parameters(ps, maxnoi, maxnonii, nooruns, imd, md, method, lim, a, prob);

            // Set up data.
            _data = new Data(mathOp, database);

            // Forbid all other cliks.
            forbid = true;

            #region console form
            // Just in case, clear textbox in console form.
            consoleForm.printoutTestBox.Text = "";
            // Show form.
            consoleForm.Visible = true;
            #endregion

            // User already added custom parameters.
            // Initialize and printout when done using threads because of animation.
            myThread = new Thread(
                () =>
                {
                    try
                    {
                        this.Invoke((MethodInvoker)delegate { waitingAnimation.Visible = true; });

                        // Reset the variables.
                        _models = new List<ABCP>();
                        _best = new Chromosome();

                        #region do the jobs
                        // Do the jobs.
                        for (var run = 0; run < _parameters.numberOfRuns; run++)
                        {
                            // Printout on console.
                            string time = Microsoft.VisualBasic.DateAndTime.Now.ToString("MM/dd/yyyy HH:mm");
                            consoleForm.Invoke((MethodInvoker)delegate { consoleForm.printoutTestBox.AppendText("\r\n[" + time + "] RUN: " + run.ToString() + "\r\n"); });

                            _models.Add(new ABCP(_parameters, _data, this, consoleForm.printoutTestBox));
                            _models[run].ABCProgramming(this, consoleForm.printoutTestBox);

                            if (_best == null)
                            {
                                _best = new Chromosome();
                                _best = _models[run].best.Clone();
                            }
                            else if (_best.fitness < _models[run].best.fitness)
                            {
                                _best = _models[run].best.Clone();
                            }

                            // Printout on console.
                            this.Invoke((MethodInvoker)delegate { consoleForm.printoutTestBox.AppendText("\r\n****************************************************************************************"); });
                        }
                        #endregion

                        System.Threading.Thread.Sleep(5000);
                        this.Invoke((MethodInvoker)delegate { waitingAnimation.Visible = false; });
                    }
                    finally
                    {
                        // Back to normal.
                        forbid = false;

                    }
                });
            myThread.Start();

        }
        #endregion

        #region lookup console form
        /// <summary>
        /// Showing form on request.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lookupConsoleFormPictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            // Show console form only if search is ongoing or done. 
            // Also, check out if console is already opened.
            if (!consoleForm.Visible && (forbid || _best != null))
            {
                // Show form.
                consoleForm.Visible = true;
                lookupConsoleFormPictureBox.Visible = false;
            }
        }
        #endregion

        #endregion

        #region HELPER METHODS

        #region Event for "KeyUp" on viewer on helper form for visualisation of model.
        /// <summary>
        /// Closing Form for visualization of model.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Viewer_KeyUp(object sender, KeyEventArgs e)
        {
            // If Esc key is pressed, close the helper form.
            if (e.KeyCode == Keys.Escape)
            {
                formForVisualizationOfModel.Close();
            }
        }
        #endregion

        #region Draw symbolic tree.
        /// <summary>
        /// Recursive helper method for adding connections between nodes in symbolic regression tree (that represents a model).
        /// Also, coloring the nodes depending on depth of the node in tree.
        /// </summary>
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
            System.Drawing.Color c = System.Drawing.ColorTranslator.FromHtml(depthColors[node.depth]);
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
        /// <param name="solutions">All known solutions.</param>
        /// <param name="dt">Data for printing evaluation of first row. - TESTING PHASE - TODO this will not be needed later. </param>
        private void printoutAllSolutions()
        {
            for (var run = 0; run < _parameters.numberOfRuns; run++)
            {
                string input1 = "\r\n BEST SOLUTION IN RUN:" + run + "\r\n Fitness:  " + _models[run].best.fitness.ToString() + "\r\n SymbolicTree:  " + _models[run].best.symbolicTree.ToStringInorder();
                this.Invoke((MethodInvoker)delegate { printoutOfAllSolutionsLabel.Text += input1; });
                string input2 = "\r\n Evaluation of the first row:" + _models[run].best.symbolicTree.Evaluate(_models[run].data.features.Rows[0]).ToString() + "\r\n ";
                this.Invoke((MethodInvoker)delegate { printoutOfAllSolutionsLabel.Text += input2; });
            }
        }
        #endregion

        #region Show parameters to add on form.
        /// <summary>
        /// Show on form parameters that can be choosen.
        /// </summary>
        private void showParameters()
        {
            mainLayout.Visible = true;
        }






        #endregion

        #region Changed visibility of console form.
        /// <summary>
        /// Make lookup sign visible after "exiting" console.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConsoleForm_VisibilityChanged(object sender, EventArgs e)
        {
            if (consoleForm.Visible == false && _models != null )
            {
                lookupConsoleFormPictureBox.Visible = true;
            }
        }
        #endregion

        #region Changed visibility of waitingAnimation
        /// <summary>
        /// Notify user about end of model search.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WaitingAnimation_VisibilityChanged(object sender, EventArgs e)
        {
            // If waitingAnimation is now hiden (-> process is finished), notify user that search of the model is finished.
            if (!waitingAnimation.Visible)
            {
                this.Invoke((MethodInvoker)delegate { waitingAnimation.Visible = false; });

                // Notify user that model is created and ask if he wants to see all the solutions.
                string message = "Model is created. Do you want to see all solutions?";
                string title = "Model is created";
                DialogResult qpresult = MessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (qpresult == DialogResult.Yes)
                {
                    this.Invoke((MethodInvoker)delegate { printoutOfAllSolutionsLabel.Text = ""; });
                    this.Invoke((MethodInvoker)delegate { printoutOfAllSolutionsLabel.Visible = true; });
                    this.Invoke((MethodInvoker)delegate { printoutOfAllSolutionsLabel.Text = ""; });
                    this.Invoke((MethodInvoker)delegate { printoutOfAllSolutionsLabel.Text += " BEST SOLUTION FITNESS: " + _best.fitness.ToString() + "\r\n \r\n"; });
                    this.Invoke((MethodInvoker)delegate { printoutOfAllSolutionsLabel.Text += "\r\n"; });
                    printoutAllSolutions();
                }
            }
        }
        #endregion

        #endregion

        #endregion

    }
}
