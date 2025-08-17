using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class BaseEnemy : MonoBehaviour, IPoolable
{
    [Header("EnemyStat ScriptableObject")]
    [SerializeField] public EnemyStatData enemyStats;

    [Header("SFX")]
    public AudioClip idleClip;
    public AudioClip wanderClip;
    public AudioClip chaseClip;
    public AudioClip attackClip;
    public AudioClip danceClip;

    private NavMeshAgent agent;
    private Animator animator;
    private Player player;
    private Transform door;
    private StateFactory<BaseEnemy> stateFactory; // 상태들 캐싱

    public Animator GetAnimator() => animator;

    [Header("AniHashData")]
    public int Idle = Animator.StringToHash("Idle");
    public int Move = Animator.StringToHash("Move");
    public int Attack = Animator.StringToHash("Attack");
    public int AttackIdle = Animator.StringToHash("AttackIdle");
    public int Dance = Animator.StringToHash("Dance");

    public event Action<GameObject> returnToPool;

    public NavMeshAgent GetAgent() => agent;
    public StateMachine<BaseEnemy> StateMachine { get; private set; }
    public StateFactory<BaseEnemy> StateFactory => stateFactory;
    public int PrefabIndex { get; set; }

    private AudioSource audioSource;
    private Coroutine sfxLoopCoroutine;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
        stateFactory = new StateFactory<BaseEnemy>(this);
        StateMachine = new StateMachine<BaseEnemy>(this);
    }
    
    private void Start()
    {
        StateMachine.ChangeState(StateFactory.Get<EnemyIdleState>());
        //StateMachine.ChangeState(new EnemyIdleState(this));
        player = Player.Instance;
    }

    private void Update()
    {
        StateMachine.Update();
    }
    
    private void FixedUpdate()
    {
        StateMachine.FixedUpdate();
    }
        
    private void LateUpdate()
    {
        StateMachine.LateUpdate();
    }

    public void MoveToPlayer()
    {
        if (player == null) return;
        agent.speed = enemyStats.moveSpeed;

        if (IsPathValid(player.transform.position)) // 플레이어에게 도달할 수 있는 경로가 있으면 추적
        {
            agent.SetDestination(player.transform.position);
        }
        else // 플레이어에게 도달할 수 있는 경로가 없으면
        {
            Transform transform = FindDestination(enemyStats.detectRange);

            if (transform != null)
            {
                agent.SetDestination(transform.position);
            }
        }
    }

    public Transform FindDestination(float radius)
    {
        Collider[] hits = Physics.OverlapSphere(agent.transform.position, radius);

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<DoorObject>(out DoorObject obstacle))
            {
                door = obstacle.gameObject.transform;
                return obstacle.transform;
            }
        }

        return null;
    }

    public bool IsPathValid(Vector3 destination)
    {
        NavMeshPath path = new NavMeshPath();
        bool hasPath = agent.CalculatePath(destination, path);

        return hasPath && path.status == NavMeshPathStatus.PathComplete;
    }

    // 범위 내 플레이어 감지
    public bool IsPlayerInRage()
    {
        Collider[] results = new Collider[1];
        int count = Physics.OverlapSphereNonAlloc(transform.position, enemyStats.detectRange, results, LayerMask.GetMask("Player"));
        return count > 0;
    }

    // 범위 내 문 감지
    public bool IsDoorInRange()
    {
        Collider[] results = new Collider[5];
        int count = Physics.OverlapSphereNonAlloc(transform.position, enemyStats.detectRange, results);

        return count > 0;
    }

    //소리 감지
    public bool IsSensedSound()
    {
        float heard = DecibelManager.Instance.GetPercivedDecibel(transform.position);
        return heard > DecibelManager.Instance.perceptibleValue;
    }
    
    public void StopMove()
    {
        agent.speed = 0;
    }
    
    // 플레이어 공격 범위에 있는지 확인
    public bool IsAttackRange()
    {
        Vector3 origin = transform.position + Vector3.up * 1.6f;
        Vector3 direction = transform.forward;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, enemyStats.attackRange))
        {
            Debug.DrawRay(origin, direction * enemyStats.attackRange, Color.red);

            if (hit.collider.CompareTag("Player"))
            {
                return true;
            }
        }

        return false;
    }

    // 문이 공격 범위에 있는지 확인
    public bool IsDoorAttackRange()
    {
        Vector3 origin = transform.position + Vector3.up * 1.6f;
        Vector3 direction = transform.forward;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, enemyStats.attackRange))
        {
            Debug.DrawRay(origin, direction * enemyStats.attackRange, Color.blue);

            if (hit.collider.CompareTag("Door"))
            {
                return true;
            }
        }

        return false;
    }

    //범위 안에서 소리가 감지되는지 확인
    public bool IsSoundSensedRange()
    {
        if (IsSensedSound() && DecibelManager.Instance.distance < enemyStats.soundSensedRange)
        {
            return true;
        }
        
        return false;
    }
    
    // 애니메이션 이벤트 호출을 위한 공격 State의 함수 불러오기
    public void OnAttackAnimationEnd()
    {
        animator.SetBool(Attack, false);
        animator.SetBool(AttackIdle, true);

        if (StateMachine.CurrentState is EnemyAttackState attackState)
        {
            attackState.OnAttackAnimationEnd(); // 상태에 알려줌
        }
    }

    // 타겟 방향으로 부드럽게 회전
    public void RotateToTarget()
    {
        if (IsPlayerInRage() || IsSoundSensedRange())
        {
            agent.updateRotation = false;
            Vector3 direction = player.transform.position - transform.position;
            direction.y = 0f;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
        }
        
    }
    
    // 타겟 방향으로 즉시 회전
    public void FaceTargetInstantly()
    {
        if (player == null) return;

        Vector3 direction = player.transform.position - transform.position;
        direction.y = 0f;
        if (direction.sqrMagnitude > 0.01f)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
    
    // 목적지로 이동
    public void WanderToNewPosition()
    {
        if (IsPathValid(GetWanderPosition()))
        {
            agent.speed = enemyStats.moveSpeed;
            agent.updateRotation = true;
            agent.SetDestination(GetWanderPosition());
        }
        
    }

    // 랜덤으로 위치 받아서 목적지 설정.
    public Vector3 GetWanderPosition()
    {
        NavMeshHit hit;
        Vector3 randomDirection = UnityEngine.Random.onUnitSphere;

        for (int i = 0; i < 30; i++)
        {
            randomDirection = UnityEngine.Random.insideUnitSphere * 10f;
            randomDirection.y = 0f;
            randomDirection += transform.position;
            if (NavMesh.SamplePosition(randomDirection, out hit, 2f, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }
        
        return transform.position;
    }

    // 공격 이벤트 실행 (Ani Event)
    public void OnAttackDamage()
    {
        Vector3 origin = transform.position + Vector3.up * 1.6f;
        Vector3 direction = transform.forward;
        RaycastHit[] hits = Physics.RaycastAll(origin, direction, enemyStats.attackRange);

        Debug.DrawRay(origin, direction * enemyStats.attackRange, Color.green);

        foreach(var hit in hits)
        {
            // 문 우선 처리
            if (hit.collider.CompareTag("Door"))
            {
                if (hit.collider.TryGetComponent<DoorObject>(out var doorObject))
                {
                    doorObject.TakeDamage(1);
                    break;
                }
            }
            else if (hit.collider.CompareTag("Player"))
            {
                if (hit.collider.TryGetComponent<PlayerCondition>(out var playerCondition))
                {
                    playerCondition.GetPhysicsDamage(enemyStats.damage);
                    // 공격 시 감염도 증가 시작 
                    if (!playerCondition.isAttacked) // 이미 감염되었으면 실행 안되게
                    {
                        playerCondition.isAttacked = true;
                    }
                    break;
                }
            }

        }
    }

    // 플레이어가 Dance 범위 내에 있는지 확인
    public bool IsInfectedPlayerInDanceRange()
    {
        if (Player.Instance.GetComponent<PlayerCondition>().GetInfection() > 50)
        {
            Collider[] results = new Collider[1];
            int count = Physics.OverlapSphereNonAlloc(transform.position, enemyStats.danceRange, results, LayerMask.GetMask("Player"));
            return count > 0;
        }
        return false;
    }

    public void Initialize(System.Action<GameObject> returnAction)
    {
        returnToPool = returnAction;
    }

    public void OnSpawn()
    {
        StateMachine.ChangeState(StateFactory.Get<EnemyIdleState>());
    }

    public void OnDespawn()
    {
        StopAllCoroutines();
    }
    
    public void PlaySFX(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    public void StopSFX()
    {
        audioSource.Stop();
    }

    public void PlaySFXLoop(AudioClip clip, float duration)
    {
        if (sfxLoopCoroutine != null)
            StopCoroutine(sfxLoopCoroutine);

        sfxLoopCoroutine = StartCoroutine(SFXLoopRoutine(clip, duration));
    }

    public void StopSFXLoop()
    {
        if (sfxLoopCoroutine != null)
        {
            audioSource.Stop();
            StopCoroutine(sfxLoopCoroutine);
            sfxLoopCoroutine = null;
        }
    }

    IEnumerator SFXLoopRoutine(AudioClip clip, float duration)
    {
        while (true)
        {
            audioSource.PlayOneShot(clip);
            yield return new WaitForSeconds(duration);
        }
    }
}
