
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Actor : MonoBehaviour, IDamageTaker
{
    [Header("Actor stats")]
    [SerializeField] public int maxHP = 100;
    [SerializeField] public int currentHP = 100;
    [SerializeField] int physicalResistance = 10;
    [SerializeField] int magicalResistance = 10;
    [SerializeField] int evasion = 0;
    [SerializeField] public float _moveSpeed = 3f;
    public DamageInfo currentDamageInfo = new DamageInfo();
    [SerializeField] public int countThroughShoot = 1;
    private List<IModifier> modifiers = new List<IModifier>();
    public int b_freezeSpeed = 0;
    private Vector3 vector;

    public float moveSpeed
    {
        get { return _moveSpeed * Mathf.Max(0, 1 - b_freezeSpeed); }
        set { _moveSpeed = value; }
    }

    protected virtual void Start()
    {
    }

    public void TakeDamage(DamageInfo damage)
    {
        int netDamage = CalculateDamage(damage);
        TakeHP(-netDamage);
    }

    public void TakeHP(int healthPoints)
    {
        currentHP = Mathf.Min(currentHP + healthPoints, maxHP);
        if (currentHP <= 0) { OnDeath(); }
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

    public void SetModifier(IModifier modifier, bool isTemporary = false, float actionTime = 0f)
    {
        modifiers.Add(modifier);
        modifier.SetModifier(this);
        if (isTemporary)
        {
            StartCoroutine(StartModifierCooldown(actionTime, modifier));
        }
    }

    public void RemoveModifier(IModifier modifier)
    {
        if (modifiers.Contains(modifier))
        {
            modifiers.Remove(modifier);
            modifier.RemoveModifier(this);
        }
    }

    private IEnumerator StartModifierCooldown(float seconds, IModifier modifier)
    {
        yield return new WaitForSeconds(seconds);
        RemoveModifier(modifier);
    }
}
