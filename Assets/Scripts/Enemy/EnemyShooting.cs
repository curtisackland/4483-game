using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : Enemy
{
    [Header("Weapon Values")]
    public Transform gunBarrel;

    [Range(0.1f, 10)]
    public float fireRate;
    
    protected float searchTimer;
    protected float moveTimer;
    
    protected float shotTimer;
    
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        
    }
    
    public override void DoAttackState()
    {
        if (CanSeePlayer())
        {
            losePlayerTimer = 0;
            moveTimer += Time.deltaTime;
            shotTimer += Time.deltaTime;
            transform.LookAt(Player().transform);
            
            if (shotTimer > fireRate)
            {
                Shoot();
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
}
