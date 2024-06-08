using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Quitbutton : MonoBehaviour
{
        public AudioSource soundSource; 

    public void button_exit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
            Debuh.Log("YOU'VE QUIT THE GAME!");
        #endif
    
    }
    void Start()
    {
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