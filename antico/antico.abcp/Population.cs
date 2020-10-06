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
using System.ComponentModel;
using System.Data;
using System.Dynamic;
using System.Text;

namespace antico.abcp
{
    #region Population class
    /// <summary>
    /// Class that represents population of chromosomes. 
    /// In other words, set of Symbolic Trees that represent one model for classification problem.
    /// 
    /// This class contains of population size variable and array of chromosomes.
    /// 
    /// </summary>
    public class Population
    {
        #region ATTRIBUTES 

        #region population size
        // Variable that represents size of population.
        private int _populationSize;

        // Property for the population_size variable.
        public int populationSize
        {
            get { return _populationSize; }
            set { _populationSize = value; }
        }
        #endregion

        #region probabilities
        // Variable that represents size of population.
        private double[] _probabilities;

        // Property for the _probabilities variable.
        public double[] probabilities
        {
            get { return _probabilities; }
            set 
            {
                // Allocate memory.
                _probabilities = new double[value.Length];

                // Deep copy.
                for( var i = 0; i < value.Length; i++)
                {
                    _probabilities[i] = value[i];
                }
            }
        }
        #endregion

        #region population
        // Variable that represents population of symbolic trees.
        private Chromosome[] _chromosomes;

        // Property for the population variable.
        public Chromosome[] chromosomes
        {
            get { return _chromosomes; }
            set 
            {
                // Allocate memory for the new population of models.
                _chromosomes = new Chromosome[value.Length];

                // Deep copy every model.
                for ( var i = 0; i < value.Length; i++)
                {
                    _chromosomes[i] = value[i].Clone();
                }
            }
        }
        #endregion

        #endregion

        #region OPERATIONS

        #region Overloading operators
        /// <summary>
        /// Overloading Equals.
        /// </summary>
        /// <param name="obj">Another population.</param>
        /// <returns> True if current population and object are same, otherwise false. </returns>
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        /// <summary>
        /// Overloading GetHashCode.
        /// </summary>
        /// <returns> Generated hash code.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Overloading ToString.
        /// </summary>
        /// <returns> Generated string representing population. </returns>
        public override string ToString()
        {
            return base.ToString();
        }

        /// <summary>
        /// Indexer for Population.
        /// </summary>
        /// <param name="index"> Variable that represents index of desired symbolic tree in population. </param>
        /// <returns>Desired symbolic tree.</returns>
        public Chromosome this[int index]
        {
            get
            {
                if (index < 0 && index >= this._chromosomes.Length)
                {
                    throw new IndexOutOfRangeException("Index out of range");
                }

                return this._chromosomes[index];
            }
            set
            {
                if (index < 0 && index >= this._chromosomes.Length)
                {
                    throw new IndexOutOfRangeException("Index out of range");
                }

                this._chromosomes[index] = value.Clone();
            }
        }

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor with given values for variables.
        /// </summary>
        /// <param name="popSize">Size of population to be generated.</param>
        /// <param name="initialMaxDepth">Initial maximal depth of symbolic tree.</param>
        /// <param name="nonTerminals">String representations of non temrinals <-> mathematical operations.</param>
        /// <param name="terminalNames">String representations of terminal names.</param>
        /// <param name="terminals">Actual values for each terminal for each training example.</param>
        /// <param name="generatingTreesMethod">Method for generating symbolic tree.</param>
        public Population(int popSize, int initialMaxDepth, string[] nonTerminals, Dictionary<string,int> mathOperationsArity, string[] terminalNames, DataTable terminals, string generatingTreesMethod )
        {
            // Set population size.
            this._populationSize = popSize;

            // Allocate memory.
            this._chromosomes = new Chromosome[popSize];

            // Generate population of chromosomes.
            for( var i = 0; i < popSize; i++)
            {
                // Check depending on symbolic tree generating method.

                // Ramped half and half method has half of population generated with full method, and other half with grow method.
                if( generatingTreesMethod == "ramped" )
                {
                    if( i < popSize / 2)
                    {
                        _chromosomes[i] = new Chromosome();
                        _chromosomes[i].Generate("full", initialMaxDepth, terminalNames, terminals, nonTerminals, mathOperationsArity);
                    }
                    else
                    {
                        _chromosomes[i] = new Chromosome();
                        _chromosomes[i].Generate("grow", initialMaxDepth, terminalNames, terminals, nonTerminals, mathOperationsArity);
                    }
                    
                }
                else
                {
                    // Full or grow method.
                    _chromosomes[i] = new Chromosome();
                    _chromosomes[i].Generate(generatingTreesMethod, initialMaxDepth, terminalNames, terminals, nonTerminals, mathOperationsArity);
                }

                
            }

            // Probabilities are calculated from algorithm.
            this._probabilities = null;

        }
        #endregion

