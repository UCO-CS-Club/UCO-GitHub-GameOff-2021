using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D projectileRB;
    public float moveSpeed = 10;


    // Start is called before the first frame update
    void Start()
    {
        projectileRB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        projectileRB.velocity = new Vector3(moveSpeed * transform.localScale.x, 0f, 0f);
    }
}
