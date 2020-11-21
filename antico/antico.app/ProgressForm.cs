//////////////////////////////////////////////////////////////////////////////////////////
// antico --- artificial bee colony programming based malware detection                 //
// Copyright 2020 Matea Stanišić                                                        //
//                                                                                      //
//                                                                                      //
// Matea Stanišić                                                                       //
// mateastanisic@outlook.com                                                            //
// Zagreb, Hrvatska                                                                     //
//////////////////////////////////////////////////////////////////////////////////////////

using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace antico
{
    #region ProgressForm
    /// 
    /// <summary>
    /// Form for visualizating progress of fitness, depths, TP, TN, FP and FN values 
    /// of best solutions over iterations of algorithm.
    /// 
    /// Every Progress form has
    ///     - variables for moving the form (flag dragging, locations dragCursorPoint and dragCursorPoint)
    ///     - (3) charts
    ///         - for depths (1 series)
    ///         - for fitness, TN, TP, FN, FP (train + test best solution)
    ///         - for TN + TP + FN + FP (train)
    ///     - dictionary of points for above mentioned data
    ///     - curently selected run and type of data to visualize on chart
    ///     - menu strip variables (because runs are dynamically added to menu from other form)
    ///     
    /// </summary>
    /// 
    public partial class ProgressForm : Form
    {
        #region ATTRIBUTES

        #region Should form exist?
        // Variable for keeping track if form should exist.
        public bool shouldExist;
        #endregion

        #region Variables for moving the form.
        // Variables needed for allowing user to move app window on screen.
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;
        #endregion

        #region Chart
        // Public variable for adding chart values to live chart from other forms.
        public LiveCharts.WinForms.CartesianChart chart;

        // Public variable for adding chart values to live chart from other forms.
        public LiveCharts.WinForms.CartesianChart depthsChart;

        // Public variable for adding chart values to live chart from other forms.
        public LiveCharts.WinForms.CartesianChart accuracyChart;
        #endregion

        #region Points
        // Points of fitness of best solution (train - Item1, and test - Item2 in tuple) over Iterations in runs (Dictionary key represent run.)
        public Dictionary<int, Tuple< List<double>, List<double> >> fitnessPoints;

        // Points of number of true positives of best solution (train - Item1, and test - Item2 in tuple) over Iterations in runs (Dictionary key represents run).
        public Dictionary<int, Tuple< List<int>, List<int> >> tpPoints;
        // Points of number of true negtives of best solution (train - Item1, and test - Item2 in tuple) over Iterations in runs (Dictionary key represents run).
        public Dictionary<int, Tuple<List<int>, List<int>>> tnPoints;
        // Points of number of false positives of best solution (train - Item1, and test - Item2 in tuple) over Iterations in runs (Dictionary key represents run).
        public Dictionary<int, Tuple<List<int>, List<int>>> fpPoints;
        // Points of number of false negatives of best solution (train - Item1, and test - Item2 in tuple) over Iterations in runs (Dictionary key represents run).
        public Dictionary<int, Tuple<List<int>, List<int>>> fnPoints;

        // "Accuracy" (TN,TP,FN,FP) points over Iterations in runs (Dictionary key represents run).
        public Dictionary<int, Tuple<List<int>, List<int>, List<int>, List<int>>> accuracyPointsTrain;

        // Points of maximal depths of best tree solutions over Iterations in runs (Dictionary key represent run.)
        public Dictionary<int, List<int>> depthPoints;
        #endregion

        #region Selected run and type of data.
        // Variable for keeping track of selected run.
        public int selectedRun;
        // Varioable for keeping track of selected type of data.
        public string typeOfData;
        #endregion

        #region Menu strip variables.
        public System.Windows.Forms.MenuStrip menuStrip;
        public System.Windows.Forms.ToolStripMenuItem runToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem dataForChartToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fitnessToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem depthOfTheSolutionTreeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tPToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tNToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fnToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fPToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem accuracyToolStripMenuItem;
        private System.Windows.Forms.ToolStripTextBox solutionProgressToolStripMenuItem;
        #endregion

        #endregion

        #region OPERATIONS

        #region Initialize.

        /// <summary>
        /// Besic initialization.
        /// </summary>
        public ProgressForm()
        {
            InitializeComponent();
            this.shouldExist = true;
        }

        /// <summary>
        /// Initialization of form with parameters.
        /// </summary>
        /// 
        /// <param name="location">Point representing location of the parent form.</param>
        public ProgressForm(Point location)
        {
            InitializeComponent();
            this.shouldExist = true;

            // Initialize dictionary of points.
            fitnessPoints = new Dictionary<int, Tuple<List<double>, List<double>>>();
            tpPoints = new Dictionary<int, Tuple<List<int>, List<int>>>();
            tnPoints = new Dictionary<int, Tuple<List<int>, List<int>>>();
            fnPoints = new Dictionary<int, Tuple<List<int>, List<int>>>();
            fpPoints = new Dictionary<int, Tuple<List<int>, List<int>>>();
            depthPoints = new Dictionary<int, List<int>>();
            accuracyPointsTrain = new Dictionary<int, Tuple<List<int>, List<int>, List<int>, List<int>>>();

            this.Location = location;

            // Menu initialization.
            this.InitializeMenu();

            // Charts initialization.
            this.InitializeCharts();

            // Flag that only initialization is done.
            this.selectedRun = -1;
            this.typeOfData = "fitness";

            // Resize enabled.
            this.FormBorderStyle = FormBorderStyle.None;
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.ResizeRedraw, true);
        }

        #region Create menu.
        /// <summary>
        /// Create and initialize menu.
        /// </summary>
        private void InitializeMenu()
        {
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.solutionProgressToolStripMenuItem = new System.Windows.Forms.ToolStripTextBox();
            this.runToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataForChartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fitnessToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.depthOfTheSolutionTreeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tNToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.accuracyToolStripMenuItem = new ToolStripMenuItem();

            this.menuStrip.SuspendLayout();

            // 
            // menuStrip
            // 
            this.menuStrip.AutoSize = false;
            this.menuStrip.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.solutionProgressToolStripMenuItem,
            this.runToolStripMenuItem,
            this.dataForChartToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1012, 31);
            this.menuStrip.TabIndex = 21;
            this.menuStrip.Text = "Menu";
            // 
            // solutionProgressToolStripMenuItem
            // 
            this.solutionProgressToolStripMenuItem.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.solutionProgressToolStripMenuItem.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.solutionProgressToolStripMenuItem.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.solutionProgressToolStripMenuItem.Font = new System.Drawing.Font("Source Sans Pro Semibold", 9.749999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.solutionProgressToolStripMenuItem.Name = "solutionProgressToolStripMenuItem";
            this.solutionProgressToolStripMenuItem.Size = new System.Drawing.Size(145, 27);
            this.solutionProgressToolStripMenuItem.Text = "        Solution progress   ";
            // 
            // runToolStripMenuItem
            // 
            this.runToolStripMenuItem.Name = "runToolStripMenuItem";
            this.runToolStripMenuItem.Size = new System.Drawing.Size(40, 27);
            this.runToolStripMenuItem.Text = "Run";
            this.runToolStripMenuItem.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.RunToolStripMenuItem_DropDownItemClicked);
            // 
            // dataForChartToolStripMenuItem
            // 
            this.dataForChartToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fitnessToolStripMenuItem,
            this.depthOfTheSolutionTreeToolStripMenuItem,
            this.tPToolStripMenuItem,
            this.tNToolStripMenuItem,
            this.fnToolStripMenuItem,
            this.fPToolStripMenuItem,
            this.accuracyToolStripMenuItem});
            this.dataForChartToolStripMenuItem.Name = "dataForChartToolStripMenuItem";
            this.dataForChartToolStripMenuItem.Size = new System.Drawing.Size(91, 27);
            this.dataForChartToolStripMenuItem.Text = "Data for chart";
            // 
            // fitnessToolStripMenuItem
            // 
            this.fitnessToolStripMenuItem.Name = "fitnessToolStripMenuItem";
            this.fitnessToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.fitnessToolStripMenuItem.Text = "Fitness";
            this.fitnessToolStripMenuItem.Click += new EventHandler(this.FitnessToolStripMenuItem_Click);
            // 
            // depthOfTheSolutionTreeToolStripMenuItem
            // 
            this.depthOfTheSolutionTreeToolStripMenuItem.Name = "depthOfTheSolutionTreeToolStripMenuItem";
            this.depthOfTheSolutionTreeToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.depthOfTheSolutionTreeToolStripMenuItem.Text = "Depth of the solution tree";
            this.depthOfTheSolutionTreeToolStripMenuItem.Click += new EventHandler(this.DepthOfTheSolutionTreeToolStripMenuItem_Click);
            // 
            // tPToolStripMenuItem
            // 
            this.tPToolStripMenuItem.Name = "tPToolStripMenuItem";
            this.tPToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.tPToolStripMenuItem.Click += new EventHandler(this.TPToolStripMenuItem_Click);
            this.tPToolStripMenuItem.Text = "TP ";
            // 
            // tNToolStripMenuItem
            // 
            this.tNToolStripMenuItem.Name = "tNToolStripMenuItem";
            this.tNToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.tNToolStripMenuItem.Text = "TN";
            this.tNToolStripMenuItem.Click += new EventHandler(this.TNToolStripMenuItem_Click);
            // 
            // fnToolStripMenuItem
            // 
            this.fnToolStripMenuItem.Name = "fnToolStripMenuItem";
            this.fnToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.fnToolStripMenuItem.Text = "FN";
            this.fnToolStripMenuItem.Click += new EventHandler(this.FNToolStripMenuItem_Click);
            // 
            // fPToolStripMenuItem
            // 
            this.fPToolStripMenuItem.Name = "fPToolStripMenuItem";
            this.fPToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.fPToolStripMenuItem.Text = "FP";
            this.fPToolStripMenuItem.Click += new EventHandler(this.FPToolStripMenuItem_Click);
            // 
            // accuracyToolStripMenuItem
            // 
            this.accuracyToolStripMenuItem.Name = "accuracyToolStripMenuItem";
            this.accuracyToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.accuracyToolStripMenuItem.Text = "TN+TP+FN+FP";
            this.accuracyToolStripMenuItem.Click += new EventHandler(this.AccuracyToolStripMenuItem_Click);

            this.Controls.Add(this.menuStrip);

            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
        }
        #endregion

        #region Create charts
        /// <summary>
        /// Create and initialize new charts.
        /// </summary>
        private void InitializeCharts()
        {
            #region basic chart
            // Create chart for visualizating fitness, tp, tn, fn and fp progress over iterations.
            this.chart = new LiveCharts.WinForms.CartesianChart();
            this.chart.AccessibleName = "";
            this.chart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chart.Font = new System.Drawing.Font("Source Sans Pro", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chart.Location = new System.Drawing.Point(20, 51);
            this.chart.Name = "chart";
            this.chart.Padding = new System.Windows.Forms.Padding(10);
            this.chart.Size = new System.Drawing.Size(972, 616);
            this.chart.TabIndex = 12;
            this.Controls.Add(this.chart);

            // Add basic series.
            this.chart.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Train",
                    Values = new ChartValues<ObservablePoint>(),
                    PointGeometrySize = 10
                },
                new LineSeries
                {
                    Title = "Test",
                    Values = new ChartValues<ObservablePoint>(),
                    PointGeometrySize = 10
                }
            };

            // Add legend and name of axes.
            this.chart.LegendLocation = LegendLocation.Bottom;
            this.chart.AxisX.Add(new Axis());
            this.chart.AxisY.Add(new Axis());
            this.chart.AxisX[0].Title = "Iterations";
            this.chart.AxisY[0].Title = "";

            // Hide for now.
            this.chart.Visible = false;
            #endregion

            #region depths chart
            // Create chart for visualizating depths progress over iterations.
            this.depthsChart = new LiveCharts.WinForms.CartesianChart();
            this.depthsChart.AccessibleName = "";
            this.depthsChart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.depthsChart.Font = new System.Drawing.Font("Source Sans Pro", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.depthsChart.Location = new System.Drawing.Point(20, 51);
            this.depthsChart.Name = "depthsChart";
            this.depthsChart.Padding = new System.Windows.Forms.Padding(10);
            this.depthsChart.Size = new System.Drawing.Size(972, 616);
            this.depthsChart.TabIndex = 12;
            this.Controls.Add(this.depthsChart);

            // Add basic series.
            this.depthsChart.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Depths",
                    Values = new ChartValues<ObservablePoint>(),
                    PointGeometrySize = 10
                }
            };

            // Add legend and name of axes.
            this.depthsChart.LegendLocation = LegendLocation.Bottom;
            this.depthsChart.AxisX.Add(new Axis());
            this.depthsChart.AxisY.Add(new Axis());
            this.depthsChart.AxisX[0].Title = "Iterations";
            this.depthsChart.AxisY[0].Title = "Depth of solution tree";

            // Hide for now.
            this.depthsChart.Visible = false;
            #endregion

            #region accuracy chart
            // Create chart for visualizating depths progress over iterations.
            this.accuracyChart = new LiveCharts.WinForms.CartesianChart();
            this.accuracyChart.AccessibleName = "";
            this.accuracyChart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.accuracyChart.Font = new System.Drawing.Font("Source Sans Pro", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.accuracyChart.Location = new System.Drawing.Point(20, 51);
            this.accuracyChart.Name = "accuracyChart";
            this.accuracyChart.Padding = new System.Windows.Forms.Padding(10);
            this.accuracyChart.Size = new System.Drawing.Size(972, 616);
            this.accuracyChart.TabIndex = 12;
            this.Controls.Add(this.accuracyChart);

            // Add basic series.
            this.accuracyChart.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "TP",
                    Values = new ChartValues<ObservablePoint>(),
                    PointGeometrySize = 10
                },
                new LineSeries
                {
                    Title = "TN",
                    Values = new ChartValues<ObservablePoint>(),
                    PointGeometrySize = 10
                },
                new LineSeries
                {
                    Title = "FP",
                    Values = new ChartValues<ObservablePoint>(),
                    PointGeometrySize = 10
                },
                new LineSeries
                {
                    Title = "FN",
                    Values = new ChartValues<ObservablePoint>(),
                    PointGeometrySize = 10
                }
            };

            // Add legend and name of axes.
            this.accuracyChart.LegendLocation = LegendLocation.Bottom;
            this.accuracyChart.AxisX.Add(new Axis());
            this.accuracyChart.AxisY.Add(new Axis());
            this.accuracyChart.AxisX[0].Title = "Iterations";
            this.accuracyChart.AxisY[0].Title = "";

            // Hide for now.
            this.accuracyChart.Visible = false;
            #endregion
        }
        #endregion

        #region Enable resize
        private const int cGrip = 16;      // Grip size
        private const int cCaption = 32;   // Caption bar height;

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x84)
            {  // Trap WM_NCHITTEST
                Point pos = new Point(m.LParam.ToInt32());
                pos = this.PointToClient(pos);
                if (pos.Y < cCaption)
                {
                    m.Result = (IntPtr)2;  // HTCAPTION
                    return;
                }
                if (pos.X >= this.ClientSize.Width - cGrip && pos.Y >= this.ClientSize.Height - cGrip)
                {
                    m.Result = (IntPtr)17; // HTBOTTOMRIGHT
                    return;
                }
            }
            base.WndProc(ref m);
        }
        #endregion

        #endregion

        #region Methods for enabling moving ProgressForm on user screen.
        /// <summary>
        /// Flag the wariable dragging false since user pressed mouse button initiating end of moving frame.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PanelForMoving_MouseUp(object sender, MouseEventArgs e)
        {
            this.dragging = false;
        }

        /// <summary>
        /// Flag the wariable dragging true since user pressed mouse button initiating beggining of moving frame.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PanelForMoving_MouseDown(object sender, MouseEventArgs e)
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
        private void PanelForMoving_MouseMove(object sender, MouseEventArgs e)
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

        #region panel for moving
        /// <summary>
        /// Hovering start.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PanelForMoving_MouseEnter(object sender, EventArgs e)
        {
            this.PanelForMoving.Cursor = Cursors.Hand;
        }

        /// <summary>
        /// Hovering end.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PanelForMoving_MouseLeave(object sender, EventArgs e)
        {
            this.PanelForMoving.Cursor = Cursors.Default;
        }
        #endregion

        #endregion

        #region CLICKING

        #region exit

        /// <summary>
        /// Hiding the form when pressing exit sign.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitSign_MouseClick(object sender, EventArgs e)
        {
            // Hide the form.
            this.Visible = false;
        }

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

        #endregion

        #region Show specific chart.
        /// <summary>
        /// Show chart.
        /// </summary>
        /// 
        /// <param name="run">Run from which data will be visualized.</param>
        /// <param name="type">Type of data to be visualized (fitness/depths/TN/TP/FN/FP). </param>
        public void ShowChart(int run, string type)
        {
            // Set up selected run and type of data.
            this.selectedRun = run;
            this.typeOfData = type;

            if (this.runToolStripMenuItem.Text.Contains("Run"))
                this.runToolStripMenuItem.Text = "Run: " + this.selectedRun;
            else if (this.runToolStripMenuItem.Text.Contains("Fold"))
                this.runToolStripMenuItem.Text = "Fold: " + this.selectedRun;

            this.dataForChartToolStripMenuItem.Text = "Data for chart: " + this.typeOfData;

            if (type == "depths")
            {
                #region check if run is in range
                if (!this.depthPoints.ContainsKey(run))
                    throw new Exception("[ShowChart - depths] Selected run wasn't previously added in dictionary.");
                #endregion

                // Clear points from chart.
                this.depthsChart.Series[0].Values.Clear();

                // Hide chart for fitness.
                this.chart.Visible = false;
                this.accuracyChart.Visible = false;
                // Make it visible.
                this.depthsChart.Visible = true;

                // Add new points.
                for (var p = 0; p < this.depthPoints[run].Count; p++)
                {
                    // Add point to the chart from selected run.
                    this.depthsChart.Series[0].Values.Add(new ObservablePoint(p, this.depthPoints[run][p]) );
                }
            }
            else if (type == "fitness")
            {
                #region check if run is in range
                if (!this.fitnessPoints.ContainsKey(run))
                    throw new Exception("[ShowChart - fitness] Selected run wasn't previously added in dictionary.");
                #endregion

                // Clear points from chart.
                this.chart.Series[0].Values.Clear();
                this.chart.Series[1].Values.Clear();

                // Hide depth chart.
                this.depthsChart.Visible = false;
                this.accuracyChart.Visible = false;
                // Make it visible.
                this.chart.Visible = true;

                // Add new points to train and test series.
                for (var p = 0; p < this.fitnessPoints[run].Item1.Count; p++)
                {
                    // Add point to the chart from selected run.
                    this.chart.Series[0].Values.Add(new ObservablePoint(p, this.fitnessPoints[run].Item1[p]));
                    if (p < this.fitnessPoints[run].Item2.Count) 
                        this.chart.Series[1].Values.Add(new ObservablePoint(p, this.fitnessPoints[run].Item2[p]));
                }
            }
            else if (type == "TP")
            {
                #region check if run is in range
                if (!this.tpPoints.ContainsKey(run))
                    throw new Exception("[ShowChart - TP] Selected run wasn't previously added in dictionary.");
                #endregion

                // Clear points from chart.
                this.chart.Series[0].Values.Clear();
                this.chart.Series[1].Values.Clear();

                // Hide depth chart.
                this.depthsChart.Visible = false;
                this.accuracyChart.Visible = false;
                // Make it visible.
                this.chart.Visible = true;

                // Add new points to train and test series.
                for (var p = 0; p < this.tpPoints[run].Item1.Count; p++)
                {
                    // Add point to the chart from selected run.
                    this.chart.Series[0].Values.Add(new ObservablePoint(p, this.tpPoints[run].Item1[p]));
                    if (p < this.tpPoints[run].Item2.Count) 
                        this.chart.Series[1].Values.Add(new ObservablePoint(p, this.tpPoints[run].Item2[p]));
                }
            }
            else if (type == "TN")
            {
                #region check if run is in range
                if (!this.tpPoints.ContainsKey(run))
                    throw new Exception("[ShowChart - TN] Selected run wasn't previously added in dictionary.");
                #endregion

                // Clear points from chart.
                this.chart.Series[0].Values.Clear();
                this.chart.Series[1].Values.Clear();

                // Hide depth chart.
                this.depthsChart.Visible = false;
                this.accuracyChart.Visible = false;
                // Make it visible.
                this.chart.Visible = true;

                // Add new points to train and test series.
                for (var p = 0; p < this.tnPoints[run].Item1.Count; p++)
                {
                    // Add point to the chart from selected run.
                    this.chart.Series[0].Values.Add(new ObservablePoint(p, this.tnPoints[run].Item1[p]));
                    if (p < this.tnPoints[run].Item2.Count)
                        this.chart.Series[1].Values.Add(new ObservablePoint(p, this.tnPoints[run].Item2[p]));
                }
            }
            else if (type == "FP")
            {
                #region check if run is in range
                if (!this.fpPoints.ContainsKey(run))
                    throw new Exception("[ShowChart - FP] Selected run wasn't previously added in dictionary.");
                #endregion

                // Clear points from chart.
                this.chart.Series[0].Values.Clear();
                this.chart.Series[1].Values.Clear();

                // Hide depth chart.
                this.depthsChart.Visible = false;
                this.accuracyChart.Visible = false;
                // Make it visible.
                this.chart.Visible = true;

                // Add new points to train and test series.
                for (var p = 0; p < this.fpPoints[run].Item1.Count; p++)
                {
                    // Add point to the chart from selected run.
                    this.chart.Series[0].Values.Add(new ObservablePoint(p, this.fpPoints[run].Item1[p]));
                    if (p < this.fpPoints[run].Item2.Count)
                        this.chart.Series[1].Values.Add(new ObservablePoint(p, this.fpPoints[run].Item2[p]));
                }
            }
            else if (type == "FN")
            {
                #region check if run is in range
                if (!this.fnPoints.ContainsKey(run))
                    throw new Exception("[ShowChart - FN] Selected run wasn't previously added in dictionary.");
                #endregion

                // Clear points from chart.
                this.chart.Series[0].Values.Clear();
                this.chart.Series[1].Values.Clear();

                // Hide depth chart.
                this.depthsChart.Visible = false;
                this.accuracyChart.Visible = false;
                // Make it visible.
                this.chart.Visible = true;

                // Add new points to train and test series.
                for (var p = 0; p < this.fnPoints[run].Item1.Count; p++)
                {
                    // Add point to the chart from selected run.
                    this.chart.Series[0].Values.Add(new ObservablePoint(p, this.fnPoints[run].Item1[p]));
                    if (p < this.fnPoints[run].Item2.Count)
                        this.chart.Series[1].Values.Add(new ObservablePoint(p, this.fnPoints[run].Item2[p]));
                }
            }
            else if (type == "accuracy")
            {
                #region check if run is in range
                if (!this.fnPoints.ContainsKey(run))
                    throw new Exception("[ShowChart - accuracy] Selected run wasn't previously added in dictionary.");
                #endregion

                this.dataForChartToolStripMenuItem.Text = "Data for chart: TN+TP+FN+FP";

                // Clear points from chart.
                this.accuracyChart.Series[0].Values.Clear();
                this.accuracyChart.Series[1].Values.Clear();
                this.accuracyChart.Series[2].Values.Clear();
                this.accuracyChart.Series[3].Values.Clear();

                // Hide depth chart.
                this.depthsChart.Visible = false;
                this.chart.Visible = false;
                // Make it visible.
                this.accuracyChart.Visible = true;

                // Add new points to train and test series.
                for (var p = 0; p < this.accuracyPointsTrain[run].Item1.Count; p++)
                {
                    // Add point to the chart from selected run.
                    this.accuracyChart.Series[0].Values.Add(new ObservablePoint(p, this.accuracyPointsTrain[run].Item1[p]));
                    if (p < this.accuracyPointsTrain[run].Item2.Count)
                        this.accuracyChart.Series[1].Values.Add(new ObservablePoint(p, this.accuracyPointsTrain[run].Item2[p]));
                    if (p < this.accuracyPointsTrain[run].Item3.Count)
                        this.accuracyChart.Series[2].Values.Add(new ObservablePoint(p, this.accuracyPointsTrain[run].Item3[p]));
                    if (p < this.accuracyPointsTrain[run].Item4.Count)
                        this.accuracyChart.Series[3].Values.Add(new ObservablePoint(p, this.accuracyPointsTrain[run].Item4[p]));
                }
            }
        }
        #endregion

        #region Menu strip clicking.

        #region fitness selected
        /// <summary>
        /// Show new chart.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FitnessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.selectedRun == -1)
            {
                // Try showing points from run 0, if there are any points in the dictionary.
                if (this.fitnessPoints.ContainsKey(0) && this.fitnessPoints[0].Item1.Count > 0)
                {
                    this.ShowChart(0, "fitness");
                }
                else
                {
                    // Show empty chart.
                    this.chart.Visible = true;
                }

                return;
            }

            this.ShowChart(this.selectedRun, "fitness");
        }
        #endregion

        #region depths selected
        /// <summary>
        /// Show new chart.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DepthOfTheSolutionTreeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.selectedRun == -1)
            {
                // Try showing points from run 0, if there are any points in the dictionary.
                if (this.depthPoints.ContainsKey(0) && this.depthPoints[0].Count > 0)
                {
                    this.ShowChart(0, "depths");
                }
                else
                {
                    // Show empty chart.
                    this.depthsChart.Visible = true;
                }

                return;
            }

            this.ShowChart(this.selectedRun, "depths");
        }
        #endregion

        #region TP selected
        /// <summary>
        /// Show new chart.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.selectedRun == -1)
            {
                // Try showing points from run 0, if there are any points in the dictionary.
                if (this.tpPoints.ContainsKey(0) && this.tpPoints[0].Item1.Count > 0)
                {
                    this.ShowChart(0, "TP");
                }
                else
                {
                    // Show empty chart.
                    this.chart.Visible = true;
                }

                return;
            }

            this.ShowChart(this.selectedRun, "TP");
        }
        #endregion

        #region TN selected
        /// <summary>
        /// Show new chart.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TNToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.selectedRun == -1)
            {
                // Try showing points from run 0, if there are any points in the dictionary.
                if (this.tnPoints.ContainsKey(0) && this.tnPoints[0].Item1.Count > 0)
                {
                    this.ShowChart(0, "TN");
                }
                else
                {
                    // Show empty chart.
                    this.chart.Visible = true;
                }

                return;
            }

            this.ShowChart(this.selectedRun, "TN");
        }
        #endregion

        #region FN selected
        /// <summary>
        /// Show new chart.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FNToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.selectedRun == -1)
            {
                // Try showing points from run 0, if there are any points in the dictionary.
                if (this.fnPoints.ContainsKey(0) && this.fnPoints[0].Item1.Count > 0)
                {
                    this.ShowChart(0, "FN");
                }
                else
                {
                    // Show empty chart.
                    this.chart.Visible = true;
                }

                return;
            }

            this.ShowChart(this.selectedRun, "FN");
        }
        #endregion

        #region FP selected
        /// <summary>
        /// Show new chart.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.selectedRun == -1)
            {
                // Try showing points from run 0, if there are any points in the dictionary.
                if (this.fpPoints.ContainsKey(0) && this.fpPoints[0].Item1.Count > 0)
                {
                    this.ShowChart(0, "FP");
                }
                else
                {
                    // Show empty chart.
                    this.chart.Visible = true;
                }

                return;
            }
            this.ShowChart(this.selectedRun, "FP");
        }
        #endregion

        #region RUN selected
        /// <summary>
        /// Show new chart.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RunToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            int run = Int32.Parse(e.ClickedItem.Text);
            this.ShowChart(run, this.typeOfData);
        }
        #endregion

        #region TN + TP + FN + FP
        /// <summary>
        /// Show new chart.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AccuracyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.selectedRun == -1)
            {
                // Try showing points from run 0, if there are any points in the dictionary.
                if (this.accuracyPointsTrain.ContainsKey(0) && this.accuracyPointsTrain[0].Item1.Count > 0)
                {
                    this.ShowChart(0, "accuracy");
                }
                else
                {
                    // Show empty chart.
                    this.accuracyChart.Visible = true;
                }

                return;
            }
            this.ShowChart(this.selectedRun, "accuracy");
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
        private void ProgressForm_SizeChanged(object sender, EventArgs e)
        {
            this.exitSign.Location = new System.Drawing.Point(this.Size.Width - 28, 5);
            this.minimizeSign.Location = new System.Drawing.Point(this.Size.Width - 56, 5);
        }
        #endregion

        #region Change text of runToolStripMenuItem.
        /// <summary>
        /// Changes text of runToolStripMenuItem.
        /// </summary>
        /// 
        /// <param name="text">Text to put on runToolStripMenuItem.Text.</param>
        public void ToolRoolStrip_ChangeText(string text)
        {
            this.runToolStripMenuItem.Text = text;
        }
        #endregion

        #region Cancle closing the form.
        /// <summary>
        /// Cancle closing the form when form should exist.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProgressForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.shouldExist)
            {
                this.Hide();
                e.Cancel = true;
            }
        }
        #endregion

        #endregion
    }
    #endregion
}
