using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Scene2MazeGenerator1 : GenerateMaze
{
    [SerializeField] private Player2 player;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Button dfsButton;
    [SerializeField] private Button dijkstraButton;
    [SerializeField] private Button aStarButton;
    // private bool mazeGenerated = false;
    private List<Room> path;

    protected override void CreateMaze()
    {
        ClearCoins();
        ClearEnemies();
        base.CreateMaze();
        SpawnEnemiesAndCoins() ;
        dfsButton.onClick.AddListener(() => StartPathfinding("DFS"));
        dijkstraButton.onClick.AddListener(() => StartPathfinding("DIJKSTRA"));
        aStarButton.onClick.AddListener(() => StartPathfinding("A*"));
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
        } while (excludedPositions.Contains(pos)); // Exclude positions where enemies are already spawned

        Room room = rooms[pos.x, pos.y];
        Vector3 spawnPosition = room.transform.position;
        spawnPosition.z = 0f;
        Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
    }
}

    private void SpawnEnemy(Room room)
    {
        Vector3 spawnPosition = room.transform.position;
        spawnPosition.z = 0f; // Ensure the enemy is spawned on the same plane as the rooms
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

    public void StartPathfinding(string algorithm)
    {
        if (rooms == null)
        {
            UnityEngine.Debug.LogError("Rooms array is not initialized!");
            return;
        }

        Room startRoom = rooms[8, 8];
        Room endRoom = rooms[0, 0];

        if (startRoom == null || endRoom == null)
        {
            UnityEngine.Debug.LogError("StartRoom or EndRoom is null!");
            return;
        }

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        if (algorithm == "DFS")
        {
            UnityEngine.Debug.Log("Algorithm selected: Depth First Search");
            path = FindPathDFS(startRoom, endRoom);
        }
        else if (algorithm == "DIJKSTRA")
        {
            UnityEngine.Debug.Log("Algorithm selected: Dijkstra");
            path = FindPathDijkstra(startRoom, endRoom);
        }
        else if (algorithm == "A*")
        {
            UnityEngine.Debug.Log("Algorithm selected: A*");
            path = FindPathAStar(startRoom, endRoom);
        }

        stopwatch.Stop();
        UnityEngine.Debug.Log($"Pathfinding duration for {algorithm}: {stopwatch.ElapsedMilliseconds} ms");

        if (path != null && path.Count > 0)
        {
            UnityEngine.Debug.Log("Path found between start and end rooms.");
            foreach (Room room in path)
            {
                UnityEngine.Debug.Log("Room Index : " + room.Index);
            }
        }
        else
        {
            UnityEngine.Debug.Log("No path found between start and end rooms.");
        }

        dfsButton.gameObject.SetActive(false);
        dijkstraButton.gameObject.SetActive(false);
        aStarButton.gameObject.SetActive(false);

        SpawnCoins();
        player.MoveAlongPath(path);
    }

    public List<Room> FindPathDFS(Room startRoom, Room endRoom)
    {
        path = new List<Room>();
        HashSet<Room> visited = new HashSet<Room>();
        DFS(startRoom, endRoom, visited);
        return path;
    }

    private bool DFS(Room currentRoom, Room endRoom, HashSet<Room> visited)
    {
        visited.Add(currentRoom);
        path.Add(currentRoom);

        if (currentRoom == endRoom)
        {
            return true;
        }

        foreach (Room neighbor in graph[currentRoom])
        {
            if (!visited.Contains(neighbor))
            {
                if (DFS(neighbor, endRoom, visited))
                {
                    return true;
                }
            }
        }

        path.Remove(currentRoom);
        return false;
    }

    public List<Room> FindPathDijkstra(Room startRoom, Room endRoom)
    {
        var distances = new Dictionary<Room, float>();
        var previousRooms = new Dictionary<Room, Room>();
        var priorityQueue = new PriorityQueue<Room, float>();

        foreach (var room in rooms)
        {
            distances[room] = float.MaxValue;
            previousRooms[room] = null;
        }

        distances[startRoom] = 0;
        priorityQueue.Enqueue(startRoom, 0);

        while (priorityQueue.Count > 0)
        {
            Room currentRoom = priorityQueue.Dequeue();
            if (currentRoom == endRoom) break;

            foreach (var neighbor in graph[currentRoom])
            {
                float newDist = distances[currentRoom] + 1;
                if (newDist < distances[neighbor])
                {
                    distances[neighbor] = newDist;
                    previousRooms[neighbor] = currentRoom;
                    priorityQueue.Enqueue(neighbor, newDist);
                }
            }
        }

        var path = new List<Room>();
        for (Room at = endRoom; at != null; at = previousRooms[at])
        {
            path.Insert(0, at);
        }

        if (path.Count == 1 && path[0] != startRoom)
        {
            return new List<Room>();
        }

        return path;
    }

    public List<Room> FindPathAStar(Room startRoom, Room endRoom)
    {
        var openSet = new PriorityQueue<Room, float>();
        var cameFrom = new Dictionary<Room, Room>();
        var gScore = new Dictionary<Room, float>();
        var fScore = new Dictionary<Room, float>();

        foreach (var room in rooms)
        {
            gScore[room] = float.MaxValue;
            fScore[room] = float.MaxValue;
        }

        gScore[startRoom] = 0;
        fScore[startRoom] = Heuristic(startRoom, endRoom);
        openSet.Enqueue(startRoom, fScore[startRoom]);

        while (openSet.Count > 0)
        {
            Room current = openSet.Dequeue();

            if (current == endRoom)
            {
                return ReconstructPath(cameFrom, current);
            }

            foreach (Room neighbor in graph[current])
            {
                float tentativeGScore = gScore[current] + 1; 

                if (tentativeGScore < gScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = gScore[neighbor] + Heuristic(neighbor, endRoom);

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Enqueue(neighbor, fScore[neighbor]);
                    }
                }
            }
        }

        return new List<Room>(); 
    }

    private float Heuristic(Room a, Room b)
    {
        return Mathf.Abs(a.Index.x - b.Index.x) + Mathf.Abs(a.Index.y - b.Index.y); 
    }

    private List<Room> ReconstructPath(Dictionary<Room, Room> cameFrom, Room current)
    {
        var totalPath = new List<Room> { current };
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            totalPath.Insert(0, current);
        }
        return totalPath;
    }


public class PriorityQueue<TElement, TPriority>
{
    private readonly SortedDictionary<TPriority, Queue<TElement>> _dictionary = new();

    public int Count { get; private set; }

    public void Enqueue(TElement element, TPriority priority)
    {
        if (!_dictionary.TryGetValue(priority, out var queue))
        {
            queue = new Queue<TElement>();
            _dictionary[priority] = queue;
        }
        queue.Enqueue(element);
        Count++;
    }

    public TElement Dequeue()
    {
        var firstPair = _dictionary.GetEnumerator();
        firstPair.MoveNext();
        var element = firstPair.Current.Value.Dequeue();
        if (firstPair.Current.Value.Count == 0)
        {
            _dictionary.Remove(firstPair.Current.Key);
        }
        Count--;
        return element;
    }

    public bool Contains(TElement element)
    {
        foreach (var pair in _dictionary)
        {
            if (pair.Value.Contains(element))
            {
                return true;
            }
        }
        return false;
    }
}
}
