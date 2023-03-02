using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastKill
{
    public class AnimatorController : MonoBehaviour, IAnimator
    {
        [SerializeField] private string horizontalAnimation = "Horizontal";
        [SerializeField] private string verticalAnimation = "Vertical";

        private int hashHorizontal;
        private int hashVertical;

        private PlayerInput _input;
        private Animator _animator;
        public Animator Animator { get => _animator; }

        private void Start()
        {

            _input = GetComponent<PlayerInput>();
            _animator = GetComponent<Animator>();
            AssignAnimationIDs();
           
        }

        private void AssignAnimationIDs()
        {
            hashHorizontal = Animator.StringToHash(horizontalAnimation);
            hashVertical = Animator.StringToHash(verticalAnimation);
        }

        public bool HasFinishedAnimation(string stateName, int layerIndex)
        {
            var stateInfo = _animator.GetCurrentAnimatorStateInfo(layerIndex);

            if (_animator.IsInTransition(layerIndex)) return false;

            if (stateInfo.IsName(stateName))
            {
                float normalizeTime = Mathf.Repeat(stateInfo.normalizedTime, 1);
                if (normalizeTime >= 0.95f) return true;
            }
            return false;
        }
        public bool HasFinishedAnimation(int layerIndex)
        {
            return _animator.GetCurrentAnimatorStateInfo(layerIndex).normalizedTime >= 0.95f && !_animator.IsInTransition(layerIndex);
        }
        public void SetAnimationState(int hashName, int layerIndex, float transitionDuration)
        {
            if (_animator.HasState(layerIndex, hashName))
                _animator.CrossFadeInFixedTime(hashName, transitionDuration, layerIndex);
        }

        public void SetAnimationState(string stateName, int layerIndex, float transitionDuration)
        {
            if (_animator.HasState(layerIndex, Animator.StringToHash(stateName)))
                _animator.CrossFadeInFixedTime(stateName, transitionDuration, layerIndex);
        }

        public void StrafeUpdate()
        {
            _animator.SetFloat(hashHorizontal, _input.Move.x, 0.1f, Time.deltaTime);
            _animator.SetFloat(hashVertical, _input.Move.y, 0.1f, Time.deltaTime);

            _animator.SetFloat("MoveAmount", _input.MoveAmount /2);
        }
    }
}
