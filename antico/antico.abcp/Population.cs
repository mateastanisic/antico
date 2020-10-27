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

namespace antico.abcp
{
    #region Population
    /// <summary>
    /// 
    /// Class that represents population of chromosomes (solutions). 
    /// In other words, set of Symbolic Trees that represent one model for classification problem.
    /// 
    /// Every population is represented with
    ///     chromosomes (list of solutions - chromosomes)
    ///     population size (number of chromosomes in solution)
    ///     probabilities (probabilities of choosing each of the solutions in onlooker bee phase)
    /// 
    /// </summary>
    [Serializable]
    public class Population
    {
        #region ATTRIBUTES 

        #region random
        // Variable for generating random numbers.
        private static Random rand;
        #endregion

        #region population size
        // Variable that represents size of population.
        private int _populationSize;

        // Property for the _populationSize variable.
        public int populationSize
        {
            get { return _populationSize; }
            set { _populationSize = value; }
        }
        #endregion

        #region probabilities
        // Variable that represents probabilities of choosing each of the solutions in onlooker bee phase.
        private double[] _probabilities;

        // Property for the _probabilities variable.
        public double[] probabilities
        {
            get { return _probabilities; }
            set 
            {
                // Allocate memory.
                _probabilities = new double[_populationSize];

                // Deep copy.
                for (var i = 0; i < value.Length; i++)
                {
                    _probabilities[i] = value[i];
                }
            }
        }
        #endregion

        #region population
        // Variable that represents set of symbolic trees.
        private List<Chromosome> _chromosomes;

        // Property for the _chromosomes variable.
        public List<Chromosome> chromosomes
        {
            get { return _chromosomes; }
            set 
            {
                // Allocate memory for the new population of chromosomes.
                _chromosomes = new List<Chromosome>();

                // Deep copy every chromosome.
                for (var i = 0; i < value.Count; i++)
                {
                    _chromosomes.Add(new Chromosome());
                    _chromosomes[i] = (Chromosome)value[i].Clone();
                }
            }
        }
        #endregion

        #endregion

        #region OPERATIONS

