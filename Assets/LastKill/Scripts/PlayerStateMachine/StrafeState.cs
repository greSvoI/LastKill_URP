using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastKill
{
	public class StrafeState : State
	{

		public StrafeState(CharacterController character, AnimatorController animator, CameraController camera, StateMachine stateMachine) : base(character, animator, camera, stateMachine)
		{
		}
		public override void Enter()
		{
			base.Enter();
			animator.isStrafe = true;
			character.CurrentSpeed = 5;
			
		}
		public override void HandleInput()
		{
			character.UpdateIput();
		}

		public override void LogicUpdate()
		{
	
			animator.UpdateInput(character.MoveInput);
		}
		public override void PhysicsUpdate()
		{
			character.PlayerMovement();
			
		}
		public override void CameraUpdate()
		{
		}
		public override void Exit()
		{

		}
	}
}
