using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastKill
{
    public class WeaponController :  MonoBehaviour, IWeapon
    {

        [SerializeField] private string animWeaponID_1;
        [SerializeField] private string animWeaponID_2;
        [SerializeField] private string animWeaponID_3;

        private int hashWeaponID;
        private int hashDrawWeapon;

        [SerializeField] private int currentWeapon = 0;
        [SerializeField] private int nextWeapon;
        [SerializeField] private bool withWeapon = false;
        [SerializeField] private string animationState;

        private PlayerInput _input;
        private Animator _animator;

        private bool hasAnimator;

        public bool WithWeapon => withWeapon;
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
           
        }
        private void Reload()
        {
            if (withWeapon && !_animator.GetBool("Reload"))
            {
                _animator.SetBool("Reload", true);
            }
        }
        private void SelectWeapon()
        {
            nextWeapon = _input.CurrentWeapon;
            StartCoroutine(ChangeWeapon());
        }
        private void AssignAnimationIDs()
        {
            hashWeaponID = Animator.StringToHash("WeaponID");
            hashDrawWeapon = Animator.StringToHash("DrawWeapon");
        }
        private void TakeUpWeapon(int id)
        {
            currentWeapon = id;
            _animator.SetInteger(hashWeaponID, id);
            _animator.SetBool(hashDrawWeapon,true);
            withWeapon = true;
        }
        private void ChangeWeapon(int hand,int spine,bool _hand,bool _spine)
        {
            handWeapons[hand - 1].SetActive(_hand);
            spineWeapons[spine - 1].SetActive(_spine);
        }
        private IEnumerator  ChangeWeapon()
        {
            if (withWeapon)
            {
                if (nextWeapon == 0)
                {
                    PutAwayWeapon();

                    yield return new WaitForSeconds(0.35f);

                    ChangeWeapon(currentWeapon, currentWeapon, false, true);
                }
                else
                {
                    PutAwayWeapon();

                    yield return new WaitForSeconds(0.35f);

                    ChangeWeapon(currentWeapon, currentWeapon, false, true);

                    yield return new WaitForSeconds(0.5f);

                    TakeUpWeapon(nextWeapon);

                    yield return new WaitForSeconds(0.35f);

                    ChangeWeapon(nextWeapon, currentWeapon, true, false);
                }
            }
            else if (!withWeapon)
            {
                TakeUpWeapon(nextWeapon);
                yield return new WaitForSeconds(0.35f);
                ChangeWeapon(currentWeapon, currentWeapon, true, false);
            }

        }

        public void PutAwayWeapon()
        {

            _animator.SetBool(hashDrawWeapon, true);
            withWeapon = false;
        }

        //Надо как убрать это, использовать animator controller
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
    }

}