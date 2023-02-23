using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LastKill
{
    [DisallowMultipleComponent]
    public class Crouch : AbstractAbilityState
    {

        [SerializeField] private LayerMask obstaclesMask;
        [SerializeField] private float capsuleHeightOnCrouch = 1f;
        [SerializeField] private float speed = 3f;

        private float _defaultCapsuleHeight = 0;
        private float _defaultCapsuleRadius = 0;

        public override void OnStartState()
        {
            nameState.text = "Crouch";
            _defaultCapsuleRadius = _capsule.GetCapsuleRadius();
            _defaultCapsuleHeight = _capsule.GetCapsuleHeight();
            _capsule.SetCapsuleSize(capsuleHeightOnCrouch, _capsule.GetCapsuleRadius());
            _move.Move(new Vector3(0, 0.5f, 0));
            SetAnimationState("Crouch.Free Crouch", 0.25f);
        }

        public override void OnStopState()
        {
            _capsule.ResetCapsuleSize();
        }

        public override bool ReadyToStart()
        {
            return _move.IsGrounded() && _input.Crouch;
        }

        public override void UpdateState()
        {
            _move.Move(_input.Move, speed);

            if (!_input.Crouch && !ForceCrouchByHeight())
                StopState();
        }
        private bool ForceCrouchByHeight()
        {
            RaycastHit hit;

            if (Physics.SphereCast(transform.position, _defaultCapsuleRadius, Vector3.up, out hit,
                _defaultCapsuleHeight, obstaclesMask, QueryTriggerInteraction.Ignore))
            {
                if (hit.point.y - transform.position.y > capsuleHeightOnCrouch)
                    return true;
            }

            return false;
        }
    }
}
