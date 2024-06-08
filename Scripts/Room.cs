
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public bool isFilled { get; set; } = false;

    public enum Directions
    {
        TOP,
        RIGHT,
        BOTTOM,
        LEFT,
        NONE,
    }

    [SerializeField] GameObject topWall;
    [SerializeField] GameObject rightWall;
    [SerializeField] GameObject bottomWall;
    [SerializeField] GameObject leftWall;

    Dictionary<Directions, GameObject> walls = new Dictionary<Directions, GameObject>();
    public Dictionary<Directions, Room> neighbors = new Dictionary<Directions, Room>();

    public Vector2Int Index { get; set; }
    public bool visited { get; set; } = false;

    Dictionary<Directions, bool> dirFlags = new Dictionary<Directions, bool>();

    private void Start()
    {
        walls[Directions.TOP] = topWall;
        walls[Directions.RIGHT] = rightWall;
        walls[Directions.BOTTOM] = bottomWall;
        walls[Directions.LEFT] = leftWall;

        dirFlags[Directions.TOP] = true;
        dirFlags[Directions.RIGHT] = true;
        dirFlags[Directions.BOTTOM] = true;
        dirFlags[Directions.LEFT] = true;
    }

    public void SetDirFlag(Directions dir, bool value)
    {
        dirFlags[dir] = value;
        if (walls.ContainsKey(dir))
        {
            walls[dir].SetActive(value);
        }
    }

    public bool IsWall(Directions dir)
    {
        return dirFlags.ContainsKey(dir) ? dirFlags[dir] : false;
    }
}
