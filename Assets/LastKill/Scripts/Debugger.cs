using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastKill
{
    public class Debugger : MonoBehaviour
    {
        
       
        private void Awake()
        {
           
        }

        private void OnStart(AbstractAbilityState state)
        {
           
            Debug.Log("Start");
        }
    }

}