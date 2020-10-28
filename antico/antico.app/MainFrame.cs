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
using System.Drawing.Text;
using System.IO;
using System.Diagnostics;
using YaraSharp;
using System.Collections.Generic;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Data;
using System.Net;
using Microsoft.WindowsAPICodePack.Shell;
using System.Text.RegularExpressions;

namespace antico
{
    #region MainFrame
    ///
    /// <summary>
    /// Main form - main page of the application.
    /// 
    /// Every MainForm has
    ///     - variables for moving the form (flag dragging, locations dragCursorPoint and dragCursorPoint)
    ///     - anticoLabelDesign (title)
    ///     - other forms (formForCreatingNewModel)
    /// 
    /// </summary>
    /// 
    public partial class MainFrame : Form
    {
        #region ATTRIBUTES

        #region random
        // Variable for generating random numbers.
        private static Random rand;
        #endregion

        #region Variables for moving the form.
        // Variables needed for allowing user to move app window on screen.
        private bool dragging = false;
        private System.Drawing.Point dragCursorPoint;
        private System.Drawing.Point dragFormPoint;
        #endregion

        #region Variables for design.
        // Variable for antico label needed after comeback from CreateNewModelForm.
        internal System.Windows.Forms.Label anticoLabelDesign;
        #endregion

        #region Other forms.
        // Form for creating new model.
        private CreateNewModelForm formForCreatingNewModel;
        #endregion

        #region Detection model.
        // Model for detection.
        UploadedModel detection;
        #endregion

        #endregion

        #region OPERATIONS

        #region Static constructor.
        /// <summary>
        /// New static random.
        /// </summary>
        static MainFrame()
        {
            rand = new Random();
        }
        #endregion

        #region Initialize.
        /// <summary>
        /// Function for initializing components on Main frame.
        /// </summary>
        internal MainFrame()
        {
            this.InitializeComponent();
            this.DoubleBuffered = true;

            // Load text font.
            PrivateFontCollection anticoFont = new PrivateFontCollection();
            anticoFont.AddFontFile("../../../../[FONTS]/UnicaOne-Regular.ttf");
            this.anticoLabel.Font = new Font(anticoFont.Families[0], 35, System.Drawing.FontStyle.Regular);
            this.anticoLabelDesign = this.anticoLabel;

            // Upload model for detection.
            // TODO: hardcoded
            LoadFile(@"../../../../[DATA]/saved/4_RUN1_best_solution_0.955__21_10_2020.dat");

        }
        #endregion

        #region Methods for enabling moving MainFrame on user screen.

