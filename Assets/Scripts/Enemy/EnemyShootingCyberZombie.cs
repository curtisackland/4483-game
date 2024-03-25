using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootingCyberZombie : Enemy
{
    [Header("Weapon Values")]
    public Transform gunBarrel;

    [Range(0.1f, 10)]
    public float fireRate;
    public float lineupTime;
    
    protected float searchTimer;
    protected float moveTimer;
    
    protected float shotTimer;
    
    // Update is called once per frame
    public override void Update()
    {
        SetIsMovingAnimation();
    }
    
    public override void DoAttackState()
    {
        if (CanSeePlayer())
        {
            losePlayerTimer = 0;
            moveTimer += Time.deltaTime;
            shotTimer += Time.deltaTime;
            transform.LookAt(Player().transform);

            if (shotTimer > fireRate - lineupTime)
            {
                animator.SetLayerWeight(1, 1);
                animator.SetBool("isShooting", true);
                if (shotTimer > fireRate)
                {
                    Shoot();
                }
            }
            else
            {
                animator.SetBool("isShooting", false);
            }

            if (shotTimer > lineupTime) // allow weapon to be lowered before disabling upper body layer
            {
                animator.SetLayerWeight(1, 0);
            }
            

            if (moveTimer > Random.Range(3, 7))
            {
                // randomly move enemy while attacking
                Agent().SetDestination(transform.position + (Random.insideUnitSphere * 5));
                moveTimer = 0;
            }

            LastKnowPos = Player().transform.position;
        }
        else
        {
            losePlayerTimer += Time.deltaTime;
            if (losePlayerTimer > 8)
            {
                // Go back to search state
                stateMachine.ChangeState(new SearchState());
            }
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

    public void Shoot()
    {
        var gunBarrelPosition = gunBarrel.transform.position;
        
        Vector3 lookPos = Player().transform.position - gunBarrelPosition;

        lookPos.y = 0;

        Quaternion rotation = Quaternion.LookRotation(lookPos);

        rotation *= Quaternion.Euler(90, 0, 0);
        
        GameObject bullet = GameObject.Instantiate(Resources.Load("Prefabs/Bullet") as GameObject, gunBarrel.position, rotation);

        Vector3 shootDirection = (Player().transform.position - gunBarrelPosition).normalized;

        bullet.GetComponent<Rigidbody>().velocity = Quaternion.AngleAxis(Random.Range(-2f, 2f), Vector3.up) * shootDirection * 100;
        
        shotTimer = 0;
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
