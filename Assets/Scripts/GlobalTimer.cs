using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalTimer : MonoBehaviour
{
    enum GameSpeed
    {
        slow, medium, fast
    }

    [SerializeField] GameSpeed _gameSpeed;

    void Awake()
    {
        _gameSpeed = GameSpeed.medium;
    }

    void UpdateGameSpeed()
    {
        switch (_gameSpeed)
        {
            case GameSpeed.slow:
                {
                    Time.timeScale = 0.5f;
                    Debug.Log(Time.timeScale);
                    break;
                }
            case GameSpeed.fast:
                {
                    Time.timeScale = 2f;
                    Debug.Log(Time.timeScale);
                    break;
                }
            case GameSpeed.medium:
            default:
                {
                    Time.timeScale = 1f;
                    Debug.Log(Time.timeScale);
                    break;
                }
        }
    }
}
