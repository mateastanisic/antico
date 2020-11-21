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
using System.ComponentModel;
using System.Windows.Forms;
using antico.abcp;
using System.Data;
using LiveCharts.Defaults;
using System.Collections.Generic;

namespace antico
{
    #region ABCP
    /// <summary>
    /// 
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
    /// 
    /// Every ABCP class is represented with
    ///     Parameters (class - e.g. maxDepth)
    ///     Data (class - e.g. databaseName)
    ///     Population (class - e.g. chromosomes)
    ///     current train and test dataset
    ///     best, best train and best test solutions and its indices (position) in the population
    /// 
    /// </summary>
    [Serializable]
    public class ABCP
    {
        #region ATTRIBUTES 

        #region random
        // Variable for generating random numbers.
        private static Random rand;
        #endregion

        #region Parameters
        // (READONLY - Setting only through constructor) 
        // Class of parameters needed for the ABC programming.
        Parameters _parameters;

        // Property for the _parameters variable.
        public Parameters parameters
        {
            get { return _parameters; }
        }
        #endregion

        #region Data
        // (READONLY - Setting only through constructor) 
        // Variable that represents features, dataset and mathematical operators.
        private Data _data;

        // Property for the _data variable.
        public Data data
        {
            get { return _data; }
        }
        #endregion

        #region Population
        // (READONLY - Setting only through constructor) 
        // Variable that represents class with models - population.
        Population _population;

        // Property for the _population variable.
        public Population population
        {
            get { return _population; }
        }
        #endregion

        #region THE BEST (indices)
        // Variable that represents best current solution (model) index on training and testing data (together).
        private int _bestIndex;

        // Property for the _bestIndex variable.
        public int bestIndex
        {
            get { return _bestIndex; }
            set { _bestIndex = value;  }
        }

        // Variable that represents best current solution (model) index on training data.
        private int _bestTrainIndex;

        // Property for the _bestTrainIndex variable.
        public int bestTrainIndex
        {
            get { return _bestTrainIndex; }
            set { _bestTrainIndex = value; }
        }

        // Variable that represents best current solution (model) index on testing data.
        private int _bestTestIndex;

        // Property for the _bestTestIndex variable.
        public int bestTestIndex
        {
            get { return _bestTestIndex; }
            set { _bestTestIndex = value; }
        }
        #endregion

        #region Current train and test data

        #region train data
        // DataTable variable containing train features that are currently used.
        private DataTable _train;

        // Property for the _train variable.
        public DataTable train
        {
            get { return _train; }
            set
            {
                _train = new DataTable();
                _train = value.Copy();
            }
        }
        #endregion

        #region test data
        // DataTable variable containing test features that are currently used.
        private DataTable _test;

        // Property for the _test variable.
        public DataTable test
        {
            get { return _test; }
            set
            {
                _test = new DataTable();
                _test = value.Copy();
            }
        }
        #endregion

        #endregion

        #endregion

        #region OPERATIONS

        #region Constructor

        #region Static constructor
        /// <summary>
        /// New static random.
        /// </summary>
        static ABCP()
        {
            rand = new Random();
        }
        #endregion

        /// <summary>
        /// Constructor with custom (sent) parameters and data.
        /// </summary>
        /// 
        /// <param name="p">Custom parameters.</param>
        /// <param name="d">Custom data.</param>
        /// <param name="formForCreatingNewModel">Form for creating a model. Needed for printouts on form.</param>
        /// <param name="consoleTextBox">TextBox in which the printouts will be added.</param>
        /// <param name="trainToBe">Train set to be used in this model.</param>
        /// <param name="testToBe">Test set to be used in this model.</param>
        public ABCP(Parameters p, Data d, DataTable trainToBe, DataTable testToBe, CreateNewModelForm formForCreatingNewModel, TextBox consoleTextBox)
        {
            // Setting custom Parameters.
            this._parameters = p;

            // Setting custom Data.
            this._data = d;

            // Setting up train features to be used.
            this._train = new DataTable();
            this._train = trainToBe.Copy();

            // Setting up test features to be used.
            this._test = new DataTable();
            this._test = testToBe.Copy();

            // Initialization of Population.
            this._population = new Population(p.populationSize, p.initialMaxDepth, d.mathOperators, d.mathOperationsArity, d.featureNames, trainToBe, testToBe, p.generatingTreesMethod);

            // Find best solution based on train/test features.
            this.BestSolutions();

            // Printout to console.
            string time = Microsoft.VisualBasic.DateAndTime.Now.ToString("MM/dd/yyyy HH:mm");
            formForCreatingNewModel.Invoke((MethodInvoker)delegate { consoleTextBox.AppendText("[" + time + "] (ABCP constructor with parameters) DONE!\r\n"); });
        }
        #endregion

        #region (Re)Setup of best solutions
        /// <summary>
        /// Method for finding the best solutions in the whole population based on train and test fitness (together and separately).
        /// Best solution is found by iterating through all solutions and setting up best, bestTrain and bestTest ABCP variables
        /// properly at the end. Also, best indices that represent index of the best solutions in the population are also reset.
        /// 
        /// Since higher fitness means higher accuracy, best solutions are choosen based on fitness.
        /// </summary>
        private void BestSolutions()
        {
            // Variables for indices in population of best solutions (models).
            int bestSolutionIndex = 0;
            int bestTrainSolutionIndex = 0;
            int bestTestSolutionIndex = 0;

            // Iterating through all chromosomes.
            for (var s = 0; s < this._population.chromosomes.Count; s++)
            {
                // If this chromosome has better train fitness change bestTrain solution.
                if (this._population.chromosomes[s].trainFitness > this._population[bestTrainSolutionIndex].trainFitness)
                    bestTrainSolutionIndex = s;

                // If this chromosome has better test fitness change bestTest solution.
                if (this._population.chromosomes[s].testFitness > this._population[bestTestSolutionIndex].testFitness)
                    bestTestSolutionIndex = s;

                // If this chromosome has better fitness (train and test together) change best solution.
                if ((this._population.chromosomes[s].trainFitness + this._population[s].testFitness) > (this._population[bestSolutionIndex].trainFitness + this._population[bestSolutionIndex].testFitness))
                    bestSolutionIndex = s;
            }

            // Reset best indices.
            this._bestIndex = bestSolutionIndex;
            this._bestTrainIndex = bestTrainSolutionIndex;
            this._bestTestIndex = bestTestSolutionIndex;
        }
        #endregion

