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

    public static event Action ON_START_GAME;
    public static event Action<GameManager.GameState> ON_GAMESTATE_UPDATE;
    public static event Action<string> ON_SCENE_LOAD;

    //Player
    public static event Action<PlayerState> ON_PLAYER_STATE;

    //Create Raisers Here
    // ex: public static void Event() => {EventName}?.Invoke();

    public static void RaiseStartGame() => ON_START_GAME?.Invoke();

    public static void RaiseUpdateGameState(GameManager.GameState state) =>
        ON_GAMESTATE_UPDATE?.Invoke(state);

    public static void RaiseUpdatePlayerState(PlayerState p_state) =>
        ON_PLAYER_STATE?.Invoke(p_state);

    public static void RaiseLoadScene(string p_scene) => ON_SCENE_LOAD?.Invoke(p_scene);
}
