using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour, ILeader
{
    public enum EnemyState
    {
        IDLE,
        PATROLING,
        DETECTING,
        FOLLOWING,
        EVADING,
    };

    public enum EnemyRole
    {
        MASTER,
        MINI,
    };

    [Header("Enemy State")]
    public EnemyState P_EnemyState;

    [Header("Enemy Parameters")]
    [SerializeField]
    private float _roamRadius = 20f;

    [SerializeField]
    private float _minWait = 1f;

    [SerializeField]
    private float _maxWait = 4f;

    [SerializeField]
    private float _currentWaitTime;

    [Header("Detection")]
    [SerializeField]
    private float _detectionRadius = 10f;

    [SerializeField]
    private float _timeToDetect = 2f;

    [SerializeField]
    private LayerMask _playerLayer;

    [SerializeField]
    private LayerMask _masterLayer;

    [SerializeField]
    private LayerMask _leaderLayer; // for minis: combined Player + EnemyMaster layers

    [Header("Role")]
    [SerializeField]
    private EnemyRole _role = EnemyRole.MASTER;

    [SerializeField]
    private Transform _target;

    private ILeader _leaderTarget; // for minis: the master or player they're following

    private NavMeshAgent _agent;
    private float _timer;

    [SerializeField]
    private float _detectionStartDelay = 0.5f;
    private float _spawnTime;

    [Header("Detection Visuals")]
    [SerializeField]
    private Renderer _renderer;

    [SerializeField]
    private Material _detectedMaterial;
    private Material _originalMaterial;

    [Header("Leader / Followers")]
    private List<EnemyMovement> _followers = new List<EnemyMovement>();

    public Transform Transform => transform;
    public int FollowerCount => _followers.Count;

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();

        if (_renderer == null)
            _renderer = GetComponentInChildren<Renderer>();

        if (_renderer != null)
            _originalMaterial = _renderer.material;
    }

    void Start()
    {
        _spawnTime = Time.time;
        OnStateEnter(P_EnemyState);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _roamRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _detectionRadius);
    }

    void Update()
    {
        bool detectionEnabled = Time.time - _spawnTime >= _detectionStartDelay;

        // Detection check runs regardless of state (except while already following/evading)
        if (
            detectionEnabled
            && P_EnemyState != EnemyState.FOLLOWING
            && P_EnemyState != EnemyState.EVADING
        )
        {
            if (CanSeeTarget())
            {
                if (P_EnemyState != EnemyState.DETECTING)
                    SetState(EnemyState.DETECTING);
            }
            else
            {
                if (P_EnemyState == EnemyState.DETECTING)
                    SetState(EnemyState.IDLE);
            }
        }

        switch (P_EnemyState)
        {
            case EnemyState.IDLE:
                TickIdle();
                break;
            case EnemyState.PATROLING:
                TickPatrol();
                break;
            case EnemyState.DETECTING:
                TickDetecting();
                break;
            case EnemyState.FOLLOWING:
                TickFollowing();
                break;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        ILeader other = collision.collider.GetComponentInParent<ILeader>();
        if (other == null || (object)other == (object)this)
            return;

        ResolveLeaderCollision(this, other);
    }

    static void ResolveLeaderCollision(ILeader a, ILeader b)
    {
        if (a.FollowerCount < b.FollowerCount)
            a.Die();
        else if (b.FollowerCount < a.FollowerCount)
            b.Die();
        // equal counts: no-op (define a tiebreaker here if desired)
    }

    public void SetState(EnemyState P_State)
    {
        if (P_EnemyState == P_State)
            return;

        P_EnemyState = P_State;
        OnStateEnter(P_State);
    }

    void OnStateEnter(EnemyState p_state)
    {
        switch (p_state)
        {
            case EnemyState.IDLE:
                if (_agent.isOnNavMesh)
                    _agent.ResetPath();
                _timer = 0f;
                _currentWaitTime = Random.Range(_minWait, _maxWait);
                EventManager.RaiseOnDetection(0f, false);
                SetDetectedVisual(false);

                if (_role == EnemyRole.MINI && _leaderTarget != null)
                {
                    _leaderTarget.UnregisterFollower(this);
                    _leaderTarget = null;
                }
                break;

            case EnemyState.PATROLING:
                SetNewRoam();
                SetDetectedVisual(false);

                if (_role == EnemyRole.MINI && _leaderTarget != null)
                {
                    _leaderTarget.UnregisterFollower(this);
                    _leaderTarget = null;
                }
                break;

            case EnemyState.DETECTING:
                if (_agent.isOnNavMesh)
                    _agent.ResetPath();
                _timer = 0f;
                EventManager.RaiseOnDetection(0f, true);
                SetDetectedVisual(true);
                break;

            case EnemyState.FOLLOWING:
                EventManager.RaiseOnDetection(1f, true);
                SetDetectedVisual(true);

                if (_role == EnemyRole.MINI && _leaderTarget != null)
                    _leaderTarget.RegisterFollower(this);
                break;
        }
    }

    void SetDetectedVisual(bool detected)
    {
        if (_renderer == null)
            return;

        if (detected && _detectedMaterial != null)
            _renderer.material = _detectedMaterial;
        else if (!detected)
            _renderer.material = _originalMaterial;
    }

    void TickIdle()
    {
        _timer += Time.deltaTime;

        if (_timer >= _currentWaitTime)
            SetState(EnemyState.PATROLING);
    }

    void TickPatrol()
    {
        if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
        {
            SetState(EnemyState.IDLE);
        }
    }

    void TickDetecting()
    {
        _timer += Time.deltaTime;
        float progress = Mathf.Clamp01(_timer / _timeToDetect);
        EventManager.RaiseOnDetection(progress, true);

        if (_timer >= _timeToDetect)
            SetState(EnemyState.FOLLOWING);
    }

    void TickFollowing()
    {
        if (_target == null)
        {
            SetState(EnemyState.IDLE);
            return;
        }

        _agent.SetDestination(_target.position);
    }

    bool CanSeeTarget()
    {
        LayerMask searchLayer = (_role == EnemyRole.MASTER) ? _playerLayer : _leaderLayer;

        Collider[] hits = Physics.OverlapSphere(transform.position, _detectionRadius, searchLayer);

        if (hits.Length == 0)
            return false;

        Transform nearest = hits[0].transform;
        float nearestDist = Vector3.SqrMagnitude(nearest.position - transform.position);

        for (int i = 1; i < hits.Length; i++)
        {
            float dist = Vector3.SqrMagnitude(hits[i].transform.position - transform.position);
            if (dist < nearestDist)
            {
                nearest = hits[i].transform;
                nearestDist = dist;
            }
        }

        if (_role == EnemyRole.MASTER)
        {
            _target = nearest;
        }
        else
        {
            ILeader leader = nearest.GetComponentInParent<ILeader>();
            if (leader == null)
                return false;

            _leaderTarget = leader;
            _target = nearest;
        }

        return true;
    }

    void SetNewRoam()
    {
        Vector3 rndDestination = transform.position + Random.insideUnitSphere * _roamRadius;
        rndDestination.y = transform.position.y;

        if (
            NavMesh.SamplePosition(
                rndDestination,
                out NavMeshHit hit,
                _roamRadius,
                NavMesh.AllAreas
            )
        )
            _agent.SetDestination(hit.position);
    }

    // --- ILeader implementation ---

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
        gameObject.SetActive(false);
    }

    public void OnLeaderDied()
    {
        _leaderTarget = null;
        _target = null;
        SetState(EnemyState.IDLE);
    }
}
