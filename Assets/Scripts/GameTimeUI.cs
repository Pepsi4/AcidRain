using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameTimeUI : MonoBehaviour
{
    [SerializeField] private GameTimeTimer gameTimeTimer;
    [SerializeField] private TextMeshProUGUI timeText;
    
    private void FixedUpdate()
    {
        if (gameTimeTimer != null && timeText != null)
        {
            timeText.text = Mathf.FloorToInt(gameTimeTimer.GetTimeRemaining()).ToString();
        }
    }
}
