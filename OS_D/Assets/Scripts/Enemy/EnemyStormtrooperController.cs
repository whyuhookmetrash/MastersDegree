using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : EnemyBaseController
{
    public override Vector2 GetNewStrafePosition()
    {
        Debug.Log("Определяем новую позицию");
        return playerPosition;
    }

    public override void Attack()
    {
        Debug.Log("Attack");
    }
}
