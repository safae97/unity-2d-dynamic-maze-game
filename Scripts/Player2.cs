using UnityEngine;
using System.Collections.Generic;

public class Player2 : MonoBehaviour
{     
     CoinManager coinManager = CoinManager.Instance;
     HealthManager healthManager = HealthManager.Instance;
    private float movementSpeed =10f; // Adjust movement speed as needed
    private List<Room> path;
    private int currentRoomIndex = 0;
    private bool isMoving = false;
    private PlayerManager playerManager;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPointmiddle;
    public Transform bulletSpawnPointup;
    public Transform bulletSpawnPointdown;
    private float bulletSpeed = 10f;
    public CoinManager cm;

    public void MoveAlongPath(List<Room> newPath)
    {
        path = newPath;
        currentRoomIndex = 0;
        isMoving = true;
    }
 
    void Shoot()
    {
        playerManager = PlayerManager.Instance;
        if (Input.GetKeyDown(KeyCode.A))
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPointmiddle.position, bulletSpawnPointmiddle.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 shootDirection = transform.localScale.x > 0 ? Vector2.left : Vector2.right;
                rb.velocity = 8 * shootDirection * bulletSpeed;
            }
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameObject bullet1 = Instantiate(bulletPrefab, bulletSpawnPointup.position, bulletSpawnPointup.rotation);
            Rigidbody2D rb = bullet1.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 shootDirection = Vector2.up;
                rb.velocity = 8 * shootDirection * bulletSpeed;
            }
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            GameObject bullet2 = Instantiate(bulletPrefab, bulletSpawnPointdown.position, bulletSpawnPointdown.rotation);
            Rigidbody2D rb = bullet2.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 shootDirection = Vector2.down;
                rb.velocity = 8 * shootDirection * bulletSpeed;
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(10);
        }
    }
    
   void TakeDamage(int damage)
{
    playerManager.currentHealth -= damage;
    if (playerManager.currentHealth <= 0)
    {
        playerManager.currentHealth = 0;
        Debug.Log("Player Died");
        Destroy(gameObject);
    }
    Debug.Log("Current Health: " + playerManager.currentHealth);
    
    HealthManager.Instance.UpdateHealthUI(playerManager.currentHealth);
}

    private void Update()
    {
        Shoot();
        if (isMoving)
        {
            Debug.Log("Player is moving along the path.");
            Room nextRoom = path[currentRoomIndex];
            Vector3 targetPosition = nextRoom.transform.position;
            targetPosition.z = transform.position.z; 
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                currentRoomIndex++;

                if (currentRoomIndex >= path.Count)
                {
                    isMoving = false; 
                    Debug.Log("Player reached the end of the path.");
                }
            }
        }
    }
    
    
    private void OnTriggerEnter2D(Collider2D other)
{
    if (other.gameObject.CompareTag("Coin"))
    {
        Destroy(other.gameObject);
        if (CoinManager.Instance != null)
        {
            CoinManager.Instance.IncrementCoinCount(); 
        } else
    {
        Debug.LogWarning("CoinManager reference is null!");
    }
    }
   
}

}
