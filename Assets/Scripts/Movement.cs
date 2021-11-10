using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody2D rb;
    public const string LEFT = "Left";
    public const string RIGHT = "Right";
    public const string JUMP = "Space";
    public int speed;
    public float jumpVelocity;
    string buttonPressed;
    bool Grounded;
    // test 
    //private Rigidbody2D rb;
    public float dashSpeed;
    private float dashTime;
    public float startDashTime;
    private int direction = 1; // default value
    public GameObject dashEffect;
    bool isDashing;
    bool startCooldown = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            buttonPressed = LEFT;
            direction = 2;
            //transform.position += transform.right * (Time.deltaTime * 5);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            buttonPressed = RIGHT;
            direction = 1;
            //transform.position += transform.right * (Time.deltaTime * 5);
        }
        else
        {
            buttonPressed = null;
        }
        if (Input.GetKey(KeyCode.Space)&& Grounded == true)
        {
            buttonPressed = JUMP;
        }

        if (startCooldown)
        {
            dashTime -= Time.deltaTime;
        }

        if (dashTime <= 0)
        {
            isDashing = false;
            startCooldown = false;
        }

        if (Input.GetKeyDown(KeyCode.E) && !isDashing)
        {
            isDashing = true;
            Dash();
            /*if (Input.GetKeyDown(KeyCode.D))
            {

                direction = 1;
                Dash();

            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                direction = 2;
                Dash();
            }
            else
            {
                direction = 1;
                Dash();

            }
            isDashing = false;*/
        }


    }
    // We then make another fuction for what the button was
    private void FixedUpdate()
    {
        if (!isDashing)
        {
            if (buttonPressed == RIGHT)
            {
                rb.velocity = new Vector2(speed, rb.velocity.y);
            }
            else if (buttonPressed == LEFT)
            {
                rb.velocity = new Vector2(-speed, rb.velocity.y);
            }
        }
        if (buttonPressed == JUMP)
        {
            rb.AddForce(new Vector2(0, jumpVelocity), ForceMode2D.Impulse);
        }

    }

    void OnCollisionStay2D(Collision2D collider)
    {
        CheckIfGrounded();
    }

    void OnCollisionExit2D(Collision2D collider)
    {
        Grounded = false;
    }

    private void CheckIfGrounded()
    {
        RaycastHit2D[] hits;

        //We raycast down 1 pixel from this position to check for a collider
        Vector2 positionToCheck = transform.position;
        hits = Physics2D.RaycastAll(positionToCheck, new Vector2(0, -1), 0.01f);

        //if a collider was hit, we are grounded
        if (hits.Length > 0)
        {
            Grounded = true;
        }
    }
    void Dash()
    {
        Instantiate(dashEffect, transform.position, Quaternion.identity);
        //direction = 0;
        dashTime = startDashTime;
        //rb.velocity = Vector2.zero;// I ran into a problem where if you press the dash butto it would not reset
        /*if (dashTime <= 0)
        {
            Instantiate(dashEffect, transform.position, Quaternion.identity);
            isDashing = false;
            direction = 0;
            dashTime = startDashTime;
            rb.velocity = Vector2.zero;// I ran into a problem where if you press the dash butto it would not reset
            dashTime -= Time.deltaTime;
        }
        else
        {
            dashTime -= Time.deltaTime;
        }*/
        if (direction == 1)
        {
            rb.velocity = Vector2.right * dashSpeed;
        }
        else if (direction == 2)
        {
            rb.velocity = Vector2.left * dashSpeed;
        }
        startCooldown = true;

    }

}


