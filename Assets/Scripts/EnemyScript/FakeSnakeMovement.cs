using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeSnakeMovement : MonoBehaviour
{
    [SerializeField]
    private Vector2 _targetPos;

    [SerializeField]
    private float _speed;
    [SerializeField]
    private bool _constantSpeed;
    [SerializeField]
    private bool _activated;

    public bool Activated { get => _activated; set => _activated = value; }

    void FixedUpdate()
    {
        Vector3 direction = (Vector3)_targetPos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.eulerAngles = Vector3.forward * angle;

        float distanceToNext = Vector2.Distance(_targetPos, transform.position);

        if (distanceToNext > _speed * Time.deltaTime && _activated)
        {
            if (_constantSpeed || true)
            {
                transform.position += _speed * Time.deltaTime * direction.normalized;
            }
            else
            {
                float movementRatio = -(1 / Mathf.Max((((distanceToNext * _speed) / 10000) + 1), 0.001f)) + 1;
                transform.position = (transform.position * (1 - movementRatio)) + ((Vector3)_targetPos * movementRatio);
            }
        }
    }
}
