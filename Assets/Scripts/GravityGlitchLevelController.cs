using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class GravityGlitchLevelController : MonoBehaviour
{
    /*
     * The glitch effect is achieved using 2 methods:
     * 
     * 1. The Post-Process Volume that is attached to the MainCamera
     * 2. The PostProcess script that feeds the camera image into the 
     */

    [Header("Duration of the glitch warning effect (Seconds)")]
    [SerializeField]
    private float glitchWarningDuration = 1f;

    [Header("The duration of the full strength glitch effect (Seconds)")]
    [SerializeField]
    private float glitchFullEffectDuration = 0.5f;

    private float gravChangeTimeTracker = 10.0f;
    private bool gravChangeReady = false;
    private float secondsUntilGravityChange;

    // sequence key:
    //  - u: up (player head facing up)
    //  - r: right (player head facing right)
    //  - d: down (player head facing down)
    //  - l: left (player head facing left)
    const string gravityDirections = "dlur"; // reference to each direction to spin character randomly


    // For controller player and glitch effect(s)
    static public Vector3 playerHeadUpDirection = Vector3.up;
    static public Vector3 gravityDirection = Constants.GRAVITY_DOWN;
    private GameObject player;
    private PostProcessVolume postProcessGlitch;

    // PostProcess component is attached to the MainCamera. We use this to adjust the variables on 
    //  the material for the gravity change warning effect
    private PostProcess glitchPostFX;
    float t = 0.0f; // time value for lerping between effect off to full effect

    // Start is called before the first frame update
    void Start()
    {
        GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");
        player = GameObject.FindGameObjectWithTag("Player");
        postProcessGlitch = cam.GetComponent<PostProcessVolume>();
        glitchPostFX = cam.GetComponent<PostProcess>();
        secondsUntilGravityChange = glitchFullEffectDuration + glitchWarningDuration;
    }

    // Update is called once per frame
    void Update()
    {
        gravChangeTimeTracker += Time.deltaTime;

        if (!gravChangeReady) return;
        Debug.Log(gravChangeTimeTracker);


        // Change gravity and turn off the warning effect
        if (gravChangeTimeTracker >= secondsUntilGravityChange)
        {
            char d = gravityDirections[Random.Range(0, 4)]; // Pick random direction

            ChangeGravity(d); // change gravity

            gravChangeReady = false;

            // turn off glitch effects
            postProcessGlitch.weight = 0f;
            glitchPostFX.waveLength = 99999;

            // reset counter for next gravity change
            gravChangeTimeTracker = 0;
            t = 0;
        }
        // gravity change WARNING effect starts [glitchWarningDuration] seconds before the gravity change
        else if (gravChangeTimeTracker >= glitchWarningDuration)
        {
            //Debug.Log("Starting Full Effect: " + gravChangeTimeTracker);
            postProcessGlitch.weight = 1f; // set post process graininess to full effect
            //postProcessGlitch.GetComponent<ColorGrading>().hueShift.Over

            char d = gravityDirections[Random.Range(0, 4)]; // generate random direction from reference of directions (gravityDirections)
            ChangeGravity(d); // change gravity to the random selection
        }
        // gravity change FULL effect triggers [glitchFullEffectDuration] seconds before the gravity change
        else
        {
            //Debug.Log("Starting Warning: " + gravChangeTimeTracker);
            glitchPostFX.waveLength = Mathf.Lerp(99999, 700, t * 5); // increase the shader wavy effect
            postProcessGlitch.weight = Mathf.Lerp(0, 0.5f, t * 5); // increase post process graininess to half effect
            t += Time.deltaTime;
        }
    }

    // Changes gravity direction.
    // Parameter Notes: "char d" (player direction) is in regard to the direction their head is facing
    // (e.g. In TheRealWorld(TM), when standing your direction is 'u' )
    void ChangeGravity(char d)
    {
        switch (d)
        {
            case 'd': // Upside down (player head facing down) 
                playerHeadUpDirection = Vector3.down;
                //Physics2D.gravity = new Vector2(0, grav);
                gravityDirection = Constants.GRAVITY_UP;
                break;
            case 'u': // Rightside up (player head facing up)
                playerHeadUpDirection = Vector3.up;
                //Physics2D.gravity = new Vector2(0, grav * -1);
                gravityDirection = Constants.GRAVITY_DOWN;
                break;
            case 'r': // (player head facing right)
                playerHeadUpDirection = Vector3.right;
                //Physics2D.gravity = new Vector2(grav * -1, 0);
                gravityDirection = Constants.GRAVITY_LEFT;
                break;
            case 'l': // (player head facing left)
                playerHeadUpDirection = Vector3.left;
                //Physics2D.gravity = new Vector2(grav, 0);
                gravityDirection = Constants.GRAVITY_RIGHT;
                break;
            default:
                playerHeadUpDirection = Vector3.up; // in case switch statement fails
                break;
        }

        // set the player's up direction 
        player.transform.up = playerHeadUpDirection;
    }

    public void TryStartGravityGlitch()
    {
        if (gravChangeTimeTracker < 10.0f) return;

        gravChangeReady = true;
        gravChangeTimeTracker = 0.0f;
    }
}
