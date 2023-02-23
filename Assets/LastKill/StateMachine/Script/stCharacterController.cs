using UnityEngine;
using LastKill;

public class stCharacterController : MonoBehaviour
{
	lkPlayerInput playerInput;
	Animator animator;
	Transform _camera;
	private void Awake()
	{
		playerInput = GetComponent<lkPlayerInput>();
		animator = GetComponent<Animator>();
		_camera = Camera.main.transform;
	}

	[SerializeField] float _targetRotation; 
	[SerializeField] Vector3 _rollDirection = Vector3.forward;
	[SerializeField] bool roll = false;
	private void Update()
	{
		if(playerInput.IsRoll)
		{
			
			animator.CrossFadeInFixedTime("Roll", 0.1f, 0);
			_rollDirection = transform.forward;
			_targetRotation = transform.eulerAngles.y;
			if (playerInput.Move != Vector2.zero)
			{
				// normalise input direction
				Vector3 inputDirection = new Vector3(playerInput.Move.x, 0.0f, playerInput.Move.y).normalized;
				_targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _camera.transform.eulerAngles.y;
				_rollDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
			}
			_rollDirection.Normalize();
			roll = false;
		}

		
		// smooth rotate character
		transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, _targetRotation, 0), 0.1f);

		if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f && !animator.IsInTransition(0) &&  !roll)
		{
			roll = true;
			Debug.Log("Stop");
		}



		//if(!animator.IsInTransition(0))
		//{
		//	Debug.Log("Transition");
		//}

	}

	private void FixedUpdate()
	{
		//if(playerInput.Sprint)
		//{
		
		//	animator.HasState(0, Animator.StringToHash("Crouch"));
		//	animator.CrossFadeInFixedTime("Crouch", 0.1f, 0);
		//}
	}
}