        #region Static constructor
        /// <summary>
        /// New static random.
        /// </summary>
        static Population()
        {
            rand = new Random();
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor with custom values for variables.
        /// </summary>
        /// 
        /// <param name="popSize">Size of population to be generated.</param>
        /// <param name="initialMaxDepth">Initial maximal depth of symbolic tree.</param>
        /// <param name="nonTerminals">String representations of non temrinals <-> mathematical operations.</param>
        /// <param name="mathOperationsArity"> Dictionary with all possible non-terminals and their arity. </param>
        /// <param name="terminalNames">String representations of terminal names.</param>
        /// <param name="trainData">Train feature values - for calculating fitness.</param>
        /// <param name="testData">Test feature values - for calculating accuracy.</param>
        /// <param name="generatingTreesMethod">Method for generating symbolic tree.</param
        public Population(int popSize, int initialMaxDepth, List<string> nonTerminals, Dictionary<string, int> mathOperationsArity, List<string> terminalNames, DataTable trainData, DataTable testData, string generatingTreesMethod)
        {
            // Set population size.
            this._populationSize = popSize;

            // Allocate memory.
            this._chromosomes = new List<Chromosome>();

            // Generate population of chromosomes.
            for (var s = 0; s < popSize; s++)
            {
                // Create new solution.
                _chromosomes.Add(new Chromosome());

                // Check depending on symbolic tree generating method.
                // Ramped half and half method has half of population generated with full method, and other half with grow method.
                if (generatingTreesMethod == "ramped")
                {
                    if (s < popSize / 2)
                    {
                        // Half population generate with full method.
                        GenerateSolutionWithDifferenceControl(s, "full", initialMaxDepth, terminalNames, trainData, testData, nonTerminals, mathOperationsArity);
                    }
                    else
                    {
                        // Other half generate with grow method.
                        GenerateSolutionWithDifferenceControl(s, "grow", initialMaxDepth, terminalNames, trainData, testData, nonTerminals, mathOperationsArity);
                    }
                }
                else
                {
                    // Full or grow method.
                    GenerateSolutionWithDifferenceControl(s, generatingTreesMethod, initialMaxDepth, terminalNames, trainData, testData, nonTerminals, mathOperationsArity);
                }
            }

            // Probabilities are calculated in ABCP algorithm.
            // Allocate memory.
            this._probabilities = new double[_populationSize];
        }
        #endregion

        #region Overloading operators
        /// <summary>
        /// Overloading Equals.
        /// </summary>
        /// 
        /// <param name="obj">Another population.</param>
        /// <returns> True if current population and object are same, otherwise false. </returns>
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        /// <summary>
        /// Overloading GetHashCode.
        /// </summary>
        /// 
        /// <returns> Generated hash code.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Overloading ToString.
        /// </summary>
        /// 
        /// <returns> Generated string representing population. </returns>
        public override string ToString()
        {
            return base.ToString();
        }

        /// <summary>
        /// Indexer for Population.
        /// </summary>
        /// 
        /// <param name="index"> Variable that represents index of desired chromosome in population. </param>
        /// <returns>Desired chromosome.</returns>
        public Chromosome this[int index]
        {
            get
            {
                if (index < 0 && index >= this._chromosomes.Count)
                    throw new IndexOutOfRangeException("[Population indexer - overloading operator] Index out of range");

                return this._chromosomes[index];
            }
            set
            {
                if (index < 0 && index >= this._chromosomes.Count)
                    throw new IndexOutOfRangeException("[Population indexer - overloading operator] Index out of range");

                this._chromosomes[index] = (Chromosome)value.Clone();
            }
        }

        #endregion

        #region Generate solution with difference control
        /// <summary>
        /// Helper method for controling difference of the solutions while generating them in initialization phase.
        /// </summary>
        /// 
        /// <param name="solutionIndex"> IOndex of the solution in the population that we are trying to generate.</param>
        /// <param name="generatingTreesMethod">Method for generating symbolic tree.</param>
        /// <param name="initialMaxDepth">Initial maximal depth of symbolic tree.</param>
        /// <param name="terminalNames">String representations of terminal names.</param>
        /// <param name="trainData">Train feature values - for calculating fitness.</param>
        /// <param name="testData">Test feature values - for calculating accuracy.</param>
        /// <param name="nonTerminals">String representations of non temrinals <-> mathematical operations.</param>
        /// <param name="mathOperationsArity"> Dictionary with all possible non-terminals and their arity. </param>
        private void GenerateSolutionWithDifferenceControl(int solutionIndex, string generatingTreesMethod, int initialMaxDepth, List<string> terminalNames, DataTable trainData, DataTable testData, List<string> nonTerminals, Dictionary<string, int> mathOperationsArity)
        {
            int counter = 0;

            // While new solution is the same as some of the previous, try making a new one.
            while (true)
            {
                // Variable that makes sure program does not stuck at infinite loop.
                counter++;

                // Generate new solution.
                this._chromosomes[solutionIndex].Generate(generatingTreesMethod, initialMaxDepth, terminalNames, trainData, testData, nonTerminals, mathOperationsArity);

                // Check if new solution is different.
                bool isDifferent = true;
                for (var i = 0; i < this._chromosomes.Count; i++)
                {
                    if (i == solutionIndex)
                        continue;

                    if (this._chromosomes[i].Equals(this._chromosomes[solutionIndex]))
                    {
                        isDifferent = false;
                        break;
                    }
                }

                // End loop if solution is different.
                if (isDifferent)
                    break;

                if (counter == 10)
                {
                    // Throw a warning if the program was not able to generate solution different than others 1000 times.
                    throw new WarningException("[GenerateSolutionWithDifferenceControl] Stuck at infinite loop while trying to generate different solution.");
                }
            } 
        }
        #endregion

        #region Crossover
        /// <summary>
        /// Crossover of the two chromosomes.
        /// </summary>
        /// 
        /// <param name="primaryParentIndex"> (Index of) First - primary chromosome (parent) that is part of the crossover. </param>
        /// <param name="secundaryParentIndex"> (Index of) Secondary chromosome (parent) that is part of the crossover. </param>
        /// <param name="maxDepth">Maximal depth of trees in population.</param>
        /// <param name="trainData">Train feature values - for calculating fitness.</param>
        /// <param name="testData">Test feature values - for calculating accuracy.</param>
        /// <param name="probability">Probability of choosing non-terminal.</param>
        /// <returns> New solution created with crossover. </returns>
        internal Chromosome Crossover(int primaryParentIndex, int secundaryParentIndex, int maxDepth, DataTable trainData, DataTable testData, double probability)
        {
            // Make clones of solutions.
            Chromosome primaryParentClone = (Chromosome)this.chromosomes[primaryParentIndex].Clone();
            Chromosome secundaryParentClone = (Chromosome)this.chromosomes[secundaryParentIndex].Clone();

            // ITEM1 - Index of crossover point in primary parent.
            // ITEM2 - Subtree of crossover point in secundary parent.
            Tuple<int, SymbolicTreeNode> subtrees = ChooseSubtrees(primaryParentClone, secundaryParentClone, probability, maxDepth);

            // Variable for tracking if we found the crossover point in PlaceNodeAtPoint method.
            bool found = false;

            // Place subtree of secondary parent in primary parent crossover point to create a child.
            var ret = PlaceNodeAtPoint((SymbolicTreeNode)primaryParentClone.symbolicTree, subtrees.Item1, subtrees.Item2, ref found);

            #region child setup
            // Solution created with crossover.
            Chromosome child = new Chromosome();

            // Update child tree.
            child.symbolicTree = ret.Item1;
            // Node indices should be re-calculated.
            child.symbolicTree.CalculateIndices();
            // Update number of possible terminals.
            child.numberOfPossibleTerminals = this.chromosomes[primaryParentIndex].numberOfPossibleTerminals;
            // Update depth of child solution.
            child.depth = child.symbolicTree.DepthOfSymbolicTree();
            // Update accuracy (train+test) (fitness+tn/tp/fn/fp)
            child.UpdateAccuracy(trainData, testData);
            #endregion

            return child;
        }

        #region Crossover with difference control
        /// <summary>
        /// Crossover of the two chromosomes with caution.
        /// (New solution must be different from all other solution in the population.)
        /// </summary>
        /// 
        /// <param name="primaryParentIndex"> First - primary chromosome (parent) participating in the crossover. </param>
        /// <param name="secundaryParentIndex"> Secondary chromosome (parent) participating in the crossover. </param>
        /// <param name="maxDepth">Maximal depth of trees in population.</param>
        /// <param name="trainData">Train feature values - for calculating fitness.</param>
        /// <param name="testData">Test feature values - for calculating accuracy.</param>
        /// <param name="probability">Probability of choosing non-terminal.</param>
        /// <returns> New solution created with crossover. </returns>
        public Chromosome CrossoverWithDifferenceControl(int primaryParentIndex, int secundaryParentIndex, int maxDepth, DataTable trainData, DataTable testData, double probability)
        {
            int counter = 0;

            // While new solution is the same as some of the previous, try making a new one.
            while (true)
            {
                // Variable that makes sure program does not stuck at infinite loop.
                counter++;

                // Do crossover.
                Chromosome newSolution = (Chromosome)Crossover(primaryParentIndex, secundaryParentIndex, maxDepth, trainData, testData, probability).Clone();

                // Check if new solution is different.
                bool isDifferent = true;
                for (var s = 0; s < this._populationSize; s++)
                {
                    if (this._chromosomes[s].Equals(newSolution))
                    {
                        isDifferent = false;
                        break;
                    }
                }

                // End loop if solution is different.
                if (isDifferent)
                    return newSolution;

                // Every 10 not-succesful runs try changing second parent.
                if (counter % 10 == 0)
                {
                    while (true)
                    {
                        secundaryParentIndex = rand.Next(this.populationSize);
                        if (secundaryParentIndex != primaryParentIndex)
                            break;
                    }
                }

                // Throw a warning if the program was not able to generate solution different than others 1000 times.
                if (counter == 1000)
                    throw new WarningException("[CrossoverWithDifferenceControl] Stuck at infinite loop while trying to generate different solution.");
            }
        }
        #endregion

        #region Helper methods for crossover
        /// <summary>
        /// Recursive helper method for separating node indices based on terminal/non-terminal property.
        /// </summary>
        /// 
        /// <param name="node">Root or current node.</param>
        /// <param name="nonTerminalIndicesOfPrimaryParent">Reference of a list of non-terminal indices.</param>
        /// <param name="terminalIndicesOfPrimaryParent">Reference of a list of terminal indices.</param>
        private void SeparateIndices(SymbolicTreeNode node, ref List<int> nonTerminalIndicesOfPrimaryParent, ref List<int> terminalIndicesOfPrimaryParent)
        {
            if (node == null)
                return;

            if (node.arity == 0)
            {
                // If terminal node.

                #region exceptions
                // Check that node type is terminal.
                if (node.type != "terminal")
                {
                    throw new Exception("[SeparateIndices] Node has arity = 0 but his type is: " + node.type + " (not terminal).");
                }

                // Check that this node has no child node.
                if (!(node.children == null || node.children.Count == 0))
                {
                    throw new Exception("[SeparateIndices] Node has arity = 0 but has " + node.children.Count + " child nodes.");
                }
                #endregion

                // Everything fine, add node to list of terminal nodes.
                terminalIndicesOfPrimaryParent.Add(node.index);
                return;
            }
            else if (node.arity == 1)
            {
                // If non-terminal node with arity 1.

                #region exceptions
                // Check that node type is non-teminal.
                if (node.type != "non-terminal")
                {
                    throw new Exception("[SeparateIndices] Node has arity = 1 but his type is: " + node.type + " (not non-terminal).");
                }

                // Check that this node has 1 child node.
                if (node.children.Count != 1)
                {
                    throw new Exception("[SeparateIndices] Node has arity = 1 but has " + node.children.Count + " child nodes.");
                }
                #endregion

                // Everything fine, add node to nonTerminal list and call SeparateNodes of his child node.
                nonTerminalIndicesOfPrimaryParent.Add(node.index);
                SeparateIndices(node.children[0], ref nonTerminalIndicesOfPrimaryParent, ref terminalIndicesOfPrimaryParent);
                return;
            }
            else if (node.arity == 2)
            {
                // If non-terminal node with arity 2.

                #region exceptions
                // Check that node type is non-teminal.
                if (node.type != "non-terminal")
                {
                    throw new Exception("[SeparateIndices] Node has arity = 2 but his type is: " + node.type + " (not non-terminal).");
                }

                // Check that this node has 2 child nodes.
                if (node.children.Count != 2)
                {
                    throw new Exception("[SeparateIndices] Node has arity = 2 but has " + node.children.Count + " child nodes.");
                }
                #endregion

                // Everything fine, call SeparateNodes on his left child node, add node to nonTerminal list and call SeparateNodes on his right child node.
                SeparateIndices(node.children[0], ref nonTerminalIndicesOfPrimaryParent, ref terminalIndicesOfPrimaryParent);
                nonTerminalIndicesOfPrimaryParent.Add(node.index);
                SeparateIndices(node.children[1], ref nonTerminalIndicesOfPrimaryParent, ref terminalIndicesOfPrimaryParent);
                return;
            }
            else
            {
                // Node with given arity is not expected. 
                throw new Exception("[SeparateIndices] Given node arity = " + node.arity + " is not expected! Arity higher than 2 is not covered yet!");
            }
        }

        /// <summary>
        /// Recursive helper method for creating preorder list of non-terminal and terminal nodes.
        /// </summary>
        /// 
        /// <param name="node">Root or current node.</param>
        /// <param name="nonTerminalNodesOfSecundaryParent">Reference of a list of non-terminal nodes.</param>
        /// <param name="terminalNodesOfSecundaryParent">Reference of a list of terminal nodes.</param>
        private void SeparateNodes(SymbolicTreeNode node, ref List<SymbolicTreeNode> nonTerminalNodesOfSecundaryParent, ref List<SymbolicTreeNode> terminalNodesOfSecundaryParent)
        {
            if (node == null)
                return;

            if (node.arity == 0)
            {
                // If terminal node.

                #region exceptions
                // Check that node type is terminal.
                if (node.type != "terminal")
                {
                    throw new Exception("[SeparateNodes] Node has arity = 0 but his type is: " + node.type + " (not terminal).");
                }

                // Check that this node has no child node.
                if (!(node.children == null || node.children.Count == 0))
                {
                    throw new Exception("[SeparateNodes] Node has arity = 0 but has " + node.children.Count + " child nodes.");
                }
                #endregion

                // Everything fine, add node to list of terminal nodes.
                terminalNodesOfSecundaryParent.Add(node);
                return;
            }
            else if (node.arity == 1)
            {
                // If non-terminal node with arity 1.

                #region exceptions
                // Check that node type is non-teminal.
                if (node.type != "non-terminal")
                {
                    throw new Exception("[SeparateNodes] Node has arity = 1 but his type is: " + node.type + " (not non-terminal).");
                }

                // Check that this node has 1 child node.
                if (node.children.Count != 1)
                {
                    throw new Exception("[SeparateNodes] Node has arity = 1 but has " + node.children.Count + " child nodes.");
                }
                #endregion

                // Everything fine, add node to nonTerminal list and call SeparateNodes of his child node.
                nonTerminalNodesOfSecundaryParent.Add(node);
                SeparateNodes(node.children[0], ref nonTerminalNodesOfSecundaryParent, ref terminalNodesOfSecundaryParent);
                return;
            }
            else if (node.arity== 2)
            {
                // If non-terminal node with arity 2.

                #region exceptions
                // Check that node type is non-teminal.
                if (node.type != "non-terminal")
                {
                    throw new Exception("[SeparateNodes] Node has arity = 2 but his type is: " + node.type + " (not non-terminal).");
                }

                // Check that this node has 2 child nodes.
                if (node.children.Count != 2)
                {
                    throw new Exception("[SeparateNodes] Node has arity = 2 but has " + node.children.Count + " child nodes.");
                }
                #endregion

                // Everything fine, call SeparateNodes on his left child node, add node to nonTerminal list and call SeparateNodes on his right child node.
                SeparateNodes(node.children[0], ref nonTerminalNodesOfSecundaryParent, ref terminalNodesOfSecundaryParent);
                nonTerminalNodesOfSecundaryParent.Add(node);
                SeparateNodes(node.children[1], ref nonTerminalNodesOfSecundaryParent, ref terminalNodesOfSecundaryParent);
                return;
            }
            else
            {
                // Node with given arity is not expected. 
                throw new Exception("[SeparateNodes] Given node arity = " + node.arity +  " is not expected! Arity higher than 2 is not covered yet!");
            }
        }

        /// <summary>
        /// Recursive helper method for placing given subtree at the specific point (at node with index).
        /// </summary>
        /// 
        /// <param name="node">Chromosome (or its child node) to be changed.</param>
        /// <param name="indexOfCrossoverPointOfPrimaryParent">Index of changing point.</param>
        /// <param name="crossoverSubtreeOfSecundaryParent">Subtree to be added at changing point.</param>
        /// <param name="found">Reference to a variable that represents if node with given index is found. It is used for better preformance.</param>
        /// /// <returns> New, or old, (sub)tree. </returns>
        public Tuple<SymbolicTreeNode, bool> PlaceNodeAtPoint(SymbolicTreeNode node, int indexOfCrossoverPointOfPrimaryParent, SymbolicTreeNode crossoverSubtreeOfSecundaryParent, ref bool found)
        {
            // Node is previously found - return tuple of current node and found variable.
            if (found)
                return new Tuple<SymbolicTreeNode, bool>(node, found);

            // If node is null call to the method should not be preformed.
            if (node == null)
                throw new Exception("[PlaceNodeAtPoint] Node is null. Not possible since all cases should be already covered.");


            if (node.arity == 0)
            {
                // If terminal node.

                #region exceptions
                // Check that node type is terminal.
                if (node.type != "terminal")
                {
                    throw new Exception("[PlaceNodeAtPoint] Node has arity = 0 but his type is: " + node.type + " (not terminal).");
                }

                // Check that this node has no child node.
                if (!(node.children == null || node.children.Count == 0))
                {
                    throw new Exception("[PlaceNodeAtPoint] Node has arity = 0 but has " + node.children.Count + " child nodes.");
                }
                #endregion

                #region check if current node is the one (return if true)
                // Everything fine, check if we found right node.
                if (indexOfCrossoverPointOfPrimaryParent == node.index)
                {
                    // Save current depth.
                    int currentDepth = node.depth;
                    // Update node.
                    node = (SymbolicTreeNode)crossoverSubtreeOfSecundaryParent.Clone();
                    // Update variable found.
                    found = true;
                    // Update depths of new subtree.
                    node.CalculateDepths(currentDepth);

                    return new Tuple<SymbolicTreeNode, bool>(node, found);
                }
                #endregion

                // Node was not found.
                return new Tuple<SymbolicTreeNode, bool>(node, found);
            }
            else if (node.arity == 1)
            {
                // If non-terminal node with arity 1.

                #region exceptions
                // Check that node type is non-teminal.
                if (node.type != "non-terminal")
                {
                    throw new Exception("[PlaceNodeAtPoint] Node has arity = 1 but his type is: " + node.type + " (not non-terminal).");
                }

                // Check that this node has 1 child node.
                if (node.children.Count != 1)
                {
                    throw new Exception("[PlaceNodeAtPoint] Node has arity = 1 but has " + node.children.Count + " child nodes.");
                }
                #endregion

                #region check if current node is the one (return if true)
                // Everything fine, check if we found right node.
                if (indexOfCrossoverPointOfPrimaryParent == node.index)
                {
                    // Save current depth.
                    int currentDepth = node.depth;
                    // Update node.
                    node = (SymbolicTreeNode)crossoverSubtreeOfSecundaryParent.Clone();
                    // Update variable found.
                    found = true;
                    // Update depths of new subtree.
                    node.CalculateDepths(currentDepth);

                    return new Tuple<SymbolicTreeNode, bool>(node, found);
                }
                #endregion

                #region check child (return if found)
                // Check subtree of current node since node is not yet found.
                var retVal = PlaceNodeAtPoint((SymbolicTreeNode)node.children[0].Clone(), indexOfCrossoverPointOfPrimaryParent, crossoverSubtreeOfSecundaryParent, ref found);
                // Update variable found.
                found = retVal.Item2;

                // Check if node is now found.
                if (found)
                {
                    // Update node.
                    node.children[0] = (SymbolicTreeNode)retVal.Item1.Clone();
                }
                #endregion

                // Node was not found.
                return new Tuple<SymbolicTreeNode, bool>(node, found);
            }
            else if (node.arity == 2)
            {
                // If non-terminal node with arity 2.

                #region exceptions 
                // Check that node type is non-teminal.
                if (node.type != "non-terminal")
                {
                    throw new Exception("[PlaceNodeAtPoint] Node has arity = 2 but his type is: " + node.type + " (not non-terminal).");
                }

                // Check that this node has 2 child nodes.
                if (node.children.Count != 2)
                {
                    throw new Exception("[PlaceNodeAtPoint] Node has arity = 2 but has " + node.children.Count + " child nodes.");
                }
                #endregion

                #region check if current node is the one (return if true)
                // Everything fine, check if we found right node.
                if (indexOfCrossoverPointOfPrimaryParent == node.index)
                {
                    // Save current depth.
                    int currentDepth = node.depth;
                    // Update node.
                    node = (SymbolicTreeNode)crossoverSubtreeOfSecundaryParent.Clone();
                    // Update variable found.
                    found = true;
                    // Update depths of new subtree.
                    node.CalculateDepths(currentDepth);

                    return new Tuple<SymbolicTreeNode, bool>(node, found);
                }
                #endregion

                #region check left child (return if found)
                // Check left subtree of current node.
                Tuple<SymbolicTreeNode, bool> retValLeft = PlaceNodeAtPoint((SymbolicTreeNode)node.children[0].Clone(), indexOfCrossoverPointOfPrimaryParent, crossoverSubtreeOfSecundaryParent, ref found);

                // Update variable found.
                found = retValLeft.Item2;

                // If node now found.
                if (found)
                {
                    // Update node left child. Right child stays the same.
                    node.children[0] = (SymbolicTreeNode)retValLeft.Item1.Clone();

                    return new Tuple<SymbolicTreeNode, bool>(node, found);
                }
                #endregion

                #region check right child (return if found)
                // Node still not found.
                // Check right subtree of current node.
                Tuple<SymbolicTreeNode, bool> retValRight = PlaceNodeAtPoint((SymbolicTreeNode)node.children[1].Clone(), indexOfCrossoverPointOfPrimaryParent, crossoverSubtreeOfSecundaryParent, ref found);

                // Update variable found.
                found = retValRight.Item2;

                // If node now found.
                if (found)
                {
                    // Update node left child. Right child stays the same.
                    node.children[1] = (SymbolicTreeNode)retValRight.Item1.Clone();
                }
                #endregion

                // Node was not found.
                return new Tuple<SymbolicTreeNode, bool>(node, found);
            }
            else
            {
                // Node with given arity is not expected. 
                throw new Exception("[PlaceNodeAtPoint] Given node arity = " + node.arity + " is not expected! Arity higher than 2 is not covered yet!");
            }

        }
        
        public Tuple<int, SymbolicTreeNode> ChooseSubtrees(Chromosome primaryParent, Chromosome secundaryParent, double chooseNonTerminalPointProbability, int maxDepth)
        {
            // Index of crossover point in primary parent.
            int indexOfCrossoverPointOfPrimaryParent;
            // Subtree of crossover point in secundary parent.
            SymbolicTreeNode crossoverSubtreeOfSecundaryParent = new SymbolicTreeNode();

            // Search for crossover points in parents until they that satisfy condition on depth of child.
            while (true)
            {
                #region primary parent crossover point selection
                // Make list of nonTerminal and terminal indices in primaryParent.
                List<int> nonTerminalIndicesOfPrimaryParent = new List<int>();
                List<int> terminalIndicesOfPrimaryParent = new List<int>();
                SeparateIndices(primaryParent.symbolicTree, ref nonTerminalIndicesOfPrimaryParent, ref terminalIndicesOfPrimaryParent);

                // Randomly select non-terminal or terminal (probability of selecting non-terminal is predefined) from secundary parent.
                if ((rand.Next(100) < (chooseNonTerminalPointProbability * 100)) && (nonTerminalIndicesOfPrimaryParent.Count > 1))
                {
                    // Index of the (to-be) selected non-terminal.
                    int crossoverPointOfPrimaryParent;

                    // Do not consider crossover point at root node (index = 0) in first parent since in that way we would be cloning second parent into a child.
                    while (true)
                    {
                        // Chose index of a non-terminal node from secundary parent.
                        crossoverPointOfPrimaryParent = rand.Next(nonTerminalIndicesOfPrimaryParent.Count);
                        if (crossoverPointOfPrimaryParent != 0)
                            break;
                    }

                    // Clone selected subtree.
                    indexOfCrossoverPointOfPrimaryParent = nonTerminalIndicesOfPrimaryParent[crossoverPointOfPrimaryParent];
                }
                else
                {
                    // Chose index of a non-terminal node from secundary parent.
                    int crossoverPointOfPrimaryParent = rand.Next(terminalIndicesOfPrimaryParent.Count);

                    // Clone selected subtree.
                    indexOfCrossoverPointOfPrimaryParent = terminalIndicesOfPrimaryParent[crossoverPointOfPrimaryParent];
                }
                #endregion

                #region secundary parent crossover point selection
                // Make list of nonTerminal and terminal nodes in secundaryParent.
                List<SymbolicTreeNode> nonTerminalNodesOfSecundaryParent = new List<SymbolicTreeNode>();
                List<SymbolicTreeNode> terminalNodesOfSecundaryParent = new List<SymbolicTreeNode>();
                SeparateNodes(secundaryParent.symbolicTree, ref nonTerminalNodesOfSecundaryParent, ref terminalNodesOfSecundaryParent);

                // Randomly select non-terminal or terminal (probability of selecting non-terminal is predefined) from secundary parent.
                if ((rand.Next(100) < (chooseNonTerminalPointProbability * 100)) && (nonTerminalNodesOfSecundaryParent.Count != 0))
                {
                    // Chose index of a non-terminal node from secundary parent.
                    int crossoverPointOfSecundaryParent = rand.Next(nonTerminalNodesOfSecundaryParent.Count);

                    // Clone selected subtree.
                    crossoverSubtreeOfSecundaryParent = (SymbolicTreeNode)nonTerminalNodesOfSecundaryParent[crossoverPointOfSecundaryParent].Clone();
                }
                else
                {
                    // Chose index of a non-terminal node from secundary parent.
                    int crossoverPointOfSecundaryParent = rand.Next(terminalNodesOfSecundaryParent.Count);

                    // Clone selected subtree.
                    crossoverSubtreeOfSecundaryParent = (SymbolicTreeNode)terminalNodesOfSecundaryParent[crossoverPointOfSecundaryParent].Clone();
                }
                #endregion

                int depth1 = primaryParent.symbolicTree.FindNodeWithIndex(indexOfCrossoverPointOfPrimaryParent).depth;
                int depth2 = crossoverSubtreeOfSecundaryParent.DepthOfSymbolicTree();
                if (depth1 + depth2 < maxDepth)
                    break;
            }

            return Tuple.Create(indexOfCrossoverPointOfPrimaryParent, crossoverSubtreeOfSecundaryParent);
        }
        #endregion

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
        /// 
        /// <param name="bestSolutionIndex"> Index of the best solution in the population. </param>
        /// <param name="alpha"> Parametar alpha. </param>
        public void CalculateProbabilities(int bestSolutionIndex, double alpha)
        {
            // Array for function fit ( fit(Solution_i) = (1 + fitness(Solution_i))/2 [cannot be zero!] ).
            double[] fit = new double[this._populationSize];

            // First, calculate fit for the best solution (quality).
            fit[bestSolutionIndex] = (1 + this._chromosomes[bestSolutionIndex].trainFitness) / 2;

            // Calculate probabilities for all solutions.
            for (var i = 0; i < this._populationSize; i++)
            {
                fit[i] = (double)(1 + this._chromosomes[i].trainFitness) / (double)2;
                this._probabilities[i] = (double)(1 - alpha) + (double)( (alpha * fit[i]) / fit[bestSolutionIndex] );
            }
        }
        #endregion

        #endregion
    }
    #endregion
}
