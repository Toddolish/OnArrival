using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//steering behaviour will only work if attached to AIagent
namespace SteeringBehaviours {
    public abstract class SteeringBehaviour : MonoBehaviour
    {
        [Header("Agent Owner")]
        public float weighting; //how much influence the behaviour has over the other Behaviours
        public AIAgent owner; //reference to the AIagent owner of behaviour

        public void Awake()
        {
            owner = GetComponent<AIAgent>();
        }
        public virtual Vector3 GetForce()
        {
            Vector3 force = new Vector3(0, 0, 0);
            return Vector3.zero;
        }
    }
}
