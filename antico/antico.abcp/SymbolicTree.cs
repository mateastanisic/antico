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
using System.Threading;

namespace antico.abcp
{
    #region SymbolicTreeNode
    /// <summary>
    /// Class for symbolic tree (model / chromosome) representation. 
    /// Model is a chromosome in Artificial Bee Colonny Programming 
    /// portrayed by symbolic tree whose inner nodes are terminals (mathematical operations) 
    /// and leaf nodes are features of the model.
    /// This class represents recurisve tree.
    /// </summary>
    public class SymbolicTreeNode : IEquatable<SymbolicTreeNode>
    {
        #region ATTRIBUTES

        #region node content
        // Variable that represents current node content.
        private string _content;

        // Property for the content variable.
        public string content
        {
            get { return _content; }
            set { _content = value; }
        }
        #endregion

        #region child nodes
        // Array variable that contains all child nodes of current node.
        private SymbolicTreeNode[] _children;

        // Property for the content variable.
        public SymbolicTreeNode[] children
        {
            get { return this._children; }
            set 
            {
                // Allocate resources.
                this._children = new SymbolicTreeNode[value.Length];

                // Deep copy.
                for (var i = 0; i < value.Length; i++)
                {
                    this._children[i].Clone(value[i]);
                }
            }
        }
        #endregion

        #region node depth
        // Variable that represents level of the node that it has in the tree
        private int _depth;

        // Property for the content variable.
        public int depth
        {
            get { return _depth; }
            set { _depth = value; }
        }
        #endregion

        #region helper variables for handling with iteration throught tree structure
        //Helper variable for handling with iteration throught tree structure
        private static ThreadLocal<Stack<SymbolicTreeNode>> _treeStack = new ThreadLocal<Stack<SymbolicTreeNode>>(() =>
        {
            return new Stack<SymbolicTreeNode>(50);
        });

        //Helper variable for handling with iteration throught tree structure
        private static ThreadLocal<Queue<SymbolicTreeNode>> _treeQueue = new ThreadLocal<Queue<SymbolicTreeNode>>(() =>
        {
            return new Queue<SymbolicTreeNode>(50);
        });

        //Helper variable for handling with iteration throught tree structure
        private static ThreadLocal<Queue<SymbolicTreeNode>> _treeQueueClone = new ThreadLocal<Queue<SymbolicTreeNode>>(() =>
        {
            return new Queue<SymbolicTreeNode>(50);
        });
        #endregion

        #endregion

        #region OPERATIONS

        #region Make new node
        /// <summary>
        /// Constructor.
        /// Method for cretaing a new node.
        /// </summary>
        /// <returns> Created new SymbolicTreeNode. </returns>
        public static SymbolicTreeNode NewNode()
        {
            // Make new node.
            var node = new SymbolicTreeNode();

            // Initiate values of new node.
            node.content = null;
            node.depth = -1;

            // Variable children is already empty array.

            // Return new node
            return node;
        }
        #endregion

        #region Deep copy
        /// <summary>
        /// Helper method for making deep copy of the Tree whose root node is current node.
        /// </summary>
        /// <returns> Cloned Tree from current Tree.</returns>
        public SymbolicTreeNode Clone()
        {
            // Make new node.
            var clonedNewNode = SymbolicTreeNode.NewNode();

            // Make sure variables _treeQueue and _treeQueueClone are empty.
            _treeQueue.Value.Clear();
            _treeQueueClone.Value.Clear();

            // Queues made of Tree Nodes.
            Queue<SymbolicTreeNode> dataTree = _treeQueue.Value;
            Queue<SymbolicTreeNode> cloneTree = _treeQueueClone.Value;

            // Variables that will represent a current node in (tree/cloned tree) the loop 
            // through all child nodes of the current node/cloned new node.
            SymbolicTreeNode node = null;
            SymbolicTreeNode clone = null;

            // Add root node at queue.
            dataTree.Enqueue(this);

            // Add new cloned root node at clone queue.
            cloneTree.Enqueue(clonedNewNode);

            // Loop through all nodes in dataTree.
            while (dataTree.Count > 0)
            {
                // Get next nodes from the beginning of the queue.
                node = dataTree.Dequeue();
                clone = cloneTree.Dequeue();

                // Copy values from the node to the clone node.
                clone.content = node.content;
                clone.depth = node.depth;

                // If the node has child nodes, add children nodes from the node to the dataTree 
                // and for every child make new clone node and add it to the cloneTree.
                if (node.children != null)
                {
                    // Cloned node will have same child nodes as the node.
                    clone.children = new SymbolicTreeNode[node.children.Length];

                    // Make clone child nodes and add children to the dataTree, cloneTree.
                    for (int i = 0; i < node.children.Length; i++)
                    {
                        clone.children[i] = SymbolicTreeNode.NewNode();
                        dataTree.Enqueue(node.children[i]);
                        cloneTree.Enqueue(clone.children[i]);
                    }
                }
            }
            return clonedNewNode;
        }

