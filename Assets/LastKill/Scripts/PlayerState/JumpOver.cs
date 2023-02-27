using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastKill
{
    public class JumpOver : AbstractAbilityState
    {
        [SerializeField] private LayerMask vaultMask;
        [SerializeField] private float capsuleCastRadius = 0.2f;
        [SerializeField] private float capsuleCastDistance = 1.2f;
        [Space]
        [SerializeField] private float maxVaultHeight = 1.5f;
        [SerializeField] private float distanceAfterVault = 0.5f;
        [Space]
        [SerializeField] private string vaultAnimationState = "Jump Over";
        [Space]
        [SerializeField] private float vaultDuration = 0.4f;
        [SerializeField] private AnimationCurve movementCurve;

        private float _currentTweenWeight = 0;
        private float _tweenStep;

        private Vector3 _tweenStartPosition;
        private Quaternion _tweenStartRotation;
        private Vector3 _tweenBezierPoint;

        [SerializeField] private Vector3 _targetPosition;
        private Quaternion _targetRotation;

        private DetectionController _detection;
        private CustomDebug _debug;
        private ICapsule _capsule;
        private IMove _move;
        private void Awake()
        {
            _debug = GetComponent<CustomDebug>();
            _move = GetComponent<IMove>();
            _capsule = GetComponent<ICapsule>();
            _detection = GetComponent<DetectionController>();
        }
        public Vector3 temp;
        public override void OnStartState()
        {
            _animator.CrossFadeInFixedTime(vaultAnimationState, 0.05f);
            _capsule.DisableCollision();


            temp = transform.position;

            // calculate tween parameters
            _currentTweenWeight = 0;
            _tweenStep = Vector3.Distance(transform.position, _targetPosition) / vaultDuration;
            _tweenStartPosition = transform.position;
            _tweenStartRotation = transform.rotation;

            //// Draw char position
            //Vector3 p1 = _targetPosition + Vector3.up * _capsule.GetCapsuleRadius();
            //Vector3 p2 = _targetPosition + Vector3.up * (_capsule.GetCapsuleHeight() - _capsule.GetCapsuleRadius());
        }
        public override bool ReadyToStart()
        {

             return _input.Move != Vector2.zero && _input.Jump && FoundVault();
             //return _input.Move != Vector2.zero && _input.Jump && _detection.JumpOver;
        }
        //private bool OnJumpOver()
        //{
        //    if (Physics.Raycast(_jumpOverPos.position, transform.forward, out _groundHit, 0.5f))
        //    {
        //        if (_input.Fire)
        //        {
        //            move.StopRootMotion();
        //            //move.ApplyRootMotion(Vector3.one, true);
        //        }

        //    }
        //}
        public override void UpdateState()
        {
            _move.StopRootMotion();
            UpdateTween();

            if (_animator.IsInTransition(0)) return;

            var state = _animator.GetCurrentAnimatorStateInfo(0);

            if (state.IsName(vaultAnimationState))
            {
                float normalizedTime = Mathf.Repeat(state.normalizedTime, 1f);
                if (normalizedTime > 0.95f)
                    StopState();
            }
        }

        public override void OnStopState()
        {
            _move.StopRootMotion();
            _capsule.EnableCollision();
        }
        public Vector3 p1;
        public Vector3 p2;
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + transform.forward * capsuleCastDistance + Vector3.up * capsuleCastRadius, capsuleCastRadius);
            Gizmos.DrawWireSphere(transform.position + transform.forward * capsuleCastDistance + Vector3.up * (maxVaultHeight - capsuleCastRadius), capsuleCastRadius);
        }
        private bool FoundVault()
        {
             p1 = transform.position + transform.forward * capsuleCastDistance + Vector3.up * capsuleCastRadius;
             p2 = transform.position + transform.forward * capsuleCastDistance + Vector3.up * (maxVaultHeight - capsuleCastRadius);

            if (_debug)
            {
                _debug.DrawCapsule(p1, p2, capsuleCastRadius, Color.white);
                _debug.DrawLabel("Vault capsule cast", p1 + Vector3.up, Color.white);
            }

            if (Physics.CapsuleCast(p1, p2, capsuleCastRadius, -transform.forward, out RaycastHit capsuleHit,
                capsuleCastDistance, vaultMask, QueryTriggerInteraction.Ignore))
            {
                
                Vector3 startTop = capsuleHit.point;
                startTop.y = transform.position.y + maxVaultHeight + capsuleCastRadius;

                if (_debug)
                    _debug.DrawSphere(capsuleHit.point, capsuleCastRadius, Color.yellow, 1f);

                // check height
                if (Physics.SphereCast(startTop, capsuleCastRadius, Vector3.down, out RaycastHit top,
                    maxVaultHeight, vaultMask, QueryTriggerInteraction.Ignore))
                {
                    capsuleHit.normal = new Vector3(capsuleHit.normal.x, 0, capsuleHit.normal.z);
                    capsuleHit.normal.Normalize();

                    _targetPosition = capsuleHit.point + capsuleHit.normal * distanceAfterVault;

                    if (Physics.Raycast(_targetPosition, Vector3.down, out RaycastHit groundHit, 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore))
                        _targetPosition.y = groundHit.point.y;
                    else
                        _targetPosition.y = transform.position.y;

                    if (_debug)
                        _debug.DrawSphere(top.point, capsuleCastRadius, Color.blue, 1f);

                    // check if position is free to vault
                    Vector3 center = _targetPosition + Vector3.up;
                    if (Physics.OverlapSphere(center, _capsule.GetCapsuleRadius(), Physics.AllLayers, QueryTriggerInteraction.Ignore).Length != 0)
                        return false;

                    _targetRotation = Quaternion.LookRotation(capsuleHit.normal);
                    _tweenBezierPoint = top.point + Vector3.down * 0.3f;
                    return true;
                }
            }

            return false;
        }

        private void UpdateTween()
        {
            if (Mathf.Approximately(_currentTweenWeight, 1))
            {
                _move.ApplyRootMotion(Vector3.one, true);
                return;
            }

            _currentTweenWeight = Mathf.MoveTowards(_currentTweenWeight, 1, _tweenStep * Time.deltaTime);

            float weight = movementCurve.Evaluate(_currentTweenWeight);

            _move.SetPosition(BezierLerp(_tweenStartPosition, _targetPosition, _tweenBezierPoint, weight));
            transform.rotation = Quaternion.Lerp(_tweenStartRotation, _targetRotation, weight);
        }
        public Vector3 BezierLerp(Vector3 start, Vector3 end, Vector3 bezier, float t)
        {
            Vector3 point = Mathf.Pow(1 - t, 2) * start;
            point += 2 * (1 - t) * t * bezier;
            point += t * t * end;

            return point;
        }
    }

}