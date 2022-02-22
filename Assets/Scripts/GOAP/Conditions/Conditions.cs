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
        public Conditions(bool?[] _newConditions, List<int> _newIndicies)
        {
            int iter = 0;
            bA_conditions = new bool?[GoapBlackboard.STATELENGTH];
            iL_conditionIndicies = _newIndicies;
            foreach (int ind in iL_conditionIndicies)
            {
                bA_conditions[ind] = _newConditions[iter];
                iter++;
            }
        }
        public Conditions(bool?[] _newConditions)
        {
            bA_conditions = _newConditions;
        }

        public static int operator -(Conditions a, Conditions b)
        {
            return 0;
        }

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