﻿using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
	#region Variables
	[Header("Weapon")]
	public float damage = 10f;
	public float range = 100f;
	public float impactForce = 100f;
	public float fireRate = 15f;
	private float nextTimeToFire = 0.2f;
	public Camera fpsCam;
	public ParticleSystem muzzelFlash;
	public GameObject impactEffect;
	public Sprite impactHole;
	public LayerMask layersToHit;
	public float rotateSpeed = 10f;
	Animator animator;
	Enemy enemyScript;

	[Header("Jav Ammo Rounds")]
	public float currentJavAmmo;
	public float maxJavAmmo;
	public bool javAmmoActive = true;
	public Transform javAmmoBar;
	float javZ = 1f;
	float javX = 1f;
	float javAmmoY = 1f;
	public Transform javSpawnPoint;
	public GameObject Javalin;
	float javSpeed = 50;
	[SerializeField] int javAmmoCartridge;
	public bool droppedCanister;
	Transform canisterHoldingPoint;
	public GameObject ammoCanister;

	[Header("UI ELEMENTS")]
	[Header("Text")]
	public Text javTextCartridgeCounter;
	public Text normalTextCartridgeCounter;
	public Text orbTextCartridgeCounter;
	#endregion

	void Start()
	{
		currentJavAmmo = maxJavAmmo;
		javAmmoBar = GameObject.Find("LiquidParent").GetComponent<Transform>();
		canisterHoldingPoint = GameObject.Find("CanisterHolderPoint").GetComponent<Transform>();
		if (enemyScript != null)
		{
			enemyScript = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();
		}
		animator = GetComponent<Animator>();
	}

	void Update()
	{
		javTextCartridgeCounter.text = javAmmoCartridge.ToString();
		Reload();
		javAmmoBar.transform.localScale = new Vector3(javX, javAmmoY, javZ);
		javZ = currentJavAmmo / 30;
		// Fire the Extraction Rifle
		if (Time.timeScale == 1)
		{
			if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire) // SemiAutoFire
			{
				if (enemyScript != null)
				{
					enemyScript.SeekRadius += 40f; // Enemys can hear loud noises
				}
				nextTimeToFire = Time.time + 1f / fireRate; // The higher the fire rate the less time between shots

				if (currentJavAmmo > 0)
				{
					shootJav();
				}
			}
		}
		// Drop the Canister when ammo is less then one
		if (!droppedCanister)
		{
			if (currentJavAmmo < 1)
			{
				// Find canisterHolder
				Transform canisterAmmo = GameObject.Find("CanisterHolder").GetComponent<Transform>();
				// Find canisterHolder Rigidbody and set kinematic to false
				canisterAmmo.GetComponent<Rigidbody>().isKinematic = false;
				// Find Liquid and change the name to "Empty"
				canisterAmmo.name = "Used Canister";
				canisterAmmo.transform.GetChild(1).GetComponent<Transform>().name = "Empty";
				// Unparent thee ammo Canister
				canisterAmmo.transform.parent = null;
				// Cannister is dropped so it = true;
				droppedCanister = true;
			}
		}
	}
	void shootJav()
	{
		muzzelFlash.Play();
		currentJavAmmo--;
		RaycastHit hit;
		if (Physics.Raycast(javSpawnPoint.transform.position, fpsCam.transform.forward, out hit, range, layersToHit))
		{
			Debug.Log(hit.transform.name);
			Target target = hit.transform.GetComponent<Target>();
			Enemy enemy = hit.transform.GetComponent<Enemy>();
			if (target != null)
			{
				target.TakeDamage(damage);
			}
			if (enemy != null)
			{
				enemy.TakeDamage(damage);
			}
			GameObject javalinPrefab = Instantiate(Javalin, javSpawnPoint.position, javSpawnPoint.rotation);
		}
	}
	public void AddSpikeAmmoCapsule()
	{
		javAmmoCartridge++;
	}
	void Reload()
	{
		// When "R" is pressed to reload and ammo is greater the zero and no more ammo in the clip
		if (Input.GetKeyDown(KeyCode.R) && javAmmoCartridge > 0 && currentJavAmmo <= 0)
		{
			// Dropped Canister bool = false as we are not dropping this canister until we have 0 ammo
			droppedCanister = false;
			// Instanciate new canister onto the canisterHolderPoint
			GameObject canister = Instantiate(ammoCanister, canisterHoldingPoint.transform.position, canisterHoldingPoint.rotation);
			// Parent canister to canisterHoldingPoint
			canister.transform.parent = canisterHoldingPoint.transform;
			// Find the settings for the new Canister
			javAmmoBar = GameObject.Find("LiquidParent").GetComponent<Transform>();
			canister.name = "CanisterHolder";
			// Reload the ammo and spikeCartridge-- 
			javAmmoCartridge--;
			// set currentJavAmmo to the maxJavAmmo
			currentJavAmmo = maxJavAmmo;
			// Check ammo cartridge so it does not go below zero andif so will always be set back to zero
			if (javAmmoCartridge <= 0)
			{
				javAmmoCartridge = 0;
			}
		}
	}
}