using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinaryBulletBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(this.gameObject);
    }

    public void SetText(string binaryString)
    {
        GameObject textObject = transform.GetChild(0).GetChild(0).gameObject;
        textObject.GetComponent<TMPro.TextMeshProUGUI>().text = binaryString;
    }
}
