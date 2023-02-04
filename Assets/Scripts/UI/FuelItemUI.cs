/*****************************************************************************
// File Name :         FuelItemUI.cs
// Author :            Andrea Swihart-DeCoster
// Creation Date :     February 4th, 2023
//
// Brief Description : This document controls the on screen fuel pickup UI.
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FuelItemUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _fuelUILayout;
    [SerializeField] private GameObject _fuelPickup;

    [SerializeField] private List<GameObject> _fuelsAdded = new List<GameObject>();

    public float NumFuel => _fuelsAdded.Count;

    /// <summary>
    /// Adds fuel to the fuel UI layout
    /// </summary>
    public void AddFuel()
    {
        GameObject newFuel = Instantiate(_fuelPickup);
        newFuel.transform.SetParent(_fuelUILayout.transform);
        newFuel.transform.localScale = new Vector2(1f, 1f);
        _fuelsAdded.Add(newFuel);
    }

    /// <summary>
    /// Removes a fuel
    /// </summary>
    public void RemoveFuel()
    {
        if(NumFuel > 0)
        {
            GameObject oldFuel = _fuelsAdded[0];
            _fuelsAdded.RemoveAt(0);
            Destroy(oldFuel);
        }
    }

    /// <summary>
    /// Centers the fuel to the screen
    /// </summary>
    public void Center()
    {
        _fuelUILayout.transform.position = new Vector2(450f, 20f);
    }

    /// <summary>
    /// Moves the fuel to the bottomm left
    /// </summary>
    public void LeftAlign()
    {
        _fuelUILayout.transform.position = new Vector2(0, 0);
    }
}
