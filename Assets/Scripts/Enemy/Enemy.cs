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
	public float chaseSpeed = 7.0f;
	Animator animator;
	MeshCollider collider;
	public float burstForce = 2f;
	bool isDestroyed;
	Wander wanderScript;
	AIAgent aiAgent;
	Weapon weaponScript;
	PlayerStats playerStats;
	public GameObject explosion;

	// Player detection
	public float SeekRadius = 10f;
	public float playerAttackRange = 2;
	public float knockbackForce = 5f;
	public float decreaseSpeed = 1f;

	// Attack
	public bool attacked = false;
	public float attackTimer;
	float timer;
	float attackRangeTimer = 0;
	float attackAfterTime = 1;
	bool attackInRange;

	//Enemy sink
	float sinkTimer;
	bool startSinkFase;

	private void Start()
	{
		collider = transform.GetChild(1).GetComponent<MeshCollider>();
		animator = GetComponent<Animator>();
		aiAgent = this.GetComponent<AIAgent>();
		rb = GetComponent<Rigidbody>();
		agent = this.GetComponent<NavMeshAgent>();
		wanderScript = GetComponent<Wander>();
		playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
		target = GameObject.Find("Player").GetComponent<Transform>();
	}
	public void Update()
	{
		sinkTime();
		ResetAttack();
		animator.SetFloat("Move", agent.speed);
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
				animator.SetBool("Run", true);
				agent.speed = chaseSpeed;
				//SeekRadius = 8;
				wanderScript.enabled = false;
				aiAgent.enabled = false;
				agent.SetDestination(target.position); // not working because destination already being set in another script
				transform.LookAt(target.transform.position);
			}
			else if (SeekRadius < disToTarget)
			{
				animator.SetBool("Run", false);
				wanderScript.enabled = true;
				aiAgent.enabled = true;
				//SeekRadius = 5;
				agent.speed = normalSpeed;
			}
		}
		if (health <= 0)
		{
				this.gameObject.tag = "noHit";
				Destroy();
		}
	}
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, SeekRadius);
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(transform.position, playerAttackRange);
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
		}
	}
	private void OnTriggerEnter(Collider other)
	{
		// Can attack when attack bool is false
		if (!attacked)
		{
			if (other.gameObject.tag == "Player")
			{
				attacked = true;
				animator.SetTrigger("Attack");
				attackInRange = true;
			}
		}
	}
	void Destroy()
	{
		//ssDestroy(this.gameObject);
		//Instantiate(explosion, transform.position, transform.rotation);
		startSinkFase = true;
		animator.enabled = false;
		agent.enabled = false;
		aiAgent.enabled = false;
		rb.constraints = RigidbodyConstraints.None; 
	}
	void ResetAttack()
	{
		if (attacked)
		{
			attackTimer += Time.deltaTime;
			if (attackTimer >= 1)
			{
				// Enemy can attack the player
				attacked = false;
				attackTimer = 0;
			}
		}
	}
	public void AttackInRange()
	{
		float distance = Vector3.Distance(agent.transform.position, target.position);
		float disToTarget = Vector3.Distance(transform.position, target.position);
		// check if player is in range
		if (playerAttackRange > disToTarget)
		{
			playerStats.curHealth -= 25;
		}
	}
	public void Knockback()
	{
		Debug.Log("Knocked Back");
		rb.AddForce(-transform.forward * burstForce * 1.5f, ForceMode.Impulse);
		rb.AddForce(transform.up * burstForce, ForceMode.Impulse);
	}
	void sinkTime()
	{
		if (startSinkFase)
		{
			sinkTimer += Time.deltaTime;
		}
		if (sinkTimer > 10)
		{
			collider.isTrigger = true;
			sinkTimer = 0;
		}
	}
}
