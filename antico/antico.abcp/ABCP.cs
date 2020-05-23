//////////////////////////////////////////////////////////////////////////////////////////
// antico --- artificial bee colony programming based malware detection                 //
// Copyright 2020 Matea Stanišić                                                        //
//                                                                                      //
//                                                                                      //
// Matea Stanišić                                                                       //
// mateastanisic@outlook.com                                                            //
// Zagreb, Hrvatska                                                                     //
//////////////////////////////////////////////////////////////////////////////////////////
using antico.data;
using System;
using System.Collections.Generic;
using System.Text;


namespace antico.abcp
{
    #region ABCP class
    /// <summary>
    /// 
    /// </summary>
    class ABCP
    {
        #region ATTRIBUTES 

        #region parametars
        // (READONLY - Setting only through constructor) 
        // Class of parameters needed for the ABC programming.
        Parameters _parameters;

        // Property for the _parameters variable.
        public Parameters parameters
        {
            get { return _parameters; }
        }
        #endregion

        #region models <-> population 
        // (READONLY - Setting only through constructor) 
        // Variable that represents class with models - population.
        private Population _population;

        // Property for the _population variable.
        public Population population
        {
            get { return _population; }
        }
        #endregion

        #region data
        // (READONLY - Setting only through constructor) 
        // Variable that represents features and mathematical operators.
        private Data _data;

        // Property for the _data variable.
        public Data data
        {
            get { return _data; }
        }

        #endregion

        #endregion

        #region OPERATIONS

        #region Constructor

        /// <summary>
        /// Basic constructor. 
        /// Generating population of chromosomes (symbolic trees) with basic values of (data and parameters) variables.
        /// </summary>
        public ABCP()
        {
            // Setting the initial values.
            _parameters = new Parameters();
            _data = new Data();
            _population = new Population(_parameters.populationSize, _parameters.initialMaxDepth, _data.mathOperations, _data.featureNames, _parameters.generatingTreesMethod);
        }

        /// <summary>
        /// Constructor for class with all setting of all variables.
        /// </summary>
        /// <param name="population_size">Size of population.</param>
        /// <param name="max_number_of_iterations">Maximal number of iterations.</param>
        /// <param name="max_number_of_not_improving_iterations">Maximal number of not improving iterations.</param>
        /// <param name="initial_max_depth">Initial maximal depth of symbolic tree.</param>
        /// <param name="max_depth">Maximal depth of symbolic tree overall.</param>
        /// <param name="generating_trees_method">Method to generate symbolic tree.</param>
        /// <param name="math_operations">Math operations <-> non terminals. </param>
        /// <param name="feature_extraction_method">Method used to extract features.</param>
        /// <param name="spliting_data_type">Type of splitting data into train and test set. (Balanced/Inbalanced)</param>
        /// <param name="number_of_top_features">Number of top features selected to be in model.</param>
        public ABCP( int population_size, int max_number_of_iterations, int max_number_of_not_improving_iterations, int initial_max_depth, int max_depth, string generating_trees_method, string[] math_operations, string feature_extraction_method, string spliting_data_type, int number_of_top_features)
        {
            // Setting the values with other constructors.
            _parameters = new Parameters(population_size, max_number_of_iterations, max_number_of_not_improving_iterations, initial_max_depth, max_depth, generating_trees_method);
            _data = new Data(math_operations, feature_extraction_method, spliting_data_type, number_of_top_features);
            _population = new Population(population_size, initial_max_depth, math_operations, _data.featureNames, _data.trainFeatures, generating_trees_method);
        }

        #endregion

        #endregion
    }
    #endregion
}
