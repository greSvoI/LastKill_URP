using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastKill
{
    public class StepController : MonoBehaviour
    {
        [SerializeField] private AudioClip[] _footStepGrass;
        [SerializeField] private AudioClip[] _footStepAsphalt;
        [SerializeField] private AudioClip[] _footStepMetall;
        [SerializeField] private AudioClip[] _footStepGravel;

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log(collision.gameObject.name);
        }
        //Need to rewrite to load from resources
        //private void OnTriggerStay(Collider other)
        //{
        //    Debug.Log("Trigger Stay "+other.gameObject.layer);
        //}
        //private void OnTriggerEnter(Collider other)
        //{
        //    Debug.Log("Triger Enter " + other.gameObject.layer);
        //}
    }
}
