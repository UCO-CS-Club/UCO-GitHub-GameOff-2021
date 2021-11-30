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

    [SerializeField]
    [Range(0, 10)]
    [Header("How often the gravity change should occur (Seconds). This val >= glitchWarningDuration + glitchFullEffectDuration")]
    private float gravityChangeFrequency = 3.0f;


    private float gravFreqCounter = 0.0f;
    private const float grav = 9.8f;

    // sequence key:
    //  - u: up (player head facing up)
    //  - r: right (player head facing right)
    //  - d: down (player head facing down)
    //  - l: left (player head facing left)
    private string[] sequences =
    {
        "dlrudlrudldruldrul",
        "dlrudlrudldruldrul",
        "dlrudlrudldruldrul"
    };

    private string currentSequence;
    private int gravSequenceCounter = 0;
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
        currentSequence = sequences[0];
        player = GameObject.FindGameObjectWithTag("Player");
        postProcessGlitch = cam.GetComponent<PostProcessVolume>();
        glitchPostFX = cam.GetComponent<PostProcess>();
    }

    // Update is called once per frame
    void Update()
    {
        gravFreqCounter += Time.deltaTime;

        // gravity change WARNING effect starts [glitchWarningDuration] seconds before the gravity change
        if (gravFreqCounter >= gravityChangeFrequency - glitchFullEffectDuration - glitchWarningDuration)
        {
            Debug.Log("Starting Warning: " + gravFreqCounter);
            glitchPostFX.waveLength = Mathf.Lerp(99999, 700, t * 5); // increase the shader wavy effect
            postProcessGlitch.weight = Mathf.Lerp(0, 0.5f, t * 5); // increase post process graininess to half effect
            t += Time.deltaTime;
        }

        // gravity change FULL effect triggers [glitchFullEffectDuration] seconds before the gravity change
        if (gravFreqCounter >= gravityChangeFrequency - glitchFullEffectDuration)
        {
            Debug.Log("Starting Full Effect: " + gravFreqCounter);
            postProcessGlitch.weight = 1f; // set post process graininess to full effect
            //postProcessGlitch.GetComponent<ColorGrading>().hueShift.Over

            char d = gravityDirections[Random.Range(0, 4)]; // generate random direction from reference of directions (gravityDirections)
            ChangeGravity(d); // change gravity to the random selection
        }

        // Actual (next in sequence) gravity change occur and glitch effects are turned off
        if (gravFreqCounter >= gravityChangeFrequency)
        {
            Debug.Log("Grav Change / Reset: " + gravFreqCounter);
            char d = currentSequence[gravSequenceCounter]; // get next direction in sequence
            ChangeGravity(d); // change gravity

            gravSequenceCounter = (gravSequenceCounter + 1) % currentSequence.Length; // increment direction for next gravity change

            // turn off glitch effects
            postProcessGlitch.weight = 0f;
            glitchPostFX.waveLength = 99999;
            //postProcessGlitch.GetComponent<ColorGrading>().hueShift.value = 0f;


            // reset counter for next gravity change
            gravFreqCounter = 0;
            t = 0;
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
}
