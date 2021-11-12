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
        if (GameTime.isPaused)
            return;

        horizontalInput = Input.GetAxis("Horizontal");

        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

        if (Input.GetKey(Constants.JUMP_KEY) && isGrounded())
            Jump();
    }

    private void Jump()
    {
        body.velocity = new Vector2(body.velocity.x, jumpPower);
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }
}
