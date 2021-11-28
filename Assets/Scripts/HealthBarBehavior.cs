using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarBehavior : MonoBehaviour
{
    private float startingPosX = -13.5f;
    private float startingPosY = 8.5f;
    private float startingWidth = 6;
    private float startingHeight = 0.5f;
    private bool  strobe = false;
    private Color healthFlashColor;
    private float flashTimer;
    private SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (strobe)
        {
            FlashHealthBar();
        }
    }

    public void TakeDamage(float damage)
    {
        if (transform.localScale.x <= 0) return;

        transform.localScale = new Vector3(transform.localScale.x - damage, startingHeight, 1);
        transform.localPosition = new Vector3(transform.localPosition.x - damage / 2, startingPosY, 1);
        
        if (transform.localScale.x <= 2)
        {
            sr.color = Color.red;
            strobe = true;
            flashTimer = Time.time;
            healthFlashColor = new Color(0.4f, 0, 0);
        }
        else if (transform.localScale.x <= 3)
        {
            sr.color = Color.yellow;
        }

        if (transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(0, startingHeight, 1);
        }
    }

    public float GetHealth()
    {
        return transform.localScale.x;
    }

    private void ResetHealth()
    {
        transform.localScale = new Vector3 (startingWidth, startingHeight, 1);
        transform.localPosition = new Vector3(startingPosX, startingPosY, 1);
        sr.color = Color.green;
        strobe = false;
    }

    private void FlashHealthBar()
    {
        float currentTime = Time.time;
        if (currentTime - flashTimer > 0.2f)
        {
            flashTimer = currentTime;
            Color lastColor = sr.color;
            sr.color = healthFlashColor;
            healthFlashColor = lastColor;
        }
    }
}
