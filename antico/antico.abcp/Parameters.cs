﻿//////////////////////////////////////////////////////////////////////////////////////////
// antico --- artificial bee colony programming based malware detection                 //
// Copyright 2020 Matea Stanišić                                                        //
//                                                                                      //
//                                                                                      //
// Matea Stanišić                                                                       //
// mateastanisic@outlook.com                                                            //
// Zagreb, Hrvatska                                                                     //
//////////////////////////////////////////////////////////////////////////////////////////
using System;

namespace antico.abcp
{
    #region Parameters
    /// <summary>
    /// 
    /// Class for parameters used in artificial bee colony programming algorithm.
    /// 
    /// Every Parameters class is represented with
    ///     population size
    ///     maximal number of iterations
    ///     maximal number of not improving iterations
    ///     number of runs
    ///     limit (number of iterations when certain solution is not changed before in scout bee phase is generated new solution)
    ///     alpha (used while calculating probabilities of solutions before onlooker bee phase)
    ///     initial max. depth
    ///     max. depth
    ///     method for generating symbolic trees (ramped, full, grow)
    ///     probability (to choose non-terminal as crossover point of tree in the crossover)
    /// 
    /// 
    /// </summary>
    [Serializable]
    public class Parameters
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

        // Property for the _maxNumberOfIterations variable.
        public int maxNumberOfIterations
        {
            get { return _maxNumberOfIterations; }
        }

        // Variable that represents number of loop iterations, when better 
        // solution is not found, after we stop searching for the best solution.
        // (READONLY - Setting only through constructor)
        private int _maxNumberOfNotImprovingIterations;

        // Property for the _maxNumberOfNotImprovingIterations variable.
        public int maxNumberOfNotImprovingIterations
        {
            get { return _maxNumberOfNotImprovingIterations; }
        }
        #endregion

        #region number of runs
        // Variable that represents number of different runs that will be created.
        // (READONLY - Setting only through constructor)
        private int _numberOfRuns;

        // Property for the _numberOfRuns variable.
        public int numberOfRuns
        {
            get { return _numberOfRuns; }
        }
        #endregion

        #region limit
        // Variable that represents number of iterations when certain solution is not changed before 
        // in scout bee phase is generated new solution.
        // (READONLY - Setting only through constructor)
        private int _limit;

        // Property for the _limit variable.
        public int limit
        {
            get { return _limit; }
        }
        #endregion

        #region alpha
        // Variable that represents parameter (between 0 and 1) alpha in ABC algorithm.
        // This parameter is used while calculating probabilities of solutions before onlooker bee phase.
        // (READONLY - Setting only through constructor)
        private double _alpha;

        // Property for the _alpha variable.
        public double alpha
        {
            get { return _alpha; }
        }
        #endregion

        #region depths of symbolic trees
        // Variable for maximal initial depth of the symbolic tree.
        // (READONLY - Setting only through constructor)
        private int _initialMaxDepth;

        // Property for the _initialMaxDepth variable.
        public int initialMaxDepth
        {
            get { return _initialMaxDepth; }
        }

        // Variable for maximal depth at any point of algorithm of the symbolic tree.
        // (READONLY - Setting only through constructor)
        private int _maxDepth;

        // Property for the _maxDepth variable.
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

        #region probability
        // Variable that represents probability to choose non-terminal as crossover point of tree in the crossover.
        // (READONLY - Setting only through constructor)
        private double _probability;

        // Property for the _probability variable.
        public double probability
        {
            get { return _probability; }
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

            this._populationSize = 30; // 500
            this._maxNumberOfIterations = 30; // 100
            this._numberOfRuns = 2;
            this._maxNumberOfNotImprovingIterations = 15; //50
            this._initialMaxDepth = 6; //6
            this._maxDepth = 20; //15
            this._limit = 10; //10
            this._generatingTreesMethod = "ramped";
            this._alpha = 0.9;
            this._probability = 0.9; //???

        }

        /// <summary>
        /// Constructor for parameters.
        /// </summary>
        /// 
        /// <param name="ps">Size of the population parameter.</param>
        /// <param name="maxnoi">Maximal number of iterations parameter.</param>
        /// <param name="maxnonii">Maximal number of not improving iterations parameter.</param>
        /// <param name="imd">Initial maximal depth parameter.</param>
        /// <param name="md">Maximal depth parameter.</param>
        /// <param name="lim">Limit parameter</param>
        /// <param name="a">Alpha parameter.</param>
        /// <param name="method">Mehod for generating symbolic trees</param>
        /// <param name="prob"> Probability of choosing non-terminal in crossover. </param>
        public Parameters(int ps, int maxnoi, int maxnonii, int nooruns, int imd, int md, string method, int lim, double a, double prob)
        {
            // Associate values sent by parameters to the class variables.
            this._populationSize = ps;
            this._maxNumberOfIterations = maxnoi;
            this._maxNumberOfNotImprovingIterations = maxnonii;
            this._numberOfRuns = nooruns;
            this._initialMaxDepth = imd;
            this._maxDepth = md;
            this._generatingTreesMethod = method;
            this._limit = lim;
            this._alpha = a;
            this._probability = prob;
        }

        /// <summary>
        /// Constructor for parameters.
        /// </summary>
        /// 
        /// <param name="p">Different Parameters class variable.</param>
        public Parameters(Parameters p)
        {
            // Associate values of class Parameters p to this instance of class variables.
            this._populationSize = p.populationSize;
            this._maxNumberOfIterations = p.maxNumberOfIterations;
            this._maxNumberOfNotImprovingIterations = p.maxNumberOfNotImprovingIterations;
            this._numberOfRuns = p.numberOfRuns;
            this._initialMaxDepth = p.initialMaxDepth;
            this._maxDepth = p.maxDepth;
            this._generatingTreesMethod = p.generatingTreesMethod;
            this._limit = p.limit;
            this._alpha = p.alpha;
            this._probability = p.probability;
        }
        #endregion

        #endregion
    }
    #endregion
}
