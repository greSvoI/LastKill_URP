using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastKill
{
    public interface IMove 
    {
        void Move(Vector2 moveInput, float targetSpeed, bool rotateCharacter = true);
        bool IsOnGround();
    }
}
