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

namespace antico.data
{
    /// <summary>
    /// 
    /// </summary>
    public class Feature
    {
        #region ATTRIBUTES

        #region feature name
        // Variable that represents feature names.
        private string _featureName;

        // Property for the _featureNames variable.
        public string featureName
        {
            get { return _featureName; }
            set { _featureName = value; }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public class Features
    {
        #region ATTRIBUTES

        #region feature names
        // Variable that represents feature names.
        private string[] _featureNames;

        // Property for the _featureNames variable.
        public string[] featureNames
        {
            get { return _featureNames; }
            set
            {
                // Allocate memory.
                _featureNames = new string[value.Length];

                // Deep copy.
                for (var i = 0; i < value.Length; i++)
                {
                    _featureNames[i] = value[i];
                }
            }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public class Data
    {
        #region ATTRIBUTES



        #endregion

        #region mathematical operations
        // Dictionary variable that represents mathematical operations. 
        // Key is the operation name and integer value represents possible arrity of the operation.
        private Dictionary<string, int> _mathOperations;

        // Property for the _mathOperations variable.
        public Dictionary<string, int> mathOperations
        {
            get { return _mathOperations; }
            set
            {
                // Define new dictionary.
                _mathOperations = new Dictionary<string, int>();

                // Deep copy.
                foreach (var item in value)
                {
                    _mathOperations.Add(item.Key, item.Value);
                }
            }
        }
        #endregion

        #region OPERATIONS
        public Data()
        {
            _mathOperations = new Dictionary<string, int>()
            {
                { "+", 2 },
                { "-", 2 },
                { "*", 2 },
                { "/", 2 },
                { "sin", 1 },
                { "cos", 1 },
                { "log", 1 },
                { "exp", 2 }
            };
        }
        #endregion

    }
}
