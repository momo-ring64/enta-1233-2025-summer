using System;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    private PlayerHUD playerHUD;
    public static GameTimer Instance { get; private set; }
    public float RemainingTime { get; private set; }
    public Action<string> OnTimerFinished;
    private bool _isRunning = true;


    public void SetPlayerHUD(PlayerHUD hud)
    {
        playerHUD = hud;
    }

    private void Awake()
    {
        Instance = this;
    }

    public void SetHUD(PlayerHUD hud)
    {
        playerHUD = hud;
    }

    public void SetTime(float time)
    {
        RemainingTime = time;
        _isRunning = true;
    }

    public void StopTimer()
    {
        _isRunning = false;
    }

    private void Update()
    {
        if (!_isRunning) return;

        RemainingTime -= Time.deltaTime;

        if (RemainingTime <= 0)
        {
            RemainingTime = 0;
            _isRunning = false;
            OnTimerFinished?.Invoke(FormatTime(RemainingTime));
        }

        if (playerHUD != null)
            playerHUD.TimeUpdated(FormatTime(RemainingTime));
    }

    public string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return $"{minutes:00}:{seconds:00}";
    }
}
