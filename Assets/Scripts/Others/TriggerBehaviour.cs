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
    [SerializeField]
    private int _switchMusic;

    [Header("Specifics")]
    [SerializeField]
    private bool _blowOutLighter;
    [SerializeField]
    private Vector2 _movePlayerEnemy;
    [SerializeField]
    private bool _endGame;

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

        if (_switchMusic == -1)
        {
            StartCoroutine(StopMusic());
        }
        else if (_switchMusic != 0)
        {
            StartCoroutine(MusicSwitch(_switchMusic - 1, 4));
        }

        if (_endGame)
        {
            _snake.EndGame();
        }
    }

    void Update()
    {
        if (Vector2.Distance(_player.transform.position, transform.position) <= _radius)
        {
            Trigger();
        }
    }

    private IEnumerator MusicSwitch(int newSongNum, float time)
    {
        AudioSource newSong = _player.GetComponents<AudioSource>()[newSongNum];
        AudioSource oldSong = _player.GetComponents<AudioSource>()[newSongNum == 0 ? 1 : 0];

        float curTime = 0;
        while (curTime < time)
        {
            oldSong.volume = Mathf.Clamp01(Mathf.Min((1 - (curTime / time)) * 0.15f, oldSong.volume));
            newSong.volume = (curTime / time) * 0.15f;

            curTime = Mathf.Min(curTime + Time.deltaTime, time);
            yield return null;
        }
        oldSong.volume = 0;
    }

    private IEnumerator StopMusic()
    {
        AudioSource curSong = _player.GetComponents<AudioSource>()[1];

        float curTime = 0;
        while (curTime < 10)
        {
            curSong.volume = (1 - (curTime / 10)) * 0.15f;

            curTime = Mathf.Min(curTime + Time.deltaTime, 10);
            yield return null;
        }
        curSong.volume = 0;
    }
}
