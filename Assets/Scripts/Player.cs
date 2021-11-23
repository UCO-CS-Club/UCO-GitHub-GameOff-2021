using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 5;

    //private Projectile prefab;
    private Rigidbody2D body;
    private float horizontalInput;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (GameTime.isPaused)
            return;*/

        horizontalInput = Input.GetAxis("Horizontal");

        if (horizontalInput != 0)
        {
            body.velocity = new Vector2(horizontalInput * speed, 0);
        }

        // no moving if dialog is open!
        if(DialogBoxSystem.isPopUpActive)
        {
            body.velocity = new Vector2(0, 0);
        }
    }
}
