using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LudoGoap
{
    public class Node
    {
        protected int i_gCost;
        protected int i_hCost;
        protected Node n_parent;
        public int GCost { get { return i_gCost; } }
        public int HCost { get { return i_hCost; } }
        public int FCost { get { return i_gCost + i_hCost; } }
        public Node Parent { get { return n_parent; } }

        public Node(int _gCost, int _hCost)
        {
            i_gCost = _gCost;
            i_hCost = _hCost;
        }

        public void UpdateCosts(int _gCost, int _hCost)
        {
            i_gCost = _gCost;
            i_hCost = _hCost;
        }
        public void ChangeParent(Node _parent)
        {
            n_parent = _parent;
        }
    }
}