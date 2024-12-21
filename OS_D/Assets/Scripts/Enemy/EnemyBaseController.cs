using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBaseController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float detectRange = 5f;
    [SerializeField] float attackRange = 5f;
    [SerializeField] float huntingRange = 10f;
    private float prepairToAttackTime = 0.5f;
    private float attackAnimationTime = 0f;
    private Rigidbody2D rb;
    private Vector2 direction = Vector2.zero;
    private Vector2 playerPosition;
    private Vector2 selfPosition;
    private Vector2 spawnPosition;
    private Vector2 goPosition;
    public LayerMask rayTargetLayer;

    private float toPlayerDistance = 0f;
    private bool detectPlayer = false;
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
    private Vector2 targetAttackPosition;

    //Agent
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
    }

    
    void Update()
    {
        
    }

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
            }
        }
        else
        {
            SetAgentPosition();
        }

        // Debug
        Debug.DrawRay(selfPosition, (playerPosition - selfPosition).normalized * huntingRange, Color.yellow);
        Debug.DrawRay(selfPosition, (playerPosition - selfPosition).normalized * detectRange, Color.red);
    }

    void SetAgentPosition()
    {
        agent.SetDestination(new Vector3(target.x, target.y, transform.position.z));
    }

    private void OnCollisionRaycast(RaycastHit2D raycast)
    {
        if (raycast)
        {
            if (raycast.collider.gameObject.CompareTag("Player"))
            {
                if (toPlayerDistance <= detectRange)
                {
                    detectPlayer = true;
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
            if (selfPosition != spawnPosition)
            {
                curEnemyState = enemyState.BackToSpawn;
            }
            else
            {
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
            StartCoroutine(PrepairToAttack());
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
    private void Attack()
    {
        Debug.Log("Attack");
    }

    private void Strafe()
    {
        goPositionEvent = true;
        goPosition = GetNewStrafePosition();
        direction = (goPosition - selfPosition).normalized;
    }

    private Vector2 GetNewStrafePosition()
    {
        Debug.Log("Определяем новую позицию");
        return playerPosition;
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
