using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovment : MonoBehaviour
{
	[Header(" Base Variables")]
	public LayerMask jumpableLayers;
	public float startMoveSpeed;
	public float moveSpeed; // The normal movement speed;
	public float crouchMoveSpeed; // When Player is Crouched
	public float sprintSpeed; // The max speed player can sprint
	public bool crouch;
	public bool sprint;
	public float staminaDecreaseRate = 1f;
	private Vector3 moveDirection;
	Animator anim;
	Rigidbody rigid;
	public Text collectText;
	public AimDownSight aimScript;
	Weapon weapon;
	PlayerStats playerStats;

	[Header("Jump")]
	public float jumpForce = 10f;
	public float downForce = 20;
	public float distToGround;
	bool down;

	// Collect ammo 
	bool pickup = false;
	float pickupTimer;
	public float DashMoveSpeed = 100;

	void Start()
	{
		weapon = GameObject.Find("Extraction_Rifle").GetComponent<Weapon>();
		aimScript = GameObject.Find("Extraction_Rifle").GetComponent<AimDownSight>();
		rigid = GetComponent<Rigidbody>();
		anim = GetComponent<Animator>();
		playerStats = GetComponent<PlayerStats>();
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Confined;
	}
	private void OnDrawGizmos()
	{
		Vector3 direction = transform.TransformDirection(Vector3.down) * distToGround;
		Gizmos.DrawRay(transform.position, direction);
	}
	private void Update()
	{
		#region Methods
		//Aiming();
		Sprinting();
		Crouch();
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
	private void FixedUpdate()
	{
		rigid.AddForce(Vector3.down * downForce * Time.deltaTime, ForceMode.VelocityChange);
		Jump();
		Movement();
	}
	private void LateUpdate()
	{
		
	}
	public void Jump()
	{
		if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
		{
			Vector3 jumpVelocity = new Vector3(0, jumpForce, 0);
			rigid.velocity = rigid.velocity + jumpVelocity;
		}
	}
	public void Movement()
	{
		float inputH = Input.GetAxis("Horizontal") * Time.deltaTime;
		float inputV = Input.GetAxis("Vertical") * Time.deltaTime;
		rigid.AddRelativeForce(new Vector3(inputH * moveSpeed, 0, inputV * moveSpeed), ForceMode.Impulse);
		anim.SetFloat("moveSpeed", inputV);
	}
	private void OnTriggerStay(Collider collision)
	{
		if (collision.gameObject.tag == "spikePlant")
		{
			collectText.enabled = true;
			if (Input.GetKeyDown(KeyCode.E))
			{
				if (!pickup)
				{
					Target spikePlant;
					spikePlant = collision.transform.GetComponent<Target>();
					weapon.AddSpikeAmmoCapsule();
					Destroy(spikePlant.gameObject);
					pickup = true;
				}
			}
		}
	}
	private void OnTriggerExit(Collider collision)
	{
		if (collision.gameObject.tag == "spikePlant")
		{
			collectText.enabled = false;
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
	public void Sprinting()
	{
			if (Input.GetKey(KeyCode.LeftShift) && playerStats.curEnergy > 0 && !crouch) // Start sprinting
			{
				playerStats.curEnergy -= staminaDecreaseRate * Time.deltaTime;
				anim.SetBool("sprinting", true);
				moveSpeed = sprintSpeed;
				sprint = true;
			}
			else
			{
				playerStats.curEnergy += playerStats.energyRegenRate * Time.deltaTime;
				anim.SetBool("sprinting", false);
				moveSpeed = startMoveSpeed;
				sprint = false;
			}
	}
	public void Aiming()
	{
		if (aimScript.aiming)
		{
			//anim.SetBool("aiming", true);
		}
		else
		{
			//anim.SetBool("aiming", false);
		}
	}
	bool IsGrounded()
	{
	  return Physics.Raycast(transform.position, Vector3.down, distToGround, jumpableLayers);
	}
}
