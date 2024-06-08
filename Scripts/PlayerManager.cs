using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    public int currentHealth;
    public int maxHealth = 100;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            currentHealth = maxHealth; 
        }
        else
        {
            Destroy(gameObject);
        }
    }
}