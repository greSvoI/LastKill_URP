using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastKill
{
    public class AudioController : MonoBehaviour
    {

        [SerializeField] private AudioSource voiceSource;
        [SerializeField] private AudioSource effectsSource;
        internal void PlayVoice(AudioClip clip)
        {
            if (effectsSource == null) return;

            effectsSource.clip = clip;
            effectsSource.Play();
        }
    }

}