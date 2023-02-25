using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LastKill
{
    public class JumpAndFalling : AbstractAbilityState
    {
        [Header("Animation State")]
        [SerializeField] private string animJumpState = "Air.Jump";
        [SerializeField] private string animFallState = "Air.Falling";
        [SerializeField] private string animHardLandState = "Air.HardLand";
        [SerializeField] private string animSoftLandState = "Air.SoftLand";
        [SerializeField] private string animDeathState = "Air.Death";
        [SerializeField] private string _playAnimState;
        [Header("Jump parameters")]
        [SerializeField] private float jumpHeight = 1.2f;
        [SerializeField] private float speedOnAir = 6f;
        [SerializeField] private float airControl = 0.5f;
        [Header("Landing")]
        [SerializeField] private float heightForSoftLand = 2f;
        [SerializeField] private float heightForHardLand = 4f;
        [SerializeField] private float heightForKillOnLand = 7f;
        [Header("Sound FX")]
        [SerializeField] private AudioClip jumpEffect;
        [SerializeField] private AudioClip _hardLandClip;
        [SerializeField] private AudioClip _softLandClip;
        [SerializeField] private AudioClip _deathClip;
        [Header("Event")]
        [SerializeField] private UnityEvent OnLanded = null;

        CameraController _cameraController;
        AudioController _audioController;

        private float _startSpeed;
        private Vector2 _startInput;
       
        private Vector2 _inputVel;
        private float _angleVel;

        private float _targetRotation;

        // vars to control landing
        private float _highestPosition = 0;
        private bool _landing = false;

        private void Awake()
        {
            _cameraController = GetComponent<CameraController>();
            _audioController = GetComponent<AudioController>();
        }
        public override void OnStartState()
        {
            
            nameState.text = "JumpAndFalling";

            _startInput = _input.Move;
            _targetRotation = _cameraController.MainCamera.eulerAngles.y;

            if (_input.Jump && _move.IsGrounded())
                PerformJump();
            else
            {
                SetAnimationState(animFallState, 0.25f);
                _startSpeed = Vector3.Scale(_move.GetVelocity(), new Vector3(1, 0, 1)).magnitude;

                  _startInput.x = Vector3.Dot(_cameraController.MainCamera.right, transform.forward);
                  _startInput.y = Vector3.Dot(Vector3.Scale(_cameraController.MainCamera.forward, new Vector3(1, 0, 1)), transform.forward);

                //_startInput = _cameraController.GetCameraDirection();

                if (_startSpeed > 3.5f)
                    _startSpeed = speedOnAir;
            }

            _highestPosition = transform.position.y;
            _landing = false;

        }

        private void PerformJump()
        {
            Vector3 velocity = _move.GetVelocity();
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * _move.GetGravity());

            _move.SetVelocity(velocity);
            _animator.CrossFadeInFixedTime(animJumpState, 0.1f);
            _startSpeed = speedOnAir;

            if (_startInput.magnitude > 0.1f)
                _startInput.Normalize();

            if (_audioController)
                _audioController.PlayVoice(jumpEffect);
        }

        public override bool ReadyToStart()
        {
            return !_move.IsGrounded() || _input.Jump;
        }

        //public override void UpdateState()
        //{
        //    if(_landing)
        //    {
        //        if (HasFinishedAnimation(animHardLandState))
        //            StopState();
        //        return;
        //    }

        //    if(_move.IsGrounded())
        //    {
        //        SetAnimationState(animHardLandState, 0.02f);
        //        _landing = true;

        //    }
        //    _move.Move(_startInput, _startSpeed, false);

        //}
        public override void UpdateState()
        {
            if (_landing)
            {
                // apply root motion
                _move.ApplyRootMotion(Vector3.one, false);

                // wait animation finish
                if (HasFinishedAnimation(_playAnimState))
                    StopState();

                return;
            }

            if (_move.IsGrounded())
            {
                if (_highestPosition - transform.position.y >= heightForKillOnLand)
                {
                    LandingSoft(true, animDeathState, _deathClip);
                    _input.Died?.Invoke(); 
                    return;
                }
                else if (_highestPosition - transform.position.y >= heightForHardLand)
                {
                    LandingSoft(true, animHardLandState, _hardLandClip);
                    return;
                }
                else if(_highestPosition - transform.position.y >= heightForSoftLand)
                {
                    LandingSoft(true, animSoftLandState, _softLandClip);
                    return;
                }
                StopState();
                
            }

            if (transform.position.y > _highestPosition)
                _highestPosition = transform.position.y;

        }

        private void LandingSoft(bool landing, string animState, AudioClip clipLanding)
        {
            _landing = landing;
            _playAnimState = animState;
            SetAnimationState(_playAnimState, 0.02f);
            // call event
            OnLanded.Invoke();
            _audioController.PlayVoice(clipLanding);
        }

        private void RotateCharacter()
        {
            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving
            if (_input.Move != Vector2.zero)
            {
                _targetRotation = Mathf.Atan2(_startInput.x, _startInput.y) * Mathf.Rad2Deg + _cameraController.MainCamera.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _angleVel, airControl);

                // rotate to face input direction relative to camera position
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }

        }

        public override void OnStopState()
        {
            base.OnStopState();

            if (_move.IsGrounded() && !_landing && _move.GetVelocity().y < -3f)
                OnLanded.Invoke();

            _landing = false;
            _highestPosition = 0;
            _move.StopRootMotion();
        }

    }
}
