using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSword : WeaponBase
{
    [Header("Weapon settings")]
    public GameObject closeCombatArea;
    private AreaTrigger closeCombatTrigger;

    protected override void Awake()
    {
        weaponModifiers.Add(new Modifiers.DamageValue(100));
        weaponModifiers.Add(new Modifiers.DamageType(DamageType.Physical));
    }
    protected override void Start()
    {
        base.Start();
        closeCombatTrigger = closeCombatArea.GetComponent<AreaTrigger>();
    }
    protected override void Attack()
    {
        foreach (GameObject trigger in closeCombatTrigger.GetTriggers())
        {
            IDamageTaker actor = trigger.GetComponent<IDamageTaker>();
            if (actor != null)
            {
                actor.TakeDamage(owner.currentDamageInfo);
            }
        }
    }

}
