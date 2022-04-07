using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

public class LevelSelect : MonoBehaviour
{
    public TMP_Text level1;
    public TMP_Text sandbox;
    void Start()
    {
        TimeSpan time;
        if (PlayerPrefs.HasKey("Level1"))
        {
            time = TimeSpan.FromSeconds(PlayerPrefs.GetFloat("Level1"));
            level1.text = time.ToString(@"mm\:ss\:fff");
        }
        if (PlayerPrefs.HasKey("Sandbox"))
        {
            time = TimeSpan.FromSeconds(PlayerPrefs.GetFloat("Sandbox"));
            sandbox.text = time.ToString(@"mm\:ss\:fff");
        }
    }

    public void LoadLevel(GameObject buttonName)
    {
        SceneManager.LoadScene(buttonName.name);
    }

    public void Back()
    {
        SceneManager.LoadScene(0);
    }
}