        #region BASIC ABCP 
        /// <summary>
        /// BASIC ARTIFICIAL BEE COLONY PROGRAMINNG ALGORITHM
        /// This method is used to find the best solution for some data using meta-heuristic abc programming.
        /// It is considered all variables for class ABCP are previously set.
        /// </summary>
        /// 
        /// <param name="formForCreatingNewModel">Form that called this method - for printouts.</param>
        /// <param name="consoleTextBox">TextBox for printouts.</param>
        /// <param name="dictKey">Dictionary key - representing current run(+1) (or fold index if using folds). </param>
        public void basicABCP(CreateNewModelForm formForCreatingNewModel, TextBox consoleTextBox, int dictKey, ProgressBar progressBar)
        {
            // Helper arrays for keeping track of case when Solution (i) is not improved.
            int[] Limits = new int[this.parameters.populationSize];
            int[] TestLimits = new int[this.parameters.populationSize];

            // 1. Produce the initial solutions (x_i) by using ramped half-and-half method, randomly.  
            // -- DONE IN CONSTRUCTOR --

            // 2: Evaluate the solutions. 
            // -- DONE IN CONSTRUCTOR --

            // 3: Memorize the best. 
            // -- DONE IN CONSTRUCTOR --

            // Add points to chart.
            AddPointsToChartAtBeginningOfABCP(formForCreatingNewModel, dictKey);

            // 4: Set the cycle counter 
            // 
            int Iteration = 0;
            int IterationNotImproving = 0;

            while (Iteration <= this._parameters.maxNumberOfIterations && IterationNotImproving <= this._parameters.maxNumberOfNotImprovingIterations)
            {
                //
                // Memorize best fitness from the start of the current iteration.
                //
                double OldBestFitness = this._population[this.bestTrainIndex].trainFitness;

                #region app related
                // Printout to console.
                PrintToConsole(formForCreatingNewModel, consoleTextBox, "ABCP STEP " + Iteration.ToString());
                #endregion

                #region ----- EMPLOYED BEES PHASE -----
                // For all employed bees do ...
                // (number of employed bees are same as population size)
                for (var e = 0; e < this._parameters.populationSize; e++)
                {
                    // Reset TestLimits[e].
                    TestLimits[e] = 0;

                    #region calculate NewSolution using information sharing mechanism
                    // Helper variables.
                    double OldFitnessEmployed, NewFitnessEmployed;

                    // Save the cost function value of the current solution.
                    OldFitnessEmployed = this._population[e].trainFitness;

                    #region information sharing mechanism
                    // Randomly choose another solution.
                    int r;

                    // Check that randomly choosen solution is noth current solution.
                    while (true)
                    {
                        r = rand.Next(this._parameters.populationSize);
                        if (r != e)
                            break;
                    }

                    Chromosome NewSolutionEmployed = (Chromosome)this._population.CrossoverWithDifferenceControl(e, r, this._parameters.maxDepth, this._train, this._test, this._parameters.probability);
                    //NewSolutionEmployed = (Chromosome)this.population.Crossover( (Chromosome)this.population[e].Clone(), (Chromosome)this.population[r].Clone(), this.parameters.maxDepth, trainData, this.parameters.probability);
                    #endregion

                    // Calculate the cost function value of new solution.
                    NewFitnessEmployed = NewSolutionEmployed.trainFitness;
                    #endregion

                    #region greedy selection between OldSolution and NewSolution
                    // Considering the cost values, apply the greedy selection between OldSolution and NewSolution.                     

                    // Update solution if better.
                    if (NewFitnessEmployed > OldFitnessEmployed)
                    {
                        this._population[e] = NewSolutionEmployed;

                        // Solution has become better. Put Limit of that solution to 0.
                        Limits[e] = 0;

                        // Solution has become better. Put TestLimit of that solution to 1.
                        TestLimits[e] = 1;
                    }
                    #endregion
                }
                #endregion

                #region app related
                // Printout to console.
                PrintToConsole(formForCreatingNewModel, consoleTextBox, "(" + Iteration.ToString() + ") employed bee phase done ");
                #endregion

                //
                // Update best solution.
                //
                BestSolutions();

                //
                // Calculate the probabilities values ( P_i ) for the solutions.
                //
                this._population.CalculateProbabilities(this.bestTrainIndex, this._parameters.alpha);

                #region ----- ONLOOK BEES PHASE -----
                // Number of onlookers.
                int o = 0;
                // Food source index <-> Chromosome index.
                int f = 0;

                // For all onlooker bees do ...
                // (number of onlookers bees are same as population size)
                while (o < this._parameters.populationSize)
                {
                    // Select a solution OldSolution depending on P_i. 
                    // Better solutions have higher probabilities so we will select this solution if it has higher probability.
                    bool SelectThisSolution = rand.Next(100) < (this._population.probabilities[f] * 100) ? true : false;

                    #region food source not selected by onlooker bee
                    // It is choosed not to select this solution.
                    if (!SelectThisSolution)
                    {
                        // Update source.
                        f++;

                        // Check if came to the end.
                        if (f == this._population.populationSize)
                            f = 0;

                        continue;
                    }
                    #endregion

                    #region food source selected by onlooker bee
                    // Use onlooker bee for this source.
                    o++;

                    #region calculate NewSolution using information sharing mechanism
                    double OldFitnessOnlook, NewFitnessOnlook;

                    // Save the cost function value of the current solution.
                    OldFitnessOnlook = this._population[f].trainFitness;

                    #region information sharing mechanism
                    // Randomly choose another solution.
                    int r;

                    // Check that randomly choosen solution is noth current solution.
                    while (true)
                    {
                        r = rand.Next(this._parameters.populationSize);
                        if (r != f)
                            break;
                    }

                    Chromosome NewSolutionOnlook = this._population.CrossoverWithDifferenceControl(f, r, this._parameters.maxDepth, this._train, this._test, this._parameters.probability);
                    //NewSolutionOnlook = (Chromosome)this.population.Crossover((Chromosome)this.population[f].Clone(), (Chromosome)this.population[r].Clone(), this.parameters.maxDepth, trainData, this.parameters.probability);
                    #endregion

                    // Calculate the cost function value of new solution.
                    NewFitnessOnlook = NewSolutionOnlook.trainFitness;
                    #endregion

                    #region greedy selection between OldSolution and NewSolution
                    // Considering the cost values, apply the greedy selection between OldSolution and NewSolution. 

                    // Update solution if better.
                    if (NewFitnessOnlook > OldFitnessOnlook)
                    {
                        this._population[f] = NewSolutionOnlook;

                        // Solution has become better. Put Limit of that solution to 0.
                        Limits[f] = 0;

                        // Solution has become better. Put TestLimit of that solution to 1.
                        TestLimits[f] = 1;
                    }
                    #endregion

                    // Update source.
                    f++;

                    // Check if came to the end.
                    if (f == this._population.populationSize)
                        f = 0;
                    #endregion
                }

                #endregion

                #region app related
                // Printout to console.
                PrintToConsole(formForCreatingNewModel, consoleTextBox, "(" + Iteration.ToString() + ") onlooker bee phase done ");
                #endregion


                #region ----- SCOUT BEES PHASE -----
                for (var s = 0; s < this._parameters.populationSize; s++)
                {
                    // Check if if EBP or OBP improved this solution.
                    if (TestLimits[s] == 0)
                    {
                        // Neither in EBP nor in OBS this solution is not improved.
                        // Keep track of that in Limits variable.
                        Limits[s]++;
                    }

                    // Check if 'limit' number of iterations in a row this solution is not improved.
                    // TODO: change best after limit?
                    if (Limits[s] >= this._parameters.limit && s != this.bestTrainIndex)
                    {
                        #region generate new solution with difference control
                        // If that is so, generate new solution using "grow" method.
                        // And make sure it's different food source than others.
                        Chromosome NewSolutionScout = new Chromosome();

                        int counter = 0;

                        // While new solution is the same as some of the previous, try making a new one.
                        while (true)
                        {
                            // Variable that makes sure program does not stuck at infinite loop.
                            counter++;

                            // Generate new solution with grow method.
                            NewSolutionScout.Generate("grow", this._parameters.initialMaxDepth, this._data.featureNames, this._train, this._test, this._data.mathOperators, this._data.mathOperationsArity);

                            // Check if new solution is different.
                            bool isDifferent = true;
                            for (var i = 0; i < this._population.populationSize; i++)
                            {
                                if (this._population.chromosomes[i].Equals(NewSolutionScout))
                                {
                                    isDifferent = false;
                                    break;
                                }
                            }

                            // End loop if solution is different.
                            if (isDifferent)
                            {
                                this._population[s] = NewSolutionScout;
                                break;
                            }

                            // Throw a warning if the program was not able to generate solution different than others 1000 times.
                            if (counter == 1000)
                                throw new WarningException("[ABCP - scout phase] Stuck at infinite loop while trying to generate different solution.");
                        }
                        #endregion
                    }
                }
                #endregion

                #region app related
                // Printout to console.
                PrintToConsole(formForCreatingNewModel, consoleTextBox, "(" + Iteration.ToString() + ") scout bee phase done ");
                #endregion

                //
                // Memorize the best solution so far.
                // 
                BestSolutions();

                //
                // Check if best solution is updated and change variable IterationNotImproving accordingly.
                // 
                if (this._population[this.bestTrainIndex].trainFitness != OldBestFitness)
                {
                    // Counter for number of continually iterations that did not improve best solution brought back to zero.
                    IterationNotImproving = 0;
                }
                else
                {
                    // Increase number of done not improved iterations.
                    IterationNotImproving++;
                }

                //
                // Increase number of done iterations.
                //
                Iteration++;

                #region app related
                // Printout to console.
                PrintoutToConsoleIterEnd(formForCreatingNewModel, consoleTextBox, Iteration);

                // Add to chart.
                AddPointsToChartInIterationOfABCP(formForCreatingNewModel, dictKey, Iteration);

                // Update progress bar.
                UpdateProgressBar(formForCreatingNewModel, dictKey, progressBar, Iteration, IterationNotImproving);
                #endregion
            }
        }
        #endregion

