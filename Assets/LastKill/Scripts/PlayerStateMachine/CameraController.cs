using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastKill
{
	public class CameraController : MonoBehaviour
	{
		[SerializeField] private float _sensivity;
		[SerializeField] private float _rotationVelocity;

		private Vector3 _direction;

		public Camera cameraMain;

		[Header("Cinemachine")]
		[Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
		public GameObject CinemachineCameraTarget;

		[Tooltip("How far in degrees can you move the camera up")]
		public float TopClamp = 70.0f;

		[Tooltip("How far in degrees can you move the camera down")]
		public float BottomClamp = -30.0f;

		[Tooltip("How far in degrees can you move the camera right")]
		public float RightClamp = 90.0f;

		[Tooltip("How far in degrees can you move the camera left")]
		public float LeftClamp = 90.0f;


		[Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
		public float CameraAngleOverride = 0.0f;

		[Tooltip("For locking the camera position on all axis")]
		public bool LockCameraPosition = false;

		[Range(0.0f, 0.3f)]
		public float RotationSmoothTime = 0.12f;

		// cinemachine
		[SerializeField] private float _cinemachineTargetYaw;
		[SerializeField] private float _cinemachineTargetY;
		[SerializeField] private float _cinemachineTargetX;


		private void Awake()
		{
			cameraMain = Camera.main;
			
		}
		public Quaternion PlayerTargetRotation(Vector2 moveInput)
		{
			float targetRotation = Mathf.Atan2(moveInput.x, moveInput.y) * Mathf.Rad2Deg +
								 cameraMain.transform.eulerAngles.y;
			float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref _rotationVelocity,
				RotationSmoothTime);
			return Quaternion.Euler(0.0f, rotation, 0.0f);
		}
		public void CameraRotate(Vector2 lookInput, bool IsStrafe)
		{
		
			_cinemachineTargetYaw += lookInput.x  * _sensivity;

			_cinemachineTargetY += lookInput.y  * _sensivity;
			_cinemachineTargetX += lookInput.x * _sensivity;

			_cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);

			_cinemachineTargetY = ClampAngle(_cinemachineTargetY, BottomClamp, TopClamp);
			_cinemachineTargetX = ClampAngle(_cinemachineTargetX, LeftClamp, RightClamp);

			if (IsStrafe)
			{
				CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetY + CameraAngleOverride,
				_cinemachineTargetYaw, 0.0f);
			}
			else
			{
				CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetY + CameraAngleOverride,
				_cinemachineTargetX, 0.0f);
			}

		}
		public Vector3 GetCameraDirection(Vector2 moveInput)
		{
			_direction = Vector3.zero;

			_direction = cameraMain.transform.forward * moveInput.y;
			_direction += cameraMain.transform.right * moveInput.x;

			_direction.y = 0f;
			return _direction;
		}
		public void SetSensivity(float sensivity)
		{
			_sensivity = sensivity;
		}
		private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
		{
			if (lfAngle < -360f) lfAngle += 360f;
			if (lfAngle > 360f) lfAngle -= 360f;
			return Mathf.Clamp(lfAngle, lfMin, lfMax);
		}
	}
}
