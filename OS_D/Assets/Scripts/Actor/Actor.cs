using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;


public class Actor : MonoBehaviour, IDamageTaker
{
    [Header("Actor stats")]
    [SerializeField] int healthPoints = 100;
    [SerializeField] int physicalResistance = 10;
    [SerializeField] int magicalResistance = 10;

    public void TakeDamage(DamageInfo damage)
    {
        int netDamage = ConvertDamage(damage);
        healthPoints = healthPoints - netDamage;
        Debug.Log(healthPoints);
        if (healthPoints <= 0)
        {
            OnActorDeath();
        }
    }

    public int ConvertDamage(DamageInfo damage)
    {
        int netDamage = damage.damageValue;
        if (damage.damageType == DamageType.Physical)
        {
            netDamage = ConvertPhysicalDamage(netDamage);
        }
        else if (damage.damageType == DamageType.Magical)
        {
            netDamage = ConvertMagicalDamage(netDamage);
        }
        else if (damage.damageType == DamageType.Mixed)
        {
            netDamage = ConvertMagicalDamage(netDamage / 2) + ConvertPhysicalDamage(netDamage / 2);
        }
        return netDamage + ConvertMagicalDamage(damage.addMagicalDamage) + ConvertPhysicalDamage(damage.addPhysicalDamage);
    }

    private int ConvertPhysicalDamage(int damage)
    {
        return (int)(damage * (1 / (1 + physicalResistance / 50f)));
    }
    private int ConvertMagicalDamage(int damage)
    {
        return (int)(damage * (1 / (1 + magicalResistance / 50f)));
    }

    protected virtual void OnActorDeath()
    {

    }
}