        /// <summary>
        /// Helper method for making deep copy of the tree sent by parameter in function onto current tree.
        /// </summary>
        /// <param name="stn"> Tree with root node stn will be cloned onto current tree. </param>
        public void Clone(SymbolicTreeNode stn)
        {
            // Make sure variables _treeQueue and _treeQueueClone are empty.
            _treeQueue.Value.Clear();
            _treeQueueClone.Value.Clear();

            // Queues made of Tree Nodes.
            Queue<SymbolicTreeNode> dataTree = _treeQueue.Value;
            Queue<SymbolicTreeNode> cloneTree = _treeQueueClone.Value;

            // Variables that will represent a current node in (tree/copy tree) the loop 
            // through all child nodes of the current copied node/ new node.
            SymbolicTreeNode clone = null;
            SymbolicTreeNode node = null;

            // Add root node at queue.
            dataTree.Enqueue(this);

            // Add new cloned root node at clone queue.
            cloneTree.Enqueue(stn);

            // Loop through all nodes in dataTree.
            while (cloneTree.Count > 0)
            {
                // Get next nodes from the beginning of the queue.
                node = dataTree.Dequeue();
                clone = cloneTree.Dequeue();

                // Copy values from the node to the clone node.
                node.content = clone.content;
                node.depth = clone.depth;

                // In case copy node has no children.
                node.children = null;

                // If the copy node has child nodes, add children nodes from the copy node to the cloneTree 
                // and for every child make new node and add it to the dataTree.
                if (clone.children != null)
                {
                    // New node will have same child nodes as the copy node.
                    node.children = new SymbolicTreeNode[clone.children.Length];

                    // Make new child nodes and add children to the dataTree, cloneTree.
                    for (var i = 0; i < clone.children.Length; i++)
                    {
                        node.children[i] = SymbolicTreeNode.NewNode();
                        dataTree.Enqueue(node.children[i]);
                        cloneTree.Enqueue(clone.children[i]);
                    }
                }
            }
        }
        #endregion

        #region Tree ToString / ToStringBuilder
        /// <summary>
        /// Helper override method for converting SymbolicTree whose root node is current node into a String.
        /// </summary>
        /// <returns> String variable that represents subtree whose root is current node. </returns>
        public override string ToString()
        {
            return this.ToStringBuilder().ToString();
        }

        /// <summary>
        /// Helper method for converting SymbolicTree whose root node is current node into a String.
        /// </summary>
        /// <returns> StringBuilder variable that represents subtree whose root is current node. </returns>
        public StringBuilder ToStringBuilder()
        {
            // Initialize string builder.
            StringBuilder treeString = new StringBuilder();

            // Stack made of Tree Nodes.
            _treeStack.Value.Clear();
            Stack<SymbolicTreeNode> dataTree = _treeStack.Value;

            // Variable that will represent a current node in the loop through all child nodes of the current node.
            SymbolicTreeNode node = null;

            // Add root node.
            dataTree.Push(this);

            // Loop through all nodes in dataTree.
            while (dataTree.Count > 0)
            {
                // Get next node.
                node = dataTree.Pop();

                // If value of next node is null continue. 
                if ( node.content == null )
                    continue;

                // Add value to string.
                treeString.Append(node.content.ToString());

                // Add all children of the current node into a stack made of tree nodes.
                if (node.children != null)
                {
                    for (int i = node.children.Length - 1; i >= 0; i--)
                    {
                        dataTree.Push(node.children[i]);
                    }  
                }
            }
            return treeString;

        }
        #endregion

