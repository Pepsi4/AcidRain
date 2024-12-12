using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float TimeValue { get; private set; }
    public Action OnTimerEnd;

    [SerializeField] private PlayerHealth playerHealth;
    private bool isRunning;
    private bool isCountingUp;
   
    public void StartTimer(float initialTime, bool countUp = false)
    {
        TimeValue = initialTime;
        isRunning = true;
        isCountingUp = countUp;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public float GetTimeRemaining()
    {
        return TimeValue;
    }

    protected virtual void OnTimerEndHandler()
    {
        OnTimerEnd?.Invoke();
    }

    private void Update()
    {
        if (isRunning)
        {
            if (isCountingUp)
            {
                TimeValue += Time.deltaTime;
            }
            else
            {
                TimeValue -= Time.deltaTime;
                if (TimeValue <= 0f)
                {
                    TimeValue = 0f;
                    isRunning = false;
                    OnTimerEndHandler();
                }
            }
        }
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
