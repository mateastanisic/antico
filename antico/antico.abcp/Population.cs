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
using System.Data;
using System.Text;

namespace antico.abcp
{
    #region Population class
    internal class Population
    {
        #region ATTRIBUTES 

        #region population size
        // Variable that represents size of population.
        // (READONLY - Setting only through constructor) 
        private int _populationSize;

        // Property for the population_size variable.
        public int populationSize
        {
            get { return _populationSize; }
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
                    _chromosomes[i].Clone(value[i]);
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

                this._chromosomes[index].Clone(value);
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
        public Population(int popSize, int initialMaxDepth, string[] nonTerminals, string[] terminalNames, DataTable terminals, string generatingTreesMethod )
        {
            // Set population size.
            this._populationSize = popSize;

            // Allocate memory.
            this._chromosomes = new Chromosome[popSize];

            // Generate population of chromosomes.
            for( var i = 0; i < popSize; i++)
            {
                // Check depending on symbolic tree generating method.

                if( generatingTreesMethod == "ramped" )
                {
                    if( i < popSize / 2)
                    {
                        _chromosomes[i].Generate("full", initialMaxDepth, terminalNames, terminals, nonTerminals);
                    }
                    else
                    {
                        _chromosomes[i].Generate("grow", initialMaxDepth, terminalNames, terminals, nonTerminals);
                    }
                    
                }
                else
                {
                    _chromosomes[i].Generate(generatingTreesMethod, initialMaxDepth, terminalNames, terminals, nonTerminals);
                }

                
            }
        }
        #endregion

        #region Deep copy
        /// <summary>
        /// Helper method for deep copying values of population of models p
        /// to values of this population of models.
        /// </summary>
        /// <param name="p">Population to be deep copied.</param>
        /// <returns>Population of models with values from Population p.</returns>
        public void Clone(Population p)
        {
            // Associate populationSize of Population p to the populationSize of this class' instance.
            this._populationSize = p.populationSize;

            // Deep copy with variable property set.
            this.chromosomes = p.chromosomes;
        }
        #endregion

        #region Crossover
        /// <summary>
        /// Crossover of the two chromosomes.
        /// </summary>
        /// <param name="parent1"> First chromosome (parent) that is part of the crossover. </param>
        /// <param name="parent2"> Second chromosome (parent) that is part of the crossover. </param>
        /// <returns> Child chromosome. </returns>
        public Chromosome crossover( Chromosome parent1, Chromosome parent2)
        {
            // TODO
            throw new NotImplementedException();
        }        
        #endregion

        #endregion
    }
    #endregion
}
