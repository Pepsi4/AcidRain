using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    [SerializeField] private Timer timer;
    [SerializeField] private TextMeshProUGUI timeText;
    
    private void FixedUpdate()
    {
        if (timer != null && timeText != null)
        {
            timeText.text = Mathf.FloorToInt(timer.GetTimeRemaining()).ToString();
        }
    }
}
