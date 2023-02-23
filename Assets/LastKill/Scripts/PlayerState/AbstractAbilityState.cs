using System;
using UnityEngine;
using UnityEngine.UI;

namespace LastKill
{
    public abstract class AbstractAbilityState : MonoBehaviour
    {
        [SerializeField] private int statePriority = 0;

        public bool IsStateRunning { get; private set; }

        public event Action<AbstractAbilityState> abilityStopped = null;
        public event Action<AbstractAbilityState> abilityStarted = null;

       public int StatePriority { get { return statePriority; } }

        // Unity components
        protected Animator _animator = null;

        // start time and stop time
        public float StartTime { get; private set; } = 0;
        public float StopTime { get; private set; } = 0;

        protected IMove _move;
        protected ICapsule _capsule;
        protected PlayerInput _input;


        //Debug
        public Text nameState;
        protected virtual void Start()
        {
            _move = GetComponent<IMove>();
            _capsule = GetComponent<ICapsule>();
            _input = GetComponent<PlayerInput>();
            _animator = GetComponent<Animator>();
        }

        public void StartState()
        {
            IsStateRunning = true;
            StartTime = Time.time;
            OnStartState();
            abilityStarted?.Invoke(this);
        }
        public void StopState()
        {
            if (Time.time - StartTime < 0.1f)
                return;

            IsStateRunning = false;
            StopTime = Time.time;
            OnStopState();
            abilityStopped?.Invoke(this);
        }

        public abstract bool ReadyToStart();

        public abstract void OnStartState();

        public abstract void UpdateState();

        public virtual void OnStopState() { }


        protected void SetAnimationState(string stateName, float transitionDuration = 0.1f)
        {
            if (_animator.HasState(0, Animator.StringToHash(stateName)))
                _animator.CrossFadeInFixedTime(stateName, transitionDuration, 0);
        }

        /// <summary>
        /// Check if a specific state has finished
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        protected bool HasFinishedAnimation(string state)
        {
            var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

            if (_animator.IsInTransition(0)) return false;

            if (stateInfo.IsName(state))
            {
                float normalizeTime = Mathf.Repeat(stateInfo.normalizedTime, 1);
                if (normalizeTime >= 0.95f) return true;
            }

            return false;
        }
    }
}
