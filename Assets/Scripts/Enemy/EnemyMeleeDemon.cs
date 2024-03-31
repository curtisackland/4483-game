using UnityEngine;

public class EnemyMeleeDemon : EnemyMelee
{
    private float lastAttackEndTime;

    public float hitAttackDistance = 5f;
    public float hitAttackTime = 3f; // Time attack takes
    public float hitAttackCooldown = 3f; // Time before attack can be done again
    private float hitAttackLastTime;
    private float agentSpeed;
    private bool isAttacking = false;
    public GameObject homeArea;
    public float homeAreaRadius;
    
    public override void DoAttackState()
    {
        if (CanSeePlayer(false))
        {
            losePlayerTimer = 0;
            Agent().SetDestination((transform.position - Player().transform.position).normalized * followDistance + Player().transform.position);
            LastKnowPos = Player().transform.position;
            if (lastAttackEndTime < Time.time && !isAttacking)
            {
                if (hitAttackLastTime + hitAttackCooldown < Time.time && (transform.position - Player().transform.position).magnitude < hitAttackDistance)
                {
                    // Check that the enemy can see the player
                    if (Physics.Raycast(transform.position, Player().transform.position - transform.position, out var hit))
                    {
                        if (hit.transform == Player().transform)
                        {
                            Player().GetComponent<PlayerHealth>().TakeDamage(10);
                            animator.Play("Attack", 0);
                            
                            // Timing
                            hitAttackLastTime = Time.time;
                            lastAttackEndTime = Time.time + hitAttackTime;
                            isAttacking = true;
                            
                            // Save speed
                            agentSpeed = Agent().speed;
                            Agent().speed = 0;
                        }
                    }
                }
            }
        }
        else
        {
            losePlayerTimer += Time.deltaTime;
            if (losePlayerTimer > 8)
            {
                // Go back to search state
                stateMachine.ChangeState(new SearchState());
                losePlayerTimer = 0;
            }
        }

        if (isAttacking && lastAttackEndTime < Time.time)
        {
            Agent().speed = agentSpeed;
            isAttacking = false;
        }
    }

    public override void DoPatrolState()
    {
        if (CanSeePlayer(true))
        {
            stateMachine.ChangeState(new AttackState());
        }

        moveTimer += Time.deltaTime;
        if (moveTimer > Random.Range(5, 7))
        {
            // randomly move enemy while attacking
            if (Agent().isOnNavMesh)
            {
                if (homeArea == null)
                {
                    Agent().SetDestination(transform.position + (Random.insideUnitSphere * 10));
                }
                else
                {
                    Agent().SetDestination(homeArea.transform.position + (Random.insideUnitSphere * homeAreaRadius));
                }
            }
            moveTimer = 0;
        }
    }
    
    public override void DoSearchState()
    {
        if (CanSeePlayer(true))
        {
            stateMachine.ChangeState(new AttackState());
        }

        if (Agent().remainingDistance < Agent().stoppingDistance)
        {
            moveTimer += Time.deltaTime;
            if (moveTimer > Random.Range(3, 5))
            {
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
    }
}