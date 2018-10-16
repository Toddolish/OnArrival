using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwap : MonoBehaviour
{
	// ++ means plus 1
	public Transform[] lookObject; //array of object to look at
    public bool smooth = true; //is smooth enabled
    public float damping = 6; //smootheness value of camera

    [Header("GUI")]
    public float scrW;
    public float scrH;

    private int lookIndex; //index into array of lookObjects
    private int lookMax; //stores the total amount of lookObjects
    private Transform target;//current target look object

	void Start ()
    {
        //last ondex of array
        lookMax = lookObject.Length-1;
	}
    private void LateUpdate()
    {
        //Get current object to look at
        target = lookObject[lookIndex];

        //if target not null
        if(target)
        {
            //is smoothing enabled?
            if(smooth)
            {
                //calculate direction to look at target
                Vector3 lookDirection = target.position - transform.position;
                Quaternion rotation = Quaternion.LookRotation(lookDirection);
                //look at and damping the rotation
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
            }
            else
            {
                //just look at the target without the smooth or damping
                transform.LookAt(target);
            }
        }
        else
        {
            //keep swapping cameras until a valid target is found
            CamSwap();
        }
    }
    void Update ()
    {
		
	}
    void CamSwap()
    {
        //increase index by 1 to select next object
            lookIndex++;

        //if index is greater then our max array size
        if (lookIndex > lookMax)
        {
            //reset cam index back to zero
            lookIndex = 0;
        }
    }
    public void swapCamera()
    {
      //swap the cameras
      CamSwap();
    }
}
