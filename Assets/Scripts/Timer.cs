using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    private float timeRemaining;
    private bool isRunning;
    private bool isCountingUp;
    public Action OnTimerEnd;
    public void StartTimer(float initialTime, bool countUp = false)
    {
        timeRemaining = initialTime;
        isRunning = true;
        isCountingUp = countUp;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    // Оновлення таймера
    private void Update()
    {
        if (isRunning)
        {
            if (isCountingUp)
            {
                timeRemaining += Time.deltaTime;
            }
            else
            {
                timeRemaining -= Time.deltaTime;
                if (timeRemaining <= 0f)
                {
                    timeRemaining = 0f;
                    isRunning = false;
                    OnTimerEndHandler();
                }
            }
        }
    }

    public float GetTimeRemaining()
    {
        return timeRemaining;
    }

    protected virtual void OnTimerEndHandler()
    {
        OnTimerEnd?.Invoke();
    }

    private void OnEnable()
    {
        playerHealth.OnDie += OnPlayerDieHandler;
    }

    private void OnDisable()
    {
        playerHealth.OnDie -= OnPlayerDieHandler;
    }

    private void OnPlayerDieHandler()
    {
        StopTimer();
        this.enabled = false;
    }
}
