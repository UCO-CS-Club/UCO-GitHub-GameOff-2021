using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    [SerializeField] private float playerMovementSpeed = 14.0f;
    [SerializeField] private float jumpHeight = 160.0f;
    [SerializeField] private GameObject binaryBullet;
    [SerializeField] private float bulletSpeed = 2000;

    private Rigidbody2D rb2D;
    private float gravity;
    private bool isGrounded;
    private bool playerJumpKeyDown;
    private float velocity;
    private GameObject healthBar;
    private Vector2 currentGravityDirection;

    private string binaryGunString = "0101010101000011010011110010000001000010011010010110011100100000010000100111001001100001011010010110111001110011"; // UCO Big Brains
    private int currentBinaryBullet;
    private int bulletsRemaining;

    private bool binaryGunIsShooting;
    private float lastFacingDirection;

    private float timeSinceLastBulletFired;
    
    private const float SECONDS_BETWEEN_BINARY_BULLETS = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = transform.GetComponent<Rigidbody2D>();
        //rb2D.gravityScale = 9;
        gravity = Constants.GRAVITY;
        isGrounded = false;
        playerJumpKeyDown = false;
        velocity = 1;
        healthBar = GameObject.Find("/Main Camera/Health Bar Parent/Health Bar");
        if (Constants.DEV_MODE)
        {
            Debug.Assert(healthBar != null);
        }
        currentBinaryBullet = 0;
        binaryGunIsShooting = false;
        bulletsRemaining = binaryGunString.Length;
        timeSinceLastBulletFired = SECONDS_BETWEEN_BINARY_BULLETS;
        lastFacingDirection = 1;
    }

    // Update is called once per frame
    void Update()
    {
        velocity = -0.1f;

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            playerJumpKeyDown = true;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            binaryGunIsShooting = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            binaryGunIsShooting = false;
        }
    }

    private void FixedUpdate()
    {

        // Simulating Gravity: We are doing this ourselves so that we can isolate the gravity change to just the player.
        rb2D.AddForce(GravityGlitchLevelController.gravityDirection);

        if (isGrounded)
        {
            velocity = 0;

            if (playerJumpKeyDown)
            {
                Jump();
            }
        }

        MovePlayer();
        
        if (binaryGunIsShooting)
        {
            float currentTime = Time.time;
            if (currentTime - timeSinceLastBulletFired > SECONDS_BETWEEN_BINARY_BULLETS)
            {
                timeSinceLastBulletFired = currentTime;
                FireBinaryBullet();
            }
        }
    }

    private void FireBinaryBullet()
    {
        // fire as shotgun
        if (currentBinaryBullet == binaryGunString.Length)
        {
            currentBinaryBullet = 0;
        }

        string binary = binaryGunString[currentBinaryBullet].ToString();

        GameObject bullet = Instantiate(binaryBullet, transform.position, new Quaternion()); // TODO figure out rotation next

        bullet.GetComponent<BinaryBulletBehavior>().SetText(binary);
        bullet.GetComponent<Rigidbody2D>().velocity = Vector2.left * -lastFacingDirection * bulletSpeed * Time.deltaTime;


        currentBinaryBullet++;
    }

    
    private void MovePlayer()
    {
        Vector3 phd = GravityGlitchLevelController.playerHeadUpDirection;
        var h = Input.GetAxisRaw("Horizontal");

        if (phd == Vector3.up) // player head pointing up
            rb2D.velocity = new Vector2(h * playerMovementSpeed, rb2D.velocity.y);
        else if (phd == Vector3.down) // player head pointing down
            rb2D.velocity = new Vector2(h * playerMovementSpeed, rb2D.velocity.y);
        else if (phd == Vector3.right) // player head pointing right
            rb2D.velocity = new Vector2(rb2D.velocity.x, h * playerMovementSpeed * -1);
        else if (phd == Vector3.left) // player head pointing left
            rb2D.velocity = new Vector2(rb2D.velocity.x, h * playerMovementSpeed);

        if (h != 0) lastFacingDirection = h;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")) // ||
             //(collision.gameObject.layer == LayerMask.NameToLayer("Wall") && GravityGlitchLevelController.playerHeadUpDirection == Vector3.left) ||
             //(collision.gameObject.layer == LayerMask.NameToLayer("Wall") && GravityGlitchLevelController.playerHeadUpDirection == Vector3.down) ||
             //(collision.gameObject.layer == LayerMask.NameToLayer("Wall") && GravityGlitchLevelController.playerHeadUpDirection == Vector3.right))
        {
            isGrounded = true;
        }

        else if (collision.gameObject.tag.Equals("Bug"))
        {
            HealthBarBehavior hbb = healthBar.GetComponent<HealthBarBehavior>();
            hbb.TakeDamage(0.23f);
            if (hbb.GetHealth() <= 0)
            {
                PlayerDeath();
            }
        }

        else if (collision.gameObject.tag.Equals("Bug Boss"))
        {
            HealthBarBehavior hbb = healthBar.GetComponent<HealthBarBehavior>();
            hbb.TakeDamage(0.46f);
            if (hbb.GetHealth() <= 0)
            {
                PlayerDeath();
            }
        }
    }

    private void PlayerDeath()
    {
        GameObject bugBoss = GameObject.Find("/BossFolder/Bug Boss");
        if (Constants.DEV_MODE)
        {
            Debug.Assert(bugBoss != null);
        }

        bugBoss.GetComponent<BugBossBehavior>().PlayerLost();
        Destroy(this.gameObject);
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
        Vector3 phd = GravityGlitchLevelController.playerHeadUpDirection;
        velocity = jumpHeight;

        if (phd == Vector3.up)
            rb2D.AddForce(new Vector2(0, velocity * gravity));
        else if (phd == Vector3.down)
            rb2D.AddForce(new Vector2(0, velocity * gravity * -1));
        else if (phd == Vector3.right)
            rb2D.AddForce(new Vector2(velocity * gravity, 0));
        else if (phd == Vector3.left)
            rb2D.AddForce(new Vector2(velocity * gravity * -1, 0));

        playerJumpKeyDown = false;
    }
}
