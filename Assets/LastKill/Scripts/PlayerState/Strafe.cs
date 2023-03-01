using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastKill
{
    public class Strafe : AbstractAbilityState
    {
        [SerializeField] private float _strafeWalkSpeed = 2f;

        [Header("Animation")]
        [SerializeField] private string strafeAnimState = "Locomotion.Strafe";

        CameraController _cameraController;

        private int hashAnimState;
       
        private void Awake()
        {
            hashAnimState = Animator.StringToHash(strafeAnimState);
            _cameraController = GetComponent<CameraController>();
        }


        public override void OnStartState()
        {
            nameState.text = "Strafe";
            _animator.SetAnimationState(hashAnimState, 0);
        }

        public override bool ReadyToStart()
        {
            return _move.IsGrounded() && _input.Aim;
        }

        public override void UpdateState()
        {
            _move.Move(_input.Move, _strafeWalkSpeed, false);
            transform.rotation = Quaternion.Euler(0, _cameraController.MainCamera.transform.eulerAngles.y, 0);

            // update animator
            _animator.StrafeUpdate();

            if (!_input.Aim || !_move.IsGrounded())
                StopState();
        }
    }
}

