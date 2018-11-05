using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Beacon : MonoBehaviour
{
    [Header("Beacon Variables")]
    public PlayerMovment playerMove;
    public Text placeText;
    public GameObject beacon1, beacon2, beacon4;
    public int beaconCount;
    MeshRenderer meshRend;
    public Material unlitBeam;
    public Material litBeam;
    void Start()
    {
        playerMove = GameObject.Find("Player").GetComponent<PlayerMovment>();
        meshRend = GetComponent<MeshRenderer>();
    }
    void Update()
    {
        if (beaconCount == 1)
        {
            beacon1.gameObject.SetActive(true);
        }
        else if(beaconCount == 2)
        {
            beacon2.gameObject.SetActive(true);
        }
        else if (beaconCount == 3)
        {
            beacon4.gameObject.SetActive(true);
            // change material
        }

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (playerMove.beaconIndex == 1)
            {
                // Show place beacon Text
                placeText.enabled = true;
                if (Input.GetKeyDown(KeyCode.E))
                {
					FindObjectOfType<AudioManager>().Play("Place");
					// when (E) is pressed place a beacon
					// Player beaconIndex = -1
					playerMove.beaconIndex--;
                    beaconCount++;
                }
            }
            else
            {
                placeText.enabled = false;
            }
        }
    }
}
