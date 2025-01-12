using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LastKill
{
    public interface IMove 
    {
        void Move(Vector2 moveInput, float targetSpeed, bool rotateCharacter = true);

        void Move(Vector2 moveInput, float targetSpeed, Quaternion cameraRotation, bool rotateCharacter = true);

        void Move(Vector3 velocity);
        void StopMovement();
        void SetVelocity(Vector3 velocity);
        Vector3 GetVelocity();
        float GetGravity();
        void EnableGravity();
        void DisableGravity();

        void SetPosition(Vector3 newPosition);
        Quaternion GetRotationFromDirection(Vector3 direction);

        bool IsGrounded();
        Collider GetGroundCollider();
        void ApplyRootMotion(Vector3 multiplier, bool applyRotation = false);
        void StopRootMotion();
        Vector3 GetRelativeInput(Vector2 input);
    }
}
