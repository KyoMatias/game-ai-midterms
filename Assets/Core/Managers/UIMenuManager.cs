using System;
using UnityEngine;

/* =============================================================================
    Project:        UIMenuManager
    File:           UIMenuManager.cs
    Author:         Kyo Matias
    Studio:         SundayMood Studios
    Engine:         Unity 6.3 LTS
    Created:        2026-06-13
    ---------------------------------------------------------------------------
    Description:
    Singleton Manager pattern.
    ---------------------------------------------------------------------------
    Notes:
    ---------------------------------------------------------------------------
    - Part of the ProjectName project by SundayMood Studios.
    - Redistribution should credit the author and studio. (Kyo Matias)
    
    Notes
   ========================================================================== */

public class UIMenuManager : MonoBehaviour
{
    public static UIMenuManager Instance { get; private set; }

    public MenuState M_State { get; private set; }

    [Header("Menus")]
    [SerializeField]
    private GameObject UI_MainCanvas;

    protected virtual void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        SetupCamera();
    }

    void OnEnable() { }

    void OnDisable() { }

    void Start()
    {
        M_State = MenuState.NONE;
    }

    void SetupCamera()
    {
        Canvas canvas = GetComponent<Canvas>();
        //NOTE:Assigns the main camera once the scene loads.
        if (canvas.renderMode == RenderMode.ScreenSpaceCamera && canvas.worldCamera == null)
            canvas.worldCamera = Camera.main;
    }

    public void SetState(MenuState m_state)
    {
        LogStateChange(m_state);
        switch (m_state)
        {
            case MenuState.NONE:
                MainMenuToggle(false);
                Debug.Log("ALL MENUS DISABLED");
                break;
            case MenuState.MAIN_MENU:
                MainMenuToggle(true);
                break;
        }
    }

    void MainMenuToggle(bool p_value)
    {
        UI_MainCanvas.SetActive(p_value);
    }

    private void LogStateChange(MenuState m_state)
    {
        Debug.Log($"Switched to: {m_state}");
    }
}

public enum MenuState
{
    NONE,
    MAIN_MENU,
    PLAY,
    PAUSE,
};
