using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LudoGoap;

namespace LudoJobs
{
    public class ThreadConductor : MonoBehaviour
    {
        private ThreadedJob[] tsA_jobs;
        private Queue<MonoBehaviour> oQ_threadObservers = new Queue<MonoBehaviour>();
        private bool b_runThreads = true;
        // Start is called before the first frame update
        public void Init<T>() where T : ThreadedJob
        {
            tsA_jobs = new T[5];
        }

        // Update is called once per frame
        void Update()
        {
            if (b_runThreads)
            {
                foreach(ThreadedJob job in tsA_jobs)
                {
                    if (job.IsDone)
                        job.Abort();
                }
            }
        }

        private void OnApplicationQuit()
        {
            b_runThreads = false;
        }
    }

    public enum EThreadNames
    {
        AStar,
    }

}