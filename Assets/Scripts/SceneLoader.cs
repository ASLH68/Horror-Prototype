/*****************************************************************************
// File Name :         SceneLoader.cs
// Author :            Andrea Swihart-DeCoster
// Creation Date :     January 28th, 2023
//
// Brief Description : This document controls the scene loading.
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    /// <summary>
    /// Loads a scene
    /// </summary>
    /// <param name="scene"></param>
    public void SceneLoad(string name)
    {
        SceneManager.LoadScene(name);
    }

    /// <summary>
    /// Quits the game
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}
