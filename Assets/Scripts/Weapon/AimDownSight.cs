using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimDownSight : MonoBehaviour
{
	public Vector3 aimDownSight; // x 0, y -0.0836, z 0.761
	public Vector3 hipFire;
	float aimSpeed = 10;
	public bool aiming;
	
	void Update()
	{
		if (Input.GetMouseButton(1)) // Set weapon to Aim down sight
		{
			transform.localPosition = Vector3.Slerp(transform.localPosition, aimDownSight, aimSpeed * Time.deltaTime);
			aiming = true;
		}
		else 
		{
			transform.localPosition = Vector3.Slerp(transform.localPosition, hipFire, aimSpeed * Time.deltaTime); 
			aiming = false;
		}
	}
}
