
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMaze : MonoBehaviour
{
  protected List<Tuple<Room, Vector3>> roomsWithCoins = new List<Tuple<Room, Vector3>>();

    protected Dictionary<Room, List<Room>> graph = new Dictionary<Room, List<Room>>();
    protected Room[,] rooms = null;
    protected int numX = 9;
    protected int numY = 9;
    float roomWidth;
    float roomHeight;
    Stack<Room> stack = new Stack<Room>();
    bool generating = false;
    [SerializeField] GameObject roomPrefab;
    protected virtual void SpawnCoins() { }

    protected virtual void SpawnEnemies() { }
    public void GetRoomSize()
    {
        SpriteRenderer[] spriteRenderers = roomPrefab.GetComponentsInChildren<SpriteRenderer>();

        Vector3 minBounds = Vector3.positiveInfinity;
        Vector3 maxBounds = Vector3.negativeInfinity;

        foreach (SpriteRenderer ren in spriteRenderers)
        {
            minBounds = Vector3.Min(minBounds, ren.bounds.min);
            maxBounds = Vector3.Max(maxBounds, ren.bounds.max);
        }

        roomWidth = maxBounds.x - minBounds.x;
        roomHeight = maxBounds.y - minBounds.y;
    }

    private void Start()
    {
        GetRoomSize();
        InitializeRooms();
    }

    void InitializeRooms()
    {
        rooms = new Room[numX, numY];
        for (int i = 0; i < numX; ++i)
        {
            for (int j = 0; j < numY; ++j)
            {
                GameObject room = Instantiate(roomPrefab, new Vector3(i * roomWidth, j * roomHeight, 0.0f), Quaternion.identity);
                room.name = "Room_" + i.ToString() + "_" + j.ToString();
                rooms[i, j] = room.GetComponent<Room>();
                rooms[i, j].Index = new Vector2Int(i, j);
            }
        }
    }

    protected virtual void CreateMaze()
    {
        if (generating) return;

        Reset();
        RemoveRoomWall(0, 0, Room.Directions.BOTTOM);
        RemoveRoomWall(numX - 1, numY - 1, Room.Directions.RIGHT);

        stack.Push(rooms[0, 0]);
        Coroutine_Generate();
        ConvertMazeToGraph();
    }

    private void RemoveRoomWall(int x, int y, Room.Directions dir)
    {
        if (dir != Room.Directions.NONE)
        {
            rooms[x, y].SetDirFlag(dir, false);
        }

        Room.Directions opp = Room.Directions.NONE;
        switch (dir)
        {
            case Room.Directions.TOP:
                if (y < numY - 1)
                {
                    opp = Room.Directions.BOTTOM;
                    ++y;
                }
                break;
            case Room.Directions.RIGHT:
                if (x < numX - 1)
                {
                    opp = Room.Directions.LEFT;
                    ++x;
                }
                break;
            case Room.Directions.BOTTOM:
                if (y > 0)
                {
                    opp = Room.Directions.TOP;
                    --y;
                }
                break;
            case Room.Directions.LEFT:
                if (x > 0)
                {
                    opp = Room.Directions.RIGHT;
                    --x;
                }
                break;
        }
        if (opp != Room.Directions.NONE)
        {
            rooms[x, y].SetDirFlag(opp, false);
        }
    }

    private void ConvertMazeToGraph()
    {
        graph.Clear();

        for (int i = 0; i < numX; ++i)
        {
            for (int j = 0; j < numY; ++j)
            {
                Room room = rooms[i, j];
                graph[room] = new List<Room>();

                // Check and add neighboring rooms as edges
                if (j < numY - 1 && !rooms[i, j].IsWall(Room.Directions.TOP) && !rooms[i, j + 1].IsWall(Room.Directions.BOTTOM))
                {
                    graph[room].Add(rooms[i, j + 1]);
                }
                if (i < numX - 1 && !rooms[i, j].IsWall(Room.Directions.RIGHT) && !rooms[i + 1, j].IsWall(Room.Directions.LEFT))
                {
                    graph[room].Add(rooms[i + 1, j]);
                }
                if (j > 0 && !rooms[i, j].IsWall(Room.Directions.BOTTOM) && !rooms[i, j - 1].IsWall(Room.Directions.TOP))
                {
                    graph[room].Add(rooms[i, j - 1]);
                }
                if (i > 0 && !rooms[i, j].IsWall(Room.Directions.LEFT) && !rooms[i - 1, j].IsWall(Room.Directions.RIGHT))
                {
                    graph[room].Add(rooms[i - 1, j]);
                }
            }
        }
    }

    private void Reset()
    {
        for (int i = 0; i < numX; ++i)
        {
            for (int j = 0; j < numY; ++j)
            {
                rooms[i, j].SetDirFlag(Room.Directions.TOP, true);
                rooms[i, j].SetDirFlag(Room.Directions.RIGHT, true);
                rooms[i, j].SetDirFlag(Room.Directions.BOTTOM, true);
                rooms[i, j].SetDirFlag(Room.Directions.LEFT, true);
                rooms[i, j].visited = false;
            }
        }
    }

    void Coroutine_Generate()
    {
        generating = true;
        bool flag = false;
        while (!flag)
        {
            flag = GenerateStep();
        }
        generating = false;
    }

    private bool GenerateStep()
    {
        if (stack.Count == 0) return true;

        Room r = stack.Peek();
        var neighbours = GetNeighboursNotVisited(r.Index.x, r.Index.y);

        if (neighbours.Count != 0)
        {
            var index = 0;
            if (neighbours.Count > 1)
            {
                index = UnityEngine.Random.Range(0, neighbours.Count);
            }

            var item = neighbours[index];
            Room neighbour = item.Item2;
            neighbour.visited = true;
            RemoveRoomWall(r.Index.x, r.Index.y, item.Item1);

            stack.Push(neighbour);
        }
        else
        {
            stack.Pop();
        }

        return false;
    }

    public List<Tuple<Room.Directions, Room>> GetNeighboursNotVisited(int cx, int cy)
    {
        List<Tuple<Room.Directions, Room>> neighbours = new List<Tuple<Room.Directions, Room>>();

        foreach (Room.Directions dir in Enum.GetValues(typeof(Room.Directions)))
        {
            int x = cx;
            int y = cy;

            switch (dir)
            {
                case Room.Directions.TOP:
                    if (y < numY - 1)
                    {
                        ++y;
                        if (!rooms[x, y].visited)
                        {
                            neighbours.Add(new Tuple<Room.Directions, Room>(Room.Directions.TOP, rooms[x, y]));
                        }
                    }
                    break;
                case Room.Directions.RIGHT:
                    if (x < numX - 1)
                    {
                        ++x;
                        if (!rooms[x, y].visited)
                        {
                            neighbours.Add(new Tuple<Room.Directions, Room>(Room.Directions.RIGHT, rooms[x, y]));
                        }
                    }
                    break;
                case Room.Directions.BOTTOM:
                    if (y > 0)
                    {
                        --y;
                        if (!rooms[x, y].visited)
                        {
                            neighbours.Add(new Tuple<Room.Directions, Room>(Room.Directions.BOTTOM, rooms[x, y]));
                        }
                    }
                    break;
                case Room.Directions.LEFT:
                    if (x > 0)
                    {
                        --x;
                        if (!rooms[x, y].visited)
                        {
                            neighbours.Add(new Tuple<Room.Directions, Room>(Room.Directions.LEFT, rooms[x, y]));
                        }
                    }
                    break;
            }
        }
        return neighbours;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!generating)
            {
                CreateMaze();
            }
        }
    }
}
