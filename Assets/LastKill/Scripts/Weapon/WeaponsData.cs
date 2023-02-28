using UnityEngine;

namespace LastKill
{
    [CreateAssetMenu(fileName = "Weapon Data", menuName = "LastKill/Weapon Data")]
    public class WeaponsData : ScriptableObject
    {
        public GameObject weapon;
        public GameObject clip;
        public Transform rightHand;
        public Transform leftHand;
        public AudioClip fire;
        public AudioClip reload;
        public AudioClip empty;
        public int damage;
        public int bulletCount;
        public int bulletClip;
    }
}
