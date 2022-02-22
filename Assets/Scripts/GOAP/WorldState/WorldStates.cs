using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LudoGoap
{
    public class WorldStates : BaseStates
    {

        private GoapController gc_agent;
        private PlayerController pc_player;
        private ShinyKey sk_key;
        private Transform t_goalPos;

        public WorldStates(GoapController _agent, PlayerController _player, ShinyKey _key, Transform _goal)
        {
            bA_state = new bool?[GoapBlackboard.STATELENGTH];
            gc_agent = _agent;
            pc_player = _player;
            sk_key = _key;
            t_goalPos = _goal;
        }

        /// <summary>
        /// Discritizing the "world state" is rough
        /// </summary>
        public void UpdateWorldState()
        {
            // Check each bit position
            for (int i = 0; i < bA_state.Length; i++)
            {
                // Update bit positions based on context
                bA_state[i] = i switch
                {
                    (int)EWorldStateBitPositions.PathToExit => CheckPathToExit(),
                    (int)EWorldStateBitPositions.PlayerRequest => CheckPlayerRequest(),
                    (int)EWorldStateBitPositions.KeyObtained => CheckKeyObtained(),
                    (int)EWorldStateBitPositions.PathToKey => CheckPathToKey(),
                    (int)EWorldStateBitPositions.ThroughDoor => CheckThroughDoor(),
                    _ => false
                };
            }
            LogStates();
        }

        #region WorldState Checks
        // Is door open?
        private bool CheckPathToExit()
        {
            return gc_agent.CheckPathTo(t_goalPos);
        }
        // Player's requesting to co-operate?
        private bool CheckPlayerRequest()
        {
            return false;
        }
        // Check key got
        private bool CheckKeyObtained()
        {
            return sk_key.Obtained;
        }
        // Agent can get to key
        private bool CheckPathToKey()
        {
            return gc_agent.CheckPathTo(sk_key.transform);
        }
        // Win
        private bool CheckThroughDoor()
        {
            return pc_player.transform.position == t_goalPos.position;
        }
        #endregion
    }

    public enum EWorldStateBitPositions
    {
        PathToExit,
        PlayerRequest,
        KeyObtained,
        PathToKey,
        ThroughDoor,
    }

}