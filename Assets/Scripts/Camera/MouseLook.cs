using UnityEngine;
using System.Collections;
//this script can be found in the Component section under the option Character Set Up 
//Mouse Look
[AddComponentMenu("FirstPerson/Camera Mouse Look")]
public class MouseLook : MonoBehaviour 
{
    #region Variables
    [Header("Rotational Axis")]
    public RotationalAxis axis = RotationalAxis.MouseX;
    //create a public link to the rotational axis called axis and set a defualt axis
    [Header("sensitivity")]
    public float sensitivityX = 10f;
    public float sensitivityY = 10f;
    //public floats for our x and y sensitivity
    [Header("Y Rotation Clamp")]
    public float minimumY = -60f;
    public float maximumY = 60f;
    //max and min Y rotation
    //we will have to invert our mouse position later to calculate our mouse look correctly
    //float for rotation Y
    public Transform Player;
    float rotationY = 0f;
    #endregion
    #region Start
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.lockState = CursorLockMode.Confined;

        //if our game object has a rigidbody attached to it
        if (this.GetComponent<Rigidbody>())
        {
            //set the rigidbodys freezRotaion to true
            this.GetComponent<Rigidbody>().freezeRotation = true;

        }
        Player = GameObject.Find("Player").GetComponent<Transform>();
    }
    #endregion
    #region Update
    void LateUpdate()
    {
            switch (axis)
            {
                #region MouseXandY
                //if our axis is set to Mouse X and Y
                case RotationalAxis.MouseXandY:
                    //float rotation x is equal to our y axis plus the mouse input on the Mouse X times our x sensitivity
                    float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX * Time.deltaTime;

                    //our rotation Y is plus equals our mouse input for Mouse Y times Y sensitivity
                    rotationY += Input.GetAxis("Mouse Y") * sensitivityY * Time.deltaTime;

                    //the rotation Y is clamped using Mathf and we are clamping the y rotation to the Y min and Y max
                    rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

                    //transform our local position to the nex vector3 rotaion - y rotaion on the x axis and x rotation on the y axis
                    transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
                    break;
                #endregion
                #region MouseX
                //we are rotating on the X
                case RotationalAxis.MouseX:
                    //transform the rotation on our game objects Y by our Mouse input Mouse X times X sensitivity
                    ////////////////(X)/////////////////(Y)/////////////////////(Z)
                    transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX * Time.deltaTime, 0);
                    break;
                #endregion
                #region MouseY
                //we are only rotation on the Y
                case RotationalAxis.MouseY:
                    //our rotation Y is pulse equals  our mouse input for Mouse Y times Y sensitivity
                    rotationY += Input.GetAxis("Mouse Y") * sensitivityY * Time.deltaTime;

                    //the rotation Y is clamped using Mathf and we are clamping the y rotation to the Y min and Y max
                    rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

                    //transform our local position to the nex vector3 rotaion - y rotaion on the x axis and local euler angle Y on the y axis
                    transform.localEulerAngles = new Vector3(-rotationY, 0, 0);
                    break;
                    #endregion
            }
        //Player.transform.rotation = new Vector3(transform.rotation, RotationalAxis.MouseX, 0);
        //Player.transform.position = RotationalAxis.MouseX;
    }
    #endregion
}
#region RotationalAxis
/*
enums are what we call state value variables 
they are comma separated lists of identifiers
we can use them to create Type or Category variables
meaning each heading of the list is a type or category element that this can be
eg weapons in an inventory are a type of inventory item
if the item is a weapon we can equipt it
if it is a consumable we can drink it
it runs different code depending on what that objects enum is set to
you can also have subtypes within those types
eg weapons are an inventory category or type
we can then have ranged, melee weapons
or daggers, short swords, long swords, mace, axe, great axe, war axe and so on
each Type or Category holds different infomation the game needs like 
what animation plays, where the hands sit on the weapon, how many hands sit on the weapon and so on
*/
//Create a public enum called RotationalAxis
public enum RotationalAxis
{
    MouseXandY, MouseX,
    MouseY
}

//Creating an enum out side the script and making it public means it can be asessed anywhere in any script with out reference
#endregion











