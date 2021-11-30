using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GameState { INTRO, MAIN_MENU }/// <summary>
/// THis will allow us to set up an intro and then a main menue 
/// From their we can String scenes togetether 
/// </summary>
// Needed a way to not delete and manage the game state
public delegate void OnStateChangeHandler();

public class GameManager : MonoBehaviour
{

    protected GameManager() { }
    private static GameManager instance = null;
    public event OnStateChangeHandler OnStateChange;
   
    public GameState gameState { get; private set; }
    // This Should be the load safe link.
    // Will manage levels and Load scenes in. 
    public static GameManager Instance
    {
        get { 
            if(GameManager.instance == null)
            {
                DontDestroyOnLoad(GameManager.instance);
                GameManager.instance = new GameManager();
            }
            return GameManager.instance;
        }

    }
    public void SetGameState(GameState state)
    {
        this.gameState = state;
        OnStateChange();
    }

    public void OnApplicationQuit()
    {
        GameManager.instance = null;
    }
}
