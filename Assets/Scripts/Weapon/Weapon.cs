using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
	[Header("Weapon")]
	public float damage = 10f;
	public float range = 100f;
	public float impactForce = 100f;
	public float fireRate = 15f;
	private float nextTimeToFire = 0f;
	public Camera fpsCam;
	public ParticleSystem muzzelFlash;
	public GameObject impactEffect;
	public Sprite impactHole;
	public LayerMask layersToHit;
	public float rotateSpeed = 10f;
	public int ammoIndex = 1;
	Animator animator;
	Enemy enemyScript;

	[Header("Normal Ammo Rounds")]
	public float currentNormalAmmo;
	public float maxNormalAmmo;
	public bool normalAmmoActive = true;
	public Transform ammoBar;
	float z = 0.1410879f;
	float x = 0.9399332f;
	float normalAmmoY = 1.498391f;
	public Transform Target1;


	[Header("Jav Ammo Rounds")]
	public float currentJavAmmo;
	public float maxJavAmmo;
	public bool javAmmoActive = true;
	public Transform javAmmoBar;
	float javZ = 0.1199799f;
	float javX = 0.9955617f;
	float javAmmoY = 1.151171f;
	public Transform Target2;
	public Transform javSpawnPoint;
	public GameObject Javalin;
	float javSpeed = 50;
	[SerializeField] int javAmmoCartridge;

	[Header("Orb Ammo Rounds")]
	public float currentOrbAmmo;
	public float maxOrbAmmo;
	public bool orbAmmoActive = true;
	public Transform orbAmmoBar;
	float orbZ = 0.06733779f;
	float orbX = 0.4455608f;
	float orbAmmoY = 1.106934f;
	public Transform Target3;
	[SerializeField] int orbAmmoCartridge;

	[Header("UI ELEMENTS")]
	[Header("Text")]
	public Text javTextCartridgeCounter;
	public Text normalTextCartridgeCounter;
	public Text orbTextCartridgeCounter;

	void Start()
	{
		currentNormalAmmo = maxNormalAmmo;
		currentJavAmmo = maxJavAmmo;
		currentOrbAmmo = maxOrbAmmo;
		ammoBar = GameObject.Find("NormalAmmo").GetComponent<Transform>();
		javAmmoBar = GameObject.Find("JavalinAmmo").GetComponent<Transform>();
		orbAmmoBar = GameObject.Find("OrbAmmo").GetComponent<Transform>();
		if (enemyScript != null)
		{
			enemyScript = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();
		}
		animator = GetComponent<Animator>();
	}

	void Update()
	{
		orbTextCartridgeCounter.text = orbAmmoCartridge.ToString();
		javTextCartridgeCounter.text = javAmmoCartridge.ToString();
		ammoBar.transform.position = Target1.transform.position;
		javAmmoBar.transform.position = Target2.transform.position;
		orbAmmoBar.transform.position = Target3.transform.position;
		Reload();
		switch (ammoIndex)
		{
			case 1:
				javAmmoActive = false;
				normalAmmoActive = true;
				orbAmmoActive = false;
				break;

			case 2:
				javAmmoActive = true;
				normalAmmoActive = false;
				orbAmmoActive = false;
				break;

			case 3:
				javAmmoActive = false;
				normalAmmoActive = false;
				orbAmmoActive = true;
		break;
		}
		// Normal
		ammoBar.transform.localScale = new Vector3(x, normalAmmoY, z);
		normalAmmoY = currentNormalAmmo / 20f;
		// Jav
		javAmmoBar.transform.localScale = new Vector3(javX, javAmmoY, javZ);
		javAmmoY = currentJavAmmo / 30;
		// Orb
		orbAmmoBar.transform.localScale = new Vector3(orbX, orbAmmoY, orbZ);
		orbAmmoY = currentOrbAmmo / 8;
		if (Time.timeScale == 1)
		{
			if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire && ammoIndex == 1) // AutoFire
			{
				nextTimeToFire = Time.time + 1f / fireRate; // The higher the fire rate the less time between shots
				if (currentNormalAmmo > 0)
				{
					shootNormal();
				}
			}
			if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire && ammoIndex == 2) // SemiAutoFire
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
			if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire && ammoIndex == 3) // AutoFire
			{
				nextTimeToFire = Time.time + 1f / fireRate; // The higher the fire rate the less time between shots
				if (currentOrbAmmo > 0)
				{
					shootOrb();
				}
			}
		}

		//Ammo Switching
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			animator.SetInteger("ammoIndex", 1);
			ammoIndex = 1;
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			animator.SetInteger("ammoIndex", 2);
			ammoIndex = 2;
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			animator.SetInteger("ammoIndex", 3);
			ammoIndex = 3;
		}
	}

	void shootNormal()
	{
		muzzelFlash.Play();
		RaycastHit hit;
		currentNormalAmmo--;
		if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range , layersToHit))
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

			if (hit.rigidbody != null)
			{
				hit.rigidbody.AddForce(-hit.normal * impactForce);
			}
			GameObject impactParticle = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
			Sprite impactSprite = Instantiate(impactHole, hit.point, Quaternion.LookRotation(hit.normal));
			Destroy(impactParticle, 2f);
			Destroy(impactSprite, 10f);
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

			if (hit.rigidbody != null)
			{
				hit.rigidbody.AddForce(-hit.normal * impactForce);
			}
			GameObject javalinPrefab = Instantiate(Javalin, javSpawnPoint.position, javSpawnPoint.rotation);
			javalinPrefab.transform.Rotate(Vector3.right * 90);
		}
	}
	void shootOrb()
	{
		muzzelFlash.Play();
		RaycastHit hit;
		currentOrbAmmo--;
		if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range, layersToHit))
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

			if (hit.rigidbody != null)
			{
				hit.rigidbody.AddForce(-hit.normal * impactForce);
			}
			GameObject impactParticle = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
			Sprite impactSprite = Instantiate(impactHole, hit.point, Quaternion.LookRotation(hit.normal));
			Destroy(impactParticle, 2f);
			Destroy(impactSprite, 10f);
		}
	}
	public void AddSpikeAmmoCapsule()
	{
		javAmmoCartridge++;
	}
	public void AddOrbAmmoCapsule()
	{
		orbAmmoCartridge++;
	}
	void Reload()
	{
		if (ammoIndex == 2 && Input.GetKeyDown(KeyCode.R) && javAmmoCartridge > 0)
		{
			// Reload the ammo and spikeCartridge-- 
			javAmmoCartridge--;
			// set currentJavAmmo to the maxJavAmmo
			currentJavAmmo = maxJavAmmo;

			if (javAmmoCartridge <= 0)
			{
				javAmmoCartridge = 0;
			}
		}

		if (ammoIndex == 3 && Input.GetKeyDown(KeyCode.R) && orbAmmoCartridge > 0)
		{
			// Reload the ammo and orbCartridge-- 
			orbAmmoCartridge--;
			// set currentOrbAmmo to the maxOrbAmmo
			currentOrbAmmo = maxOrbAmmo;

			if (orbAmmoCartridge <= 0)
			{
				orbAmmoCartridge = 0;
			}
		}
	}
}
