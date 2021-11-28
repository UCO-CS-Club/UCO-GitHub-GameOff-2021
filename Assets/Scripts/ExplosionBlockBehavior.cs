using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBlockBehavior : MonoBehaviour
{

    private float x;
    private float y;

    private float scale;

    private Vector2 trajectory;

    private float timeUntilDeath;
    private float timeOfBirth;

    private const float MAX_SIZE = 0.2f;
    private const float MAX_MOVE_SPEED = 0.005f;


    // Start is called before the first frame update
    void Start()
    {
        x = Random.value % MAX_MOVE_SPEED;
        y = Random.value % MAX_MOVE_SPEED;

        float chanceX = Random.value;

        if (chanceX < 0.5f)
        {
            x = -x;
        }

        float chanceY = Random.value;

        if (chanceY < 0.5f)
        {
            y = -y;
        }

        scale = Random.value % MAX_SIZE;

        transform.localScale = new Vector2(scale, scale);

        timeUntilDeath = Random.value;
        timeOfBirth = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        float currentTime = Time.time;

        if (currentTime - timeOfBirth > timeUntilDeath)
        {
            Destroy(this.gameObject);
            return;
        }


        float posX = transform.position.x + x;
        float posY = transform.position.y + y;

        trajectory = new Vector2(posX, posY);

        transform.position = trajectory;
    }
}
