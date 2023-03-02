using UnityEngine;

namespace LastKill
{
    [CreateAssetMenu(fileName = "Weapon Data", menuName = "LastKill/Weapon Data")]
    public class WeaponsData : ScriptableObject
    {
        public GameObject weapon;
        public GameObject clip;
        public Transform transform;
        public Transform rightHandIK;
        public Transform leftHandIK;
        public AudioClip fire;
        public AudioClip reload;
        public AudioClip empty;
        public float bodyPoseID;
        public int reloadID;
        public bool onShoulders;
        public int damage;
        public int bulletCount;
        public int bulletClip;
    }
}
