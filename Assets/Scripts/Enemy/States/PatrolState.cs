using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : BaseState
{

    public int waypointIndex;
    public float waitTimer;
    
    private float moveTimer;
    public override void Enter()
    {
        
    }
    
    public override void Perform()
    {
        PatrolCycle();
        if (enemy.CanSeePlayer())
        {
            stateMachine.ChangeState(new AttackState());
        }
    }
    
    public override void Exit()
    {
        
    }

    public void PatrolCycle()
    {
        moveTimer += Time.deltaTime;
        if (moveTimer > Random.Range(3, 5))
        {
            // randomly move enemy while attacking
            enemy.Agent().SetDestination(enemy.transform.position + (Random.insideUnitSphere * 10));
            moveTimer = 0;
        }
        
        /*
        if (enemy.Agent().remainingDistance < 0.2f)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer > 3)
            {
                if (waypointIndex < enemy.path.waypoints.Count - 1)
                {
                    waypointIndex++;
                }
                else
                {
                    waypointIndex = 0;
                }

                enemy.Agent().SetDestination(enemy.path.waypoints[waypointIndex].position);
                waitTimer = 0;
            }
        }
        */
    }
}
