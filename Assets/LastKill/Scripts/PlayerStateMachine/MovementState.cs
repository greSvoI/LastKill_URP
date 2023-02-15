
using System;
using System.Diagnostics;
using System.Numerics;

namespace LastKill
{
	public class MovementState : State
	{
		public MovementState(CharacterController character, AnimatorController animator, CameraController camera, StateMachine stateMachine) : base(character, animator, camera, stateMachine)
		{
		}

		private float _walkingSpeed = 3f;
		private float _runningSpeed = 5f;
		private float _sprintingSpeed = 8f;

		public override void Enter()
		{
			base.Enter();
			character.CurrentSpeed = _walkingSpeed;
		}

		public override void HandleInput()
		{
			character.UpdateIput();
		}

		public override void LogicUpdate()
		{
			SetSpeed();
			animator.UpdateInput(character.MoveInput);
		}
		public override void PhysicsUpdate()
		{
			character.PlayerMovement();
			character.PlayerRotate();
		}
		public override void CameraUpdate()
		{
			camera.CameraRotate(character.LookInput, true);
		}
		public override void Exit()
		{

		}
		private void SetSpeed()
		{
			float magnitude = character.MoveInput.magnitude;

			if(magnitude<=0f)
			{
				character.Magnitude = 0f;
				return;
			}

			if(character.playerInput.IsSprint)
			{
				character.Magnitude = 1.5f;
				character.CurrentSpeed = _sprintingSpeed;
			}

			else if (magnitude > 0f && !character.playerInput.IsSprint)
			{
				character.Magnitude = magnitude;
				character.CurrentSpeed = magnitude < 0.55f ? _walkingSpeed : _runningSpeed;
			}
		}

		


	}
}
