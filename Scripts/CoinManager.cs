using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement; 

public class CoinManager : MonoBehaviour
{
    // Singleton instance
    public static CoinManager Instance { get; private set; }

    public int coinCount;
    public GameObject coinUI;
    public Image coinImage;
    public TMP_Text coinCountText;

    void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
             coinCount = 0; 

        }
        else
        {             Destroy(gameObject);

            Instance.coinUI = coinUI;
            Instance.coinImage = coinImage;
            Instance.coinCountText = coinCountText;
        }
    }
     void Start()
    {
        UpdateUI(CoinManager.Instance.coinCount);
        
    }
    public void IncrementCoinCount()
    {
        coinCount++;
        UpdateUI(coinCount);
    }

    void UpdateUI(int coinCount)
    {
        if (coinCountText != null)
        {
            coinCountText.text = " " + coinCount.ToString();
        }
    }

}