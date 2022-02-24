using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LudoGoap
{
    public class GoapNode : Node
    {
        protected int i_actionCost;
        protected string s_nodeName;
        protected Conditions preConditions;
        protected Conditions postConditions;
        protected GoapAction ga_action;
        protected GoapNode[] gn_connection;
        public string NodeName { get { return s_nodeName; } }
        public Conditions PreConditions { get { return preConditions; } }
        public Conditions PostConditions { get { return postConditions; } }
        public GoapNode[] Neighbours { get { return gn_connection; } }
        public GoapNode(int _gCost, int _hCost, int _actionCost, Conditions _pre, Conditions _post, GoapAction _action) : base(_gCost, _hCost)
        {
            i_gCost = 1000;
            i_hCost = 1000;
            i_actionCost = _actionCost;
            preConditions = _pre;
            postConditions = _post;
            ga_action = _action;
        }

        public void InitConnections(GoapNode[] _connections)
        {
            gn_connection = _connections;
        }

        public void SetName(string _name)
        {
            s_nodeName = _name;
        }

        public GoapAction PerformAction()
        {
            return ga_action;
        }

        #region Distance Functions
        // NB I've opted for explicit functions such as GetDistanceFromPreToPost, which all refer to GetDistanceFromNodes, for clarity when referenced elsewhere

        /// <summary>
        /// Used to calculate the distance between node using the difference in bits
        /// </summary>
        /// <param name="_nodeA">Current node</param>
        /// <param name="_nodeB">Target node</param>
        /// <returns>Distance</returns>
        private static int GetDistanceFromNodes(bool?[] _indsA, bool?[] _indsB)
        {
            // Similarity starts at maximum
            int similarity = GoapBlackboard.STATELENGTH;
            // Take a reductive approach to evaluate the similarity
            for (int i = 0; i < GoapBlackboard.STATELENGTH-1; i++)
                if (_indsA[i] == _indsB[i])
                    similarity--;
            return similarity;
        }

        public static int GetDitanceFromPreToPost(GoapNode _currentNode, GoapNode _targetNode)
        {
            return GetDistanceFromNodes(_currentNode.preConditions.BitConditions, _targetNode.postConditions.BitConditions);
        }

        public static int GetDistanceFromPreConditions(GoapNode _currentNode, GoapNode _targetNode)
        {
            return GetDistanceFromNodes(_currentNode.preConditions.BitConditions, _targetNode.preConditions.BitConditions);
        }


        public static int GetDistanceFromPostConditions(GoapNode _currentNode, GoapNode _targetNode)
        {
            return GetDistanceFromNodes(_currentNode.postConditions.BitConditions, _targetNode.postConditions.BitConditions);
        }

        public static int GetDistanceFromPostToPreConditions(GoapNode _currentNode, GoapNode _targetNode)
        {
            return GetDistanceFromNodes(_currentNode.postConditions.BitConditions, _targetNode.preConditions.BitConditions);
        }
        #endregion
    }

    public delegate void GoapAction(GameObject _target);
}