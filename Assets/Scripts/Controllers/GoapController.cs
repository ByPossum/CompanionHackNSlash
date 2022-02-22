using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LudoGoap;
using UnityEngine.AI;

public class GoapController : BaseInput
{
    private bool b_planning = true;
    private int i_stateParameter;
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

    private NavMeshPath nmp_checkingPath;
    private NavMeshPath nmp_followingPath;

    public AgentType TypeOfAgent { get { return at_type; } }
    public void Init(IStates _world, IStates _agent, GoapNode[] _possibleActions)
    {
        Is_worldState = _world;
        Is_agentState = _agent;
        gA_possibleActions = _possibleActions;
        gA_lastAction = _possibleActions[0];
        gs_goal = new GoalState(L_goalStates, L_goalPositions.ConvertAll<int>(new System.Converter<EWorldStateBitPositions, int>(GoalState.WorldStateToInt)));
        as_planner = GetComponent<SingleThreadedAStar>() ?? gameObject.AddComponent<SingleThreadedAStar>();

    }

    // Update is called once per frame
    void Update()
    {
        if (b_planning && !as_planner.Running)
            Plan();
        else
        {
            gA_lastAction = gA_currentActions[0];
            gA_lastAction.PerformAction();
            Debug.Log(gA_lastAction.PerformAction());
        }
    }

    private async void Plan()
    {
        Conditions targetConditions = new Conditions(gs_goal.GetState());
        GoapNode targetNode = new GoapNode(0, 0, 0, targetConditions, null, null);
        as_planner.Init(gA_possibleActions, targetNode, gA_lastAction);
        await as_planner.Search(gA_lastAction);
        if (as_planner.GetPath() != null)
        {
            gA_currentActions = as_planner.GetPath().ToArray();
            b_planning = false;
        }
    }

    public bool CheckPathTo(Transform _target)
    {
        nmp_checkingPath = new NavMeshPath();
        return NavMesh.CalculatePath(transform.position, _target.position, NavMesh.AllAreas, nmp_checkingPath);
    }
    
    public void MoveTo(GameObject _target)
    {
        if(nmp_followingPath == null)
        {
            nmp_followingPath = new NavMeshPath();
            NavMesh.CalculatePath(transform.position, _target.transform.position, NavMesh.AllAreas, nmp_followingPath);
        }
    }

    public void Jump(GameObject _target)
    {
        transform.position = _target.transform.position;
    }
}
