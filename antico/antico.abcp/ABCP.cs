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
    class ABCP
    {
        #region ATTRIBUTES 

        #region parametars
        // Class of parameters needed for the ABC programming.
        Parameters _parameters;

        // Property for the _parameters variable.
        public Parameters parameters
        {
            get { return _parameters; }
            set { _parameters.Clone(value); }
        }
        #endregion

        #region models <-> population 
        // Variable that represents class with models - population.
        private Population _Population;

        // Property for the _population variable.
        public Population Population
        {
            get { return _Population; }
            set { _Population.Clone(value); }
        }
        #endregion

        #endregion

        #region OPERATIONS

        #endregion
    }
}
