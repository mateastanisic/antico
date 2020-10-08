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
using System.Data;
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

        #region random
        // Variable for generating random numbers.
        private static Random rand;
        #endregion

        #region node description

        #region node type (terminal/non-terminal)
        // Variable that represents the type of the node -> terminal or non-terminal.
        private string _type;

        // Property for the _type variable.
        public string type
        {
            get { return _type; }
            set { _type = value; }
        }
        #endregion

        #region node content
        // Variable that represents current node content.
        private string _content;

        // Property for the _content variable.
        public string content
        {
            get { return _content; }
            set { _content = value; }
        }
        #endregion

        #region node index
        // Variable that represents index of the node in the symbolic tree.
        private int _index;

        // Property for the _index variable.
        public int index
        {
            get { return _index; }
            set { _index = value; }
        }
        #endregion

        #region node depth
        // Variable that represents depth of the node that it has in the tree.
        private int _depth;

        // Property for the _depth variable.
        public int depth
        {
            get { return _depth; }
            set { _depth = value; }
        }
        #endregion

        #region node arity
        // Variable that represents arity of non-terminal. 
        // If node is terminal, arity is 0. If node arity is not defined, arity is -1.
        private int _arity;

        // Property for the _arity variable.
        public int arity
        {
            get { return _arity; }
            set { _arity = value; }
        }
        #endregion

        #endregion

        #region child nodes
        // Array variable that contains all child nodes of current node.
        private List<SymbolicTreeNode> _children;

        // Property for the content variable.
        public List<SymbolicTreeNode> children
        {
            get { return this._children; }
            set 
            {
                // Allocate resources.
                this._children = new List<SymbolicTreeNode>();

                // Deep copy.
                for (var i = 0; i < value.Count; i++)
                {
                    this._children.Add(new SymbolicTreeNode());
                    this._children[i] = (SymbolicTreeNode)value[i].Clone();
                }
            }
        }
        #endregion

        #region helper variables for handling with iteration throught tree structure
        //Helper variable for handling with iteration throught tree structure.
        private static ThreadLocal<Stack<SymbolicTreeNode>> _treeStack = new ThreadLocal<Stack<SymbolicTreeNode>>(() =>
        {
            return new Stack<SymbolicTreeNode>(50);
        });

        //Helper variable for handling with iteration throught tree structure.
        private static ThreadLocal<Queue<SymbolicTreeNode>> _treeQueue = new ThreadLocal<Queue<SymbolicTreeNode>>(() =>
        {
            return new Queue<SymbolicTreeNode>(50);
        });

        //Helper variable for handling with iteration throught tree structure.
        private static ThreadLocal<Queue<SymbolicTreeNode>> _treeQueueClone = new ThreadLocal<Queue<SymbolicTreeNode>>(() =>
        {
            return new Queue<SymbolicTreeNode>(50);
        });

        #endregion

        #endregion

        #region OPERATIONS

        #region Static constructor
        /// <summary>
        /// New static random.
        /// </summary>
        static SymbolicTreeNode()
        {
            rand = new Random();
        }
        #endregion

        #region Overloading operators
        /// <summary>
        /// Overloading operator ==.
        /// </summary>
        /// <param name="obj1">First node.</param>
        /// <param name="obj2">Second node.</param>
        /// <returns>True if nodes are equal, otherwise false. </returns>
        public static bool operator ==( SymbolicTreeNode obj1, SymbolicTreeNode obj2 )
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

            return (obj1.type == obj2.type
                    && obj1.content == obj2.content
                    && obj1.index == obj2.index
                    && obj1.depth == obj2.depth
                    && obj1.arity == obj2.arity
                    && obj1.children == obj2.children);
        }

        /// <summary>
        /// Overloading operator !=.
        /// </summary>
        /// <param name="obj1">First node.</param>
        /// <param name="obj2">Second node.</param>
        /// <returns>False if nodes are equal, otherwise true. </returns>
        public static bool operator !=( SymbolicTreeNode obj1, SymbolicTreeNode obj2 )
        {
            return !(obj1 == obj2);
        }

        /// <summary>
        /// Overloading Equals.
        /// </summary>
        /// <param name="other">Node to compare current node.</param>
        /// <returns>True if nodes are equal, otherwise false. </returns>
        public bool Equals( SymbolicTreeNode other )
        {
            return other != null
                    && _type == other.type
                    && _content == other.content
                    && _index == other.index
                    && _arity == other.arity
                    && _depth == other.depth
                    && EqualityComparer<List<SymbolicTreeNode>>.Default.Equals(_children, other.children);
        }

        /// <summary>
        /// Overloading Equals using object.
        /// </summary>
        /// <param name="obj">Node to compare current node.</param>
        /// <returns>True if nodes are equal, otherwise false.</returns>
        public override bool Equals( object obj )
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
            int hashCode = 1395794698;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(_type);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(_content);
            hashCode = hashCode * -1521134295 + _index.GetHashCode();
            hashCode = hashCode * -1521134295 + _depth.GetHashCode();
            hashCode = hashCode * -1521134295 + _arity.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer< List<SymbolicTreeNode> >.Default.GetHashCode(_children);
            return hashCode;
        }

        #endregion

        #region Basic constructor

        /// <summary>
        /// Method for cretaing a new node.
        /// </summary>
        /// <returns> Created new SymbolicTreeNode. </returns>
        public SymbolicTreeNode()
        {
            // Initiate values of new node.
            this._type = null;
            this._content = null;
            this._index = -1;
            this._depth = -1;
            this._arity = -2;

            // Variable children is already empty array.
            this._children = null;
        }
        #endregion

        #region Deep copy
        /// <summary>
        /// Helper method for making deep copy of the Tree whose root node is current node.
        /// (method return cloned SymbolicTreeNode)
        /// </summary>
        /// <returns> Cloned Tree from current Tree.</returns>
        public SymbolicTreeNode Clone()
        {
            // Make new node.
            var clonedNewNode = new SymbolicTreeNode();

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
                clone.CopyNodeDescription(node);

                // In case copy node has no children.
                // clone.children = null; ERROR IN SETTING <- 

                // If the node has child nodes, add children nodes from the node to the dataTree 
                // and for every child make new clone node and add it to the cloneTree.
                if (node.children != null)
                {
                    // Cloned node will have same child nodes as the node.
                    clone.children = new List<SymbolicTreeNode>();

                    // Make clone child nodes and add children to the dataTree, cloneTree.
                    for (int i = 0; i < node.children.Count; i++)
                    {
                        clone.children.Add(new SymbolicTreeNode());
                        dataTree.Enqueue(node.children[i]);
                        cloneTree.Enqueue(clone.children[i]);
                    }
                }
            }
            return clonedNewNode;
        }

        /// <summary>
        /// Helper method for copying node description from sent node to current node.
        /// </summary>
        /// <param name="node">Node from which node description is being copied.</param>
        private void CopyNodeDescription(SymbolicTreeNode node)
        {
            this.type = node.type;
            this.content = node.content;
            this.index = node.index;
            this.depth = node.depth;
            this.arity = node.arity;
        }
        #endregion

        #region SymbolicTree to String
        #region (POSTORDER) Tree ToString / ToStringBuilder
        /// <summary>
        /// Helper method for converting (postorder) SymbolicTree whose root node is current node into a String.
        /// </summary>
        /// <returns> String variable that represents subtree whose root is current node. </returns>
        public string ToStringPostorder()
        {
            return this.ToStringBuilderPostorder().ToString();
        }

        /// <summary>
        /// Helper method for converting (postorder) SymbolicTree whose root node is current node into a String.
        /// </summary>
        /// <returns> StringBuilder variable that represents subtree whose root is current node. </returns>
        public StringBuilder ToStringBuilderPostorder()
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

                // If node has no child nodes, continue.
                if (node.children == null)
                    continue;

                // Add all children of the current node into a stack made of tree nodes.
                for (var i = 0; i < node.children.Count ; i++)
                {
                    dataTree.Push(node.children[i]);
                }  
            }

            string treeStringString = treeString.ToString();
            char[] treeStringCharArray = treeStringString.ToCharArray();
            Array.Reverse(treeStringCharArray);
            string reverseTreeString = new string(treeStringCharArray);

            treeString = new StringBuilder();
            treeString.Append(reverseTreeString);

            return treeString;

        }
        #endregion

        #region (PREORDER) Tree ToString / ToStringBuilder
        /// <summary>
        /// Helper method for converting (postorder) SymbolicTree whose root node is current node into a String.
        /// </summary>
        /// <returns> String variable that represents subtree whose root is current node. </returns>
        public string ToStringPreorder()
        {
            return this.ToStringBuilderPreorder().ToString();
        }

        /// <summary>
        /// Helper method for converting (preorder) SymbolicTree whose root node is current node into a String.
        /// </summary>
        /// <returns> StringBuilder variable that represents subtree whose root is current node. </returns>
        public StringBuilder ToStringBuilderPreorder()
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
                if (node.content == null)
                    continue;

                // Add value to string.
                treeString.Append(node.content.ToString());

                // If node has no child nodes, continue.
                if (node.children == null)
                    continue;

                // Add all children of the current node into a stack made of tree nodes.
                for (int i = node.children.Count - 1; i >= 0; i--)
                {
                    dataTree.Push(node.children[i]);
                }
            }
            return treeString;
        }
        #endregion

        #region (INORDER) Tree ToString / ToStringBuilder
        /// <summary>
        /// Helper method for converting (inorder) SymbolicTree whose root node is current node into a String.
        /// </summary>
        /// <returns> String variable that represents subtree whose root is current node. </returns>
        public string ToStringInorder()
        {
            return this.ToStringBuilderInorder().ToString();
        }

        /// <summary>
        /// Helper method for converting (inorder) SymbolicTree whose root node is current node into a String.
        /// </summary>
        /// <returns> StringBuilder variable that represents subtree whose root is current node. </returns>
        public StringBuilder ToStringBuilderInorder()
        {
            // Check if node has content.
            if (this.content == null)
            {
                throw new Exception("[ToStringBuilderInorder] Content of the node is null.");
            }

            /// <summary>
            /// IF node is terminal (feature) return name of the feature - node content
            /// ELSE IF node is non-terminal of arity 1 - return node.content + '(' readInorder StringBuilder of "right" node + ')'
            /// ELSE IF node is non-terminal of arity 2 - for every child node (from left to right; maximal 3 nodes) return 
            ///      '(' '(' readInorder(firstchild) + ')' + node.content + '(' readInorder(secondChild) + ')' + node.content + '(' readInorder(thirdChild) + ')'
            /// ELSE throw new exception.
            /// </summary>
            if (this.arity == 0)
            {
                #region exceptions
                // Check that node type is terminal.
                if (this.type != "terminal")
                {
                    throw new Exception("[ToStringBuilderInorder] Node has arity = 0 but his type is: " + this.type + " (not terminal).");
                }

                // Check that this node has no child node.
                if (this.children != null)
                {
                    throw new Exception("[ToStringBuilderInorder] Node has arity = 0 but has " + this.children.Count + " child nodes.");
                }
                #endregion

                // Return node content.
                return new StringBuilder(this.content);
            }
            else if (this.arity == 1)
            {
                // cos, sin, log

                #region exceptions
                // Check that node type is non-teminal.
                if (this.type != "non-terminal")
                {
                    throw new Exception("[ToStringBuilderInorder] Node has arity = 1 but his type is: " + this.type + " (not non-terminal).");
                }

                // Check that this node has 1 child node.
                if (this.children.Count != 1)
                {
                    throw new Exception("[ToStringBuilderInorder] Node has arity = 1 but has " + this.children.Count + " child nodes.");
                }
                #endregion

                // Read inorder of a child node.
                StringBuilder sb = new StringBuilder();
                sb.Append(this.content);
                sb.Append("(");
                sb.Append(this.children[0].ToStringBuilderInorder().ToString());
                sb.Append(")");

                return sb;
            }
            else if (this.arity == 2)
            {
                #region exceptions
                // Check that node type is non-teminal.
                if (this.type != "non-terminal")
                {
                    throw new Exception("[ToStringBuilderInorder] Node has arity = 2 but his type is: " + this.type + " (not non-terminal).");
                }

                // Check that this node has 2 child nodes.
                if (this.children.Count != 2)
                {
                    throw new Exception("[ToStringBuilderInorder] Node has arity = 2 but has " + this.children.Count + " child nodes.");
                }
                #endregion

                StringBuilder sb = new StringBuilder();

                sb.Append("(");    
                // Read inorder of the left child node, then add content of the current node and finally read inorder of the right child node.
                sb.Append(this.children[0].ToStringBuilderInorder().ToString());
                // Any other mathematical operation - +,-,*,/ .
                sb.Append(this.content);
                sb.Append(this.children[1].ToStringBuilderInorder().ToString());
                sb.Append(")");

                return sb;
            }
            else
            {
                // Node with given arity is not expected.
                throw new Exception("[ToStringBuilderInorder] Given node arity = " + this.arity + "  is not expected! Arity higher than 2 is not covered yet!");
            }
        }

        #endregion

        #endregion

        #region Evaluate symbolic tree
        /// <summary>
        /// Recursive method for evaluating symbolic tree.
        /// </summary>
        /// <param name="data"> Row of a table with all features. </param>
        /// <returns> Evaluation of symbolic tree whose root node is current node for given DataRow.</returns>
        public double Evaluate( DataRow data )
        {

            // Check if node has content.
            if (this.content == null)
            {
                throw new Exception("[Evaluate] Content of the node is null");
            }

            /// <summary>
            /// IF node is terminal (feature) return name of the feature - node content return feature value using DataRow.
            /// ELSE IF node is non-terminal of arity 1 - return evaluation Math.Func(this.children[0].evaluate())
            /// ELSE IF node is non-terminal of arity 2 - return evaluation (for example if node.content is "*" )
            ///         this.children[0].evaluate() * this.children[1].evaluate() (* this.children[2].evaluate())*
            /// ELSE throw new exception.
            /// </summary>
            if (this.arity == 0)
            {
                #region exceptions
                // Check that node type is terminal.
                if (this.type != "terminal")
                {
                    throw new Exception("[Evaluate] Node has arity = 0 but his type is: " + this.type + " (not terminal).");
                }

                // Check that this node has no child node.
                if (this.children != null)
                {
                    throw new Exception("[Evaluate] Node has arity = 0 but has " + this.children.Count + " child nodes.");
                }
                #endregion

                // Evaluate feature with featureName ( == this.content ) using DataRow.
                return Convert.ToDouble(data[this.content]);
            }
            else if (this.arity == 1)
            {
                // cos, sin, rlog, exp

                #region exceptions
                // Check that node type is non-teminal.
                if (this.type != "non-terminal")
                {
                    throw new Exception("[Evaluate] Node has arity = 1 but his type is: " + this.type + " (not non-terminal).");
                }

                // Check that this node has 1 child node.
                if (this.children.Count != 1)
                {
                    throw new Exception("[Evaluate] Node has arity = 1 but has " + this.children.Count + " child nodes.");
                }
                #endregion

                #region evaluate child node
                // Evaluate child node.
                double childEvaluation = this.children[0].Evaluate(data);
                #endregion

                #region evaluate current non-terminal
                // Evaluate function.
                switch (this.content)
                {
                    case "sin":
                        return Math.Sin(childEvaluation);
                    case "cos":
                        return Math.Cos(childEvaluation);
                    case "rlog":
                        if (childEvaluation == 0)
                        {
                            return 0;
                        }
                        return Math.Log(Math.Abs(childEvaluation));
                    case "exp":
                        return Math.Exp(childEvaluation);
                    default:
                        throw new Exception("[Evaluate] Sent unary mathematical operation is not expected.");
                }
                #endregion
            }
            else if (this.arity == 2)
            {
                // *, /, +, -

                #region exceptions
                // Check that node type is non-teminal.
                if (this.type != "non-terminal")
                {
                    throw new Exception("[Evaluate] Node has arity = 2 but his type is: " + this.type + " (not non-terminal).");
                }

                // Check that this node has 2 child nodes.
                if (this.children.Count != 2)
                {
                    throw new Exception("[Evaluate] Node has arity = 2 but has " + this.children.Count + " child nodes.");
                }
                #endregion

                #region evaluate child nodes
                double child1Evaluation = this.children[0].Evaluate(data);
                double child2Evaluation = this.children[1].Evaluate(data);
                #endregion

                #region evaluate current non-terminal
                switch (this.content)
                {
                    case "+":
                        return child1Evaluation + child2Evaluation;
                    case "-":
                        return child1Evaluation - child2Evaluation;
                    case "*":
                        return child1Evaluation * child2Evaluation;
                    case "/":
                        if (child2Evaluation == 0)
                        {
                            // Protected division
                            return 1;
                        }
                        return child1Evaluation / child2Evaluation;
                    default:
                        throw new Exception("[Evaluate] Sent binary mathematical operation is not expected.");
                            
                }
                #endregion
            }
            else
            {
                // Node with given arity is not expected.
                throw new Exception("[Evaluate] Given node arity = " + this.arity + "  is not expected! Arity higher than 2 is not covered yet!");
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

                // If node doesn't have children, continue.
                if (node.children == null)
                    continue;

                // Add all children of the current node into a stack made of tree nodes.
                for (int i = node.children.Count - 1; i >= 0; i--)
                {
                    dataTree.Push(node.children[i]);
                }
            }
            return noOfNodes;
        }
        #endregion

        #region Find node with index
        /// <summary>
        /// Helper method that returns node with wanted index from the subtree whose root 
        /// is the current node, based on Depth-First search method.
        /// </summary>
        /// <param name="i">Index of a node to be fined.</param>
        /// <returns> Node with wanted index in a Subtree whose root is the current node. </returns>
        public SymbolicTreeNode FindNodeWithIndex( int i )
        {
            // Make sure index is positive number.
            if (i < 0)
            {
                throw new IndexOutOfRangeException("[FindNodeWithIndex] Given index = " + i +" is not positive.");
            }

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

                // If value of the node is null continue. 
                if (node.content == null)
                    continue;

                // If current node has the index we are looking for, return current node.
                if (node.index == i)
                {
                    return node;
                }

                // If current node has no child nodes, continue with loop.
                if (node.children == null)
                    continue;

                // Add all children of the current node into a stack made of tree nodes.
                for (var j = 0; j < node.children.Count; j++)
                {
                    dataTree.Enqueue(node.children[j]);
                }
            }

            // In case we came to the end and still haven't found node with wanted index, throw exception.
            throw new IndexOutOfRangeException("[FindNodeWithIndex] Node with given index " + i + " is not found.");
        }
        #endregion

        #region Depth of node with index
        /// <summary>
        /// Helper method that returns level of a node with a given index.
        /// </summary>
        /// <param name="i">Index of the node.</param>
        /// <returns>Level of the node in a Tree.</returns>
        public int DepthOfNodeWithIndex( int i )
        {
            // Make sure index is positive number.
            if (i < 0)
            {
                throw new IndexOutOfRangeException("[DepthOfNodeWithIndex] Index out of range");
            }

            // Get node with helper method NodeAt.
            SymbolicTreeNode nodeWithGivenIndex = FindNodeWithIndex(i);

            // Return the level of the node.
            return nodeWithGivenIndex.depth;
        }
        #endregion

        #region Depth of the symbolic tree whose root node is current node
        /// <summary>
        /// Helper method that calculates maximal depth of the tree based on node variables depth 
        /// (and not on actual depth in the subtree whose root is current node) in the given subtree.
        /// </summary>
        /// <returns> Maximal depth of the subtree (maximal variable depth of nodes in subtree). </returns>
        public int DepthOfSymbolicTree()
        {
            // Set initial depth as -1.
            int MaxDepth = -1;

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

                // If value of next node is null, continue. 
                if (node.content == null)
                    continue;

                // If current node has the index we are looking for, return current node.
                if (node.depth > MaxDepth )
                {
                    MaxDepth = node.depth;
                }

                // If node has no child nodes, continue.
                if (node.children == null)
                    continue;

                // Add all children of the current node into a stack made of tree nodes.
                for (var j = 0; j < node.children.Count; j++)
                {
                    dataTree.Enqueue(node.children[j]);
                }
            }

            if ( MaxDepth == -1)
            {
                throw new Exception("[DepthOfSymbolicTree] Symbolic tree is empty and depth is -1.");
            }

            return (MaxDepth - this.depth);
        }
        #endregion

        #region Generate tree
        /// <summary>
        /// (RECURSIVE) Generate tree using full method.
        /// 
        /// Each individual created by the full method has the maximum possible size, in the sense 
        /// that the distance from the root node to each leaf is equal to the maximum tree depth.
        /// 
        /// A consequence of the full method is that a large quantity of the trees in the
        /// initial population will have a similar structure.Consequently, the initial population
        /// is less diverse due to the similarity in tree structures.
        /// 
        /// </summary>
        /// <param name="maxDepth"> Maximal depth.</param>
        /// <param name="currentDepth"> Current depth - recursion. </param>
        /// <param name="terminalsMarks"> Marks for leaf nodes. </param>
        /// <param name="nonTerminals"> Marks for inner nodes. </param>
        /// <param name="mathOperationsArity"> Dictionary with all possible non-terminals and their arity. </param>
        public void GenerateFullSymbolicTree( int maxDepth, int currentDepth, string[] terminalsMarks, string[] nonTerminals, Dictionary<string, int> mathOperationsArity )
        {
            #region leaf node
            // If we came to the leaf node.
            if (currentDepth == maxDepth)
            {
                // Set type of node as terminal.
                this._type = "terminal";

                // Set depth of a node.
                this._depth = maxDepth;

                // Set arity of a node.
                this._arity = 0;

                // Choose feature that will represent this node.
                int indexOfTerminal = rand.Next(terminalsMarks.Length);
                this._content = terminalsMarks[indexOfTerminal];

                // No child nodes.
                this._children = null;

                // Done.
                return;
            }
            #endregion

            #region inner node <-> non-terminal
            // Set content of current inner node. 
            this._type = "non-terminal";
            this._depth = currentDepth;
            this._content = nonTerminals[rand.Next(nonTerminals.Length)];

            // Get mathematical operation arity.
            int ar = getMathOperationArity(mathOperationsArity, this._content);

            // ****************************************************************************************
            // FOR FUTURE USE - TODO - if node can have arity greater than 2.
            // If arity is not 1, choose random aritiy between 2 and 3.
            // if (ar != 1)
            // {
            //    // Since operations with arity different from 1 are +, -, *, / that all have arity >= 2.
            //    // For simplicity, here are considered only operations with arity 2 and 3.
            //    ar = rand.Next(2, 3); // TODO: for now, aritiy can only be 2
            // }
            // ****************************************************************************************

            // Set node arity.
            this._arity = ar;

            // Allocate memory for child nodes and recurive set contents of those nodes.
            this._children = new List<SymbolicTreeNode>();

            for (var i = 0; i < this._arity; i++)
            {
                // Generate (sub)trees of all child nodes.
                this._children.Add(new SymbolicTreeNode());
                this._children[i].GenerateFullSymbolicTree(maxDepth, currentDepth + 1, terminalsMarks, nonTerminals,  mathOperationsArity);
            }
            #endregion
        }

        /// <summary>
        /// (RECURSIVE) Generate tree using grow method.
        /// 
        /// The grow method creates trees of different shapes and sizes. When creating a
        /// tree using this method, at each depth an element from the non-terminal set or from the
        /// terminal set can be randomly selected. However, at the maximum depth, only an
        /// element from the terminal set can be selected.
        /// 
        /// This method benefits from the fact that trees of different sizes are created which
        /// will result in greater diversity as opposed to the full method. Although the grow
        /// method results in greater diversity, the method also suffers from the randomness
        /// involved in creating the trees.
        /// 
        /// </summary>
        /// <param name="maxDepth"> Maximal depth.</param>
        /// <param name="currentDepth"> Current depth - recursion. </param>
        /// <param name="terminalsMarks"> Marks for leaf nodes. </param>
        /// <param name="nonTerminals"> Marks for inner nodes. </param>
        /// <param name="mathOperationsArity"> Dictionary with all possible non-terminals and their arity. </param>
        public void GenerateGrowSymbolicTree(int maxDepth, int currentDepth, string[] terminalsMarks, string[] nonTerminals, Dictionary<string, int> mathOperationsArity)
        {
            #region leaf node
            // If we came to the leaf node.
            if (currentDepth == maxDepth)
            {
                // Set type of node as terminal.
                this._type = "terminal";

                // Set depth of a node.
                this._depth = maxDepth;

                // Set arity of a node.
                this._arity = 0;

                // Choose feature that will represent this node.
                int indexOfTerm = rand.Next(terminalsMarks.Length);
                this._content = terminalsMarks[indexOfTerm];

                // No child nodes.
                this._children = null;

                // Done.
                return;
            }
            #endregion

            // Choose randomly if this node should be terminal or non-terminal.
            bool chooseNonTerminal = rand.Next(100) < 50 ? true : false; // TODO different probability?

            #region non-terminal is choosen
            // If this node will be non-terminal (math.op) 
            // OR if it choosen that this node should be terminal, but we are at root node 
            // (since root node has to be non-terminal)
            // make this node non-terminal
            if ((!chooseNonTerminal && currentDepth == 0) || chooseNonTerminal)
            {
                // Set content of current inner node. 
                this._type = "non-terminal";
                this._depth = currentDepth;
                int indexOfNonTerminal = rand.Next(nonTerminals.Length);
                this._content = nonTerminals[indexOfNonTerminal];

                // Get mathematical operation arity.
                int ar = getMathOperationArity(mathOperationsArity, this._content);

                // ****************************************************************************************
                // FOR FUTURE USE - TODO - if node can have arity greater than 2.
                // If arity is not 1, choose random aritiy between 2 and 3.
                // if (ar != 1)
                // {
                //    // Since operations with arity different from 1 are +, -, *, / that all have arity >= 2.
                //    // For simplicity, here are considered only operations with arity 2 and 3.
                //    ar = rand.Next(2, 3); // TODO: for now, aritiy can only be 2
                // }
                // ****************************************************************************************

                // Set node arity.
                this._arity = ar;

                // Allocate memory for child nodes and recurive set contents of those nodes.
                this._children = new List<SymbolicTreeNode>();

                for (var i = 0; i < this._arity; i++)
                {
                    // Generate (sub)trees of all child nodes.
                    this._children.Add(new SymbolicTreeNode());
                    this._children[i].GenerateGrowSymbolicTree(maxDepth, currentDepth + 1, terminalsMarks, nonTerminals, mathOperationsArity);
                }

                // Done.
                return;
            }
            #endregion

            #region terminal is choosen
            // ELSE. This node will be terminal.

            // Set type of node as terminal.
            this._type = "terminal";

            // Set depth of a node.
            this._depth = currentDepth;

            // Set arity of a node.
            this._arity = 0;

            // Choose feature that will represent this node.
            int indexOfTerminal = rand.Next(terminalsMarks.Length);
            this._content = terminalsMarks[indexOfTerminal];

            // No child nodes.
            this._children = null;
            #endregion
        }

        /// <summary>
        /// Helper method for determinating aritiy of mathematical operators.
        /// </summary>
        /// <param name="mathOperationsArity">Dictionary of all non-terminals with their arity.</param>
        /// <param name="mathOperation">Non-terminal for which is needed to determinate his arity..</param>
        /// <returns>Aritiy of the sent mathematical operator.</returns>
        private int getMathOperationArity(Dictionary<string, int> mathOperationsArity, string mathOperation)
        {
            if (mathOperationsArity.ContainsKey(mathOperation))
            {
                return mathOperationsArity[mathOperation];
            }
            else
            {
                throw new Exception("[SymbolicTreeNode::getMathOperationArity] Sent mathematical operator is not knows.");
            }
        }
        #endregion

        #region Calculate indices of the nodes in the tree
        /// <summary>
        /// Calculate indices of the Symbolic Tree whose root node is the current node.
        /// Indices represent preorder of nodes.
        /// </summary>
        public void CalculateIndices( )
        {
            // Helper variable for assigning indices to the nodes.
            int count = 0;

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
                if (node.content == null)
                    continue;

                // Set node index.
                node.index = count;

                // Increase count. 
                count++;

                // Add all children of the current node into a stack made of tree nodes.
                if (node.children != null)
                {
                    for (int i = node.children.Count - 1; i >= 0; i--)
                    {
                        dataTree.Push(node.children[i]);
                    }
                }
            }
        }
        #endregion

        #region Calculate depths
        /// <summary>
        /// Method for calculating depths of subtree with given starting depth.
        /// </summary>
        /// <param name="currentDepth">Starting depth.</param>
        public void CalculateDepths( int currentDepth )
        {
            // Update depth of the node.
            this.depth = currentDepth;

            // If node is null call to the method should not be preformed.
            if (this == null)
            {
                throw new Exception("[CalculateDepths] Node is null. Not possible since all cases should be already covered.");
            }


            if (this.arity == 0)
            {
                // If terminal node.

                #region exceptions
                // Check that node type is terminal.
                if (this.type != "terminal")
                {
                    throw new Exception("[CalculateDepths] Node has arity = 0 but his type is: " + this.type + " (not terminal).");
                }

                // Check that this node has no child node.
                if (this.children != null)
                {
                    throw new Exception("[CalculateDepths] Node has arity = 0 but has " + this.children.Count + " child nodes.");
                }
                #endregion

                // Done.
                return;
            }
            else if (this.arity == 1)
            {
                // If non-terminal node with arity 1.

                #region exceptions
                // Check that node type is non-teminal.
                if (this.type != "non-terminal")
                {
                    throw new Exception("[CalculateDepths] Node has arity = 1 but his type is: " + this.type + " (not non-terminal).");
                }

                // Check that this node has 1 child node.
                if (this.children.Count != 1)
                {
                    throw new Exception("[CalculateDepths] Node has arity = 1 but has " + this.children.Count + " child nodes.");
                }
                #endregion

                // Update depth of the child node.
                this.children[0].CalculateDepths(currentDepth + 1);

                // Done.
                return;
            }
            else if (this.arity == 2)
            {
                // If non-terminal node with arity 2.

                #region exceptions 
                // Check that node type is non-teminal.
                if (this.type != "non-terminal")
                {
                    throw new Exception("[CalculateDepths] Node has arity = 2 but his type is: " + this.type + " (not non-terminal).");
                }

                // Check that this node has 2 child nodes.
                if (this.children.Count != 2)
                {
                    throw new Exception("[CalculateDepths] Node has arity = 2 but has " + this.children.Count + " child nodes.");
                }
                #endregion

                // Update depth of the child nodes.
                this.children[0].CalculateDepths(currentDepth + 1);
                this.children[1].CalculateDepths(currentDepth + 1);

                // Done.
                return;
            }
            else
            {
                // Node with given arity is not expected. 
                throw new Exception("[CalculateDepths] Given node arity = " + this.arity + " is not expected! Arity higher than 2 is not covered yet!");
            }

        }

        /// <summary>
        /// Calculate depth of subtree.
        /// </summary>
        /// <param name="maxDepth">Reference to variable that remembers current maximal depth of a tree.</param>
        public void CalculateDepth(ref int maxDepth)
        {
            // If node is null call to the method should not be preformed.
            if (this == null)
            {
                throw new Exception("[CalculateDepth] Node is null. Not possible since all cases should be already covered.");
            }

            if (this.arity == 0)
            {
                // If terminal node.

                #region exceptions
                // Check that node type is terminal.
                if (this.type != "terminal")
                {
                    throw new Exception("[CalculateDepth] Node has arity = 0 but his type is: " + this.type + " (not terminal).");
                }

                // Check that this node has no child node.
                if (this.children != null)
                {
                    throw new Exception("[CalculateDepth] Node has arity = 0 but has " + this.children.Count + " child nodes.");
                }
                #endregion

                // Check if higher depth.
                if (this.depth > maxDepth)
                {
                    maxDepth = this.depth;
                }

                // Done.
                return;
            }
            else if (this.arity == 1)
            {
                // If non-terminal node with arity 1.

                #region exceptions
                // Check that node type is non-teminal.
                if (this.type != "non-terminal")
                {
                    throw new Exception("[CalculateDepth] Node has arity = 1 but his type is: " + this.type + " (not non-terminal).");
                }

                // Check that this node has 1 child node.
                if (this.children.Count != 1)
                {
                    throw new Exception("[CalculateDepth] Node has arity = 1 but has " + this.children.Count + " child nodes.");
                }
                #endregion

                // Update depth of the child node.
                this.children[0].CalculateDepth(ref maxDepth);

                // Done.
                return;
            }
            else if (this.arity == 2)
            {
                // If non-terminal node with arity 2.

                #region exceptions 
                // Check that node type is non-teminal.
                if (this.type != "non-terminal")
                {
                    throw new Exception("[CalculateDepth] Node has arity = 2 but his type is: " + this.type + " (not non-terminal).");
                }

                // Check that this node has 2 child nodes.
                if (this.children.Count != 2)
                {
                    throw new Exception("[CalculateDepth] Node has arity = 2 but has " + this.children.Count + " child nodes.");
                }
                #endregion

                // Update depth of the child nodes.
                this.children[0].CalculateDepth(ref maxDepth);
                this.children[1].CalculateDepth(ref maxDepth);

                // Done.
                return;
            }
            else
            {
                // Node with given arity is not expected. 
                throw new Exception("[CalculateDepth] Given node arity = " + this.arity + " is not expected! Arity higher than 2 is not covered yet!");
            }
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
    public class Chromosome : IEquatable<Chromosome>
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
                this._symbolicTree = (SymbolicTreeNode)value.Clone();
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

        #region number of nodes in symbolic tree
        // Variable that represents number of nodes in symbolic tree.
        private int _numberOfNodesInTree;

        // Property for the _numberOfNodesInTree variable.
        public int numberOfNodesInTree
        {
            get { return _numberOfNodesInTree; }
            set { _numberOfNodesInTree = value; }
        }
        #endregion

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

        #region Basic constructor
        /// <summary>
        /// Constructing new empty Chromosome.
        /// </summary>
        public Chromosome()
        {
            this._fitness = -1;
            this._depth = -1;
            this._numberOfPossibleTerminals = -1;
            this._numberOfNodesInTree = -1;
            this._symbolicTree = new SymbolicTreeNode();
        }
        #endregion

        #region Deep Copy
        /// <summary>
        /// Helper method for deep copying values of model st to values of this model.
        /// </summary>
        public Chromosome Clone( )
        {
            Chromosome c = new Chromosome();

            // Copy fitness from chromosome c to this chromosome.
            c._fitness = this.fitness;

            // Copy numberOfPossibleTerminals from chromosome c to this chromosome.
            c._numberOfPossibleTerminals = this.numberOfPossibleTerminals;

            //Copy _numberOfNodesInTree from chromosome c to this chromosome.
            c._numberOfNodesInTree = this.numberOfNodesInTree;

            // Copy depth of tree from chromosome c to this chromosome.
            c._depth = this.depth;

            // Deep copy with variable property set.
            c.symbolicTree = (SymbolicTreeNode)this.symbolicTree.Clone();

            return c;
        }
        #endregion

        #region Overloading operators
        /// <summary>
        /// Overloading operator ==.
        /// </summary>
        /// <param name="obj1">First chromosome.</param>
        /// <param name="obj2">Second chromosome.</param>
        /// <returns>True if chromosomes are equal, otherwise false. </returns>
        public static bool operator ==( Chromosome obj1, Chromosome obj2 )
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
                    && obj1.numberOfNodesInTree == obj2.numberOfNodesInTree
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
        public static bool operator !=( Chromosome obj1, Chromosome obj2 )
        {
            return !(obj1 == obj2);
        }

        /// <summary>
        /// Overloading Equals.
        /// </summary>
        /// <param name="other">Chromosome to compare current chromosome.</param>
        /// <returns>True if chromosomes are equal, otherwise false. </returns>
        public bool Equals( Chromosome other )
        {
            return other != null 
                    && _fitness == other._fitness 
                    && _numberOfNodesInTree == other.numberOfNodesInTree
                    && EqualityComparer<SymbolicTreeNode>.Default.Equals(_symbolicTree, other._symbolicTree) 
                    && _depth == other._depth 
                    && _numberOfPossibleTerminals == other._numberOfPossibleTerminals;
        }

        /// <summary>
        /// Overloading Equals using object.
        /// </summary>
        /// <param name="obj">Chromosome to compare current chromosome.</param>
        /// <returns>True if chromosomes are equal, otherwise false.</returns>
        public override bool Equals( object obj )
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj.GetType() == this.GetType() && Equals((Chromosome)obj);
        }

        /// <summary>
        /// Generated GetHashCode method.
        /// </summary>
        /// <returns> Generated hash code. </returns>
        public override int GetHashCode()
        {
            int hashCode = 710112629;
            hashCode = hashCode * -1521134295 + _fitness.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<SymbolicTreeNode>.Default.GetHashCode(_symbolicTree);
            hashCode = hashCode * -1521134295 + _depth.GetHashCode();
            hashCode = hashCode * -1521134295 + _numberOfNodesInTree.GetHashCode();
            hashCode = hashCode * -1521134295 + _numberOfPossibleTerminals.GetHashCode();
            return hashCode;
        }

        #endregion

        #region Fitness
        /// <summary>
        /// Method for calculating fitness of current model.
        /// Fitness is calculated as proportion of ( true positives + true negatives) and total number of files.
        /// </summary>
        /// <param name="data">Feature values from train/test set needed for calculating fitness.</param>
        public void CalculateFitness( DataTable data )
        {
            // Number of true positives.
            int TP = 0;
            // Number of true negatives.
            int TN = 0;
            // Number of data rows.
            int NumberOfFiles = data.Rows.Count;

            foreach (DataRow row in data.Rows)
            {
                // Evaluation of curretn row of data.
                double evaluation = this._symbolicTree.Evaluate(row);
                int classification = Convert.ToInt32(row["label"]); // TODO "label" is hardcoded!

                if ( evaluation > 0 && classification == 1)
                {
                    // If evaluation is greater than 0 file is assumed to be malicious.
                    TP++;
                }
                else if( evaluation <= 0 && classification == 0)
                {
                    // If evaluation isless or equal to zero file is assumed to be benign.
                    TN++;
                }
            }

            // Number of files is always greater than 0.
            if (NumberOfFiles == 0)
            {
                throw new Exception("[CalculateFitness] Number of files is zero!");
            }
            // Calculate fitness.
            this._fitness =  (TP + TN) / (double) (NumberOfFiles) ;
        }
        #endregion

        #region Generate initial chromosome
        /// <summary>
        /// Generate new symbolic tree of maximal depth maxDepth whose inner node contents 
        /// are strings from nonTerminals, and leafs have content from terminals.
        /// </summary>
        /// 
        /// <param name="method"> 
        /// Method for generating initial symbolic tree. Options are:
        /// *full method where chromosome has maximal possible size - the distance from the root node to each leaf is equal to the maximum tree depth;
        /// *grow method where chromosome has random size, but not greater than maxDepth;
        /// </param>
        /// <param name="maxDepth"> Maximal depth of (to-be generated) symbolic tree. </param>
        /// <param name="terminalsMarks"> Array of strings representing possible terminals (features names). </param>
        /// <param name="terminals"> Array of possible terminals of to-be generated symbolic tree.</param>
        /// <param name="nonTerminals"> Array of possible terminals of to-be generated symbolic tree. </param>
        /// <param name="mathOperationsArity"> Dictionary with all non-terminals and their arity. </param>
        public void Generate( string method, int maxDepth, string[] terminalsMarks, DataTable terminals, string[] nonTerminals, Dictionary<string, int> mathOperationsArity )
        {
            switch (method)
            {
                case "full":
                    // Setting number of possible terminals as length of the array terminalsMarks.
                    this._numberOfPossibleTerminals = terminalsMarks.Length;

                    // Generate symbolic tree.
                    this._symbolicTree.GenerateFullSymbolicTree(maxDepth, 0, terminalsMarks, nonTerminals, mathOperationsArity);

                    // Calculate indices of the nodes.
                    this._symbolicTree.CalculateIndices();

                    // Calculate depth of a generated symbolic tree.
                    // In this method, the distance from the root node to each leaf is equal to the maximum tree depth.
                    this._depth = maxDepth;

                    // Set number of nodes in the generated symbolic tree.
                    this._numberOfNodesInTree = this._symbolicTree.NumberOfNodes();

                    // Calculate fitness.
                    CalculateFitness(terminals);
                    break;

                case "grow":
                    // Setting number of possible terminals as length of the array terminalsMarks.
                    this._numberOfPossibleTerminals = terminalsMarks.Length;

                    // Generate symbolic tree.
                    this._symbolicTree.GenerateGrowSymbolicTree(maxDepth, 0, terminalsMarks, nonTerminals, mathOperationsArity);

                    // Calculate indices of the nodes.
                    this._symbolicTree.CalculateIndices();

                    // Calculate depth of a generated symbolic tree.
                    this._depth = this._symbolicTree.DepthOfSymbolicTree();

                    // Set number of nodes in the generated symbolic tree.
                    this._numberOfNodesInTree = this._symbolicTree.NumberOfNodes();

                    // Calculate fitness.
                    CalculateFitness(terminals);
                    break;

                default:
                    throw new System.ArgumentException("[Generate] Method is not supported", "method");
            }
        }

        #endregion

        #region Accuracy of the model
        /// <summary>
        /// Calculate accuracy of the model (symbolic tree) on test data sent in DataTable.
        /// </summary>
        /// <param name="data"> DataTable representing test data. </param>
        /// <returns>Accuracy of the model.</returns>
        internal double Accuracy( DataTable data )
        {
            int correct = 0;
            foreach (DataRow row in data.Rows)
            {
                if (this._symbolicTree.Evaluate(row) == Convert.ToInt32(row["label"])) // TODO "label" is hardcoded!
                {
                    correct++;
                }
            }

            // Calculate accuracy and return.
            return correct / (double) (data.Rows.Count);
        }
        #endregion

        #region Calculate new depth of chromosome tree
        /// <summary>
        /// Helper method for updating new depth of chromosome tree structure.
        /// </summary>
        internal void CalculateNewDepthOfTree()
        {
            int newDepth = 0;
            this.symbolicTree.CalculateDepth(ref newDepth);
            this.depth = newDepth;
        }
        #endregion

        #endregion
    }
    #endregion
}
