using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LudoJobs;

public class ThreadConductor : MonoBehaviour
{
    WorldStates ws_currentWorldState;
    [SerializeField] private ThreadedJob[] tsA_jobs;
    private Queue<Object> oQ_threadObservers = new Queue<Object>();
    private bool b_runThreads = true;
    // Start is called before the first frame update
    void Start()
    {
        tsA_jobs = new ThreadedJob[1];
        StartCoroutine(UpdateWorldState());
    }

    // Update is called once per frame
    void Update()
    {
        if (tsA_jobs[(int)EThreadNames.worldSensor].IsDone)
        {
            PushWorldStates();
        }
    }

    private void OnApplicationQuit()
    {
        b_runThreads = false;
    }

    public void GetWorldState(Object o_threadChecker)
    {
        if (tsA_jobs[(int)EThreadNames.worldSensor].IsDone)
        {
            //If thread is done immediately update o_threadChecker
        }
        else
        {
            oQ_threadObservers.Enqueue(o_threadChecker);
        }
    }

    private void PushWorldStates()
    {
        if (oQ_threadObservers.Count > 0)
        {
            foreach (Object threadChecker in oQ_threadObservers)
            {
                //threadChecker.
            }
            oQ_threadObservers.Clear();
        }
    }

    private IEnumerator UpdateWorldState()
    {
        ThreadedWorldState tws_sensor = new ThreadedWorldState();
        tsA_jobs[(int)EThreadNames.worldSensor] = tws_sensor;
        tws_sensor.Start();
        yield return StartCoroutine(tws_sensor.WaitFor());
        ws_currentWorldState = tws_sensor.SensorStates;
    }
}

public enum EThreadNames
{
    worldSensor,
}
