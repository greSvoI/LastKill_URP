using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastKill
{
    public class JumpOver : AbstractAbilityState
    {
        [SerializeField] private LayerMask obstacleMask;
        [SerializeField] private float capsuleRadius = 0.2f;
        [SerializeField] private float capsuleDistance = 1.2f;
        [Space]
        [SerializeField] private float maxObstacleHeight = 1.5f;
        [SerializeField] private float distanceAfterObstacle = 0.5f;
        [Space]
        [SerializeField] private string obstacleAnimationState = "Jump Over";
        [Space]
        [SerializeField] private float obstacleDuration = 0.4f;

        private float currentTweenWeight = 0;
        private float tweenStep;

        private Vector3 targetPosition;
        private Vector3 startPosition;
        private Vector3 bezierPoint;

        private int hashAnimState;
        private void Awake()
        {
            hashAnimState = Animator.StringToHash(obstacleAnimationState);
        }
        public override void OnStartState()
        {
            _animator.SetAnimationState(hashAnimState, 0 , 0.05f);
            _capsule.DisableCollision();

            currentTweenWeight = 0;
            tweenStep = Vector3.Distance(transform.position, targetPosition) / obstacleDuration;
            startPosition = transform.position;
        }
        public override bool ReadyToStart()
        {
            return _input.Move == Vector2.zero && _input.Jump && Jump();
        }
        [SerializeField] private AnimatorStateInfo state1;
        public override void UpdateState()
        {
            _move.StopRootMotion();

            UpdateJump();

            _animator.HasFinishedAnimation(0);
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
            Gizmos.DrawWireSphere(transform.position + transform.forward * capsuleDistance + Vector3.up * capsuleRadius, capsuleRadius);
            Gizmos.DrawWireSphere(transform.position + transform.forward * capsuleDistance + Vector3.up * maxObstacleHeight, capsuleRadius);
            Gizmos.DrawSphere(bezierPoint + Vector3.up * 0.3f, 0.1f);
        }
        private bool Jump()
        {
            Vector3 bottom = transform.position + transform.forward * capsuleDistance + Vector3.up * capsuleRadius;
            Vector3 top = transform.position + transform.forward * capsuleDistance + Vector3.up * (maxObstacleHeight - capsuleRadius);

            if(Physics.CapsuleCast(bottom,top, capsuleRadius,-transform.forward, out RaycastHit capsuleHit,
                capsuleDistance,obstacleMask, QueryTriggerInteraction.Ignore))
            {
                Vector3 startTop = capsuleHit.point;
                startTop.y = transform.position.y + maxObstacleHeight + capsuleRadius;

                if (Physics.SphereCast(startTop, capsuleRadius, Vector3.down, out RaycastHit topHit,
                  maxObstacleHeight, obstacleMask, QueryTriggerInteraction.Ignore))
                {
                    targetPosition = capsuleHit.point + capsuleHit.normal * distanceAfterObstacle;

                    if (Physics.Raycast(targetPosition, Vector3.down, out RaycastHit groundHit, 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore))
                        targetPosition.y = groundHit.point.y;
                    else
                        targetPosition.y = transform.position.y;

                    // check if position is free to jump over
                    Vector3 center = targetPosition + Vector3.up;
                    if (Physics.OverlapSphere(center, _capsule.GetCapsuleRadius(), Physics.AllLayers, QueryTriggerInteraction.Ignore).Length != 0)
                        return false;

                    bezierPoint = topHit.point + Vector3.down * 0.3f;
                    return true;
                }
            }

            return false;

        }
        private void UpdateJump()
        {
            //need to move along a parabola
            //transform.position = Vector3.MoveTowards(transform.position, _targetPosition,Time.deltaTime);

            if (currentTweenWeight == 1f)
            {
                _move.ApplyRootMotion(Vector3.one, true);
                return;
            }

            currentTweenWeight = Mathf.MoveTowards(currentTweenWeight, 1, tweenStep * Time.deltaTime);

            _move.SetPosition(QuadraticBezierCurves(startPosition, targetPosition, bezierPoint, currentTweenWeight));

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