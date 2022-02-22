using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LudoJobs;
using LudoGoap;

public class AStar : ThreadedJob
{
    GoapNode[] gn_openSet;
    GoapNode[] gn_closedSet;
    // Start is called before the first frame update
    public void Init()
    {
        
    }

    // Update is called once per frame
    public override bool Update()
    {
        return true;
    }
}
