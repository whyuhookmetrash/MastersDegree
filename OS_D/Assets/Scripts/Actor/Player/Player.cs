using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor
{
    [Header("Player stats")]
    [SerializeField] float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 direction;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        direction.x = Input.GetAxis("Horizontal");
        direction.y = Input.GetAxis("Vertical");
        if (direction.magnitude > 1)
        {
            direction.Normalize();
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
    }

    protected override void OnActorDeath()
    {
        //Debug.Log("PlayerDeath");
    }
}
