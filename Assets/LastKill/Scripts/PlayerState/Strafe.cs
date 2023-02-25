using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastKill
{
    public class Strafe : AbstractAbilityState
    {
        [SerializeField] private float _strafeWalkSpeed = 2f;

        [Header("Animation")]
        [SerializeField] private string _strafeAnimState = "Locomotion.Strafe";
        [SerializeField] private string _horizontalAnimation = "Horizontal";
        [SerializeField] private string _verticalAnimation = "Vertical";

        CameraController _cameraController;

        private int _horizontal;
        private int _vertical;
        private void Awake()
        {
            _horizontal = Animator.StringToHash(_horizontalAnimation);
            _vertical = Animator.StringToHash(_verticalAnimation);
            _cameraController = GetComponent<CameraController>();
        }


        public override void OnStartState()
        {
            nameState.text = "Strafe";
            SetAnimationState(_strafeAnimState);
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
            _animator.SetFloat(_horizontal, _input.Move.x, 0.1f, Time.deltaTime);
            _animator.SetFloat(_vertical, _input.Move.y, 0.1f, Time.deltaTime);

            if (!_input.Aim || !_move.IsGrounded())
                StopState();
        }
    }
}

