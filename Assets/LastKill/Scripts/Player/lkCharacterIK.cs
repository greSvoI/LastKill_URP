using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lkCharacterIK : MonoBehaviour
{

		Animator _animator;

		Vector3 leftFootPos;
		Vector3 rightFootPos;

		Quaternion leftFootRot;
		Quaternion rightFootRot;

		Transform leftFoot;
		Transform rightFoot;

		float leftFootWeight;
		float rightFootWeight;

	    public float offsetY;

		public float lookIKWeight;
		public float headWeight;
		public float bodyWeight;
		public float eyesWeight;
	    public float clampWeight;


		public Transform targetPosition;

	private void Start()
	{
		_animator = GetComponent<Animator>();
		leftFoot = _animator.GetBoneTransform(HumanBodyBones.LeftFoot);
		rightFoot = _animator.GetBoneTransform(HumanBodyBones.RightFoot);

		leftFootRot = leftFoot.rotation;
		rightFootRot = rightFoot.rotation;
	}
	private void Update()
	{
		RaycastHit leftHit;
		Vector3 lpos = leftFoot.position;

		if(Physics.Raycast(lpos + Vector3.up * 0.5f,Vector3.down, out leftHit,1))
		{
			leftFootPos = Vector3.Lerp(lpos, leftHit.point + Vector3.up * offsetY, Time.deltaTime * 10f);
			leftFootRot = Quaternion.FromToRotation(transform.up, leftHit.normal) * transform.rotation;
			Debug.DrawLine(lpos,leftFootPos);
		}
		RaycastHit rightHit;
		Vector3 rpos = rightFoot.position;

		if (Physics.Raycast(rpos + Vector3.up * 0.5f, Vector3.down, out rightHit, 1))
		{
			rightFootPos = Vector3.Lerp(rpos, rightHit.point + Vector3.up * offsetY, Time.deltaTime * 10f);
			rightFootRot = Quaternion.FromToRotation(transform.up, rightHit.normal) * transform.rotation;
			Debug.DrawLine(rpos, rightFootPos);
		}
		OnAnimatorIK();
	}
	private void OnAnimatorIK()
		{
			Debug.Log("AnimatorIK");
			_animator.SetLookAtWeight(lookIKWeight, bodyWeight, headWeight, eyesWeight, clampWeight);
			_animator.SetLookAtPosition(targetPosition.position);
			// foot IK
			leftFootWeight = _animator.GetFloat("LeftFoot");

		    _animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, leftFootWeight);
			_animator.SetIKPosition(AvatarIKGoal.LeftFoot, leftFootPos);

			_animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, leftFootWeight);
			_animator.SetIKRotation(AvatarIKGoal.LeftFoot,leftFootRot);

		rightFootWeight = _animator.GetFloat("RightFoot");

		_animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, rightFootWeight);
		_animator.SetIKPosition(AvatarIKGoal.RightFoot, rightFootPos);

		_animator.SetIKRotationWeight(AvatarIKGoal.RightHand, rightFootWeight);
		_animator.SetIKRotation(AvatarIKGoal.RightHand, rightFootRot);

	}

		void FindFloorPositions(Transform t, ref Vector3 targetPosition, ref Quaternion targetRotation, Vector3 direction)
		{
			RaycastHit hit;
			Vector3 rayOrigin = t.position;
			// move the ray origin back a bit
			rayOrigin += direction * 0.3f;

			// raycast in the given direction
			Debug.DrawRay(rayOrigin, -direction, Color.green);
			if (Physics.Raycast(rayOrigin, -direction, out hit, 3))
			{
				// the hit point is the position of the hand/foot
				targetPosition = hit.point;
				// then rotate based on the hit normal
				Quaternion rot = Quaternion.LookRotation(transform.forward);
				targetRotation = Quaternion.FromToRotation(Vector3.up, hit.normal) * rot;
			}
		}
	
}
