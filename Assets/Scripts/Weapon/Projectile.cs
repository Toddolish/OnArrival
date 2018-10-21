﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
	public float speed;
	Rigidbody rb;
	CapsuleCollider CapCollider;
	bool flying = true;
	float depth = 0.30F;
	Camera cam;
	private Transform anchor;
	void Start()
	{
		rb = GetComponent<Rigidbody>();
		CapCollider = GetComponent<CapsuleCollider>();
		cam = GameObject.Find("Main Camera").GetComponent<Camera>();
	}

	void FixedUpdate()
	{
		if (flying)
		{
			rb.AddRelativeForce(-Vector3.forward * speed * Time.deltaTime, ForceMode.Impulse);
		}
		if (this.anchor != null)
		{
			this.transform.position = anchor.transform.position;
			this.transform.rotation = anchor.transform.rotation;
		}
	}
	private void OnCollisionEnter(Collision collision)
	{
		transform.position = collision.contacts[0].point;
		Enemy enemyScript = collision.gameObject.GetComponent<Enemy>();
		Rigidbody enemyRigid = collision.gameObject.GetComponent<Rigidbody>();
		Transform enemyTransform = collision.gameObject.GetComponent<Transform>();
		CapsuleCollider enemyCollider = collision.gameObject.GetComponent<CapsuleCollider>();

		// Stick into objects
		if (collision.gameObject.tag == "Trees" || collision.gameObject.tag == "spikePlant")
		{
			GameObject anchor = new GameObject("Javalin_Anchor");
			anchor.transform.position = this.transform.position;
			anchor.transform.rotation = this.transform.rotation;
			anchor.transform.parent = collision.transform;
			this.anchor = anchor.transform;
			rb.isKinematic = true;
			this.transform.GetChild(0).GetComponent<CapsuleCollider>().isTrigger = true;
			rb.constraints = RigidbodyConstraints.FreezeAll;
			flying = false;
			speed = 0;
		}
		// Stick into enemy if health is greater then 0
		if (collision.gameObject.tag == "Enemy")
		{
			//if (enemyScript.health > 0)
		    //{
				enemyRigid.AddForce(-transform.forward * enemyScript.burstForce, ForceMode.Impulse);
				GameObject anchor = new GameObject("Javalin_Anchor");
				anchor.transform.position = this.transform.position;
				anchor.transform.rotation = this.transform.rotation;
				anchor.transform.parent = collision.transform;
				this.anchor = anchor.transform;
				rb.isKinematic = true;
				this.transform.GetChild(0).GetComponent<CapsuleCollider>().isTrigger = true;
				rb.constraints = RigidbodyConstraints.FreezeAll;
				flying = false;
				speed = 0;
			//}
		}/*
		// Parent the enemy to the projectile 
		if(collision.gameObject.tag == "Enemy")
		{
			if (enemyScript.health <= 0)
			{
				enemyTransform.SetParent(this.transform);
				//enemyTransform.position = this.transform.position;
				enemyRigid.isKinematic = true; // This would be rag doll
				if (collision.gameObject.tag == "Trees" || collision.gameObject.tag == "spikePlant")
				{
					//enemyCollider.isTrigger = true;
					GameObject anchor = new GameObject("Javalin_Anchor");
					anchor.transform.position = this.transform.position;
					anchor.transform.rotation = this.transform.rotation;
					anchor.transform.parent = collision.transform;
					this.anchor = anchor.transform;
					rb.isKinematic = true;
					this.transform.GetChild(0).GetComponent<CapsuleCollider>().isTrigger = true;
					rb.constraints = RigidbodyConstraints.FreezeAll;
					flying = false;
					speed = 0;
				}
			}
		}*/
	}
}