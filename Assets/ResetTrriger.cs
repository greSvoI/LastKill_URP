using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetTrriger : StateMachineBehaviour
{
    [SerializeField] private string nameBool;
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(!string.IsNullOrEmpty(nameBool))
        animator.SetBool(nameBool,false);
    }

  
}
