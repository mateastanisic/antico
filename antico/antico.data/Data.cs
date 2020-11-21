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
using System.Data;
using System.IO;
using System.Linq;

namespace antico.data
{
    #region Data class

    /// <summary>
    /// 
    /// Class for data needed for application.
    /// Here we define mathematical operations (non-terminals in the symbolic tree) and feature names. 
    /// We also connect to the postgresql database where features are being kept.
    /// 
    /// Even though main mathematical operators for this problem are +, -, *, /, rlog, exp, sin, cos, user will
    /// get a chance to choose wich operators to use in modeling by ABCP by simple checkbox option in the app.
    /// 
    /// 
    /// Every Data class is represented with
    ///     mathOperationsArity (fixed dictionary of all math. operators and their arity)
    ///     mathOperators (custom - choosed; math. operators)
    ///     feature names 
    ///     number of features
    ///     train and test data
    ///     number of folds (for cross validation, if used)
    ///     folds of train and test data (if number of folds != 0)
    ///     database name
    ///     connection and connection string to postgrsql database
    ///     fixed dictionary of all possible database names and it possible number of folds (with names)
    ///     
    /// 
    /// </summary>
    [Serializable]
    public class Data
    {
        #region ATTRIBUTES

        #region mathematical operations
        // (READONLY - Setting only through constructor)
        // Dictionary variable that represents mathematical operations. 
        // Key is the operation name and integer value represents possible arrity of the operation.
        // This variable is constant and it is never changed.
        private Dictionary<string, int> _mathOperationsArity = new Dictionary<string, int>()
        {
            { "+", 2 },
            { "-", 2 },
            { "*", 2 },
            { "/", 2 },
            { "sin", 1 },
            { "cos", 1 },
            { "rlog", 1 },
            { "exp", 1 }
        };

        // Property for the _mathOperationsArity variable.
        public Dictionary<string, int> mathOperationsArity
        {
            get { return _mathOperationsArity; }
        }

        // (READONLY - Setting only through constructor) 
        // Variable that represents all mathematical operations that will be used for making a model.
        private List<string> _mathOperators;

        // Property for the _mathOperators variable.
        public List<string> mathOperators
        {
            get { return _mathOperators; }
        }
        #endregion

        #region features 

        #region features names - connection to database
        // (READONLY - Setting only through constructor) 
        // String array containing feature names.
        private List<string> _featureNames;

        // Property for the _featureNames variable.
        public List<string> featureNames
        {
            get { return _featureNames; }
        }

        #endregion

        #region number of features
        // (READONLY - Setting only through constructor) 
        // Variable that represents number of features. 
        private int _numberOfFeatures;

        // Property for the _featureNames variable.
        public int numberOfFeatures
        {
            get { return _numberOfFeatures; }
        }
        #endregion

        #region train features
        // DataTable variable containing train features.
        private DataTable _trainFeatures;

        // Property for the _features variable.
        public DataTable trainFeatures
        {
            get { return _trainFeatures; }
            set 
            {
                _trainFeatures = new DataTable();
                _trainFeatures = value.Copy(); 
            }
        }
        #endregion

        #region folds
        // (READONLY - Setting only through constructor) 
        // Number representing number of folds.
        private int _numberOfFolds;

        // Property for the _numberOfFolds variable.
        public int numberOfFolds
        {
            get { return _numberOfFolds; }
        }

        // (READONLY - Setting only through constructor) 
        // List of DataTable variables containing train features of specific fold.
        private List<DataTable> _trainFeaturesFolds;

        // Property for the _trainFeaturesFolds variable.
        public List<DataTable> trainFeaturesFolds
        {
            get { return _trainFeaturesFolds; }
        }

        // (READONLY - Setting only through constructor) 
        // List of DataTable variables containing test features of specific fold.
        private List<DataTable> _testFeaturesFolds;

        // Property for the _testFeaturesFolds variable.
        public List<DataTable> testFeaturesFolds
        {
            get { return _testFeaturesFolds; }
        }
        #endregion

        #region test features
        // DataTable variable containing test features.
        private DataTable _testFeatures;

        // Property for the _features variable.
        public DataTable testFeatures
        {
            get { return _testFeatures; }
            set
            {
                _testFeatures = new DataTable();
                _testFeatures = value.Copy();
            }
        }
        #endregion

        #endregion

        #region database

        #region database name
        // (READONLY - Setting only through constructor) 
        // String variable containing name of the database.
        private string _databaseName;

        // Property for the _database variable.
        public string databaseName
        {
            get { return _databaseName; }
        }
        #endregion

