/*****************************************************************************
// File Name :         LighterBehaviour.cs
// Author :            Charlie Polonus, Andrea Swihart-DeCoster
// Creation Date :     January 25th, 2023
//
// Brief Description : This script controls the enabling/disabling of the lighter
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LighterBehaviour : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Rigidbody2D _plrRb2d;
    private Light2D _light;
    [SerializeField] PlayerController _pc;

    [Header("General Info")]
    private bool _turnedOn;
    [SerializeField]
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
    public float FuelAddAmount;

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
    [SerializeField]
    private AnimationCurve _lighterRatio;

    [Header("Internal")]
    private float _targetSize;
    private Coroutine _lightCoroutine;
    private bool _isSafe;
    private GameObject[] _staticLights;

    public bool OutOfFuel => _fuelAmount <= 0;
    public bool TurnedOn => _turnedOn;
    public float FuelAmount => _fuelAmount;
    public bool IsSafe => _isSafe;

    // Start is called before the first frame update
    void Start()
    {
        _light = GetComponent<Light2D>();

        _fuelAmount = 0f;
        _targetSize = 0;
        _turnedOn = false;

        _flickFailChance = 0;
        _lightCoroutine = StartCoroutine(FlickLighter());
        _flickFailChance = 0.1f;

        _staticLights = GameObject.FindGameObjectsWithTag("SafeLight");
    }

    // Update is called once per frame
    void Update()
    {
        if (_plrRb2d.velocity.magnitude > _maxMoveSpeed)
        {
            if (Random.value > 0.9975f)
            {
                BlowOutLighter();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
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
            else if(!_pc.IsRunning())
            {
                _lightCoroutine = StartCoroutine(FlickLighter());
            }
        }

        bool isSafe = false;
        foreach (GameObject curObj in _staticLights)
        {
            Light2D light = curObj.GetComponent<Light2D>();
            if (curObj.GetComponent<CircleCollider2D>().OverlapPoint(transform.position))
            {
                isSafe = true;
                break;
            }
        }
        _isSafe = isSafe;
    }

    void FixedUpdate()
    {
        float sizeRatio = -Mathf.Pow(2, Mathf.Pow(Mathf.Clamp01(1 - _lighterRatio.Evaluate(_fuelAmount)), _sizeShrinkRate)) + 2;
        float flickerSizeRatio = Random.Range(_flickerSize.x, _flickerSize.y);

        _light.pointLightOuterRadius = ((_targetSize * _lightSpeed) + (_light.pointLightOuterRadius * (1 - _lightSpeed))) * sizeRatio * flickerSizeRatio;
    }

    public void AddFuel(float fuelAmount)
    {
        if (_fuelAmount < 1)
        {
            _fuelAmount += fuelAmount;
        }
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
                    _fuelAmount -= _passiveFuelCost * (_fuelAmount > 0.25f ? 1 : 0.5f);
                    yield return new WaitForSeconds(1);
                }

            }
            _turnedOn = false;
            _targetSize = 0;
        }
    }

    public void BlowOutLighter(bool instant = false)
    {
        if (_lightCoroutine != null)
        {
            StopCoroutine(_lightCoroutine);
        }

        if (instant)
        {
            _light.pointLightOuterRadius = 0;
        }

        transform.localScale = Vector2.zero;
        _targetSize = 0;
        _turnedOn = false;
    }
}
