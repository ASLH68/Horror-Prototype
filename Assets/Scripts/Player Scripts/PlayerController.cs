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
    [Header("Movement Information")]
    [SerializeField] private int _speed = 10;

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    /// <summary>
    /// Moves the player using WASD or arrow keys
    /// </summary>
    private void Move()
    { 
        Vector3 Movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"),0);
        transform.position += Movement * _speed * Time.deltaTime;
    }
}
