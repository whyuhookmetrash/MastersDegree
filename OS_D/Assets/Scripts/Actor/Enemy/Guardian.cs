using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guardian : EnemyRange
{
    [Header("Projectiles")]
    public GameObject projectilePrefab;
    protected override void Attack()
    {
        targetAttackPosition = playerPosition;
        GameObject laser = Instantiate(projectilePrefab, selfPosition + seeDirection * 0.25f, Quaternion.identity);
        laser.GetComponent<ProjectileBase>().direction = (targetAttackPosition - selfPosition).normalized;
        laser.GetComponent<ProjectileBase>().owner = this;
    }
}
