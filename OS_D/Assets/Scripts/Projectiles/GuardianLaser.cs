using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardianLaser : ProjectileBase
{
    [Header("Laser stats")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float maxDistance = 25f;

    public Vector2 direction;
    private float distance = 0f;
 
    protected override void ChildStart()
    {
        float angle = Vector2.SignedAngle(Vector2.right, direction.normalized);
        transform.Rotate(0, 0, angle);
    }

    void FixedUpdate()
    {
        Vector2 pos = transform.position;
        pos = pos + direction * moveSpeed * Time.deltaTime;
        transform.position = pos;
        distance += moveSpeed * Time.deltaTime;
        if (distance > maxDistance)
        {
            OnSelfDestroy();
        }
    }

}
