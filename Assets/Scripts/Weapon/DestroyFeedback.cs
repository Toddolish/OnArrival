using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyFeedback : MonoBehaviour {

	public float speed;
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        rb.AddRelativeForce(Vector3.forward * speed * Time.deltaTime, ForceMode.Impulse);
        Destroy(this.gameObject, 0.02f);
    }
}
