using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class SpawnEnemyInRadius : MonoBehaviour
{
    public float enableSpawnRadius;
    public float spawnRadius;
    [Tooltip("Distance between player and enemy where enemy despawns")]
    public float despawnRadius;
    public float spawnRate;
    [FormerlySerializedAs("maxNmEnemies")] public int maxNumEnemies;
    public float playerAvoidanceRadius;

    private PlayerXP player;

    public List<GameObject> spawnableEnemies;
    public List<float> spawnableEnemiesRates;
    private List<Enemy> enemiesInArea = new();
    private float timeSinceLastSpawn;
    
    public void Awake()
    {
        player = FindFirstObjectByType<PlayerXP>();
    }

    public void Update()
    {
        enemiesInArea.RemoveAll(item => item.IsDestroyed()); // Remove destroyed items
        
        if ((player.transform.position - transform.position).magnitude < enableSpawnRadius)
        {
            if (enemiesInArea.Count < maxNumEnemies && timeSinceLastSpawn + spawnRate < Time.time)
            {
                // Choose enemy
                float chanceSum = 0;
                foreach (var rate in spawnableEnemiesRates)
                {
                    chanceSum += rate;
                }

                float val = Random.Range(0, chanceSum);

                int chosenEnemy = spawnableEnemies.Count - 1;
                float passedEnemiesChanceSum = 0;
                for (int i = 0; i < spawnableEnemiesRates.Count; i++)
                {
                    if (val <= passedEnemiesChanceSum + spawnableEnemiesRates[i])
                    {
                        chosenEnemy = i;
                        break;
                    }
                    else
                    {
                        passedEnemiesChanceSum += spawnableEnemiesRates[i];
                    }
                }
                
                // Place enemy
                bool placedEnemy = false;
                for (int i = 0; i < 3 && !placedEnemy; i++)
                {
                    if (RandomPointRadiusNavMesh(transform.position, spawnRadius, out var spawnPoint))
                    {
                        if ((spawnPoint - player.transform.position).magnitude > playerAvoidanceRadius)
                        {
                            var newEnemy = Instantiate(spawnableEnemies[chosenEnemy], spawnPoint,
                                Quaternion.Euler(0, Random.Range(0, 360), 0));
                            enemiesInArea.Add(newEnemy.GetComponent<Enemy>());
                            timeSinceLastSpawn = Time.time;
                            placedEnemy = true;
                        }
                    }
                }

            }
        }
        
        
        foreach (var e in enemiesInArea)
        {
            if ((player.transform.position - e.transform.position).magnitude > despawnRadius)
            {
                Destroy(e.gameObject);
            }
        }
    }

    bool RandomPointRadiusNavMesh(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, range, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        var playerPos = FindFirstObjectByType<PlayerXP>().transform.position;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, enableSpawnRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(playerPos, despawnRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(playerPos, playerAvoidanceRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);

        if (enemiesInArea != null)
        {
            Gizmos.color = Color.red;
            foreach (var e in enemiesInArea)
            {
                if (!e.IsDestroyed())
                {
                    var p = e.transform.position;
                    p.y += 20f;
                    Gizmos.DrawSphere(p, 5f);
                }
            }
        }
    }
}