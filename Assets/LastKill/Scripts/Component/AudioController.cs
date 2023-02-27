using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastKill
{
    public class AudioController : MonoBehaviour
    {
        [SerializeField] private FootStepCollection _asphalt;
        [SerializeField] private AudioSource voiceSource;
        [SerializeField] private AudioSource effectsSource;


        private void Start()
        {
          
        }
        internal void PlayVoice(AudioClip clip)
        {
            if (effectsSource == null) return;

            effectsSource.clip = clip;
            effectsSource.Play();
        }

        private string NameTag()
        {
            RaycastHit hit;
  
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 1f))

            return hit.collider.tag;

            else return null;
        }
        private  void WalkFootStep()
        {
            effectsSource.PlayOneShot(_asphalt.walkSounds[UnityEngine.Random.Range(0,_asphalt.walkSounds.Count)]);

        }
      
    }

}