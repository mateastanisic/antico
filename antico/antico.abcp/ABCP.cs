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
    /// Artificial bee colony programming (ABCP) is a novel evolutionary computation based auto- matic programming method, 
    /// which uses the basic structure of artificial bee colony (ABC) algorithm.
    /// Artificial bee colony algorithm simulating the intelligent foraging behavior of honey bee swarms is one of 
    /// the most popular swarm based optimization algorithms. It has been introduced in 2005 and applied in several fields 
    /// to solve different problems up to date.
    /// 
    /// In this application we are using ABCP for symbolic regression - meaning, we are using it to make a model for classification
    /// of software into malicious and not-malicious. 
    /// This class is the main class of the application containing all the data, parameters and population needed for the
    /// algorithm which is also implemented as a method of this class.
    /// 
    /// </summary>
    public class ABCP
    {
        #region ATTRIBUTES 

        #region random
        // Variable for generating random numbers.
        private static Random rand;
        #endregion

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
        Population _population;

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

        #region Static constructor
        /// <summary>
        /// New static random.
        /// </summary>
        static ABCP()
        {
            rand = new Random();
        }
        #endregion

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
            _population = new Population(_parameters.populationSize, _parameters.initialMaxDepth, _data.mathOperations, _data.mathOperationsArity, _data.featureNames, _data.trainFeatures, _parameters.generatingTreesMethod);
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
        /// <param name="limit">Number of iterations when certain solution is not changed before in scout bee phase is generated new solution.</param>
        public ABCP( int population_size, int max_number_of_iterations, int max_number_of_not_improving_iterations, int initial_max_depth, int max_depth, string generating_trees_method, string[] math_operations, string feature_extraction_method, string spliting_data_type, int number_of_top_features, int limit, double alpha, double probability )
        {
            // Setting the values with other constructors.
            _parameters = new Parameters(population_size, max_number_of_iterations, max_number_of_not_improving_iterations, initial_max_depth, max_depth, generating_trees_method, limit, alpha, probability);
            _data = new Data(math_operations, feature_extraction_method, spliting_data_type, number_of_top_features);
            _population = new Population(population_size, initial_max_depth, math_operations, _data.mathOperationsArity, _data.featureNames, _data.trainFeatures, generating_trees_method);
        }

        #endregion

        #region artificial bee colony programming 
        /// <summary>
        /// Artificial bee colony programming algorithm.
        /// This method is used to find the best model for some data using heuristic abc programming.
        /// It is considered all variables for class ABCP are set.
        /// </summary>
        /// <returns> Best model. </returns>
        public Chromosome ABCProgrammingFindBestModel()
        {
            // Helper arrays for keeping track of case when Solution (i) is not improved.
            int[] Limits = new int[this.parameters.populationSize];
            int[] TestLimits = new int[this.parameters.populationSize];

            // 1. Produce the initial solutions (x_i) by using ramped half-and-half method, randomly.  
            // -- DONE IN CONSTRUCTOR --

            // 2: Evaluate the solutions. 
            // -- DONE IN CONSTRUCTOR --

            // 3: Memorize the best. 
            Tuple<Chromosome, int> BestSolutionAndIndex = this.population.BestSolution();
            Chromosome BestSolution = BestSolutionAndIndex.Item1;
            double BestFitness = BestSolution.fitness;
            int BestIndex = BestSolutionAndIndex.Item2;

            // 4: Set the cycle counter(cycle = 0)
            int Iteration = 0;
            int IterationNotImproving = 0;

            while (Iteration <= this.parameters.maxNumberOfIterations && IterationNotImproving <= this.parameters.maxNumberOfNotImprovingIterations)
            {
                #region ----- EMPLOYED BEES PHASE -----
                // For all employed bees do ...
                // (number of employed bees are same as population size)
                for ( var e = 0; e < this.parameters.populationSize; e++)
                {
                    // Calculate NewSolution using information sharing mechanism.
                    Chromosome NewSolutionEmployed;

                    // Helper variables.
                    double OldFitnessEmployed, NewFitnessEmployed;

                    // Save the cost function value of the current solution.
                    OldFitnessEmployed = this.population[e].fitness;

                    #region information sharing mechanism
                    // Randomly choose another solution.
                    int r;

                    // Check that randomly choosen solution is noth current solution.
                    while (true)
                    {
                        r = rand.Next(this.parameters.populationSize);
                        if (r != e)
                            break;
                    }

                    NewSolutionEmployed = (Chromosome)this.population.Crossover( (Chromosome)this.population[e].Clone(), (Chromosome)this.population[r].Clone(), this.parameters.maxDepth, this.data.trainFeatures, this.parameters.probability);
                    #endregion

                    // Calculate the cost function value of new solution.
                    NewFitnessEmployed = NewSolutionEmployed.fitness;

                    #region greedy selection between OldSolution and NewSolution
                    // TODO
                    // Considering the cost values, apply the greedy selection between OldSolution and NewSolution.                     

                    // Update solution if better.
                    if (NewFitnessEmployed > OldFitnessEmployed)
                    {
                        this.population[e] = (Chromosome)NewSolutionEmployed.Clone();

                        // Solution has become better. Put Limit of that solution to 0.
                        Limits[e] = 0;

                        // Solution has become better. Put TestLimit of that solution to 1.
                        TestLimits[e] = 1;
                    }
                    #endregion
                }
                #endregion

                // Find new best solution.
                Tuple<Chromosome, int> BestSolutionAfterE = this.population.BestSolution();
                // Calculate the probability values ( P_i ) for the solutions.
                this.population.CalculateProbabilities(BestSolutionAfterE.Item2, this.parameters.alpha);

                #region ----- ONLOOK BEES PHASE -----
                // Number of onlookers.
                int o = 0;
                // Food source index <-> Chromosome index.
                int f = 0;

                // For all onlooker bees do ...
                // (number of onlookers bees are same as population size)
                while (o < this.parameters.populationSize)
                {
                    // Select a solution OldSolution depending on P_i. 
                    // Better solutions have higher probabilities so we will select this solution if it has higher probability.
                    bool SelectThisSolution = rand.Next(100) < (this.population.probabilities[f] * 100) ? true : false;

                    #region food source not selected by onlooker bee
                    // It is choosed not to select this solution.
                    if (!SelectThisSolution)
                    {
                        // Update source.
                        f++;
                        // Check if came to the end.
                        if (f == this.population.populationSize)
                        {
                            f = 0;
                        }
                        continue;
                    }
                    #endregion

                    #region food source selected by onlooker bee
                    // Use onlooker bee for this source.
                    o++;

                    // Calculate NewSolution using information sharing mechanism.
                    Chromosome NewSolutionOnlook = new Chromosome();

                    double OldFitnessOnlook, NewFitnessOnlook;

                    // Save the cost function value of the current solution.
                    OldFitnessOnlook = this.population[f].fitness;

                    #region information sharing mechanism
                    // Randomly choose another solution.
                    int r;

                    // Check that randomly choosen solution is noth current solution.
                    while (true)
                    {
                        r = rand.Next(this.parameters.populationSize);
                        if (r != f)
                            break;
                    }

                    NewSolutionOnlook = (Chromosome)this.population.Crossover((Chromosome)this.population[f].Clone(), (Chromosome)this.population[r].Clone(), this.parameters.maxDepth, this.data.trainFeatures, this.parameters.probability);
                    #endregion

                    // Calculate the cost function value of new solution.
                    NewFitnessOnlook = NewSolutionOnlook.fitness;

                    #region greedy selection between OldSolution and NewSolution
                    // TODO
                    // Considering the cost values, apply the greedy selection between OldSolution and NewSolution. 

                    // Update solution if better.
                    if (NewFitnessOnlook > OldFitnessOnlook)
                    {
                        this.population[f] = (Chromosome)NewSolutionOnlook.Clone();

                        // Solution has become better. Put Limit of that solution to 0.
                        Limits[f] = 0;

                        // Solution has become better. Put TestLimit of that solution to 1.
                        TestLimits[f] = 1;
                    }
                    #endregion

                    // Update source.
                    f++;
                    // Check if came to the end.
                    if (f == this.population.populationSize)
                    {
                        f = 0;
                    }
                    #endregion
                }

                #endregion

                #region ----- SCOUT BEES PHASE -----
                for (var s = 0; s < this.parameters.populationSize; s++ )
                {
                    // Check if if EBP or OBP improved this solution.
                    if ( TestLimits[s] == 0)
                    {
                        // Neither in EBP nor in OBS this solution is not improved.
                        // Keep track of that in Limits variable.
                        Limits[s]++;
                    }
                        
                    // Check if 'limit' number of iterations in a row this solution is not improved.
                    if ( Limits[s] >= this.parameters.limit )
                    {
                        // If that is so, generate new solution using "grow" method.
                        Chromosome NewSolutionScout = new Chromosome();
                        NewSolutionScout.Generate("grow", this.parameters.initialMaxDepth, this.data.featureNames, this.data.trainFeatures, this.data.mathOperations, this.data.mathOperationsArity);
                        this.population[s] = (Chromosome)NewSolutionScout.Clone();
                    }
                }
                #endregion

                // Save new best solution.
                var NewBestSolutionAndIndex = this.population.BestSolution();

                // Check if best solution is updated and change variable IterationNotImproving accordingly.
                if (NewBestSolutionAndIndex.Item1.fitness != BestFitness )
                {
                    // Counter for number of continually iterations that did not improve best solution brought back to zero.
                    IterationNotImproving = 0;

                    // Update BestSolution, BestFitness and bestIndex.
                    BestSolution = (Chromosome)NewBestSolutionAndIndex.Item1.Clone();
                    BestFitness = BestSolution.fitness;
                    BestIndex = NewBestSolutionAndIndex.Item2;
                }
                else
                {
                    // Increase number of done not improved iterations.
                    IterationNotImproving++;
                }

                // Increase number of done iterations.
                Iteration++;

                // Maybe this?
                // if( BestFitness == 1 ) break;
            }

            return BestSolution;
        }
        #endregion

        #endregion
    }
    #endregion
}
