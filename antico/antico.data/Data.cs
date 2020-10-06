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
using System.Runtime.CompilerServices;
using Npgsql;
// using Microsoft.ML;

namespace antico.data
{
    #region Data class

    /// <summary>
    /// Class for data needed for application.
    /// Here we define mathematical operations (non-terminals in the symbolic tree) and feature names. 
    /// We also connect to the postgresql database where features are being kept.
    /// 
    /// Even though main mathematical operators for this problem are +,-,*,/,log,exp,sin,cos, user will
    /// get a chance to choose wich operators to use in modeling by ABCP by simple checkbox option in the app.
    /// 
    /// TOBE IMPLEMENTED: (since features are still not collected)
    /// Depending on the feature extraction method (TF/TFIDF/Fisher score) user will have a chance to choose feature set. 
    /// Also, user will also have a chance to choose making model based on balanced or imbalanced data.
    /// 
    /// </summary>
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

        // Number of mathematical operators.
        // (READONLY - Setting only through constructor)
        private int _numberOfMathOperators;

        // Property for the _numberOfMathOperators variable.
        public int numberOfMathOperators
        {
            get { return _numberOfMathOperators; }
        }

        // (READONLY - Setting only through constructor) 
        // Variable that represents all mathematical operations that will be used for making a model.
        private string[] _mathOperations;

        // Property for the _mathOperations variable.
        public string[] mathOperations
        {
            get { return _mathOperations; }
        }
        #endregion

        #region features 

        #region features - connection to database
        // Variable for connection to POSTGRESQL database.
        // ( PRIVATE ) used only in this class
        private NpgsqlConnection connection;

        // Connection string for connection to the database.
        // ( PRIVATE ) used only in this class
        private string connectionString = String.Format("Server={0};Port={1};User Id={2};Password={3};Database={4};", "localhost", "5432", "postgres", "postgres", "MalwareDetection");

        // (READONLY - Setting only through constructor) 
        // Extracted features from database.
        private DataTable _features;

        // Property for the _features variable.
        public DataTable features
        {
            get { return _features; }
        }
        #endregion

        #region features names - connection to database
        // (READONLY - Setting only through constructor) 
        // String array containing feature names.
        private string[] _featureNames;

        // Property for the _featureNames variable.
        public string[] featureNames
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
        // (READONLY - Setting only through constructor) 
        // DataTable variable containing train features.
        private DataTable _trainFeatures;

        // Property for the _features variable.
        public DataTable trainFeatures
        {
            get { return _trainFeatures; }
        }
        #endregion

        #region test features
        // (READONLY - Setting only through constructor) 
        // DataTable variable containing test features.
        private DataTable _testFeatures;

        // Property for the _features variable.
        public DataTable testFeatures
        {
            get { return _testFeatures; }
        }
        #endregion

        #endregion

        #endregion

        #region OPERATIONS

        #region Constructor
        /// <summary>
        /// Constructor.
        /// Basic mathematical operators, loading features and feature names from postgresql database. 
        /// (FOR TESTING PURPOSES)
        /// </summary>
        public Data( )
        {
            // Deafult mathematical operations.
            _mathOperations = new string[] { "+", "-", "*", "/", "sin", "cos", "rlog", "exp"};
            
            // Setting number of mathematical operations.
            _numberOfMathOperators = _mathOperations.Length;

            // Defining connection.
            connection = new NpgsqlConnection(connectionString);

            // Try loading data into features DataTable from database.
            try
            {
                // Open connection.
                connection.Open();

                // SQL query.
                string sql_features = "SELECT * FROM clamp";
                NpgsqlCommand command = new NpgsqlCommand(sql_features, connection);

                // Loading data in DataTable variable.
                _features = new DataTable();
                _features.Load(command.ExecuteReader());

                // Allocate memory for feature names. (Remove one for label column)
                // TODO: prettier
                _featureNames = new string[_features.Columns.Count - 1];

                // Close the connection.
                connection.Close();
            }
            catch
            {
                // Could not open the database. Throw exception.
                connection.Close();
                throw new NpgsqlException("Failed loading data from database!");
            }

            // Try loading feature names from database.
            try
            {
                // Open connection.
                connection.Open();

                // SQL query.
                string sql_feature_names = "SELECT column_name FROM information_schema.columns WHERE TABLE_NAME = 'clamp'";
                NpgsqlCommand command2 = new NpgsqlCommand(sql_feature_names, connection);

                // Loading feature names into _featureNames variable variable.
                DataTable featureNamesDataTable = new DataTable();
                featureNamesDataTable.Load(command2.ExecuteReader());

                // Fill string array with feature names from DataTable.
                var i = 0;
                foreach (DataRow row in featureNamesDataTable.Rows)
                {
                    foreach (var item in row.ItemArray)
                    {
                        // Column "label" is not a feature!
                        if (item.ToString() == "label")
                            continue;

                        _featureNames[i] = item.ToString();
                        i++;
                    }

                }

                // Close the connection.
                connection.Close();
            }
            catch
            {
                // Could not open the database. Throw exception.
                connection.Close();
                throw new NpgsqlException("Failed loading data from database!");
            }


            // Setting number of the features.
            _numberOfFeatures = _featureNames.Length;

            // Balanced division of data into train and test.
            // makeTrainAndTest();
        }

