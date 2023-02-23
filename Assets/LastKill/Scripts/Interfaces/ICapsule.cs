using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastKill
{
    public interface ICapsule
    {
        void SetCapsuleSize(float newHeight, float newRadius);
        void ResetCapsuleSize();
        float GetCapsuleHeight();
        float GetCapsuleRadius();

        void EnableCollision();
        void DisableCollision();
    }
}
