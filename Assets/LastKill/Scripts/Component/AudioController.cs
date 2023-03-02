using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastKill
{
    public class AudioController : MonoBehaviour , IAudio
    {
        [SerializeField] private FootStepCollection stepAsphalt;
        [SerializeField] private FootStepCollection stepMetall;
        [SerializeField] private AudioClip rollFx;

        [SerializeField] private AudioSource voiceSource;
        [SerializeField] private AudioSource effectsSource;
        
        private DetectionController _detection;

        private void Start()
        {
            _detection = GetComponent<DetectionController>();
        }
        public void PlayEffect(AudioClip clip)
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
        private void OnLand()
        {

        }
        private  void WalkFootStep()
        {
           switch(_detection.GroundLayer)
            {
                case 6: 
                       effectsSource.PlayOneShot(stepAsphalt.walkSounds[UnityEngine.Random.Range(0,stepAsphalt.walkSounds.Count)]);
                       break;

                case 7:
                       effectsSource.PlayOneShot(stepMetall.walkSounds[UnityEngine.Random.Range(0, stepMetall.walkSounds.Count)]);
                       break;

            }
        }
        private void RollEvent()
        {
            effectsSource.PlayOneShot(rollFx);
        }

        
    }

}