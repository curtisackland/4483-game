using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class EnemyBossMonkey : Enemy
{
    [Header("Monkey settings")]
    public float agentSpeed = 2.5f;
    
    [Header("Whirl Attack")]
    public float fastAttackCooldown = 3f;
    public float fastAttackTime = 2f;
    private float fastAttackLastTime;

    [Header("Shatter Attack")] 
    public GameObject shatterObject;
    public float shatterCooldown = 15f;
    public float shatterTime = 4f;
    public float shatterWindupTime = 1.5f;
    private float shatterLastTime;

    [Header("Attack State")]
    public float losePlayerTime = 5f;
    private string lastAttackMove = "";
    private float lastAttackEndTime;

    public GameObject monkeyRotator;
    private Quaternion defaultAngle = Quaternion.Euler(0, 180, 0);
    private Quaternion offsetAngle = Quaternion.Euler(0, 180, 0);

    public MeleeCollider meleeCollider;
    
    
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
                        animator.Play("MK_quickSwipe", 0);
                        Agent().speed = 0;
                        offsetAngle = Quaternion.Euler(0, 220, 0);
                    }
                    else if (distanceFromPlayer < 18f && shatterLastTime + shatterCooldown < Time.time)
                    {
                        lastAttackMove = "Shatter";
                        shatterLastTime = Time.time;
                        lastAttackEndTime = Time.time + shatterTime;
                        animator.Play("MK_stabJumpFward", 0);
                        Agent().speed = 0;

                        var shatter = Instantiate(shatterObject, transform.position, transform.rotation);
                        var ps = shatter.GetComponent<ParticleSystem>();
                        var psMain = ps.main;
                        psMain.startDelay = shatterWindupTime;
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

        monkeyRotator.transform.localRotation = Quaternion.RotateTowards(monkeyRotator.transform.localRotation, offsetAngle, 500f * Time.deltaTime);
        
        SetWalkingAnimationFromNav();
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

    private void SetWalkingAnimationFromNav()
    {
        if (Agent().velocity.magnitude > 0.2f && Agent().remainingDistance > 1f)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", true);
        }

        animator.SetFloat("walkSpeed", Agent().velocity.magnitude / agentSpeed);
    }
}
