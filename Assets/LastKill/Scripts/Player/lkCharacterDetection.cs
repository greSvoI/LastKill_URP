using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lkCharacterDetection : MonoBehaviour
{
	Vector3 raycastFloorPos;
	Vector3 floorMovement;
	Vector3 gravity;
	Vector3 CombinedRaycast;

	public bool OnGround(float stepHeight)
	{
		RaycastHit hit;
		return Physics.Raycast(transform.position + new Vector3(0, 0.3f, 0), Vector3.down, out hit, stepHeight);
	}
	public Vector3 FindFloor()
	{
		// width of raycasts around the centre of your character
		float raycastWidth = 0.25f;
		// check floor on 5 raycasts   , get the average when not Vector3.zero  
		int floorAverage = 1;

		CombinedRaycast = FloorRaycasts(0, 0, 1.6f);
		floorAverage += (getFloorAverage(raycastWidth, 0) + getFloorAverage(-raycastWidth, 0) + getFloorAverage(0, raycastWidth) + getFloorAverage(0, -raycastWidth));

		return CombinedRaycast / floorAverage;
	}
	public Vector3 FloorRaycasts(float offsetx, float offsetz, float raycastLength)
	{
		RaycastHit hit;
		// move raycast
		raycastFloorPos = transform.TransformPoint(0 + offsetx, 0 + 0.5f, 0 + offsetz);

		Debug.DrawRay(raycastFloorPos, Vector3.down, Color.magenta);
		if (Physics.Raycast(raycastFloorPos, -Vector3.up, out hit, raycastLength))
		{
			return hit.point;
		}
		else return Vector3.zero;
	}
	int getFloorAverage(float offsetx, float offsetz)
	{

		if (FloorRaycasts(offsetx, offsetz, 1.6f) != Vector3.zero)
		{
			CombinedRaycast += FloorRaycasts(offsetx, offsetz, 1.6f);
			return 1;
		}
		else { return 0; }
	}
}
