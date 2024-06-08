using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMainScene : MonoBehaviour
{
    private string sceneName="Main";


    public void LoadScene()
    {
           CoinManager coinManager = CoinManager.Instance;
                      PlayerManager playerManager = PlayerManager.Instance;

           coinManager.coinCount = 0;
           playerManager.currentHealth = 100;
        SceneManager.LoadScene(sceneName);
      
    }
}
