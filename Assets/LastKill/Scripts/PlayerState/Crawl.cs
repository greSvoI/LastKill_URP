using UnityEngine;

namespace LastKill
{
    public class Crawl : AbstractAbilityState
    {
        [SerializeField] private float _crawlSpeed = 2f;
        [SerializeField] private float _capsuleHeight = 0.5f;

        [Header("Parameters")]
        [SerializeField] private LayerMask obstaclesMask;
        [SerializeField] private float MaxHeightToStartCrawl = 0.75f;

        [Header("Animation States")]
        [SerializeField] private string _startCrawlAnimationState = "Stand to Crawl";
        [SerializeField] private string _stopCrawlAnimationState = "Crawl to Stand";

        private bool _startingCrawl = false;
        private bool _stoppingCrawl = false;

        private float _defaultCapsuleRadius = 0;

        public override void OnStartState()
        {

            _startingCrawl = true;

            SetAnimationState(_startCrawlAnimationState);
        }

        public override void OnStopState()
        {
           
        }

        public override bool ReadyToStart()
        {
            if (!_move.IsGrounded()) return false;
            if (_input.IsCrawl) return true;
            return false;
        }

        public override void UpdateState()
        {

        }
    }

}