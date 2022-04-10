using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class Menu : MonoBehaviour
{
    public AudioMixer audioMixer;

    private void Awake()
    {
        
    }

    private void Start()
    {
        audioMixer.SetFloat("volume", PlayerPrefs.GetFloat("Volume", -20f));

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void LevelSelect()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    
}