        /// <summary>
        /// Flag the wariable dragging true since user pressed mouse button initiating beggining of moving frame.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainFrame_MouseDown(object sender, MouseEventArgs e)
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
        private void MainFrame_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.dragging)
            {
                System.Drawing.Point dif = System.Drawing.Point.Subtract(Cursor.Position, new System.Drawing.Size(this.dragCursorPoint));
                this.Location = System.Drawing.Point.Add(this.dragFormPoint, new System.Drawing.Size(dif));
            }
        }

        /// <summary>
        /// Flag the wariable dragging false since user pressed mouse button initiating end of moving frame.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainFrame_MouseUp(object sender, MouseEventArgs e)
        {
            this.dragging = false;
        }

        #endregion

        #region HOVER

        #region Making hand cursor when hovering picture boxes that are behaving like buttons.

        #region exit 
        /// <summary>
        /// Hovering start.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitPictureBox_MouseEnter(object sender, EventArgs e)
        {
            this.exitPictureBox.Cursor = Cursors.Hand;
        }

        /// <summary>
        /// Hovering end.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitPictureBox_MouseLeave(object sender, EventArgs e)
        {
            this.exitPictureBox.Cursor = Cursors.Default;
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
        private void GoBack_MouseEnter(object sender, EventArgs e)
        {
            this.goBackSign.Cursor = Cursors.Hand;
        }

        /// <summary>
        /// Hovering end.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GoBack_MouseLeave(object sender, EventArgs e)
        {
            this.goBackSign.Cursor = Cursors.Default;
        }
        #endregion

        #region create model
        /// <summary>
        ///  Hovering start.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateNewModelSign_MouseEnter(object sender, EventArgs e)
        {
            this.createNewModelSign.Cursor = Cursors.Hand;
        }

        /// <summary>
        /// Hovering end.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateNewModelSign_MouseLeave(object sender, EventArgs e)
        {
            this.createNewModelSign.Cursor = Cursors.Default;
        }
        #endregion

        #region is this file malicious
        /// <summary>
        ///  Hovering start.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IsThisMaliciousSign_MouseEnter(object sender, EventArgs e)
        {
            this.isThisMaliciousSign.Cursor = Cursors.Hand;
        }

        /// <summary>
        /// Hovering end.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IsThisMaliciousSign_MouseLeave(object sender, EventArgs e)
        {
            this.isThisMaliciousSign.Cursor = Cursors.Default;
        }
        #endregion

        #region about antico
        /// <summary>
        ///  Hovering start.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AboutAnticoSign_MouseEnter(object sender, EventArgs e)
        {
            this.aboutAnticoSign.Cursor = Cursors.Hand;
        }

        /// <summary>
        /// Hovering end.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AboutAnticoSign_MouseLeave(object sender, EventArgs e)
        {
            this.aboutAnticoSign.Cursor = Cursors.Default;
        }
        #endregion

        #endregion

        #region Showing ToolTip when hovering picture boxes that are behaving like buttons.

        #region exit 
        /// <summary>
        /// Show that pressing exitPictureBox means exiting the application.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitPictureBox_MouseHover(object sender, EventArgs e)
        {
            ToolTip tt = new ToolTip();
            tt.SetToolTip(this.exitPictureBox, "exit");
        }
        #endregion

        #region minimize
        /// <summary>
        /// Show that pressing minimizeSign means minimizing the application.
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

        #region go back
        /// <summary>
        /// Show that pressing goBackSign means going back to main frame.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GoBack_MouseHover(object sender, EventArgs e)
        {
            ToolTip tt = new ToolTip();
            tt.SetToolTip(this.goBackSign, "go back");
        }
        #endregion

        #region create model
        /// <summary>
        /// Show what createNewModelSign picture box represents.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateNewModelSign_MouseHover(object sender, EventArgs e)
        {
            ToolTip tt = new ToolTip();
            tt.SetToolTip(this.createNewModelSign, "create new model");
        }
        #endregion

        #region is this file malicious
        /// <summary>
        /// Show what isThisMaliciousSign picture box represents.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IsThisMaliciousSign_MouseHover(object sender, EventArgs e)
        {
            ToolTip tt = new ToolTip();
            tt.SetToolTip(this.isThisMaliciousSign, "check is your file malicious");
        }
        #endregion

        #region about antico
        /// <summary>
        /// Show what aboutAnticoSign picture box represents.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AboutAnticoSign_MouseHover(object sender, EventArgs e)
        {
            ToolTip tt = new ToolTip();
            tt.SetToolTip(this.aboutAnticoSign, "about antico");
        }
        #endregion

        #endregion

        #endregion

        #region CLICK

        #region exit
        /// <summary>
        /// Closing form when pressing Exit sign.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitPictureBox_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }
        #endregion

        #region minimize
        /// <summary>
        /// Minimize form when pressing Minimize sign.
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
        /// "Goes back" to main frame when pressing GoBack sign.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GoBack_MouseClick(object sender, MouseEventArgs e)
        {
            // Remove label about antico.
            this.Controls.Remove(this.aboutAnticoLabel);
            // Remove go back sign.
            this.Controls.Remove(this.goBackSign);

            // Show signs for new form, file detection and about antico.
            this.aboutAnticoSign.Visible = true;
            this.createNewModelSign.Visible = true;
            this.isThisMaliciousSign.Visible = true;
        }
        #endregion

        #region new model 
        /// <summary>
        /// Forward user to createNewModel form when clicking on createNewModelSign.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateNewModelSign_MouseClick(object sender, MouseEventArgs e)
        {
            // Create form for model creation.
            this.formForCreatingNewModel = new CreateNewModelForm(this);

            // Hide current form.
            this.Visible = false;
            // Show new form.
            this.formForCreatingNewModel.Show();
        }
        #endregion

        #region is this file malicious 
        /// <summary>
        /// Forward user to form where he can check if his file is malicious.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IsThisMaliciousSign_MouseClick(object sender, MouseEventArgs e)
        {
            // TODO: only for clamp database.

            // Get path to \Downloads folder.
            string downloadsPath = KnownFolders.Downloads.Path;

            DialogResult result = this.chooseFileToDetermineIfMaliciousDialog.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                string file = this.chooseFileToDetermineIfMaliciousDialog.FileName;
                string script = "../../../../[PYTHON]/extractFeatures.py";

                #region handle non-english charachters in path
                if (!File.Exists(downloadsPath + @"\\" + Path.GetFileName(script)))
                {
                    // Move script to \Downloads.
                    File.Copy(Path.GetFullPath(script).Replace("/", @"\\"), downloadsPath + @"\\" + Path.GetFileName(script));
                }

                // Move file to \Downloads.
                File.Copy(file, downloadsPath + @"\\" + Path.GetFileName(file));

                var targetDirPath = new DirectoryInfo(downloadsPath + @"\\Python");
                var sourceDirPath = new DirectoryInfo(Path.GetFullPath("../../../../[PYTHON]/Python/"));

                CopyAll(targetDirPath, sourceDirPath);

                script = downloadsPath + @"\\" + Path.GetFileName(script);
                string outputFile = (downloadsPath + @"\" + Path.GetFileName(file) + rand.Next(10000).ToString() + "_clamp_features.csv").ToString();
                #endregion

                #region extract features with python script
                try
                {
                    // Create process info.
                    var psi = new ProcessStartInfo();
                    // Location to python.
                    psi.FileName = Path.GetFullPath(@"../../../../[PYTHON]/Python/python.exe").ToString();

                    // Provide script and arguments.
                    psi.Arguments = string.Format("{0} {1} {2}", script, downloadsPath + @"\\" + Path.GetFileName(file), outputFile);

                    // Process configuration.
                    psi.UseShellExecute = false;
                    psi.CreateNoWindow = true;
                    psi.RedirectStandardOutput = true;
                    psi.RedirectStandardError = true;

                    // Execute process and get output.
                    var errors = "";
                    var results = "";

                    // Execute script.
                    using (var process = Process.Start(psi))
                    {
                        errors = process.StandardError.ReadToEnd();
                        results = process.StandardOutput.ReadToEnd();
                    }
                    if (errors != "")
                    {
                        throw new Exception("[IsThisMaliciousSign_MouseClick]\r\n" + errors);
                    }

                }
                catch (IOException)
                {
                    throw new Exception("[UploadSign_MouseClick] Coudn't process the file.");
                }
                #endregion

                int packed = 0;
                string packer = "NoPacker";

                #region find out packer and packer type using YaraSharp
                //  All API calls happens here.
                YSInstance instance = new YSInstance();

                //  Declare external variables (could be null).
                Dictionary<string, object> externals = new Dictionary<string, object>()
                {
                    { "filename", string.Empty },
                    { "filepath", string.Empty },
                    { "extension", string.Empty }
                };

                //  Get list of YARA rules.
                List<string> rulesFile = new List<string>();

                // Download rules (peid.yara) to users computer since YaraSharp can't read non-english charachters in file path.
                WebClient Client = new WebClient();
                Client.DownloadFile(new Uri("https://raw.githubusercontent.com/urwithajit9/ClaMP/master/scripts/peid.yara"), downloadsPath + @"\peid.yara");

                rulesFile.Add(Path.GetFullPath(downloadsPath + @"\peid.yara").ToString());

                //  Context is where yara is initialized.
                //  From yr_initialize() to yr_finalize().
                using (YSContext context = new YSContext())
                {
                    //  Compiling rules.
                    using (YSCompiler compiler = instance.CompileFromFiles(rulesFile, externals))
                    {
                        //  Get compiled rules.
                        YSRules rules = compiler.GetRules();

                        //  Get errors.
                        YSReport errors = compiler.GetErrors();
                        //  Get warnings.
                        YSReport warnings = compiler.GetWarnings();


                        //  Some file to test yara rules.
                        string Filename = file;

                        //  Get matches.
                        List<YSMatches> Matches = instance.ScanFile(Filename, rules,
                                new Dictionary<string, object>()
                                {
                                    { "filename", Path.GetFileName(Filename) },
                                    { "filepath", Path.GetFullPath(Filename) },
                                    { "extension", Path.GetExtension(Filename) }
                                },
                                0);

                        //  Get packer name if packed.
                        if (Matches.Count > 0)
                        {
                            packed = 1;
                            packer = Matches[0].Rule.Identifier;
                        }
                    }
                }
                #endregion

                #region determine if file is malicious using best model so far

                DataTable fileFeatures = new DataTable();

                #region csv to datatable

                using (StreamReader sr = new StreamReader(outputFile))
                {
                    string[] headers = sr.ReadLine().Split(',');
                    foreach (string header in headers)
                    {
                        fileFeatures.Columns.Add(header);
                    }
                    while (!sr.EndOfStream)
                    {
                        string[] rows = sr.ReadLine().Split(',');

                        if (rows[0] == "")
                            continue;

                        DataRow dr = fileFeatures.NewRow();
                        for (int i = 0; i < headers.Length; i++)
                        {
                            if (headers[i] == "packer")
                            {
                                dr[i] = packed;
                                continue;
                            }
                            if (headers[i] == "packer_type")
                            {
                                dr[i] = packer;
                                continue;
                            }

                            dr[i] = rows[i];
                        }

                        fileFeatures.Rows.Add(dr);
                    }
                }
                #endregion

                double evaluation = this.detection.model.symbolicTree.Evaluate(fileFeatures.Rows[0]);

                if (evaluation >= 0)
                {
                    string mnok = "Your file is malicious!";

                    CustomDialogBox dialog = new CustomDialogBox("Malicious", mnok, global::antico.Properties.Resources.nok_shield, MessageBoxButtons.OK);
                    dialog.ShowDialog();
                }
                else
                {
                    string mok = "Your file is benign!";

                    CustomDialogBox dialog = new CustomDialogBox("Benign", mok, global::antico.Properties.Resources.ok_shield, MessageBoxButtons.OK);
                    dialog.ShowDialog();
                }
                #endregion

                #region handle non-english charachters in path
                // Delete and move files at the end.
                if (File.Exists(downloadsPath + @"\\" + Path.GetFileName(script)))
                {
                    // Delete script from \Downloads directory.
                    File.Delete(downloadsPath + @"\\" + Path.GetFileName(script));
                }
                if (File.Exists(outputFile))
                {
                    // Move file to its true destination.
                    File.Move(outputFile, @"..\\..\\..\\..\\[PYTHON]\\ExtractedFeatures\\" + Path.GetFileName(outputFile));
                }
                if (File.Exists(downloadsPath + @"\\" + Path.GetFileName(file)))
                {
                    File.Delete(downloadsPath + @"\\" + Path.GetFileName(file));
                }
                if (Directory.Exists(downloadsPath + @"\\Python"))
                {
                    Directory.Delete(downloadsPath + @"\\Python", true);
                }
                #endregion

                // Delete downloaded peid.yara file.
                if (File.Exists(downloadsPath + @"\peid.yara"))
                {
                    File.Delete(downloadsPath + @"\peid.yara");
                }
            }
        }

        #region directory copy
        /// <summary>
        /// Helper method for copying whole directory.
        /// </summary>
        /// 
        /// <param name="targetDirPath">Path to target directory.</param>
        /// <param name="sourceDirPath">Path to source directory.</param>
        private static void CopyAll(DirectoryInfo targetDirPath, DirectoryInfo sourceDirPath)
        {

            Directory.CreateDirectory(targetDirPath.FullName);

            // Copy each file into the new directory.
            foreach (FileInfo fi in sourceDirPath.GetFiles())
            {
                fi.CopyTo(Path.Combine(targetDirPath.FullName, fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in sourceDirPath.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir = targetDirPath.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(nextTargetSubDir, diSourceSubDir);
            }
        }
        #endregion

        #endregion

        #region about antico 
        /// <summary>
        /// Forward user to form where he can learn about antico project.
        /// </summary>
        /// 
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AboutAnticoSign_MouseClick(object sender, MouseEventArgs e)
        {
            // About antico text.
            string input = "";
            input += "Antico is an aplication for generating a model for detecting malware\r\n";
            input += "using metaheuristic artificial bee colony programming.\r\n";

            input += "\r\n\r\n";
            input += "The source code can be found at:\r\n";
            input += "https://github.com/mateastanisic/antico";

            input += "\r\n\r\n\r\n\r\n\r\n\r\n\r\n";
            input += "\u00a9 Matea Stanišić (mateastanisic@outlook.com)\r\n";
            input += "University of Zagreb, Faculty of Science\r\n";
            input += "Department of mathematics, Graduate university programme in computer science and mathematics";

            // 
            // aboutAnticoLabel
            // 
            this.aboutAnticoLabel = new System.Windows.Forms.Label();
            this.aboutAnticoLabel.BackColor = System.Drawing.SystemColors.Control;
            this.aboutAnticoLabel.Font = new System.Drawing.Font("Source Sans Pro", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.aboutAnticoLabel.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.aboutAnticoLabel.Location = new System.Drawing.Point(120, 116);
            this.aboutAnticoLabel.Name = "aboutAnticoLabel";
            this.aboutAnticoLabel.Size = new System.Drawing.Size(772, 471);
            this.aboutAnticoLabel.TabIndex = 17;
            this.aboutAnticoLabel.Text = input;
            this.aboutAnticoLabel.AutoSize = false;
            this.aboutAnticoLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(this.aboutAnticoLabel);
            // Set up background of the label for the printout of the solutions.
            this.aboutAnticoLabel.BackColor = System.Drawing.Color.FromArgb(80, System.Drawing.Color.WhiteSmoke);

            // Hide signs for new form, file detection and about antico.
            this.aboutAnticoSign.Visible = false;
            this.createNewModelSign.Visible = false;
            this.isThisMaliciousSign.Visible = false;

            // Add go back sign.
            // 
            // goBackSign
            // 
            this.goBackSign = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.goBackSign)).BeginInit();
            this.goBackSign.BackColor = System.Drawing.Color.Transparent;
            this.goBackSign.Image = global::antico.Properties.Resources.back_white;
            this.goBackSign.Location = new System.Drawing.Point(858, 21);
            this.goBackSign.Name = "goBackSign";
            this.goBackSign.Size = new System.Drawing.Size(33, 31);
            this.goBackSign.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.goBackSign.TabIndex = 17;
            this.goBackSign.TabStop = false;
            this.goBackSign.MouseClick += new System.Windows.Forms.MouseEventHandler(this.GoBack_MouseClick);
            this.goBackSign.MouseEnter += new System.EventHandler(this.GoBack_MouseEnter);
            this.goBackSign.MouseLeave += new System.EventHandler(this.GoBack_MouseLeave);
            this.goBackSign.MouseHover += new System.EventHandler(this.GoBack_MouseHover);
            ((System.ComponentModel.ISupportInitialize)(this.goBackSign)).EndInit();
            this.Controls.Add(this.goBackSign);

        }

        #endregion

        #endregion

        #region Load model for detection
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
            this.detection = new UploadedModel();

            // Load values from file into UploadedModel class.
            foreach (DictionaryEntry de in addresses)
            {
                this.detection.LoadBasic(de.Key.ToString(), de.Value);
            }
        }
        #endregion

        #endregion

    }
    #endregion
}
