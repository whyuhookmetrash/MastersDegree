using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor
{
    public GameObject equipedWeapon = null;
    private Rigidbody2D rb;
    private Vector2 direction;
    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        if (equipedWeapon != null)
        {
            OnWeaponEquip(equipedWeapon);
        }
    }

    void Update() 
    {
        direction.x = Input.GetAxis("Horizontal");
        direction.y = Input.GetAxis("Vertical");
        if (direction.magnitude > 1) { direction.Normalize(); }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
    }

    private void OnWeaponEquip(GameObject weapon)
    {
        GameObject newWeapon = Instantiate(weapon, transform.position, Quaternion.identity);
        newWeapon.gameObject.GetComponent<WeaponBase>().OnWeaponEquip(this);
        equipedWeapon = newWeapon;
    }
}
