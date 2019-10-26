using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalTimer : MonoBehaviour
{
    enum GameSpeed
    {
        slow, medium, fast
    }

    float _time;
    GameSpeed _gameSpeed;
    public static bool s_nextTick;

    void Awake()
    {
        _time = 0f;
        _gameSpeed = GameSpeed.medium;
    }

    void Update()
    {
        _time += Time.deltaTime;
        switch (_gameSpeed)
        {
            case GameSpeed.slow:
                {
                    if (_time >= 5)
                    {
                        s_nextTick = true;
                        _time = 0;
                    }
                    break;
                }
            case GameSpeed.fast:
                {
                    if (_time >= 1)
                    {
                        s_nextTick = true;
                        _time = 0;
                    }
                    break;
                }
            case GameSpeed.medium:
            default:
                {
                    if (_time >= 3)
                    {
                        s_nextTick = true;
                        _time = 0;
                    }
                    break;
                }
        }
    }
}
