using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Prerequisites")]
    [SerializeField] private PlayerMove _move;
    
    [Header("Camera")]
    [SerializeField] private Camera _camera;

    [Header("Player Variables")] 
    [SerializeField] private string _playerName;

    [SerializeField] private string _playerHP;


    [Header("State")] public PlayerState PState { get; private set; }


    private void Awake()
    {
        if (_move == null)
        {
            Debug.Log("Move Controller not Found!");
        }
    }

    void Start()
    {
        SetPlayerState(PlayerState.IDLE);   

    }

    public void SetPlayerState(PlayerState state)
    {
        PState = state;
    }

    private void Update()
    {
        _move.Tick();
    }
}
