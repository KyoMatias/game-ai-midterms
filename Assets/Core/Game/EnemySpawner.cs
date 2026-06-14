using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField]
    private EnemyMovement _masterPrefab;

    [SerializeField]
    private EnemyMovement _miniPrefab;

    [Header("Pool Sizes")]
    [SerializeField]
    private int _masterCount = 2;

    [SerializeField]
    private int _miniCount = 6;

    [System.Serializable]
    public class SpawnPoint
    {
        public Transform point;
        public float radius = 10f;
    }

    [Header("Spawn Area")]
    [SerializeField]
    private SpawnPoint[] _spawnPoints;

    [SerializeField]
    private float _defaultSpawnRadius = 30f;

    private List<EnemyMovement> _masterPool = new List<EnemyMovement>();
    private List<EnemyMovement> _miniPool = new List<EnemyMovement>();

    void Awake()
    {
        BuildPool(_masterPrefab, _masterCount, _masterPool);
        BuildPool(_miniPrefab, _miniCount, _miniPool);
    }

    void Start()
    {
        SpawnAll(_masterPool);
        SpawnAll(_miniPool);
    }

    void BuildPool(EnemyMovement prefab, int count, List<EnemyMovement> pool)
    {
        if (prefab == null)
        {
            Debug.LogWarning($"{name}: prefab not assigned, skipping pool of size {count}.");
            return;
        }

        for (int i = 0; i < count; i++)
        {
            EnemyMovement instance = Instantiate(prefab, transform);
            instance.gameObject.SetActive(false);
            pool.Add(instance);
        }
    }

    void SpawnAll(List<EnemyMovement> pool)
    {
        foreach (EnemyMovement enemy in pool)
        {
            Vector3 spawnPos = GetSpawnPosition();
            enemy.transform.position = spawnPos;
            enemy.gameObject.SetActive(true);

            NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
            if (agent != null)
                agent.Warp(spawnPos);
        }
    }

    Vector3 GetSpawnPosition()
    {
        Vector3 basePos;
        float radius;

        if (_spawnPoints != null && _spawnPoints.Length > 0)
        {
            SpawnPoint chosen = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
            basePos = chosen.point.position;
            radius = chosen.radius;
        }
        else
        {
            basePos = transform.position;
            radius = _defaultSpawnRadius;
        }

        Vector3 randomOffset = Random.insideUnitSphere * radius;
        randomOffset.y = 0f;
        Vector3 candidate = basePos + randomOffset;

        if (NavMesh.SamplePosition(candidate, out NavMeshHit hit, 5f, NavMesh.AllAreas))
            return hit.position;

        Debug.LogWarning(
            $"{name}: SamplePosition failed for candidate {candidate}, using basePos."
        );
        return basePos;
    }

    public EnemyMovement GetMaster()
    {
        return GetFromPool(_masterPool);
    }

    public EnemyMovement GetMini()
    {
        return GetFromPool(_miniPool);
    }

    EnemyMovement GetFromPool(List<EnemyMovement> pool)
    {
        foreach (EnemyMovement enemy in pool)
        {
            if (!enemy.gameObject.activeSelf)
                return enemy;
        }
        return null;
    }

    public void ReturnToPool(EnemyMovement enemy)
    {
        enemy.gameObject.SetActive(false);
    }
}