        #region dictionary of all possible databases and its folds
        // (READONLY - Setting only through constructor) 
        // Dictionary for information about number of folds of specific database and their names.
        private Dictionary<string, Dictionary<int, List<string>>> _databaseFoldsAndNames = new Dictionary<string, Dictionary<int, List<string>>>()
        {
            ["clamp"] = new Dictionary<int, List<string>>()
            {
                [0] = new List<string>()
                {
                    "clamp_train_80",
                    "clamp_test_20"
                },
                [3] = new List<string>()
                {
                    "clamp_fold1_train",
                    "clamp_fold1_test",
                    "clamp_fold2_train",
                    "clamp_fold2_test",
                    "clamp_fold3_train",
                    "clamp_fold3_test"
                }
            },
            ["ember"] = new Dictionary<int, List<string>>()
            {
                [0] = new List<string>()
                {
                    "ember_train_selected5_641f",
                    "ember_test_selected5_641f"
                }
            },
            ["android"] = new Dictionary<int, List<string>>()
            {
                [0] = new List<string>()
                {
                    "android_train_80",
                    "android_test_20",
                    "android_train_70",
                    "android_test_30",
                    "android_train_selected_80",
                    "android_test_selected_20",
                    "android_train_selected_70",
                    "android_test_selected_30",
                    "android_train_selected_rfecv_80",
                    "android_test_selected_rfecv_20",
                    "android_train_selected_rfecv_70",
                    "android_test_selected_rfecv_30"

                },
                [3] = new List<string>()
                {
                    "android_train_fold1",
                    "android_test_fold1",
                    "android_train_fold2",
                    "android_test_fold2",
                    "android_train_fold3",
                    "android_test_fold3"
                }
            }
        };

        // Property for the _databaseFoldsAndNames variable.
        public Dictionary<string, Dictionary<int, List<string>>> databaseFoldsAndNames
        {
            get { return _databaseFoldsAndNames; }
        }

        // Property for getting database names.
        public List<string> databaseNames
        {
            get { return new List<string>(_databaseFoldsAndNames.Keys); }
        }

        #endregion

        #endregion

        #endregion

        #region OPERATIONS

        #region Constructors

        #region FOR TESTING 
        /// <summary>
        /// Constructor.
        /// Basic mathematical operators, loading features and feature names from postgresql database. 
        /// (FOR TESTING PURPOSES)
        /// </summary>
        public Data()
        {
            // Deafult mathematical operations.
            this._mathOperators = new List<string>(this._mathOperationsArity.Keys);

            // Set up database name.
            this._databaseName = "clamp";

            // Set up number of folds.
            List<string> databaseFoldsNames = SettingNumberOfFoldsAndReturnDatabaseNames(0);

            // Just in case, check if number of names is 2 -> train + test.
            if (databaseFoldsNames.Count != 2)
                throw new Exception("[Data basic constructor(1)] Number of database names (" + databaseFoldsNames.Count + ") is not maching desired (2).");

            // Load data to _trainFeatures from database with name databaseFoldsNames[0].
            LoadTrain(databaseFoldsNames, 0, false);

            // Load data to _testFeatures from database with name databaseFoldsNames[1].
            LoadTest(databaseFoldsNames, 1, false);

            // Set up number of features.
            // DataTable _trainFeatures contains column label for classification so number of features is down for one.
            this._numberOfFeatures = this._trainFeatures.Columns.Count - 1;

            // Check up number of features.
            if (_numberOfFeatures != _testFeatures.Columns.Count - 1)
                throw new Exception("[Data basic constructor(1)] Number of features in train (" + this._numberOfFeatures + ") and test (" + (this._testFeatures.Columns.Count - 1) + ") set do not match.");

            // Load feature names from database.
            LoadFeatureNames(databaseFoldsNames[0]);
        }
        #endregion

