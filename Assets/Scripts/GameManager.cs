using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public Transform player;
    public GameObject endScreen;
    public static bool isComplete;
    bool firstInput = false;

    void Update()
    {
        if(Input.anyKeyDown && !firstInput)
        {
            Stopwatch.StartStopwatch();
            firstInput = true;
        }

        if(player.position.y < -10)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
        
    }

    public void Finish()
    {
        endScreen.SetActive(true);
        Time.timeScale = 0f;
        Stopwatch.StopStopwatch();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Debug.Log(Stopwatch.currentTime);
        if (!PlayerPrefs.HasKey(SceneManager.GetActiveScene().name) || PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name) > Stopwatch.currentTime)
        {
            PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name, Stopwatch.currentTime);
        }
        isComplete = true;
    }

    public void NextLevel()
    {
        isComplete = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = 1f;
    }
}
