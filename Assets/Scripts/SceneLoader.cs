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
    [SerializeField] private GameObject _InfoScreen;
    [SerializeField] private float _loadDelayTime;

    /// <summary>
    /// Loads a scene
    /// </summary>
    /// <param name="scene"></param>
    public void SceneLoad(string name)
    {
        if (name.Equals("Game"))
        {
            StartCoroutine(DelayedGameLoad(name));
        }
        else
        {
            SceneManager.LoadScene(name);
        }
    }

    /// <summary>
    /// Loads the game scene after a 3 second delay
    /// </summary>
    /// <returns></returns>
    private IEnumerator DelayedGameLoad(string name)
    {
        _InfoScreen.SetActive(true);
        yield return new WaitForSeconds(_loadDelayTime);
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
