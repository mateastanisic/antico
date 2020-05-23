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
using System.Text;

namespace antico.abcp
{
    #region Parameters class
    /// <summary>
    /// Class for parameters used in artificial bee colony programming algorithm.
    /// </summary>
    class Parameters
    {
        #region ATTRIBUTES 

        #region population size <-> number of models
        // Variable for number of chromosomes in population. 
        // Meaning - number of models (SymbolicTrees) in the population.
        // (READONLY - Setting only through constructor)
        private int _populationSize;

        // Property for the population_size variable.
        public int populationSize   
        {
            get { return _populationSize; }
        }
        #endregion

        #region number of iterations
        // Variable that represents number of loop iterations after we stop 
        // searching for the best solution.
        // (READONLY - Setting only through constructor)
        private int _maxNumberOfIterations;

        // Variable that represents number of loop iterations, when better 
        // solution is not found, after we stop searching for the best solution.
        // (READONLY - Setting only through constructor)
        private int _maxNumberOfNotImprovingIterations;

        // Property for the population_size variable.
        public int maxNumberOfIterations
        {
            get { return _maxNumberOfIterations; }
        }

        // Property for the population_size variable.
        public int maxNumberOfNotImprovingIterations
        {
            get { return _maxNumberOfNotImprovingIterations; }
        }
        #endregion

        #region depths of symbolic trees
        // Variable for maximal initial depth of the symbolic tree.
        // (READONLY - Setting only through constructor)
        private int _initialMaxDepth;

        // Variable for maximal depth at any point of algorithm of the symbolic tree.
        // (READONLY - Setting only through constructor)
        private int _maxDepth;

        // Property for the initial_max_depth variable.
        public int initialMaxDepth
        {
            get { return _initialMaxDepth; }
        }

        // Property for the max_depth variable.
        public int maxDepth
        {
            get { return _maxDepth; }
        }
        #endregion

        #region method for generating symbolic trees
        // Variable that represents method for generating symbolic trees - full/grow/combination.
        // (READONLY - Setting only through constructor)
        private string _generatingTreesMethod;

        // Property for the population_size variable.
        public string generatingTreesMethod
        {
            get { return _generatingTreesMethod; }
        }
        #endregion

        #endregion

        #region OPERATIONS

        #region Constructors
        /// <summary>
        /// Initial construcor for parameters.
        /// </summary>
        public Parameters()
        {
            // Associate initial values. 

            this._populationSize = 500;
            this._maxNumberOfIterations = 100;
            this._maxNumberOfNotImprovingIterations = 50;
            this._initialMaxDepth = 6;
            this._maxDepth = 15;
            this._generatingTreesMethod = "full";

        }

        /// <summary>
        /// Constructor for parameters.
        /// </summary>
        /// <param name="ps">populationSize</param>
        /// <param name="maxnoi">maxNumberOfIterations</param>
        /// <param name="maxnonii">maxNumberOfNotImprovingIterations</param>
        /// <param name="imd">initialMaxDepth</param>
        /// <param name="md">maxDepth</param>
        /// <param name="nontermi">nonTerminals</param>
        /// <param name="termi">Terminals</param>
        /// <param name="method">Mehod for generating symbolic trees</param>
        public Parameters(int ps, int maxnoi, int maxnonii, int imd, int md, string method)
        {
            // Associate values sent by parameters to the class variables.
            this._populationSize = ps;
            this._maxNumberOfIterations = maxnoi;
            this._maxNumberOfNotImprovingIterations = maxnonii;
            this._initialMaxDepth = imd;
            this._maxDepth = md;
            this._generatingTreesMethod = method;
        }

        /// <summary>
        /// Constructor for parameters.
        /// </summary>
        /// <param name="p">Different Parameters class variable.</param>
        public Parameters(Parameters p)
        {
            // Associate values of class Parameters p to this instance of class variables.
            this._populationSize = p.populationSize;
            this._maxNumberOfIterations = p.maxNumberOfIterations;
            this._maxNumberOfNotImprovingIterations = p.maxNumberOfNotImprovingIterations;
            this._initialMaxDepth = p.initialMaxDepth;
            this._maxDepth = p.maxDepth;
            this._generatingTreesMethod = p.generatingTreesMethod;
        }
        #endregion

        #region Copy
        /// <summary>
        /// Helper method for cloning Parameters p values to this values.
        /// </summary>
        /// <param name="p">Another class variable Parameters</param>
        public void Clone( Parameters p )
        {
            // Associate values of class Parameters p to this instance of class variables.
            this._populationSize = p.populationSize;
            this._maxNumberOfIterations = p.maxNumberOfIterations;
            this._maxNumberOfNotImprovingIterations = p.maxNumberOfNotImprovingIterations;
            this._initialMaxDepth = p.initialMaxDepth;
            this._maxDepth = p.maxDepth;
            this._generatingTreesMethod = p.generatingTreesMethod;

        }
        #endregion

        #endregion
    }
    #endregion
}