        #region qsABCP
        /// <summary>
        /// QUICK SEMANTIC ARTIFICIAL BEE COLONY PROGRAMINNG ALGORITHM.
        /// This method is used to find the best solution for some data using meta-heuristic abc programming.
        /// It is considered all variables for class ABCP are set.
        /// </summary>
        /// 
        /// <param name="formForCreatingNewModel">Form that called this method - for printouts.</param>
        /// <param name="consoleTextBox">TextBox for printouts.</param>
        /// <param name="dictKey">Dictionary key - representing current run(+1) (or fold index if using folds). </param>
        public void qsABCP(CreateNewModelForm formForCreatingNewModel, TextBox consoleTextBox, int dictKey, ProgressBar progressBar)
        {
            // Helper arrays for keeping track of case when Solution (i) is not improved.
            int[] Limits = new int[this.parameters.populationSize];
            int[] TestLimits = new int[this.parameters.populationSize];

            // 1. Produce the initial solutions (x_i) by using ramped half-and-half method, randomly.  
            // -- DONE IN CONSTRUCTOR --

            // 2: Evaluate the solutions. 
            // -- DONE IN CONSTRUCTOR --

            // 3: Memorize the best. 
            // -- DONE IN CONSTRUCTOR --

            // Add points to chart.
            AddPointsToChartAtBeginningOfABCP(formForCreatingNewModel, dictKey);

            // 4: Set the cycle counter 
            // 
            int Iteration = 0;
            int IterationNotImproving = 0;

            while (Iteration <= this._parameters.maxNumberOfIterations && IterationNotImproving <= this._parameters.maxNumberOfNotImprovingIterations)
            {
                //
                // Memorize best fitness from the start of the current iteration.
                //
                double OldBestFitness = this._population[this.bestTrainIndex].trainFitness;

                #region app related
                // Printout to console.
                PrintToConsole(formForCreatingNewModel, consoleTextBox, "ABCP STEP " + Iteration.ToString());
                #endregion

                #region ----- EMPLOYED BEES PHASE -----
                // For all employed bees do ...
                // (number of employed bees are same as population size)
                for (var e = 0; e < this._parameters.populationSize; e++)
                {
                    // Reset TestLimits[e].
                    TestLimits[e] = 0;

                    #region calculate NewSolution using information sharing mechanism
                    // Helper variables.
                    double OldFitnessEmployed, NewFitnessEmployed;

                    // Save the cost function value of the current solution.
                    OldFitnessEmployed = this._population[e].trainFitness;

                    // [NEW] - semantic ABCP
                    // TODO custom parameters 'MAX', 'maxTrial' and 'LBSS'
                    Chromosome NewSolutionEmployed = SemanticInformationSharingMechanism(this._population[e].Clone(), e, 1000000, 10, 0.01);

                    // Calculate the cost function value of new solution.
                    NewFitnessEmployed = NewSolutionEmployed.trainFitness;
                    #endregion

                    #region greedy selection between OldSolution and NewSolution
                    // Considering the cost values, apply the greedy selection between OldSolution and NewSolution.                     

                    // Update solution if better.
                    if (NewFitnessEmployed > OldFitnessEmployed)
                    {
                        this._population[e] = NewSolutionEmployed;

                        // Solution has become better. Put Limit of that solution to 0.
                        Limits[e] = 0;

                        // Solution has become better. Put TestLimit of that solution to 1.
                        TestLimits[e] = 1;
                    }
                    #endregion
                }
                #endregion

                #region app related
                // Printout to console.
                PrintToConsole(formForCreatingNewModel, consoleTextBox, "(" + Iteration.ToString() + ") employed bee phase done ");
                #endregion

                //
                // Update best solution.
                //
                BestSolutions();

                //
                // Calculate the probabilities values ( P_i ) for the solutions.
                //
                this._population.CalculateProbabilities(this.bestTrainIndex, this._parameters.alpha);


                #region ----- ONLOOK BEES PHASE -----
                // Number of onlookers.
                int o = 0;
                // Food source index <-> Chromosome index.
                int f = 0;

                // For all onlooker bees do ...
                // (number of onlookers bees are same as population size)
                while (o < this._parameters.populationSize)
                {
                    // Select a solution OldSolution depending on P_i. 
                    // Better solutions have higher probabilities so we will select this solution if it has higher probability.
                    bool SelectThisSolution = rand.Next(100) < (this._population.probabilities[f] * 100) ? true : false;

                    #region food source not selected by onlooker bee
                    // It is choosed not to select this solution.
                    if (!SelectThisSolution)
                    {
                        // Update source.
                        f++;

                        // Check if came to the end.
                        if (f == this._population.populationSize)
                            f = 0;

                        continue;
                    }
                    #endregion

                    #region food source selected by onlooker bee
                    // Use onlooker bee for this source.
                    o++;

                    #region calculate NewSolution using information sharing mechanism
                    double OldFitnessOnlook, NewFitnessOnlook;

                    // Save the cost function value of the current solution.
                    OldFitnessOnlook = this._population[f].trainFitness;

                    // [NEW] - quick ABCP
                    // TODO: custom parameter 'r'

                    // Update neighbourhood.
                    this._population.neighbors[f] = new List<int>();
                    this._population.SearchNeighbourhood(f, 1, train);

                    Chromosome BestNeighbourOnlook = this._population[this._population.bestNeighbors[f]].Clone();

                    // [NEW] - semantic ABCP
                    // TODO custom parameters 'MAX', 'maxTrial' and 'LBSS'
                    Chromosome NewSolutionOnlook = SemanticInformationSharingMechanism(BestNeighbourOnlook, f, 1000000, 10, 0.01);

                    // Calculate the cost function value of new solution.
                    NewFitnessOnlook = NewSolutionOnlook.trainFitness;
                    #endregion

                    #region greedy selection between OldSolution and NewSolution
                    // Considering the cost values, apply the greedy selection between OldSolution and NewSolution. 

                    // Update solution if better.
                    if (NewFitnessOnlook > BestNeighbourOnlook.trainFitness)
                    {
                        this._population[f] = NewSolutionOnlook;

                        // Solution has become better. Put Limit of that solution to 0.
                        Limits[f] = 0;

                        // Solution has become better. Put TestLimit of that solution to 1.
                        TestLimits[f] = 1;
                    }
                    #endregion

                    // Update source.
                    f++;

                    // Check if came to the end.
                    if (f == this._population.populationSize)
                        f = 0;
                    #endregion
                }

                #endregion

                #region app related
                // Printout to console.
                PrintToConsole(formForCreatingNewModel, consoleTextBox, "(" + Iteration.ToString() + ") onlooker bee phase done ");
                #endregion


                #region ----- SCOUT BEES PHASE -----
                for (var s = 0; s < this._parameters.populationSize; s++)
                {
                    // Check if if EBP or OBP improved this solution.
                    if (TestLimits[s] == 0)
                    {
                        // Neither in EBP nor in OBS this solution is not improved.
                        // Keep track of that in Limits variable.
                        Limits[s]++;
                    }

                    // Check if 'limit' number of iterations in a row this solution is not improved.
                    // TODO: change best after limit?
                    if (Limits[s] >= this._parameters.limit)
                    {
                        #region generate new solution with difference control
                        // If that is so, generate new solution using "grow" method.
                        // And make sure it's different food source than others.
                        Chromosome NewSolutionScout = new Chromosome();

                        int counter = 0;

                        // While new solution is the same as some of the previous, try making a new one.
                        while (true)
                        {
                            // Variable that makes sure program does not stuck at infinite loop.
                            counter++;

                            // Generate new solution with grow method.
                            NewSolutionScout.Generate("grow", this._parameters.initialMaxDepth, this._data.featureNames, this._train, this._test, this._data.mathOperators, this._data.mathOperationsArity);

                            // Check if new solution is different.
                            bool isDifferent = true;
                            for (var i = 0; i < this._population.populationSize; i++)
                            {
                                if (this._population.chromosomes[i].Equals(NewSolutionScout))
                                {
                                    isDifferent = false;
                                    break;
                                }
                            }

                            // End loop if solution is different.
                            if (isDifferent)
                            {
                                this._population[s] = NewSolutionScout;
                                break;
                            }

                            // Throw a warning if the program was not able to generate solution different than others 1000 times.
                            if (counter == 1000)
                                throw new WarningException("[ABCP - scout phase] Stuck at infinite loop while trying to generate different solution.");
                        }
                        #endregion
                    }
                }
                #endregion

                #region app related
                // Printout to console.
                PrintToConsole(formForCreatingNewModel, consoleTextBox, "(" + Iteration.ToString() + ") scout bee phase done ");
                #endregion

                //
                // Memorize the best solution so far.
                // 
                BestSolutions();

                //
                // Check if best solution is updated and change variable IterationNotImproving accordingly.
                // 
                if (this._population[this.bestTrainIndex].trainFitness != OldBestFitness)
                {
                    // Counter for number of continually iterations that did not improve best solution brought back to zero.
                    IterationNotImproving = 0;
                }
                else
                {
                    // Increase number of done not improved iterations.
                    IterationNotImproving++;
                }

                //
                // Increase number of done iterations.
                //
                Iteration++;

                #region app related
                // Printout to console.
                PrintoutToConsoleIterEnd(formForCreatingNewModel, consoleTextBox, Iteration);

                // Add to chart.
                AddPointsToChartInIterationOfABCP(formForCreatingNewModel, dictKey, Iteration);

                // Update progress bar.
                UpdateProgressBar(formForCreatingNewModel, dictKey, progressBar, Iteration, IterationNotImproving);
                #endregion
            }
        }

