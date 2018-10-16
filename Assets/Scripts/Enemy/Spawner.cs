using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

	[Header("Spawner")]
	public GameObject Enemy;
	public float countDown;

	void Start()
	{

	}

	void Update()
	{
		countDown += Time.deltaTime;

		if (countDown > 10)
		{
			Instantiate(Enemy, transform.position, transform.rotation);
			countDown = 0;
		}
	}
}
