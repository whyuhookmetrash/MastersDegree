using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;

public class EnemyBaseController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float detectRange = 5f;
    [SerializeField] float attackRange = 5f;
    [SerializeField] float huntingRange = 10f;
    private Rigidbody2D rb;
    private Vector2 direction = Vector2.zero;
    private Vector2 playerPosition;
    private Vector2 selfPosition;
    private Vector2 spawnPosition;
    public LayerMask rayTargetLayer;

    private float toPlayerDistance = 0f;
    private bool detectPlayer = false;
    private bool seePlayer = false;
    public enum enemyState { Idle, PrepairToAttack, Attack, Strafe, Hunting, BackToSpawn }
    public enemyState curEnemyState = enemyState.Idle;
    private enemyState prevEnemyState = enemyState.Idle;
    private bool isSeriesEvent = false;
    private Vector2 targetAttackPosition;

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

        if (!isSeriesEvent)
        {
            DefineEnemyState();
            OnEnemyState();
        }


        rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);


        // Debug
        Debug.DrawRay(selfPosition, (playerPosition - selfPosition).normalized * huntingRange, Color.yellow);
        Debug.DrawRay(selfPosition, (playerPosition - selfPosition).normalized * detectRange, Color.red);
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
            if (!seePlayer | toPlayerDistance < attackRange)
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
            PrepairToAttack();
        }
    }

    private void PrepairToAttack()
    {
        isSeriesEvent = true; // Attack - Strafe - ?PrepairToAttack


        //Attack();
        //if (seePlayer && toPlayerDistance <= attackRange)
        //{
        //    Strafe();
        //}
        //else
        //{
        //    isSeriesEvent = false;
        //}
    }

    private void Attack()
    {
        ToAttack();
        if (seePlayer && toPlayerDistance <= attackRange)
        {
            Strafe();
        }
        else
        {
            isSeriesEvent = false;
        }
    }

    private void ToAttack()
    {
        Debug.Log("Attack");
    }

    private void Strafe()
    {

    }

    private void Idle()
    {
        direction = Vector2.zero;
    }

    private void Hunting()
    {

    }

    private void BackToSpawn()
    {

    }
}
