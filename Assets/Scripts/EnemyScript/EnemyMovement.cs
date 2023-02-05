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
    public enum MovementType { FollowObj, FollowMouse, CircleAroundObj, Static, GoTo, Wander }

    [Header("References")]
    [SerializeField]
    private LighterBehaviour _lighter;

    [Header("Movement Settings")]
    [SerializeField]
    private MovementType _movementType;
    [SerializeField]
    private float _speed;
    [SerializeField]
    private bool _constantSpeed;
    [Space(30)]
    [SerializeField]
    private float _passiveSpeed;
    [SerializeField]
    private float _aggroSpeed;

    [Header("Follow Object")]
    [SerializeField]
    private Transform _followObj;
    private Vector2 _followPos;

    [Header("Circle Around Object")]
    [SerializeField]
    private float _radius;

    [Header("Go To")]
    [SerializeField]
    private Vector2 _targetPos;

    [Header("Internal")]
    private Coroutine _chargeCoroutine;
    private bool _attacking;
    private bool _normalMovement = true;

    public bool Attacking => _attacking;
    public bool endGame = false;

    public void SetMovementType(MovementType type)
    {
        _movementType = type;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_normalMovement)
        {
            if (_lighter.TurnedOn || _lighter.IsSafe)
            {
                if (_chargeCoroutine != null)
                {
                    StopCoroutine(_chargeCoroutine);
                    _attacking = false;
                    _chargeCoroutine = null;
                }
                _speed = _passiveSpeed;
                _movementType = MovementType.CircleAroundObj;
                _constantSpeed = false;
            }
            else if (_chargeCoroutine == null)
            {
                _chargeCoroutine = StartCoroutine(Charge());
            }
        }

        if (_movementType == MovementType.FollowObj)
        {
            _followPos = _followObj.position;
        }
        else if (_movementType == MovementType.FollowMouse)
        {
            _followPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else if (_movementType == MovementType.GoTo)
        {
            _followPos = _targetPos;
        }
        else if (_movementType == MovementType.Static)
        {
            _followPos = transform.position;
        }
        else if (_movementType == MovementType.CircleAroundObj)
        {
            Vector2 circleDir = (transform.position - _followObj.position).normalized;
            float circleAngle = Mathf.Atan2(circleDir.y, circleDir.x) * Mathf.Rad2Deg;
            float nextAngle = (circleAngle + ((10 * _speed) / Mathf.Max(_radius, 1))) % 360;

            _followPos = (Vector2)_followObj.position + AddVector(nextAngle, _radius);
        }
        else if (_movementType == MovementType.Wander)
        {
            if (Vector2.Distance(transform.position, _followPos) < 1)
            {
                float distanceRatio = Mathf.Clamp01(Vector2.Distance(transform.position, _followObj.position) / _radius);
                float newRadius = Mathf.Clamp((-3 * ((distanceRatio + Random.Range(-0.25f, 0.25f)) / 4f) + 1), 0.25f, 1) * _radius;

                Vector2 circleDir = (transform.position - _followObj.position).normalized;
                float circleAngle = Mathf.Atan2(circleDir.y, circleDir.x) * Mathf.Rad2Deg;
                float nextAngle = (circleAngle + ((Random.Range(-45f, 45f) * _speed) / Mathf.Max(_radius, 1))) % 360;

                _followPos = (Vector2)_followObj.position + AddVector(nextAngle, newRadius);
            }
        }

        Vector3 direction = (Vector3)_followPos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.eulerAngles = Vector3.forward * angle;

        float distanceToNext = Vector2.Distance(_followPos, transform.position);

        if (distanceToNext > 0)
        {
            if (_constantSpeed || true)
            {
                transform.position += _speed * Time.deltaTime * direction.normalized;
            }
            else
            {
                float movementRatio = -(1 / Mathf.Max((((distanceToNext * _speed) / 10000) + 1), 0.001f)) + 1;
                transform.position = (transform.position * (1 - movementRatio)) + ((Vector3)_followPos * movementRatio);
            }
        }
    }

    public void EndGame()
    {
        _normalMovement = false;
        StopAllCoroutines();
        StartCoroutine(TriggerEndgame());
    }

    private IEnumerator TriggerEndgame()
    {
        _movementType = MovementType.CircleAroundObj;

        while (_radius > 6)
        {
            _radius -= Time.deltaTime;
            Camera.main.orthographicSize += 0.1f * Time.deltaTime;
            yield return null;
        }
        _lighter.BlowOutLighter(true);
        transform.position += Vector3.up * 200;
        _movementType = MovementType.Static;

        while (!_lighter.TurnedOn)
        {
            yield return null;
        }
        _movementType = MovementType.FollowObj;
        _speed = 50;
        _attacking = true;
        endGame = true;
    }

    private Vector2 AddVector(float angle, float radius)
    {
        return new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * radius;
    }
    
    private IEnumerator Charge()
    {
        _movementType = MovementType.GoTo;

        Vector2 circleDir = (transform.position - _followObj.position).normalized;
        float circleAngle = Mathf.Atan2(circleDir.y, circleDir.x) * Mathf.Rad2Deg;
        _targetPos = (Vector2)_followObj.position + AddVector(circleAngle, 100);

        _speed = _passiveSpeed / 2;

        while (Vector2.Distance(transform.position, _targetPos) > 1)
        {
            yield return null;
        }

        _attacking = true;
        _constantSpeed = true;
        _movementType = MovementType.FollowObj;
        _speed = _aggroSpeed;
    }

    public void Regroup(Vector2 difference)
    {
        foreach (SegmentBehaviour segment in GetComponent<SnakeBehaviour>().segments)
        {
            segment.Regroup(difference);
        }
    }
}
