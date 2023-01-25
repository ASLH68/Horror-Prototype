/*****************************************************************************
// File Name :         CameraFollow.cs
// Author :            Charlie Polonus
// Creation Date :     January 25th, 2023
//
// Brief Description : This script controls the movement of the camera
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform _followTransform;

    [Header("Camera Settings")]
    [SerializeField]
    [Range(0, 1)]
    private float _moveIntensity;
    [SerializeField]
    private Vector3 _cameraOffset;

    void Update()
    {
        Vector3 objPosition = new Vector3(_followTransform.position.x, _followTransform.position.y, -10) + _cameraOffset;

        transform.position = transform.position * (1 - _moveIntensity) + objPosition * _moveIntensity;
    }
}
