using System;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    public Transform Destination;

    private bool _canMove;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }


    public void Tick()
    {
        if (!_canMove) return;
        if(_canMove) _agent.SetDestination(Destination.position);
    }

    public void StartMoving()
    {
        _canMove = true;
        _agent.isStopped = false;
    }
    public void StopMoving()
    {
        _canMove = false;
        _agent.isStopped = true;
        _agent.ResetPath();
    }
    
}
