/*****************************************************************************
// File Name :         LighterBehaviour.cs
// Author :            Charlie Polonus
// Creation Date :     January 25th, 2023
//
// Brief Description : This script controls the enabling/disabling of the lighter
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LighterBehaviour : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Rigidbody2D _plrRb2d;

    [Header("General Info")]
    private bool _turnedOn;
    private float _fuelAmount;

    [Header("Lighter Settings")]
    [SerializeField]
    [Tooltip("The percentage of fuel to drain each second")]
    [Range(0, 1)]
    private float _passiveFuelCost;
    [SerializeField]
    [Tooltip("The percentage of fuel to use each time the lighter is flicked")]
    [Range(0, 1)]
    private float _flickFuelCost;
    [SerializeField]
    [Tooltip("The percentage chance that the lighter fails to light")]
    [Range(0, 1)]
    private float _flickFailChance;
    [SerializeField]
    private float _maxMoveSpeed;

    [Header("Visual Settings")]
    [SerializeField]
    private float _lightSize;
    [SerializeField]
    private float _flickSize;
    [SerializeField] [Range(0, 1)]
    private float _lightSpeed;
    [SerializeField]
    private Vector2 _flickerSize;
    [SerializeField]
    private float _sizeShrinkRate;

    [Header("Internal")]
    private float _targetSize;
    private Coroutine _lightCoroutine;

    public bool OutOfFuel => _fuelAmount <= 0;
    public bool TurnedOn => _turnedOn;

    // Start is called before the first frame update
    void Start()
    {
        _fuelAmount = 1f;
        _targetSize = 0;
        _turnedOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_plrRb2d.velocity.magnitude > _maxMoveSpeed)
        {
            if (_lightCoroutine != null)
            {
                StopCoroutine(_lightCoroutine);
            }
            transform.localScale = Vector2.zero;
            _targetSize = 0;
            _turnedOn = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (_lightCoroutine != null)
            {
                StopCoroutine(_lightCoroutine);
            }

            if (_turnedOn)
            {
                _targetSize = 0;
                _turnedOn = false;
            }
            else
            {
                _lightCoroutine = StartCoroutine(FlickLighter());
            }
        }
    }

    void FixedUpdate()
    {
        float sizeRatio = -Mathf.Pow(2, Mathf.Pow(Mathf.Clamp01(1 - _fuelAmount), _sizeShrinkRate)) + 2;
        float flickerSizeRatio = Random.Range(_flickerSize.x, _flickerSize.y);

        transform.localScale = ((_targetSize * _lightSpeed * Vector3.one) + (transform.localScale * (1 - _lightSpeed))) * sizeRatio * flickerSizeRatio;
    }

    private IEnumerator FlickLighter()
    {
        if (_fuelAmount > 0)
        {
            _turnedOn = true;
            _fuelAmount -= _flickFuelCost;

            _targetSize = _flickSize;
            yield return new WaitForSeconds(0.1f);

            if (Random.Range(0f, 1f) > _flickFailChance)
            {
                _targetSize = _lightSize;
                while (_fuelAmount > 0)
                {
                    _fuelAmount -= _passiveFuelCost;
                    yield return new WaitForSeconds(1);
                }

            }
            _turnedOn = false;
            _targetSize = 0;
        }
    }
}
