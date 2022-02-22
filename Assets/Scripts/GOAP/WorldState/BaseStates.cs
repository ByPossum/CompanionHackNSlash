using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace LudoGoap
{
    [System.Serializable]
    public abstract class BaseStates : IStates
    {

        [SerializeField] protected bool?[] bA_state;

        public bool?[] GetState()
        {
            return bA_state;
        }

        public string LogStates()
        {
            return LogStates("");
        }

        public string LogStates(string _message)
        {
          string logger = _message + " ";
            foreach (bool? state in bA_state)
                logger += state + " ";
            Debug.Log(logger);
            return logger;
        }
    }
}