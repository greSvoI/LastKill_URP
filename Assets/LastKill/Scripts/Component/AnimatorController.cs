using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastKill
{
    public class AnimatorController : MonoBehaviour, IAnimator
    {
        [Header("Animator Prametrs s string")]
        [SerializeField] private string s_horizontalAnimation = "Horizontal";
        [SerializeField] private string s_verticalAnimation = "Vertical";
        [SerializeField] private string s_moveAmount = "MoveAmount";
        [SerializeField] private string s_isStrafe = "isStrafe";
        [SerializeField] private string s_isAiming = "isAiming";
        [SerializeField] private string s_isCrouch = "isCrouch";

        private int hashHorizontal;
        private int hashVertical;
        private int hashIsStrafe;
        private int hashIsAiming;
        private int hashMoveAmount;
        private int hashIsCrouch;

        private PlayerInput _input;
        private Animator _animator;
        public bool isAiming => _animator.GetBool(hashIsAiming);
        public Animator Animator { get => _animator; }

        private void Start()
        {
            _input = GetComponent<PlayerInput>();
            _animator = GetComponent<Animator>();
            AssignAnimationIDs();
        }

        private void AssignAnimationIDs()
        {
            hashHorizontal = Animator.StringToHash(s_horizontalAnimation);
            hashVertical = Animator.StringToHash(s_verticalAnimation);
            hashIsAiming = Animator.StringToHash(s_isAiming);
            hashIsStrafe = Animator.StringToHash(s_isStrafe);
            hashMoveAmount = Animator.StringToHash(s_moveAmount);
            hashIsCrouch = Animator.StringToHash(s_isCrouch);
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
            _animator.SetBool(hashIsCrouch, _input.Crouch);
            _animator.SetFloat(hashHorizontal, _input.Move.x, 0.1f, Time.deltaTime);
            _animator.SetFloat(hashVertical, _input.Move.y, 0.1f, Time.deltaTime);
            float moveAmount = _input.MoveAmount;
            if (moveAmount < 0.1f) moveAmount = 0f;
            _animator.SetFloat(hashMoveAmount, moveAmount, 0.1f,Time.deltaTime);
        }
        public void LocomotionUpdate()
        {

        }
        public void CrouchUpdate()
        {

        }
        public void CrawlUpdate()
        {

        }
        public void LadderUpdate()
        {
            
        }
    }
}