        #region Crossover
        /// <summary>
        /// Crossover of the two chromosomes.
        /// </summary>
        /// <param name="parent1"> First chromosome (parent) that is part of the crossover. </param>
        /// <param name="parent2"> Second chromosome (parent) that is part of the crossover. </param>
        /// <returns> Child chromosome. </returns>
        public Chromosome Crossover( Chromosome parent1, Chromosome parent2, int maxDepth, DataTable data, double probability )
        {
            // Child chromosome that will be created using crossover of parent1 and parent2.
            Chromosome child = new Chromosome();

            // Using random for generating random position of crossover point.
            var rand = new Random();

            // With probability sent by parameter, choose if crossoverPoint will be terminal or non terminal.
            bool chooseNonTerminal = rand.Next(100) < (probability * 100) ? true : false;

            // Randomly select breaking points of parent chromosomes.
            // Those integers will represent index of the node in preorder touring of tree.
            // Do not consider crossover point at root node in first parent since in that way we would be cloning second parent into a child.
            int crossoverPoint1 = rand.Next(1, parent1.numberOfNodesInTree);
            int crossoverPoint2 = rand.Next(parent2.numberOfNodesInTree);

            // Variables to be used in loop.
            int depth1, depth2;
            SymbolicTreeNode nodeAtPoint1, nodeAtPoint2;

            // Until we found crossover points that satisfy condition on depth of child tree.
            while ( true )
            {
                // Find nodes from parents with randomly choosen index (index of preorder touring of the tree).
                nodeAtPoint1 = parent1.symbolicTree.FindNodeWithIndex(crossoverPoint1);
                nodeAtPoint2 = parent2.symbolicTree.FindNodeWithIndex(crossoverPoint2);

                // Check if choosen crossover points are nonterminals/terminals, depending on randomly 
                // defined variable chooseNonTerminal.
                if( (chooseNonTerminal && ( nodeAtPoint1.type != "non-terminal" || nodeAtPoint2.type != "non-terminal" ) )
                    || (!chooseNonTerminal && (nodeAtPoint1.type != "terminal" || nodeAtPoint2.type != "terminal")))
                {
                    // generate crossover points again since conditions are not met.
                    crossoverPoint1 = rand.Next(1, parent1.numberOfNodesInTree);
                    crossoverPoint2 = rand.Next(parent2.numberOfNodesInTree);
                }


                // Get depth of the selected node from the first parent.
                depth1 = nodeAtPoint1.depth;

                // Get depth of the subtree whose root node is selected node from second parent.
                depth2 = nodeAtPoint2.DepthOfSymbolicTree();

                /// <summary>
                /// Child node will be created from tree of first parent without subtree whose root node is 
                /// the selected node from first parent 
                /// AND 
                /// from subtree whose root node is the selected node from second parent.
                /// 
                /// So, we need to check that child tree created in described way will not exceed maximal depth
                /// of the tree defined by parameters.
                /// </summary>
                if ((depth1 + depth2) < maxDepth)
                    break;

                // Condition is not met. Randomly generate new crossover points.

                // Randomly select breaking points of parent chromosomes.
                // Those integers will represent index of the node in preorder touring of tree.
                // Do not consider crossover point at root node in first parent since in that way we would be cloning second parent into a child.
                crossoverPoint1 = rand.Next(1, parent1.numberOfNodesInTree);
                crossoverPoint2 = rand.Next(parent2.numberOfNodesInTree);
            }

            // We found crossover points. 
            // Create child.

            // Number of possible terminals is same as on parents.
            child.numberOfPossibleTerminals = parent1.numberOfPossibleTerminals;

            //Change depth of the created child.
            if( parent1.depth > (depth1 + depth2))
            {
                // Adding subtree from second parent will not enlarge depth of a child.
                child.depth = parent1.depth;
            }
            else
            {
                // Adding subtree from second parent enlarges depth of a child.
                child.depth = depth1 + depth2;
            }

            // Create child from parents.
            child.symbolicTree = parent1.symbolicTree.CreateUsing(crossoverPoint1, nodeAtPoint2);

            // Calculate fitness of the newly created child.
            child.CalculateFitness(data);

            // Calculating number of nodes in newly created SYmbolicTree.
            child.numberOfNodesInTree = child.symbolicTree.NumberOfNodes();

            // DONE.
            return child;

        }
        #endregion

        #region Find best chromosome 
        /// <summary>
        /// Method for finding the best chromosome in the whole population.
        /// Best chromosome is found by iterating through all chromosomes and 
        /// returning the one with best ( maximal ) fitness, since better fitness
        /// means higher accuracy on train set <-> fitness value closer to 1.
        /// </summary>
        /// <returns> Best chromosome in population and its index in population (faster algorithm). </returns>
        public Tuple<Chromosome, int> BestSolution()
        {
            // Variable for best chromosome.
            Chromosome best = new Chromosome();
            int bestIndex = 0;

            // Iterating through all chromosomes.
            for( var i = 0; i < this.chromosomes.Length; i++)
            {
                // If this chromosome has better fitness change best chromosome.
                if ( this.chromosomes[i].fitness > best.fitness)
                {
                    // Deep copy.
                    best = this.chromosomes[i].Clone();
                    bestIndex = i;
                }
            }

            return Tuple.Create(best, bestIndex);
        }

        #endregion

        #region Calculate probabilities
        /// <summary>
        /// Calculate probabilities of choosing some solution in onlooker bee phase.
        /// 
        /// This value is calculated for every solution in population with formula:
        /// P_i = (alpha * fit(Solution_i) ) / fit(Solution_BEST) + (1 - alpha)
        /// Where function fit is defined with:
        /// fit(Solution_i) = (1 + fitness(Solution_i))/2 [cannot be zero!]
        /// 
        /// </summary>
        /// <param name="bestSolutionIndex"> Index of the best solution in the population. </param>
        public void CalculateProbabilities( int bestSolutionIndex, double alpha )
        {
            // Array for function fit ( fit(Solution_i) = (1 + fitness(Solution_i))/2 [cannot be zero!] ).
            double[] fit = new double[this._populationSize];

            // First, calculate fit for the best solution.
            fit[bestSolutionIndex] = (1 + this._chromosomes[bestSolutionIndex].fitness) / 2;

            // Calculate probabilities for all solutions.
            for ( var i = 0; i < this._populationSize; i++)
            {
                fit[i] = (1 + this._chromosomes[i].fitness) / 2;
                this._probabilities[i] = (1 - alpha) + ( (alpha * fit[i]) / fit[bestSolutionIndex] );
            }
        }
        #endregion

        #endregion
    }
    #endregion
}
