using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraController : MonoBehaviour
{
    public GameObject player;

    private Vector3 offset;
    public float smooth;
    public float offsetAmount;

    // Start is called before the first frame update
    void Start()
    {
        //initialize the offset location and assign the camera location to it
        offset = new Vector3(player.transform.position.x + offsetAmount, player.transform.position.y, transform.position.z);
        transform.position = offset;
    }

    // Update is called once per frame
    void Update()
    {
        //continually update the offset location and assign the camera to it based on the player's direction
        offset = new Vector3(player.transform.position.x + offsetAmount, player.transform.position.y, transform.position.z);
        if (Movement.direction == 2) //facing left
        {
            offset = new Vector3(player.transform.position.x - offsetAmount, player.transform.position.y, transform.position.z);
        }
        else if (Movement.direction == 1) //facing right
        {
            offset = new Vector3(player.transform.position.x + offsetAmount, player.transform.position.y, transform.position.z);
        }
        transform.position = Vector3.Lerp(transform.position, offset, Time.deltaTime * smooth);
        //transform.position = Vector3.Lerp(transform.position, player.transform.position + offset, Time.deltaTime * smooth);
    }


}


