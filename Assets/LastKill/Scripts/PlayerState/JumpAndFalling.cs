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
        [SerializeField] private string playAnimState;
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
        [SerializeField] private AudioClip hardLandClip;
        [SerializeField] private AudioClip softLandClip;
        [SerializeField] private AudioClip deathClip;

        //???
        [Header("Event")]
        [SerializeField] private UnityEvent OnLanded = null;

        CameraController _cameraController;

        private float startSpeed;
        private Vector2 startInput;
       
        private Vector2 inputVel;
        private float angleVel;

        private float targetRotation;

        // vars to control landing
        private float highestPosition = 0;
        private bool landing = false;

        private void Awake()
        {
            _cameraController = GetComponent<CameraController>();
        }
        public override void OnStartState()
        {
            

            startInput = _input.Move;
            targetRotation = _cameraController.MainCamera.eulerAngles.y;

            if (_input.Jump && _move.IsGrounded())
                PerformJump();
            else
            {
                _animator.SetAnimationState(animFallState, 0,0.25f);
                startSpeed = Vector3.Scale(_move.GetVelocity(), new Vector3(1, 0, 1)).magnitude;

                  startInput.x = Vector3.Dot(_cameraController.MainCamera.right, transform.forward);
                  startInput.y = Vector3.Dot(Vector3.Scale(_cameraController.MainCamera.forward, new Vector3(1, 0, 1)), transform.forward);

                //_startInput = _cameraController.GetCameraDirection();

                if (startSpeed > 3.5f)
                    startSpeed = speedOnAir;
            }

            highestPosition = transform.position.y;
            landing = false;

        }

        private void PerformJump()
        {
            Vector3 velocity = _move.GetVelocity();
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * _move.GetGravity());

            _move.SetVelocity(velocity);
            _animator.SetAnimationState(animJumpState, 0,0.1f);
            startSpeed = speedOnAir;

            if (startInput.magnitude > 0.1f)
                startInput.Normalize();

                _audio.PlayEffect(jumpEffect);
        }

        public override bool ReadyToStart()
        {
            return !_move.IsGrounded() || _input.Jump;
        }

        public override void UpdateState()
        {
            if (landing)
            {
                // apply root motion
                _move.ApplyRootMotion(Vector3.one, false);

                // wait animation finish
                if (_animator.HasFinishedAnimation(playAnimState,0))
                    StopState();

                return;
            }

            if (_move.IsGrounded())
            {
                if (highestPosition - transform.position.y >= heightForKillOnLand)
                {
                    LandingSoft(true, animDeathState, deathClip);
                    _input.OnDied?.Invoke(); 
                    return;
                }
                else if (highestPosition - transform.position.y >= heightForHardLand)
                {
                    LandingSoft(true, animHardLandState, hardLandClip);
                    return;
                }
                else if(highestPosition - transform.position.y >= heightForSoftLand)
                {
                    LandingSoft(true, animSoftLandState, softLandClip);
                    return;
                }
                StopState();
                
            }

            if (transform.position.y > highestPosition)
                highestPosition = transform.position.y;

        }

        private void LandingSoft(bool landing, string animState, AudioClip clipLanding)
        {
            this.landing = landing;
            playAnimState = animState;
           _animator.SetAnimationState(playAnimState,0,0.02f);
            _audio.PlayEffect(clipLanding);

            // call event
            OnLanded.Invoke();
        }

        private void RotateCharacter()
        {
            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving
            if (_input.Move != Vector2.zero)
            {
                targetRotation = Mathf.Atan2(startInput.x, startInput.y) * Mathf.Rad2Deg + _cameraController.MainCamera.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref angleVel, airControl);

                // rotate to face input direction relative to camera position
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }

        }

        public override void OnStopState()
        {
            base.OnStopState();

            if (_move.IsGrounded() && !landing && _move.GetVelocity().y < -3f)
                OnLanded.Invoke();

            landing = false;
            highestPosition = 0;
            _move.StopRootMotion();
        }

    }
}
