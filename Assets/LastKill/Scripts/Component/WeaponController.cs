using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace LastKill
{
    public class WeaponController :  MonoBehaviour, IWeapon
    {

        [SerializeField] private string animWeaponID_1;
        [SerializeField] private string animWeaponID_2;
        [SerializeField] private string animWeaponID_3;

        private int hashWeaponID;
        private int hashDrawWeapon;
        private int  hashIsAim;
        private int hashOnAim;
        private int hashUpperPose;

        [SerializeField] private int currentWeapon = 0;
        [SerializeField] private int nextWeapon;
        [SerializeField] private bool withWeapon = false;
        [SerializeField] private string animationState;

        //[SerializeField] private RigLayer rigLayer;

        private PlayerInput _input;
        private Animator _animator;

        private bool hasAnimator;
        private bool equipWeapon = false;

        public bool WithWeapon => withWeapon;
        public WeaponsData rifle;

        public List<GameObject> spineWeapons;
        public List<GameObject> handWeapons;

        private void Awake()
        {
            _input = GetComponent<PlayerInput>();
            _animator = GetComponent<Animator>();
            _input.OnSelectWeapon += SelectWeapon;
            _input.OnReload += Reload;
        }
        private void Start()
        {
            hasAnimator = TryGetComponent(out _animator);
            AssignAnimationIDs();
        }
        private void Update()
        {
            //if (withWeapon)
            //{
            //    _animator.SetBool(hashIsAim, true);
            //   //_animator.SetBool(hashOnAim, true);
            //}
            //else
            //{
            //    _animator.SetBool(hashIsAim, false);
            //    //_animator.SetBool(hashOnAim, false);
            //}
        }
        private void AssignAnimationIDs()
        {
            hashWeaponID = Animator.StringToHash("WeaponID");
            hashDrawWeapon = Animator.StringToHash("DrawWeapon");
            hashOnAim = Animator.StringToHash("noAiming");
            hashIsAim = Animator.StringToHash("isAiming");
            hashUpperPose = Animator.StringToHash("UpperBodyID");
        }
        private void Reload()
        {
            if (withWeapon && !_animator.GetBool("Reload"))
            {
                Debug.Log("Reload");
                _animator.CrossFadeInFixedTime("Reload", 0.2f, 2);
            }
        }
        private void SelectWeapon()
        {
            nextWeapon = _input.CurrentWeapon;
            withWeapon = true;
            if (currentWeapon == 0)
            {   currentWeapon = nextWeapon;
                equipWeapon = true;
            }
            else if(currentWeapon == nextWeapon)
            {
                equipWeapon = false;
                withWeapon = false;
                _animator.SetBool(hashDrawWeapon, true);
                _animator.SetBool(hashIsAim, false);
                _animator.SetBool(hashOnAim, false);
                return;
            }
            else if(currentWeapon != nextWeapon)
            {
                equipWeapon = false;
                _animator.SetBool(hashDrawWeapon, true);
                _animator.SetBool(hashIsAim, false);
                _animator.SetBool(hashOnAim, false);
                return;
            }

            _animator.SetBool(hashOnAim, true);
            _animator.SetBool(hashIsAim, true);
            _animator.SetBool(hashDrawWeapon, true);
            _animator.SetInteger(hashWeaponID, nextWeapon);
            _animator.SetFloat(hashUpperPose, nextWeapon == 3 ? 2 : nextWeapon);

        }


        public void DownWeapon()
        {
            _animator.SetFloat(hashUpperPose, 0);
            _animator.SetBool(hashDrawWeapon, true);
            _animator.SetBool(hashIsAim, false);
            _animator.SetBool(hashOnAim, false);
            equipWeapon = false;
        }
        private void EquipWeapon()
        {
            _animator.SetInteger(hashWeaponID, currentWeapon);
            _animator.SetFloat(hashUpperPose, currentWeapon == 3 ? 2 : currentWeapon);
            _animator.SetBool(hashDrawWeapon,true);
            _animator.SetBool(hashIsAim, true);
            _animator.SetBool(hashOnAim, true);
            equipWeapon = true;
        }
        private void ChangeWeapon(int hand,int spine,bool _hand,bool _spine)
        {
            handWeapons[hand - 1].SetActive(_hand);
            spineWeapons[spine - 1].SetActive(_spine);
        }

        private void WeaponHand(int id,bool state)
        {
            handWeapons[id-1].SetActive(state);
        }
        private void WeaponSpine(int id,bool state)
        {
            spineWeapons[id-1].SetActive(state);
        }
        //Pause put away weapon to get next 
        #region
        //private IEnumerator DrawWeaponCheck()
        //{
        //    while (true)
        //    {
        //        if (_animator.GetBool(hashDrawWeapon))
        //        {
        //            DrawWeapon();
        //            equipWeapon = true;
        //            break;
        //        }
        //        yield return null;
        //    }

        //}
        //private IEnumerator  ChangeWeapon()
        //{
        //    if (withWeapon)
        //    {
        //        if (nextWeapon == 0)
        //        {
        //            PutAwayWeapon();

        //            yield return new WaitForSeconds(0.35f);

        //            ChangeWeapon(currentWeapon, currentWeapon, false, true);
        //        }
        //        else
        //        {
        //            PutAwayWeapon();

        //            yield return new WaitForSeconds(0.35f);

        //            ChangeWeapon(currentWeapon, currentWeapon, false, true);

        //            yield return new WaitForSeconds(0.5f);

        //            TakeUpWeapon(nextWeapon);

        //            yield return new WaitForSeconds(0.35f);

        //            ChangeWeapon(nextWeapon, currentWeapon, true, false);
        //        }
        //    }
        //    else if (!withWeapon)
        //    {
        //        TakeUpWeapon(nextWeapon);
        //        yield return new WaitForSeconds(0.35f);
        //        ChangeWeapon(currentWeapon, currentWeapon, true, false);
        //    }

        //}
        #endregion
        //Надо как то убрать это, использовать animator controller
        protected void SetAnimationState(string stateName, float transitionDuration = 0.1f)
        {
           // if (_animator.HasState(2, Animator.StringToHash(stateName)))
          //  {
                _animator.CrossFadeInFixedTime(stateName, transitionDuration, 2);
           // }
        }
        protected bool HasFinishedAnimation(string state)
        {
            var stateInfo = _animator.GetCurrentAnimatorStateInfo(2);

            if (_animator.IsInTransition(2)) return false;

            if (stateInfo.IsName(state))
            {
                float normalizeTime = Mathf.Repeat(stateInfo.normalizedTime, 1);
                if (normalizeTime >= 0.95f) return true;
            }

            return false;
        }

       

        //Animation Event
        private void HighLeft()
        {
            WeaponHand(currentWeapon, equipWeapon);
            WeaponSpine(currentWeapon, !equipWeapon);
            if(currentWeapon != nextWeapon)
            {
                WeaponHand(nextWeapon, !equipWeapon);
                WeaponSpine(nextWeapon, equipWeapon);
                _animator.SetBool(hashOnAim, true);
                _animator.SetBool(hashIsAim, true);
                _animator.SetInteger(hashWeaponID, nextWeapon);
                _animator.SetFloat(hashUpperPose, nextWeapon == 3 ? 2 : nextWeapon);
                currentWeapon = nextWeapon;
                return;
            }

            if (!equipWeapon)
            {
                _animator.SetInteger(hashWeaponID, 0);
                _animator.SetFloat(hashUpperPose, 0);
                currentWeapon = 0;
            }
        }
        private void HighRight()
        {
            WeaponHand(currentWeapon, equipWeapon);
            WeaponSpine(currentWeapon, !equipWeapon);

            if (currentWeapon != nextWeapon)
            {
                WeaponHand(nextWeapon, !equipWeapon);
                WeaponSpine(nextWeapon, equipWeapon);
                _animator.SetBool(hashOnAim, true);
                _animator.SetBool(hashIsAim, true);
                _animator.SetInteger(hashWeaponID, nextWeapon);
                _animator.SetFloat(hashUpperPose, nextWeapon == 3 ? 2 : nextWeapon);
                currentWeapon = nextWeapon;
                return;

            }
            if (!equipWeapon)
            {
                _animator.SetInteger(hashWeaponID, 0);
                _animator.SetFloat(hashUpperPose, 0);
                currentWeapon = 0;
            }
        }
        private void LowLeft()
        {
            WeaponHand(currentWeapon, equipWeapon);
            WeaponSpine(currentWeapon, !equipWeapon);

            if (currentWeapon != nextWeapon)
            {
                WeaponHand(nextWeapon, !equipWeapon);
                WeaponSpine(nextWeapon, equipWeapon);
                _animator.SetBool(hashOnAim, true);
                _animator.SetBool(hashIsAim, true);
                _animator.SetInteger(hashWeaponID, nextWeapon);
                _animator.SetFloat(hashUpperPose, nextWeapon == 3 ? 2 : nextWeapon);
                currentWeapon = nextWeapon;
                return;

            }
            if (!equipWeapon)
            {
                _animator.SetInteger(hashWeaponID, 0);
                _animator.SetFloat(hashUpperPose, 0);
                currentWeapon = 0;
            }

        }
    }
   
}