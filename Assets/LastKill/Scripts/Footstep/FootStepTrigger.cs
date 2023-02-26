using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastKill
{
    public class FootStepTrigger : MonoBehaviour
    {
        protected Collider trigger;

        private void Start()
        {
            trigger = GetComponent<Collider>();
        }
        void OnDrawGizmos()
        {
            if (!trigger) return;
            Color color = Color.green;
            color.a = 0.5f;
            Gizmos.color = color;
            if (trigger is SphereCollider)
            {
                Gizmos.DrawSphere((trigger.bounds.center), (trigger as SphereCollider).radius);
            }
        }
        private void OnTriggerEnter(Collider other)
        {
           
        }
        private void OnTriggerStay(Collider other)
        {
            
        }

    }
}
