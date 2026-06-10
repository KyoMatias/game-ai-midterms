using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class SceneMasterManager : MonoBehaviour
{
    [SerializeField] private GameObject _loadingScreen; 
    public UnityEngine.UI.Slider _progressBar;
    
    [Header("Scene Layer Names")]
    [SerializeField] private string _persistentSceneName = "PERSISTENT";
    
    // Tracks the currently active map scene so we can unload it later
    private string _currentLoadedMapScene = "";
    private bool _isPersistentSceneLoaded = false;

    public static SceneMasterManager Instance { get; private set; }

    private void OnEnable()
    {
        EventManager.ON_SCENE_LOAD += LoadScene;
    }

    private void OnDisable()
    {
        EventManager.ON_SCENE_LOAD -= LoadScene;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Changing Start to an async void method lets us safely await on Unity startup
    private async void Start()
    {
        try
        {
            await LoadPersistentOnStartAsync();
            
            // Once persistent is securely online, safely initialize your game loop
            Init();
        }
        catch (Exception e)
        {
            Debug.LogError($"Error during startup scene initialization: {e.Message}");
        }
    }

    private async Awaitable LoadPersistentOnStartAsync()
    {
        // Guard against duplicate loads if running inside the Editor with layers open
        Scene persistentScene = SceneManager.GetSceneByName(_persistentSceneName);
        if (!persistentScene.isLoaded)
        {
            AsyncOperation persistentLoad = SceneManager.LoadSceneAsync(_persistentSceneName, LoadSceneMode.Additive);
            if (persistentLoad != null)
            {
                await persistentLoad;
            }
        }
        _isPersistentSceneLoaded = true;
    }

    public void Init()
    {
        EventManager.RaiseLoadScene("MAP");       
    }

    public async void LoadScene(string sceneName)
    {
        try
        {
            await LoadSceneAsync(sceneName);
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to load Scene: " + e.Message);
        }
    }

    private async Awaitable LoadSceneAsync(string newMapSceneName)
    {
        _loadingScreen.SetActive(true);
        if (_progressBar != null) _progressBar.value = 0f;

        // 1. Unload the previous map if one exists
        if (!string.IsNullOrEmpty(_currentLoadedMapScene))
        {
            AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(_currentLoadedMapScene);
            if (unloadOp != null)
            {
                await unloadOp;
            }
        }

        // 2. Load the Persistent Layer if it isn't already there (Backup safety guard)
        if (!_isPersistentSceneLoaded)
        {
            await LoadPersistentOnStartAsync();
        }

        // 3. Load the new Map Layer additively
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(newMapSceneName, LoadSceneMode.Additive);
        loadOperation.allowSceneActivation = false;

        // Monitor progress
        while (loadOperation.progress < 0.9f)
        {
            if (_progressBar != null)
            {
                float p_value = Mathf.Clamp01(loadOperation.progress / 0.9f);
                _progressBar.value = p_value;
            }

            await Awaitable.NextFrameAsync();
        }

        if (_progressBar != null) _progressBar.value = 1f;
        await Awaitable.WaitForSecondsAsync(1f);

        // 4. Activate and fully merge the map scene
        loadOperation.allowSceneActivation = true;
        await loadOperation;

        // 5. Set the map as the Active Scene
        Scene newlyLoadedScene = SceneManager.GetSceneByName(newMapSceneName);
        if (newlyLoadedScene.IsValid())
        {
            SceneManager.SetActiveScene(newlyLoadedScene);
        }

        // Track the map name so we can wipe it out on the next transition
        _currentLoadedMapScene = newMapSceneName;
        
        _loadingScreen.SetActive(false);
        
    }
}
