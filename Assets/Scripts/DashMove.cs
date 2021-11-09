using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DashMove : MonoBehaviour 
{
    private  Rigidbody2D rb;
    public float dashSpeed;
    private float dashTime;
    public float startDashTime;
    private int direction;
    public GameObject dashEffect;
    void Start()
    {
        rb = GetComponent < Rigidbody2D>();
        dashTime = startDashTime;
    }
   
    

    public void Update()
    {
        if(direction == 0 )
        {
            if (Input.GetKey(KeyCode.E))
            {
                
                if (Input.GetKeyDown(KeyCode.D))
               {
                    
                    direction = 1;
                    Dash();
                   
                }
                if (Input.GetKeyDown(KeyCode.A))
                {
                    direction = 2;
                    Dash();
                }
                else
                {
                    direction = 1;
                    Dash();
                }

                
            }

        }       
    }
    void Dash()
    {

        Instantiate(dashEffect, transform.position, Quaternion.identity);
        if (dashTime <= 0)
            {
                direction = 0;
                dashTime = startDashTime;
                rb.velocity = Vector2.zero;// I ran into a problem where if you press the dash butto it would not reset
            }
            else
            {
                dashTime -= Time.deltaTime;
            }
            if (direction == 1)
            {
                rb.velocity = Vector2.right * dashSpeed;
            
            }
            else if (direction == 2)
            {
                rb.velocity = Vector2.left * dashSpeed;
            
        }

        

    }

}
