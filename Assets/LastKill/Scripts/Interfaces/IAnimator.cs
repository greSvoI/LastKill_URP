using UnityEngine;

namespace LastKill
{
    public interface IAnimator
    {
        public Animator Animator { get;}
        public bool isAiming { get; }
        public void SetAnimationState(int hashName, int layerIndex, float transitionDuration = 0.1f);
        public void SetAnimationState(string stateName,int layerIndex, float transitionDuration = 0.1f);
        public bool HasFinishedAnimation(string stateName, int layerIndex);
        public bool HasFinishedAnimation(int layerIndex);
        public void StrafeUpdate();
        public void LocomotionUpdate();
        public void CrouchUpdate();
        public void CrawlUpdate();
        public void LadderUpdate();
    }
}
