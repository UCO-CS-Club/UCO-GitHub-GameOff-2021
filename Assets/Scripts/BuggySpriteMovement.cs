using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuggySpriteMovement : MonoBehaviour
{
    Player player;
    int frameCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
