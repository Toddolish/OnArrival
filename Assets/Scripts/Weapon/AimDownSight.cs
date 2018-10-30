using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimDownSight : MonoBehaviour
{
	public Vector3 aimDownSight; // x 0, y -0.0836, z 0.761
	public Vector3 hipFire;
	public Weapon weapon;
	public PlayerMovment playerMove;
	float aimSpeed = 10;
	public Camera cam;
	// Bool to control spring and walk animation
	public bool aiming;

	private void Start()
	{
		cam = GameObject.Find("Main Camera").GetComponent<Camera>();
		playerMove = GameObject.Find("Player").GetComponent<PlayerMovment>();
	}
	void Update()
	{
		if (Input.GetMouseButton(1) && !weapon.reloading) // Set weapon to Aim down sight
		{
			// Disable all movement animation
			// Cannot sprint
			transform.localPosition = Vector3.Slerp(transform.localPosition, aimDownSight, aimSpeed * Time.deltaTime);
			cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 40, 0.1f);
			aiming = true;
		}
		else
		{
			transform.localPosition = Vector3.Slerp(transform.localPosition, hipFire, aimSpeed * Time.deltaTime);
			cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 60, 0.1f);
			aiming = false;
		}
	}
	public void ActivateReload()
	{
		weapon.SpawnCanister();
	}
	public void ActivateExtraction()
	{
		playerMove.ExtractPlant();
	}
	public void ActivatePulseDischarge()
	{
		playerMove.PulseDischarge();
	}
}
