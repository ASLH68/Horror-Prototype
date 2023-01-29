/*****************************************************************************
// File Name :         FuelIndicator.cs
// Author :            Andrea Swihart-DeCoster
// Creation Date :     January 28th, 2023
//
// Brief Description : This document controls the fuel indicator UI.
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelIndicator : MonoBehaviour
{
    [Header("Script Components")]
    [SerializeField] private LighterBehaviour _lighterBehaviour;

    [Header("Fuel UI Components")]
    [SerializeField] private GameObject _fuelIndicatorScreen;
    [SerializeField] private Image _fuel;


    private void Update()
    {
        if(Input.GetKey(KeyCode.Tab))
        {
            ShowIndicator();
        }
        else
        {
            _fuelIndicatorScreen.SetActive(false);
        }
    }

    /// <summary>
    /// Sets the fuel indicator to active and sets the fuel amount
    /// </summary>
    private void ShowIndicator()
    {
        _fuelIndicatorScreen.SetActive(true);
        _fuel.fillAmount = _lighterBehaviour.FuelAmount;
    }
}
