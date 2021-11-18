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
    //  - u: up
    //  - r: right
    //  - d: down
    //  - l: left
    string currentSequence;
    string lvl1Sequence = "urdl";
    int gravSequenceCounter = 0;



    // Start is called before the first frame update
    void Start()
    {
        currentSequence = lvl1Sequence;
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
        Debug.Log(Physics2D.gravity);
        switch (d)
        {
            case 'u': // Up
                transform.eulerAngles = new Vector3(
                transform.eulerAngles.x,
                transform.eulerAngles.y,
                transform.eulerAngles.z + 180
            );
                Physics2D.gravity = new Vector2(0, grav);
                break;
            case 'd': // Down
                 
                Physics2D.gravity = new Vector2(0, grav * -1);
                break;
            case 'l': // Left

                Physics2D.gravity = new Vector2(grav * -1, 0);
                break;
            case 'r': // Right

                Physics2D.gravity = new Vector2(grav, 0);
                break;
        }
    }
}
