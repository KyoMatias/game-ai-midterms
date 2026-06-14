using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    private PlayerInput _playerInput;
    private bool _canMove = false;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
    }

    void OnEnable()
    {
        EventManager.ON_PLAYERINPUT_TOGGLE += ToggleInput;
    }

    void OnDisable()
    {
        EventManager.ON_PLAYERINPUT_TOGGLE -= ToggleInput;
    }

    void Start() { }

    public void Tick() { }

    void ToggleInput(bool p_value)
    {
        if (!_playerInput)
            return;
        if (_playerInput && _canMove)
        {
            _playerInput.ActivateInput();
            Debug.Log($"Player Input is toggled to: {_canMove}");
        }
        else if (_playerInput && !_canMove)
        {
            _playerInput.DeactivateInput();
            Debug.Log($"Player Input is toggled to: {_canMove}");
        }
    }
}
