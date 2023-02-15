using LastKill;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
enum MovementState
{
	Idle,
	Walking,
	Running,
	Sprinting
}
public class lkCharacterController : MonoBehaviour
{
    private	lkPlayerInput playerInput;
	private lkCharacterMovement characterMovement;
	private lkCameraController cameraController;
	private lkCharacterAnimator characterAnimator;
	private lkCharacterDetection characterDetection;
	private lkRagdollOn characterRagdoll;

	[Header("Animator parametrs")]
	public float _animationSpeed;


	[Header("Movement")]
	public float walkingSpeed = 1.5f;
	public float strafeSpeed = 3f;
	public float runningSpeed = 5f;
	public float sprintingSpeed = 7f;
	public float rotationSpeed = 15f;

	[SerializeField] private float _currentSpeed;
	public float CurrentSpeed { get => _currentSpeed; set { _currentSpeed = value; } }

	[SerializeField] private Vector2 moveDirection;
	[SerializeField] private Vector2 moveDirectionNormalize;
	[SerializeField] private float magnituda;



	[SerializeField] private float stepHeight = 0.7f;
	 public bool isGround;


	Animator animator;
	private void Awake()
	{
		playerInput = GetComponent<lkPlayerInput>();
		characterMovement = GetComponent<lkCharacterMovement>();
		cameraController = GetComponent<lkCameraController>();
		characterAnimator = GetComponent<lkCharacterAnimator>();
		characterDetection = GetComponent<lkCharacterDetection>();
		characterRagdoll = GetComponent<lkRagdollOn>();
		animator = GetComponent<Animator>();

	}

	private void Update()
	{
		if(playerInput.Move != Vector2.zero)
		{
			moveDirection = playerInput.Move;
			moveDirectionNormalize = playerInput.Move.normalized;
			magnituda = playerInput.Move.magnitude;
		}

		isGround = characterDetection.OnGround(stepHeight);
		SetCurrentSpeed();


		if(playerInput.IsRoll)
		{
			//characterRagdoll.RagdollEnable();
					Debug.Log("Enter Roll");
			if(animator.HasState(0, Animator.StringToHash("Strafe")))
			{
			   animator.CrossFadeInFixedTime("Strafe", 0.1f, 0);
			}
		}
	}
	private void FixedUpdate()
	{

		characterMovement.UpdateMovement();

		characterAnimator.UpdateMoveInputParametr(playerInput.Move, _animationSpeed);

	}
	private void LateUpdate()
	{
		cameraController.FreeMovementCamera();
		
	}
	private void SetCurrentSpeed()
	{
		
		if(playerInput.MoveAmount > 0f)
		{
			if (playerInput.MoveAmount <= 0.5f)
			{
				_currentSpeed = walkingSpeed;
				_animationSpeed = 0.5f;
				return;
			}
			else
			{
				
				if (playerInput.IsSprint)
				{
					_currentSpeed = sprintingSpeed;
					_animationSpeed = 1.5f;
					return;
				}
				else
				{
					_currentSpeed = runningSpeed;
					_animationSpeed = 1f;
					return;
				}
			}
			
		}
		else
		{
			_animationSpeed = 0f;
		}
	}

}
