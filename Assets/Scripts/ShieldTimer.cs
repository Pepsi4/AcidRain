using UnityEngine;

public class ShieldTimer : Timer
{
    [SerializeField] private GameObject shieldObject;

    public void ActivateShield(float duration)
    {
        shieldObject.SetActive(true);
        StartTimer(duration, countUp: false);
    }

    protected override void OnTimerEndHandler()
    {
        base.OnTimerEnd();
        shieldObject.SetActive(false);
        Debug.Log("Shield has expired.");
    }
}
