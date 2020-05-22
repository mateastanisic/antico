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

namespace antico.abcp
{
    internal class Population
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

        #region population
        // Variable that represents population of symbolic trees.
        private SymbolicTree[] _population;

        // Property for the population variable.
        public SymbolicTree[] population
        {
            get { return _population; }
            set 
            {
                // Allocate memory for the new population of models.
                _population = new SymbolicTree[value.Length];

                // Deep copy every model.
                for ( var i = 0; i < value.Length; i++)
                {
                    _population[i].Clone(value[i]);
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
        public SymbolicTree this[int index]
        {
            get
            {
                if (index < 0 && index >= this._population.Length)
                {
                    throw new IndexOutOfRangeException("Index out of range");
                }

                return this._population[index];
            }
            set
            {
                if (index < 0 && index >= this._population.Length)
                {
                    throw new IndexOutOfRangeException("Index out of range");
                }

                this._population[index].Clone(value);
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
            this.population = p.population;
        }
        #endregion

        #region Crossover
        /// <summary>
        /// Crossover of the two chromosomes.
        /// </summary>
        /// <param name="parent1"> First chromosome (parent) that is part of the crossover. </param>
        /// <param name="parent2"> Second chromosome (parent) that is part of the crossover. </param>
        /// <returns> Child chromosome. </returns>
        public SymbolicTree crossover( SymbolicTree parent1, SymbolicTree parent2)
        {
            // TODO
            throw new NotImplementedException();
        }        
        #endregion

        #endregion
    }
}