        #region Semantic information sharing mechanism
        /// 
        /// <summary>
        /// In order to improve the performance of the standard ABCP, a higher locality is attempted to be achieved by using a
        /// semantic based neighborhood searching mechanism (information sharing mechanism), which is used to generate 
        /// candidate solutions. 
        /// 
        /// It is expected that ABCP will have more controlled fluctuations in the employed and onlooker bee phases. 
        /// Improved structure of the most semantically similar crossover (MSSC) method is used  
        /// </summary>
        /// 
        /// <param name="primaryParent">First - primary chromosome (parent) participating in the crossover. </param>
        /// <param name="primaryParentIndex"> Index of the first - primary chromosome (parent) participating in the crossover.</param>
        /// <param name="max">MAX parametera.</param>
        /// <param name="maxTrial">MaxTrial parameter.</param>
        /// <param name="lbss">Lower bound for semantic sensitivity.</param>
        /// <returns> New solution. </returns>
        private Chromosome SemanticInformationSharingMechanism(Chromosome primaryParent, int primaryParentIndex, double max, int maxTrial, double lbss)
        {
            // Randomly choose second parent.
            int r;

            // Check that randomly choosen solution is not current solution.
            while (true)
            {
                r = rand.Next(this._parameters.populationSize);
                if (r != primaryParentIndex)
                    break;
            }

            int count = 0;

            // Choosen crossover point (index of node) of primary parent.
            int primaryParentCrossoverPoint = -1;

            // Choosen crossover point (node) of secundary parent.
            SymbolicTreeNode secundaryParentCrossoverNode = new SymbolicTreeNode();

            while (count < maxTrial)
            {
                //
                // Randomly select a node from primaryParent and so, determine the subtree ST_p.
                // Randomly select a node from secundaryParent and so, determine the sub tree ST_s.
                //

                Tuple<int, SymbolicTreeNode> subtrees = this._population.ChooseSubtrees(primaryParent, this._population[r].Clone(), this._parameters.probability, this._parameters.maxDepth);
                SymbolicTreeNode primarySubtree = primaryParent.symbolicTree.FindNodeWithIndex(subtrees.Item1);


                //
                // Calculate distances of the other solutions in the population to x_i.
                //

                #region distance of subtrees
                // Distance between primary subtree and secundary subtree.
                double d = 0;

                // Number of samples.
                int T = this.train.Rows.Count;

                // Calculate x_i values of train data.
                for (var t = 0; t < T; t++)
                {
                    d += Math.Abs( primarySubtree.Evaluate(this.train.Rows[t]) - subtrees.Item2.Evaluate(this.train.Rows[t]) );

                    #region handling infinity
                    if (Double.IsPositiveInfinity(d))
                    {
                        d = 999999;
                    }
                    else if (Double.IsNegativeInfinity(d))
                    {
                        d = -999999;
                    }
                    #endregion
                }

                d = (double)((double) d / (double) T);
                #endregion

                if (d < max && d > lbss)
                {
                    max = d;
                    primaryParentCrossoverPoint = subtrees.Item1;
                    secundaryParentCrossoverNode = subtrees.Item2;
                }

                count++;
            }

            // Check if subtrees are choosen. If not, do basic crossover.
            if (primaryParentCrossoverPoint == -1)
            {
                // Do basic crossover.
                return (Chromosome)this._population.CrossoverWithDifferenceControl(primaryParentIndex, r, this._parameters.maxDepth, this._train, this._test, this._parameters.probability);

            }

            // Subtrees are found. Make child solution out of it.

            // Variable for tracking if we found the crossover point in PlaceNodeAtPoint method.
            bool found = false;

            // Place subtree of secondary parent in primary parent crossover point to create a child.
            var ret = this._population.PlaceNodeAtPoint((SymbolicTreeNode)primaryParent.symbolicTree, primaryParentCrossoverPoint, secundaryParentCrossoverNode, ref found);

            #region child setup
            // Solution created with crossover.
            Chromosome child = new Chromosome();

            // Update child tree.
            child.symbolicTree = ret.Item1;
            // Node indices should be re-calculated.
            child.symbolicTree.CalculateIndices();
            // Update number of possible terminals.
            child.numberOfPossibleTerminals = primaryParent.numberOfPossibleTerminals;
            // Update depth of child solution.
            child.depth = child.symbolicTree.DepthOfSymbolicTree();
            // Update accuracy (train+test) (fitness+tn/tp/fn/fp)
            child.UpdateTNTPFNFP(this._train, this._test);
            #endregion

            #region check if child is unique
            // Check if new solution is different.
            bool isDifferent = true;
            for (var s = 0; s < this._population.populationSize; s++)
            {
                if (this._population[s].Equals(child))
                {
                    isDifferent = false;
                    break;
                }
            }

            // End loop if solution is different.
            if (isDifferent)
                return child;
            else
            {
                // Do basic crossover.
                return (Chromosome)this._population.CrossoverWithDifferenceControl(primaryParentIndex, r, this._parameters.maxDepth, this._train, this._test, this._parameters.probability);
            }
            #endregion
        }
        #endregion

