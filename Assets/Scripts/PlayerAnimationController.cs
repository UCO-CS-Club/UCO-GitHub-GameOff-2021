using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private DialogBoxSystem dialogBoxSystem;

    // Start is called before the first frame update
    void Start()
    {
        dialogBoxSystem = GameObject.FindGameObjectWithTag("GameManager").GetComponent<DialogBoxSystem>();
    }

    public void dialogClosed()
    {
        dialogBoxSystem.getDialogBoxGameObject().SetActive(false);
    }

    
    

}
