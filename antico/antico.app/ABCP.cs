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

        #region the best

        #region best train and test combined
        // Variable that represents best current solution (model) on training and testing data (together).
        private Chromosome _best;

        // Property for the _best variable.
        public Chromosome best
        {
            get { return _best; }
            set
            {
                // Deep copy.
                _best = (Chromosome)value.Clone();
            }
        }
        #endregion

        #region best train
        // Variable that represents best current solution (model) on training data.
        private Chromosome _bestTrain;

        // Property for the _bestTrain variable.
        public Chromosome bestTrain
        {
            get { return _bestTrain; }
            set
            {
                // Deep copy.
                _bestTrain = (Chromosome)value.Clone();
            }
        }
        #endregion

        #region best test
        // Variable that represents best current solution (model) on testing data.
        private Chromosome _bestTest;

        // Property for the _bestTest variable.
        public Chromosome bestTest
        {
            get { return _bestTest; }
            set
            {
                // Deep copy.
                _bestTest = (Chromosome)value.Clone();
            }
        }
        #endregion

        #region best indices
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

        #endregion

        #region current train and test data

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

            // Reset known best solutions.
            this.best = new Chromosome();
            this.best = (Chromosome)this._population[bestSolutionIndex].Clone();

            this.bestTrain = new Chromosome();
            this.bestTrain = (Chromosome)this._population[bestTrainSolutionIndex].Clone();

            this.bestTest = new Chromosome();
            this.bestTest = (Chromosome)this._population[bestTestSolutionIndex].Clone();

            // Reset best indices.
            this._bestIndex = bestSolutionIndex;
            this._bestTrainIndex = bestTrainSolutionIndex;
            this._bestTestIndex = bestTestSolutionIndex;
        }
        #endregion

        #region Artificial bee colony programming 
        /// <summary>
        /// Artificial bee colony programming algorithm with specific train dataset.
        /// This method is used to find the best model for some data using heuristic abc programming.
        /// It is considered all variables for class ABCP are set.
        /// </summary>
        /// 
        /// <param name="formForCreatingNewModel">Form that called this method - for printouts.</param>
        /// <param name="consoleTextBox">TextBox for printouts.</param>
        public void ABCProgramming(CreateNewModelForm formForCreatingNewModel, TextBox consoleTextBox)
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

            // 4: Set the cycle counter(cycle = 0)
            int Iteration = 0;
            int IterationNotImproving = 0;

            while (Iteration <= this.parameters.maxNumberOfIterations && IterationNotImproving <= this.parameters.maxNumberOfNotImprovingIterations)
            {
                // Remeber best fitness from the beggining of the iteration.
                double OldBestFitness = this._bestTrain.trainFitness;

                // Printout to console.
                string time = Microsoft.VisualBasic.DateAndTime.Now.ToString("MM/dd/yyyy HH:mm");
                formForCreatingNewModel.Invoke((MethodInvoker)delegate { consoleTextBox.AppendText("\r\n[" + time + "] ABCP STEP " + Iteration.ToString() + "\r\n"); });

                #region ----- EMPLOYED BEES PHASE -----
                // For all employed bees do ...
                // (number of employed bees are same as population size)
                for (var e = 0; e < this.parameters.populationSize; e++)
                {
                    #region calculate NewSolution using information sharing mechanism
                    Chromosome NewSolutionEmployed = new Chromosome();

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

                    NewSolutionEmployed = (Chromosome)this._population.CrossoverWithDifferenceControl((Chromosome)this._population[e].Clone(), (Chromosome)this._population[r].Clone(), this._parameters.maxDepth, this._train, this._test, this._parameters.probability);
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
                        this._population[e] = (Chromosome)NewSolutionEmployed.Clone();

                        // Solution has become better. Put Limit of that solution to 0.
                        Limits[e] = 0;

                        // Solution has become better. Put TestLimit of that solution to 1.
                        TestLimits[e] = 1;
                    }
                    #endregion
                }
                #endregion

                // Printout to console.
                time = Microsoft.VisualBasic.DateAndTime.Now.ToString("MM/dd/yyyy HH:mm");
                formForCreatingNewModel.Invoke((MethodInvoker)delegate { consoleTextBox.AppendText("[" + time + "] (" + Iteration.ToString() + ") employed bee phase done\r\n"); });

                // Reset best solutions.
                BestSolutions();

                // Calculate the probability values ( P_i ) for the solutions.
                this._population.CalculateProbabilities(this._bestTrainIndex, this._parameters.alpha);

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
                    Chromosome NewSolutionOnlook = new Chromosome();

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

                    NewSolutionOnlook = (Chromosome)this._population.CrossoverWithDifferenceControl((Chromosome)this._population[f].Clone(), (Chromosome)this._population[r].Clone(), this._parameters.maxDepth, this._train, this._test, this._parameters.probability);
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
                        this._population[f] = (Chromosome)NewSolutionOnlook.Clone();

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

                // Printout to console.
                time = Microsoft.VisualBasic.DateAndTime.Now.ToString("MM/dd/yyyy HH:mm");
                formForCreatingNewModel.Invoke((MethodInvoker)delegate { consoleTextBox.AppendText("[" + time + "] (" + Iteration.ToString() + ") onlooker bee phase done\r\n"); });

                #region ----- SCOUT BEES PHASE -----
                for (var s = 0; s < this.parameters.populationSize; s++)
                {
                    // Check if if EBP or OBP improved this solution.
                    if (TestLimits[s] == 0)
                    {
                        // Neither in EBP nor in OBS this solution is not improved.
                        // Keep track of that in Limits variable.
                        Limits[s]++;
                    }

                    // Check if 'limit' number of iterations in a row this solution is not improved.
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
                                this._population[s] = (Chromosome)NewSolutionScout.Clone();
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

                // Printout to console.
                time = Microsoft.VisualBasic.DateAndTime.Now.ToString("MM/dd/yyyy HH:mm");
                formForCreatingNewModel.Invoke((MethodInvoker)delegate { consoleTextBox.AppendText("[" + time + "] (" + Iteration.ToString() + ") scout bee phase done\r\n"); });

                // Reset best solutions.
                BestSolutions();

                // Check if best solution is updated and change variable IterationNotImproving accordingly.
                if (this._bestTrain.trainFitness != OldBestFitness)
                {
                    // Counter for number of continually iterations that did not improve best solution brought back to zero.
                    IterationNotImproving = 0;
                }
                else
                {
                    // Increase number of done not improved iterations.
                    IterationNotImproving++;
                }

                // Printout to console.
                time = Microsoft.VisualBasic.DateAndTime.Now.ToString("MM/dd/yyyy HH:mm");
                formForCreatingNewModel.Invoke((MethodInvoker)delegate { consoleTextBox.AppendText("[" + time + "] (" + Iteration.ToString() + ") DONE.\r\n"); });
                formForCreatingNewModel.Invoke((MethodInvoker)delegate { consoleTextBox.AppendText("[" + time + "] (" + Iteration.ToString() + ") (train) fitness: " + this._bestTrain.trainFitness.ToString() + "\r\n"); });
                time = Microsoft.VisualBasic.DateAndTime.Now.ToString("MM/dd/yyyy HH:mm");
                formForCreatingNewModel.Invoke((MethodInvoker)delegate { consoleTextBox.AppendText("[" + time + "] (" + Iteration.ToString() + ") (test) fitness: " + this._bestTest.testFitness.ToString() + "\r\n"); });
                time = Microsoft.VisualBasic.DateAndTime.Now.ToString("MM/dd/yyyy HH:mm");
                formForCreatingNewModel.Invoke((MethodInvoker)delegate { consoleTextBox.AppendText("[" + time + "] (" + Iteration.ToString() + ") (train + test) fitness: " + ((double)((this._best.trainFitness + this._best.testFitness) / 2)).ToString() + "\r\n"); });

                // Increase number of done iterations.
                Iteration++;

                // Maybe this?
                // if( BestFitness == 1 ) break;
            }
        }
        #endregion

        #endregion
    }
    #endregion
}
