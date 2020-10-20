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
        /// <param name="run">Number of run - needed for live chart series selection.</param>
        public void ABCProgramming(CreateNewModelForm formForCreatingNewModel, TextBox consoleTextBox, int run, ProgressBar progressBar)
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

            #region add to chart
            // Add to progress form dictionary.
            this.AddToProgressFormDictionary(formForCreatingNewModel, run, 0);

            // Check if nothing is yet shown on progress form.
            if (!formForCreatingNewModel.progressForm.chart.Visible && !formForCreatingNewModel.progressForm.depthsChart.Visible)
            {
                // No chart is shown. Start showing chart of this run with fitness values.
                AddToNewChart(formForCreatingNewModel, run);
            }
            else if (formForCreatingNewModel.progressForm.selectedRun == run)
            {
                // Something is already visible. 
                // Add to the chart if this run.
                AddPointsToChart(formForCreatingNewModel, run, 0);
            }
            #endregion

            // 4: Set the cycle counter(cycle = 0)
            int Iteration = 0;
            int IterationNotImproving = 0;

            while (Iteration <= this._parameters.maxNumberOfIterations && IterationNotImproving <= this._parameters.maxNumberOfNotImprovingIterations)
            {
                // Remeber best fitness from the beggining of the iteration.
                double OldBestFitness = this._population[this._bestTrainIndex].trainFitness;

                // Printout to console.
                string time = Microsoft.VisualBasic.DateAndTime.Now.ToString("MM/dd/yyyy HH:mm");
                formForCreatingNewModel.Invoke((MethodInvoker)delegate { consoleTextBox.AppendText("\r\n[" + time + "] ABCP STEP " + Iteration.ToString() + "\r\n"); });

                #region ----- EMPLOYED BEES PHASE -----
                // For all employed bees do ...
                // (number of employed bees are same as population size)
                for (var e = 0; e < this._parameters.populationSize; e++)
                {
                    // Reset TestLimit[e].
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

                        if (e == bestTrainIndex)
                            Console.WriteLine("");

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
                this.BestSolutions();

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

                        if (f == bestTrainIndex)
                            Console.WriteLine("");
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
                    if (Limits[s] >= this._parameters.limit && s != this._bestTrainIndex)
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
                                if (s == bestTrainIndex)
                                    Console.WriteLine("");
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
                this.BestSolutions();

                // Check if best solution is updated and change variable IterationNotImproving accordingly.
                if (this._population[this._bestTrainIndex].trainFitness != OldBestFitness)
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
                PrintoutToConsoleIterEnd(formForCreatingNewModel, consoleTextBox, Iteration);

                // Increase number of done iterations.
                Iteration++;

                // Maybe this?
                // if( BestFitness == 1 ) break;

                #region add to chart
                // Add to chart.
                AddToProgressFormDictionary(formForCreatingNewModel, run, Iteration);
                if (formForCreatingNewModel.progressForm.selectedRun == run)
                {
                    // Something is already visible. 
                    // Add to the chart if this run.
                    AddPointsToChart(formForCreatingNewModel, run, Iteration);
                }
                #endregion

                if (IterationNotImproving > this._parameters.maxNumberOfNotImprovingIterations || Iteration > this._parameters.maxNumberOfIterations)
                {
                    formForCreatingNewModel.Invoke((MethodInvoker)delegate { progressBar.Value = run * this._parameters.maxNumberOfIterations; });
                }
                else
                {
                    formForCreatingNewModel.Invoke((MethodInvoker)delegate { progressBar.Value = progressBar.Value + 1; });
                }
            }
        }
        #endregion

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
            string time = Microsoft.VisualBasic.DateAndTime.Now.ToString("MM/dd/yyyy HH:mm");
            formForCreatingNewModel.Invoke((MethodInvoker)delegate { consoleTextBox.AppendText("[" + time + "] (" + Iteration.ToString() + ") DONE.\r\n"); });

            if (this._bestTrainIndex == this._bestIndex)
            {
                if (this._bestTestIndex == this._bestIndex)
                {
                    // All three are the same.
                    formForCreatingNewModel.Invoke((MethodInvoker)delegate { consoleTextBox.AppendText("[" + time + "] (" + Iteration.ToString() + ") (=train=test) train fitness: " + this._population[this._bestIndex].trainFitness.ToString() + "\r\n"); });
                    formForCreatingNewModel.Invoke((MethodInvoker)delegate { consoleTextBox.AppendText("[" + time + "] (" + Iteration.ToString() + ") (=train=test) test fitness: " + this._population[this._bestIndex].testFitness.ToString() + "\r\n"); });
                    formForCreatingNewModel.Invoke((MethodInvoker)delegate { consoleTextBox.AppendText("[" + time + "] (" + Iteration.ToString() + ") (=train=test) train + test fitness: " + ((double)((this._population[this._bestIndex].trainFitness + this._population[this._bestIndex].testFitness) / 2)).ToString() + "\r\n"); });
                }
                else
                {
                    // BestTest is different.
                    formForCreatingNewModel.Invoke((MethodInvoker)delegate { consoleTextBox.AppendText("[" + time + "] (" + Iteration.ToString() + ") (=train!=test) (train) train fitness: " + this._population[this._bestIndex].trainFitness.ToString() + "\r\n"); });
                    formForCreatingNewModel.Invoke((MethodInvoker)delegate { consoleTextBox.AppendText("[" + time + "] (" + Iteration.ToString() + ") (=train!=test) (train) test fitness: " + this._population[this._bestIndex].testFitness.ToString() + "\r\n"); });
                    formForCreatingNewModel.Invoke((MethodInvoker)delegate { consoleTextBox.AppendText("[" + time + "] (" + Iteration.ToString() + ") (=train!=test) (train) train + test fitness: " + ((double)((this._population[this._bestIndex].trainFitness + this._population[this._bestIndex].testFitness) / 2)).ToString() + "\r\n"); });

                    formForCreatingNewModel.Invoke((MethodInvoker)delegate { consoleTextBox.AppendText("[" + time + "] (" + Iteration.ToString() + ") (=train!=test) (test) train fitness: " + this._population[this._bestTestIndex].trainFitness.ToString() + "\r\n"); });
                    formForCreatingNewModel.Invoke((MethodInvoker)delegate { consoleTextBox.AppendText("[" + time + "] (" + Iteration.ToString() + ") (=train!=test) (test) test fitness: " + this._population[this._bestTestIndex].testFitness.ToString() + "\r\n"); });
                    formForCreatingNewModel.Invoke((MethodInvoker)delegate { consoleTextBox.AppendText("[" + time + "] (" + Iteration.ToString() + ") (=train!=test) (test) train + test fitness: " + ((double)((this._population[this._bestTestIndex].trainFitness + this._population[this._bestTestIndex].testFitness) / 2)).ToString() + "\r\n"); });
                }
            }
            else
            {
                if (this._bestTestIndex == this._bestIndex)
                {
                    // BestTrain is different.
                    formForCreatingNewModel.Invoke((MethodInvoker)delegate { consoleTextBox.AppendText("[" + time + "] (" + Iteration.ToString() + ") (=test!=train) (test) train fitness: " + this._population[this._bestIndex].trainFitness.ToString() + "\r\n"); });
                    formForCreatingNewModel.Invoke((MethodInvoker)delegate { consoleTextBox.AppendText("[" + time + "] (" + Iteration.ToString() + ") (=test!=train) (test) test fitness: " + this._population[this._bestIndex].testFitness.ToString() + "\r\n"); });
                    formForCreatingNewModel.Invoke((MethodInvoker)delegate { consoleTextBox.AppendText("[" + time + "] (" + Iteration.ToString() + ") (=test!=train) (test) train + test fitness: " + ((double)((this._population[this._bestIndex].trainFitness + this._population[this._bestIndex].testFitness) / 2)).ToString() + "\r\n"); });

                    formForCreatingNewModel.Invoke((MethodInvoker)delegate { consoleTextBox.AppendText("[" + time + "] (" + Iteration.ToString() + ") (=test!=train) (train) train fitness: " + this._population[this._bestTrainIndex].trainFitness.ToString() + "\r\n"); });
                    formForCreatingNewModel.Invoke((MethodInvoker)delegate { consoleTextBox.AppendText("[" + time + "] (" + Iteration.ToString() + ") (=test!=train) (train) test fitness: " + this._population[this._bestTrainIndex].testFitness.ToString() + "\r\n"); });
                    formForCreatingNewModel.Invoke((MethodInvoker)delegate { consoleTextBox.AppendText("[" + time + "] (" + Iteration.ToString() + ") (=test!=train) (train) train + test fitness: " + ((double)((this._population[this._bestTrainIndex].trainFitness + this._population[this._bestTrainIndex].testFitness) / 2)).ToString() + "\r\n"); });
                }
                else
                {
                    // All are different.
                    formForCreatingNewModel.Invoke((MethodInvoker)delegate { consoleTextBox.AppendText("[" + time + "] (" + Iteration.ToString() + ") (!=train!=test) () train fitness: " + this._population[this._bestIndex].trainFitness.ToString() + "\r\n"); });
                    formForCreatingNewModel.Invoke((MethodInvoker)delegate { consoleTextBox.AppendText("[" + time + "] (" + Iteration.ToString() + ") (!=train!=test) () test fitness: " + this._population[this._bestIndex].testFitness.ToString() + "\r\n"); });
                    formForCreatingNewModel.Invoke((MethodInvoker)delegate { consoleTextBox.AppendText("[" + time + "] (" + Iteration.ToString() + ") (!=train!=test) () train + test fitness: " + ((double)((this._population[this._bestIndex].trainFitness + this._population[this._bestIndex].testFitness) / 2)).ToString() + "\r\n"); });

                    formForCreatingNewModel.Invoke((MethodInvoker)delegate { consoleTextBox.AppendText("[" + time + "] (" + Iteration.ToString() + ") (!=train!=test) (train) train fitness: " + this._population[this._bestTrainIndex].trainFitness.ToString() + "\r\n"); });
                    formForCreatingNewModel.Invoke((MethodInvoker)delegate { consoleTextBox.AppendText("[" + time + "] (" + Iteration.ToString() + ") (!=train!=test) (train) test fitness: " + this._population[this._bestTrainIndex].testFitness.ToString() + "\r\n"); });
                    formForCreatingNewModel.Invoke((MethodInvoker)delegate { consoleTextBox.AppendText("[" + time + "] (" + Iteration.ToString() + ") (!=train!=test) (train) train + test fitness: " + ((double)((this._population[this._bestTrainIndex].trainFitness + this._population[this._bestTrainIndex].testFitness) / 2)).ToString() + "\r\n"); });

                    formForCreatingNewModel.Invoke((MethodInvoker)delegate { consoleTextBox.AppendText("[" + time + "] (" + Iteration.ToString() + ") (!=train!=test) (test) train fitness: " + this._population[this._bestTestIndex].trainFitness.ToString() + "\r\n"); });
                    formForCreatingNewModel.Invoke((MethodInvoker)delegate { consoleTextBox.AppendText("[" + time + "] (" + Iteration.ToString() + ") (!=train!=test) (test) test fitness: " + this._population[this._bestTestIndex].testFitness.ToString() + "\r\n"); });
                    formForCreatingNewModel.Invoke((MethodInvoker)delegate { consoleTextBox.AppendText("[" + time + "] (" + Iteration.ToString() + ") (!=train!=test) (test) train + test fitness: " + ((double)((this._population[this._bestTestIndex].trainFitness + this._population[this._bestTestIndex].testFitness) / 2)).ToString() + "\r\n"); });
                }
            }
        }
        #endregion

        #region Helper method for chart handling.

        /// <summary>
        /// This method is called when points needs to be added to chart that is currently showing on progress form.
        /// </summary>
        /// 
        /// <param name="formForCreatingNewModel">Form that called ABCP.</param>
        /// <param name="run">Current run (+1) of the search for best solution.</param>
        /// <param name="Iteration">Iteration from which this method is called.</param>
        private static void AddPointsToChart(CreateNewModelForm formForCreatingNewModel, int run, int Iteration)
        {
            switch (formForCreatingNewModel.progressForm.typeOfData)
            {
                case "fitness":
                    // Add new points to train and test series.
                    formForCreatingNewModel.Invoke((MethodInvoker)delegate { 
                        formForCreatingNewModel.progressForm.chart.Series[0].Values.Add(new ObservablePoint(Iteration, formForCreatingNewModel.progressForm.fitnessPoints[run].Item1[Iteration])); 
                    });
                    formForCreatingNewModel.Invoke((MethodInvoker)delegate { 
                        formForCreatingNewModel.progressForm.chart.Series[1].Values.Add(new ObservablePoint(Iteration, formForCreatingNewModel.progressForm.fitnessPoints[run].Item2[Iteration])); 
                    });
                    break;
                case "depths":
                    formForCreatingNewModel.Invoke((MethodInvoker)delegate { 
                        formForCreatingNewModel.progressForm.depthsChart.Series[0].Values.Add(new ObservablePoint(Iteration, formForCreatingNewModel.progressForm.depthPoints[run][Iteration])); 
                    });
                    break;
                case "TP":
                    // Add new point to train and test series.
                    formForCreatingNewModel.Invoke((MethodInvoker)delegate {
                        formForCreatingNewModel.progressForm.chart.Series[0].Values.Add(new ObservablePoint(Iteration, formForCreatingNewModel.progressForm.tpPoints[run].Item1[Iteration]));
                    });
                    formForCreatingNewModel.Invoke((MethodInvoker)delegate {
                        formForCreatingNewModel.progressForm.chart.Series[1].Values.Add(new ObservablePoint(Iteration, formForCreatingNewModel.progressForm.tpPoints[run].Item2[Iteration]));
                    });
                    break;
                case "TN":
                    // Add new point to train and test series.
                    formForCreatingNewModel.Invoke((MethodInvoker)delegate {
                        formForCreatingNewModel.progressForm.chart.Series[0].Values.Add(new ObservablePoint(Iteration, formForCreatingNewModel.progressForm.tnPoints[run].Item1[Iteration]));
                    });
                    formForCreatingNewModel.Invoke((MethodInvoker)delegate {
                        formForCreatingNewModel.progressForm.chart.Series[1].Values.Add(new ObservablePoint(Iteration, formForCreatingNewModel.progressForm.tnPoints[run].Item2[Iteration]));
                    });
                    break;
                case "FN":
                    // Add new point to train and test series.
                    formForCreatingNewModel.Invoke((MethodInvoker)delegate {
                        formForCreatingNewModel.progressForm.chart.Series[0].Values.Add(new ObservablePoint(Iteration, formForCreatingNewModel.progressForm.fnPoints[run].Item1[Iteration]));
                    });
                    formForCreatingNewModel.Invoke((MethodInvoker)delegate {
                        formForCreatingNewModel.progressForm.chart.Series[1].Values.Add(new ObservablePoint(Iteration, formForCreatingNewModel.progressForm.fnPoints[run].Item2[Iteration]));
                    });
                    break;
                case "FP":
                    // Add new point to train and test series.
                    formForCreatingNewModel.Invoke((MethodInvoker)delegate {
                        formForCreatingNewModel.progressForm.chart.Series[0].Values.Add(new ObservablePoint(Iteration, formForCreatingNewModel.progressForm.fpPoints[run].Item1[Iteration]));
                    });
                    formForCreatingNewModel.Invoke((MethodInvoker)delegate {
                        formForCreatingNewModel.progressForm.chart.Series[1].Values.Add(new ObservablePoint(Iteration, formForCreatingNewModel.progressForm.fpPoints[run].Item2[Iteration]));
                    });
                    break;
                case "accuracy":
                    // Add new point to train and test series.
                    formForCreatingNewModel.Invoke((MethodInvoker)delegate {
                        formForCreatingNewModel.progressForm.accuracyChart.Series[0].Values.Add(new ObservablePoint(Iteration, formForCreatingNewModel.progressForm.accuracyPointsTrain[run].Item1[Iteration]));
                    });
                    formForCreatingNewModel.Invoke((MethodInvoker)delegate {
                        formForCreatingNewModel.progressForm.accuracyChart.Series[1].Values.Add(new ObservablePoint(Iteration, formForCreatingNewModel.progressForm.accuracyPointsTrain[run].Item2[Iteration]));
                    });
                    formForCreatingNewModel.Invoke((MethodInvoker)delegate {
                        formForCreatingNewModel.progressForm.accuracyChart.Series[2].Values.Add(new ObservablePoint(Iteration, formForCreatingNewModel.progressForm.accuracyPointsTrain[run].Item3[Iteration]));
                    });
                    formForCreatingNewModel.Invoke((MethodInvoker)delegate {
                        formForCreatingNewModel.progressForm.accuracyChart.Series[3].Values.Add(new ObservablePoint(Iteration, formForCreatingNewModel.progressForm.accuracyPointsTrain[run].Item4[Iteration]));
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
        /// <param name="run">Current run of the search for best solution.</param>
        private static void AddToNewChart(CreateNewModelForm formForCreatingNewModel, int run)
        {
            // No chart is shown. Start showing chart of this run with fitness values.
            formForCreatingNewModel.Invoke((MethodInvoker)delegate { formForCreatingNewModel.progressForm.selectedRun = run; });
            formForCreatingNewModel.Invoke((MethodInvoker)delegate { formForCreatingNewModel.progressForm.typeOfData = "fitness"; });

            // Add selected run and type to the menu.
            formForCreatingNewModel.Invoke((MethodInvoker)delegate { formForCreatingNewModel.progressForm.runToolStripMenuItem.Text = "Run: " + run ; });
            formForCreatingNewModel.Invoke((MethodInvoker)delegate { formForCreatingNewModel.progressForm.dataForChartToolStripMenuItem.Text = "Data for chart: fitness"; });

            // Clear points from chart.
            formForCreatingNewModel.Invoke((MethodInvoker)delegate { formForCreatingNewModel.progressForm.chart.Series[0].Values.Clear(); });
            formForCreatingNewModel.Invoke((MethodInvoker)delegate { formForCreatingNewModel.progressForm.chart.Series[1].Values.Clear(); });

            // Make it visible.
            formForCreatingNewModel.Invoke((MethodInvoker)delegate { formForCreatingNewModel.progressForm.chart.Visible = true; });

            // Add new points to train and test series.
            for (var p = 0; p < formForCreatingNewModel.progressForm.fitnessPoints[run].Item1.Count; p++)
            {
                // Add point to the chart from selected run.
                formForCreatingNewModel.Invoke((MethodInvoker)delegate { formForCreatingNewModel.progressForm.chart.Series[0].Values.Add(new ObservablePoint(p, formForCreatingNewModel.progressForm.fitnessPoints[run].Item1[p])); });
                if (p < formForCreatingNewModel.progressForm.fitnessPoints[run].Item2.Count)
                    formForCreatingNewModel.Invoke((MethodInvoker)delegate { formForCreatingNewModel.progressForm.chart.Series[1].Values.Add(new ObservablePoint(p, formForCreatingNewModel.progressForm.fitnessPoints[run].Item2[p])); });
            }
        }

        /// <summary>
        /// Adding values to dictionaries in progress form needed for showing the progress of different data.
        /// </summary>
        /// 
        /// <param name="formForCreatingNewModel">Form that called ABCP.</param>
        /// <param name="run">Current run of the search for best solution.</param>
        /// <param name="Iteration">Iteration of ABCP algorithm.</param>
        private void AddToProgressFormDictionary(CreateNewModelForm formForCreatingNewModel, int run, int Iteration)
        {
            // Add points to dictionaries.

            // fitness
            formForCreatingNewModel.Invoke((MethodInvoker)delegate {
                formForCreatingNewModel.progressForm.fitnessPoints[run].Item1.Add(this._population[this._bestTrainIndex].trainFitness);
            });
            formForCreatingNewModel.Invoke((MethodInvoker)delegate {
                formForCreatingNewModel.progressForm.fitnessPoints[run].Item2.Add(this._population[this._bestTrainIndex].testFitness);
            });

            // depth
            formForCreatingNewModel.Invoke((MethodInvoker)delegate {
                formForCreatingNewModel.progressForm.depthPoints[run].Add(this._population[this._bestIndex].depth);
            });

            // TP
            formForCreatingNewModel.Invoke((MethodInvoker)delegate {
                formForCreatingNewModel.progressForm.tpPoints[run].Item1.Add(this._population[this._bestTrainIndex].Train_TP_TN_FP_FN["TP"]);
            });
            formForCreatingNewModel.Invoke((MethodInvoker)delegate {
                formForCreatingNewModel.progressForm.tpPoints[run].Item2.Add(this._population[this._bestTrainIndex].Test_TP_TN_FP_FN["TP"]);
            });

            // TN
            formForCreatingNewModel.Invoke((MethodInvoker)delegate {
                formForCreatingNewModel.progressForm.tnPoints[run].Item1.Add(this._population[this._bestTrainIndex].Train_TP_TN_FP_FN["TN"]);
            });
            formForCreatingNewModel.Invoke((MethodInvoker)delegate {
                formForCreatingNewModel.progressForm.tnPoints[run].Item2.Add(this._population[this._bestTrainIndex].Test_TP_TN_FP_FN["TN"]);
            });

            // FP
            formForCreatingNewModel.Invoke((MethodInvoker)delegate {
                formForCreatingNewModel.progressForm.fpPoints[run].Item1.Add(this._population[this._bestTrainIndex].Train_TP_TN_FP_FN["FP"]);
            });
            formForCreatingNewModel.Invoke((MethodInvoker)delegate { 
                formForCreatingNewModel.progressForm.fpPoints[run].Item2.Add(this._population[this._bestTrainIndex].Test_TP_TN_FP_FN["FP"]);
            });

            // FN
            formForCreatingNewModel.Invoke((MethodInvoker)delegate {
                formForCreatingNewModel.progressForm.fnPoints[run].Item1.Add(this._population[this._bestTrainIndex].Train_TP_TN_FP_FN["FN"]);
            });
            formForCreatingNewModel.Invoke((MethodInvoker)delegate {
                formForCreatingNewModel.progressForm.fnPoints[run].Item2.Add(this._population[this._bestTrainIndex].Test_TP_TN_FP_FN["FN"]);
            });

            // TP + TN + FP + FN
            formForCreatingNewModel.Invoke((MethodInvoker)delegate {
                formForCreatingNewModel.progressForm.accuracyPointsTrain[run].Item1.Add(this._population[this._bestTrainIndex].Train_TP_TN_FP_FN["TP"]);
            });
            formForCreatingNewModel.Invoke((MethodInvoker)delegate {
                formForCreatingNewModel.progressForm.accuracyPointsTrain[run].Item2.Add(this._population[this._bestTrainIndex].Train_TP_TN_FP_FN["TN"]);
            });
            formForCreatingNewModel.Invoke((MethodInvoker)delegate {
                formForCreatingNewModel.progressForm.accuracyPointsTrain[run].Item3.Add(this._population[this._bestTrainIndex].Train_TP_TN_FP_FN["FP"]);
            });
            formForCreatingNewModel.Invoke((MethodInvoker)delegate {
                formForCreatingNewModel.progressForm.accuracyPointsTrain[run].Item4.Add(this._population[this._bestTrainIndex].Train_TP_TN_FP_FN["FN"]);
            });
        }

        #endregion

        #endregion
    }
    #endregion
}
