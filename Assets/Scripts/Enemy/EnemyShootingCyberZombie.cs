using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyShootingCyberZombie : EnemyShooting
{
    [FormerlySerializedAs("fireRate")] [Range(0.1f, 10)]
    public float fireTime;
    public float lineupTime;
    public float lowerTime;
    private bool weaponIsLowering = false;
    
    // Update is called once per frame
    public override void Update()
    {
        SetIsMovingAnimation();
    }
    
    public override void DoAttackState()
    {
        if (CanSeePlayer(false))
        {
            losePlayerTimer = 0;
            moveTimer += Time.deltaTime;
            shotTimer += Time.deltaTime;

            if (shotTimer > fireTime - lowerTime && weaponIsLowering) // allow weapon to be lowered before disabling upper body layer
            {
                if (shotTimer > fireTime)
                {
                    animator.SetLayerWeight(1, 0);
                    weaponIsLowering = false;
                    shotTimer = 0;
                }
            }
            else if (!weaponIsLowering) // lining up to shoot
            {
                animator.SetLayerWeight(1, 1);
                animator.SetBool("isShooting", true);
                if (shotTimer > lineupTime) // Shoot
                {
                    Shoot();
                    animator.SetBool("isShooting", false);
                    weaponIsLowering = true;
                }
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
                animator.SetBool("isShooting", false);
                animator.SetLayerWeight(1, 0);

                // Go back to search state
                stateMachine.ChangeState(new SearchState());

            }
        }
    }

    public override void DoPatrolState()
    {
        if (CanSeePlayer(true))
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
        if (CanSeePlayer(true))
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