        #region Tree ToList
        /// <summary>
        /// Helper method for converting SymbolicTree whose root node is current node into a List.
        /// </summary>
        /// <returns> List whose values are values from a SymbolicTree whose root is the current node.</returns>
        public List<string> ToList()
        {
            // Initialize list of nodes.
            List<string> treeList = new List<string>(30);

            // Stack made of Tree Nodes.
            Stack<SymbolicTreeNode> dataTree = new Stack<SymbolicTreeNode>();

            // Variable that will represent a current node in the loop through all child nodes of the current node.
            SymbolicTreeNode node = null;

            // Add root node.
            dataTree.Push(this);

            // Loop through all nodes in dataTree.
            while (dataTree.Count > 0)
            {
                // Get next node.
                node = dataTree.Pop();

                // If value of next node is null continue. 
                if (node.content == null)
                    continue;

                // Add value to string.
                treeList.Add(node.content);

                // Add all children of the current node into a stack made of tree nodes.
                if (node.children != null)
                {
                    for (int i = node.children.Length - 1; i >= 0; i--)
                    {
                        dataTree.Push(node.children[i]);
                    }  
                }
            }
            return treeList;

        }

        /// <summary>
        /// Helper method for converting SymbolicTree whose root node is current node into 
        /// a already created List sent through the parameter. Sent list is not empty.
        /// </summary>
        /// <returns> Nothing. </returns>
        public void ToList(List<string> treeList)
        {
            // Make sure treeList is not a empty list.
            if (treeList == null)
            {
                throw new System.ArgumentException("Parameter cannot be empty", "treeList");
            }

            // Make sure variable _treeStack is empty.
            _treeStack.Value.Clear();

            // Stack made of Tree Nodes.
            Stack<SymbolicTreeNode> dataTree = _treeStack.Value;

            // Variable that will represent a current node in the loop through all child nodes of the current node.
            SymbolicTreeNode node = null;

            // Variable for indexing list.
            int index = 0;

            // Add root node.
            dataTree.Push(this);

            // Loop through all nodes in dataTree.
            while (dataTree.Count > 0)
            {
                // Get next node.
                node = dataTree.Pop();

                // If value of next node is null continue.
                if (node.content == null)
                    continue;

                // Add value to list depending on the size of list.
                if (treeList.Count < index + 1)
                {
                    // No space left on the list. Add dynamicly new node value into a list.
                    treeList.Add(node.content);
                }
                else
                {
                    // There is still space for new node value in the list.
                    treeList[index] = node.content;
                }

                // Increase index since we added another node in the list.
                index++;

                // Add all children of the current node into a stack made of tree nodes.
                if (node.children != null)
                {
                    for (int i = node.children.Length - 1; i >= 0; i--)
                    {
                        dataTree.Push(node.children[i]);
                    }
                }
            }
        }
        #endregion

        #region Number of nodes in tree with current node as root node
        /// <summary>
        /// Helper method that counts number of nodes in the subtree whose root node is current node.
        /// </summary>
        /// <returns> Number of nodes in the tree. </returns>
        public int NumberOfNodes()
        {
            // There is no nodes in the beggining.
            int noOfNodes = 0;

            // Make sure variable _treeStack is empty.
            _treeStack.Value.Clear();

            // Stack made of Tree Nodes.
            Stack<SymbolicTreeNode> dataTree = _treeStack.Value;

            // Variable that will represent a current node in the loop through all child nodes of the current node.
            SymbolicTreeNode node = null;

            // Add root node.
            dataTree.Push(this);

            // Loop through all nodes in dataTree.
            while (dataTree.Count > 0)
            {
                // Get next node.
                node = dataTree.Pop();

                // If value of next node is null continue. 
                if (node.content == null)
                    continue;

                // Increase number of nodes since we found one more.
                noOfNodes++;

                // Add all children of the current node into a stack made of tree nodes.
                if (node.children != null)
                {
                    for (int i = node.children.Length - 1; i >= 0; i--)
                    {
                        dataTree.Push(node.children[i]);
                    }
                }
            }
            return noOfNodes;
        }
        #endregion

