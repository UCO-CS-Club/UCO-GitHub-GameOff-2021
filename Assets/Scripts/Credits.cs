using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{

    public string titleScreen;

    public void backToTitle()
    {
        SceneManager.LoadScene(titleScreen);
    }
}
