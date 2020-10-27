//////////////////////////////////////////////////////////////////////////////////////////
// antico --- artificial bee colony programming based malware detection                 //
// Copyright 2020 Matea Stanišić                                                        //
//                                                                                      //
//                                                                                      //
// Matea Stanišić                                                                       //
// mateastanisic@outlook.com                                                            //
// Zagreb, Hrvatska                                                                     //
//////////////////////////////////////////////////////////////////////////////////////////

using antico.abcp;
using System;
using System.Collections.Generic;
using System.Windows;

namespace antico
{
    #region UploadedModel
    /// 
    /// <summary>
    /// 
    /// UploadedModel is a class for memorizing data from uploaded file - model.
    /// 
    /// Every UploadedModel has a:
    ///     - version of the saved model
    ///     - model (Chromosome)
    ///     - parameters of the model
    ///     - name of the database on which model was found
    ///     - names of the features used while creating a model
    ///     - number of folds of the data
    ///     - mathematical operators used while creating a model
    ///     - fitness points over iterations (train and test)
    ///     - TN, TP, FN, FP points over iterations (and together)
    ///     - depths points over iterations
    ///     - console output during search
    ///     
    /// 
    /// </summary>
    /// 
    class UploadedModel
    {
        #region ATTRIBUTES

        // "Save" version. 
        public int version;

        // Saved solution - model - chromosome.
        public Chromosome model;

        // Choosen run of all runs in that search.
        public int choosenRun;

        // Parameters of the model.
        public Parameters parameters;

        // Name of the database.
        public string databaseName;

        // Name of the features.
        private List<string> featuresNames;

        // Number of folds of data.
        private int numberOfFolds;

        // Mathematical opeators used.
        public List<string> mathOperators;

        // Points of fitness of best solution (train - Item1, and test - Item2 in tuple) over Iterations in runs (Dictionary key represent run.)
        public Dictionary<int, Tuple<List<double>, List<double>>> fitnessPoints;

        // Points of number of true positives of best solution (train - Item1, and test - Item2 in tuple) over Iterations in runs (Dictionary key represents run).
        private Dictionary<int, Tuple<List<int>, List<int>>> tpPoints;
        // Points of number of true negtives of best solution (train - Item1, and test - Item2 in tuple) over Iterations in runs (Dictionary key represents run).
        private Dictionary<int, Tuple<List<int>, List<int>>> tnPoints;
        // Points of number of false positives of best solution (train - Item1, and test - Item2 in tuple) over Iterations in runs (Dictionary key represents run).
        private Dictionary<int, Tuple<List<int>, List<int>>> fpPoints;
        // Points of number of false negatives of best solution (train - Item1, and test - Item2 in tuple) over Iterations in runs (Dictionary key represents run).
        private Dictionary<int, Tuple<List<int>, List<int>>> fnPoints;

        // "Accuracy" (TN,TP,FN,FP) points over Iterations in runs (Dictionary key represents run).
        public Dictionary<int, Tuple<List<int>, List<int>, List<int>, List<int>>> accuracyPointsTrain;

        // Points of maximal depths of best tree solutions over Iterations in runs (Dictionary key represent run.)
        public Dictionary<int, List<int>> depthPoints;

        // Console output of the search for the model.
        public string consoleOutput;

        #endregion

        #region OPERATIONS

        #region Constructor.
        /// <summary>
        /// Basic constructor.
        /// </summary>
        public UploadedModel()
        {
            this.version = -1;
            this.model = new Chromosome();
            this.databaseName = "";
            this.featuresNames = new List<string>();
            this.numberOfFolds = -1;
            this.mathOperators = new List<string>();
            this.fitnessPoints = new Dictionary<int, Tuple<List<double>, List<double>>>();
            this.tpPoints = new Dictionary<int, Tuple<List<int>, List<int>>>();
            this.tnPoints = new Dictionary<int, Tuple<List<int>, List<int>>>();
            this.fpPoints = new Dictionary<int, Tuple<List<int>, List<int>>>();
            this.fnPoints = new Dictionary<int, Tuple<List<int>, List<int>>>();
            this.accuracyPointsTrain = new Dictionary<int, Tuple<List<int>, List<int>, List<int>, List<int>>>();
            this.depthPoints = new Dictionary<int, List<int>>();
            this.consoleOutput = "";
            this.choosenRun = 0;
        }
        #endregion

