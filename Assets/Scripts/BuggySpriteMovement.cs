using UnityEngine;

public class BuggySpriteMovement : MonoBehaviour
{
    [Range(0, 75)]
    [SerializeField] float followSpeed = 50f;


    Rigidbody2D playerBody;
    Rigidbody2D body;

    // Start is called before the first frame update
    void Start()
    {
        playerBody = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().GetComponent<Rigidbody2D>();
        body = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        body.position = Vector2.Lerp(body.position, playerBody.position, followSpeed * Time.deltaTime);
    }
}
