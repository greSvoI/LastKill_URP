using UnityEngine;

namespace LastKill
{
    public interface IAnimator
    {
        public void SetAnimationState(int hashName, int layerIndex, float transitionDuration = 0.1f);
        public void SetAnimationState(string stateName,int layerIndex, float transitionDuration = 0.1f);
        public bool HasFinishedAnimation(string stateName, int layerIndex);
        public bool HasFinishedAnimation(int layerIndex);
        public bool isMatchTarget();
        public void MatchTarget(Vector3 position, Quaternion quaternion,AvatarTarget avatar, MatchTargetWeightMask mask,float startTime,float targetTime);
        public AnimatorStateInfo GetCurrentStateInfo(int layerIndex);
        public int GetLayerIndex(string nameLayer);
        public void StrafeUpdate();
    }
}
