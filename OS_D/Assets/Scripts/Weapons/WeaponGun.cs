using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class WeaponGun : WeaponBase
{
    [Header("Weapon projectile")]
    public GameObject projectilePrefab;
    protected override void Attack()
    {
        GameObject projectile = Instantiate(projectilePrefab, shootMark.transform.position, Quaternion.identity);
        projectile.GetComponent<ProjectileBase>().direction = direction.normalized;
        projectile.GetComponent<ProjectileBase>().owner = owner;
    }
}
