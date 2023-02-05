/*****************************************************************************
// File Name :         SnakeBehaviour.cs
// Author :            Charlie Polonus
// Creation Date :     January 25th, 2023
//
// Brief Description : This script controls the head of the snake
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBehaviour : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Transform _segmentObj;
    [SerializeField]
    private Sprite _tailSprite;

    [Header("Snake Settings")]
    [SerializeField]
    private int _segmentCount;
    [SerializeField]
    private float _segmentSize;

    public List<SegmentBehaviour> segments;

    // Start is called before the first frame update
    void Start()
    {
        segments = new();
        CreateSnake();
    }

    private void CreateSnake()
    {
        Transform curNext = transform;
        Transform curInit;
        for (int i = 0; i < _segmentCount; i++)
        {
            curInit = Instantiate(_segmentObj, transform.position, Quaternion.identity);
            segments.Add(curInit.GetComponent<SegmentBehaviour>());
            curInit.GetComponent<SegmentBehaviour>().Init(curNext, _segmentSize);
            curNext = curInit;

            if (i == _segmentCount - 1)
            {
                curInit.GetComponent<SpriteRenderer>().sprite = _tailSprite;
            }
        }
    }
}