        #region WITH PARAMETERS
        /// <summary>
        /// Constructor. 
        /// With specific mathematical operators and specific database.
        /// </summary>
        /// 
        /// <param name="mathOperators"> String array containing choosen mathematical operators for training a model.</param>
        /// <param name="databaseName"> Name of the database. </param>
        /// <param name="nof">Number of folds.</param>
        public Data(List<string> mathOperators, string databaseName, int nof)
        {
            // Deafult mathematical operations.
            this._mathOperators = new List<string>(mathOperators);

            // Set up database name.
            this._databaseName = databaseName;

            // Set up number of folds. 
            // TODO: custom number of folds
            List<string> databaseFoldsNames = SettingNumberOfFoldsAndReturnDatabaseNames(nof);

            // Setting up the variables based on number of folds.
            if (this._numberOfFolds == 0)
            {
                if (databaseFoldsNames.Count < 2)
                    throw new Exception("[Data constructor(2)] Number of database names (" + databaseFoldsNames.Count + ") is not maching desired (2).");

                // Load data to _trainFeatures from database with name databaseFoldsNames[0].
                LoadTrain(databaseFoldsNames, 0, false);

                // Load data to _testFeatures from database with name databaseFoldsNames[1].
                LoadTest(databaseFoldsNames, 1, false);

                // Set up number of features.
                // DataTable _trainFeatures contains column label for classification so number of features is down for one.
                this._numberOfFeatures = this._trainFeatures.Columns.Count - 1;

                // Check up number of features.
                if (this._numberOfFeatures != this._testFeatures.Columns.Count - 1)
                    throw new Exception("[Data basic constructor(2)] Number of features in train (" + this._numberOfFeatures + ") and test (" + (this._testFeatures.Columns.Count - 1) + ") set do not match.");

                // Load feature names from database.
                LoadFeatureNames(databaseFoldsNames[0]);
            }
            else
            {
                if (databaseFoldsNames.Count != this._numberOfFolds * 2)
                    throw new Exception("[Data constructor(2)] Number of database names (" + databaseFoldsNames.Count + ") is not maching desired (" + this._numberOfFolds *2 + ").");

                #region trainFeatures & testFeatures
                // For setting trainFeatures i testFeatures variables.
                List<string> databaseZeroFolds = databaseFoldsAndNames[this._databaseName][0];

                if (databaseZeroFolds.Count < 2)
                    throw new Exception("[Data constructor(2)] Number of database names (" + databaseZeroFolds.Count + ") is not maching desired (2).");

                // Load data to _trainFeatures from database with name databaseZeroFolds[0].
                LoadTrain(databaseZeroFolds, 0, false);

                // Load data to _testFeatures from database with name databaseZeroFolds[1].
                LoadTest(databaseZeroFolds, 1, false);
                #endregion

                #region trainFeaturesFolds & testFeaturesFolds
                // Initialization.
                this._trainFeaturesFolds = new List<DataTable>();
                this._testFeaturesFolds = new List<DataTable>();

                // Load data to _trainFeaturesFolds list and _testFeaturesFolds list.
                for (var i = 0; i < this._numberOfFolds; i++)
                {
                    // Load data to _trainFeaturesFolds from database with name databaseFoldsNames[i*2].
                    LoadTrain(databaseFoldsNames, i*2, true);

                    // Load data to _testFeaturesFolds from database with name databaseFoldsNames[i*2+1].
                    LoadTest(databaseFoldsNames, i*2 + 1, true);

                    // Set up and check numberOfFeatures.
                    if (i == 0)
                    {
                        // Set up number of features.
                        // DataTable _trainFeatures contains column label for classification so number of features is down for one.
                        this._numberOfFeatures = this._trainFeaturesFolds[0].Columns.Count - 1;

                        // Check up number of features.
                        if (this._numberOfFeatures != this._testFeaturesFolds[0].Columns.Count - 1)
                            throw new Exception("[Data basic constructor(1)] Number of features in train (" + this._numberOfFeatures + ") and test (" + (this._testFeaturesFolds[0].Columns.Count - 1) + ") set do not match.");
                    }
                    else
                    {
                        // Check up number of features.
                        if (this._numberOfFeatures != this._trainFeaturesFolds[i].Columns.Count - 1)
                            throw new Exception("[Data basic constructor(1)] Number of features in train (" + this._numberOfFeatures + ") and test (" + (this._trainFeaturesFolds[i].Columns.Count - 1) + ") set do not match.");

                        // Check up number of features.
                        if (this._numberOfFeatures != this._testFeaturesFolds[i].Columns.Count - 1)
                            throw new Exception("[Data basic constructor(1)] Number of features in train (" + this._numberOfFeatures + ") and test (" + (this._testFeaturesFolds[i].Columns.Count - 1) + ") set do not match.");
                    }
                }
                #endregion

                // Load feature names from database.
                LoadFeatureNames(databaseFoldsNames[0]);
            }
        }
        #endregion

        #endregion

