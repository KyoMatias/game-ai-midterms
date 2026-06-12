using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour
{
    [Header("Scene Parameters")]
    [SerializeField]
    private string[] _scenes;

    [Header("Debug")]
    [SerializeField]
    private float _timeRemaining = 10;

    [SerializeField]
    private bool _isRunning = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartTimer();
    }

    private void StartTimer()
    {
        _isRunning = true;
    }

    void Update()
    {
        if (_isRunning)
        {
            EventManager.RaiseUpdateTimer(_timeRemaining);
            if (_timeRemaining > 0)
            {
                _timeRemaining -= Time.deltaTime;
            }
            else
            {
                _timeRemaining = 0;
                _isRunning = false;
                StartCoroutine(LoadScenes());
            }
            return;
        }
    }

    private IEnumerator LoadScenes()
    {
        foreach (var p_scene in _scenes)
        {
            var load = SceneManager.LoadSceneAsync(p_scene, LoadSceneMode.Additive);
            yield return load;
        }
        Debug.Log("Loading Scenes");
        SceneManager.UnloadSceneAsync(gameObject.scene);
    }
}
