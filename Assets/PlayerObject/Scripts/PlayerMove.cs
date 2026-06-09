using System;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    public Transform Destination;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }


    public void Tick()
    {
        _agent.SetDestination(Destination.position);
    }
    
    
}
