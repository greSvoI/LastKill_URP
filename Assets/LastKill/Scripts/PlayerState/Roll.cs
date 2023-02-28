using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastKill
{
    public class Roll : AbstractAbilityState
    {
        [SerializeField] private float _rollSpeed = 7f;
        [SerializeField] private float _capsuleHeightOnRoll = 1f;

        [SerializeField] private string _freeRollAnimation = "Roll.RollForward";
        [SerializeField] private string _aimedRollAnimation = "Roll.RollAimForward";
        

        private AudioController _audioController;
        [SerializeField] private AudioClip _freeRollAudio;
        [SerializeField] private AudioClip _aimedRollAudio;

        // direction and rotation
        private Vector3 _rollDirection = Vector3.forward;
        private float _targetRotation = 0;
        private Transform _camera = null;


        private void Awake()
        {
            _audioController = GetComponent<AudioController>();
            _camera = Camera.main.transform;   
        }
        public override void OnStartState()
        {
            nameState.text = "Roll";

            _animator.CrossFadeInFixedTime(weapon.WithWeapon() ? _aimedRollAnimation : _freeRollAnimation, 0.1f);

            _capsule.SetCapsuleSize(_capsuleHeightOnRoll, _capsule.GetCapsuleRadius());

            _rollDirection = transform.forward;
            _targetRotation = transform.eulerAngles.y;

            _move.ApplyRootMotion(Vector3.one, false);
     

            if (_input.Move != Vector2.zero)
            {
                // normalise input direction
                Vector3 moveDirection = new Vector3(_input.Move.x, 0.0f, _input.Move.y).normalized;
                _targetRotation = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + _camera.transform.eulerAngles.y;
                _rollDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
            }

            _rollDirection.Normalize();
        }

        public override bool ReadyToStart()
        {
            return _input.Roll && _move.IsGrounded();
        }

        public override void UpdateState()
        {
            _move.StopMovement();
            _move.Move(_rollDirection * _rollSpeed);
            // smooth rotate character
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, _targetRotation, 0), 0.1f);

            if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f && !_animator.IsInTransition(0))
                StopState();
        }
        public override void OnStopState()
        {
            base.OnStopState();
            _capsule.ResetCapsuleSize();
            _move.StopMovement();
            _move.StopRootMotion();
        }
    }
}
