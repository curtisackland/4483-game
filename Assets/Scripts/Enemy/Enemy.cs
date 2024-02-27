using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, Damageable
{

    private StateMachine stateMachine;
    private NavMeshAgent agent;
    private Vector3 lastKnowPos;

    public Path path;

    private GameObject player;
    
    public Vector3 LastKnowPos
    {
        get => lastKnowPos;
        set => lastKnowPos = value;
    }

    [Header("Sight Values")]
    public float sightDistance = 20f;

    public float fieldOfView = 85f;

    public float eyeHeight = 0.5f;

    [Header("Weapon Values")]
    public Transform gunBarrel;

    [Range(0.1f, 10)]
    public float fireRate;
    
    // for debugging
    [SerializeField]
    private string currentState;

    [Header("Health and XP Points")]
    public float health = 100f;

    public float xpWorth = 40f;
    
    // Start is called before the first frame update
    void Start()
    {
        stateMachine = GetComponent<StateMachine>();
        agent = GetComponent<NavMeshAgent>();
        stateMachine.Initialize();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        CanSeePlayer();
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

    public bool CanSeePlayer()
    {
        if (player != null)
        {
            // if player is within distance
            if (Vector3.Distance(transform.position, player.transform.position) < sightDistance)
            {
                // if player is in field of view
                Vector3 targetDirection = player.transform.position - transform.position - (Vector3.up * eyeHeight);
                float angleToPlayer = Vector3.Angle(targetDirection, transform.forward);
                if (angleToPlayer >= -fieldOfView && angleToPlayer <= fieldOfView)
                {
                    // if enemy can actually see the player
                    Ray ray = new Ray(transform.position + (Vector3.up * eyeHeight), targetDirection);
                    RaycastHit hitInfo = new RaycastHit();
                    if (Physics.Raycast(ray, out hitInfo, sightDistance))
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

    public void Damage(float damageAmount)
    {
        health -= damageAmount;
        
        if (health <= 0)
        {
            player.GetComponent<PlayerXP>().AddXP(xpWorth);
            Destroy(gameObject);
        }
    }
}
