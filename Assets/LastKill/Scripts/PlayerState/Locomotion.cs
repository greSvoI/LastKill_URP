using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastKill
{
    public class Locomotion : AbstractAbilityState 
    {
        [SerializeField] private float walkSpeed = 2f;
        [SerializeField] private float sprintSpeed = 5f;
        [SerializeField] private string animatorBlendState = "Locomotion";

        private int hashAnimState;

        private int animatorIdSpeed;
        private void Awake()
        {
            animatorIdSpeed = Animator.StringToHash("Speed");
            hashAnimState = Animator.StringToHash(animatorBlendState);
        }

        public override void OnStartState()
        {
            _animator.SetAnimationState(hashAnimState,0, 0.25f);

            if(_input.Move.magnitude < 0.1f)
            {
                // reset movement parameters
                _animator.Animator.SetFloat(animatorIdSpeed, 0f, 0f, Time.deltaTime);
            }
            
        }
        public override void UpdateState()
        {
            float targetSpeed = 0f;

            targetSpeed = _input.Sprint ? walkSpeed : sprintSpeed;

            targetSpeed = _input.Move == Vector2.zero ? 0f : targetSpeed;

            _move.Move(_input.Move, targetSpeed);
        }

        public override bool ReadyToStart()
        {
            return _move.IsGrounded();
        }

      
    }
}
