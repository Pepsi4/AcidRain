using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartGameTransition : MonoBehaviour
{
    [Header("Transition Settings")]
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private string gameplaySceneName = "Gameplay";

    private bool isTransitioning = false;

    public void OnStartGameButtonPressed()
    {
        if (!isTransitioning)
        {
            StartCoroutine(TransitionToGameplay());
        }
    }

    private System.Collections.IEnumerator TransitionToGameplay()
    {
        isTransitioning = true;

        Color color = fadeImage.color;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        SceneManager.LoadScene(gameplaySceneName);
    }
}