        #region Node with index
        /// <summary>
        /// Helper method that returns node with wanted index from the subtree whose root 
        /// is the current node, based on Depth-First search method.
        /// </summary>
        /// <param name="index"></param>
        /// <returns> Node with wanted index in a Subtree whose root is the current node. </returns>
        public SymbolicTreeNode NodeAt(int index)
        {
            // Make sure index is positive number.
            if( index < 0 )
            {
                throw new IndexOutOfRangeException("Index out of range");
            }

            // There is no nodes in the beginning.
            int count = 0;

            // Make sure variable _treeQueue is empty.
            _treeQueue.Value.Clear();

            // Queue made of Tree Nodes.
            Queue<SymbolicTreeNode> dataTree = _treeQueue.Value;

            // Variable that will represent a current node in the loop through all child nodes of the current node.
            SymbolicTreeNode node = null;

            // Add root node at queue.
            dataTree.Enqueue(this);

            // Loop through all nodes in dataTree.
            while (dataTree.Count > 0)
            {
                // Get next node from the beginning of the queue.
                node = dataTree.Dequeue();

                // If value of next node is null continue. 
                if (node.content == null)
                    continue;

                // Increase counter since we found another node.
                count++;

                // If current node has the index we are looking for, return current node.
                if (count == index)
                {
                    return node;
                }

                // Add all children of the current node into a stack made of tree nodes.
                if (node.children != null)
                {
                    for (int i = 0; i < node.children.Length; i++)
                    {
                        dataTree.Enqueue(node.children[i]);
                    }
                }
            }

            // In case we came to the end and still haven't found node with wanted index, return exception.
            throw new IndexOutOfRangeException("Index out of tree length range");
        }
        #endregion

        #region Level of node with index
        /// <summary>
        /// Helper method that returns level of a node with a given index.
        /// </summary>
        /// <param name="index">Index of the node.</param>
        /// <returns>Level of the node in a Tree.</returns>
        public int LevelOfNodeWithIndex(int index)
        {
            // Make sure index is positive number.
            if ( index < 0 )
            {
                throw new IndexOutOfRangeException("Index out of range");
            }

            // Get node with helper method NodeAt.
            SymbolicTreeNode nodeWithGivenIndex = NodeAt(index);

            // Return the level of the node.
            return nodeWithGivenIndex.depth;
        }
        #endregion

        #region Overloading operators
        /// <summary>
        /// Overloading operator ==.
        /// </summary>
        /// <param name="obj1">First node.</param>
        /// <param name="obj2">Second node.</param>
        /// <returns>True if nodes are equal, otherwise false. </returns>
        public static bool operator ==(SymbolicTreeNode obj1, SymbolicTreeNode obj2)
        {
            if (ReferenceEquals(obj1, obj2))
            {
                return true;
            }

            if (ReferenceEquals(obj1, null))
            {
                return false;
            }

            if (ReferenceEquals(obj2, null))
            {
                return false;
            }

            return (obj1.content == obj2.content
                    && obj1.depth == obj2.depth
                    && obj1.children == obj2.children);
        }

        /// <summary>
        /// Overloading operator !=.
        /// </summary>
        /// <param name="obj1">First node.</param>
        /// <param name="obj2">Second node.</param>
        /// <returns>False if nodes are equal, otherwise true. </returns>
        public static bool operator !=(SymbolicTreeNode obj1, SymbolicTreeNode obj2)
        {
            return !(obj1 == obj2);
        }

