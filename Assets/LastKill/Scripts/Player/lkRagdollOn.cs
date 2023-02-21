using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lkRagdollOn : MonoBehaviour
{

    public Rigidbody []_rigidBodyRagdoll;

    private void Awake()
    {
        _rigidBodyRagdoll = GetComponentsInChildren<Rigidbody>();

    }
    public void RagdollEnable()
    {
		foreach (var rigidBody in _rigidBodyRagdoll)
		{
			rigidBody.isKinematic = false;
		}
	}
    public void RagdollDisable()
    {
        foreach(var rigidBody in _rigidBodyRagdoll)
        {
            rigidBody.isKinematic = true;
        }
    }
}
