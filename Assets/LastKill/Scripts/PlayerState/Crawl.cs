using System;
using UnityEngine;

namespace LastKill
{
    public class Crawl : AbstractAbilityState
    {
        [Header("Speed")]
        [SerializeField] private float crawlSpeed = 2f;
        [SerializeField] private float capsuleHeight = 0.5f;

        [Header("Parameters")]
        [SerializeField] private LayerMask obstaclesMask;
        [SerializeField] private float maxHeightToStartCrawl = 0.75f;

        [Header("Animation States")]
        [SerializeField] private string startToCrawlAnimation = "Stand to Crawl";
        [SerializeField] private string stopToSdantAnimation = "Crawl to Stand";

        private int hashStartCrawl;
        private int hashStopCrawl;

        private bool startingCrawl = false;
        private bool stoppingCrawl = false;

        private float defaultCapsuleRadius = 0;


        private void Awake()
        {
            hashStartCrawl = Animator.StringToHash(startToCrawlAnimation);
            hashStopCrawl = Animator.StringToHash(stopToSdantAnimation);
        }
        public override void OnStartState()
        {
            startingCrawl = true;
            _animator.SetAnimationState(hashStartCrawl,0);
        }

        public override void OnStopState()
        {
            // reset control variables
            startingCrawl = false;
            stoppingCrawl = false;

            _capsule.ResetCapsuleSize();
        }

        public override bool ReadyToStart()
        {
            if (!_move.IsGrounded()) return false;
            if (_input.Crawl || CanGetUp()) return true;
            return false;
        }

        private bool CanGetUp()
        {
            RaycastHit hit;

            if (Physics.SphereCast(transform.position, defaultCapsuleRadius, Vector3.up, out hit,
                maxHeightToStartCrawl, obstaclesMask, QueryTriggerInteraction.Ignore))
            {
                if (hit.point.y - transform.position.y > capsuleHeight)
                    return true;
            }
            return false;
        }

        public override void UpdateState()
        {
            if(startingCrawl)
            {
                if(!_animator.HasFinishedAnimation(startToCrawlAnimation,0))
                    startingCrawl = false;
            }
            if(stoppingCrawl)
            {
                if(_animator.HasFinishedAnimation(0))
                    StopState();
                return;
            }
            _move.Move(_input.Move, crawlSpeed);

            if(!_input.Crawl && !CanGetUp())
            {
              _animator.SetAnimationState(hashStopCrawl,0);
                stoppingCrawl = true;
                _move.StopMovement();
            }
        }
    }

}