using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class Boot : MonoBehaviour
{

    [SerializeField] private GameManager _gameManager;
    [SerializeField] private EventManager _eventManager;
    
    //*--PRIVATE VARIABLES--*//
    public float TimeRemaining = 5f;
    public bool IsTimeRunning = false;
    private void Awake()
    {
        StartDebugTimer();
    }

    void StartDebugTimer()
    {
        IsTimeRunning = true;
    }

    void Update()
    {
        if (IsTimeRunning)
        {
            if (TimeRemaining > 0)
            {
                TimeRemaining -= Time.deltaTime;
            }
            else
            {
                TimeRemaining = 0;
                IsTimeRunning = false;
                Init();
            }
            return;
        }
    }

    private void Init()
    {
        Debug.Log("BOOTING GAME!");
        SceneManager.LoadScene("PERSISTENT", LoadSceneMode.Additive);
        if(!_gameManager) Debug.Log("GameManager Not Found");
        if(!_eventManager) Debug.Log("EventManager Not Found");
        
    }

    private void StartBoot()
    {
        if(_gameManager) _gameManager.Init();
        Debug.Log("GameManager Initialized!");
        if(_eventManager) Debug.Log("EventManager Initialized");
    }
}


