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

        #region ABCP variable.
        // Main variable for creating a new model.
        private ABCP _model;
        #endregion

        #region ABCP parameters
        // Variable for saving custom parameters.
        Parameters _parameters;
        #endregion

        #region Other forms.
        // Helper form for showing symbolic tree structure of a (best) model.
        private HelperForm formForVisualizationOfModel;
        // Main frame.
        private MainFrame _mainFrame;
        #endregion

        #region Forbid clicking on form.
        // Variable that is used to forbid user clicking on signs.
        bool forbid = false;
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
        public CreateNewModelForm(MainFrame mainFrame, ref ABCP model)
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
            _model = model;
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
        /// Flag the wariable dragging false since user pressed mouse button initiating end of moving frame.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateNewModelForm_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }
        #endregion

        #region HOVERING

        #region Making hand cursor when hovering GoBack sign.

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
            if (_model != null && _model.best != null)
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
            if (_model != null && _model.best != null)
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

        #endregion

        #region Showing ToolTip when hovering GoBack sign.

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
            this.Close();

            // Update new location of main frame.
            _mainFrame.Location = this.Location;
            // Make main form visible again.
            _mainFrame.Visible = true;
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
            // Check if another process already runnning.
            if (forbid)
            {
                string m = "Wait! Model calculation is still in progress.";
                string t = "Warning!";
                MessageBox.Show(m, t, MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            // Just in case, remove text box for printout.
            printoutOfAllSolutionsTextBox.Visible = false;

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
            // Check if another process already runnning.
            if (forbid)
            {
                string m = "Wait! Model calculation is still in progress.";
                string t = "Warning!";
                MessageBox.Show(m, t, MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            // Just in case, hide custom parameters layout.
            mainLayout.Visible = false;

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

                    // Initialize and printout when done using threads because of animation.
                    Thread myThread = new Thread(
                        () =>
                        {
                            try
                            {
                                this.Invoke((MethodInvoker)delegate { waitingAnimation.Visible = true; });

                                _model = new ABCP();
                                _model.ABCProgramming();

                                System.Threading.Thread.Sleep(5000);
                                this.Invoke((MethodInvoker)delegate { waitingAnimation.Visible = false; });
                            }
                            finally
                            {
                                this.Invoke((MethodInvoker)delegate { waitingAnimation.Visible = false; });

                                // Notify user that model is created and ask if he wants to see all the solutions.
                                string qmessage = "Model is created. Do you want to see all solutions?";
                                string qtitle = "Model is created";
                                DialogResult qresult = MessageBox.Show(qmessage, qtitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                                if (qresult == DialogResult.Yes)
                                {
                                    this.Invoke((MethodInvoker)delegate { printoutOfAllSolutionsTextBox.Text = ""; });
                                    this.Invoke((MethodInvoker)delegate { printoutOfAllSolutionsTextBox.Visible = true; });
                                    this.Invoke((MethodInvoker)delegate { printoutOfAllSolutionsTextBox.Text = ""; });
                                    this.Invoke((MethodInvoker)delegate { printoutOfAllSolutionsTextBox.Text += " BEST SOLUTION FITNESS: " + _model.best.fitness.ToString() + "\r\n \r\n"; });
                                    this.Invoke((MethodInvoker)delegate { printoutOfAllSolutionsTextBox.Text += "\r\n"; });
                                    printoutAllSolutions(_model.population, _model.data);
                                }

                                // Back to normal.
                                forbid = false;
                            }
                        });
                    myThread.Start();
                }

                return;
            }

            // Forbid all other cliks.
            forbid = true;

            // User already added custom parameters.
            // Initialize and printout when done using threads because of animation.
            Thread myThreadp = new Thread(
                () =>
                {
                    try
                    {
                        this.Invoke((MethodInvoker)delegate { waitingAnimation.Visible = true; });

                        _model = new ABCP(_parameters);
                        _model.ABCProgramming();

                        System.Threading.Thread.Sleep(5000);
                        this.Invoke((MethodInvoker)delegate { waitingAnimation.Visible = false; });
                    }
                    finally
                    {
                        this.Invoke((MethodInvoker)delegate { waitingAnimation.Visible = false; });

                        // Notify user that model is created and ask if he wants to see all the solutions.
                        string qpmessage = "Model is created. Do you want to see all solutions?";
                        string qptitle = "Model is created";
                        DialogResult qpresult = MessageBox.Show(qpmessage, qptitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (qpresult == DialogResult.Yes)
                        {
                            this.Invoke((MethodInvoker)delegate { printoutOfAllSolutionsTextBox.Text = ""; });
                            this.Invoke((MethodInvoker)delegate { printoutOfAllSolutionsTextBox.Visible = true; });
                            this.Invoke((MethodInvoker)delegate { printoutOfAllSolutionsTextBox.Text = ""; });
                            this.Invoke((MethodInvoker)delegate { printoutOfAllSolutionsTextBox.Text += " BEST SOLUTION FITNESS: " + _model.best.fitness.ToString() + "\r\n \r\n"; });
                            this.Invoke((MethodInvoker)delegate { printoutOfAllSolutionsTextBox.Text += "\r\n"; });
                            printoutAllSolutions(_model.population, _model.data);
                        }

                        // Back to normal.
                        forbid = false;
                    }
                });
            myThreadp.Start();

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
            // Check if another process already runnning.
            if (forbid)
            {
                string m = "Wait! Model calculation is still in progress.";
                string t = "Warning!";
                MessageBox.Show(m, t, MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            // Check if model exists.
            if (_model != null && _model.best != null)
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
                DrawSymbolicTree(_model.best.symbolicTree, ref graph);
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
            // Check if another process already runnning.
            if (forbid)
            {
                string m = "Wait! Model calculation is still in progress.";
                string t = "Warning!";
                MessageBox.Show(m, t, MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            // Check if model exists.
            if (_model != null && _model.best != null)
            {
                string message = "Feature still not implemented!";
                string title = "antico responds";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Error);
                return;
            }

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
            // Just in case, remove text box for printout.
            printoutOfAllSolutionsTextBox.Visible = false;

            // Hide parameters layout.
            mainLayout.Visible = false;

            //(int ps, int maxnoi, int maxnonii, int nooruns, int imd, int md, string method, int lim, double a, double prob)
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

            _parameters = new Parameters(ps, maxnoi, maxnonii, nooruns, imd, md, method, lim, a, prob);

            // Forbid all other cliks.
            forbid = true;

            // User already added custom parameters.
            // Initialize and printout when done using threads because of animation.
            Thread myThreadp = new Thread(
                () =>
                {
                    try
                    {
                        this.Invoke((MethodInvoker)delegate { waitingAnimation.Visible = true; });

                        _model = new ABCP(_parameters);
                        _model.ABCProgramming();

                        System.Threading.Thread.Sleep(5000);
                        this.Invoke((MethodInvoker)delegate { waitingAnimation.Visible = false; });
                    }
                    finally
                    {
                        this.Invoke((MethodInvoker)delegate { waitingAnimation.Visible = false; });

                        // Notify user that model is created and ask if he wants to see all the solutions.
                        string qpmessage = "Model is created. Do you want to see all solutions?";
                        string qptitle = "Model is created";
                        DialogResult qpresult = MessageBox.Show(qpmessage, qptitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (qpresult == DialogResult.Yes)
                        {
                            this.Invoke((MethodInvoker)delegate { printoutOfAllSolutionsTextBox.Text = ""; });
                            this.Invoke((MethodInvoker)delegate { printoutOfAllSolutionsTextBox.Visible = true; });
                            this.Invoke((MethodInvoker)delegate { printoutOfAllSolutionsTextBox.Text = ""; });
                            this.Invoke((MethodInvoker)delegate { printoutOfAllSolutionsTextBox.Text += " BEST SOLUTION FITNESS: " + _model.best.fitness.ToString() + "\r\n \r\n"; });
                            this.Invoke((MethodInvoker)delegate { printoutOfAllSolutionsTextBox.Text += "\r\n"; });
                            printoutAllSolutions(_model.population, _model.data);
                        }

                        // Back to normal.
                        forbid = false;
                    }
                });
            myThreadp.Start();

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
                    // Add edge between the current node and his child node.
                    graph.AddEdge("(" + node.index + ") " + node.content, "(" + node.children[i].index + ") " + node.children[i].content);

                    // Do the same for child subtree.
                    DrawSymbolicTree(node.children[i], ref graph);
                }
            }

            // Coloring the node.
            System.Drawing.Color c = System.Drawing.ColorTranslator.FromHtml(depthColors[node.depth]);
            graph.FindNode("(" + node.index + ") " + node.content).Attr.FillColor = new Microsoft.Msagl.Drawing.Color((byte)c.R, (byte)c.G, (byte)c.B);
        }
        #endregion

        #region Printout of all solutions.
        /// <summary>
        /// Prints all solutions to TextBox in form.
        /// </summary>
        /// <param name="solutions">All known solutions.</param>
        /// <param name="dt">Data for printing evaluation of first row. - TESTING PHASE - TODO this will not be needed later. </param>
        private void printoutAllSolutions(Population solutions, Data dt)
        {
            for (var i = 0; i < solutions.populationSize; i++)
            {
                string input1 = "\r\n SOLUTION:" + i + "\r\n Fitness:" + solutions.chromosomes[i].fitness.ToString() + "\r\n SymbolicTree:" + solutions.chromosomes[i].symbolicTree.ToStringInorder();
                this.Invoke((MethodInvoker)delegate { printoutOfAllSolutionsTextBox.Text += input1; });
                string input2 = "\r\n Evaluation of the first row:" + solutions.chromosomes[i].symbolicTree.Evaluate(dt.features.Rows[0]).ToString() + "\r\n ";
                this.Invoke((MethodInvoker)delegate { printoutOfAllSolutionsTextBox.Text += input2; });
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

        #endregion

        #endregion



    }
}
