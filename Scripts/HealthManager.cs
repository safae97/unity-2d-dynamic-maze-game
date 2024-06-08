using UnityEngine;
using UnityEngine.UI;
using TMPro; 
            using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    public static HealthManager Instance;

    public Image healthBar;
   public TextMeshProUGUI healthText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {

            Instance.healthBar = healthBar;
            Instance.healthText = healthText;
            Destroy(gameObject);
        }
    }


    void Start()
    {
        UpdateHealthUI(PlayerManager.Instance.currentHealth);
    }

    public void UpdateHealthUI(int currentHealth)
    {
        healthBar.fillAmount = (float)currentHealth / PlayerManager.Instance.maxHealth;

        healthText.text = "" + currentHealth.ToString();

        if(currentHealth <= 0)
        {

        SceneManager.LoadScene("Lose");
    }}
}
