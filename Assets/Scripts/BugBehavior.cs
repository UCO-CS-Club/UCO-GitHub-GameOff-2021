using UnityEngine;

public class BugBehavior : MonoBehaviour
{
    [SerializeField] private GameObject explosionBlock;
    [SerializeField] private float movementSpeed = 1f;
    [Tooltip("When the bug changes direction, this value (n*10%) will be used to determine a chance of picking the players direction")]
    [SerializeField] private float distanceBeforeJumpingAtPlayer = 4;
    [Range(1, 20)]
    [SerializeField] private int jumpYForce = 1;
    [Range(1, 20)]
    [SerializeField] private int jumpXForce = 1;
    [Range(5, 100)]
    [SerializeField] private int lifespanOfBug = 20;


    private Rigidbody2D rb2D;
    private GameObject player;
    private GameObject boss;
    private static int direction = 1; // used to set the initial l/r movement on the bug
    private bool isGrounded;

    // Crucial for the logic to determine which wall/floor/ceiling to follow
    private bool goingLeft;
    private bool goingRight;
    private bool goingUp;
    private bool goingDown;

    private int fixedFramesSinceLastOpenCornerMovementUpdate;

    private LayerMask floorLayer;
    private LayerMask wallLayer;

    // horizontal direction of the bug
    private float x;

    // vertical direction of the bug
    private float y;

    float lOffset;
    float rOffset;
    float tOffset;
    float bOffset;

    private const float MAX_RAY_DIST = 0.00001f;
    private const float OPEN_CORNER_MAX_RAY_DIST = 0.1f;
    private const float RAY_ORIGIN_OFFSET = 0.1f;
    private const int NUM_OF_EXPLOSION_SPRITES = 15;

    private float timeOfBirth;

    private bool attackingBoss;

    private bool hasChangedDirectionAlready;


    //Adding Animations 
    // First we need to access the overloaded Animation
    // Then We change the speend based on dirction 
    public Animator anim;
    private float animationSpeed = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        timeOfBirth = Time.time; 
        rb2D = transform.GetComponent<Rigidbody2D>();
        player = GameObject.Find("/PlayerFolder/Player");
        boss = GameObject.Find("/BossFolder/Bug Boss");
        Vector3 charecterScale = transform.localScale;// The reason for this is to allow the Bug To be animate and shift the models direction 
        if (direction == 1)
        {
            goingRight = true;
            goingLeft = false;
            charecterScale.x = 10;
            animationSpeed = 1.0f;
            
        }
        else
        {
            goingLeft = true;
            goingRight = false;
            charecterScale.x = -10;
            animationSpeed = +1.0f;
            
        }

        goingUp = false;
        goingDown = false;

        x = direction;
        direction = -direction;
        isGrounded = false;
        y = 0;

        floorLayer = LayerMask.NameToLayer("Ground");
        wallLayer = LayerMask.NameToLayer("Wall");

        lOffset = -(transform.localScale.x / 2.0f) - 0.001f;
        rOffset = +(transform.localScale.x / 2.0f) + 0.001f;
        tOffset = +(transform.localScale.y / 2.0f) + 0.001f;
        bOffset = -(transform.localScale.y / 2.0f) - 0.001f;

        fixedFramesSinceLastOpenCornerMovementUpdate = 0;

        attackingBoss = false;

        GetComponent<SpriteRenderer>().color = Color.red;
        hasChangedDirectionAlready = false;
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("Speed", Mathf.Abs(animationSpeed));
        float timeSinceBirth = Time.time;
        if (timeSinceBirth - timeOfBirth >= lifespanOfBug)
        {
            BugDeath();
        }

