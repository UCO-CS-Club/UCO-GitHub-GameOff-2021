using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityGlitchLevelController : MonoBehaviour
{

    // controls the glitch frequency
    [SerializeField]
    [Range(0, 10)]
    private float gravityChangeFrequency = 3.0f;
    private float gravFreqCounter = 0.0f;
    private const float grav = 9.8f;

    // sequence key:
    //  - u: up (player head facing up)
    //  - r: right (player head facing right)
    //  - d: down (player head facing down)
    //  - l: left (player head facing left)
    string currentSequence;
    string lvl1Sequence = "dlur";
    int gravSequenceCounter = 0;

    GameObject player;

    static public Vector3 playerHeadUpDirection = Vector3.up;



    // Start is called before the first frame update
    void Start()
    {
        currentSequence = lvl1Sequence;
        player = GameObject.FindGameObjectWithTag("Player");

    }

    // Update is called once per frame
    void Update()
    {
        gravFreqCounter += Time.deltaTime;

        // gravity change occurs here (every [gravityChangeFrequency] seconds)
        if (gravFreqCounter >= gravityChangeFrequency)
        {
            char d = currentSequence[gravSequenceCounter];

            ChangeGravity(d);
            
            gravFreqCounter = 0;
            gravSequenceCounter = (gravSequenceCounter + 1) % currentSequence.Length;
        }

    }

    
    void ChangeGravity(char d)
    {
        Debug.Log(d);

        playerHeadUpDirection = Vector3.up;
        switch (d)
        {
            case 'd': // Gravity Down (player head facing down)
                playerHeadUpDirection = Vector3.down;
                Physics2D.gravity = new Vector2(0, grav);
                break;
            case 'u': // Gravity Up (player head facing up)
                playerHeadUpDirection = Vector3.up;
                Physics2D.gravity = new Vector2(0, grav * -1);
                break;
            case 'r': // Gravity Right (player head facing right)
                playerHeadUpDirection = Vector3.right;
                Physics2D.gravity = new Vector2(grav * -1, 0);
                break;
            case 'l': // Gravity Left (player head facing left)
                playerHeadUpDirection = Vector3.left; 
                Physics2D.gravity = new Vector2(grav, 0);
                break;
        }

        player.transform.up = playerHeadUpDirection;
    }
}
