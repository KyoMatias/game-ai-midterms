using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour
{
    [Header("Scene Parameters")]
    [SerializeField]
    private string _persistentScene = "PERSISTENT";

    [SerializeField]
    private string _mapScene = "MAP";

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
        var loadPersistent = SceneManager.LoadSceneAsync(_persistentScene, LoadSceneMode.Additive);
        yield return loadPersistent;

        var loadMap = SceneManager.LoadSceneAsync(_mapScene, LoadSceneMode.Additive);
        yield return loadMap;

        SceneManager.UnloadSceneAsync(gameObject.scene);
    }
}