        #region Get mathematical operator arity
        /// <summary>
        /// Helper method for determinating aritiy of mathematical operators.
        /// </summary>
        /// 
        /// <param name="mathOp">Mathematical operator string.</param>
        /// <returns>Aritiy of the sent mathematical operator.</returns>
        internal int getMathOperationArity(string mathOp)
        {
            if (mathOperationsArity.ContainsKey(mathOp))
                return mathOperationsArity[mathOp];
            else
                throw new Exception("[Data::getMathOperationArity] Sent mathematical operator is not knows.");
        }
        #endregion

        #region Number of folds and its names
        /// <summary>
        /// Method for setting up number of folds of train set and returning desired names of database folds names.
        /// </summary>
        /// 
        /// <param name="folds">Number of folds of the train dataset.</param>
        /// <returns>List of database names splited in specific number of folds.</returns>
        private List<string> SettingNumberOfFoldsAndReturnDatabaseNames(int folds)
        {
            // One fold is actually zero folds.
            if (folds == 1)
            {
                folds = 0;
            }

            // Check if database name is set up.
            if (this._databaseName == "")
                throw new Exception("[SettingNumberOfFolds] Database name is not defined.");

            // Check if databaseName is in dictionary of known databases.
            if (!this._databaseFoldsAndNames.Keys.Contains<string>(this._databaseName))
                throw new Exception("[SettingNumberOfFolds] Database '" + this._databaseName + "' is not defined.");

            // Check if database with specific number of folds exists.
            if (!this._databaseFoldsAndNames[this._databaseName].Keys.Contains<int>(folds))
                throw new Exception("[SettingNumberOfFolds] Database '" + this._databaseName + "' doesn't have option of " + folds.ToString() + " folds.");

            // Check if database with 0 folds exists. (Every database should have this version also.)
            if (!this._databaseFoldsAndNames[this._databaseName].Keys.Contains<int>(0))
                throw new Exception("[SettingNumberOfFolds] Database '" + this._databaseName + "' doesn't have option of " + 0 + " folds.");

            // Everything is fine, set up number of folds.
            this._numberOfFolds = folds;

            // Return names of folds.
            return new List<string>(this._databaseFoldsAndNames[this._databaseName][this._numberOfFolds]);
        }
        #endregion

        #region Loading data from databases

        #region train
        /// <summary>
        /// Load data into trainFeatures(Folds) variable from specific database.
        /// </summary>
        /// 
        /// <param name="databaseNames">List of names of the databases. </param>
        /// <param name="index">Index of the database in the databaseNames list.</param>
        /// <param name="flag">Flag to know if trainFeatures - 0 should filled or trainFeaturesFold - 1.</param>
        private void LoadTrain(List<string> databaseNames, int index, bool flag)
        {
            // Check if index is out of bounds for databaseNames.
            if (databaseNames.Count <= index)
                throw new Exception("[LoadTrain] Index " + index + " is out of bounds (" + databaseNames.Count + ").");

            // Define train database name.
            string trainDatabaseName = databaseNames[index];

            // Load data to trainFeatures from database (.csv file).

            if (flag == false)
            {
                // Define path to database file.
                string path_to_database = "../../../../[DATA]/database/" + this._databaseName + "/0/" + trainDatabaseName + ".csv";

                if (!File.Exists(Path.GetFullPath(path_to_database)))
                    throw new Exception("[LoadTrain] File " + trainDatabaseName + ".csv on path " + path_to_database + " does not exists!");

                // Load data into trainFeatures variable.
                this._trainFeatures = new DataTable("Train Features.");

                using (StreamReader sr = new StreamReader(path_to_database))
                {
                    string[] headers = sr.ReadLine().Split(',');
                    foreach (string header in headers)
                    {
                        this._trainFeatures.Columns.Add(header);
                    }
                    while (!sr.EndOfStream)
                    {
                        string[] rows = sr.ReadLine().Split(',');
                        DataRow dr = this._trainFeatures.NewRow();
                        for (int i = 0; i < headers.Length; i++)
                        {
                            dr[i] = rows[i];
                        }
                        this._trainFeatures.Rows.Add(dr);
                    }
                }
            }
            else
            {
                // Define path to database file.
                string path_to_database = "../../../../[DATA]/database/" + this._databaseName + "/" + this._numberOfFolds + "/" + trainDatabaseName + ".csv";

                if (!File.Exists(Path.GetFullPath(path_to_database)))
                    throw new Exception("[LoadTrain] File " + trainDatabaseName + ".csv on path " + path_to_database + " does not exists!");

                // Load data into trainFeaturesFolds variable.
                DataTable temp = new DataTable("Train Features Folds");

                using (StreamReader sr = new StreamReader(path_to_database))
                {
                    string[] headers = sr.ReadLine().Split(',');
                    foreach (string header in headers)
                    {
                        temp.Columns.Add(header);
                    }
                    while (!sr.EndOfStream)
                    {
                        string[] rows = sr.ReadLine().Split(',');
                        DataRow dr = temp.NewRow();
                        for (int i = 0; i < headers.Length; i++)
                        {
                            dr[i] = rows[i];
                        }
                        temp.Rows.Add(dr);
                    }

                }

                this._trainFeaturesFolds.Add(temp.Copy());
            }

        }
        #endregion

