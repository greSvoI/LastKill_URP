using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastKill
{
    public class Roll : AbstractAbilityState
    {
       
        public override void OnStartState()
        {
        }

        public override bool ReadyToStart()
        {
            return _input.Roll;
        }

        public override void UpdateState()
        {
           
        }
    }
}
