/*****************************************************************************
// File Name :         PlayerController.cs
// Author :            Andrea Swihart-DeCoster
// Creation Date :     January 23rd, 2023
//
// Brief Description : This document controls the player movement.
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Variables

    [Header("components")]
    [SerializeField] private Rigidbody2D _rb2d;

    #region Movement Variables

    [Header("Movement Information")]
    [SerializeField] private int _speed = 10;
    [SerializeField] private int _runSpeed = 15;

    private int _currentSpeed;
    #endregion
    #endregion

    #region Functions
    // Update is called once per frame
    void Update()
    {
        Move();
    }

    #region Gameplay
    /// <summary>
    /// Moves the player using WASD or arrow keys
    /// </summary>
    private void Move()
    {
        _currentSpeed = _speed;
        if(IsRunning())
        {
            _currentSpeed = _runSpeed;
        }
        _rb2d.velocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized * _currentSpeed;
    }

    /// <summary>
    /// Determines if the player is running
    /// </summary>
    /// <returns> True if player is holding shift </returns>
    private bool IsRunning()
    {
        return Input.GetKey(KeyCode.LeftShift);
    }
    #endregion
    #endregion
}
