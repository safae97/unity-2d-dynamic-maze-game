using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class Scene3MazeGenerator1 : GenerateMaze
{
    [SerializeField]
    private GameObject coinPrefab;
    [SerializeField]
    private GameObject bonusCoinPrefab;  
    [SerializeField]
    private GameObject enemyPrefab;

    protected override void CreateMaze()
    {
        ClearCoins();
        ClearEnemies();
        base.CreateMaze();
        SpawnEnemiesAndCoins();
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

        GameObject[] bonusCoins = GameObject.FindGameObjectsWithTag("BonusCoin");
        foreach (GameObject bonusCoin in bonusCoins)
        {
            Destroy(bonusCoin);
        }
    }

    private void SpawnCoin(Room room, GameObject coinPrefab, string coinTag)
    {
        Vector3 spawnPosition = room.transform.position;
        spawnPosition.z = 0f; 
        GameObject coin = Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
        coin.tag = coinTag;
    }

    protected override void SpawnCoins()
    {
        int numCoinsToSpawn = 8;  
        int numBonusCoinsToSpawn = 5;  

        HashSet<Vector2Int> excludedPositions = new HashSet<Vector2Int>();

        for (int i = 0; i < numCoinsToSpawn; i++)
        {
            Vector2Int pos;
            do
            {
                pos = new Vector2Int(UnityEngine.Random.Range(0, numX), UnityEngine.Random.Range(0, numY));
            } while (excludedPositions.Contains(pos));
            
            excludedPositions.Add(pos);
            Room room = rooms[pos.x, pos.y];
            SpawnCoin(room, coinPrefab, "Coin");
        }

        for (int i = 0; i < numBonusCoinsToSpawn; i++)
        {
            Vector2Int pos;
            do
            {
                pos = new Vector2Int(UnityEngine.Random.Range(0, numX), UnityEngine.Random.Range(0, numY));
            } while (excludedPositions.Contains(pos));
            
            excludedPositions.Add(pos);
            Room room = rooms[pos.x, pos.y];
            SpawnCoin(room, bonusCoinPrefab, "BonusCoin");
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
    }

    private void SpawnEnemiesAndCoins()
    {
        int numEnemiesToSpawn = 6;
        int numCoinsToSpawn = 8; 
        int numBonusCoinsToSpawn = 5; 

        HashSet<Vector2Int> excludedPositions = new HashSet<Vector2Int>();

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

            excludedPositions.Add(pos);
            Room room = rooms[pos.x, pos.y];
            Vector3 spawnPosition = room.transform.position;
            spawnPosition.z = 0f;
            Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
        }

        for (int i = 0; i < numBonusCoinsToSpawn; i++)
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
            Instantiate(bonusCoinPrefab, spawnPosition, Quaternion.identity);
        }
    }
}