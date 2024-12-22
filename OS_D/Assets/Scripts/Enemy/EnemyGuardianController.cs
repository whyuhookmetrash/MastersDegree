using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGuardianController : EnemyBaseController
{
    [Header("Strafe area sizes")]
    [SerializeField] float maxStep = 2f;
    [SerializeField] float dangerousRange = 5f;
    [SerializeField] float strafeRange = 7f;
    [SerializeField] float safeRange = 10f;
    [Header("Projectiles")]
    public GameObject laserProjectile;

    private int prevK;

    protected override void ChildStart()
    {
        prevK = Random.Range(0, 2);
    }
    protected override void Attack()
    {
        targetAttackPosition = playerPosition;
        GameObject laser = Instantiate(laserProjectile, selfPosition + seeDirection * 0.3f, Quaternion.identity);
        laser.GetComponent<GuardianLaserController>().direction = (targetAttackPosition - selfPosition).normalized;
    }
    protected override Vector2 GetNewStrafePosition()
    {
        if (toPlayerDistance <= dangerousRange)
        {
            return OnDangerousZone();
        }
        else if (toPlayerDistance <= strafeRange)
        {
            return OnStrafeZone();
        }
        else
        {
            return selfPosition;
        }
    }
    private Vector2 OnStrafeZone()
    {
        Vector2 leftCirclePosition = playerPosition + RotateVector(selfPosition - playerPosition, -15f).normalized * toPlayerDistance;
        Vector2 rightCirclePosition = playerPosition + RotateVector(selfPosition - playerPosition, 15f).normalized * toPlayerDistance;
        int k = Random.Range(0, 2);

        if (Random.Range(0, 2) == 0) { k = prevK; }
        else { prevK = k; }

        if (k == 0)
        {
            if (!OnObstacle(leftCirclePosition))
            {
                return NormalizePosition(leftCirclePosition);
            }
            else if (!OnObstacle(rightCirclePosition))
            {
                return NormalizePosition(rightCirclePosition);
            }
            else
            {
                return selfPosition;
            }
        }
        else
        {
            if (!OnObstacle(rightCirclePosition))
            {
                return NormalizePosition(rightCirclePosition);
            }
            else
            {
                return selfPosition;
            }
        }
    }
    private Vector2 OnDangerousZone()
    {
        Vector2 rootCirclePosition = GetRootCirclePoint(playerPosition, selfPosition, dangerousRange);
        Vector2 circlePosition = rootCirclePosition;
        int k = 0;
        float angle = 15f;
        while (OnObstacle(circlePosition) && k < 13)
        {
            k += 1;
            circlePosition = playerPosition + RotateVector(selfPosition - playerPosition, angle).normalized * dangerousRange;
            angle = angle * -1;
            if (k % 2 == 0) { angle += 15; }
        }
        if (k != 13)
        {
            return NormalizePosition(circlePosition);
        }
        else
        {
            return selfPosition;
        }

    }

    private bool OnObstacle(Vector2 goPosition)
    {
        return Physics2D.Raycast(selfPosition, goPosition - selfPosition, Vector3.Distance(goPosition, selfPosition), rayTargetLayer);
    }
    private Vector2 NormalizePosition(Vector2 goPosition)
    {
        if (Vector3.Distance(goPosition, selfPosition) > maxStep)
        {
            Vector2 newGoPosition = selfPosition + (goPosition - selfPosition).normalized * maxStep;
            return newGoPosition;
        }
        else
        {
            return goPosition;
        }
    }

    private Vector2 GetRootCirclePoint(Vector2 A, Vector2 B, float R)
    {
        Vector2 unitAB = (B - A).normalized;
        Vector2 P = A + unitAB * R;
        return P;
    }

    private Vector2 RotateVector(Vector2 AB, float angle)
    {
        float radianAngle = angle * Mathf.Deg2Rad;
        float cos = Mathf.Cos(radianAngle);
        float sin = Mathf.Sin(radianAngle);
        float newX = AB.x * cos - AB.y * sin;
        float newY = AB.x * sin + AB.y * cos;
        return new Vector2(newX, newY);
    }
}
