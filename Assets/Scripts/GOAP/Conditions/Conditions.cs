using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LudoGoap
{
    public class Conditions
    {
        protected bool?[] bA_conditions;
        protected List<int> iL_conditionIndicies;
        public bool?[] BitConditions { get { return bA_conditions; } }
        public List<int> ConditionIndicies;

        /// <summary>
        /// Explicitly set up the conditions
        /// </summary>
        /// <param name="_newConditions">The boolean states we wish to have</param>
        /// <param name="_newIndicies">Which booleans to set</param>
        public Conditions(bool?[] _newConditions, List<int> _newIndicies)
        {
            // Initialize conditions
            bA_conditions = new bool?[GoapBlackboard.STATELENGTH];
            for (int i = 0; i < GoapBlackboard.STATELENGTH; i++)
                bA_conditions[i] = null;
            // Set only the conditions passed in
            int iter = 0;
            iL_conditionIndicies = _newIndicies;
            foreach (int ind in iL_conditionIndicies)
            {
                bA_conditions[ind] = _newConditions[iter];
                iter++;
            }
        }

        /// <summary>
        /// Assign conditions to a preset array
        /// </summary>
        /// <param name="_newConditions"></param>
        public Conditions(bool?[] _newConditions)
        {
            bA_conditions = _newConditions;
            // Validate the size of passed in conditions
            if(bA_conditions.Length != GoapBlackboard.STATELENGTH)
            {
                Debug.LogError("Conditions not fixed size");
                System.Array.Resize(ref bA_conditions, GoapBlackboard.STATELENGTH);
            }
        }

        /// <summary>
        /// Check if a condition is different
        /// </summary>
        /// <param name="a">A condition to check</param>
        /// <param name="b">Another condition to check</param>
        /// <returns>true when the conditions match</returns>
        public static bool Evaluate(bool?[] a, bool?[] b)
        {
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i])
                    return false;
            }
            return true;
        }
    }

}