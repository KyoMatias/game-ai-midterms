using System.Collections.Generic;
using UnityEngine;

public class PlayerLeader : MonoBehaviour, ILeader
{
    private List<EnemyMovement> _followers = new List<EnemyMovement>();

    public Transform Transform => transform;
    public int FollowerCount => _followers.Count;

    void OnCollisionEnter(Collision collision)
    {
        ILeader other = collision.collider.GetComponentInParent<ILeader>();
        if (other == null || (object)other == (object)this)
            return;

        if (FollowerCount < other.FollowerCount)
            Die();
        else if (other.FollowerCount < FollowerCount)
            other.Die();
        // equal counts: no-op
    }

    public void RegisterFollower(EnemyMovement mini)
    {
        if (!_followers.Contains(mini))
            _followers.Add(mini);
    }

    public void UnregisterFollower(EnemyMovement mini)
    {
        _followers.Remove(mini);
    }

    public void Die()
    {
        foreach (EnemyMovement mini in _followers.ToArray())
            mini.OnLeaderDied();

        _followers.Clear();
        Debug.Log("Player died.");
        // hook up game-over / respawn logic here
    }
}
