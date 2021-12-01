using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpPower = 5f;
    [SerializeField] private LayerMask groundLayer;

    //private Projectile prefab;
    private Rigidbody2D body;
    private BoxCollider2D boxCollider;
    private Animator anim;
    private float horizontalInput;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (GameTime.isPaused)
            return;*/

        Move();

        if (Input.GetKey(Constants.JUMP_KEY) && isGrounded())
            Jump();
    }

    private void Jump()
    {
        Vector3 phd = GravityGlitchLevelController.playerHeadUpDirection;

        if (phd == Vector3.up)
            body.velocity = new Vector2(body.velocity.x, jumpPower);
        else if (phd == Vector3.down)
            body.velocity = new Vector2(body.velocity.x, jumpPower * -1);
        else if (phd == Vector3.right)
            body.velocity = new Vector2(jumpPower, body.velocity.y);
        else if (phd == Vector3.left)
            body.velocity = new Vector2(jumpPower * -1, body.velocity.y);
    }

    private void Move()
    {
        Vector3 phd = GravityGlitchLevelController.playerHeadUpDirection;
        horizontalInput = Input.GetAxis("Horizontal");

        if (phd == Vector3.up) // player head pointing up
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);
        else if (phd == Vector3.down) // player head pointing down
            body.velocity = new Vector2(horizontalInput * speed * -1, body.velocity.y);
        else if (phd == Vector3.right) // player head pointing right
            body.velocity = new Vector2(body.velocity.x, horizontalInput * speed * -1);
        else if (phd == Vector3.left) // player head pointing left
            body.velocity = new Vector2(body.velocity.x, horizontalInput * speed);
    }

    private bool isGrounded()
    {
        Vector3 phd = GravityGlitchLevelController.playerHeadUpDirection;
        horizontalInput = Input.GetAxis("Horizontal");

        phd *= -1;

        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, phd, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }
}
