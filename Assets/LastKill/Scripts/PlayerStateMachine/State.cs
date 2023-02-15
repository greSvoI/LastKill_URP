
using UnityEditorInternal;
using UnityEngine;

namespace LastKill
{
	public abstract class State
	{
		protected CharacterController character;
		protected AnimatorController animator;
		protected CameraController camera;
		protected StateMachine stateMachine;

		protected State(CharacterController character, AnimatorController animator,CameraController camera, StateMachine stateMachine)
		{
			this.character = character;
			this.animator = animator;
			this.camera = camera;
			this.stateMachine = stateMachine;
		}

		public virtual void Enter()
		{
		}

		public virtual void HandleInput()
		{

		}

		public virtual void LogicUpdate()
		{

		}

		public virtual void PhysicsUpdate()
		{

		}
		public virtual void CameraUpdate()
		{

		}
		public virtual void Exit()
		{

		}

	}
}
