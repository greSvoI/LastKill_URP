using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LastKill
{
    
    public class DetectionController : MonoBehaviour
    {
        private PlayerInput _input;
        private MoveController move;
        private Animator animator;



        private RaycastHit _groundHit;
        private RaycastHit _jumpOverHit;




        [SerializeField] private int _groundLayer;
        [SerializeField] private bool _jumpOver;


        [SerializeField] private Transform _jumpOverPos;

        public int GroundLayer => _groundLayer;
        public bool JumpOver => _jumpOver;
        
        void Start()
        {
            _input = GetComponent<PlayerInput>();
            animator = GetComponent<Animator>();
            move = GetComponent<MoveController>();

        }

        void Update()
        {
            Debug.DrawRay(_jumpOverPos.position, transform.forward * 1f, Color.black);

            if(Physics.Raycast(_jumpOverPos.position, transform.forward, out _jumpOverHit, 0.5f))
            {
                if (_jumpOverHit.collider.tag == "JumpOver")
                {
                    _jumpOver = true;
                }
                
            }
                else _jumpOver = false; 

            if (Physics.Raycast(transform.position, Vector3.down, out _groundHit, 0.5f))
            {
               _groundLayer = _groundHit.collider.gameObject.layer;
            }
            else
            {
              _groundLayer = -1;
            }

        }

    }

}