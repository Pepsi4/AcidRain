using UnityEngine;

public class GameTimeTimer : Timer
{
    private void Start()
    {
        StartTimer(0f, countUp: true);
    }
}
