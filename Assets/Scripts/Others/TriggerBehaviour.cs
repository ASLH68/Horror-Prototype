using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBehaviour : MonoBehaviour
{
    [Header("References")]
    private EnemyMovement _snake;
    private PlayerController _player;
    private LighterBehaviour _lighter;

    [Header("General")]
    [SerializeField]
    private float _radius;
    [SerializeField]
    private int _triggerTimes;
    [SerializeField]
    private int _curTriggerTimes;
    [SerializeField]
    private FakeSnakeMovement _fakeSnake;
    [SerializeField]
    private GameObject _destroyObj;

    [Header("Specifics")]
    [SerializeField]
    private bool _blowOutLighter;
    [SerializeField]
    private Vector2 _movePlayerEnemy;

    private void Start()
    {
        _snake = FindObjectOfType<EnemyMovement>();
        _player = FindObjectOfType<PlayerController>();
        _lighter = FindObjectOfType<LighterBehaviour>();
    }

    private void Trigger()
    {
        if (_triggerTimes != -1)
        {
            if (_curTriggerTimes == _triggerTimes)
            {
                return;
            }
            _curTriggerTimes++;
        }

        if (_blowOutLighter)
        {
            _lighter.BlowOutLighter();
        }
        if (_movePlayerEnemy != Vector2.zero)
        {
            _player.transform.position += (Vector3)_movePlayerEnemy;
            _snake.transform.position += (Vector3)_movePlayerEnemy;
        }

        if (_fakeSnake != null)
        {
            _fakeSnake.Activated = true;
        }
    }

    void Update()
    {
        if (Vector2.Distance(_player.transform.position, transform.position) <= _radius)
        {
            Trigger();
        }
    }
}
