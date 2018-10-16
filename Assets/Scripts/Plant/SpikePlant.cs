using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikePlant : MonoBehaviour
{
	public Transform targetVac;
	public bool beingPulled = false;
	public float pullSpeed = 20;
	private void Start()
	{
		targetVac = GameObject.Find("Player").GetComponent<Transform>();
	}
	void Update()
	{
		if (beingPulled)
		{
			Vector3.MoveTowards(transform.position, targetVac.transform.position, pullSpeed * Time.deltaTime);
		}
	}
	public void Pull()
	{
		this.transform.parent = null;
		beingPulled = true;
	}
}
