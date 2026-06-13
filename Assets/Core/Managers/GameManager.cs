using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Instance Settings

    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("No GameManager Found!");

            return _instance;
        }
    }

    #endregion

    [Header("GameState")]
    [SerializeField]
    private GameState g_state;
    public GameState G_State => g_state;

    void Awake()
    {
        _instance = this;
        Init();
    }

    private void OnEnable()
    {
        EventManager.ON_GAMESTATE_UPDATE += HandleGameState;
    }

    private void OnDisable()
    {
        EventManager.ON_GAMESTATE_UPDATE -= HandleGameState;
    }

    void Start() { }

    public void Init()
    {
        HandleGameState(GameState.BOOT);
    }

    private void HandleGameState(GameState g_state)
    {
        switch (g_state)
        {
            case GameState.BOOT:
                InitProperties();
                //Load Stuff
                break;
            case GameState.PRELOAD:
                EventManager.RaiseUpdatePlayerState(PlayerState.IDLE);
                UIMenuManager.Instance.SetState(MenuState.NONE);
                // Load Active Objects
                break;
            case GameState.MENU:
                UIMenuManager.Instance.SetState(MenuState.MAIN_MENU);
                //Open Menu via SceneManager
                break;
            case GameState.PREGAME:
                //Pregame Settings
                break;
            case GameState.LIVE:
                EventManager.RaiseUpdatePlayerState(PlayerState.MOVING);
                //Game is Live
                break;
            case GameState.WIN:
                break;
            case GameState.LOSE:
                break;
            case GameState.END:
                //Stops everything on the scene
                break;
            case GameState.POST:
                // Unloads and Clears
                break;
        }
    }

    private void InitProperties()
    {
        Debug.Log("GAMEMANAGER: Loading Properties");
    }

    public enum GameState
    {
        BOOT,
        PRELOAD,
        MENU,
        PREGAME,
        LIVE,
        WIN,
        LOSE,
        POST,
        END,
    }
}
