// using UnityEngine;

// public class EnemySpawner : MonoBehaviour
// {
//     public GameObject enemyPrefab; // Reference to the enemy prefab to spawn
//     public Vector2 spawnAreaSize; // Size of the area where enemies can spawn
//     public int numberOfEnemiesToSpawn; // Number of enemies to spawn

//     // Update is called once per frame
//     void Update()
//     {
//         // Check if the space bar is pressed
//         if (Input.GetKeyDown(KeyCode.Space))
//         {
//             // Spawn enemies in random positions inside the specified area
//             SpawnEnemies(numberOfEnemiesToSpawn);
//         }
//     }

//     // Function to spawn a specified number of enemies in random positions
//     void SpawnEnemies(int numberOfEnemies)
//     {
//         for (int i = 0; i < numberOfEnemies; i++)
//         {
//             // Generate random positions within the spawn area
//             float randomX = Random.Range(transform.position.x - spawnAreaSize.x / 2f, transform.position.x + spawnAreaSize.x / 2f);
//             float randomY = Random.Range(transform.position.y - spawnAreaSize.y / 2f, transform.position.y + spawnAreaSize.y / 2f);
            
//             // Set the z position to -1
//             Vector3 spawnPosition = new Vector3(randomX, randomY, -1f);

//             // Spawn the enemy prefab at the random position
//             Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
//         }
//     }
// }
