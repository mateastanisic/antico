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
                    this._children[i] = value[i].Clone();
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

        //Helper variable for handling with iteration throught tree structure.
        private static ThreadLocal<Queue<SymbolicTreeNode>> _treeQueueParent1 = new ThreadLocal<Queue<SymbolicTreeNode>>(() =>
        {
            return new Queue<SymbolicTreeNode>(50);
        });

        //Helper variable for handling with iteration throught tree structure.
        private static ThreadLocal<Queue<SymbolicTreeNode>> _treeQueueParent2 = new ThreadLocal<Queue<SymbolicTreeNode>>(() =>
        {
            return new Queue<SymbolicTreeNode>(50);
        });

        #endregion

        #endregion

        #region OPERATIONS

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
                    && _type == other.type
                    && _content == other.content
                    && _index == other.index
                    && _arity == other.arity
                    && _depth == other.depth
                    && EqualityComparer<SymbolicTreeNode[]>.Default.Equals(_children, other.children);
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
            int hashCode = 1395794698;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(_type);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(_content);
            hashCode = hashCode * -1521134295 + _index.GetHashCode();
            hashCode = hashCode * -1521134295 + _depth.GetHashCode();
            hashCode = hashCode * -1521134295 + _arity.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<SymbolicTreeNode[]>.Default.GetHashCode(_children);
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
                node.children = null;

                // If the node has child nodes, add children nodes from the node to the dataTree 
                // and for every child make new clone node and add it to the cloneTree.
                if (node.children != null)
                {
                    // Cloned node will have same child nodes as the node.
                    clone.children = new SymbolicTreeNode[node.children.Length];

                    // Make clone child nodes and add children to the dataTree, cloneTree.
                    for (int i = 0; i < node.children.Length; i++)
                    {
                        clone.children[i] = new SymbolicTreeNode();
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

                // Add all children of the current node into a stack made of tree nodes.
                if (node.children != null)
                {
                    for (var i = 0; i < node.children.Length ; i++)
                    {
                        dataTree.Push(node.children[i]);
                    }  
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
            return ReadInorder(this);
        }

        /// <summary>
        /// Recursive helper method for converting SymbolicTree into a readable form.
        /// </summary>
        /// <param name="node"> Node whose inorder we want to show as stringBuilder. </param>
        /// <returns> StringBuilder inorder of a symbolic tree. </returns>
        private StringBuilder ReadInorder(SymbolicTreeNode node)
        {
            // Check if node has content.
            if ( node.content == null )
            {
                throw new Exception("Content of the node is null");
            }

            /// <summary>
            /// IF node is terminal (feature) return name of the feature - node content
            /// ELSE IF node is non-terminal of arity 1 - return node.content + '(' readInorder StringBuilder of "right" node + ')'
            /// ELSE IF node is non-terminal of arity 2 - for every child node (from left to right; maximal 3 nodes) return 
            ///      '(' '(' readInorder(firstchild) + ')' + node.content + '(' readInorder(secondChild) + ')' + node.content + '(' readInorder(thirdChild) + ')'
            /// ELSE throw new exception.
            /// </summary>
            if ( node.arity == 0 )
            {
                // Check that node type is terminal.
                if (node.type != "terminal")
                {
                    throw new Exception("Arity of node is zero but node is not terminal.");
                }

                // Return node content.
                return new StringBuilder(node.content);
            }
            else if( node.arity == 1 )
            {
                // cos, sin, log

                // Check that node type is non-teminal and that this node has 1 child node.
                if (node.type != "non-termianl")
                {
                    throw new Exception("Arity of node is zero but node is not terminal.");
                }

                // Check that this node has 1 child node.
                if (node.children.Length != 1)
                {
                    throw new Exception("Arity of node is 1 but node has more than one child node.");
                }

                // Read inorder of a child node.
                StringBuilder sb = new StringBuilder();
                sb.Append(node.content);
                sb.Append("(");
                sb.Append(ReadInorder(node.children[0]).ToString());
                sb.Append(")");

                return sb;

            }
            else if( node.arity == 2 )
            {
                // Check that node type is non-teminal.
                if (node.type != "non-termianl" )
                {
                    throw new Exception("Arity of node is 2 or greater but node is not terminal.");
                }

                // Check that this node has 2 or 3 child nodes.
                if ( node.children.Length < 2 || node.children.Length > 3)
                {
                    throw new Exception("Arity of node is not 2 or 3.");
                }

                // Check that arity of node is same as number of child nodes.
                if ( node.children.Length != node.arity )
                {
                    throw new Exception("Arity of node is not same as the number of child nodes.");
                }

                StringBuilder sb = new StringBuilder();

                sb.Append("(");
                // Any other mathematical operation - +,-,*,/ .
                for( var i = 0; i < node.children.Length; i++)
                {
                    // Read inorder of a child node.
                    sb.Append(ReadInorder(children[i]).ToString());

                    // If this child node is not last child node add operation between two expressions.
                    if (i + 1 != node.children.Length)
                    {
                        sb.Append(node.content);
                    } 
                }
                sb.Append(")");

                return sb;
            }
            else
            {
                // Node with given arity is not expected.
                throw new Exception("Given node arity is not expected!");
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
        public double Evaluate(DataRow data)
        {

            // Check if node has content.
            if (this.content == null)
            {
                throw new Exception("Content of the node is null");
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
                // Check that node type is terminal.
                if (this.type != "terminal")
                {
                    throw new Exception("Arity of node is zero but node is not terminal.");
                }

                // Evaluate feature with featureName ( == this.content ) using DataRow.
                return Convert.ToDouble(data[this.content]);
            }
            else if (this.arity == 1)
            {
                // cos, sin, rlog, exp

                // Check that node type is non-teminal and that this node has 1 child node.
                if (this.type != "non-terminal")
                {
                    throw new Exception("Arity of node is zero but node is not terminal.");
                }

                // Check that this node has 1 child node.
                if (this.children.Length != 1)
                {
                    throw new Exception("Arity of node is 1 but node has more than one child node.");
                }

                // Evaluate child node.
                double childEvaluation = this.children[0].Evaluate(data);

                // Evaluate function.
                switch (this.content)
                {
                    case "sin":
                        return Math.Sin(childEvaluation);
                    case "cos":
                        return Math.Cos(childEvaluation);
                    case "rlog":
                        return Math.Log(Math.Abs(childEvaluation));
                    case "exp":
                        return Math.Exp(Math.Exp(childEvaluation));
                    default:
                        throw new Exception("Sent unary mathematical operation is not expected.");
                }

            }
            else if (this.arity == 2)
            {
                // *, /, +, -

                // Check that node type is non-teminal.
                if (this.type != "non-terminal")
                {
                    throw new Exception("Arity of node is 2 or greater but node is not terminal.");
                }

                // Check that this node has 2 or 3 child nodes.
                if (this.children.Length < 2 || this.children.Length > 3)
                {
                    throw new Exception("Arity of node is not 2 or 3.");
                }

                // Check that arity of node is same as number of child nodes.
                if (this.children.Length != this.arity)
                {
                    throw new Exception("Arity of node is not same as the number of child nodes.");
                }

                double childEvaluation;
                double result = 0;

                // Evaluate.
                for (var i = 0; i < this.children.Length; i++)
                {
                    // Evaluate child node.
                    childEvaluation = this.children[i].Evaluate(data);

                    switch (this.content)
                    {
                        case "+":
                            result += childEvaluation;
                            break;
                        case "-":
                            result -= childEvaluation;
                            break;
                        case "*":
                            result *= childEvaluation;
                            break;
                        case "/":
                            if( childEvaluation == 0)
                            {
                                // Protected division
                                result = 1;
                            }
                            else
                            {
                                result /= childEvaluation;
                            }
                            break;
                        default:
                            throw new Exception("Sent binary mathematical operation is not expected.");
                            
                    }

                }

                return result;
            }
            else
            {
                // Node with given arity is not expected.
                throw new Exception("Given node arity is not expected!");
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

        #region Find node with index
        /// <summary>
        /// Helper method that returns node with wanted index from the subtree whose root 
        /// is the current node, based on Depth-First search method.
        /// </summary>
        /// <param name="index"></param>
        /// <returns> Node with wanted index in a Subtree whose root is the current node. </returns>
        public SymbolicTreeNode FindNodeWithIndex(int i)
        {
            // Make sure index is positive number.
            if( i < 0 )
            {
                throw new IndexOutOfRangeException("Index out of range");
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

                // If value of next node is null continue. 
                if (node.content == null)
                    continue;

                // If current node has the index we are looking for, return current node.
                if (node.index == i)
                {
                    return node;
                }

                // Add all children of the current node into a stack made of tree nodes.
                if (node.children != null)
                {
                    for (var j = 0; j < node.children.Length; j++)
                    {
                        dataTree.Enqueue(node.children[j]);
                    }
                }
            }

            // In case we came to the end and still haven't found node with wanted index, return exception.
            throw new IndexOutOfRangeException("Index out of tree length range");
        }
        #endregion

        #region Depth of node with index
        /// <summary>
        /// Helper method that returns level of a node with a given index.
        /// </summary>
        /// <param name="i">Index of the node.</param>
        /// <returns>Level of the node in a Tree.</returns>
        public int DepthOfNodeWithIndex(int i)
        {
            // Make sure index is positive number.
            if ( i < 0 )
            {
                throw new IndexOutOfRangeException("Index out of range");
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

                // If value of next node is null continue. 
                if (node.content == null)
                    continue;

                // If current node has the index we are looking for, return current node.
                if (node.depth > MaxDepth )
                {
                    MaxDepth = node.depth;
                }

                // Add all children of the current node into a stack made of tree nodes.
                if (node.children != null)
                {
                    for (var j = 0; j < node.children.Length; j++)
                    {
                        dataTree.Enqueue(node.children[j]);
                    }
                }
            }

            if ( MaxDepth == -1)
            {
                throw new Exception("Symbolic tree is empty and depth is -1.");
            }

            return MaxDepth;
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
        public void GenerateFullSymbolicTree(int maxDepth, int currentDepth, string[] terminalsMarks, string[] nonTerminals, Dictionary<string, int> mathOperationsArity)
        {
            var rand = new Random();

            // If we came to the leaf node.
            if ( currentDepth == maxDepth )
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

            // Set content of current inner node. 
            this._type = "non-terminal";
            this._depth = currentDepth;
            int indexOfNonTerminal = rand.Next(nonTerminals.Length);
            this._content = nonTerminals[indexOfNonTerminal];

            // Get mathematical operation arity.
            int ar = getMathOperationArity(mathOperationsArity, this._content);

            // If arity is not 1, choose random aritiy between 2 and 3.
            if ( ar != 1 )
            {
                // Since operations with arity different from 1 are +, -, *, / that all have arity >= 2.
                // For simplicity, here are considered only operations with arity 2 and 3.
                ar = rand.Next(2, 3); // TODO: for now, aritiy can only be 2
            }

            // Set node arity.
            this._arity = ar;

            // Allocate memory for child nodes and recurive set contents of those nodes.
            this._children = new SymbolicTreeNode[this._arity];

            for( var i = 0; i < this._arity; i++)
            {
                this._children[i] = new SymbolicTreeNode();
                this._children[i].GenerateFullSymbolicTree(maxDepth, currentDepth + 1, terminalsMarks, nonTerminals,  mathOperationsArity);
            }
            
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
        public void GenerateGrowSymbolicTree(int maxDepth, int currentDepth, string[] terminalsMarks, string[] nonTerminals, Dictionary<string, int> mathOperationsArity)
        {
            var rand = new Random();

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

            // Choose randomly if this node should be terminal or non-terminal.
            bool chooseNonTerminal = rand.Next(100) < 50 ? true : false;

            // If this node will be non-terminal (math.op) 
            // OR if it choosen that this node should be terminal, but we are at root node 
            // (since root node has to be non-terminal)
            // make this node non-terminal
            if( (!chooseNonTerminal && currentDepth == 0) || chooseNonTerminal )
            {
                // Set content of current inner node. 
                this._type = "non-terminal";
                this._depth = currentDepth;
                int indexOfNonTerminal = rand.Next(nonTerminals.Length);
                this._content = nonTerminals[indexOfNonTerminal];

                // Get mathematical operation arity.
                int ar = getMathOperationArity(mathOperationsArity, this._content);

                // If arity is not 1, choose random aritiy between 2 and 3.
                if (ar != 1)
                {
                    // Since operations with arity different from 1 are +, -, *, / that all have arity >= 2.
                    // For simplicity, here are considered only operations with arity 2 and 3.
                    ar = rand.Next(2, 3); // TODO: for now, aritiy can only be 2
                }

                // Set node arity.
                this._arity = ar;

                // Allocate memory for child nodes and recurive set contents of those nodes.
                this._children = new SymbolicTreeNode[this._arity];

                for (var i = 0; i < this._arity; i++)
                {
                    this._children[i] = new SymbolicTreeNode();
                    this._children[i].GenerateGrowSymbolicTree(maxDepth, currentDepth + 1, terminalsMarks, nonTerminals, mathOperationsArity);
                }

                // Done.
                return;

            }

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

        }

        /// <summary>
        /// Helper method for determinating aritiy of mathematical operators.
        /// </summary>
        /// <param name="mathOp">Mathematical operator string.</param>
        /// <returns>Aritiy of the sent mathematical operator.</returns>
        private int getMathOperationArity(Dictionary<string, int> mathOperationsArity, string mathOperation)
        {
            if (mathOperationsArity.ContainsKey(mathOperation))
            {
                return mathOperationsArity[mathOperation];
            }
            else
            {
                throw new Exception("Sent mathematical operator is not knows.");
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
                    for (int i = node.children.Length - 1; i >= 0; i--)
                    {
                        dataTree.Push(node.children[i]);
                    }
                }
            }
        }
        #endregion

        #region Create new Symbolic Tree using current tree <- in crossover
        /// <summary>
        /// Creates new SymbolicTree from current tree and tree sent as parameter. 
        /// </summary>
        /// <param name="crossoverPoint1"> Index of node in current tree that needs to be changed. </param>
        /// <param name="t2"> Subtree to be copied at new node at position crossoverPoint1. </param>
        /// <returns>New SymbolicTree.</returns>
        public SymbolicTreeNode CreateUsing(int crossoverPoint1, SymbolicTreeNode st2)
        {
            // Make new Symbolic tree whose root node will be 'child'.
            SymbolicTreeNode child = new SymbolicTreeNode();

            // Make sure variables _treeQueue, _treeQueueClone and _treeQueueParent are empty.
            _treeQueue.Value.Clear();
            _treeQueueClone.Value.Clear();
            _treeQueueParent1.Value.Clear();
            _treeQueueParent2.Value.Clear();

            // Queues made of Tree Nodes.
            Queue<SymbolicTreeNode> childTree = _treeQueue.Value;
            Queue<SymbolicTreeNode> childSubtree = _treeQueueClone.Value;
            Queue<SymbolicTreeNode> parent1Tree = _treeQueueParent1.Value;
            Queue<SymbolicTreeNode> parent2Subtree = _treeQueueParent2.Value;

            // Variables that will represent a current node in (child tree/parent tree) the loop 
            // through all child nodes of the parents trees.
            SymbolicTreeNode child_node = null;
            SymbolicTreeNode parent1_node = null;
            SymbolicTreeNode parent2_node = null;

            // Add root node at queue.
            childTree.Enqueue(child);

            // Add root node at queue.
            parent1Tree.Enqueue(this);

            // Add root node at queue.
            parent2Subtree.Enqueue(st2);

            // Loop through all nodes in parent1Tree.
            while (parent1Tree.Count > 0)
            {
                // Get next nodes from the beginning of the queue.
                child_node = childTree.Dequeue();
                parent1_node = parent1Tree.Dequeue();

                if( parent1_node.index == crossoverPoint1)
                {
                    // If this node is crossoverPoint, change that subtree from a second parent.
                    // that is, copy subtree st2 to this node.

                    // Helper variable for looping through nodes.
                    SymbolicTreeNode child_subtree_node = null;

                    // Add root node at queue for new node subtree.
                    childSubtree.Enqueue(child_node);

                    // Depth of root node of subtree in second parent.
                    // Will be needed for calculating correct depth of new node in child tree.
                    int initDepthParent2Subtree = st2.depth;

                    // Loop through all nodes in parent2Subtree.
                    while (parent2Subtree.Count > 0)
                    {
                        // Get next nodes from the beginning of the queue.
                        parent2_node = parent2Subtree.Dequeue();
                        child_subtree_node = childSubtree.Dequeue();

                        // Copy values from the parent subtree node to the new child node.
                        child_subtree_node.CopyNodeDescription(parent2_node);

                        // Set correct depth.
                        // Node from first parent has some depth. To that depth we need to add depth of the node from the 
                        // subtree we are currently adding. That depth is calculated: (parent2_node.depth - initDepthParent2Subtree)
                        // since nodes from subtree st2 have depth values like they are still part of the second parent.
                        child_subtree_node.depth = parent1_node.depth + (parent2_node.depth - initDepthParent2Subtree) ;

                        // In case parent subtree node has no children.
                        child_subtree_node.children = null;

                        // If the parent subtree node has children nodes, add children nodes from the parent node to the 
                        // parent2Subtree and for every children make new children node for child and add it to the childSubtree.
                        if (parent2_node.children != null)
                        {
                            // Child node will have same number of children nodes as parent subtree node.
                            child_subtree_node.children = new SymbolicTreeNode[parent2_node.children.Length];

                            // Make new children nodes for child node.
                            for (int i = 0; i < parent2_node.children.Length; i++)
                            {
                                child_subtree_node.children[i] = new SymbolicTreeNode();
                                parent2Subtree.Enqueue(parent2_node.children[i]);
                                childSubtree.Enqueue(child_subtree_node.children[i]);
                            }
                        }
                    }

                    // Now we copied subtree st2 to node child_node. 
                    // Skip this (main) iteration of loop.
                    continue;

                }

                // Copy values from the parent node to the new child node.
                child_node.CopyNodeDescription(parent1_node);

                // In case parent node has no children.
                child_node.children = null;

                // If the parent node has children nodes, add children nodes from the parent node to the 
                // parent1Tree and for every children make new children node for child and add it to the childTree.
                if (parent1_node.children != null)
                {
                    // Child node will have same number of children nodes as parent node.
                    child_node.children = new SymbolicTreeNode[parent1_node.children.Length];

                    // Make new children nodes for child node.
                    for (int i = 0; i < parent1_node.children.Length; i++)
                    {
                        child_node.children[i] = new SymbolicTreeNode();
                        parent1Tree.Enqueue(parent1_node.children[i]);
                        childTree.Enqueue(child_node.children[i]);
                    }
                }
            }

            // Calculate new indices.
            child.CalculateIndices();

            return child; 
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
                this._symbolicTree = value.Clone();
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
        /// <param name="st">Model whose values will be deep copied to this model.</param>
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
            c.symbolicTree = this.symbolicTree;

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
        public static bool operator ==(Chromosome obj1, Chromosome obj2)
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
        public static bool operator !=(Chromosome obj1, Chromosome obj2)
        {
            return !(obj1 == obj2);
        }

        /// <summary>
        /// Overloading Equals.
        /// </summary>
        /// <param name="other">Chromosome to compare current chromosome.</param>
        /// <returns>True if chromosomes are equal, otherwise false. </returns>
        public bool Equals(Chromosome other)
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
        /// <returns>Fintess of the model.</returns>
        public void CalculateFitness(DataTable data)
        {
            int TP = 0;
            int TN = 0;
            int NumberOfFiles = data.Rows.Count;

            foreach(DataRow row in data.Rows)
            {
                double evaluation = this._symbolicTree.Evaluate(row);
                int classification = Convert.ToInt32(row["label"]);

                if( evaluation > 0 && classification == 1)
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

            this._fitness = (TP + TN) / NumberOfFiles;
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
        /// <param name="terminals"> Array of possible terminals of to-be generated symbolic tree.</param>
        /// <param name="nonTerminals"> Array of possible terminals of to-be generated symbolic tree. </param>
        /// <returns></returns>
        public void Generate(string method, int maxDepth, string[] terminalsMarks, DataTable terminals, string[] nonTerminals, Dictionary<string, int> mathOperationsArity)
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
                    throw new System.ArgumentException("Method is not supported", "method");
            }
        }

        #endregion

        #region Accuracy of the model
        /// <summary>
        /// Calculate accuracy of the model (symbolic tree) on test data sent in DataTable.
        /// </summary>
        /// <param name="data"> DataTable representing test data. </param>
        /// <returns>Accuracy of the model.</returns>
        public double Accuracy(DataTable data)
        {
            int correct = 0;
            foreach(DataRow row in data.Rows)
            {
                if( this._symbolicTree.Evaluate(row) == Convert.ToInt32(row["classification"]))
                {
                    correct++;
                }
            }
            return correct / data.Rows.Count;
        }
        #endregion

        #endregion
    }
    #endregion
}
