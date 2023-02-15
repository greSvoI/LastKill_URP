using LastKill;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class lkCameraController : MonoBehaviour
{
	lkPlayerInput playerInput;


    [SerializeField] private Transform _mainCamera;
	[SerializeField] private Transform _cameraRoot;
	[SerializeField] private float _sensivity;

	[Header("Cinemachine")]
	[Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
	public GameObject CinemachineCameraTarget;
	public float cinemachineTargetYaw;
	public float cinemachineTargetPitch;

	[Tooltip("How far in degrees can you move the camera up")]
	public float TopClamp = 70.0f;

	[Tooltip("How far in degrees can you move the camera down")]
	public float BottomClamp = -30.0f;

	[Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
	public float CameraAngleOverride = 0.0f;
	public float angleFire = 0f;

	private void Start()
    {
		playerInput = GetComponent<lkPlayerInput>();
        _mainCamera = Camera.main.transform;
    }

	//public float lookAngle;
	//public float pivotAngle;
	//public float cameraLookSpeed = 2f;
	//public float cameraPivotSpeed  =2f;

	//public void RotateCamera()
	//{
	//	lookAngle += playerInput.Look.x *  cameraLookSpeed;
	//	pivotAngle += playerInput.Look.y * cameraPivotSpeed;

	//	Vector3 rotation = Vector3.zero;
	//	rotation.y = lookAngle;

	//	Quaternion targetRotation = Quaternion.Euler(rotation);
	//	_cameraRoot.rotation = targetRotation;

	//	rotation = Vector3.zero;
	//	rotation.x = pivotAngle;
	//	targetRotation = Quaternion.Euler(rotation);
	//	_cameraRoot.rotation = targetRotation;
	//}

	public void FreeMovementCamera()
	{
		cinemachineTargetYaw += playerInput.Look.x  * _sensivity;
		cinemachineTargetPitch += playerInput.Look.y  * _sensivity;

		cinemachineTargetYaw = ClampAngle(cinemachineTargetYaw, float.MinValue, float.MaxValue);
		cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, BottomClamp, TopClamp);

		CinemachineCameraTarget.transform.rotation = Quaternion.Euler(cinemachineTargetPitch + CameraAngleOverride,
			cinemachineTargetYaw, angleFire);
	}


	public Vector3 GetCameraDirection()
	{
		Vector3 direction = Vector3.zero;
		direction = _mainCamera.forward * playerInput.Move.y;
		direction += _mainCamera.right * playerInput.Move.x;

		direction.Normalize();
		direction.y = 0f;
		return direction;
	}
	private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
	{
		if (lfAngle < -360f) lfAngle += 360f;
		if (lfAngle > 360f) lfAngle -= 360f;
		return Mathf.Clamp(lfAngle, lfMin, lfMax);
	}
}
