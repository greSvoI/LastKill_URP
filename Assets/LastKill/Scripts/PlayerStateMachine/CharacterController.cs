using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace LastKill
{
	public class CharacterController : MonoBehaviour
	{
		AnimatorController animator;
		new CameraController camera;
		public lkPlayerInput playerInput;
		[Header("Speed")]
		public float strafeSpeed = 5f;

		Rigidbody rigidBody;

		StateMachine stateMachine;
		MovementState movementState;
		StrafeState strafeState;

	    [Header("Input")] 
		[SerializeField] private Vector3 _moveDirection;
		[SerializeField] private Vector2 _moveInput;
		[SerializeField] private Vector2 _lookInput;

		[SerializeField] private float _magnitude;
		[SerializeField] private float _moveSpeed;
		[SerializeField] private float _rotateSpeed = 15f;

		public Vector3 MoveDirection { get => _moveDirection; set { _moveDirection = value; } }
		public Vector2 MoveInput { get => _moveInput; private set { _moveInput = value; } }
		public Vector2 LookInput { get => _lookInput; set { _lookInput = value; } }
		public float Magnitude { get => _magnitude; set { _magnitude = value; } }
		public float CurrentSpeed { get => _moveSpeed; set { _moveSpeed = value; } }

		private void Awake()
		{
			playerInput = GetComponent<lkPlayerInput>();
			rigidBody = GetComponent<Rigidbody>();
		 	camera = GetComponent<CameraController>();
			animator = GetComponent<AnimatorController>();

			stateMachine = new StateMachine();
			movementState = new MovementState(this, animator ,camera, stateMachine);
			strafeState = new StrafeState(this, animator,camera, stateMachine);

			stateMachine.Initialize(strafeState);
		}

		private void Update()
		{
			stateMachine.CurrentState.HandleInput();
			stateMachine.CurrentState.LogicUpdate();
		}



		public void UpdateIput()
		{
			MoveInput = playerInput.Move;
			LookInput = playerInput.Look;
			Magnitude = MoveInput.magnitude < 1f ? MoveInput.magnitude : 1f;
		}

		private void FixedUpdate()
		{
			stateMachine.CurrentState.PhysicsUpdate();
		}
		private void LateUpdate()
		{
			stateMachine.CurrentState.CameraUpdate();
		}
		public void PlayerMovement()
		{
			MoveDirection = camera.GetCameraDirection(MoveInput);
			rigidBody.velocity = _moveDirection * _moveSpeed;
		}
		public void PlayerRotate()
		{
			transform.rotation = camera.PlayerTargetRotation(MoveInput);
		}
	}

}