using System;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LastKill
{
	public class lkPlayerInput : MonoBehaviour
	{
		private PlayerInputSystem inputActions = null;

		[SerializeField] private Vector2 _move;
		[SerializeField] private Vector2 _look;
		[SerializeField] private bool _sprint;
		[SerializeField] private bool _jump;
		[SerializeField] private bool _crouch;
		[SerializeField] private bool _roll;
		[SerializeField] private bool _fire;
		[SerializeField] private bool _aim;
		[SerializeField] private bool _strafe;
		[SerializeField] private float _moveAmount;


		public Vector2 Move => _move;
		public Vector2 Look => _look;
		public bool IsSprint => _sprint;
		public bool IsJump => _jump;
		public bool IsCrouch => _crouch;
		public bool IsRoll => _roll;
		public bool IsFire => _fire;	
		public float MoveAmount => _moveAmount;
		

		private void Awake()
		{
			if (inputActions == null)
				inputActions = new PlayerInputSystem();

			inputActions.Player.Move.performed += OnMove;
			inputActions.Player.Move.canceled += OnMove;

			inputActions.Player.Look.performed += ctx => _look = ctx.ReadValue<Vector2>();
			inputActions.Player.Look.canceled += ctx => _look = ctx.ReadValue<Vector2>();

			inputActions.Player.Jump.performed += ctx => _jump = ctx.ReadValueAsButton();
			inputActions.Player.Jump.canceled += ctx => _jump = ctx.ReadValueAsButton();

			inputActions.Player.Fire.performed += ctx => _fire = ctx.ReadValueAsButton();
			inputActions.Player.Fire.canceled += ctx => _fire = ctx.ReadValueAsButton();


			inputActions.Player.Roll.performed += ctx => _roll = ctx.ReadValueAsButton();
			inputActions.Player.Roll.canceled += ctx => _roll = ctx.ReadValueAsButton();

			inputActions.Player.Aim.performed += ctx => { _aim = !_aim; };

			inputActions.Player.Sprint.performed += ctx => { _sprint = !_sprint; };

			inputActions.Player.Crouch.performed += ctx => { _crouch = !_crouch; };

		}

		private void OnMove(InputAction.CallbackContext obj)
		{
			_move = obj.ReadValue<Vector2>();
			_moveAmount = Mathf.Clamp01(Mathf.Abs(_move.x) + Mathf.Abs(_move.y));
		}

		private void OnEnable()
		{
			inputActions.Enable();
		}
		private void OnDisable()
		{
			inputActions.Disable();
		}

	}
}