        #endregion

        #region APP RELATED HELPER METHODS FOR ABCP

        #region progress bar
        /// <summary>
        /// Helper method for updating progress bar at the end of iteration.
        /// </summary>
        /// 
        /// <param name="formForCreatingNewModel">Form that called this method - for printouts.</param>
        /// <param name="progressBar">Progress bar to be updated.</param>
        /// <param name="dictKey">Dictionary key - representing current run(+1) (or fold index if using folds). </param>
        /// <param name="Iteration">Current iteration.</param>
        /// <param name="IterationNotImproving">Current count of iterations in row that did not improve the best solution.</param>
        private void UpdateProgressBar(CreateNewModelForm formForCreatingNewModel, int dictKey, ProgressBar progressBar, int Iteration, int IterationNotImproving)
        {
            if (IterationNotImproving > this._parameters.maxNumberOfNotImprovingIterations || Iteration > this._parameters.maxNumberOfIterations)
            {
                formForCreatingNewModel.Invoke((MethodInvoker)delegate { progressBar.Value = dictKey * this._parameters.maxNumberOfIterations; });
            }
            else
            {
                formForCreatingNewModel.Invoke((MethodInvoker)delegate { progressBar.Value = progressBar.Value + 1; });
            }
        }
        #endregion

        #region chart
        /// <summary>
        /// Helper method for adding points to chart in progress form in app at the beggining of ABCP.
        /// </summary>
        /// 
        /// <param name="formForCreatingNewModel">Form that called this method - for printouts.</param>
        /// <param name="dictKey">Dictionary key - representing current run(+1) (or fold index if using folds). </param>
        private void AddPointsToChartAtBeginningOfABCP(CreateNewModelForm formForCreatingNewModel, int dictKey)
        {
            // Add to progress form dictionary.
            this.AddToProgressFormDictionary(formForCreatingNewModel, dictKey);

            // Check if nothing is yet shown on progress form.
            if (!formForCreatingNewModel.progressForm.chart.Visible && !formForCreatingNewModel.progressForm.depthsChart.Visible)
            {
                // No chart is shown. Start showing chart of this run with fitness values.
                AddToNewChart(formForCreatingNewModel, dictKey);
            }
            else if (formForCreatingNewModel.progressForm.selectedRun == dictKey)
            {
                // Something is already visible. 
                // Add to the chart if this run.
                AddPointsToChart(formForCreatingNewModel, dictKey, 0);
            }
        }

        /// <summary>
        /// Helper method for adding points to chart in progress form in app at the end of current iteration of ABCP.
        /// </summary>
        /// 
        /// <param name="formForCreatingNewModel">Form that called this method - for printouts.</param>
        /// <param name="dictKey">Dictionary key - representing current run(+1) (or fold index if using folds). </param>
        /// <param name="Iteration">Current iteration.</param>
        private void AddPointsToChartInIterationOfABCP(CreateNewModelForm formForCreatingNewModel, int dictKey, int Iteration)
        {
            // Add to chart.
            AddToProgressFormDictionary(formForCreatingNewModel, dictKey);
            if (formForCreatingNewModel.progressForm.selectedRun == dictKey)
            {
                // Something is already visible. 
                // Add to the chart if this run.
                AddPointsToChart(formForCreatingNewModel, dictKey, Iteration);
            }
        }

