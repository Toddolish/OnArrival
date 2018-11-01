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

    void LateUpdate()
    {
        rb.AddRelativeForce(Vector3.forward * speed * Time.deltaTime, ForceMode.Impulse);
        Destroy(this.gameObject, 0.04f);
    }
}
