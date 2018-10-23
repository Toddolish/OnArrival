using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
	[Header("Health")]
	public float curHealth;
	public float maxHealth = 100;
	public Image healthBar;
	// Get the Helath bar Object
	public GameObject healthBar_Object;
	// Get the scale x,y and z and set them all the the correct size
	float heathBarScaleY = 0.3519601f;

	[Header("Energy")]
	public float curEnergy;
	public float maxEnergy = 100f;
	public Image energyBar;
	public float energyRegenRate = 1f;

	private float energyRegenReadyTimer;

	void Start()
	{
		curHealth = maxHealth;
		curEnergy = maxEnergy;
		healthBar = GameObject.Find("HealthBar").GetComponent<Image>();
		energyBar = GameObject.Find("EnergyBar").GetComponent<Image>();
	}

	void Update()
	{
		#region Health
		if (curHealth <= 0)
		{
			GameOver();
			curHealth = 0;
		}
		else if (curHealth > maxHealth)
		{
			curHealth = maxHealth;
		}
		healthBar.fillAmount = curHealth / 100;
		healthBar_Object.transform.localScale = new Vector3(0.125382f, heathBarScaleY, 0.125382f);
		heathBarScaleY = curHealth / 283;
		#endregion

		#region Energy
		if (curEnergy <= 0)
		{
			curEnergy = 0;
		}
		else if (curEnergy > maxEnergy)
		{
			curEnergy = maxEnergy;
		}
		energyBar.fillAmount = curEnergy / 100;
		
		#endregion
	}
	public void GameOver()
	{
		SceneManager.LoadScene(1);
	}
}
