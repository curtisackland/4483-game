using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : Enemy
{
    protected float searchTimer;
    protected float moveTimer;

    public float attackDistance = 2;
    public float followDistance = 1;
    public float attackRate = 1;
    protected float lastAttackTimer;

    public override void DoAttackState()
    {
        if (!CanSeePlayer())
        {
            stateMachine.ChangeState(new AttackState());
        }
        else
        {
            Agent().SetDestination((transform.position - Player().transform.position).normalized * followDistance + Player().transform.position);
        }

        lastAttackTimer += Time.deltaTime;
        if ((transform.position - Player().transform.position).magnitude < attackDistance && lastAttackTimer > attackRate)
        {
            Player().GetComponent<PlayerHealth>().TakeDamage(10);
            lastAttackTimer = 0;
        }
    }
    
    public override void DoPatrolState()
    {
        if (CanSeePlayer())
        {
            stateMachine.ChangeState(new AttackState());
        }

        moveTimer += Time.deltaTime;
        if (moveTimer > Random.Range(3, 5))
        {
            // randomly move enemy while attacking
            Agent().SetDestination(transform.position + (Random.insideUnitSphere * 10));
            moveTimer = 0;
        }
    }

    public override void DoSearchState()
    {
        if (CanSeePlayer())
        {
            stateMachine.ChangeState(new AttackState());
        }

        if (Agent().remainingDistance < Agent().stoppingDistance)
        {
            searchTimer += Time.deltaTime;
            moveTimer += Time.deltaTime;
            if (moveTimer > Random.Range(3, 5))
            {
                // randomly move enemy while attacking
                Agent().SetDestination(transform.position + (Random.insideUnitSphere * 10));
                moveTimer = 0;
            }
            
            if (searchTimer > 10)
            {
                stateMachine.ChangeState(new PatrolState());
            }
        }
    }
}
