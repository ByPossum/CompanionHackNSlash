using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LudoJobs;
public class ThreadedWorldState : ThreadedJob
{

    private WorldStates ws_sensorStates;
    public WorldStates SensorStates { get { return ws_sensorStates; } }
    [SerializeField] List<PlayerController> players = new List<PlayerController>();
    public override void Start()
    {
        base.Start();
        foreach(PlayerController pc in GameObject.FindObjectsOfType<PlayerController>())
        {
            players.Add(pc);
        }
    }

    // Update is called once per frame
    public override bool Update()
    {
        return base.Update();
    }

    protected override void ThreadJob()
    {
        GetPointInFrustum();
    }

    protected override void OnFinished()
    {
        Debug.Log("Thread ended");
        base.OnFinished();
    }

    private void GetPointInFrustum()
    {
        float f = Vector3.Distance(Vector3.zero, Vector3.one);
        Vector3 newPoint = new Vector3(1, 0.0f, 1);
        ws_sensorStates.WA_pointInFrustum = new WorldAttribute<Vector3>(true, newPoint);
        Debug.Log("New thread is running");
    }
}
