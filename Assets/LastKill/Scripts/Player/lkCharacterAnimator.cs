using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lkCharacterAnimator : MonoBehaviour
{
    private Animator _animator;

    private int _vertical = Animator.StringToHash("Vertical");
    private int _horizontal = Animator.StringToHash("Horizontal");
    private int _magnituda = Animator.StringToHash("MoveAmount");
    private int _jump = Animator.StringToHash("Jump");
    private int _falling = Animator.StringToHash("Falling");
    private int _landSoft = Animator.StringToHash("LandSoft");
    private int _landHard = Animator.StringToHash("LandHard");

    [SerializeField] private float JumpPower = 1f;
    public bool IsAction { get { return _animator.GetBool("Action"); } set { _animator.SetBool("Action", value); } }

	public Transform LookAt;
	void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void UpdateMoveInputParametr(Vector2 moveInput,float moveAmount)
    {
        _animator.SetFloat(_vertical, moveInput.y,0.1f,Time.fixedDeltaTime);
        _animator.SetFloat(_horizontal, moveInput.x, 0.1f, Time.fixedDeltaTime);
        _animator.SetFloat(_magnituda, moveAmount);
	}
    public void Jump()
    {
        IsAction = true;
        PlayTargetAnimation(_jump);
	}
    public void Falling()
    {
        IsAction = true;
        PlayTargetAnimation(_falling);
    }
    public void Landing(float timer)
    {
        PlayTargetAnimation(timer < JumpPower ? _landSoft : _landHard);
    }
    private void PlayTargetAnimation(int animation)
    {
		_animator.CrossFade(animation, 0.2f);
	}


    public void UpdateLookPosition()
    {
		float distanceFaceObject = Vector3.Distance(_animator.GetBoneTransform(HumanBodyBones.Head).position, LookAt.position);

		_animator.SetLookAtPosition(LookAt.position);
		// blend based on the distance
		_animator.SetLookAtWeight(Mathf.Clamp01(2 - distanceFaceObject), Mathf.Clamp01(1 - distanceFaceObject));
	}
}
