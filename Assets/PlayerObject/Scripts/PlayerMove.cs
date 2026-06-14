using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    private PlayerInput _playerInput;
    private bool _canMove = false;
    private Vector2 _moveInput;

    [SerializeField]
    private float _moveSpeed = 10f;

    [SerializeField]
    private Rigidbody rb;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        _playerInput.actions["Move"].performed += OnMove;
        _playerInput.actions["Move"].canceled += OnMove;
        EventManager.ON_PLAYERINPUT_TOGGLE += ToggleInput;
    }

    void OnDisable()
    {
        _playerInput.actions["Move"].performed -= OnMove;
        _playerInput.actions["Move"].performed -= OnMove;
        EventManager.ON_PLAYERINPUT_TOGGLE -= ToggleInput;
    }

    void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    public void FixedTick()
    {
        if (!_canMove)
            return;

        Vector3 move = new Vector3(_moveInput.x, 0f, _moveInput.y) * _moveSpeed;
        rb.MovePosition(rb.position + move * Time.fixedDeltaTime);
    }

    void ToggleInput(bool p_value)
    {
        if (!_playerInput)
            return;

        if (p_value)
        {
            _playerInput.ActivateInput();
            ToggleGravity(true);
        }
        else
        {
            _playerInput.DeactivateInput();
            ToggleGravity(false);
        }

        _canMove = p_value;
        Debug.Log($"Player Input is toggled to: {_canMove}");
    }

    void ToggleGravity(bool p_value)
    {
        if (!rb)
            return;

        rb.useGravity = p_value;
    }
}
