using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LudoGoap
{

    public class AgentManager : MonoBehaviour
    {
        private WorldStates ws_world;
    
        [SerializeField] private GoapController[] gcA_agents;
        [SerializeField] PlayerController pc_player;
        [SerializeField] ShinyKey sk_key;
        [SerializeField] private Transform t_goalPos;
        [SerializeField] private GameObject go_jumpPoint;
        // Start is called before the first frame update
        void Start()
        {
            // Create the worldstates to manage
            ws_world = new WorldStates(gcA_agents[(int)AgentType.companion], pc_player, sk_key, t_goalPos);
            // Initialize all agents
            for (int i = 0; i < gcA_agents.Length; i++)
            {
                switch (gcA_agents[i].TypeOfAgent)
                {
                    case AgentType.companion:
                        gcA_agents[i].Init(ws_world, null, BuildActions(i));
                        break;
                }
            }
        }

        /// <summary>
        /// Builds the GoapNodes which contain actions. NB With more time I'd manage this via XML or something similar
        /// </summary>
        /// <param name="ind">Which agent to assign actions from</param>
        /// <returns>All of the agents actions</returns>
        private GoapNode[] BuildActions(int ind)
        {
            // Move to key action
            GoapNode[] actions = new GoapNode[3];
            GoapAction moveTo = (GameObject) => gcA_agents[ind].MoveTo(sk_key.gameObject);
            actions[0] = new GoapNode(0, 0, 1,
                new Conditions(new bool?[3] { true, false, false },
                new List<int>() { (int)EWorldStateBitPositions.PathToKey, (int)EWorldStateBitPositions.ThroughDoor, (int)EWorldStateBitPositions.PlayerRequest }),
                new Conditions(new bool?[2] { false, true },
                new List<int>() { (int)EWorldStateBitPositions.PathToKey, (int)EWorldStateBitPositions.KeyObtained }), moveTo);
            actions[0].SetName("MoveTo");
            // Player help request action
            GoapAction playerHelp = (GameObject) => gcA_agents[ind].Jump(pc_player.gameObject);
            actions[1] = new GoapNode(0, 0, 1,
                new Conditions(new bool?[1] { true },
                new List<int>() {(int)EWorldStateBitPositions.PlayerRequest}),
                new Conditions(new bool?[2] { true, false },
                new List<int>() { (int)EWorldStateBitPositions.PathToKey, (int)EWorldStateBitPositions.KeyObtained }), playerHelp);
            actions[1].SetName("PlayerHelp");
            // Move to player action
            GoapAction moveToPlayer = (GameObject) => gcA_agents[ind].MoveTo(pc_player.gameObject);
            actions[2] = new GoapNode(0, 0, 0,
                new Conditions(new bool?[2] { false, true },
                new List<int>() { (int)EWorldStateBitPositions.NearPlayer, (int)EWorldStateBitPositions.PathToPlayer}),
                new Conditions(new bool?[1] { true },
                new List<int>() { (int)EWorldStateBitPositions.NearPlayer }), moveToPlayer);
            actions[2].SetName("MoveToPlayer");
            return actions;
        }

        // Update is called once per frame
        void Update()
        {
            ws_world.UpdateWorldState();
        }
    }

    public enum AgentType
    {
        companion,
    }

}