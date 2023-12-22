using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownRangeEnemyController : TopDownEnemyController
{
    [SerializeField] private float followRange = 15f;
    [SerializeField] private float shootRange = 10f;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        float distance = DistanceToTarget();
        Vector2 direction = DirectionToTarget();

        IsAttacking = false;
        if(distance<=followRange)
        {
            if(distance<=shootRange)
            {
                int layerMaskTarget = Stats.CurrentStats.attackSO.target;
                //Level 레이어랑 layerMaskTarget(Player) 레이어 만나면 hit 반환
                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 11f, (1 << LayerMask.NameToLayer("Level"))|layerMaskTarget);

                if(hit.collider!=null && layerMaskTarget ==(layerMaskTarget |(1<<hit.collider.gameObject.layer)))
                {
                    CallLookEvent(direction);
                    CallMoveEvent(Vector2.zero);
                    IsAttacking = true;
                }
                else
                {
                    CallLookEvent(direction);
                    CallMoveEvent(direction);
                }
             }
            else
            {
                CallMoveEvent(direction);
            }
        }
        else
        {
            CallMoveEvent(direction);
        }
    }
}
