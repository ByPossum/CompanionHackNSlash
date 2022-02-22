using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LudoGoap;

public class SingleThreadedAStar : MonoBehaviour
{
    private List<GoapNode> gnA_initialSet = new List<GoapNode>();
    private List<GoapNode> gnA_openSet = new List<GoapNode>();
    private List<GoapNode> gnA_closedSet = new List<GoapNode>();
    private List<GoapNode> gnA_path = new List<GoapNode>();
    private GoapNode gn_targetNode;
    private bool b_searchRunning = false;
    private bool b_searchDone = false;
    public bool Running { get { return b_searchRunning; } }
    // Start is called before the first frame update
    public void Init(GoapNode[] _actions, GoapNode _target, GoapNode _lastAction)
    {
        gnA_openSet = new List<GoapNode>(_actions);
        gnA_initialSet = gnA_openSet;
        gn_targetNode = _target;
        Search(_lastAction);
    }

    private void Search(GoapNode _lastAction)
    {
        if (!b_searchRunning)
        {
            b_searchRunning = true;
            StartCoroutine(RunSearch(_lastAction));
        }
    }

    public List<GoapNode> GetPath()
    {
        if (b_searchRunning)
            return null;
        if (b_searchDone)
        {
            b_searchDone = b_searchRunning = false;
            return gnA_path;
        }
        return null;
    }

    private IEnumerator TracePath(GoapNode _startNode, GoapNode _endNode)
    {
        List<GoapNode> path = new List<GoapNode>();
        GoapNode currentNode = _endNode;
        while(currentNode != _startNode)
        {
            path.Add(currentNode);
            currentNode = (GoapNode)currentNode.Parent;
        }
        path.Reverse();
        b_searchDone = true;
        yield return new WaitForEndOfFrame();
    }

    private IEnumerator RunSearch(GoapNode _startNode)
    {
        // The "Start" node doens't really exist, so the last action taken is used instead
        gnA_openSet.Add(_startNode);
        while(gnA_openSet.Count > 0)
        {
            // Getting the lowest cost node
            GoapNode currentNode = gnA_openSet[0];
            for (int i = 1; i < gnA_openSet.Count; i++)
            {
                if (gnA_openSet[i].FCost < currentNode.FCost || gnA_openSet[i].FCost == currentNode.FCost && gnA_openSet[i].HCost < currentNode.HCost)
                    currentNode = gnA_openSet[i];
            }
            
            gnA_openSet.Remove(currentNode);
            gnA_closedSet.Add(currentNode);
            
            if (Conditions.Evaluate(currentNode.PostConditions.BitConditions, gn_targetNode.PreConditions.BitConditions))
            {
                // Currently at the target node. Retrace path
                StartCoroutine(TracePath(_startNode, gn_targetNode));
                break;
            }
            // Building the tree
            List<GoapNode> potentialNeighbours = gnA_initialSet;
            foreach (GoapNode nodeInSet in gnA_closedSet)
                potentialNeighbours.Remove(nodeInSet);
            // Storing the neighbours in the node itself
            currentNode.InitConnections(potentialNeighbours.ToArray());

            foreach(GoapNode neighbour in currentNode.Neighbours)
            {

                int neighbourCost = currentNode.GCost + GoapNode.GetDitanceFromPreToPost(currentNode, neighbour);
                if(neighbourCost < neighbour.GCost || !gnA_openSet.Contains(neighbour))
                {
                    neighbour.UpdateCosts(neighbourCost, GoapNode.GetDistanceFromPostConditions(neighbour, gn_targetNode));
                    neighbour.ChangeParent(currentNode);
                }
            }
            yield return new WaitForEndOfFrame();
        }
        b_searchRunning = false;
    }
}
