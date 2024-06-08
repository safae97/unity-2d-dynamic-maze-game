using UnityEngine;

public class camera : MonoBehaviour {
public Transform player;

    void Update()
    {
        if (player != null)
        {
            transform.position = player.position + new Vector3(-3, -1, -6);
        }
    }
}


