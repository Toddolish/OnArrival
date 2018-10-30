using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

	[Header("Spawner")]
	public GameObject Enemy;
	public float countDown;
	int spawnIndex;

	void Start()
	{

	}

	void Update()
	{
		countDown += Time.deltaTime;

		if (countDown > 10)
		{
			spawnIndex ++;
			Instantiate(Enemy, transform.position, transform.rotation);
			countDown = 0;
		}
	}
}
