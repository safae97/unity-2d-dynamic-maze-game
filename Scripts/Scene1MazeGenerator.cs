
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
public class Scene1MazeGenerator : GenerateMaze
{
    [SerializeField]
    private GameObject coinPrefab;
      [SerializeField]
    private GameObject enemyPrefab;
    

     protected override void CreateMaze()
    {ClearCoins();
    ClearEnemies();
        base.CreateMaze();
        
SpawnEnemiesAndCoins() ;
   }


    private void ClearEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
    }

    private void ClearCoins()
{
    GameObject[] coins = GameObject.FindGameObjectsWithTag("Coin");
    foreach (GameObject coin in coins)
    {
        Destroy(coin);
    }
}

private void SpawnCoin(Room room)
{
    Vector3 spawnPosition = room.transform.position;
    spawnPosition.z = 0f; 
    Instantiate(coinPrefab, spawnPosition, Quaternion.identity);

    roomsWithCoins.Add(new Tuple<Room, Vector3>(room, spawnPosition));
}

protected override void SpawnCoins()
{
    int numCoinsToSpawn = 10; 

    for (int i = 0; i < numCoinsToSpawn; i++)
    {
        int randomX = UnityEngine.Random.Range(0, numX);
        int randomY = UnityEngine.Random.Range(0, numY);
        Room room = rooms[randomX, randomY];
        SpawnCoin(room);
    }
}

   private void SpawnEnemiesAndCoins()
{
    int numEnemiesToSpawn = 6; 
    int numCoinsToSpawn = 10; 

    List<Vector2Int> excludedPositions = new List<Vector2Int>(); 
    for (int i = 0; i < numEnemiesToSpawn; i++)
    {
        Vector2Int pos;
        do
        { 
            pos = new Vector2Int(UnityEngine.Random.Range(1, numX - 1), UnityEngine.Random.Range(1, numY - 1));
        } while (excludedPositions.Contains(pos)); 

        excludedPositions.Add(pos); 

        Room room = rooms[pos.x, pos.y];
        Vector3 spawnPosition = room.transform.position;
        spawnPosition.z = 0f;
        GameObject enemyObj = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        EnemyMouvement enemy = enemyObj.GetComponent<EnemyMouvement>();
        enemy.SetIndex(i);
    }

    for (int i = 0; i < numCoinsToSpawn; i++)
    {
        Vector2Int pos;
        do
        {
            pos = new Vector2Int(UnityEngine.Random.Range(1, numX - 1), UnityEngine.Random.Range(1, numY - 1));
        } while (excludedPositions.Contains(pos)); 

        Room room = rooms[pos.x, pos.y];
        Vector3 spawnPosition = room.transform.position;
        spawnPosition.z = 0f;
        Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
    }
}

    private void SpawnEnemy(Room room)
    {
        Vector3 spawnPosition = room.transform.position;
        spawnPosition.z = 0f; 
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }

    protected override void SpawnEnemies()
    {
        int numEnemiesToSpawn = 5;
        List<Room> roomsToSpawnEnemies = new List<Room>();

        for (int i = 0; i < numEnemiesToSpawn; i++)
        {
          int randomX = UnityEngine.Random.Range(0, numX);
          int randomY = UnityEngine.Random.Range(0, numY);
          Room room = rooms[randomX, randomY];
          roomsToSpawnEnemies.Add(room);
        }

        foreach (Room room in roomsToSpawnEnemies)
        {
          SpawnEnemy(room);
        }
    }}