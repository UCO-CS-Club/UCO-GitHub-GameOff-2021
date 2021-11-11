using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public GameObject spawnPoint;
    public GameObject player;
    public int bottomDeath;
    public int top;
    public int left;
    public int right; // we can use these to set the limits of the game and force the players to stay in bounds


    void Update()
    {
        if (player.transform.position.y <= bottomDeath)//(player.transform.position.x >= top || same for other postions)
        {
            player.transform.position = spawnPoint.transform.position;
        }
    }
}
