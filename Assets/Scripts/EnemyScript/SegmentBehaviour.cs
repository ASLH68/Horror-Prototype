/*****************************************************************************
// File Name :         SegmentBehaviour.cs
// Author :            Charlie Polonus
// Creation Date :     January 25th, 2023
//
// Brief Description : This script controls the individual segments of the snake
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentBehaviour : MonoBehaviour
{
    private Transform _next;
    private float _size;

    public void Init(Transform next, float size)
    {
        _next = next;
        _size = size;
    }

    void Update()
    {
        Vector3 direction = _next.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.eulerAngles = Vector3.forward * angle;

        float distanceToNext = Vector2.Distance(_next.position, transform.position);

        if (distanceToNext > _size)
        {
            float movementRatio = -(1 / Mathf.Max((distanceToNext - _size + 1), 0.001f)) + 1;
            transform.position = (transform.position * (1 - movementRatio)) + (_next.position * movementRatio);
        }
    }
}