        /// <summary>
        /// This method is called when points needs to be added to chart that is currently showing on progress form.
        /// </summary>
        /// 
        /// <param name="formForCreatingNewModel">Form that called ABCP.</param>
        /// <param name="dictKey">Dictionary key - representing current run (+1) (or fold index if using folds). </param>
        /// <param name="Iteration">Iteration from which this method is called.</param>
        private static void AddPointsToChart(CreateNewModelForm formForCreatingNewModel, int dictKey, int Iteration)
        {
            switch (formForCreatingNewModel.progressForm.typeOfData)
            {
                case "fitness":
                    // Add new points to train and test series.
                    formForCreatingNewModel.Invoke((MethodInvoker)delegate {
                        formForCreatingNewModel.progressForm.chart.Series[0].Values.Add(new ObservablePoint(Iteration, formForCreatingNewModel.progressForm.fitnessPoints[dictKey].Item1[Iteration]));
                    });
                    formForCreatingNewModel.Invoke((MethodInvoker)delegate {
                        formForCreatingNewModel.progressForm.chart.Series[1].Values.Add(new ObservablePoint(Iteration, formForCreatingNewModel.progressForm.fitnessPoints[dictKey].Item2[Iteration]));
                    });
                    break;
                case "depths":
                    formForCreatingNewModel.Invoke((MethodInvoker)delegate {
                        formForCreatingNewModel.progressForm.depthsChart.Series[0].Values.Add(new ObservablePoint(Iteration, formForCreatingNewModel.progressForm.depthPoints[dictKey][Iteration]));
                    });
                    break;
                case "TP":
                    // Add new point to train and test series.
                    formForCreatingNewModel.Invoke((MethodInvoker)delegate {
                        formForCreatingNewModel.progressForm.chart.Series[0].Values.Add(new ObservablePoint(Iteration, formForCreatingNewModel.progressForm.tpPoints[dictKey].Item1[Iteration]));
                    });
                    formForCreatingNewModel.Invoke((MethodInvoker)delegate {
                        formForCreatingNewModel.progressForm.chart.Series[1].Values.Add(new ObservablePoint(Iteration, formForCreatingNewModel.progressForm.tpPoints[dictKey].Item2[Iteration]));
                    });
                    break;
                case "TN":
                    // Add new point to train and test series.
                    formForCreatingNewModel.Invoke((MethodInvoker)delegate {
                        formForCreatingNewModel.progressForm.chart.Series[0].Values.Add(new ObservablePoint(Iteration, formForCreatingNewModel.progressForm.tnPoints[dictKey].Item1[Iteration]));
                    });
                    formForCreatingNewModel.Invoke((MethodInvoker)delegate {
                        formForCreatingNewModel.progressForm.chart.Series[1].Values.Add(new ObservablePoint(Iteration, formForCreatingNewModel.progressForm.tnPoints[dictKey].Item2[Iteration]));
                    });
                    break;
                case "FN":
                    // Add new point to train and test series.
                    formForCreatingNewModel.Invoke((MethodInvoker)delegate {
                        formForCreatingNewModel.progressForm.chart.Series[0].Values.Add(new ObservablePoint(Iteration, formForCreatingNewModel.progressForm.fnPoints[dictKey].Item1[Iteration]));
                    });
                    formForCreatingNewModel.Invoke((MethodInvoker)delegate {
                        formForCreatingNewModel.progressForm.chart.Series[1].Values.Add(new ObservablePoint(Iteration, formForCreatingNewModel.progressForm.fnPoints[dictKey].Item2[Iteration]));
                    });
                    break;
                case "FP":
                    // Add new point to train and test series.
                    formForCreatingNewModel.Invoke((MethodInvoker)delegate {
                        formForCreatingNewModel.progressForm.chart.Series[0].Values.Add(new ObservablePoint(Iteration, formForCreatingNewModel.progressForm.fpPoints[dictKey].Item1[Iteration]));
                    });
                    formForCreatingNewModel.Invoke((MethodInvoker)delegate {
                        formForCreatingNewModel.progressForm.chart.Series[1].Values.Add(new ObservablePoint(Iteration, formForCreatingNewModel.progressForm.fpPoints[dictKey].Item2[Iteration]));
                    });
                    break;
                case "accuracy":
                    // Add new point to train and test series.
                    formForCreatingNewModel.Invoke((MethodInvoker)delegate {
                        formForCreatingNewModel.progressForm.accuracyChart.Series[0].Values.Add(new ObservablePoint(Iteration, formForCreatingNewModel.progressForm.accuracyPointsTrain[dictKey].Item1[Iteration]));
                    });
                    formForCreatingNewModel.Invoke((MethodInvoker)delegate {
                        formForCreatingNewModel.progressForm.accuracyChart.Series[1].Values.Add(new ObservablePoint(Iteration, formForCreatingNewModel.progressForm.accuracyPointsTrain[dictKey].Item2[Iteration]));
                    });
                    formForCreatingNewModel.Invoke((MethodInvoker)delegate {
                        formForCreatingNewModel.progressForm.accuracyChart.Series[2].Values.Add(new ObservablePoint(Iteration, formForCreatingNewModel.progressForm.accuracyPointsTrain[dictKey].Item3[Iteration]));
                    });
                    formForCreatingNewModel.Invoke((MethodInvoker)delegate {
                        formForCreatingNewModel.progressForm.accuracyChart.Series[3].Values.Add(new ObservablePoint(Iteration, formForCreatingNewModel.progressForm.accuracyPointsTrain[dictKey].Item4[Iteration]));
                    });
                    break;
                default:
                    throw new Exception("[ABCProgramming] Type " + formForCreatingNewModel.progressForm.typeOfData + " is not supported.");
            }
        }

