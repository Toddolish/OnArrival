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
	CapsuleCollider collider;
	public float burstForce = 2f;
	bool isDestroyed;
	Wander wanderScript;
	AIAgent aiAgent;
	Weapon weaponScript;
	PlayerStats playerStats;
	public GameObject explosion;

	[Header("Player Detection")]
	public float SeekRadius = 10f;
	public float knockbackForce = 5f;
	public float decreaseSpeed = 1f;

	[Header("Agro")]
	public bool agro;
	public float agroRange = 200f;

	[Header("Attack")]
	public bool attacked = false;
	public float attackTimer;
	float timer;
	float attackRangeTimer = 0;
	float attackAfterTime = 1;
	bool attackInRange;
	public float AttackRange = 2;

	// Enemy Knockback 
	// Take minor melee damage
	float resetTime;
	bool knockedBack;

	void SetKinematic(bool newValue)
	{
		Rigidbody[] bodies = GetComponentsInChildren<Rigidbody>();

		foreach (Rigidbody rb in bodies)
		{
			rb.isKinematic = newValue;
		}
	}

	private void Start()
	{
		SetKinematic(true);
		collider = this.GetComponent<CapsuleCollider>();
		animator = GetComponent<Animator>();
		aiAgent = this.GetComponent<AIAgent>();
		rb = GetComponent<Rigidbody>();
		agent = this.GetComponent<NavMeshAgent>();
		wanderScript = GetComponent<Wander>();
		playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
		target = GameObject.Find("Player").GetComponent<Transform>();
		collider.enabled = false;
	}
	public void Update()
	{
		#region Enemy seek and Attack
		if (agro)
		{
			SeekRadius = agroRange;
		}
		ResetAttack();
		animator.SetFloat("Move", agent.speed);
		float distance = Vector3.Distance(agent.transform.position, target.position);
		float disToTarget = Vector3.Distance(transform.position, target.position);
		#endregion
		#region Enemy Health
		if (health > 0)
		{
			if (SeekRadius > disToTarget)
			{
				collider.enabled = true;
				animator.SetBool("Run", true);
				agent.speed = chaseSpeed;
				wanderScript.enabled = false;
				aiAgent.enabled = false;
				agent.SetDestination(target.position); // not working because destination already being set in another script
				transform.LookAt(target.transform.position);
			}
			else if (SeekRadius < disToTarget)
			{
				animator.SetBool("Run", false);
				//wanderScript.enabled = true;
				//aiAgent.enabled = true;
				agent.speed = normalSpeed;
			}
		}
		if (health <= 0)
		{
			this.gameObject.tag = "noHit";
			this.gameObject.layer = LayerMask.NameToLayer("Ignore");
			Destroy();
		}
		#endregion
		#region Knockback
		if (knockedBack)
		{
			resetTime += Time.deltaTime;
			rb.isKinematic = false;
			rb.AddForce(-transform.forward * burstForce * 1.5f, ForceMode.Impulse);
			rb.AddForce(transform.up * burstForce, ForceMode.Impulse);
		}

		if (resetTime > 0.1)
		{
			knockedBack = false;
			rb.isKinematic = true;
			resetTime = 0;
		}
		#endregion
	}
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, SeekRadius);
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(transform.position, AttackRange);
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
		if (collision.gameObject.tag == "noHit")
		{
			Physics.IgnoreCollision(this.collider, collider);
		}
		if (collision.gameObject.tag == "spike")
		{
			health -= 10;
			if (agro != true)
			{
				agro = true;
			}
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
		//Destroy(this.gameObject);
		//Instantiate(explosion, transform.position, transform.rotation);
		//rb.constraints = RigidbodyConstraints.None;
		SetKinematic(false);
		animator.enabled = false;
		agent.enabled = false;
		aiAgent.enabled = false;
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
		// Check if player is in range
		if (AttackRange > disToTarget)
		{
			playerStats.curHealth -= 25;
			playerStats.SplashDamage();
		}
	}
	public void Knockback()
	{
		if (health <= 0)
		{
			SetKinematic(false); // Make sure to set kinematic false before any knockback occurs
			rb.AddForce(-transform.forward * burstForce * 2f, ForceMode.Impulse);
			rb.AddForce(transform.up * burstForce * 1f, ForceMode.Impulse);
		}
		if (health > 0)
		{
			knockedBack = true;
		}
	}
}
