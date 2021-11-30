using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogBoxSystem : MonoBehaviour
{
    [SerializeField] float normalTextSpeed = 0.035f;
    [SerializeField] float fastTextSpeed = 0.010f;
    [SerializeField] Color textColor = Color.white;
    public static bool isPopUpActive;
    
    TMP_Text textComponent;
    string story;
    float textSpeed;
    bool textFinished;
    GameObject dialogBox;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        //textComponent.color = textColor;
        isPopUpActive = false;
        textFinished = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPopUpActive)
        {
            bool speedUpRequested = Input.GetMouseButtonDown(0);

            if (speedUpRequested && textFinished)
            {
                anim.SetTrigger("close");
                isPopUpActive = false;
                GameTime.Play();
            }
            else if (speedUpRequested)
                textSpeed = fastTextSpeed;
        }
    }

    public void Open(string message, GameObject dialogBox, Animator anim)
    {
        this.dialogBox = dialogBox;
        this.anim = anim;
        textFinished = false;

        // Setting up for type writer effect and flipping flag that so input is now being checked (Mouse1)
        //  Mouse1 closes the dialog box when the text is finished being displayed (see Update() )
        textComponent = dialogBox.GetComponentInChildren<TMP_Text>();

        isPopUpActive = true;
        textSpeed = normalTextSpeed;
        textComponent.text = "";
        story = message;

        dialogBox.SetActive(true);
        anim.SetTrigger("open");
        GameTime.Pause();

        StartCoroutine("PlayText");
    }

    IEnumerator PlayText()
    {
        foreach (char c in story)
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        textFinished = true;
    }

    public GameObject getDialogBoxGameObject()
    {
        return dialogBox;
    }
}
