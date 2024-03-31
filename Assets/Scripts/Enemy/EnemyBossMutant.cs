using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class EnemyBossMutant : Enemy
{
    [Header("Monkey settings")]
    public float agentSpeed = 2.5f;
    
    [Header("Whirl Attack")]
    public float fastAttackCooldown = 3f;
    public float fastAttackTime = 2f;
    private float fastAttackLastTime;

    public GameObject launchedObject;
    public GameObject launchSource;
    public float projectileCooldown = 15f;
    public float projectileTime = 4f;
    [FormerlySerializedAs("shatterWindupTime")] public float throwWindupTime = 1.5f;
    private float projectileLastTime;

    [Header("Attack State")]
    public float losePlayerTime = 5f;
    private string lastAttackMove = "";
    private float lastAttackEndTime;

    private Quaternion defaultAngle = Quaternion.Euler(0, 180, 0);
    private Quaternion offsetAngle = Quaternion.Euler(0, 180, 0);

    public MeleeCollider meleeCollider;
    public GameObject homeArea;
    public float homeAreaRadius;

    
    // Update is called once per frame
    public override void Update()
    {
        
    }
    
    public override void DoAttackState()
    {
        if (CanSeePlayer(false))
        {
            losePlayerTimer = 0;
            // Debug.Log(transform.position);
            // Debug.Log(Player().transform.position + (Player().transform.position - transform.position).normalized * 2f);
            // Debug.Log(Player().transform.position);
            Agent().SetDestination(Player().transform.position + (Player().transform.position - transform.position).normalized * 2f);
            if (lastAttackMove == "") // No attack is currently happening
            {
                if (Random.value < 0.5f) // Randomly start new attack
                {
                    float distanceFromPlayer = (transform.position - Player().transform.position).magnitude;
                    if (distanceFromPlayer < 3f && fastAttackLastTime + fastAttackCooldown < Time.time)
                    {
                        lastAttackMove = "FastAttack";
                        fastAttackLastTime = Time.time;
                        lastAttackEndTime = Time.time + fastAttackTime;
                        animator.Play("Attack(3)", 0);
                        Agent().speed = 0;
                        offsetAngle = Quaternion.Euler(0, 220, 0);
                    }
                    else if (distanceFromPlayer < 18f && projectileLastTime + projectileCooldown < Time.time)
                    {
                        lastAttackMove = "Projectile";
                        projectileLastTime = Time.time;
                        lastAttackEndTime = Time.time + projectileTime;
                        animator.Play("Shout", 0);
                        Agent().speed = 0;
                        
                        var shatter = Instantiate(launchedObject, transform.position, transform.rotation);
                        var ps = shatter.GetComponentInChildren<ParticleSystem>();
                        var psMain = ps.main;
                        psMain.startDelay = throwWindupTime;
                    }
                }
            } 
        }
        else
        {
            losePlayerTimer += Time.deltaTime;
            if (losePlayerTimer > losePlayerTime)
            {
                stateMachine.ChangeState(new SearchState());
            }
        }
        
        if (lastAttackEndTime < Time.time) // Last attack has ended
        {
            lastAttackMove = "";
            Agent().speed = agentSpeed;
            offsetAngle = defaultAngle;
        }
    }

    public override void DoPatrolState()
    {
        if (CanSeePlayer(true))
        {
            stateMachine.ChangeState(new AttackState());
        }
        else
        {
            transform.Rotate(0, 1, 0);
        }
    }

    public override void DoSearchState()
    {
        stateMachine.ChangeState(new PatrolState());
    }
}