        /// <summary>
        /// Overloading Equals.
        /// </summary>
        /// <param name="other">Node to compare current node.</param>
        /// <returns>True if nodes are equal, otherwise false. </returns>
        public bool Equals(SymbolicTreeNode other)
        {
            return other != null 
                    && _content == other.content 
                    && EqualityComparer<SymbolicTreeNode[]>.Default.Equals(_children, other.children) 
                    && _depth == other.depth;
        }

        /// <summary>
        /// Overloading Equals using object.
        /// </summary>
        /// <param name="obj">Node to compare current node.</param>
        /// <returns>True if nodes are equal, otherwise false.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == this.GetType() && Equals((SymbolicTreeNode)obj);
        }


        /// <summary>
        /// Generated GetHashCode() method.
        /// </summary>
        /// <returns>Generated hash code.</returns>
        public override int GetHashCode()
        {
            int hashCode = -1605392116;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(_content);
            hashCode = hashCode * -1521134295 + EqualityComparer<SymbolicTreeNode[]>.Default.GetHashCode(_children);
            hashCode = hashCode * -1521134295 + _depth.GetHashCode();
            return hashCode;
        }


        #endregion

        #endregion
    }
    #endregion

    #region Chromosome
    /// <summary>
    /// Class for chromosome representation in Artificial Bee Colonny Programming.
    /// Every chromosome has defined its fitness value which represents how good it is.
    /// - meaning, how good that model for classification sooftware into malicious and not-malicious.
    /// Every chromosome is represented by symbolic tree class. Also, depth of the symbolic tree is 
    /// defined for every instance of chromosome. 
    /// Finally, all instances of this class have defined numberOfPossibleTerminals which represents
    /// number of possible features that can be part of the model.
    /// </summary>
    public class SymbolicTree : IEquatable<SymbolicTree>
    {
        #region ATTRIBUTES

        #region fitness
        // Variable that represents fitness of the model.
        private double _fitness;

        // Property for the fitness variable.
        public double fitness
        {
            get { return _fitness; }
            set { _fitness = value; }
        }
        #endregion

        #region (root node of) symbolic tree
        // Variable that represents root of the Symbolic Tree that represents the model.
        private SymbolicTreeNode _symbolicTree;

        // Property for the model variable.
        public SymbolicTreeNode symbolicTree
        {
            get { return this._symbolicTree; }
            set 
            {
                // Deep copy of value.
                this._symbolicTree.Clone(value);
            }
        }
        #endregion

        #region depth
        // Variable that represents depth of the symbolic tree - model.
        private int _depth;

        // Property for the fitness variable.
        public int depth
        {
            get { return _depth; }
            set { _depth = value; }
        }
        #endregion depth

        #region number of possible terminals <-> number of possible features
        // Variable that represents number of features in model.
        private int _numberOfPossibleTerminals;

        // Property for the number_of_features variable.
        public int numberOfPossibleTerminals
        {
            get { return _numberOfPossibleTerminals; }
            set { _numberOfPossibleTerminals = value; }
        }
        #endregion

        #endregion

        #region OPERATIONS 

        #region Deep Copy
        /// <summary>
        /// Helper method for deep copying values of model st to values of this model.
        /// </summary>
        /// <param name="st">Model whose values will be deep copied to this model.</param>
        public void Clone( SymbolicTree st )
        {
            // Copy fitness from model st to this model.
            this._fitness = st.fitness;

            // Copy numberOfPossibleTerminals from model st to this model.
            this._numberOfPossibleTerminals = st.numberOfPossibleTerminals;

            // Copy depth of tree from model st to this model.
            this._depth = st.depth;

            // Deep copy with variable property set.
            this.symbolicTree = st.symbolicTree;
        }
        #endregion

        #region Overloading operators
        /// <summary>
        /// Overloading operator ==.
        /// </summary>
        /// <param name="obj1">First chromosome.</param>
        /// <param name="obj2">Second chromosome.</param>
        /// <returns>True if chromosomes are equal, otherwise false. </returns>
        public static bool operator ==(SymbolicTree obj1, SymbolicTree obj2)
        {
            if (ReferenceEquals(obj1, obj2))
            {
                return true;
            }

            if (ReferenceEquals(obj1, null))
            {
                return false;
            }

            if (ReferenceEquals(obj2, null))
            {
                return false;
            }

            return (obj1.fitness == obj2.fitness
                    && obj1.depth == obj2.depth
                    && obj1.numberOfPossibleTerminals == obj2.numberOfPossibleTerminals
                    && obj1.symbolicTree == obj2.symbolicTree);
        }

        /// <summary>
        /// Overloading operator !=.
        /// </summary>
        /// <param name="obj1">First chromosome.</param>
        /// <param name="obj2">Second chromosome.</param>
        /// <returns>False if chromosomes are equal, otherwise true. </returns>
        public static bool operator !=(SymbolicTree obj1, SymbolicTree obj2)
        {
            return !(obj1 == obj2);
        }

        /// <summary>
        /// Overloading Equals.
        /// </summary>
        /// <param name="other">Chromosome to compare current chromosome.</param>
        /// <returns>True if chromosomes are equal, otherwise false. </returns>
        public bool Equals(SymbolicTree other)
        {
            return other != null 
                    && _fitness == other._fitness 
                    && EqualityComparer<SymbolicTreeNode>.Default.Equals(_symbolicTree, other._symbolicTree) 
                    && _depth == other._depth 
                    && _numberOfPossibleTerminals == other._numberOfPossibleTerminals;
        }

        /// <summary>
        /// Overloading Equals using object.
        /// </summary>
        /// <param name="obj">Chromosome to compare current chromosome.</param>
        /// <returns>True if chromosomes are equal, otherwise false.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == this.GetType() && Equals((SymbolicTree)obj);
        }

        /// <summary>
        /// Generated GetHashCode method.
        /// </summary>
        /// <returns> Generated hash code. </returns>
        public override int GetHashCode()
        {
            int hashCode = 472542077;
            hashCode = hashCode * -1521134295 + _fitness.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<SymbolicTreeNode>.Default.GetHashCode(_symbolicTree);
            hashCode = hashCode * -1521134295 + _depth.GetHashCode();
            hashCode = hashCode * -1521134295 + _numberOfPossibleTerminals.GetHashCode();
            return hashCode;
        }

        #endregion

        #region Fitness
        /// <summary>
        /// Method for calculating fitness of current model.
        /// </summary>
        /// <returns>Fintess of the model.</returns>
        public double calculateFitness()
        {
            // TODO
            throw new NotImplementedException();
        }
        #endregion

        #region Generate initial 
        /// <summary>
        /// Generate new symbolic tree of maximal depth maxDepth whose inner node contents 
        /// are strings from nonTerminals, and leafs have content from terminals.
        /// </summary>
        /// 
        /// <param name="method"> 
        /// Methode for generating initial symbolic tree. Options are:
        /// *full method where chromosome has maximal possible size - the distance from the root node to each leaf is equal to the maximum tree depth;
        /// *grow method where chromosome has random size, but not greater than maxDepth;
        /// </param>
        /// <param name="maxDepth"> Maximal depth of (to-be generated) symbolic tree. </param>
        /// <param name="terminals"> Array of possible terminals of to-be generated symbolic tree.</param>
        /// <param name="nonTerminals"> Array of possible terminals of to-be generated symbolic tree. </param>
        /// <returns></returns>
        public SymbolicTree Generate(string method, int maxDepth, string[] terminals, string[] nonTerminals)
        {
            SymbolicTree chromosome = new SymbolicTree();

            switch (method)
            {
                case "full":
                    {
                        // TODO
                        throw new NotImplementedException();
                        break;
                    }
                case "grow":
                    {
                        // TODO
                        throw new NotImplementedException();
                        break;
                    }
                default:
                    throw new System.ArgumentException("Method is not supported", "method");
            }

            return chromosome;
        }
        #endregion

        #endregion
    }
    #endregion
}
