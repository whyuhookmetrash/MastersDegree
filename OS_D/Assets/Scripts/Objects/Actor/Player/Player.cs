using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor
{

    private Rigidbody2D rigidBody;

    private void Awake()
    {
        this.rigidBody = GetComponent<Rigidbody2D>();
    }
    protected override void Start()
    {
        base.Start();

    }

    public void Move(Vector2 direction)
    {
        this.rigidBody.MovePosition(this.rigidBody.position + direction * moveSpeed * Time.fixedDeltaTime); //TODO: Time.fixedDeltaTime разобраться
    }

}
