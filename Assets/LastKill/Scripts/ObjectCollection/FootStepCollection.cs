using System.Collections.Generic;
using UnityEngine;

namespace LastKill
{
    [CreateAssetMenu(fileName = "Footstep sound collection", menuName = "LastKill/Footstep sound Collection")]
    public class FootStepCollection : ScriptableObject
    {
        public List<AudioClip> walkSounds = new List<AudioClip>();
        public List<AudioClip> runSounds = new List<AudioClip>();
        public List<AudioClip> jumpStartSounds = new List<AudioClip>();
        public List<AudioClip> jumpLandSounds = new List<AudioClip>();
    }
}
