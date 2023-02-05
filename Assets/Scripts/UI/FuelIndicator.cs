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
    [SerializeField] private FuelItemUI _fuelItemUI;

    [Header("Fuel UI Components")]
    [SerializeField] private GameObject _fuelIndicatorScreen;
    [SerializeField] private Image _fuel;

    [SerializeField] private float _refuelTime; // total time it takes to add the full fuel amount
    [SerializeField] private float _incrementTime;  // # times the fuel will tick up


    private void Update()
    {
        // If player is looking at the fuel level indicator and presses E, they can refuel as long as they have fuel.
        // If they release E, the fueling stops and they lose the fuel
        if(Input.GetKey(KeyCode.Tab))
        {
            ShowIndicator();

            if(Input.GetKeyDown(KeyCode.E))
            {
                if (_fuelItemUI.NumFuel > 0)
                {
                    StartCoroutine(AddFuel(_lighterBehaviour.FuelAddAmount));
                }
            }
            if (Input.GetKeyUp(KeyCode.E))
            {
                StopAllCoroutines();
                _fuelItemUI.RemoveFuel();
            }
        }
        else
        {
            _fuelIndicatorScreen.SetActive(false);
            _fuelItemUI.LeftAlign();
        }
    }

    /// <summary>
    /// Sets the fuel indicator to active and sets the fuel amount
    /// </summary>
    private void ShowIndicator()
    {
        _fuelIndicatorScreen.SetActive(true);
        _fuel.fillAmount = _lighterBehaviour.FuelAmount;
        _fuelItemUI.Center();
    }

    /// <summary>
    /// Adds amount to in incremements of _incrementTime length over duration of _refuelTime
    /// </summary>
    /// <param name="amount"> total amount of fuel being added </param>
    /// <returns></returns>
    private IEnumerator AddFuel(float amount)
    {
        float time = 0;
        float amountPerTick = amount / (_refuelTime / _incrementTime);

        while(time < _refuelTime)
        {
            time += _incrementTime;
            _lighterBehaviour.AddFuel(amountPerTick);
            yield return new WaitForSeconds(_incrementTime);
        }
        _fuelItemUI.RemoveFuel();
    }
}