        /// <summary>
        /// Constructor. 
        /// With specific mathematical operators, specific features (depending on the method) 
        /// and specific type of train/test data (balanced or inbalanced).
        /// </summary>
        /// <param name="mathOperators"> String array containing choosen mathematical operators for training a model.</param>
        /// <param name="featureExtractionMethod"> Feature extraction method used - tf/tfidf/fisher. </param>
        /// <param name="trainingDataType"> Balanced or inbalanced data. </param>
        /// <param name="numberOfBestFeatures">Number of best features used in predicting a model.</param>
        public Data( string[] mathOperators, string featureExtractionMethod, string trainingDataType, int numberOfBestFeatures)
        {
            // Choosed mathematical operations
            _mathOperations = new string[mathOperators.Length];

            // Deep copy.
            for (var i = 0; i < mathOperators.Length; i++)
            {
                _mathOperations[i] = mathOperators[i];
            }

            // Setting number of mathematical operations.
            _numberOfMathOperators = _mathOperations.Length;

            string database;

            // Depending on the desired feature extraction method, define different database for loading data.
            switch (featureExtractionMethod)
            {
                case "TF":
                    database = "malware_detection_features_tf";
                    break;
                case "TFIDF":
                    database = "malware_detection_features_tfidf";
                    break;
                case "fisher":
                    database = "malware_detection_features_fisher";
                    break;
                default:
                    throw new Exception("Desired feature extraction method is not available.");
            }

            // Depending on the desired number of the top features, define different database for loading data.
            switch (numberOfBestFeatures)
            {
                case 50:
                    database += "_50";
                    break;
                case 100:
                    database += "_100";
                    break;
                case 200:
                    database += "_200";
                    break;
                default:
                    throw new Exception("Desired number of best features is not available.");
            }

            // Defining connection.
            connection = new NpgsqlConnection(connectionString);

            // Try loading data into features DataTable from database.
            try
            {
                // Open connection.
                connection.Open();

                // SQL query.
                string sql_features = "SELECT * FROM " + database;
                NpgsqlCommand command = new NpgsqlCommand(sql_features, connection);

                // Loading data in DataTable variable.
                _features = new DataTable();
                _features.Load(command.ExecuteReader());

                // Close the connection.
                connection.Close();
            }
            catch
            {
                // Could not open the database. Throw exception.
                connection.Close();
                throw new NpgsqlException("Failed loading data from database!");
            }

            // Try loading feature names from database.
            try
            {
                // Open connection.
                connection.Open();

                // SQL query.
                string sql_feature_names = "SELECT column_name FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = N'malware_detection_features'";
                NpgsqlCommand command2 = new NpgsqlCommand(sql_feature_names, connection);

                // Loading feature names into _featureNames variable variable.
                DataTable featureNamesDataTable = new DataTable();
                featureNamesDataTable.Load(command2.ExecuteReader());

                // Allocate memory for feature names.
                _featureNames = new string[featureNamesDataTable.Rows.Count];

                // Fill string array with feature names from DataTable.
                var i = 0;
                foreach (DataRow row in featureNamesDataTable.Rows)
                {
                    _featureNames[i] = (row.ItemArray).ToString();
                    i++;
                }

                // Close the connection.
                connection.Close();
            }
            catch
            {
                // Could not open the database. Throw exception.
                connection.Close();
                throw new NpgsqlException("Failed loading data from database!");
            }

            // Setting number of the features.
            _numberOfFeatures = _featureNames.Length;

            // Divide data into train and test data.
            switch (trainingDataType)
            {
                case "balanced":
                    makeTrainAndTest();
                    break;
                case "inbalanced":
                    makeTrainAndTestInbalanced();
                    break;
                default:
                    throw new Exception("Balanced and inbalanced division of the data is only allowed.");
            }
        }
        #endregion

        #region get mathematical operator arity
        /// <summary>
        /// Helper method for determinating aritiy of mathematical operators.
        /// </summary>
        /// <param name="mathOp">Mathematical operator string.</param>
        /// <returns>Aritiy of the sent mathematical operator.</returns>
        public int getMathOperationArity(string mathOp)
        {
            if (mathOperationsArity.ContainsKey(mathOp))
            {
                return mathOperationsArity[mathOp];
            }
            else
            {
                throw new Exception("Sent mathematical operator is not knows.");
            }
        }
        #endregion

        #region divide features into train and test data
        private void makeTrainAndTest()
        {
            throw new NotImplementedException();
        }

        private void makeTrainAndTestInbalanced()
        {
            throw new NotImplementedException();
        }
        #endregion

        #endregion

    }

    #endregion
}
