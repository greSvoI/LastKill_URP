using System;
using UnityEngine;

namespace LastKill
{ 
public abstract class AbstractState : MonoBehaviour
    {
        private bool _isStateRunning;

        [SerializeField] private int _statePriority = 0;
        public bool IsStateRunning => _isStateRunning;
        public int StatePriority => _statePriority;

        public event Action<AbstractState> stateStarted = null;
        public event Action<AbstractState> stateSopped = null;

        protected Animator _animator = null;
        protected PlayerInput _input;

        protected virtual void Start()
        {
            _animator = GetComponent<Animator>();
            _input = GetComponent<PlayerInput>();
        }

        public void StartState()
        {
            _isStateRunning = true;
            OnStartState();
        }
        public abstract bool ReadyToStart();
        public void StopState()
        {
            _isStateRunning = false;
            OnStopState();
        }
        public abstract void OnStartState();
        public abstract void UpdateState();
        public abstract void OnStopState();

        protected void SetAnimationState(string stateName, float duration = 0.1f)
        {
            if (_animator.HasState(0, Animator.StringToHash(stateName)))
                _animator.CrossFadeInFixedTime(stateName, duration, 0);
        }
        protected bool IsFinishedAnimation(string stateName)
        {
            var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

            if (_animator.IsInTransition(0)) return false;

            if (stateInfo.IsName(stateName))
            {
                float normalizeTime = Mathf.Repeat(stateInfo.normalizedTime, 1);
                if (normalizeTime >= 0.95f) return true;
            }

            return false;
        }

    }
}
