using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    private Rigidbody2D rb;
    public const string LEFT = "Left";
    public const string RIGHT = "Right";
    public const string JUMP = "Space";
    [SerializeField] public float speed;
    public float jumpVelocity;
    string buttonPressed;
<<<<<<< HEAD
  
=======
    private float animationSpeed = 0.0f;
>>>>>>> 67ee329a874bd499f28876ee8b0c8b2aa1d9f94d

    public float dashSpeed;
    private float dashTime;
    public float startDashTime;
    public static int direction = 1; // default value
    public GameObject dashEffect;
    bool isDashing;
    bool startCooldown = false;
    private BoxCollider2D boxCollider;
    public Animator anim;
<<<<<<< HEAD
    private float animationSpeed = 0.0f;
=======

>>>>>>> 67ee329a874bd499f28876ee8b0c8b2aa1d9f94d
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    void Update()
    {

        anim.SetFloat("Speed", Mathf.Abs(animationSpeed));
        /*if (Input.GetKey(KeyCode.A))*/
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            buttonPressed = LEFT;
            direction = 2;

        }
        /*else if (Input.GetKey(KeyCode.D))*/
        else if (Input.GetAxisRaw("Horizontal") > 0)
        {

            buttonPressed = RIGHT;
            direction = 1;

        }
        else
        {
            buttonPressed = null;
        }
        if (Input.GetKey(KeyCode.Space) && isGrounded())
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
            if (isGrounded()) rb.velocity = new Vector2(0, rb.velocity.y);
        }

<<<<<<< HEAD
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
=======
        if (Input.GetKeyDown(KeyCode.E) && !isDashing)
>>>>>>> 67ee329a874bd499f28876ee8b0c8b2aa1d9f94d
        {
            isDashing = true;
            Dash();
        }
        if (!Input.anyKey)
        {
            animationSpeed = 0.0f;
        }
    }

    private void FixedUpdate()
    {
        Vector3 charecterScale = transform.localScale;
        if (!isDashing)
        {

            if (buttonPressed == RIGHT)
            {
                charecterScale.x = 10;
                animationSpeed = 1.0f;
                rb.velocity = new Vector2(speed, rb.velocity.y);
            }
            else if (buttonPressed == LEFT)
            {
                charecterScale.x = -10;
                animationSpeed = +1.0f;
                rb.velocity = new Vector2(-speed, rb.velocity.y);

            }
            transform.localScale = charecterScale;
        }
        if (buttonPressed == JUMP)
        {
            rb.AddForce(new Vector2(0, jumpVelocity), ForceMode2D.Impulse);
        }

    }


    private bool isGrounded()
    {

        RaycastHit2D hits = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);//Center Size and 0
        return hits.collider != null;

    }
    void Dash()
    {
        //Instantiate(dashEffect, transform.position, Quaternion.identity);

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


