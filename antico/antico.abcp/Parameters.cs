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
    class Parameters
    {
        #region ATTRIBUTES 

        #region population size <-> number of models
        // Variable for number of chromosomes in population. 
        // Meaning - number of models (SymbolicTrees) in the population.
        private int _populationSize;

        // Property for the population_size variable.
        public int populationSize   
        {
            get { return _populationSize; }
            set { _populationSize = value; }
        }
        #endregion

        #region number of iterations
        // Variable that represents number of loop iterations after we stop 
        // searching for the best solution.
        private int _maxNumberOfIterations;

        // Variable that represents number of loop iterations, when better 
        // solution is not found, after we stop searching for the best solution.
        private int _maxNumberOfNotImprovingIterations;

        // Property for the population_size variable.
        public int maxNumberOfIterations
        {
            get { return _maxNumberOfIterations; }
            set { _maxNumberOfIterations = value; }
        }

        // Property for the population_size variable.
        public int maxNumberOfNotImprovingIterations
        {
            get { return _maxNumberOfNotImprovingIterations; }
            set { _maxNumberOfNotImprovingIterations = value; }
        }
        #endregion

        #region depths of symbolic trees
        // Variable for maximal initial depth of the symbolic tree.
        private int _initialMaxDepth;

        // Variable for maximal depth at any point of algorithm of the symbolic tree.
        private int _maxDepth;

        // Property for the initial_max_depth variable.
        public int initialMaxDepth
        {
            get { return _initialMaxDepth; }
            set { _initialMaxDepth = value; }
        }

        // Property for the max_depth variable.
        public int maxDepth
        {
            get { return _maxDepth; }
            set { _maxDepth = value; }
        }
        #endregion

        #region (non)terminals <-> mathematical operations and features
        // Variable that represents number of possible non-terminals. 
        // In our perspective, that means number of possible mathematical operations on inner nodes in symbolic tree.
        private int _numberOfNonTerminals;

        // Variable that represents non terminals (mathematical operations) as strings.
        private string[] _nonTerminals;

        // Variable that represents number of possible terminals. 
        // In our perspective, that means number of possible features on leaf nodes in symbolic tree.
        private int _numberOfTerminals;

        // Variable that represents terminals (features) as strings.
        private string[] _Terminals;

        // Property for the number_of_non_terminals variable.
        public int numberOfNonTerminals
        {
            get { return _numberOfNonTerminals; }
            set { _numberOfNonTerminals = value; }
        }

        // Property for the number_of_terminals variable.
        public int numberOfTerminals
        {
            get { return _numberOfTerminals; }
            set { _numberOfTerminals = value; }
        }

        // Property for the non_terminals variable.
        public string[] nonTerminals
        {
            get { return _nonTerminals; }
            set 
            {
                // Allocate resources.
                _nonTerminals = new string[value.Length];

                // Deep copy all non terminals.
                for ( var i = 0; i < value.Length; i++)
                {
                    _nonTerminals[i] = value[i];
                }
            }
        }

        // Property for the terminals variable.
        public string[] Terminals
        {
            get { return _Terminals; }
            set
            {
                // Allocate resources.
                _Terminals = new string[value.Length];

                // Deep copy all terminals.
                for (var i = 0; i < value.Length; i++)
                {
                    _Terminals[i] = value[i];
                }
            }
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

            this._nonTerminals = new string[9] { "+", "-", "*", "/", "sin", "cos", "exp", "iff", "log" };
            this._numberOfNonTerminals = 9;

            // Flag that terminals are not initialed.
            this._numberOfTerminals = -1;
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
        public Parameters(int ps, int maxnoi, int maxnonii, int imd, int md, string[] nontermi, string[] termi)
        {
            // Associate values sent by parameters to the class variables.
            this._populationSize = ps;
            this._maxNumberOfIterations = maxnoi;
            this._maxNumberOfNotImprovingIterations = maxnonii;
            this._initialMaxDepth = imd;
            this._maxDepth = md;

            this._numberOfNonTerminals = nontermi.Length;
            this._numberOfTerminals = termi.Length;

            // Deep copy with variable property set.
            this.nonTerminals = nontermi;
            this.Terminals = termi;
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

            this._numberOfNonTerminals = p.nonTerminals.Length;
            this._numberOfTerminals = p.Terminals.Length;

            // Deep copy with variable property set.
            this.nonTerminals = p.nonTerminals;
            this.Terminals = p.Terminals;
        }
        #endregion

        #region Deep copy
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

            this._numberOfNonTerminals = p.nonTerminals.Length;
            this._numberOfTerminals = p.Terminals.Length;

            // Deep copy with variable property set.
            this.nonTerminals = p.nonTerminals;
            this.Terminals = p.Terminals;
        }
        #endregion

        #endregion
    }
}
