using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LastKill
{
    [DisallowMultipleComponent]
    public class Crouch : AbstractAbilityState
    {
        [SerializeField] private string crouchAnimState = "Crouch.Free Crouch";
        [SerializeField] private LayerMask obstaclesMask;
        [SerializeField] private float capsuleHeightOnCrouch = 1f;
        [SerializeField] private float speed = 3f;

        private float defaultCapsuleHeight = 0;
        private float defaultCapsuleRadius = 0;


        private int hashAnimState;
        private void Awake()
        {
            hashAnimState = Animator.StringToHash(crouchAnimState);
        }
        public override void OnStartState()
        {
            nameState.text = "Crouch";
            defaultCapsuleRadius = _capsule.GetCapsuleRadius();
            defaultCapsuleHeight = _capsule.GetCapsuleHeight();
            _capsule.SetCapsuleSize(capsuleHeightOnCrouch, _capsule.GetCapsuleRadius());
            _move.Move(new Vector3(0, 0.5f, 0));
            _animator.SetAnimationState(hashAnimState,0, 0.25f);
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

            if (Physics.SphereCast(transform.position, defaultCapsuleRadius, Vector3.up, out hit,
                defaultCapsuleHeight, obstaclesMask, QueryTriggerInteraction.Ignore))
            {
                if (hit.point.y - transform.position.y > capsuleHeightOnCrouch)
                    return true;
            }

            return false;
        }
    }
}
