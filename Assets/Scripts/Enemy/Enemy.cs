using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using SteeringBehaviours;

public class Enemy : MonoBehaviour
{
	[Header("Base Variables")]
	public float health = 50f;
	Rigidbody rb;
	NavMeshAgent agent;
	public Transform target;
	public float normalSpeed = 3.5f;
	public float chaseSpeed = 5.0f;
	//bool enemyKilled;
	public float burstForce = 2f;
	bool isDestroyed;
	Wander wanderScript;
	AIAgent aiAgent;
	Weapon weaponScript;

	// Player detection
	public float SeekRadius = 10f;
	public float knockbackForce = 5f;
	public float decreaseSpeed = 1f;

	// Attack
	public bool attacked = false;
	public float attackTimer;
	float timer;
	private void Start()
	{
		aiAgent = this.GetComponent<AIAgent>();
		rb = GetComponent<Rigidbody>();
		agent = this.GetComponent<NavMeshAgent>();
		wanderScript = GetComponent<Wander>();
		target = GameObject.Find("Player").GetComponent<Transform>();
		weaponScript = GameObject.Find("Weapon").GetComponent<Weapon>();
	}
	public void Update()
	{
		if (SeekRadius < 20)
		{
			SeekRadius += Time.deltaTime;
		}
		if (SeekRadius > 20)
		{
			SeekRadius -= decreaseSpeed * Time.deltaTime;
		}
		if (SeekRadius > 40)
		{
			SeekRadius = 40;
		}
		float distance = Vector3.Distance(agent.transform.position, target.position);
		float disToTarget = Vector3.Distance(transform.position, target.position);
		if (health > 0)
		{
			if (SeekRadius > disToTarget)
			{
				agent.speed = chaseSpeed;
				//SeekRadius = 8;
				wanderScript.enabled = false;
				aiAgent.enabled = false;
				agent.SetDestination(target.position); // not working because destination already being set in another script
				transform.LookAt(target.transform.position);
			}
			else if (SeekRadius < disToTarget)
			{
				wanderScript.enabled = true;
				aiAgent.enabled = true;
				//SeekRadius = 5;
				agent.speed = normalSpeed;
			}
		}
	}
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, SeekRadius);
	}
	public void TakeDamage(float amount)
	{
		health -= amount;
		if (health <= 0f)
		{
			Destroy();
		}
	}
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "spike")
		{
			health -= 25;
			if (health <= 0f)
			{
				Destroy();
			}
		}
		if (!attacked)
		{
			if (collision.gameObject.tag == "Player")
			{
				attacked = true;
				Rigidbody player = collision.gameObject.GetComponent<Rigidbody>();
				PlayerStats playeHealth = collision.gameObject.GetComponent<PlayerStats>();
				player.AddRelativeForce(-Vector3.forward * knockbackForce, ForceMode.Impulse);
				playeHealth.curHealth -= 25;
			}
		}
	}
	void Destroy()
	{
		//rb.AddForce(-transform.forward * burstForce, ForceMode.Impulse);
		//enemyKilled = true;
		agent.enabled = false;
		rb.constraints = RigidbodyConstraints.None;
		//Destroy(this.gameObject);
	}
	void stickToProjectile()
	{

	}
}
