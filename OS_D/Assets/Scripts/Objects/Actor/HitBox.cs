using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour, IDamageTaker
{
    IDamageTaker owner;
    
    void Start()
    {
        owner = transform.parent?.gameObject.GetComponent<IDamageTaker>();
    }

    public void TakeDamage(DamageInfo damageInfo)
    {
        owner.TakeDamage(damageInfo);
    }
}
