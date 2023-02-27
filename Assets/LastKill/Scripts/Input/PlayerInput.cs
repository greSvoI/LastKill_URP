using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LastKill
{
	public class PlayerInput : MonoBehaviour
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
		[SerializeField] private bool _crawl;
		[SerializeField] private float _moveAmount;
		[SerializeField] private int _currentWeapon;
		[SerializeField] private int _lastWeapon;

		public Action Died;


		//hold button or single click
		[SerializeField] public bool HoldButton;

		public Vector2 Move => _move;
		public Vector2 Look => _look;
		public bool Sprint => _sprint;
		public bool Jump => _jump;
		public bool Crouch => _crouch;
		public bool Roll => _roll;
		public bool Fire => _fire;
		public bool Crawl => _crawl;
		public bool Aim => _aim;
		public float MoveAmount => _moveAmount;
		public int CurrentWeapon => _currentWeapon;

		//Priority 
		//Remove the ability to jump while crawling
		//Locomotion 0
		//Strafe 1
		//Crouch 2
		//Crawl 3
		Animator animator;
		private void Awake()
		{
			animator = GetComponent<Animator>();
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

			//inputActions.Player.Aim.performed += ctx =>
			//{
			//    _aim = ctx.ReadValueAsButton();
			//    _aim = !_aim;
			//};
			inputActions.Player.Aim.performed += OnAim;
			inputActions.Player.Aim.canceled += OnAim;

			inputActions.Player.Crawl.performed += OnCrawl;
			inputActions.Player.Crawl.canceled += OnCrawl;

			inputActions.Player.Crouch.performed += OnCrouch;
			inputActions.Player.Crouch.canceled += OnCrouch;

			inputActions.Player.Sprint.performed += OnSprint;
			inputActions.Player.Sprint.canceled += OnSprint;

			//inputActions.Player.Sprint.performed += ctx => {

			//	_sprint = ctx.ReadValueAsButton();
			//	//_sprint = !_sprint;
			//};

			//inputActions.Player.Crouch.performed += ctx => {
			//	_crouch = ctx.ReadValueAsButton();
			//	//_crouch = !_crouch;
			//};
			//inputActions.Player.Crawl.performed += ctx => {
			//	_crawl = ctx.ReadValueAsButton();
			//	//_crawl = !_crawl;
			//};

			inputActions.Player.Weapon.performed += SetWeapon;

		}

        private void OnSprint(InputAction.CallbackContext obj)
        {
			if (HoldButton)
			{
				_sprint = obj.ReadValueAsButton();
			}
			else
			{
				_sprint = !_sprint;
			}
		}

        private void OnCrouch(InputAction.CallbackContext obj)
        {
            if(HoldButton)
            {
				_crouch = obj.ReadValueAsButton();
            }
			else
            {
				_crouch = !_crouch;
            }
        }

        private void OnCrawl(InputAction.CallbackContext obj)
        {
			if (HoldButton)
			{
				_crawl = obj.ReadValueAsButton();
			}
			else
			{
				_crawl = !_crawl;
				_crouch = false;
			}
		}

        private void OnAim(InputAction.CallbackContext obj)
        {
			if (HoldButton)
			{
				_aim = obj.ReadValueAsButton();
			}
			else
			{
				_aim = !_aim;
			}
		}

        private void SetWeapon(InputAction.CallbackContext obj)
        {
			_lastWeapon = _currentWeapon;
			int.TryParse(obj.control.displayName,out _currentWeapon);

            if (_lastWeapon == _currentWeapon)
            {
                _currentWeapon = 0;
            }
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
