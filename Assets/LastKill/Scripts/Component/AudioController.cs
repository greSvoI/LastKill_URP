using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastKill
{
    public class AudioController : MonoBehaviour
    {
        [SerializeField] private FootStepCollection _stepAsphalt;
        [SerializeField] private FootStepCollection _stepMetall;
        [SerializeField] private AudioSource voiceSource;
        [SerializeField] private AudioSource effectsSource;
        
        private DetectionController _detection;

        private void Start()
        {
            _detection = GetComponent<DetectionController>();
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
           switch(_detection.GroundLayer)
            {
                case 6: 
                       effectsSource.PlayOneShot(_stepAsphalt.walkSounds[UnityEngine.Random.Range(0,_stepAsphalt.walkSounds.Count)]);
                       break;

                case 7:
                       effectsSource.PlayOneShot(_stepMetall.walkSounds[UnityEngine.Random.Range(0, _stepMetall.walkSounds.Count)]);
                       break;

            }
        }
      
    }

}