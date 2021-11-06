using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 10;

    public GameObject projectile;
    private float horizontalInput;
    private Rigidbody2D body;



    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        horizontalInput = Input.GetAxis("Horizontal");

        if (horizontalInput != 0)
        {
            body.velocity = new Vector2(horizontalInput * speed, 0);
        }

        Vector3 charecterScale = transform.localScale;
        if (Input.GetAxis("Horizontal") < 0)
        {
            charecterScale.x = -1;
        }
        if (Input.GetAxis("Horizontal") > 0)
        {
            charecterScale.x = 1;
        }
        transform.localScale = charecterScale;


        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject shot = (GameObject) Instantiate(projectile, transform.position, transform.rotation);
            shot.transform.localScale = new Vector2(shot.transform.localScale.x * transform.localScale.x, shot.transform.localScale.y);

        }
    }
}
