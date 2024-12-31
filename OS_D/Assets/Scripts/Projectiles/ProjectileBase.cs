using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    public Actor owner;
    private DamageInfo projectileDamage;

    void Start()
    {
        projectileDamage = owner.currentDamageInfo;
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
        }
        OnSelfDestroy();
    }
    protected virtual void OnSelfDestroy()
    {
        Destroy(gameObject);
    }

}
