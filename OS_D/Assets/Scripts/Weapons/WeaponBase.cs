using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    public Player owner = null;
    [Header("Weapon stats")]
    public WeaponType weaponType = WeaponType.RANGE;
    public float attackCooldownTime = 1f;
    public float attackAnimationTime = 0.5f;
    private bool canAttack = true;
    public GameObject shootMark;
    protected Vector2 direction;
    protected List<IModifier> weaponModifiers = new List<IModifier>();

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {

    }
    
    void Update()
    {
        if (owner != null)
        {
            transform.position = owner.transform.position;
            RotateWeapon();
            if (canAttack && Input.GetMouseButton(0))
            {
                Attack();
                if (weaponType == WeaponType.MELEE)
                {
                    owner.SetModifier(new Modifiers.FreezeSpeed(), true, attackAnimationTime);
                }
                StartCoroutine(StartAttackCooldown());
            }
        }
    }

    protected virtual void Attack()
    {

    }

    private IEnumerator StartAttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldownTime);
        canAttack = true;
    }

    private void RotateWeapon()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = mousePosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    public void OnWeaponEquip(Player player)
    {
        owner = player;
        foreach (var mod in weaponModifiers)
        {
            owner.SetModifier(mod);
        }
    }

    public void OnWeaponRemove()
    {
        foreach (var mod in weaponModifiers)
        {
            owner.RemoveModifier(mod);
        }
        owner = null;
    }
}
