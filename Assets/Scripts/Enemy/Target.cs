using UnityEngine;

public class Target : MonoBehaviour
{
	public float health = 50f;
	Rigidbody rb;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
	}
	public void TakeDamage(float amount)
	{
		health -= amount;
		if (health <= 0f)
		{
			Destroy();
		}
	}
	void Destroy()
	{
		rb.constraints = RigidbodyConstraints.None;
		Destroy(gameObject);
	}
}