        /// <summary>
        /// This method is called when progress form is not showing any chart.
        /// 
        /// Sets selectedRun and typeOfData variables in progress form on current run and fitness, 
        /// respectively. Clears chart and adds two new points.
        /// 
        /// </summary>
        /// 
        /// <param name="formForCreatingNewModel">Form that called ABCP.</param>
        /// <param name="dictKey">Dictionary key - representing current run (+1) (or fold index if using folds). </param>
        private void AddToNewChart(CreateNewModelForm formForCreatingNewModel, int dictKey)
        {
            // No chart is shown. Start showing chart of this run with fitness values.
            formForCreatingNewModel.Invoke((MethodInvoker)delegate { formForCreatingNewModel.progressForm.selectedRun = dictKey; });
            formForCreatingNewModel.Invoke((MethodInvoker)delegate { formForCreatingNewModel.progressForm.typeOfData = "fitness"; });

            // Add selected run and type to the menu.
            if (this.data.numberOfFolds == 0)
                formForCreatingNewModel.Invoke((MethodInvoker)delegate { formForCreatingNewModel.progressForm.runToolStripMenuItem.Text = "Run: " + dictKey; });
            else
                formForCreatingNewModel.Invoke((MethodInvoker)delegate { formForCreatingNewModel.progressForm.runToolStripMenuItem.Text = "Fold: " + dictKey; });

            formForCreatingNewModel.Invoke((MethodInvoker)delegate { formForCreatingNewModel.progressForm.dataForChartToolStripMenuItem.Text = "Data for chart: fitness"; });

            // Clear points from chart.
            formForCreatingNewModel.Invoke((MethodInvoker)delegate { formForCreatingNewModel.progressForm.chart.Series[0].Values.Clear(); });
            formForCreatingNewModel.Invoke((MethodInvoker)delegate { formForCreatingNewModel.progressForm.chart.Series[1].Values.Clear(); });

            // Make it visible.
            formForCreatingNewModel.Invoke((MethodInvoker)delegate { formForCreatingNewModel.progressForm.chart.Visible = true; });

            // Add new points to train and test series.
            for (var p = 0; p < formForCreatingNewModel.progressForm.fitnessPoints[dictKey].Item1.Count; p++)
            {
                // Add point to the chart from selected run.
                formForCreatingNewModel.Invoke((MethodInvoker)delegate { formForCreatingNewModel.progressForm.chart.Series[0].Values.Add(new ObservablePoint(p, formForCreatingNewModel.progressForm.fitnessPoints[dictKey].Item1[p])); });
                if (p < formForCreatingNewModel.progressForm.fitnessPoints[dictKey].Item2.Count)
                    formForCreatingNewModel.Invoke((MethodInvoker)delegate { formForCreatingNewModel.progressForm.chart.Series[1].Values.Add(new ObservablePoint(p, formForCreatingNewModel.progressForm.fitnessPoints[dictKey].Item2[p])); });
            }
        }

        /// <summary>
        /// Adding values to dictionaries in progress form needed for showing the progress of different data.
        /// </summary>
        /// 
        /// <param name="formForCreatingNewModel">Form that called ABCP.</param>
        /// <param name="dictKey">Dictionary key - representing current run (+1) (or fold index if using folds). </param>
        private void AddToProgressFormDictionary(CreateNewModelForm formForCreatingNewModel, int dictKey)
        {
            // Add points to dictionaries.

            // fitness
            formForCreatingNewModel.Invoke((MethodInvoker)delegate {
                formForCreatingNewModel.progressForm.fitnessPoints[dictKey].Item1.Add(this._population[this.bestTrainIndex].trainFitness);
            });
            formForCreatingNewModel.Invoke((MethodInvoker)delegate {
                formForCreatingNewModel.progressForm.fitnessPoints[dictKey].Item2.Add(this._population[this.bestTrainIndex].testFitness);
            });

            // depth
            formForCreatingNewModel.Invoke((MethodInvoker)delegate {
                formForCreatingNewModel.progressForm.depthPoints[dictKey].Add(this._population[this.bestTrainIndex].depth);
            });

            // TP
            formForCreatingNewModel.Invoke((MethodInvoker)delegate {
                formForCreatingNewModel.progressForm.tpPoints[dictKey].Item1.Add(this._population[this.bestTrainIndex].Train_TP_TN_FP_FN["TP"]);
            });
            formForCreatingNewModel.Invoke((MethodInvoker)delegate {
                formForCreatingNewModel.progressForm.tpPoints[dictKey].Item2.Add(this._population[this.bestTrainIndex].Test_TP_TN_FP_FN["TP"]);
            });

            // TN
            formForCreatingNewModel.Invoke((MethodInvoker)delegate {
                formForCreatingNewModel.progressForm.tnPoints[dictKey].Item1.Add(this._population[this.bestTrainIndex].Train_TP_TN_FP_FN["TN"]);
            });
            formForCreatingNewModel.Invoke((MethodInvoker)delegate {
                formForCreatingNewModel.progressForm.tnPoints[dictKey].Item2.Add(this._population[this.bestTrainIndex].Test_TP_TN_FP_FN["TN"]);
            });

            // FP
            formForCreatingNewModel.Invoke((MethodInvoker)delegate {
                formForCreatingNewModel.progressForm.fpPoints[dictKey].Item1.Add(this._population[this.bestTrainIndex].Train_TP_TN_FP_FN["FP"]);
            });
            formForCreatingNewModel.Invoke((MethodInvoker)delegate {
                formForCreatingNewModel.progressForm.fpPoints[dictKey].Item2.Add(this._population[this.bestTrainIndex].Test_TP_TN_FP_FN["FP"]);
            });

            // FN
            formForCreatingNewModel.Invoke((MethodInvoker)delegate {
                formForCreatingNewModel.progressForm.fnPoints[dictKey].Item1.Add(this._population[this.bestTrainIndex].Train_TP_TN_FP_FN["FN"]);
            });
            formForCreatingNewModel.Invoke((MethodInvoker)delegate {
                formForCreatingNewModel.progressForm.fnPoints[dictKey].Item2.Add(this._population[this.bestTrainIndex].Test_TP_TN_FP_FN["FN"]);
            });

            // TP + TN + FP + FN
            formForCreatingNewModel.Invoke((MethodInvoker)delegate {
                formForCreatingNewModel.progressForm.accuracyPointsTrain[dictKey].Item1.Add(this._population[this.bestTrainIndex].Train_TP_TN_FP_FN["TP"]);
            });
            formForCreatingNewModel.Invoke((MethodInvoker)delegate {
                formForCreatingNewModel.progressForm.accuracyPointsTrain[dictKey].Item2.Add(this._population[this.bestTrainIndex].Train_TP_TN_FP_FN["TN"]);
            });
            formForCreatingNewModel.Invoke((MethodInvoker)delegate {
                formForCreatingNewModel.progressForm.accuracyPointsTrain[dictKey].Item3.Add(this._population[this.bestTrainIndex].Train_TP_TN_FP_FN["FP"]);
            });
            formForCreatingNewModel.Invoke((MethodInvoker)delegate {
                formForCreatingNewModel.progressForm.accuracyPointsTrain[dictKey].Item4.Add(this._population[this.bestTrainIndex].Train_TP_TN_FP_FN["FN"]);
            });
        }
        #endregion

        #region console
        /// <summary>
        /// Helper method for printing to console in app.
        /// </summary>
        /// 
        /// <param name="formForCreatingNewModel">Form that called this method - for printouts.</param>
        /// <param name="consoleTextBox">TextBox for printouts.</param>
        /// <param name="input"> String to be printed.</param>
        private void PrintToConsole(CreateNewModelForm formForCreatingNewModel, TextBox consoleTextBox, string input)
        {
            string time = Microsoft.VisualBasic.DateAndTime.Now.ToString("MM/dd/yyyy HH:mm");
            formForCreatingNewModel.Invoke((MethodInvoker)delegate { consoleTextBox.AppendText("\r\n[" + time + "] " + input + "\r\n"); });
        }

