using UnityEngine;
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
	public Animator PlayerAnim;
	Enemy enemyScript;
	PlayerMovment playerMove;

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
    public GameObject JavalinFeedback;
	float javSpeed = 50;
	public int javAmmoCartridge;
	public bool droppedCanister;
	Transform canisterHoldingPoint;
	public GameObject ammoCanister;
	public bool reloading;

	[Header("UI ELEMENTS")]
	[Header("Text")]
	public Text javTextCartridgeCounter;

	// Hologram Image bar on the weapon Canvas to display to cur ammo;
	public Image ammoBarImage;
	#endregion

	void Start()
	{
		currentJavAmmo = 0;
		javAmmoBar = GameObject.Find("LiquidParent").GetComponent<Transform>();
		canisterHoldingPoint = GameObject.Find("CanisterHolderPoint").GetComponent<Transform>();
		playerMove = GameObject.Find("Player").GetComponent<PlayerMovment>();
		ammoBarImage = GameObject.Find("AmmoBar").GetComponent<Image>();
		
		if (enemyScript != null)
		{
			enemyScript = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();
		}
		animator = GetComponent<Animator>();
	}

	void Update()
	{
		ammoBarImage.fillAmount = currentJavAmmo / 30;
		javTextCartridgeCounter.text = javAmmoCartridge.ToString();
		Reload();
		javAmmoBar.transform.localScale = new Vector3(javX, javAmmoY, javZ);
		javZ = currentJavAmmo / 30;
		// Fire the Extraction Rifle
		if (Time.timeScale == 1 && !playerMove.sprint)
		{
			if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire) // SemiAutoFire
			{
				nextTimeToFire = Time.time + 1f / fireRate; // The higher the fire rate the less time between shots

				if (currentJavAmmo > 0)
				{
					FindObjectOfType<AudioManager>().Play("Shoot");
					shootJav();
				}
			}
		}
		if (javAmmoCartridge <= 0)
		{
			javAmmoCartridge = 0;
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
				//canisterAmmo.GetComponent<Rigidbody>().AddForce
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
		if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range, layersToHit))
		{
			Debug.Log(hit.transform.gameObject.name);
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
		}
		// Instanciate outside of ray as it has been causeing spawn issues
		Instantiate(Javalin, hit.point, fpsCam.transform.rotation);
        GameObject feedback = Instantiate(JavalinFeedback, javSpawnPoint.position, fpsCam.transform.rotation);
        feedback.transform.parent = this.transform.parent;
        Instantiate(impactEffect, hit.point, Quaternion.LookRotation(-hit.point));
	}
	public void AddSpikeAmmoCapsule()
	{
		javAmmoCartridge += 2;
	}
	void Reload()
	{
		// When "R" is pressed to reload and ammo is greater the zero and no more ammo in the clip
		if (Input.GetKeyDown(KeyCode.R) && javAmmoCartridge > 0 && currentJavAmmo <= 0 && !reloading)
		{
			reloading = true;
			// Play the reload animation
			//////////////////////////////////////////////////// change will be need for animation to spawn
			PlayerAnim.SetTrigger("Reload");
			
			// Check ammo cartridge so it does not go below zero and if so will always be set back to zero --- canister counter
			if (javAmmoCartridge <= 0)
			{
				javAmmoCartridge = 0;
			}
		}
	}
	public void SpawnCanister()
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
		// Check ammo cartridge so it does not go below zero and if so will always be set back to zero --- canister counter
		if (javAmmoCartridge <= 0)
		{
			javAmmoCartridge = 0;
		}
		reloading = false;
	}
}