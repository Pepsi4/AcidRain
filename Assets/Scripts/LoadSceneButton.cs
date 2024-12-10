using UnityEngine;
using UnityEngine.SceneManagement; // Для роботи зі сценами
using UnityEngine.UI; // Для роботи з кнопками

[RequireComponent(typeof(Button))] // Перевірка наявності компонента Button
public class LoadSceneButton : MonoBehaviour
{
    [SerializeField] private string sceneName; // Назва сцени для завантаження

    private void Start()
    {
        // Отримуємо компонент Button і додаємо обробник події натискання
        Button button = GetComponent<Button>();
        button.onClick.AddListener(LoadScene);
    }

    private void LoadScene()
    {
        // Завантажуємо сцену по імені
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("Scene name is empty or null!");
        }
    }
}
