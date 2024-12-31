using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : Actor
{
    [Header("Enemy stats")]
    [SerializeField] int damageValue = 0;
    [SerializeField] DamageType damageType = DamageType.Net;
    [Header("Area sizes")]
    [SerializeField] float detectRange = 5f;
    [SerializeField] float attackRange = 5f;
    [SerializeField] float huntingRange = 10f;
    [SerializeField] float helpRange = 5f;
    [Header("Timings")]
    [SerializeField] float prepairToAttackTime = 0.5f;
    [SerializeField] float attackAnimationTime = 0f;
    [SerializeField] float minCooldownAttackTime = 2f;
    [Header("Ray Mask")]
    public LayerMask rayTargetLayer;
    public LayerMask obstacleTargetLayer;

    private Rigidbody2D rb;
    private Vector2 direction = Vector2.zero;
    protected Vector2 seeDirection = Vector2.right; // зарандомить
    protected Vector2 playerPosition;
    protected Vector2 selfPosition;
    private Vector2 spawnPosition;
    private Vector2 goPosition;

    protected float toPlayerDistance = 0f;
    public bool detectPlayer = false;
    private bool seePlayer = false;
    public enum enemyState { Idle, PrepairToAttack, Attack, Strafe, Hunting, BackToSpawn }
    private enemyState prevEnemyState = enemyState.Idle;
    private enemyState _curEnemyState = enemyState.Idle;
    public enemyState curEnemyState
    {
        get {return _curEnemyState;}
        set
        {
            prevEnemyState = _curEnemyState;
            _curEnemyState = value;
        }
    }
    private bool isSeriesEvent = false;
    private bool goPositionEvent = false;
    private bool isAgent = false;
    protected Vector2 targetAttackPosition;
    private bool canAttack = true;

    private Vector2 target;
    NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = moveSpeed;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spawnPosition = transform.position;
        currentDamageInfo.damageValue = damageValue;
        currentDamageInfo.damageType = damageType;
        ChildStart();
    }

    protected virtual void ChildStart()
    {

    }

    void Update()
    {
        
    }
    // объект двигают сразу 2 класса: agent и rigidbody. При задействии одного, другой нужно отключать
    private void FixedUpdate()
    {
        playerPosition = GameManager.Instance.playerTransform.position;
        selfPosition = transform.position;
        toPlayerDistance = Vector2.Distance(selfPosition, playerPosition);

        RaycastHit2D raycast = Physics2D.Raycast(selfPosition, playerPosition - selfPosition, huntingRange, rayTargetLayer);
        OnCollisionRaycast(raycast);

        isAgent = false;
        if (!isSeriesEvent)
        {
            DefineEnemyState();
            OnEnemyState();
        }
        if (!isAgent)
        {
            float step = moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + direction * step);
            //rb.velocity = Vector2.zero; //вместо того который в Strafe
            if (goPositionEvent)
            {
                direction = (goPosition - selfPosition).normalized;
                if (Vector2.Distance(selfPosition, goPosition) <= step)
                {
                    selfPosition = goPosition;
                    goPositionEvent = false;
                    isSeriesEvent = false;
                    direction = Vector2.zero;
                }
                else
                {
                    OnFrontObstacle();
                }
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
            SetAgentPosition();
        }

        if (direction != Vector2.zero)
        {
            seeDirection = direction.normalized;
        }
        if (curEnemyState == enemyState.PrepairToAttack)
        {
            seeDirection = (playerPosition - selfPosition).normalized;
        }

        // Debug
        //Debug.DrawRay(selfPosition, (playerPosition - selfPosition).normalized * huntingRange, Color.yellow);
        //Debug.DrawRay(selfPosition, (playerPosition - selfPosition).normalized * detectRange, Color.red);
        //Debug.DrawRay(selfPosition + (goPosition - selfPosition).normalized * 0.3f, (goPosition - selfPosition).normalized * 0.3f, Color.blue);
    }

    private void SetAgentPosition()
    {
        agent.SetDestination(new Vector3(target.x, target.y, transform.position.z));
    }

    private void OnCollisionRaycast(RaycastHit2D raycast)
    {
        if (raycast)
        {
            if (raycast.collider.gameObject.CompareTag("Player"))
            {
                if (toPlayerDistance <= detectRange && !detectPlayer)
                {
                    detectPlayer = true;
                    CallForHelp();
                }
                seePlayer = true;
            }
            else
            {
                if (detectPlayer && toPlayerDistance > huntingRange)
                {
                    detectPlayer = false;
                }
                seePlayer = false;
            }
        } 
        else
        {
            detectPlayer = false;
            seePlayer = false;
        }
    }
    private void CallForHelp()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(enemy.transform.position, selfPosition);
            if (distance <= helpRange)
            {
                enemy.GetComponent<EnemyBase>().detectPlayer = true;
            }
        }
    }

    private void OnFrontObstacle()
    {
        RaycastHit2D obstacle = Physics2D.Raycast(selfPosition + 0.26f * direction, direction, 0.1f, obstacleTargetLayer);
        if (obstacle)
        {
            goPositionEvent = false;
            isSeriesEvent = false;
            direction = Vector2.zero;
        }
    }
    private void DefineEnemyState()
    {
        prevEnemyState = curEnemyState;
        if (detectPlayer)
        {
            if (!seePlayer | toPlayerDistance > attackRange)
            {
                curEnemyState = enemyState.Hunting;
            }
            else
            {
                curEnemyState = enemyState.PrepairToAttack;
            }
        }
        else
        {
            if (Vector2.Distance(selfPosition, spawnPosition) > moveSpeed * Time.deltaTime)
            {
                curEnemyState = enemyState.BackToSpawn;
            }
            else
            {
                selfPosition = spawnPosition;
                curEnemyState = enemyState.Idle;
            }
        }
    }

    private void OnEnemyState()
    {
        if (curEnemyState == enemyState.Idle)
        {
            OnPreviousPathFinding();
            Idle();
        }
        if (curEnemyState == enemyState.Hunting)
        {
            Hunting();
        }
        if (curEnemyState == enemyState.BackToSpawn)
        {
            BackToSpawn();
        }
        if (curEnemyState == enemyState.PrepairToAttack)
        {
            OnPreviousPathFinding();
            if (canAttack)
            {
                StartCoroutine(PrepairToAttack());
                if (minCooldownAttackTime > 0)
                {
                    StartCoroutine(StartAttackCooldown());
                }
            }
            else
            {
                isSeriesEvent = true;
                direction = Vector2.zero;
                curEnemyState = enemyState.Strafe;
                Strafe();
            }
        }
    }

    private void OnPreviousPathFinding()
    {
        if (prevEnemyState == enemyState.Hunting | prevEnemyState == enemyState.BackToSpawn)
        {
            target = selfPosition;
            SetAgentPosition();
            agent.isStopped = true;
        }
    }

    private IEnumerator PrepairToAttack()
    {
        isSeriesEvent = true;
        direction = Vector2.zero;
        yield return new WaitForSeconds(prepairToAttackTime);
        curEnemyState = enemyState.Attack;
        Attack();
        yield return new WaitForSeconds(attackAnimationTime);
        if (seePlayer && toPlayerDistance <= attackRange)
        {
            curEnemyState = enemyState.Strafe;
            Strafe();
        }
        else
        {
            isSeriesEvent = false;
        }
    }
    private IEnumerator StartAttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(minCooldownAttackTime);
        canAttack = true;
    }
    protected virtual void Attack()
    {

    }

    private void Strafe()
    {
        goPositionEvent = true;
        rb.velocity = Vector2.zero;
        goPosition = GetNewStrafePosition();
        direction = (goPosition - selfPosition).normalized;
    }

    protected virtual Vector2 GetNewStrafePosition()
    {
        return selfPosition;
    }

    private void Idle()
    {
        direction = Vector2.zero;
    }

    private void Hunting()
    {
        isAgent = true;
        agent.isStopped = false;
        target = playerPosition;
    }

    private void BackToSpawn()
    {
        isAgent = true;
        agent.isStopped = false;
        target = spawnPosition;
    }
}
