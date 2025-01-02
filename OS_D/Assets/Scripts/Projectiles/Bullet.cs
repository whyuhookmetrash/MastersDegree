using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : ProjectileBase
{
    [Header("Bullet stats")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float maxDistance = 25f;

    private float distance = 0f;
    private Rigidbody2D rb;

    protected override void ChildStart()
    {
        float angle = Vector2.SignedAngle(Vector2.right, direction.normalized);
        transform.Rotate(0, 0, angle);
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
        //Vector2 pos = transform.position;
        //pos = pos + direction * moveSpeed * Time.fixedDeltaTime;
        //transform.position = pos;
        distance += moveSpeed * Time.fixedDeltaTime;
        if (distance > maxDistance)
        {
            OnSelfDestroy();
        }
    }
}
