using LastKill;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class lkCharacterMovement : MonoBehaviour
{
	[SerializeField] private float _rotationSpeed = 15f;
	[SerializeField] private Vector3 _moveDirection;
	[SerializeField] private Vector3 _targetDirection;


	lkPlayerInput playerInput;
	lkCharacterController characterController;
	lkCharacterDetection characterDetection;
	lkCharacterAnimator characterAnimator;
	lkCameraController cameraController;
	CapsuleCollider capsuleCollider;
	Rigidbody rigidBody;


	[SerializeField] private Vector3 floorMovement;
	[SerializeField] private float floorOffsetY = 0.1f;

	[Header("Jumping")]
	public float jumpHeight = 3f;
	public float gravityIntensity = -15;


	[Header("Falling")]
	public float _inAirTimer;
	public float leapingVelocity;
	public float fallingVelocity;
	public float rayCastHeightOffset = 0.5f;

	[Header("Movement Flags")]
	public bool _isJump;
	public bool isGrounded;
	public bool _landing;
	public LayerMask groundLayer;

	private void Awake()
	{
		characterController = GetComponent<lkCharacterController>();
		characterDetection = GetComponent<lkCharacterDetection>();
		characterAnimator = GetComponent<lkCharacterAnimator>();
		cameraController = GetComponent<lkCameraController>();
		capsuleCollider = GetComponent<CapsuleCollider>();
		playerInput = GetComponent<lkPlayerInput>();
		rigidBody = GetComponent<Rigidbody>();
		rigidBody.isKinematic = false;
	}
	public void UpdateMovement()
	{
		Gravity();
		Falling();
		if(playerInput.IsJump)
		{
			Jump();
		}
		if(characterAnimator.IsAction)
		{
			return;
		}
		Movement();
		MovementRotate();
	}
	public void Gravity()
    {
		
		if (_moveDirection.y < 2.0f)
		{
			_moveDirection.y = -5f;
		}
	}
	public void Movement()
	{
		_moveDirection = cameraController.GetCameraDirection() * characterController.CurrentSpeed;
		rigidBody.velocity = _moveDirection;
	}
	public void MovementRotate()
	{
		_targetDirection = cameraController.GetCameraDirection();
		if (_targetDirection == Vector3.zero)
			_targetDirection = transform.forward;

		Quaternion targetRotation = Quaternion.LookRotation(_targetDirection);
		Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime);
		transform.rotation = playerRotation;
	}
	public void Jump()
	{
		if(characterDetection.OnGround(0.5f))
		{
			characterAnimator.Jump();
			float jumpVelocity = Mathf.Sqrt(-2 * gravityIntensity * jumpHeight);
			_moveDirection.y = jumpVelocity;
			rigidBody.velocity = _moveDirection;
			_landing = true;
		}
	}
	public void Falling()
	{
		if(!characterDetection.OnGround(0.5f))
		{
			if (!characterAnimator.IsAction)
			{
				characterAnimator.Falling();
				_landing = true;
			}
				_inAirTimer += Time.deltaTime;
				rigidBody.AddForce(transform.forward * leapingVelocity);
				rigidBody.AddForce(Vector3.down * fallingVelocity * _inAirTimer);
		}
		if (characterDetection.OnGround(0.5f) && characterAnimator.IsAction && _landing)
		{
			characterAnimator.Landing(_inAirTimer);
			_inAirTimer = 0;
			_landing = false;
		}

		//RaycastHit hit;
		//Vector3 rayCastOrigin = transform.position;
		//rayCastOrigin.y += rayCastHeightOffset;

		//if (!isGrounded)
		//{
		//	if (!characterAnimator.IsAction)
		//	{
		//		characterAnimator.Falling();
		//	}
		//	_inAirTimer += Time.deltaTime;
		//	rigidBody.AddForce(transform.forward * leapingVelocity);
		//	rigidBody.AddForce(Vector3.down * fallingVelocity * _inAirTimer);
		//}

		//if (Physics.SphereCast(rayCastOrigin, 0.2f, Vector3.down, out hit, groundLayer))
		//{
		//	if (!isGrounded && characterAnimator.IsAction)
		//	{
		//		characterAnimator.Landing(_inAirTimer);
		//		_inAirTimer = 0;
		//		isGrounded = true;
		//	}
		//}
		//else
		//{
		//	isGrounded = false;
		//}
	}

}
