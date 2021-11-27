using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNonBuggyBehavior : MonoBehaviour
{
    [SerializeField] private float playerMovementSpeed = 14.0f;
    [SerializeField] private float gravity = 9.8f;
    [SerializeField] private float jumpHeight = 160.0f;

    private Rigidbody2D rb2D;
    private bool isGrounded;
    private bool playerJumpKeyDown;
    private float velocity;
    private float x;
    private float y;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = transform.GetComponent<Rigidbody2D>();
        rb2D.gravityScale = 9;
        isGrounded = false;
        playerJumpKeyDown = false;
        velocity = 1;
    }

    // Update is called once per frame
    void Update()
    {
        velocity = -0.1f;

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            playerJumpKeyDown = true;
        }
    }

    private void FixedUpdate()
    {
        if (isGrounded)
        {
            velocity = 0;

            if (playerJumpKeyDown)
            {
                Jump();
            }
        }

        MovePlayer();
    }

    
    private void MovePlayer()
    {
        var horizontalInput = Input.GetAxisRaw("Horizontal");
        rb2D.velocity = new Vector2(horizontalInput * playerMovementSpeed, rb2D.velocity.y);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = false;
        }
    }

    private void Jump()
    {
        velocity = jumpHeight;
        rb2D.AddForce(new Vector2(0, velocity * gravity));
        playerJumpKeyDown = false;
    }
}
