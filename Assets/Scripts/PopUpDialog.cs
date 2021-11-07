using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpDialog : MonoBehaviour
{
    [SerializeField] public GameObject dialogPanel;
    [SerializeField] float normalTextSpeed = 0.035f;
    [SerializeField] float fastTextSpeed = 0.010f;
    [SerializeField] Color textColor = Color.white;
    [SerializeField] Color backgroundColor = Color.black;


    Text text;
    string story;
    bool textFinished = false;
    float textSpeed;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        bool isMouseOneDown = Input.GetMouseButtonDown(0);
        if (isMouseOneDown && textFinished)
        {
            GameTime.Play();
            dialogPanel.SetActive(false);
        }
        else if (isMouseOneDown)
            textSpeed = fastTextSpeed;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        text = dialogPanel.GetComponentInChildren<Text>();

        textSpeed = normalTextSpeed;
        textFinished = false;
        story = text.text;
        text.text = "";
        text.color = textColor;


        dialogPanel.SetActive(true);
        GameTime.Pause();

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
            yield return new WaitForSeconds(textSpeed);
        }

        textFinished = true;
    }
}
