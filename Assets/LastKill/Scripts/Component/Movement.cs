using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastKill
{
    public class Movement : MonoBehaviour, IMove
    {
        [Header("Player")]
        [Tooltip("Acceleration and deceleration")]
        public float SpeedChangeRate = 10.0f;
        public bool _useCameraOrientation = true;
        [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;

        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        public bool OnGround = true;
        [Tooltip("Useful for rough ground")]
        public float GroundedOffset = -0.14f;
        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float GroundedRadius = 0.28f;
        [Tooltip("What layers the character uses as ground")]
        public LayerMask GroundLayers;
        [Tooltip("The engine default is -9.8f")]
        [SerializeField] private float Gravity = -15.0f;


        private Animator _animator;
        private CharacterController _controller;
        private Camera _mainCamera;

        private bool _hasAnimator;
        // player
        private float _speed;
        private float _animationBlend;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _terminalVelocity = 53.0f;
        private float _initialCapsuleHeight = 2f;
        private float _initialCapsuleRadius = 0.28f;

        // animation IDs
        private int _animIDSpeed;
        private int _animIDMotionSpeed;

        // controls character velocity
        private Vector3 _velocity;
        private float _timeoutToResetVars = 0;

        private void Awake()
        {
            _mainCamera = Camera.main;
            _controller = GetComponent<CharacterController>();
        }
        private void Start()
        {
            _hasAnimator = TryGetComponent(out _animator);
            AssignAnimationIDs();
        }
        private void Update()
        {
            GravityControl();
            GroundedCheck();
        }
        private void AssignAnimationIDs()
        {
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDMotionSpeed = Animator.StringToHash("Motion Speed");
        }
        private void GravityControl()
        {
            if (_controller.isGrounded)
            {
                // stop our velocity dropping infinitely when grounded
                if (_velocity.y < 2.0f)
                {
                    _velocity.y = -5f;
                }
            }

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (_velocity.y < _terminalVelocity)
            {
                _velocity.y += Gravity * Time.deltaTime;
            }
        }

        private void GroundedCheck()
        {
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
            OnGround = Physics.CheckSphere(spherePosition, _controller.radius, GroundLayers, QueryTriggerInteraction.Ignore);

            if (!OnGround && !_controller.isGrounded) return;
        }

        public bool IsOnGround()
        {
            return OnGround;
        }
        public void Move(Vector2 moveInput, float targetSpeed, bool rotateCharacter = true)
        {
            Move(moveInput, targetSpeed, _mainCamera.transform.rotation, rotateCharacter);
        }
        public void Move(Vector2 moveInput, float targetSpeed, Quaternion cameraRotation, bool rotateCharacter = true)
        {
            if (moveInput == Vector2.zero) targetSpeed = 0f;

        }
    }
}