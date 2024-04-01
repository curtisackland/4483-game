using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour, Damageable
{

    protected StateMachine stateMachine;
    private NavMeshAgent agent;
    private Vector3 lastKnowPos;
    protected float losePlayerTimer;

    public Path path;

    private GameObject player;
    
    public Animator animator;

    
    public Vector3 LastKnowPos
    {
        get => lastKnowPos;
        set => lastKnowPos = value;
    }

    [Header("Sight Values")]
    public float sightDistance = 20f;

    public float fieldOfView = 85f;

    public float eyeHeight = 0.5f;


    
    // for debugging
    [SerializeField]
    private string currentState;

    public string enemyName;

    [Header("Health and XP Points")]
    public float health = 100f;
    public float maxHealth;
    public float xpWorth = 40f;

    public float monsterPointsWorth = 5f;
    
    // Start is called before the first frame update
    public virtual void Start()
    {
        stateMachine = GetComponent<StateMachine>();
        agent = GetComponent<NavMeshAgent>();
        stateMachine.Initialize();
        player = GameObject.FindGameObjectWithTag("Player");
        maxHealth = health;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        currentState = stateMachine.activeState.ToString();
    }

    public NavMeshAgent Agent()
    {
        return agent;
    }

    public GameObject Player()
    {
        return player;
    }

    public bool CanSeePlayer(bool useViewAngle)
    {
        if (player != null)
        {
            // if player is within distance
            if (Vector3.Distance(transform.position, player.transform.position) < sightDistance)
            {
                // if player is in field of view
                Vector3 targetDirection = player.transform.position - transform.position - (Vector3.up * eyeHeight);
                float angleToPlayer = Vector3.Angle(targetDirection, transform.forward);
                if ((angleToPlayer >= -fieldOfView && angleToPlayer <= fieldOfView) || !useViewAngle)
                {
                    // if enemy can actually see the player
                    Ray ray = new Ray(transform.position + (Vector3.up * eyeHeight), targetDirection);
                    RaycastHit hitInfo;
                    if (Physics.Raycast(ray, out hitInfo, sightDistance, LayerMask.NameToLayer("Enemy")))
                    {
                        if (hitInfo.transform.gameObject == player)
                        {
                            Debug.DrawRay(ray.origin, ray.direction * sightDistance);
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }

    public abstract void DoAttackState();

    public abstract void DoPatrolState();

    public abstract void DoSearchState();

    public void Damage(float damageAmount)
    {
        health -= damageAmount;
        stateMachine.ChangeState(new AttackState());
        if (health <= 0)
        {
            PlayerXP p = player.GetComponent<PlayerXP>();
            p.AddXP(xpWorth);
            p.AddMonsterPoints(monsterPointsWorth);
            Destroy(gameObject);
        }
    }

    public void SetNavDestinationWithSpace(float stopRadius)
    {
        if ((transform.position - Player().transform.position).magnitude > stopRadius && Agent().speed != 0)
        {
            Agent().SetDestination(Player().transform.position +
                                   (transform.position - Player().transform.position).normalized * stopRadius);
        }
        else
        {
            // When too close to player, just rotate to look at them
            var dir = (Player().transform.position - transform.position).normalized;
            Agent().transform.rotation = Quaternion.RotateTowards(Agent().transform.rotation,
                Quaternion.LookRotation(new Vector3(dir.x, transform.position.y, dir.z)), Agent().angularSpeed * Time.deltaTime);
        }
    }
}
