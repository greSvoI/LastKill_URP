using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastKill
{
    public class JumpOver : AbstractAbilityState
    {
        [SerializeField] private LayerMask _obstacleMask;
        [SerializeField] private float _capsuleRadius = 0.2f;
        [SerializeField] private float _capsuleDistance = 1.2f;
        [Space]
        [SerializeField] private float _maxObstacleHeight = 1.5f;
        [SerializeField] private float _distanceAfterObstacle = 0.5f;
        [Space]
        [SerializeField] private string _obstacleAnimationState = "Jump Over";
        [Space]
        [SerializeField] private float _obstacleDuration = 0.4f;
        [SerializeField] private AnimationCurve _movementCurve;

        private float _currentTweenWeight = 0;
        private float _tweenStep;

        private Vector3 _targetPosition;
        private Vector3 _startPosition;
        private Vector3 _bezierPoint;

        private DetectionController _detection;

        private void Awake()
        {
            _detection = GetComponent<DetectionController>();
        }
        public override void OnStartState()
        {
            _animator.CrossFadeInFixedTime(_obstacleAnimationState, 0.05f);
            _capsule.DisableCollision();

            _currentTweenWeight = 0;
            _tweenStep = Vector3.Distance(transform.position, _targetPosition) / _obstacleDuration;
            _startPosition = transform.position;

        }
        public override bool ReadyToStart()
        {
            //return _input.Move != Vector2.zero && _input.Jump && Jump();
            return false;
        }
        [SerializeField] private AnimatorStateInfo state1;
        public override void UpdateState()
        {
            _move.StopRootMotion();

            UpdateJump();

            if (_animator.IsInTransition(0)) return;

            if(HasFinishedAnimation(_obstacleAnimationState))
                    StopState();

        }

        public override void OnStopState()
        {
            _move.StopRootMotion();
            _capsule.EnableCollision();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position + transform.forward * _capsuleDistance + Vector3.up * _capsuleRadius, _capsuleRadius);
            Gizmos.DrawWireSphere(transform.position + transform.forward * _capsuleDistance + Vector3.up * _maxObstacleHeight, _capsuleRadius);
            Gizmos.DrawSphere(_bezierPoint + Vector3.up * 0.3f, 0.1f);
        }
        private bool Jump()
        {
            Vector3 bottom = transform.position + transform.forward * _capsuleDistance + Vector3.up * _capsuleRadius;
            Vector3 top = transform.position + transform.forward * _capsuleDistance + Vector3.up * (_maxObstacleHeight - _capsuleRadius);

            if(Physics.CapsuleCast(bottom,top, _capsuleRadius,-transform.forward, out RaycastHit capsuleHit,
                _capsuleDistance,_obstacleMask, QueryTriggerInteraction.Ignore))
            {
                Vector3 startTop = capsuleHit.point;
                startTop.y = transform.position.y + _maxObstacleHeight + _capsuleRadius;

                if (Physics.SphereCast(startTop, _capsuleRadius, Vector3.down, out RaycastHit topHit,
                  _maxObstacleHeight, _obstacleMask, QueryTriggerInteraction.Ignore))
                {
                    _targetPosition = capsuleHit.point + capsuleHit.normal * _distanceAfterObstacle;

                    if (Physics.Raycast(_targetPosition, Vector3.down, out RaycastHit groundHit, 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore))
                        _targetPosition.y = groundHit.point.y;
                    else
                        _targetPosition.y = transform.position.y;

                    // check if position is free to jump over
                    Vector3 center = _targetPosition + Vector3.up;
                    if (Physics.OverlapSphere(center, _capsule.GetCapsuleRadius(), Physics.AllLayers, QueryTriggerInteraction.Ignore).Length != 0)
                        return false;

                    _bezierPoint = topHit.point + Vector3.down * 0.3f;
                    return true;
                }
            }

            return false;

        }
        private void UpdateJump()
        {
            //need to move along a parabola
            //transform.position = Vector3.MoveTowards(transform.position, _targetPosition,Time.deltaTime);

            if (_currentTweenWeight == 1f)
            {
                _move.ApplyRootMotion(Vector3.one, true);
                return;
            }

            _currentTweenWeight = Mathf.MoveTowards(_currentTweenWeight, 1, _tweenStep * Time.deltaTime);


            _move.SetPosition(QuadraticBezierCurves(_startPosition, _targetPosition, _bezierPoint, _currentTweenWeight));

        }


        Vector3 CubicBezierCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            Vector3 a = Vector3.Lerp(p0, p1, t);
            Vector3 b = Vector3.Lerp(p1, p2, t);
            Vector3 c = Vector3.Lerp(p2, p3, t);
            Vector3 d = Vector3.Lerp(a, b, t);
            Vector3 e = Vector3.Lerp(b, c, t);
            return  Vector3.Lerp(d, e, t);
        }

        public Vector3 QuadraticBezierCurves(Vector3 start, Vector3 end, Vector3 bezier, float t)
        {
            //Vector3 point = Mathf.Pow(1 - t, 2) * start;
            //point += 2 * (1 - t) * t * bezier;
            //point += t * t * end;
            Vector3 point = (1f - t) * (1f - t) * start + 2f * (1f - t) * t * bezier + t * t * end;
            return point;
        }
    }

}