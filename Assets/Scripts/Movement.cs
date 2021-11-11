using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField]private LayerMask groundLayer;
    private Rigidbody2D rb;
    public const string LEFT = "Left";
    public const string RIGHT = "Right";
    public const string JUMP = "Space";
    [SerializeField] public int speed;
    public float jumpVelocity;
    string buttonPressed;

    public float dashSpeed;
    private float dashTime;
    public float startDashTime;
    private int direction = 1; // default value
    public GameObject dashEffect;
    bool isDashing;
    bool startCooldown = false;
    private BoxCollider2D boxCollider;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        boxCollider = GetComponent<BoxCollider2D>();
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            buttonPressed = LEFT;
            direction = 2;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            buttonPressed = RIGHT;
            direction = 1;
        }
        else
        {
            buttonPressed = null;
        }
        if (Input.GetKey(KeyCode.Space)&& isGrounded())
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
        }
    }

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


    private bool isGrounded()
    {

        RaycastHit2D hits = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f,groundLayer);//Center Size and 0
        return hits.collider != null;
        // Maybe find a way to togle is grounded to get the sling shot effect
        
    }
    void Dash()
    {
        Instantiate(dashEffect, transform.position, Quaternion.identity);
        dashTime = startDashTime;
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


