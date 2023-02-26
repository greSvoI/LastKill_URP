using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastKill
{
    public class Locomotion : AbstractAbilityState 
    {
        [SerializeField] private float _walkSpeed = 2f;
        [SerializeField] private float _sprintSpeed = 5f;
        [SerializeField] private string _animatorBlendState = "Locomotion.Free Movement";

        private AudioController _audioController;
        //private IMove _move = null;
        private int _animatorIdSpeed;
        private void Awake()
        {
            _audioController = GetComponent<AudioController>();
            _animatorIdSpeed = Animator.StringToHash("Speed");
        }

        public override void OnStartState()
        {
            nameState.text = "Locomotion";
            SetAnimationState(_animatorBlendState, 0.25f);

            if(_input.Move.magnitude < 0.1f)
            {
                // reset movement parameters
                _animator.SetFloat(_animatorIdSpeed, 0f, 0f, Time.deltaTime);
            }
            
        }
        public override void UpdateState()
        {
            float targetSpeed = 0f;

            targetSpeed = _input.Sprint ? _sprintSpeed : _walkSpeed;

            targetSpeed = _input.Move == Vector2.zero ? 0f : targetSpeed;

            _move.Move(_input.Move, targetSpeed);
        }

        public override bool ReadyToStart()
        {
            return _move.IsGrounded();
        }

      
    }
}
