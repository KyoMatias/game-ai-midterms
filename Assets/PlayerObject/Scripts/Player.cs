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

    void Start()
    {
        //Fetch Prerequisites First
        FetchComponents();

        //Set Player
        SetPlayerState(PlayerState.IDLE);
        InitializePlayer(P_Data);
        _ui.SetPlayerName(PlayerName);
    }

    public PlayerState SetPlayerState(PlayerState state)
    {
        return PState = state;
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
