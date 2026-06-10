using System;
using UnityEngine;

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

    [Header("GameState")] public GameState G_State;

    [Header("Player")] [SerializeField] private Player _player;

    [SerializeField] private string _playerName;

    [SerializeField] private float _playerHP;

    void Awake()
    {
        _instance = this;
    }

    private void OnEnable()
    {
        EventManager.ON_GAMESTATE_UPDATE += HandleGameState;
    }

    private void OnDisable()
    {
        EventManager.ON_GAMESTATE_UPDATE -= HandleGameState;
    }

    void Start()
    {

    }

    public void Init()
    {
        SetupPlayer();
        EventManager.RaiseUpdateGameState(GameState.BOOT);   
    }

    void SetupPlayer()
    {
        _playerName = _player.P_Data.Player_Name;
    }

    private void HandleGameState(GameState g_state)
    {
        switch (g_state)
        {
            case GameState.BOOT:
                InitProperties();
                //Load Stuff
                break;
            case GameState.MENU:
                //Open Menu via SceneManager
                break;
            case GameState.PRELOAD:
                EventManager.RaiseUpdatePlayerState(PlayerState.IDLE);
                // Load Active Objects
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


    private void  InitProperties()
    {
        
    }

    public enum GameState
    {
        BOOT,
        MENU,
        PRELOAD,
        LIVE,
        WIN,
        LOSE,
        POST,
        END
    }
}

