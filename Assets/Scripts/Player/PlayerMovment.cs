using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovment : MonoBehaviour
{
	#region Base Variables
	[Header(" Base Variables")]
	private CharacterController controller;
	public LayerMask jumpableLayers;
	public float startMoveSpeed;
	public float moveSpeed; // The normal movement speed;
	public float crouchMoveSpeed; // When Player is Crouched
	public float sprintSpeed; // The max speed player can sprint
	public bool crouch;
	public bool sprint;
	public float staminaDecreaseRate = 1f;
	private Vector3 moveDirection;
	Animator anim; // Used for crouch only
	public Animator animMain;
	public Text collectText;
	public AimDownSight aimScript;
	Weapon weapon;
	PlayerStats playerStats;
	public LayerMask spikePlant;
	//sprint 
	float cannotSprintTimer;
	#endregion
	#region Jump Variables
	[Header("Jump")]
	bool isJumping;
	[SerializeField] private AnimationCurve jumpFallOff;
	[SerializeField] private float jumpMultiplier;
	bool down;
	#endregion
	#region Ammo Collection 
	// Collect ammo 
	public bool pickup = false;
	float pickupTimer;
	#endregion
	#region Pulse Skill
	[Header("Pulse Skill")]
	// pulseReady will be a special skill and when its true u can use it then it will go to false
	public bool pulseReady;
	// when timer is over pulseSkill will be ready to use again
	float pulseTimer;
	float pulseCooldownTime = 0.5f;
	public float range = 100f;
	public float radius;
	Ray shootRay;
	RaycastHit shootHit;
	int shootableMask;
	Enemy enemyScript;
    #endregion
	//Beacon
	public Text beaconText;
	void Start()
	{
		pulseReady = true;
		weapon = GameObject.Find("Extraction_Rifle").GetComponent<Weapon>();
		aimScript = GameObject.Find("Player_Animations_with_rifle").GetComponent<AimDownSight>();
		controller = GetComponent<CharacterController>();
		anim = GetComponent<Animator>();
		playerStats = GetComponent<PlayerStats>();
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Confined;
	}
	private void Update()
	{
		Extract();
		Jump();
		// When player stop sprinting
		// Stop walk animation
		if (aimScript.aiming)
		{
			animMain.SetBool("Sprinting", false);
			animMain.SetFloat("Walking", 0);
			moveSpeed = startMoveSpeed;
			sprint = false;
		}
		else
		{
			// I dont know!
		}
		#region Methods
		Crouch();
		PulseCharge();
		Sprinting();
		#endregion
		if (pickup)
		{
			collectText.enabled = false;
			pickupTimer += Time.deltaTime;
			if (pickupTimer > 0.5)
			{
				pickup = false;
				pickupTimer = 0;
			}
		}
	}
	private void LateUpdate()
	{
		Movement();
	}
	public void Jump()
	{
		if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
		{
			// When player jumps disable sprint and walk animations
			animMain.SetBool("Jumping", true);
			isJumping = true;
			StartCoroutine(JumpEvent());
		}
		if (controller.isGrounded)
		{
			animMain.SetBool("Jumping", false);
		}
	}
	public void Movement()
	{
		float horizInput = Input.GetAxis("Horizontal") * moveSpeed;
		float vertInput = Input.GetAxis("Vertical") * moveSpeed;

		Vector3 forwardMovement = transform.forward * vertInput;
		Vector3 rightMovement = transform.right * horizInput;

		controller.SimpleMove(forwardMovement + rightMovement);

		animMain.SetFloat("Walking", vertInput);
	}
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireSphere(transform.position, radius);
	}
	void Extract()
	{
		RaycastHit[] hits;
		hits = Physics.SphereCastAll(transform.position, radius, transform.forward, range, spikePlant);

		if (hits.Length > 0)
		{
			for (int i = 0; i < hits.Length; i++)
			{
				shootHit = hits[i];
				SpikePlant spikePlant;
				spikePlant = shootHit.transform.GetComponent<SpikePlant>();
				if (spikePlant.curSap >= 2.4)
				{
					collectText.enabled = true;
					if (Input.GetKeyDown(KeyCode.E) && spikePlant.timeForHarvest)
					{
						if (!pickup)
						{
							animMain.SetTrigger("Extract");
						}
					}
				}

				else
				{
					collectText.enabled = false;
				}
			}
		}
		if (hits.Length <= 0)
		{
			collectText.enabled = false; 
		}
	}
	private void OnTriggerStay(Collider other)
	{
		if (other.gameObject.tag == "Beacon")
		{
			beaconText.enabled = true;
			if (Input.GetKeyDown(KeyCode.E))
			{
				Destroy(other.gameObject);
			}
		}
		else
		{
			beaconText.enabled = false;
		}
	}
	public void Crouch()
	{
		if (Input.GetKeyDown(KeyCode.C) && !crouch)
		{
			// crouch is animation and bool is true
			anim.SetBool("crouch", true);
			crouch = true;
			// Sprinting bool and animation is false
			anim.SetBool("sprinting", false);
			sprint = false;
			// movement speed is now crouch speed
			moveSpeed = crouchMoveSpeed;
		}
		else if (Input.GetKeyDown(KeyCode.C) && crouch)
		{
			anim.SetBool("crouch", false);
			crouch = false;
			moveSpeed = startMoveSpeed;
		}
	}
	public void PulseCharge()
	{
		// If pulseReady is true it can be used with middle mouse button
		if (pulseReady && Input.GetKeyDown(KeyCode.Mouse2))
		{
            animMain.SetTrigger("Palm");
            if (weapon.javAmmoCartridge > 0)
            {
                weapon.javAmmoCartridge --;
            }
        }
		if (!pulseReady)
		{
			pulseTimer += Time.deltaTime;
			if (pulseTimer >= pulseCooldownTime)
			{
				pulseReady = true;
				pulseTimer = 0;
			}
		}
	}
	public void Sprinting()
	{
		if (!aimScript.aiming)
		{
			if (playerStats.cannotSprint == false)
			{
				if (Input.GetKey(KeyCode.LeftShift) && playerStats.curEnergy > 0 && !crouch) // Start sprinting
				{
					animMain.SetBool("Sprinting", true);
					playerStats.curEnergy -= staminaDecreaseRate * Time.deltaTime;
					moveSpeed = sprintSpeed;
					sprint = true;
				}
				else
				{
					animMain.SetBool("Sprinting", false);
					playerStats.curEnergy += playerStats.energyRegenRate * Time.deltaTime;
					moveSpeed = startMoveSpeed;
					sprint = false;
				}
			}
		}
		if (playerStats.cannotSprint == true)
		{
			animMain.SetBool("Sprinting", false);
			playerStats.curEnergy += playerStats.energyRegenRate * Time.deltaTime;
			moveSpeed = startMoveSpeed;
			sprint = false;
			cannotSprintTimer += Time.deltaTime;
			if (cannotSprintTimer > 5)
			{
				playerStats.cannotSprint = false;
				cannotSprintTimer = 0;
			}
		}
	}
	public void ExtractPlant()
	{
		SpikePlant spikePlant;
		if (spikePlant = shootHit.transform.GetComponent<SpikePlant>())
		{
			spikePlant.curSap = 0;
			weapon.AddSpikeAmmoCapsule();
			pickup = true;
		}
	}
	public void PulseDischarge()
	{
		Debug.Log("use pulse skill");
		RaycastHit[] hits;
		hits = Physics.SphereCastAll(transform.position, radius, transform.forward, range);

		if (hits.Length > 0)
		{
			for (int i = 0; i < hits.Length; i++)
			{
				Debug.Log("Searching for enemy script");
				Debug.Log(hits[i].collider.name);
				shootHit = hits[i];
				if (enemyScript = shootHit.collider.gameObject.GetComponent<Enemy>())
				{
					if (weapon.javAmmoCartridge > 0)
					{
						enemyScript.Knockback();
						enemyScript.health -= 50;
					}
					else
					{
						enemyScript.Knockback();
						enemyScript.health -= 25;
					}
				}
			}
		}
		// Shoot projectiles like a shotgun and knockback enemies
		// Once used timer will start a countdown and pulseReady will be false as it is no longer ready
		pulseReady = false;
	}
	private IEnumerator JumpEvent()
	{
		//controller.slopeLimit = 90f;
		float timeInAir = 0.0f;

		do
		{
			float jumpForce = jumpFallOff.Evaluate(timeInAir);
			controller.Move(Vector3.up * jumpForce * jumpMultiplier * Time.deltaTime);
			timeInAir += Time.deltaTime;
			yield return null;
		}
		while (!controller.isGrounded && controller.collisionFlags != CollisionFlags.Above);

		controller.slopeLimit = 45f;
		isJumping = false;
	}
}
