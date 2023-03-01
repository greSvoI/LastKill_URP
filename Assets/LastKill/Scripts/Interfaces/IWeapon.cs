using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastKill
{
    public interface IWeapon 
    {
        public bool WithWeapon { get; }
        public void PutAwayWeapon();
    }
}
