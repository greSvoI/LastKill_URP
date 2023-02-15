using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastKill
{
	public class AnimatorController : MonoBehaviour
	{
		private CharacterController _character;
		private Animator _animator;

		private int hashVertical = Animator.StringToHash("Vertical");
		private int hashHorizontal = Animator.StringToHash("Horizontal");
		private int hashMagnituda = Animator.StringToHash("Magnituda");

		private int hashAction = Animator.StringToHash("Action");

		protected int hashStrafe = Animator.StringToHash("Strafe");
		protected int hashCrouch = Animator.StringToHash("Crouch");


		public bool isAction { get => _animator.GetBool(hashAction); set { _animator.SetBool(hashAction, value); } }
		public bool isStrafe { get => _animator.GetBool(hashStrafe);  set { _animator.SetBool(hashStrafe, value); } }

		private void Awake()
		{
			_animator = GetComponent<Animator>();
			_character = GetComponent<CharacterController>();
		}
		public void PlayTargetAnimation()
		{

		}
		public void UpdateInput(Vector2 inputMove)
		{
			_animator.SetFloat(hashMagnituda, _character.Magnitude);
			_animator.SetFloat(hashHorizontal, inputMove.x, 0.2f,Time.deltaTime);
			_animator.SetFloat(hashVertical, inputMove.y, 0.2f, Time.deltaTime);
		}
	}
}
