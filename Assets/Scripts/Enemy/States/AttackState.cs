using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AttackState : BaseState
{
    private float moveTimer;

    private float losePlayerTimer;

    private float shotTimer;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Enter()
    {
        
    }

    public override void Perform()
    {
        if (enemy.CanSeePlayer())
        {
            losePlayerTimer = 0;
            moveTimer += Time.deltaTime;
            shotTimer += Time.deltaTime;
            enemy.transform.LookAt(enemy.Player().transform);
            
            if (shotTimer > enemy.fireRate)
            {
                Shoot();
            }
            
            if (moveTimer > Random.Range(3, 7))
            {
                // randomly move enemy while attacking
                enemy.Agent().SetDestination(enemy.transform.position + (Random.insideUnitSphere * 5));
                moveTimer = 0;
            }

            enemy.LastKnowPos = enemy.Player().transform.position;
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

    public void Shoot()
    {

        Transform gunBarrel = enemy.gunBarrel;

        Vector3 lookPos = enemy.Player().transform.position - gunBarrel.transform.position;

        lookPos.y = 0;

        Quaternion rotation = Quaternion.LookRotation(lookPos);

        rotation *= Quaternion.Euler(90, 0, 0);
        
        GameObject bullet = GameObject.Instantiate(Resources.Load("Prefabs/Bullet") as GameObject, gunBarrel.position, rotation);

        Vector3 shootDirection = (enemy.Player().transform.position - gunBarrel.transform.position).normalized;

        bullet.GetComponent<Rigidbody>().velocity = Quaternion.AngleAxis(Random.Range(-2f, 2f), Vector3.up) * shootDirection * 100;
        
        shotTimer = 0;
    }

    public override void Exit()
    {
        
    }
}
