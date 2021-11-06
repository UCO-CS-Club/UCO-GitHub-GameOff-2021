using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpDialog : MonoBehaviour
{
    [SerializeField] public GameObject dialogPanel;

    Text text;
    string story;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        text = dialogPanel.GetComponentInChildren<Text>();
        story = text.text;
        Debug.Log(story);
        text.text = "";
        dialogPanel.SetActive(true);

        StartCoroutine("PlayText");
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    story = txt.text;
    //    txt.text = "";

    //    // TODO: add optional delay when to start
    //    StartCoroutine("PlayText");
    //}

    IEnumerator PlayText()
    {
        foreach (char c in story)
        {
            text.text += c;
            yield return new WaitForSeconds(0.075f);
        }
    }
}
