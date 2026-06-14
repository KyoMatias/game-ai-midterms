using UnityEngine;

public interface ILeader
{
    Transform Transform { get; }
    int FollowerCount { get; }
    void RegisterFollower(EnemyMovement mini);
    void UnregisterFollower(EnemyMovement mini);
    void Die();
}
