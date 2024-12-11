using UnityEngine;

public class ShieldTimer : Timer
{
    public void Activate(float duration)
    {
        StartTimer(duration, countUp: false);
    }

    protected override void OnTimerEndHandler()
    {
        base.OnTimerEnd();
        Debug.Log("Shield has expired.");
    }
}
