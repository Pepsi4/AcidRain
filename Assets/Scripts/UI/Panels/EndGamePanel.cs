using System.Collections;
using System.Text;
using UnityEngine;

public class EndGamePanel : MonoBehaviour
{
    [SerializeField] private float delayBeforeShowing = 2f;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TMPro.TextMeshProUGUI bestScoreText;
    [SerializeField] private TMPro.TextMeshProUGUI currentScoreText;
    [SerializeField] private GameTimeTimer gameTimeTimer;

    private const string BestScoreKey = "BestScore";

    private void Start()
    {
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        canvasGroup.alpha = 0f;

        if (bestScoreText != null)
        {
            int bestScore = PlayerPrefs.GetInt(BestScoreKey, 0);
            bestScoreText.text = BuildScoreText("Best Score: ", bestScore);
        }
    }

    public void Show()
    {
        int currentScore = Mathf.FloorToInt(gameTimeTimer.TimeValue);
        UpdateBestScore(currentScore);

        if (currentScoreText != null)
        {
            currentScoreText.text = BuildScoreText("Your Score: ", currentScore);
        }

        StartCoroutine(ShowWithDelay());
    }

    private IEnumerator ShowWithDelay()
    {
        yield return new WaitForSeconds(delayBeforeShowing);
        canvasGroup.gameObject.SetActive(true);
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }

    private void UpdateBestScore(int currentScore)
    {
        int bestScore = PlayerPrefs.GetInt(BestScoreKey, 0);

        if (currentScore > bestScore)
        {
            PlayerPrefs.SetInt(BestScoreKey, currentScore);
            PlayerPrefs.Save();
        }

        if (bestScoreText != null)
        {
            bestScoreText.text = BuildScoreText("Best Score: ", Mathf.Max(bestScore, currentScore));
        }
    }

    private string BuildScoreText(string prefix, int score)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append(prefix);
        stringBuilder.Append(score.ToString());
        return stringBuilder.ToString();
    }
}
