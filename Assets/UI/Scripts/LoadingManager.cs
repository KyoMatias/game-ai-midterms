using UnityEngine;
using UnityEngine.UI;

/* =============================================================================
    Project:        LoadingManager
    File:           LoadingManager.cs
    Author:         Kyo Matias
    Studio:         SundayMood Studios
    Engine:         Unity 6.3 LTS
    Created:        2026-06-13
    ---------------------------------------------------------------------------
    Description:
    Standard MonoBehaviour component.
    ---------------------------------------------------------------------------
    Notes:
    ---------------------------------------------------------------------------
    - Part of the ProjectName project by SundayMood Studios.
    - Redistribution should credit the author and studio. (Kyo Matias)
    
    Notes
   ========================================================================== */

public class LoadingManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _loadingCanvasUI;

    [SerializeField]
    private Slider _slider;

    void OnEnable()
    {
        EventManager.ON_TIMER_UPDATE += ProgressBar;
    }

    void OnDisable()
    {
        EventManager.ON_TIMER_UPDATE -= ProgressBar;
    }

    void Awake() { }

    void ProgressBar(float p_value)
    {
        float percent = (10f - p_value) / 10f;
        _slider.value = Mathf.Clamp01(percent);
    }
}
