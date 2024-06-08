using UnityEngine;

public class Player1 : MonoBehaviour
{
    public Transform bulletSpawnPointmiddle;
    public Transform bulletSpawnPointup;
    public Transform bulletSpawnPointdown;
    public GameObject bulletPrefab;
    private float bulletSpeed = 10f;
    public float speed = 10f; 
    private PlayerManager playerManager;
    HealthManager healthManager = HealthManager.Instance;
    CoinManager coinManager = CoinManager.Instance;
    public CoinManager cm;
    void Start()
    {
        
    }

    void Update()
    {
        playerManager = PlayerManager.Instance;
        MovePlayer();
        Shoot();
    }

    void MovePlayer()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        transform.Translate(new Vector3(horizontalInput * speed * Time.deltaTime, verticalInput * speed * Time.deltaTime, 0f));

        Vector3 characterScale = transform.localScale;
        if (horizontalInput < 0)
        {
            characterScale.x = 2;
        }
        else if (horizontalInput > 0)
        {
            characterScale.x = -2;
        }
        transform.localScale = characterScale;
    }

    void Shoot()
    {
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
    private void OnTriggerEnter2D(Collider2D other)
{
    if (other.gameObject.CompareTag("Coin"))
    {
        Destroy(other.gameObject);
        if (CoinManager.Instance != null)
        {
            CoinManager.Instance.IncrementCoinCount(); 
        } 
        else
    {
        Debug.LogWarning("CoinManager reference is null!");
    }
    }
    else if (other.gameObject.CompareTag("BonusCoin"))
    {   
        Destroy(other.gameObject);
        if (CoinManager.Instance != null)
        {
            CoinManager.Instance.IncrementCoinCount(); 
            CoinManager.Instance.IncrementCoinCount(); 
        }
        else
    {
        Debug.LogWarning("CoinManager reference is null!");
    }
    }

   
}


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("fin"))
        {
            FindObjectOfType<TimerManager>().PlayerReachedDestination();
        }
    }
}
