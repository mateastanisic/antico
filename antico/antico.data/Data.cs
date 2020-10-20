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
using System.Linq;
using Npgsql;

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

        #region connection to database
        // Variable for connection to POSTGRESQL database.
        // ( PRIVATE ) used only in this class
        private NpgsqlConnection connection;

        // Connection string for connection to the database.
        // ( PRIVATE ) used only in this class
        private string connectionString = String.Format("Server={0};Port={1};User Id={2};Password={3};Database={4};", "localhost", "5432", "postgres", "postgres", "MalwareDetection");
        #endregion

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
                    "clamp_folds1_train_80_20",
                    "clamp_folds1_test_80_20",
                    "clamp_folds2_train_80_20",
                    "clamp_folds2_test_80_20",
                    "clamp_folds3_train_80_20",
                    "clamp_folds3_test_80_20"
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

            // Defining connection.
            this.connection = new NpgsqlConnection(this.connectionString);

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
        public Data(List<string> mathOperators, string databaseName)
        {
            // Deafult mathematical operations.
            this._mathOperators = new List<string>(mathOperators);

            // Defining connection.
            this.connection = new NpgsqlConnection(this.connectionString);

            // Set up database name.
            this._databaseName = databaseName;

            // Set up number of folds. 
            // TODO: custom number of folds
            List<string> databaseFoldsNames = SettingNumberOfFoldsAndReturnDatabaseNames(0);

            // Setting up the variables based on number of folds.
            if (this._numberOfFolds == 0)
            {
                if (databaseFoldsNames.Count != 2)
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
                    throw new Exception("[Data basic constructor(1)] Number of features in train (" + this._numberOfFeatures + ") and test (" + (this._testFeatures.Columns.Count - 1) + ") set do not match.");

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

                if (databaseZeroFolds.Count != 2)
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
        /// <param name="flag">Flag to know if trainFeatures(0) should filled or trainFeaturesFold(1).</param>
        private void LoadTrain(List<string> databaseNames, int index, bool flag)
        {
            // Check if index is out of bounds for databaseNames.
            if (databaseNames.Count <= index)
                throw new Exception("[LoadTrain] Index " + index + " is out of bounds (" + databaseNames.Count + ").");

            // Define train database name.
            string trainDatabaseName = databaseNames[index];

            // Try loading data into train features DataTable from database.
            try
            {
                // Open connection.
                this.connection.Open();

                // SQL query.
                string sql_features = "SELECT * FROM " + trainDatabaseName;
                NpgsqlCommand command = new NpgsqlCommand(sql_features, this.connection);

                if (flag == false)
                {
                    // Load data into trainFeatures variable.
                    this._trainFeatures = new DataTable();
                    this._trainFeatures.Load(command.ExecuteReader());
                }
                else
                {
                    // Load data into trainFeaturesFolds variable.
                    DataTable temp = new DataTable();
                    temp.Load(command.ExecuteReader());
                    this._trainFeaturesFolds.Add(temp.Copy());
                }

                // Close the connection.
                this.connection.Close();
            }
            catch
            {
                // Could not open the database. Throw exception.
                this.connection.Close();
                throw new NpgsqlException("[LoadTrain] Failed loading data from database " + this._databaseName + " (" + trainDatabaseName + ")!");
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

            // Try loading data into test features DataTable from database.
            try
            {
                // Open connection.
                this.connection.Open();

                // SQL query.
                string sql_features = "SELECT * FROM " + testDatabaseName;
                NpgsqlCommand command = new NpgsqlCommand(sql_features, this.connection);

                if (flag == false)
                {
                    // Load data into testFeatures variable.
                    this._testFeatures = new DataTable();
                    this._testFeatures.Load(command.ExecuteReader());
                }
                else
                {
                    // Load data into testFeaturesFolds variable.
                    DataTable temp = new DataTable();
                    temp.Load(command.ExecuteReader());
                    this._testFeaturesFolds.Add(temp.Copy());
                }

                // Close the connection.
                this.connection.Close();
            }
            catch
            {
                // Could not open the database. Throw exception.
                this.connection.Close();
                throw new NpgsqlException("[LoadTest] Failed loading data from database " + this._databaseName + " (" + testDatabaseName + ")!");
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
            try
            {
                // Open connection.
                this.connection.Open();

                // SQL query.
                string sql_feature_names = "SELECT column_name FROM information_schema.columns WHERE TABLE_NAME = '" + database + "'";
                NpgsqlCommand command2 = new NpgsqlCommand(sql_feature_names, this.connection);

                // Loading feature names into _featureNames variable variable.
                DataTable featureNamesDataTable = new DataTable();
                featureNamesDataTable.Load(command2.ExecuteReader());

                // Initialize _featureNames.
                this._featureNames = new List<string>();

                // Fill string array with feature names from DataTable.
                foreach (DataRow row in featureNamesDataTable.Rows)
                {
                    foreach (var item in row.ItemArray)
                    {
                        // (TODO: HARDCODED) Column "label" is not a feature!
                        if (item.ToString() == "label")
                            continue;

                        this._featureNames.Add(item.ToString());
                    }
                }

                // Close the connection.
                this.connection.Close();
            }
            catch
            {
                // Could not open the database. Throw exception.
                this.connection.Close();
                throw new NpgsqlException("[Data basic constructor(1)] Failed loading data from database " + this._databaseName + " (" + database + ")!");
            }
        }
        #endregion

        #endregion

        #endregion

    }
    #endregion
}
