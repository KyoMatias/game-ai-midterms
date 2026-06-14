using System;
using UnityEngine;

/* =============================================================================
    Project:        Midtersm
    File:           EventManager.cs
    Author:         Kyo Matias
    Studio:         SundayMood Studios
    Engine:         Unity 6.3 LTS
    Created:        2026-06-10
    ---------------------------------------------------------------------------
    Description:
    SundayMood Studios Standalone Modular EventManager.
    ---------------------------------------------------------------------------
    Notes:
    ---------------------------------------------------------------------------
    - Part of the ProjectName project by SundayMood Studios.
    - Redistribution should credit the author and studio. (Kyo Matias)
    
    Notes
   ========================================================================== */

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    protected virtual void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    //Create Events Here
    // ex: public static event Action {EventName};
    //*-------------------------------------------*//

    //GAME STATES
    public static event Action ON_START_GAME;
    public static event Action<GameState> ON_GAMESTATE_UPDATE;
    public static event Action<float> ON_TIMER_UPDATE;

    //MENUS

    //Player
    public static event Action<PlayerState> ON_PLAYER_STATE;
    public static event Action<bool> ON_PLAYERINPUT_TOGGLE;

    //UI
    public static event Action<float, bool> ON_DETECTION;

    //Create Raisers Here
    // ex: public static void Event() => {EventName}?.Invoke();

    public static void RaiseStartGame() => ON_START_GAME?.Invoke();

    public static void RaiseUpdateGameState(GameState state) => ON_GAMESTATE_UPDATE?.Invoke(state);

    public static void RaiseUpdatePlayerState(PlayerState p_state) =>
        ON_PLAYER_STATE?.Invoke(p_state);

    public static void RaiseUpdateTimer(float p_value) => ON_TIMER_UPDATE?.Invoke(p_value);

    public static void RaiseTogglePlayerInput(bool p_value) =>
        ON_PLAYERINPUT_TOGGLE?.Invoke(p_value);

    public static void RaiseOnDetection(float p_value, bool b_value) =>
        ON_DETECTION?.Invoke(p_value, b_value);
}
