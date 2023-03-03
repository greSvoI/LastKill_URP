using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LastKill
{
    public class MoveController : MonoBehaviour, IMove, ICapsule 
    {
		[Header("Player")]
		[Tooltip("How fast the character turns to face movement direction")]
		[Range(0.0f, 0.3f)]
		public float RotationSmoothTime = 0.12f;
		[Tooltip("Acceleration and deceleration")]
		public float SpeedChangeRate = 10.0f;
		public bool _useCameraOrientation = true;

		[Header("Player Grounded")]
		[Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
		public bool Grounded = true;
		[Tooltip("Useful for rough ground")]
		public float GroundedOffset = -0.14f;
		[Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
		public float GroundedRadius = 0.28f;
		[Tooltip("What layers the character uses as ground")]
		public LayerMask GroundLayers;

		[Header("Gravity")]
		[Tooltip("Should apply gravity?")]
		[SerializeField] private bool UseGravity = true;
		[Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
		[SerializeField] private float Gravity = -15.0f;

		// player
		private bool _died = false;
		[SerializeField] private float _speed;
		private float _animationBlend;
		private float _targetRotation = 0.0f;
		private float _rotationVelocity;
		private float _terminalVelocity = 53.0f;
		private float _initialCapsuleHeight = 2f;
		private float _initialCapsuleRadius = 0.28f;

		// variables for root motion
		private bool _useRootMotion = false;
		private Vector3 _rootMotionMultiplier = Vector3.one;
		private bool _useRotationRootMotion = true;

		// animation IDs
		private int _animIDSpeed;
		private int _animIDMotionSpeed;

		private CharacterController _controller;
		private PlayerInput _input;
		private DetectionController _detectionController;
		private IAnimator _animator;
		private iCamera _camera;


		// controls character velocity
		[SerializeField] private  Vector3 _velocity;
		private float _timeoutToResetVars = 0;

		private void Awake()
		{
			_camera = GetComponent<iCamera>();
			_controller = GetComponent<CharacterController>();
			_input = GetComponent<PlayerInput>();
			_detectionController = GetComponent<DetectionController>();
			_animator = GetComponent<IAnimator>();
			_input.OnDied += OnDied;

			_initialCapsuleHeight = _controller.height;
			_initialCapsuleRadius = _controller.radius;
		}



		private void Start()
		{
			//_hasAnimator = TryGetComponent(out _animator);
			AssignAnimationIDs();
		
		}
		bool temp = false;
		private void Update()
		{
		
			if (_died) return;
			GravityControl();
			GroundedCheck();

			if(_input.Fire && !temp)
            {
				Temp();
				temp = true;
            }

			if (_timeoutToResetVars <= 0)
			{
				_speed = 0;
				_animationBlend = 0;
				_animator.Animator.SetFloat(_animIDSpeed, 0);
				_timeoutToResetVars = 0;
			}
			else
				_timeoutToResetVars -= Time.deltaTime;

			if (_useRootMotion)
				return;

			if (!_controller.enabled) return;

			_controller.Move(_velocity * Time.deltaTime);
		}
		private void Temp()
        {
			Collider[] hitColliders = Physics.OverlapSphere(transform.position, 5);
			foreach (var hitCollider in hitColliders)
        {

			}
		}
		private void OnDied()
		{
			StopMovement();
			_died = true;
			StartCoroutine(OnAlive());
		}
		IEnumerator OnAlive()
		{
			yield return new WaitForSecondsRealtime(3);
			_died = false;
		}
		private void OnDrawGizmos()
        {
			if (Grounded)
				Gizmos.color = Color.green;
			else Gizmos.color = Color.red;

			Gizmos.DrawSphere(transform.position, GroundedRadius);

		}
        private void OnAnimatorMove()
		{
			if (!_useRootMotion) return;

			if (_controller.enabled)
				_controller.Move(_animator.Animator.deltaPosition);
			else
				_animator.Animator.ApplyBuiltinRootMotion();

			transform.rotation *= _animator.Animator.deltaRotation;
		}

		private void AssignAnimationIDs()
		{
			_animIDSpeed = Animator.StringToHash("Speed");
			_animIDMotionSpeed = Animator.StringToHash("Motion Speed");
		}

		private void GroundedCheck()
		{
			// set sphere position, with offset
			//Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
			//Grounded = Physics.CheckSphere(spherePosition, _controller.radius, GroundLayers, QueryTriggerInteraction.Ignore);
			//RaycastHit hit;
			//Grounded = Physics.Raycast(transform.position + new Vector3(0, 0.3f, 0), Vector3.down, out hit, 0.5f);
			Grounded = Physics.CheckSphere(transform.position, GroundedRadius,GroundLayers);


			if (!Grounded && !_controller.isGrounded) return;

            Depenetrate();
        }
		private void Depenetrate()
		{
			if (!_controller.enabled) return;

			// first check if there is a possible ground in all grounds
			RaycastHit[] hits = Physics.SphereCastAll(transform.position + Vector3.up, _controller.radius, Vector3.down,
				1 - GroundedOffset, Physics.AllLayers, QueryTriggerInteraction.Ignore);

			foreach (RaycastHit h in hits)
			{
				if (h.distance != 0 && Vector3.Dot(h.normal, Vector3.up) > 0.7f)
					return;
			}

			// if not depenetrate char
			RaycastHit hit;
			if (Physics.SphereCast(transform.position + Vector3.up, _controller.radius, Vector3.down,
				out hit, 1 - GroundedOffset, Physics.AllLayers, QueryTriggerInteraction.Ignore))
			{
				if (Vector3.Dot(hit.normal, Vector3.up) < 0.5f)
				{
					Grounded = false;
					Vector3 direction = hit.normal;
					direction.y = -1;
					_controller.Move(direction.normalized * _controller.skinWidth * 3);
				}
			}
		}

		public Collider GetGroundCollider()
		{
			if (!Grounded) return null;

			Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
			Collider[] grounds = Physics.OverlapSphere(spherePosition, _controller.radius, GroundLayers, QueryTriggerInteraction.Ignore);

			if (grounds.Length > 0)
				return grounds[0];

			return null;
		}

		public void Move(Vector2 moveInput, float targetSpeed, bool rotateCharacter = true)
		{
			Move(moveInput, targetSpeed, _camera.GetTransform.transform.rotation, rotateCharacter);
		}
        public void Move(Vector2 moveInput, float targetSpeed, Quaternion cameraRotation, bool rotateCharacter = true)
		{
			// note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is no input, set the target speed to 0
			if (moveInput == Vector2.zero) targetSpeed = 0.0f;

			// a reference to the players current horizontal velocity
			float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

			float speedOffset = 0.1f;
			float inputMagnitude = moveInput.magnitude; // input.analogMovement ? input.move.magnitude : 1f;

			if (inputMagnitude > 1)
				inputMagnitude = 1f;

			// accelerate or decelerate to target speed
			if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
			{
				// creates curved result rather than a linear one giving a more organic speed change
				// note T in Lerp is clamped, so we don't need to clamp our speed
				_speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);

				// round speed to 3 decimal places
				_speed = Mathf.Round(_speed * 1000f) / 1000f;
			}
			else
			{
				_speed = targetSpeed * inputMagnitude;
			}
			_animationBlend = Mathf.Lerp(_animationBlend, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);

			// normalise input direction
			Vector3 inputDirection = new Vector3(moveInput.x, 0.0f, moveInput.y).normalized;

			// note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is a move input rotate player when the player is moving
			if (moveInput != Vector2.zero)
			{
				_targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + (_useCameraOrientation ? cameraRotation.eulerAngles.y : 0);
				float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, RotationSmoothTime);

				// rotate to face input direction relative to camera position
				if (rotateCharacter)
					transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
			}

			// update animator if using character

			
				//speed did not go beyond
				//affects animation events
				if (_animationBlend < 0.2f && _input.Move == Vector2.zero) _animationBlend = 0f;

				_animator.Animator.SetFloat(_animIDSpeed, _animationBlend);
				_animator.Animator.SetFloat(_animIDMotionSpeed, inputMagnitude);

			Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
			_velocity = targetDirection.normalized * _speed + new Vector3(0.0f, _velocity.y, 0.0f);
			_timeoutToResetVars = 0.5f;
		}

		public void Move(Vector3 velocity)
		{
			Vector3 newVelocity = velocity;
			if (UseGravity)
				newVelocity.y = _velocity.y;

			_velocity = newVelocity;
		}

		private void GravityControl()
		{
			if (_controller.isGrounded)
			{
				// stop our velocity dropping infinitely when grounded
				if (_velocity.y < 2.0f)
				{
					_velocity.y = -2f;
				}
			}

			// apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
			if (UseGravity && _velocity.y < _terminalVelocity)
			{
				_velocity.y += Gravity * Time.deltaTime;
			}
		}

		/// <summary>
		/// Get rotation to face desired direction
		/// </summary>
		/// <returns></returns>
		public Quaternion GetRotationFromDirection(Vector3 direction)
		{
			float yaw = Mathf.Atan2(direction.x, direction.z);
			return Quaternion.Euler(0, yaw * Mathf.Rad2Deg, 0);
		}

		public void SetPosition(Vector3 newPosition)
		{
			bool currentEnable = _controller.enabled;

			_controller.enabled = false;
			transform.position = newPosition;
			_controller.enabled = currentEnable;
		}

		public void DisableCollision()
		{
			_controller.enabled = false;
		}

		public void EnableCollision()
		{
			_controller.enabled = true;
		}

		public void SetCapsuleSize(float newHeight, float newRadius)
		{
			if (newRadius > newHeight * 0.5f)
				newRadius = newHeight * 0.5f;

			_controller.radius = newRadius;
			_controller.height = newHeight;
			_controller.center = new Vector3(0, newHeight * 0.5f, 0);
		}

		public void ResetCapsuleSize()
		{
			SetCapsuleSize(_initialCapsuleHeight, _initialCapsuleRadius);
		}

		public void SetVelocity(Vector3 velocity)
		{
			this._velocity = velocity;
		}

		public Vector3 GetVelocity()
		{
			return _velocity;
		}

		public float GetGravity()
		{
			return Gravity;
		}

		public void ApplyRootMotion(Vector3 multiplier, bool applyRotation = false)
		{
			_useRootMotion = true;
			_rootMotionMultiplier = multiplier;
			_useRotationRootMotion = applyRotation;
		}

		public void StopRootMotion()
		{
			_useRootMotion = false;
			_useRotationRootMotion = false;
		}

		public float GetCapsuleHeight()
		{
			return _controller.height;
		}

		public float GetCapsuleRadius()
		{
			return _controller.radius;
		}

		public void EnableGravity()
		{
			UseGravity = true;
		}

		public void DisableGravity()
		{
			UseGravity = false;
		}
		bool IMove.IsGrounded()
		{
			return Grounded;
		}

		public void StopMovement()
		{
			_velocity = Vector3.zero;
			_speed = 0;

			_animator.Animator.SetFloat(_animIDSpeed, 0);
			_animator.Animator.SetFloat(_animIDMotionSpeed, 0);
		}

		public Vector3 GetRelativeInput(Vector2 input)
		{
			Vector3 relative = _camera.GetTransform.transform.right * input.x +
				Vector3.Scale(_camera.GetTransform.transform.forward, new Vector3(1, 0, 1)) * input.y;

			return relative;
		}

    }
}
