using UnityEngine;

public class EnemyMeleeZombie : EnemyMelee
{
    public override void DoAttackState()
    {
        if (!CanSeePlayer())
        {
            stateMachine.ChangeState(new PatrolState());
        }
        else
        {
            animator.SetBool("isRunning", true);
            animator.SetFloat("walkSpeedFactor", 1f);
            Agent().speed = 3.7f; // Based on animation
            Agent().SetDestination((transform.position - Player().transform.position).normalized * followDistance + Player().transform.position);
        }

        lastAttackTimer += Time.deltaTime;
        if (lastAttackTimer < attackRate)
        {
            animator.SetBool("isAttacking", false);
        }
        else if ((transform.position - Player().transform.position).magnitude < attackDistance)
        {
            Player().GetComponent<PlayerHealth>().TakeDamage(10);
            lastAttackTimer = 0;
            animator.SetBool("isAttacking", true);
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
            animator.SetBool("isRunning", false);
            animator.SetFloat("walkSpeedFactor", 1f);
            Agent().speed = 0.266f; // Based on animation
            Agent().SetDestination(transform.position + (Random.insideUnitSphere * 10));
            moveTimer = 0;
        }
        SetIsMovingAnimation();
    }

    private void SetIsMovingAnimation()
    {
        if (Agent().velocity.magnitude > 0.2f)
        {
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }
}