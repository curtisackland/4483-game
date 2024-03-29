using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    public GameObject enemy;
    public GameObject enemyZombie;
    public GameObject enemyCyberZombie;
    public GameObject enemyDemonTree;
    public GameObject enemyMonkeyKing;

    public float maxSpawnRadius = 25f;

    public float minSpawnRadius = 10f;

    public float despawnRadius = 40f;

    public int maxAmountOfEnemies = 20;
    
    public LayerMask terrainLayer;

    private NavMeshSurface navMeshSurface;

    private List<GameObject> enemies = new List<GameObject>();
    
    // Start is called before the first frame update
    void Start()
    {
        navMeshSurface = FindObjectOfType<NavMeshSurface>();
    }

    // Update is called once per frame
    void Update()
    {
        
        for (int i = 0; i < enemies.Count; i++)
        {
            // Remove deleted enemies from the list
            if (enemies[i] == null)
            {
                enemies.RemoveAt(i);
            }
            else
            {
                // Delete enemies that travel too far from the player
                if (Vector3.Distance(enemies[i].transform.position, transform.position) > despawnRadius)
                {
                    Destroy(enemies[i]);
                    enemies.RemoveAt(i);
                }
            }
        }

        if (enemies.Count <= 3)
        {
            int oldNumEnemies = enemies.Count;
            for (int i = 0; i < maxAmountOfEnemies - oldNumEnemies; i++)
            {
                Vector3 randomPosition = Random.insideUnitSphere * maxSpawnRadius;
                randomPosition += transform.position;
                randomPosition.y = 0f;

                RaycastHit hit;
                if (Physics.Raycast(randomPosition + Vector3.up * 1000f, Vector3.down, out hit, Mathf.Infinity, terrainLayer))
                {
                    randomPosition.y = hit.point.y; // Set the Y position to the terrain height
                }
                
                // Check if the random position is too close to the player
                if (Vector3.Distance(randomPosition, transform.position) < minSpawnRadius)
                {
                    // Adjust position to maintain minimum distance from the player
                    randomPosition += (randomPosition - transform.position).normalized * minSpawnRadius;
                }

                float spawnValue = Random.value;

                GameObject enemyChosen;
                if (spawnValue < 0.2f)
                {
                    enemyChosen = enemyMonkeyKing;
                } 
                else if (spawnValue < 0.4f)
                {
                    enemyChosen = enemyDemonTree;
                }
                else if (spawnValue < 0.6f)
                {
                    enemyChosen = enemyCyberZombie;
                } 
                else if (spawnValue < 0.8f)
                {
                    enemyChosen = enemyZombie;
                }
                else
                {
                    enemyChosen = enemy;
                }

                //enemyChosen = enemyDemonTree;

                // Spawn enemy at the random position
                GameObject newEnemy = Instantiate(enemyChosen, randomPosition, Quaternion.identity);
                
                enemies.Add(newEnemy);
                
            }
            
            // Ensure enemy is on the NavMesh
            //navMeshSurface.BuildNavMesh();
            
        }
    }
}
