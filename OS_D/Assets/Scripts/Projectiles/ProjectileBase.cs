using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    public Actor owner;
    private DamageInfo projectileDamage;

    public Vector2 direction;
    private int countThroughShoot = 1;

    void Start()
    {
        countThroughShoot = owner.countThroughShoot;
        projectileDamage = owner.currentDamageInfo;
        if (owner.gameObject.CompareTag("Player"))
        {
            gameObject.layer = LayerMask.NameToLayer("PlayerBreakableProjectiles");
        }
        else if (owner.gameObject.CompareTag("Enemy"))
        {
            gameObject.layer = LayerMask.NameToLayer("EnemyBreakableProjectiles");
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("BreakableProjectiles");
        }
        ChildStart();
    }

    protected virtual void ChildStart()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        IDamageTaker actor = other.gameObject.GetComponent<IDamageTaker>();
        if (actor != null)
        {
            actor.TakeDamage(projectileDamage);
            countThroughShoot--;
        }
        else
        {
            OnSelfDestroy();
        }
        if (countThroughShoot <= 0)
        {
            OnSelfDestroy();
        }
    }
    protected virtual void OnSelfDestroy()
    {
        Destroy(gameObject);
    }

}
