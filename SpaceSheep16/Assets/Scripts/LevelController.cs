using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    private int currentLevelIndex;

    private void Start()
    {
        currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            LoadNextLevel();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetLevel();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            LoadPreviousLevel();
        }
    }

    private void LoadNextLevel()
    {
        if(currentLevelIndex + 1 < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(currentLevelIndex + 1);
    }

    private void LoadPreviousLevel()
    {
        if(currentLevelIndex - 1 >= 0)
            SceneManager.LoadScene(currentLevelIndex - 1);
    }

    private void ResetLevel()
    {
        SceneManager.LoadScene(currentLevelIndex);
    }
}
