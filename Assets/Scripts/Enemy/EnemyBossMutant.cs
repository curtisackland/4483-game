using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class EnemyBossMutant : Enemy
{
    [Header("Mutant settings")]
    public float agentSpeed = 2.5f;

    [Header("Melee Attack")] 
    public float fastAttackRange;
    public float fastAttackCooldown = 3f;
    public float fastAttackTime = 2f;
    public float fastAttackDamage = 10f;
    private float fastAttackLastTime;

    [Header("Fireball Attack")] 
    public float fireballAttackRange;
    public GameObject launchedObject;
    public GameObject launchSource;
    public float projectileCooldown = 15f;
    public float projectileTime = 4f;
    [FormerlySerializedAs("shatterWindupTime")] public float throwWindupTime = 0.5f;
    private float projectileLastTime;

    [Header("Attack State")]
    public float losePlayerTime = 5f;
    private string lastAttackMove = "";
    private float lastAttackEndTime;
    private bool isMeleeAttacking = false;

    private Quaternion defaultAngle = Quaternion.Euler(0, 180, 0);
    private Quaternion offsetAngle = Quaternion.Euler(0, 180, 0);
    private float moveTimer;

    // public List<MeleeTrigger> meleeColliders = new();
    public GameObject homeArea;
    public float homeAreaRadius;

    public void Awake()
    {
        // SetCollidersDamage(false);
    }

    // Update is called once per frame
    public override void Update()
    {
        
    }
    
    public override void DoAttackState()
    {
        if (CanSeePlayer(false))
        {
            losePlayerTimer = 0;
            SetNavDestinationWithSpace(6f);

            if (lastAttackMove == "") // No attack is currently happening
            {
                if (Random.value < 0.5f) // Randomly start new attack
                {
                    float distanceFromPlayer = (transform.position - Player().transform.position).magnitude;
                    if (distanceFromPlayer < fastAttackRange && fastAttackLastTime + fastAttackCooldown < Time.time)
                    {
                        lastAttackMove = "FastAttack";
                        fastAttackLastTime = Time.time;
                        lastAttackEndTime = Time.time + fastAttackTime;
                        animator.Play("Attack(3)", 0);
                        Agent().speed = 0;
                        offsetAngle = Quaternion.Euler(0, 220, 0);
                        Player().GetComponent<PlayerHealth>().TakeDamage(fastAttackDamage);
                        // SetCollidersDamage(true);
                    }
                    else if (distanceFromPlayer < fireballAttackRange && projectileLastTime + projectileCooldown < Time.time)
                    {
                        lastAttackMove = "Projectile";
                        projectileLastTime = Time.time;
                        lastAttackEndTime = Time.time + projectileTime;
                        animator.Play("Shout", 0);
                        Agent().speed = 0;
                        
                        var fireball = Instantiate(launchedObject, launchSource.transform.position, launchSource.transform.rotation);

                        var fireballCollider = fireball.GetComponentInChildren<ThrowCollider>();
                        fireballCollider.startDelay = throwWindupTime;
                        
                        var ps = fireball.GetComponentInChildren<ParticleSystem>();
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
            // SetCollidersDamage(false);
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
    }

    public override void DoSearchState()
    {
        stateMachine.ChangeState(new PatrolState());
    }

    // public void SetCollidersDamage(bool doDamage)
    // {
    //     isMeleeAttacking = doDamage;
    //     foreach (var meleeCollider in meleeColliders)
    //     {
    //         meleeCollider.damageEnabled = doDamage;
    //     }
    // }
}
