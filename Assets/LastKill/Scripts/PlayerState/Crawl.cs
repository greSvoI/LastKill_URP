using System;
using UnityEngine;

namespace LastKill
{
    public class Crawl : AbstractAbilityState
    {
        [SerializeField] private float _crawlSpeed = 2f;
        [SerializeField] private float _capsuleHeight = 0.5f;

        [Header("Parameters")]
        [SerializeField] private LayerMask _obstaclesMask;
        [SerializeField] private float _maxHeightToStartCrawl = 0.75f;

        [Header("Animation States")]
        [SerializeField] private string _startToCrawlAnimation = "Stand to Crawl";
        [SerializeField] private string _stopToSdantAnimation = "Crawl to Stand";

        private AudioController _audioController;

        private bool _startingCrawl = false;
        private bool _stoppingCrawl = false;

        private float _defaultCapsuleRadius = 0;


        private void Awake()
        {
            _audioController = GetComponent<AudioController>();
        }
        public override void OnStartState()
        {
            nameState.text = "Crawl";
            _startingCrawl = true;
            SetAnimationState(_startToCrawlAnimation);
        }

        public override void OnStopState()
        {
            // reset control variables
            _startingCrawl = false;
            _stoppingCrawl = false;

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

            if (Physics.SphereCast(transform.position, _defaultCapsuleRadius, Vector3.up, out hit,
                _maxHeightToStartCrawl, _obstaclesMask, QueryTriggerInteraction.Ignore))
            {
                if (hit.point.y - transform.position.y > _capsuleHeight)
                    return true;
            }

            return false;
        }

        public override void UpdateState()
        {
            if(_startingCrawl)
            {
                if (_animator.IsInTransition(0)) return;

                if (!_animator.GetCurrentAnimatorStateInfo(0).IsName(_startToCrawlAnimation))
                    _startingCrawl = false;

                return;
            }
            if(_stoppingCrawl)
            {
                if (_animator.IsInTransition(0)) return;

                if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.85f)
                    StopState();

                return;
            }
            _move.Move(_input.Move, _crawlSpeed);

            if(!_input.Crawl && !CanGetUp())
            {
                SetAnimationState(_stopToSdantAnimation);
                _stoppingCrawl = true;
                _move.StopMovement();
            }
        }
    }

}