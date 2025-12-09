using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [Header("Настройки сцен")]
    public string firstSceneName;   // Имя первой сцены для загрузки
    public string nextSceneName;    // Имя следующей сцены
    public float delayBeforeNextScene = 5f; // Время до смены сцены (в секундах)

    [Header("Музыка")]
    public AudioSource musicSource; // Ссылка на AudioSource с музыкой

    private void Awake()
    {
        // Делаем так, чтобы объект с музыкой не уничтожался при смене сцен
        if (musicSource != null)
        {
            DontDestroyOnLoad(musicSource.gameObject);

            if (!musicSource.isPlaying)
                musicSource.Play();
        }
    }

    private void Start()
    {
        // Загружаем первую сцену
        if (!string.IsNullOrEmpty(firstSceneName))
        {
            SceneManager.LoadScene(firstSceneName);
        }

        // Запускаем смену сцены через заданное время
        Invoke(nameof(LoadNextScene), delayBeforeNextScene);
    }

    private void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