        #region Load data into attributes.
        /// <summary>
        /// Loads data into one attribute.
        /// </summary>
        /// 
        /// <param name="keyName">Name of the attribute.</param>
        /// <param name="keyValue">(ToBe)Value of the attribute.</param>
        /// <param name="progressForm">Progress form whose dictionary of points needs to be updated.</param>
        public void Load(string keyName, object keyValue, ref ProgressForm progressForm)
        {
            if (keyName == "Solution")
            {
                this.model = (Chromosome)keyValue;
                return;
            }
            else if (keyName == "SAVEVersion")
            {
                this.version = (int)keyValue;
                return;
            }
            else if (keyName == "Parameters")
            {
                this.parameters = new Parameters((Parameters)keyValue);
                return;
            }
            else if (keyName == "DatabaseName")
            {
                this.databaseName = (string)keyValue;
                return;
            }
            else if (keyName == "FeatureNames")
            {
                this.featuresNames = (List<string>)keyValue;
                return;
            }
            else if (keyName == "NumberOfFolds")
            {
                this.numberOfFolds = (int)keyValue;
                return;
            }
            else if (keyName == "MathOperators")
            {
                this.mathOperators = (List<string>)keyValue;
                return;
            }
            else if (keyName == "FitnessProgress")
            {
                this.fitnessPoints = (Dictionary<int, Tuple<List<double>, List<double>>>)keyValue;

                // Update fitness points in progress form.
                progressForm.fitnessPoints = new Dictionary<int, Tuple<List<double>, List<double>>>();
                progressForm.fitnessPoints = this.fitnessPoints;

                return;
            }
            else if (keyName == "TPProgress")
            {
                this.tpPoints = (Dictionary<int, Tuple<List<int>, List<int>>>)keyValue;

                // Update TP points in progress form.
                progressForm.tpPoints = new Dictionary<int, Tuple<List<int>, List<int>>>();
                progressForm.tpPoints = this.tpPoints;

                return;
            }
            else if (keyName == "TNProgress")
            {
                this.tnPoints = (Dictionary<int, Tuple<List<int>, List<int>>>)keyValue;

                // Update TN points in progress form.
                progressForm.tnPoints = new Dictionary<int, Tuple<List<int>, List<int>>>();
                progressForm.tnPoints = this.tnPoints;

                return;
            }
            else if (keyName == "FPProgress")
            {
                this.fpPoints = (Dictionary<int, Tuple<List<int>, List<int>>>)keyValue;

                // Update FP points in progress form.
                progressForm.fpPoints = new Dictionary<int, Tuple<List<int>, List<int>>>();
                progressForm.fpPoints = this.fpPoints;

                return;
            }
            else if (keyName == "FNProgress")
            {
                this.fnPoints = (Dictionary<int, Tuple<List<int>, List<int>>>)keyValue;

                // Update FN points in progress form.
                progressForm.fnPoints = new Dictionary<int, Tuple<List<int>, List<int>>>();
                progressForm.fnPoints = this.fnPoints;

                return;
            }
            else if (keyName == "TP_TN_FP_FN_Progress")
            {
                this.accuracyPointsTrain = (Dictionary<int, Tuple<List<int>, List<int>, List<int>, List<int>>>)keyValue;

                // Update TP_TN_FP_FN points in progress form.
                progressForm.accuracyPointsTrain = new Dictionary<int, Tuple<List<int>, List<int>, List<int>, List<int>>>();
                progressForm.accuracyPointsTrain = this.accuracyPointsTrain;

                return;
            }
            else if (keyName == "DepthsProgress")
            {
                this.depthPoints = (Dictionary<int, List<int>>)keyValue;

                // Update depths points in progress form.
                progressForm.depthPoints = new Dictionary<int, List<int>>();
                progressForm.depthPoints = this.depthPoints;

                return;
            }
            else if (keyName == "ConsoleOutput")
            {
                this.consoleOutput = keyValue.ToString();
                return;
            }
            else if (keyName == "ChoosedRun")
            {
                this.choosenRun = (int)keyValue;
                return;
            }
        }
        #endregion

        #region Basic load into attributes.
        /// <summary>
        /// Loads data into one attribute.(Some of.)
        /// </summary>
        /// 
        /// <param name="keyName">Name of the attribute.</param>
        /// <param name="keyValue">(ToBe)Value of the attribute.</param>
        public void LoadBasic(string keyName, object keyValue)
        {
            if (keyName == "Solution")
            {
                this.model = (Chromosome)keyValue;
                return;
            }
            else if (keyName == "SAVEVersion")
            {
                this.version = (int)keyValue;
                return;
            }
            else if (keyName == "DatabaseName")
            {
                this.databaseName = (string)keyValue;
                return;
            }
            else if (keyName == "FeatureNames")
            {
                this.featuresNames = (List<string>)keyValue;
                return;
            }
        }
        #endregion

        #endregion
    }
    #endregion
}
