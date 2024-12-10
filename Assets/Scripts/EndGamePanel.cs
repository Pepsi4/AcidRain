using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Для доступу до Image компонента

public class EndGamePanel : MonoBehaviour
{
    [SerializeField] private float delayBeforeShowing = 2f; // Затримка перед показом
    [SerializeField] private float fadeDuration = 1f; // Час для анімації появи панелі
    [SerializeField] private CanvasGroup canvasGroup; // CanvasGroup для панелі

    private void Start()
    {
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>(); // Перевірка чи є CanvasGroup
        }

        // Початковий стан: панель повністю прозора
        canvasGroup.alpha = 0f;
    }

    public void Show()
    {
        // Викликаємо корутину для анімації з затримкою
        StartCoroutine(ShowWithDelay());
    }

    private IEnumerator ShowWithDelay()
    {
        // Затримка перед анімацією
        yield return new WaitForSeconds(delayBeforeShowing);
        canvasGroup.gameObject.SetActive(true);
        // Анімація з плавним появленням
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            // Лінійне збільшення альфа-каналу
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Гарантуємо, що альфа-канал буде дорівнювати 1 після завершення анімації
        canvasGroup.alpha = 1f;

        // Показуємо повідомлення в консолі
        Debug.Log("End Game Panel Show");
    }
}