        if (!hasChangedDirectionAlready && attackingBoss)
        {
            hasChangedDirectionAlready = true;
            GetComponent<SpriteRenderer>().color = Color.green;
            ChangeDirection();
        }
    }

    private void FixedUpdate()
    {
        if (!player)
        {
            BugDeath();
            return;
        }

        fixedFramesSinceLastOpenCornerMovementUpdate--;

        // Player detected in range
        if (!attackingBoss && isGrounded)
        {
            // Cast a ray at the player, if hit and distance is less than distance before jumping at player, jump at player
            Vector2 unitVectorInTheDirectionOfThePlayer = (player.transform.position - transform.position).normalized;
            Vector2 originOfRayOutsideOfThisCollider = (Vector2)transform.position + unitVectorInTheDirectionOfThePlayer;
            var rayToPlayer = Physics2D.Raycast(originOfRayOutsideOfThisCollider, unitVectorInTheDirectionOfThePlayer, rb2D.Distance(player.gameObject.GetComponent<Collider2D>()).distance);
            if (rayToPlayer == null)
            {
                return;
            }
            
            var distanceToPlayer = rayToPlayer.collider.gameObject.name.Equals("Player") ? rayToPlayer.distance : 10000;

            if (distanceToPlayer < distanceBeforeJumpingAtPlayer)
            {
                JumpAtPlayer();
                return;
            }
        }

        if (attackingBoss && isGrounded)
        {
            // Cast a ray at the boss, if hit and distance is less than distance before jumping at player, jump at boss
            Vector2 unitVectorInTheDirectionOfTheBoss = (boss.transform.position - transform.position).normalized;
            Vector2 originOfRayOutsideOfThisCollider = (Vector2)transform.position + unitVectorInTheDirectionOfTheBoss;
            var rayToBoss = Physics2D.Raycast(originOfRayOutsideOfThisCollider, unitVectorInTheDirectionOfTheBoss, rb2D.Distance(boss.gameObject.GetComponent<Collider2D>()).distance);
            var distanceToBoss = rayToBoss.collider.gameObject.name.Equals("Bug Boss") ? rayToBoss.distance : 10000;

            if (distanceToBoss < distanceBeforeJumpingAtPlayer)
            {
                JumpAtPlayer();
                return;
            }
        }

        // Player not detected but bug has touched ground since jump
        if (isGrounded)
        {
            if (fixedFramesSinceLastOpenCornerMovementUpdate <= 0)
            {
                RaycastToSetDirection();
            }
            rb2D.velocity = new Vector2(x * movementSpeed, y * movementSpeed);
        }
    }

    private void JumpAtPlayer()
    {
        isGrounded = false;
        rb2D.gravityScale = 1;
        Vector2 jump;
        if (!attackingBoss)
        {
            jump = player.transform.position - transform.position;
        }
        else
        {
            jump = boss.transform.position - transform.position;
        }

        jump.Normalize();

        if (jump.y >= 0)
        {
            jump.y = 0.9f;
        }
        else
        {
            jump.y = -0.9f;
        }

        goingUp = goingDown = false;
        goingLeft = jump.x < 0;
        goingRight = jump.x > 0;

        jump.x *= jumpXForce;
        jump.y *= jumpYForce;
        rb2D.velocity = jump;
    }

    private void RaycastToSetDirection()
    {
        // Cast rays in all directions from origins outside the collider and at the corners of the bugs collider
        //       ^ ^
        //       | | 
        //     :=[B]=: Imagine this idea in all directions
        //       | |
        //       v v
        //------|  the floor fell out as we went right. Need to go down.

        Vector2 upLeftRayOrigin      = new Vector2(transform.position.x + lOffset, transform.position.y + transform.localScale.y / 2.0f + RAY_ORIGIN_OFFSET);
        Vector2 upRightRayOrigin     = new Vector2(transform.position.x + rOffset, transform.position.y + transform.localScale.y / 2.0f + RAY_ORIGIN_OFFSET);
        Vector2 downLeftRayOrigin    = new Vector2(transform.position.x + lOffset, transform.position.y - transform.localScale.y / 2.0f - RAY_ORIGIN_OFFSET);
        Vector2 downRightRayOrigin   = new Vector2(transform.position.x + rOffset, transform.position.y - transform.localScale.y / 2.0f - RAY_ORIGIN_OFFSET);
        Vector2 leftTopRayOrigin     = new Vector2(transform.position.x - transform.localScale.x / 2.0f - RAY_ORIGIN_OFFSET, transform.position.y + tOffset);
        Vector2 leftBottomRayOrigin  = new Vector2(transform.position.x - transform.localScale.x / 2.0f - RAY_ORIGIN_OFFSET, transform.position.y + bOffset);
        Vector2 rightTopRayOrigin    = new Vector2(transform.position.x + transform.localScale.x / 2.0f + RAY_ORIGIN_OFFSET, transform.position.y + tOffset);
        Vector2 rightBottomRayOrigin = new Vector2(transform.position.x + transform.localScale.x / 2.0f + RAY_ORIGIN_OFFSET, transform.position.y + bOffset);

        RaycastHit2D rayUpLeftCorner      = Physics2D.Raycast(upLeftRayOrigin,      Vector2.up,    MAX_RAY_DIST);
        RaycastHit2D rayUpRightCorner     = Physics2D.Raycast(upRightRayOrigin,     Vector2.up,    MAX_RAY_DIST);
        RaycastHit2D rayDownLeftCorner    = Physics2D.Raycast(downLeftRayOrigin,    Vector2.down,  MAX_RAY_DIST);
        RaycastHit2D rayDownRightCorner   = Physics2D.Raycast(downRightRayOrigin,   Vector2.down,  MAX_RAY_DIST);
        RaycastHit2D rayLeftTopCorner     = Physics2D.Raycast(leftTopRayOrigin,     Vector2.left,  MAX_RAY_DIST);
        RaycastHit2D rayLeftBottomCorner  = Physics2D.Raycast(leftBottomRayOrigin,  Vector2.left,  MAX_RAY_DIST);
        RaycastHit2D rayRightTopCorner    = Physics2D.Raycast(rightTopRayOrigin,    Vector2.right, MAX_RAY_DIST);
        RaycastHit2D rayRightBottomCorner = Physics2D.Raycast(rightBottomRayOrigin, Vector2.right, MAX_RAY_DIST);

        // Ray collision with another bug makes for dumb dumb. This solution sucks for a lot of reasons, but it's prob enough to get by for now
        if ((rayUpLeftCorner.collider      && rayUpLeftCorner     .collider.gameObject.CompareTag("Bug"))
        ||  (rayUpRightCorner.collider     && rayUpRightCorner    .collider.gameObject.CompareTag("Bug"))
        ||  (rayDownLeftCorner.collider    && rayDownLeftCorner   .collider.gameObject.CompareTag("Bug"))
        ||  (rayDownRightCorner.collider   && rayDownRightCorner  .collider.gameObject.CompareTag("Bug"))
        ||  (rayLeftTopCorner.collider     && rayLeftTopCorner    .collider.gameObject.CompareTag("Bug"))
        ||  (rayLeftBottomCorner.collider  && rayLeftBottomCorner .collider.gameObject.CompareTag("Bug"))
        ||  (rayRightTopCorner.collider    && rayRightTopCorner   .collider.gameObject.CompareTag("Bug"))
        ||  (rayRightBottomCorner.collider && rayRightBottomCorner.collider.gameObject.CompareTag("Bug"))
        ) { return; }


        // MOVEMENT LOGIC FROM HERE ON OUT

        // Going left on the floor and hitting the bottom left corner
        if (goingLeft && rayLeftBottomCorner.collider && rayDownLeftCorner.collider) 
        {
            rb2D.gravityScale = 0;
            goingLeft = goingRight = goingDown = false;
            goingUp = true;
            x = 0;
            y = 1;
            return;
        }

        // Hitting top left corner while going up the left wall
        if (goingUp && rayUpLeftCorner.collider && rayLeftTopCorner.collider)
        {
            goingLeft = goingUp = goingDown = false;
            goingRight = true;
            x = 1;
            y = 0;
            return;
        }

        // Running out of ceiling/floor while going right
        if ((goingRight || goingLeft) && !rayUpLeftCorner.collider && !rayUpRightCorner.collider && !rayDownLeftCorner.collider && !rayDownRightCorner.collider)
        {
            fixedFramesSinceLastOpenCornerMovementUpdate = 1;
            // Check above
            Vector2 posAboveBug = new Vector2(transform.position.x, transform.position.y + transform.localScale.y);
            Vector2 rRayOrigin  = new Vector2(posAboveBug.x + transform.localScale.x / 2.0f + RAY_ORIGIN_OFFSET, posAboveBug.y + tOffset + bOffset);
            Vector2 lRayOrigin  = new Vector2(posAboveBug.x - transform.localScale.x / 2.0f - RAY_ORIGIN_OFFSET, posAboveBug.y + tOffset + bOffset);
            RaycastHit2D leftRay  = Physics2D.Raycast(lRayOrigin, Vector2.left,  OPEN_CORNER_MAX_RAY_DIST);
            RaycastHit2D rightRay = Physics2D.Raycast(rRayOrigin, Vector2.right, OPEN_CORNER_MAX_RAY_DIST);

            if ((leftRay.collider && !leftRay.collider.gameObject.tag.Equals("Bug")) || (rightRay && !rightRay.collider.gameObject.tag.Equals("Bug")))
            {
                goingLeft = goingRight = goingDown = false;
                goingUp = true;
                x = 0;
                y = 1;
                return;
            }

            // Nothing found above bug. Wall must be beneath. Go down
            goingLeft = goingRight = goingUp = false;
            goingDown = true;
            x = 0;
            y = -1;
            return;
        }

        if ((goingUp || goingDown) && !rayLeftTopCorner.collider && !rayLeftBottomCorner.collider && !rayRightTopCorner.collider && !rayRightBottomCorner.collider)
        {
            fixedFramesSinceLastOpenCornerMovementUpdate = 1;
            // Check left
            Vector2 posLeftOfBug = new Vector2(transform.position.x - transform.localScale.x, transform.position.y);
            Vector2 uRayOrigin   = new Vector2(posLeftOfBug.x, posLeftOfBug.y + transform.localScale.y / 2.0f + RAY_ORIGIN_OFFSET);
            Vector2 dRayOrigin   = new Vector2(posLeftOfBug.x, posLeftOfBug.y - transform.localScale.y / 2.0f - RAY_ORIGIN_OFFSET);
            RaycastHit2D upRay   = Physics2D.Raycast(uRayOrigin, Vector2.up,   OPEN_CORNER_MAX_RAY_DIST);
            RaycastHit2D downRay = Physics2D.Raycast(dRayOrigin, Vector2.down, OPEN_CORNER_MAX_RAY_DIST);

            if ((upRay.collider && !upRay.collider.gameObject.tag.Equals("Bug")) || (downRay && !downRay.collider.gameObject.tag.Equals("Bug")))
            {
                goingRight = goingUp = goingDown = false;
                goingLeft = true;
                x = -1;
                y = 0;
                return;
            }

            goingLeft = goingUp = goingDown = false;
            goingRight = true;
            x = 1;
            y = 0;
            return;
        }

        // Going right on the ceiling and hitting top right corner
        if (goingRight && rayUpRightCorner.collider && rayRightTopCorner.collider)
        {
            goingRight = goingLeft = goingUp = false;
            goingDown = true;
            x = 0;
            y = -1;
            return;
        }

        // Going down the right wall and hitting the ground at the bottom right corner
        if (goingDown && rayRightBottomCorner.collider && rayDownRightCorner.collider)
        {
            goingRight = goingUp = goingDown = false;
            goingLeft = true;
            x = -1;
            y = 0;
            return;
        }

        // Going right on the ground and hitting the bottom right corner
        if (goingRight && rayDownRightCorner.collider && rayRightBottomCorner.collider)
        {
            rb2D.gravityScale = 0;
            goingRight = goingDown = goingLeft = false;
            goingUp = true;
            x = 0;
            y = 1;
            return;
        }

        // Going up the right wall and hitting the top right corner
        if (goingUp && rayRightTopCorner.collider && rayUpRightCorner.collider)
        {
            goingDown = goingUp = goingRight = false;
            goingLeft = true;
            x = -1;
            y = 0;
            return;
        }

        // Going left on the ceiling and hitting the top left corner
        if (goingLeft && rayUpLeftCorner.collider && rayLeftTopCorner.collider)
        {
            goingLeft = goingRight = goingUp = false;
            goingDown = true;
            x = 0;
            y = -1;
            return;
        }

        // Going down the left wall and hitting the bottom left corner
        if (goingDown && rayLeftBottomCorner.collider && rayDownLeftCorner.collider)
        {
            goingDown = goingLeft = goingUp = false;
            goingRight = true;
            x = 1;
            y = 0;
            return;
        }
    }

    private void ChangeDirection()
    {
        if (x != 0)
        {
            x = -x;
            return;
        }
        
        y = -y;
    }

    private void BugDeath()
    {
        for (int i = 0; i < NUM_OF_EXPLOSION_SPRITES; i++)
        {
            Instantiate(explosionBlock, new Vector3(transform.position.x, transform.position.y, 0), new Quaternion());
        }
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        LayerMask collisionLayer = collision.gameObject.layer;

        if (collisionLayer == floorLayer || collisionLayer == wallLayer)
        {
            isGrounded = true;
            return;
        }


        if (!attackingBoss && collision.gameObject.CompareTag("Bug Boss"))
        {
            Physics2D.IgnoreCollision(transform.GetComponent<Collider2D>(), collision.collider);
            return;
        }
        else if (attackingBoss && collision.gameObject.CompareTag("Bug Boss"))
        {
            Debug.Log("Boss Hit");
            GameObject.Find("/GameManager").GetComponent<GravityGlitchLevelController>().TryStartGravityGlitch();
            collision.gameObject.GetComponent<BugBossBehavior>().TakeDamage();
            BugDeath();
            return;
        }


        if (collision.gameObject.CompareTag("Bug"))
        {
            Physics2D.IgnoreCollision(transform.GetComponent<Collider2D>(), collision.collider);
            return;
        }


        if (!attackingBoss && collision.gameObject.layer.Equals(LayerMask.NameToLayer("BinaryBullet")))
        {
            GameObject.Find("/Bug Kill Tracker").GetComponent<BugKillTracker>().KillBug();
            BugDeath();
            return;
        } 
        else if (!attackingBoss && collision.gameObject.Equals(player))
        {
            BugDeath();
        }
        else if (attackingBoss && collision.gameObject.Equals(player))
        {
            Physics2D.IgnoreCollision(transform.GetComponent<Collider2D>(), collision.collider);

            return;
        }

    }

    public void SetAttackingBoss(bool attackingBoss)
    {
        this.attackingBoss = true;
    }

    public bool GetAttackingBoss()
    {
        return attackingBoss;
    }
}
