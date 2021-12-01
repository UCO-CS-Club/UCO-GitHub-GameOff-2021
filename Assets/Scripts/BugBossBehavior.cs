using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugBossBehavior : MonoBehaviour
{
    [SerializeField] private GameObject bug;
    [Tooltip("The Bug Boss releases this many bugs every Time Between Bugs")]
    [Range(0, 100)]
    [SerializeField] private int numberOfBugsReleased = 5;
    [Tooltip("The time elapsed between the last bug release and the next bug release")]
    [Range(0.1f, 30)]
    [SerializeField] private float timeBetweenBugs = 12;
    [Range(0, 50)]
    [SerializeField] private float movementSpeed = 4;
    [SerializeField] private float jumpHeight = 160.0f;
    [SerializeField] private float gravity = 9.8f;
    [Tooltip("This value serves as a timer to decrement the other timer so that bugs start appearing sooner as the game progresses")]
    [SerializeField] private float decrementTimeBetweenBugs = 10;
    [Range(0, 100)]
    [SerializeField] private int health = 100;

    private float timeSinceLastBugRelease;
    private float timeSinceLastDecrement;
    private Rigidbody2D rb2D;
    private LayerMask floorLayer;

    private GameObject player;
    private bool isGrounded;
    private bool floorAlreadyDetected;

    private const float WALL_CHECK_DIST = 0.1f;
    private const float MAX_DIST_CHECK_FOR_FLOOR = 4.5f;
    private const float SIDEWAYS_PLAYER_OFFSET = 5.1f;
    private const float CHANCE_OF_JUMPING = 0.43f;

    private const float TIMER_DECREMENT = 0.1f;
    // horizontal direction of the boss
    private float x;


    // Need a way to show him moving
    public Animator anim;
    private float animationSpeed = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = transform.GetComponent<Rigidbody2D>();
        player = GameObject.Find("/PlayerFolder/Player");

        isGrounded = true;
        x = -1;

        floorLayer = LayerMask.NameToLayer("Ground");

        floorAlreadyDetected = false;
        timeSinceLastDecrement = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        float currentTime = Time.time;
        anim.SetFloat("Speed", Mathf.Abs(animationSpeed));
        if (currentTime - timeSinceLastBugRelease >= timeBetweenBugs)
        {
            timeSinceLastBugRelease = currentTime;
            for (int i = 0; i < numberOfBugsReleased; i++)
            {
                // The very left of the bug boss
                float posX = transform.position.x;
                float halfWidth = transform.localScale.x / 2.0f;
                float bugWidth = bug.transform.localScale.x;
                posX = posX - halfWidth - bugWidth;

                // alternate sides of the big boss
                if (i % 2 == 0)
                {
                    posX += 2 * halfWidth + 2 * bugWidth;
                }

                // The feet of the bug boss
                float posY = transform.position.y;
                float halfHeight = transform.localScale.y / 2.0f;
                float halfBugHeight = bug.transform.localScale.y / 2.0f;
                posY = posY - halfHeight + halfBugHeight;

                Instantiate(bug, new Vector3(posX, posY, 0), new Quaternion());
            }
        }

        if (timeBetweenBugs >= 1 && currentTime - timeSinceLastDecrement >= decrementTimeBetweenBugs)
        {
            timeSinceLastDecrement = currentTime;
            timeBetweenBugs -= TIMER_DECREMENT;
        }
    }

    private void FixedUpdate()
    {
        RayCheckForWalls();
        CheckForPlatformsToJumpTo();
        rb2D.velocity = new Vector2(x * movementSpeed, rb2D.velocity.y);
    }

    private void RayCheckForWalls()
    {
        Vector2 offsetLeft  = new Vector2(transform.position.x - transform.localScale.x / 2.0f - 0.1f, transform.position.y + transform.localScale.y / 2.0f + 0.1f);
        Vector2 offsetRight = new Vector2(transform.position.x + transform.localScale.x / 2.0f + 0.1f, transform.position.y + transform.localScale.y / 2.0f + 0.1f);

        RaycastHit2D leftRay  = Physics2D.Raycast(offsetLeft,  Vector2.left,  WALL_CHECK_DIST);
        RaycastHit2D rightRay = Physics2D.Raycast(offsetRight, Vector2.right, WALL_CHECK_DIST);

        if ((leftRay.collider && !leftRay.collider.CompareTag("Player")) || (rightRay.collider && !rightRay.collider.CompareTag("Player")))
            x = -x;
    }

    private void CheckForPlatformsToJumpTo()
    {
        Vector2 upOffset = new Vector2(transform.position.x, transform.position.y + transform.localScale.y / 2.0f + 0.1f);
        RaycastHit2D directlyAbovePlayer = Physics2D.Raycast(upOffset, Vector2.up, MAX_DIST_CHECK_FOR_FLOOR);
        if (directlyAbovePlayer.collider)
        {
            floorAlreadyDetected = false;
            return;
        }

        Vector2 playerOffsetPosition;

        if (x == -1)
        {
            playerOffsetPosition = new Vector2(transform.position.x - transform.localScale.x * SIDEWAYS_PLAYER_OFFSET, transform.position.y + transform.localScale.y / 2.0f + 0.1f);
        }
        else
        {
            playerOffsetPosition = new Vector2(transform.position.x + transform.localScale.x * SIDEWAYS_PLAYER_OFFSET, transform.position.y + transform.localScale.y / 2.0f + 0.1f);
        }


        var rayUpFromOffsetPosition = Physics2D.Raycast(playerOffsetPosition, Vector2.up, MAX_DIST_CHECK_FOR_FLOOR); ;

        LayerMask collisionLayer = rayUpFromOffsetPosition.collider ? rayUpFromOffsetPosition.collider.gameObject.layer : -1;

        float rVal = Random.value;
        if (collisionLayer.value == floorLayer.value && isGrounded)
        {
            if (rVal < CHANCE_OF_JUMPING && !floorAlreadyDetected)
            {
                Jump();
            }
            else
            {
                floorAlreadyDetected = true;
            }
        }
    }

    private void Jump()
    {
        rb2D.AddForce(new Vector2(0, jumpHeight * gravity));
        isGrounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("BinaryBullet")))
        {
            Physics2D.IgnoreCollision(transform.GetComponent<Collider2D>(), collision.collider);
            return;
        }

        LayerMask collisionLayer = collision.gameObject.layer;

        if (collisionLayer == floorLayer)
        {
            isGrounded = true;
        }

        if (collision.gameObject.Equals(player))
        {
            Physics2D.IgnoreCollision(transform.GetComponent<Collider2D>(), collision.collider);
            return;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == floorLayer)
        {
            isGrounded = false;
        }
    }

    public void PlayerLost()
    {
        numberOfBugsReleased = 0;
    }

    public void TakeDamage()
    {
        health -= 5;
        if (health <= 0)
        {
            BossDeath();
        }
        Debug.Log(health);
    }

    private void BossDeath()
    {
        Debug.Log("Boss Death");
    }

}
