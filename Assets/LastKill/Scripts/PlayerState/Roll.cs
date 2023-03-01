using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastKill
{
    public class Roll : AbstractAbilityState
    {
        [SerializeField] private string freeRollAnimation = "Roll.RollForward";
        [SerializeField] private string aimedRollAnimation = "Roll.RollAimForward";

        private int hashAimRoll;
        private int hashFreeRoll;
        private int layerIndex = 0;

        [SerializeField] private float rollSpeed = 7f;
        [SerializeField] private float capsuleHeightOnRoll = 1f;

        [SerializeField] private AudioClip _freeRollAudio;
        [SerializeField] private AudioClip _aimedRollAudio;

        // direction and rotation
        private Vector3 rollDirection = Vector3.forward;
        private float targetRotation = 0;
        private Transform _camera = null;


        private void Awake()
        {
            _camera = Camera.main.transform;   
        }
        public override void OnStartState()
        {
            nameState.text = "Roll";

            _animator.SetAnimationState(_weapon.WithWeapon() ? aimedRollAnimation : freeRollAnimation, 0, 0.1f);

            _capsule.SetCapsuleSize(capsuleHeightOnRoll, _capsule.GetCapsuleRadius());

            rollDirection = transform.forward;
            targetRotation = transform.eulerAngles.y;

            if (_input.Move != Vector2.zero)
            {
                // normalise input direction
                Vector3 moveDirection = new Vector3(_input.Move.x, 0.0f, _input.Move.y).normalized;
                targetRotation = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + _camera.transform.eulerAngles.y;
                rollDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;
            }

            rollDirection.Normalize();
        }

        public override bool ReadyToStart()
        {
            return _input.Roll && _move.IsGrounded();
        }

        public override void UpdateState()
        {
            _move.StopMovement();
            _move.Move(rollDirection * rollSpeed);
            // smooth rotate character
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, targetRotation, 0), 0.1f);

           if(_animator.HasFinishedAnimation(layerIndex))
                StopState();
        }
        public override void OnStopState()
        {
            base.OnStopState();
            _capsule.ResetCapsuleSize();
            _move.StopMovement();
        }
    }
}
