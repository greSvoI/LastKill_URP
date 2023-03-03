using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastKill
{
    public class Strafe : AbstractAbilityState
    {
        [Header("Speed")]
        [SerializeField] private float _strafeWalkSpeed = 2f;

        [Header("Animation Parametrs")]
        [SerializeField] private string s_strafeAnimState = "Strafe";
        [SerializeField] private string s_strafeAnimBool = "isStrafe";
        [SerializeField] private int layerIndex;

        private int hashAnimBool;
        private int hashAnimState;

        private void Awake()
        {
            hashAnimState = Animator.StringToHash(s_strafeAnimState);
            hashAnimBool = Animator.StringToHash(s_strafeAnimBool);
        }
        public override void OnStartState()
        {
            _animator.Animator.SetBool(hashAnimBool, true);
            _animator.SetAnimationState(hashAnimState,0);
        }

        public override bool ReadyToStart()
        {
            return _move.IsGrounded() && _animator.isAiming;
        }

        public override void UpdateState()
        {
            _move.Move(_input.Move,_input.Crouch ? 1f: _strafeWalkSpeed, false);
            transform.rotation = Quaternion.Euler(0, _camera.GetTransform.transform.eulerAngles.y, 0);

            // update to animator controller
            _animator.StrafeUpdate();

            if (!_weapon.WithWeapon || !_move.IsGrounded())
            {
                _animator.Animator.SetBool(hashAnimBool, true);
                StopState();
            }
        }
        public override void OnStopState()
        {
            base.OnStopState();
            _animator.Animator.SetBool(hashAnimBool, false);
        }
    }
}

