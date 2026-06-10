using System;
using UnityEngine;

public class Boot : MonoBehaviour
{

    [SerializeField] private GameManager _gameManager;
    [SerializeField] private SceneMasterManager _sceneManager;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        Init();
    }

    void Start()
    {
    }

    private void Init()
    {
        if (_gameManager == null) _gameManager = gameObject.GetComponentInChildren<GameManager>(true);
        if(!_gameManager) Debug.Log("GameManager Not Found");

        if (_sceneManager == null) _sceneManager = gameObject.GetComponentInChildren<SceneMasterManager>(true);
        
        StartBoot();
    }

    private void StartBoot()
    {
        _sceneManager.Init();
        _gameManager.Init();
        Debug.Log("GameManager Initialized!");
    }
}


