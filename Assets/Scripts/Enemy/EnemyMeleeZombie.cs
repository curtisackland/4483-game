using UnityEngine;

public class EnemyMeleeZombie : EnemyMelee
{
    public override void DoAttackState()
    {
        if (!CanSeePlayer())
        {
            losePlayerTimer += Time.deltaTime;
            if (losePlayerTimer > 8)
            {
                // Go back to search state
                stateMachine.ChangeState(new SearchState());
                SetWalkingSpeed(1);
                losePlayerTimer = 0;
            }
        }
        else
        {
            losePlayerTimer = 0;
            SetRunningSpeed(1);
            Agent().SetDestination((transform.position - Player().transform.position).normalized * followDistance + Player().transform.position);
            LastKnowPos = Player().transform.position;
            lastAttackTimer += Time.deltaTime;
            if (lastAttackTimer < attackRate)
            {
                animator.SetBool("isAttacking", false);
            }
            else if ((transform.position - Player().transform.position).magnitude < attackDistance)
            {
                // Check that the enemy can see the player
                if (Physics.Raycast(transform.position, Player().transform.position - transform.position, out var hit))
                {
                    if (hit.transform == Player().transform)
                    {
                        Player().GetComponent<PlayerHealth>().TakeDamage(10);
                        lastAttackTimer = 0;
                        animator.SetBool("isAttacking", true);
                    }
                }
            }
        }

        
        SetIsMovingAnimation();
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
            SetWalkingSpeed(1);
            if (Agent().isOnNavMesh)
            {
                Agent().SetDestination(transform.position + (Random.insideUnitSphere * 10));
            }
            moveTimer = 0;
        }
        SetIsMovingAnimation();
    }
    
    public override void DoSearchState()
    {
        if (CanSeePlayer())
        {
            stateMachine.ChangeState(new AttackState());
        }

        if (Agent().remainingDistance < Agent().stoppingDistance)
        {
            moveTimer += Time.deltaTime;
            if (moveTimer > Random.Range(3, 5))
            {
                SetWalkingSpeed(2);
                // randomly move enemy while attacking
                Agent().SetDestination(transform.position + (Random.insideUnitSphere * 10));
                moveTimer = 0;
            }
            
        }
        searchTimer += Time.deltaTime;
        if (searchTimer > 10)
        {
            stateMachine.ChangeState(new PatrolState());
        }
        SetIsMovingAnimation();
    }
    
    private void SetWalkingSpeed(float multiplier)
    {
        animator.SetBool("isRunning", false);
        animator.SetFloat("walkSpeedFactor", multiplier);
        Agent().speed = 0.266f * multiplier; // Based on animation
    }
    
    private void SetRunningSpeed(float multiplier)
    {
        animator.SetBool("isRunning", true);
        animator.SetFloat("walkSpeedFactor", multiplier);
        Agent().speed = 3.7f * multiplier; // Based on animation
    }


    private void SetIsMovingAnimation()
    {
        if (Agent().velocity.magnitude > 0.2f && Agent().remainingDistance > 0.5f)
        {
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }
}