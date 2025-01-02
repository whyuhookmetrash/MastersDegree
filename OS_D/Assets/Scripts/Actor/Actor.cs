using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Actor : MonoBehaviour, IDamageTaker
{
    [Header("Actor stats")]
    [SerializeField] int maxHP = 100;
    public int currentHP;
    [SerializeField] int physicalResistance = 10;
    [SerializeField] int magicalResistance = 10;
    [SerializeField] int evasion = 0;
    [SerializeField] public float moveSpeed = 3f;
    public DamageInfo currentDamageInfo = new DamageInfo();
    [SerializeField] public int countThroughShoot = 1;
    //Modifiers

    protected virtual void Start()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(DamageInfo damage)
    {
        int netDamage = CalculateDamage(damage);
        currentHP = currentHP - netDamage;
        Debug.Log(currentHP);
        if (currentHP <= 0)
        {
            OnDeath();
        }
    }

    private int CalculateDamage(DamageInfo damage)
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

    protected virtual void OnDeath()
    {

    }
}
