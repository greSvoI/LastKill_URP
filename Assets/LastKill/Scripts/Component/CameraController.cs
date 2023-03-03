using UnityEngine;
using Cinemachine;

namespace LastKill
{
    public class CameraController : MonoBehaviour  ,iCamera
    {
		[SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
		[SerializeField] private Transform _mainCamera;
		[SerializeField] private Transform _cameraRoot;
		public float Sensitivity;
		public float SensitivityNormal;

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

		public Transform GetTransform => _mainCamera.transform;

		PlayerInput _input;
		[SerializeField] private LayerMask layerMask;
		[SerializeField] private Transform debugTransform;

		private void Start()
		{
			_input = GetComponent<PlayerInput>();
		}
		public Transform MainCamera => _mainCamera;
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
        private void Update()
        {
            if(_input.Aim)
			{
				aimVirtualCamera.gameObject.SetActive(true);
			}
			else
            {
				aimVirtualCamera.gameObject.SetActive(false);
            }
			Vector2 screenPoint = new Vector2(Screen.width / 2f, Screen.height / 2);

			Ray ray = Camera.main.ScreenPointToRay(screenPoint);
			if(Physics.Raycast(ray ,out RaycastHit hit,200f,layerMask))
            {
				debugTransform.position = hit.point;
            }


        }
        private void LateUpdate()
        {
			FreeMovementCamera();
        }
        public void FreeMovementCamera()
		{
			cinemachineTargetYaw += _input.Look.x * Sensitivity;
			cinemachineTargetPitch += _input.Look.y * Sensitivity;

			cinemachineTargetYaw = ClampAngle(cinemachineTargetYaw, float.MinValue, float.MaxValue);
			cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, BottomClamp, TopClamp);

			CinemachineCameraTarget.transform.rotation = Quaternion.Euler(cinemachineTargetPitch + CameraAngleOverride,
				cinemachineTargetYaw, angleFire);
		}
		public void SetSensitivity(float sensetivity)
        {
			Sensitivity = sensetivity;
        }


		public Vector3 GetCameraDirection()
		{
			Vector3 direction = Vector3.zero;
			direction = _mainCamera.forward * _input.Move.y;
			direction += _mainCamera.right * _input.Move.x;

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
}
