
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TimerManager : MonoBehaviour
{
    public float totalTime = 60f; 
    private float remainingTime;
    public TextMeshProUGUI timerText;
    private bool gameStarted = false;
    private bool gameEnded = false;

    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex != 1)
        {
            timerText.gameObject.SetActive(false);
            return;
        }

        remainingTime = totalTime;
        
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex != 1)
        {
            return;
        }

        if (!gameStarted)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(StartGame());
            }
        }
        else
        {
            if (gameEnded) return;

            remainingTime -= Time.deltaTime;
            timerText.text = "" + Mathf.Clamp(remainingTime, 0, totalTime).ToString("F2");

            if (remainingTime <= 0)
            {
                EndGame();
            }
        }
    }

    private System.Collections.IEnumerator StartGame()
    {
        yield return new WaitForSeconds(1f);
        gameStarted = true;
        remainingTime = totalTime;
    }

    void EndGame()
    {
        gameEnded = true;
        SceneManager.LoadScene("Lose");
    }

    public void PlayerReachedDestination()
    {
        if (!gameEnded)
        {
            Debug.Log("Player reached the destination!");
        
    }
}}
