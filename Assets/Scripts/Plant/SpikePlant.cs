using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikePlant : MonoBehaviour
{
	// Create a float for the current ammount of sap
	public float curSap;
	// Create a float for the maximum ammount of sap
	float maxSap = 2.4f;
	// create an object reference for the child 0 object sap
	public Transform sap;
	float sapGrothRate = 0.01f;
	// bool for time for harvest
	public bool timeForHarvest;
    public bool collectHealth;

	private void Start()
	{
		// set current sap to max sap
		curSap = maxSap;
		// get the sap gameObjectComponent
		sap = transform.GetChild(0).GetComponent<Transform>();
	}

	private void Update()
	{
		// Get the sap local scale
		// Set the sap localScale to curSap
		sap.transform.localScale = new Vector3(curSap, curSap, curSap);
		// Increase sap scale with growth rate when curSap is less them maxSap
		if (curSap < maxSap)
		{
			curSap += sapGrothRate * Time.deltaTime;
		}
		// If curSap is greater then or equal to maxSap then set a limit of maxSap
		if (curSap >= maxSap)
		{
			curSap = maxSap;
		}
		// Make a limit for curSap so that it never falls below zero
		if (curSap <= 0)
		{
			curSap = 0;
		}
		Harvest();
	}
	public void Harvest()
	{
		if (curSap >= 2.4)
		{
			timeForHarvest = true;
			// can be extracted
		}
		else
		{
			timeForHarvest = false;
		}
	}
}
