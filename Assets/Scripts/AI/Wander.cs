using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SteeringBehaviours;
using GGL;

public class Wander : SteeringBehaviour
{
    public float offset = 1f;
    public float radius = 1f;
    public float jitter = 0.2f;

    private Vector3 targetDir;
    private Vector3 randomDir;
    
    void Start()
    {

    }
    void Update()
    {

    }
    public override Vector3 GetForce()
    {
        Vector3 force = Vector3.zero;
        float randX = Random.Range(0, 0x7fff) - (0x7fff / 2);
        float randZ = Random.Range(0, 0x7fff) - (0x7fff / 2);

        #region Calculate RandomDir
        // Create the randomdir
        randomDir = new Vector3(randX, 0, randZ);
        // Normalize randomDir
        randomDir.Normalize();
        //Apply jitter to it
        randomDir *= jitter;
        #endregion

        #region Calculate TargetDir
        // Offset targetDir with randomDir;
        targetDir += randomDir;
        // Normalize the targetDir;
        targetDir.Normalize();
        // Apply radius to it
        targetDir *= radius;

        #endregion
        
        // Get position of point
        Vector3 seekPos = transform.position + targetDir;
        // Offset the seek position
        seekPos += transform.forward * offset;
		
        #region GizmosGL
        GizmosGL.color = Color.red;
        GizmosGL.AddCircle(seekPos + Vector3.up * 0.2f, 0.5f, Quaternion.LookRotation(Vector3.down));
        Vector3 offsetPos = transform.position + transform.forward * offset;

        GizmosGL.color = Color.blue;
        GizmosGL.AddCircle(offsetPos + Vector3.up * 0.1f, radius, Quaternion.LookRotation(Vector3.down));

        GizmosGL.color = Color.cyan;
        GizmosGL.AddLine(transform.position, offsetPos, 0.1f, 0.1f);
        #endregion

        // Calculate direction
        Vector3 direction = seekPos - transform.position;

        Vector3 desiredPos = Vector3.zero;

        // Check if direction is valid
        if(direction != Vector3.zero) // or direction.magnitude != 0
        {
            // Apply a weighting to the direction
            desiredPos = direction.normalized * weighting;
            // Apply the force
            force = desiredPos - owner.velocity;
        }
        return force;
    }
}