        #region test
        /// <summary>
        /// Load data into testFeatures(Folds) variable from specific database.
        /// </summary>
        /// 
        /// <param name="databaseNames">List of names of the databases. </param>
        /// <param name="index">Index of the database in the databaseNames list.</param>
        /// <param name="flag">Flag to know if testFeatures(0) should filled or testFeaturesFold(1).</param>
        private void LoadTest(List<string> databaseNames, int index, bool flag)
        {
            // Check if index is out of bounds for databaseFoldsNames.
            if (databaseNames.Count <= index)
                throw new Exception("[LoadTest] Index " + index + " is out of bounds (" + databaseNames.Count + ").");

            // Define test database name.
            string testDatabaseName = databaseNames[index];

            // Load data to testFeatures from database (.csv file).
            if (flag == false)
            {
                // Define path to database file.
                string path_to_database = "../../../../[DATA]/database/" + this._databaseName + "/0/" + testDatabaseName + ".csv";

                if (!File.Exists(Path.GetFullPath(path_to_database)))
                    throw new Exception("[LoadTest] File " + testDatabaseName + ".csv on path " + path_to_database + " does not exists!");

                // Load data into testFeatures variable.
                this._testFeatures = new DataTable();

                using (StreamReader sr = new StreamReader(path_to_database))
                {
                    string[] headers = sr.ReadLine().Split(',');
                    foreach (string header in headers)
                    {
                        this._testFeatures.Columns.Add(header);
                    }
                    while (!sr.EndOfStream)
                    {
                        string[] rows = sr.ReadLine().Split(',');
                        DataRow dr = this._testFeatures.NewRow();
                        for (int i = 0; i < headers.Length; i++)
                        {
                            dr[i] = rows[i];
                        }
                        this._testFeatures.Rows.Add(dr);
                    }
                }
            }
            else
            {
                // Define path to database file.
                string path_to_database = "../../../../[DATA]/database/" + this._databaseName + "/" + this._numberOfFolds + "/" + testDatabaseName + ".csv";

                if (!File.Exists(Path.GetFullPath(path_to_database)))
                    throw new Exception("[LoadTest] File " + testDatabaseName + ".csv on path " + path_to_database + " does not exists!");

                // Load data into testFeaturesFolds variable.
                DataTable temp = new DataTable();

                using (StreamReader sr = new StreamReader(path_to_database))
                {
                    string[] headers = sr.ReadLine().Split(',');
                    foreach (string header in headers)
                    {
                        temp.Columns.Add(header);
                    }
                    while (!sr.EndOfStream)
                    {
                        string[] rows = sr.ReadLine().Split(',');
                        DataRow dr = temp.NewRow();
                        for (int i = 0; i < headers.Length; i++)
                        {
                            dr[i] = rows[i];
                        }
                        temp.Rows.Add(dr);
                    }

                }
                this._testFeaturesFolds.Add(temp.Copy());
            }

        }
        #endregion

        #region feature names
        /// <summary>
        /// Loads feature names from specific database into a _featuresNames variable.
        /// </summary>
        /// 
        /// <param name="database">Name of the database from where feature names will be collected.</param>
        private void LoadFeatureNames(string database)
        {
            // Try loading feature names from database.

            // Initialize _featureNames.
            this._featureNames = new List<string>();

            // Define path to database file.
            string path_to_database = "../../../../[DATA]/database/" + this._databaseName + "/" + this._numberOfFolds + "/" + database + ".csv";

            if (!File.Exists(Path.GetFullPath(path_to_database)))
                throw new Exception("[LoadFeatureNames] File " + database + ".csv on path " + path_to_database + " does not exists!");

            using (StreamReader sr = new StreamReader(path_to_database))
            {
                string[] headers = sr.ReadLine().Split(',');
                foreach (string header in headers)
                {
                    // (TODO: HARDCODED) Column "label" is not a feature!
                    if (header.ToString() == "label")
                        continue;

                    this._featureNames.Add(header.ToString());
                }

            }

        }
        #endregion

        #endregion

        #endregion
    }
    #endregion
}
