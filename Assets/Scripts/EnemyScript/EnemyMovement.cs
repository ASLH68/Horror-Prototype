/*****************************************************************************
// File Name :         EnemyMovement.cs
// Author :            Charlie Polonus
// Creation Date :     January 25th, 2023
//
// Brief Description : This script controls the movement of the snake
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private enum MovementType { FollowObj, FollowMouse, CircleAroundObj, Static }

    [Header("Movement Settings")]
    [SerializeField]
    private MovementType _movementType;
    [SerializeField]
    private float _speed;
    [SerializeField]
    private bool _constantSpeed;

    [Header("Follow Object")]
    [SerializeField]
    private Transform _followObj;

    [Header("Circle Around Object")]
    [SerializeField]
    private float _radius;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _movementType = _movementType == MovementType.CircleAroundObj ? MovementType.FollowObj : MovementType.CircleAroundObj;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 followPos = transform.position;

        if (_movementType == MovementType.FollowObj)
        {
            followPos = _followObj.position;
        }
        else if (_movementType == MovementType.FollowMouse)
        {
            followPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (_movementType == MovementType.CircleAroundObj)
        {
            Vector2 circleDir = (transform.position - _followObj.position).normalized;
            float circleAngle = Mathf.Atan2(circleDir.y, circleDir.x) * Mathf.Rad2Deg;
            float nextAngle = (circleAngle + ((10 * _speed) / _radius)) % 360;

            followPos = (Vector2)_followObj.position + AddVector(nextAngle, _radius);
        }

        Vector3 direction = (Vector3)followPos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.eulerAngles = Vector3.forward * angle;

        float distanceToNext = Vector2.Distance(followPos, transform.position);

        if (distanceToNext > 0)
        {
            float movementRatio = -(1 / (((distanceToNext * _speed) / 10000) + 1)) + 1;
            transform.position = (transform.position * (1 - movementRatio)) + ((Vector3)followPos * movementRatio);
        }
    }

    private Vector2 AddVector(float angle, float radius)
    {
        return new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * radius;
    }
}
