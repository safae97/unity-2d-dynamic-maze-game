using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LOST : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    public TMP_Text coinCountText;
        public AudioSource soundSource; 
    void Start()
    {
        healthText.text = "Health: " + PlayerManager.Instance.currentHealth;
        coinCountText.text = "Coins: " + CoinManager.Instance.coinCount;
        PlaySound();
        }

    public void PlaySound()
    {
        if (soundSource != null && soundSource.clip != null)
        {
            soundSource.Play();
        }
        else
        {
            Debug.LogWarning("AudioSource or AudioClip is missing.");
        }
    }
}