        #region Printout to console at the end of the iteration.
        /// <summary>
        /// Printout to console at the end of one iteration of ABCP algorithm.
        /// </summary>
        /// 
        /// <param name="formForCreatingNewModel">Form that called ABCP method.</param>
        /// <param name="consoleTextBox">Console text box to witch printout needs to be added.</param>
        /// <param name="Iteration">Current iteration of the algorithm.</param>
        private void PrintoutToConsoleIterEnd(CreateNewModelForm formForCreatingNewModel, TextBox consoleTextBox, int Iteration)
        {
            PrintToConsole(formForCreatingNewModel, consoleTextBox, "(" + Iteration.ToString() + ") DONE.");

            if (this._bestTrainIndex == this._bestIndex)
            {
                if (this._bestTestIndex == this._bestIndex)
                {
                    // All three are the same.
                    PrintToConsole(formForCreatingNewModel, consoleTextBox, "(" + Iteration.ToString() + ") (=train=test) train fitness: " + this._population[this._bestIndex].trainFitness);
                    PrintToConsole(formForCreatingNewModel, consoleTextBox, "(" + Iteration.ToString() + ") (=train=test) test fitness: " + this._population[this._bestIndex].testFitness);
                    PrintToConsole(formForCreatingNewModel, consoleTextBox, "(" + Iteration.ToString() + ") (=train=test) train + test fitness: " + ((double)((this._population[this._bestIndex].trainFitness + this._population[this._bestIndex].testFitness) / 2)).ToString());
                }
                else
                {
                    // BestTest is different.
                    PrintToConsole(formForCreatingNewModel, consoleTextBox, "(" + Iteration.ToString() + ") (=train!=test)  train fitness: " + this._population[this._bestIndex].trainFitness);
                    PrintToConsole(formForCreatingNewModel, consoleTextBox, "(" + Iteration.ToString() + ") (=train!=test) train fitness: " + this._population[this._bestIndex].testFitness);
                    PrintToConsole(formForCreatingNewModel, consoleTextBox, "(" + Iteration.ToString() + ") (=train!=test) train fitness: " + ((double)((this._population[this._bestIndex].trainFitness + this._population[this._bestIndex].testFitness) / 2)).ToString());

                    PrintToConsole(formForCreatingNewModel, consoleTextBox, "(" + Iteration.ToString() + ") (=train!=test) (test) train fitness: " + this._population[this._bestTestIndex].trainFitness);
                    PrintToConsole(formForCreatingNewModel, consoleTextBox, "(" + Iteration.ToString() + ") (=train!=test) (test) train fitness: " + this._population[this._bestTestIndex].testFitness);
                    PrintToConsole(formForCreatingNewModel, consoleTextBox, "(" + Iteration.ToString() + ") (=train!=test) (test) train fitness: " + ((double)((this._population[this._bestTestIndex].trainFitness + this._population[this._bestTestIndex].testFitness) / 2)).ToString());
                }
            }
            else
            {
                if (this._bestTestIndex == this._bestIndex)
                {
                    // BestTrain is different.
                    PrintToConsole(formForCreatingNewModel, consoleTextBox, "(" + Iteration.ToString() + ") (=test!=train) (test) train fitness: " + this._population[this._bestIndex].trainFitness);
                    PrintToConsole(formForCreatingNewModel, consoleTextBox, "(" + Iteration.ToString() + ") (=test!=train) (test) train fitness: " + this._population[this._bestIndex].testFitness);
                    PrintToConsole(formForCreatingNewModel, consoleTextBox, "(" + Iteration.ToString() + ") (=test!=train) (test) train fitness: " + ((double)((this._population[this._bestIndex].trainFitness + this._population[this._bestIndex].testFitness) / 2)).ToString());

                    PrintToConsole(formForCreatingNewModel, consoleTextBox, "(" + Iteration.ToString() + ") (=test!=train) (train) train fitness: " + this._population[this._bestTrainIndex].trainFitness);
                    PrintToConsole(formForCreatingNewModel, consoleTextBox, "(" + Iteration.ToString() + ") (=test!=train) (train) train fitness: " + this._population[this._bestTrainIndex].testFitness);
                    PrintToConsole(formForCreatingNewModel, consoleTextBox, "(" + Iteration.ToString() + ") (=test!=train) (train) train fitness: " + ((double)((this._population[this._bestTrainIndex].trainFitness + this._population[this._bestTrainIndex].testFitness) / 2)).ToString());
                }
                else
                {
                    // All are different.
                    PrintToConsole(formForCreatingNewModel, consoleTextBox, "(" + Iteration.ToString() + ") (!=train!=test) (best) train fitness: " + this._population[this._bestIndex].trainFitness);
                    PrintToConsole(formForCreatingNewModel, consoleTextBox, "(" + Iteration.ToString() + ") (!=train!=test) (best) train fitness: " + this._population[this._bestIndex].testFitness);
                    PrintToConsole(formForCreatingNewModel, consoleTextBox, "(" + Iteration.ToString() + ") (!=train!=test) (best) train fitness: " + ((double)((this._population[this._bestIndex].trainFitness + this._population[this._bestIndex].testFitness) / 2)).ToString());

                    PrintToConsole(formForCreatingNewModel, consoleTextBox, "(" + Iteration.ToString() + ") (!=train!=test) (train) train fitness: " + this._population[this._bestTrainIndex].trainFitness);
                    PrintToConsole(formForCreatingNewModel, consoleTextBox, "(" + Iteration.ToString() + ") (!=train!=test) (train) train fitness: " + this._population[this._bestTrainIndex].testFitness);
                    PrintToConsole(formForCreatingNewModel, consoleTextBox, "(" + Iteration.ToString() + ") (!=train!=test) (train) train fitness: " + ((double)((this._population[this._bestTrainIndex].trainFitness + this._population[this._bestTrainIndex].testFitness) / 2)).ToString());

                    PrintToConsole(formForCreatingNewModel, consoleTextBox, "(" + Iteration.ToString() + ") (!=train!=test) (test) train fitness: " + this._population[this._bestTestIndex].trainFitness);
                    PrintToConsole(formForCreatingNewModel, consoleTextBox, "(" + Iteration.ToString() + ") (!=train!=test) (test) train fitness: " + this._population[this._bestTestIndex].testFitness);
                    PrintToConsole(formForCreatingNewModel, consoleTextBox, "(" + Iteration.ToString() + ") (!=train!=test) (test) train fitness: " + ((double)((this._population[this._bestTestIndex].trainFitness + this._population[this._bestTestIndex].testFitness) / 2)).ToString());
                }
            }
        }
        #endregion

        #endregion

        #endregion

        #endregion
    }
    #endregion
}
