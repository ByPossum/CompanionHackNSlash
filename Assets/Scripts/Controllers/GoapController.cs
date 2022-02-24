using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LudoGoap;
using UnityEngine.AI;

public class GoapController : BaseInput
{
    private bool b_planning = true;
    private int i_stateParameter;
    private Rigidbody rb;
    private IStates Is_worldState;
    private IStates Is_agentState;
    private GoapNode[] gA_possibleActions;
    private GoapNode[] gA_currentActions;
    private GoapNode gA_lastAction;
    private SingleThreadedAStar as_planner;
    [SerializeField] private AgentType at_type;
    [SerializeField] private GoalState gs_goal;
    [SerializeField] private List<EWorldStateBitPositions> L_goalPositions = new List<EWorldStateBitPositions>();
    [SerializeField] private List<bool> L_goalStates = new List<bool>();
    [SerializeField] private float f_speed;

    private NavMeshPath nmp_checkingPath;
    private NavMeshPath nmp_followingPath;

    public AgentType TypeOfAgent { get { return at_type; } }
    /// <summary>
    /// Initialize agent, world state, actions and goal state
    /// </summary>
    /// <param name="_world">Object that manages the worldstate</param>
    /// <param name="_agent">Current Agent State</param>
    /// <param name="_possibleActions">Actions to be taken</param>
    public void Init(IStates _world, IStates _agent, GoapNode[] _possibleActions)
    {
        Is_worldState = _world;
        Is_agentState = _agent;
        // Obtain possible actions
        gA_possibleActions = _possibleActions;
        // Arbitrarily assign a last action NB Could randomise
        gA_lastAction = _possibleActions[0];
        rb = GetComponent<Rigidbody>() ?? gameObject.AddComponent<Rigidbody>();
        // Goal State is serialized
        gs_goal = new GoalState(L_goalStates, L_goalPositions.ConvertAll<int>(new System.Converter<EWorldStateBitPositions, int>(GoalState.WorldStateToInt)));
        as_planner = GetComponent<SingleThreadedAStar>() ?? gameObject.AddComponent<SingleThreadedAStar>();

    }

    // Update is called once per frame
    void Update()
    {
        // If planner is running, wait.
        if (b_planning && !as_planner.Running)
        {
            Plan();
        }
        // if Planner isn't running, follow path
        else if(gA_currentActions != null)
        {
            if(gA_currentActions.Length > 0)
            {
                gA_lastAction = gA_currentActions[0];
                gA_lastAction.PerformAction();
                //Debug.Log(gA_lastAction.PerformAction());
            }
        }
    }

    private async void Plan()
    {
        // Assing target node
        Conditions targetConditions = new Conditions(gs_goal.GetState());
        GoapNode targetNode = new GoapNode(0, 0, 0, targetConditions, null, null);
        // Initialize planner and await for results
        as_planner.Init(gA_possibleActions, targetNode, gA_lastAction);
        await as_planner.Search(gA_lastAction);
        // If a path is obtained, assign it as the path to follow
        if (as_planner.GetPath() != null)
        {
            gA_currentActions = as_planner.GetPath().ToArray();
            Debug.Log("Path Obtained");
            b_planning = false;
        }
    }
    
    /// <summary>
    /// Used when updating worldstate from this agents perspective
    /// </summary>
    /// <param name="_target">The object we're checking a path towards</param>
    /// <returns>If a path is available</returns>
    public bool CheckPathTo(Transform _target)
    {
        nmp_checkingPath = new NavMeshPath();
        return NavMesh.CalculatePath(transform.position, _target.position, NavMesh.AllAreas, nmp_checkingPath);
    }
    #region Action Delegates
    public void MoveTo(GameObject _target)
    {
        if(nmp_followingPath == null)
        {
            nmp_followingPath = new NavMeshPath();
            NavMesh.CalculatePath(transform.position, _target.transform.position, NavMesh.AllAreas, nmp_followingPath);
        }
        rb.velocity = (transform.position - nmp_followingPath.corners[0]).normalized * f_speed;
    }

    public void Jump(GameObject _target)
    {
        transform.position = _target.transform.position;
    }

    public void MoveToPlayer(GameObject _target)
    {
        if(nmp_followingPath == null)
        {
            nmp_followingPath = new NavMeshPath();
            NavMesh.CalculatePath(transform.position, _target.transform.position, NavMesh.AllAreas, nmp_followingPath);
        }
        rb.velocity = (transform.position - nmp_followingPath.corners[0]).normalized * f_speed;
    }
    #endregion
}
