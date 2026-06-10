using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Prerequisites")]
    [SerializeField]
    private PlayerMove _move;
    private PlayerUI _ui;

    [Header("Camera")]
    [SerializeField]
    private Camera _camera;

    [Header("Player Variables")]
    public string PlayerName;
    public float PlayerHP;

    [Header("Player Triggers")] [SerializeField]
    private bool _canMove;
    
    [Header("State")]
    public PlayerState PState { get; private set; }

    public PlayerData P_Data;

    private void Awake()
    {
        if (_move == null)
        {
            Debug.Log("Move Controller not Found!");
        }
    }

    private void OnEnable() => EventManager.ON_PLAYER_STATE += SetPlayerState;

    private void OnDisable() => EventManager.ON_PLAYER_STATE -= SetPlayerState;

    void Start()
    {
        //Fetch Prerequisites First
        FetchComponents();

        //Set Player
        SetPlayerState(PlayerState.IDLE);
        InitializePlayer(P_Data);
        _ui.SetPlayerName(PlayerName);
    }

    public void SetPlayerState(PlayerState p_state)
    {
        if (P_Data.Player_State== p_state) return;
        P_Data.Player_State = p_state;
        HandlePlayerState(p_state);
    }

    private void HandlePlayerState(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.IDLE:
                _move.StopMoving();
                break;
            case PlayerState.MOVING:
                _move.StartMoving();
                break;
            case PlayerState.DEAD:
                //PlayerDead
                break;
        }
    }

    private void InitializePlayer(PlayerData data)
    {
        PlayerName = data.Player_Name;
        PlayerHP = data.Player_HP;
    }

    private void FetchComponents()
    {
        _ui = GetComponent<PlayerUI>();
    }

    private void Update()
    {
        _move.Tick();
    }
}
