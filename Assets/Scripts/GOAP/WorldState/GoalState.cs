using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LudoGoap
{
    public class GoalState : BaseStates
    {
        public GoalState(List<bool> _goals, List<int> _positions)
        {
            bA_state = new bool?[GoapBlackboard.STATELENGTH];
            int iter = 0;
            foreach (int pos in _positions)
            {
                bA_state[pos] = _goals[iter];
                iter++;
            }
        }

        public static int WorldStateToInt(EWorldStateBitPositions _state)
        {
            return (int)_state;
        }
    }

}