using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

namespace LudoJobs
{
    [System.Serializable]
    public abstract class ThreadedJob
    {
        protected Thread T_thread = null;
        protected bool b_isDone;
        public bool IsDone { get { bool tmp = b_isDone; return tmp; } set { b_isDone = value; } }

        public virtual void Start()
        {
            T_thread = new Thread(Run);
            T_thread.Name = "LudoJob";
            T_thread.Start();
        }

        public virtual void SetAffinity()
        {
            T_thread.Priority = System.Threading.ThreadPriority.Highest;
            T_thread.IsBackground = true;
        }

        public virtual void Abort()
        {
            T_thread.Abort();
        }

        protected virtual void ThreadJob() { }
        protected virtual void OnFinished() { Abort(); }
        public virtual bool Update()
        {
            if (b_isDone)
            {
                OnFinished();
                return true;
            }
            return false;
        }
        protected void Run()
        {
            ThreadJob();
            b_isDone = true;
        }

        public IEnumerator WaitFor()
        {
            while (!Update())
                yield return new WaitForSeconds(4f);
            Debug.Log("done");
        }
    }
}