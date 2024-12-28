using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStormtrooperController : EnemyBaseController
{
    [Header("Close Combat")]
    [SerializeField] int damage = 10;
    [SerializeField] float closeCombatRange = 2f;
    protected override void Attack()
    {
        if (toPlayerDistance <= closeCombatRange) {
            GameManager.Instance.DealDamageToPlayer(damage);
        }
    }
}
