using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ButtonClickSound : MonoBehaviour
{
    public AudioClip clickSound; // Звук клика
    public Button[] buttons; // Массив кнопок
    private AudioSource audioSource;

    void Awake()
    {
        // добавляем канвасу AudioSource, если он отсутствует
        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas != null)
        {
            if (!canvas.TryGetComponent<AudioSource>(out audioSource))
            {
                audioSource = canvas.gameObject.AddComponent<AudioSource>();
            }
        }
        else
        {
            Debug.LogError("Canvas not found");
        }

        GameObject soundObject = GameObject.Find("StartGameObject");
        if (soundObject != null)
        {
            if (!soundObject.TryGetComponent<AudioSource>(out audioSource))
            {
                audioSource = soundObject.AddComponent<AudioSource>();
            }
            audioSource.clip = clickSound;
            if (clickSound == null)
            {
                Debug.LogError("Click sound is null");
            }
            audioSource.volume = 0.5f; // Установка громкости в 50%
            StartCoroutine(LoadAudioAndSetupButtons()); // Предварительная загрузка звукового клипа
        }
        else
        {
            Debug.LogError("DontDestroyOnLoad object not found");
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void OnButtonClick(Button button)
    {
        if (audioSource != null && this != null)
        {
            StartCoroutine(PlaySoundWithDelay(0.1f));
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // при загрузке сцены снова добавляем обработчик событий для каждой кнопки
        buttons = FindObjectsOfType<Button>();
        foreach (Button button in buttons)
        {
            button.onClick.AddListener(() => OnButtonClick(button));
        }
    }

    IEnumerator LoadAudioAndSetupButtons()
    {
        audioSource.clip.LoadAudioData();
        while (audioSource.clip.loadState != AudioDataLoadState.Loaded)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.1f); // Добавляем небольшую задержку

        // Добавляем обработчик событий для каждой кнопки
        foreach (Button button in buttons)
        {
            button.onClick.AddListener(() => OnButtonClick(button));
        }
    }
    IEnumerator PlaySoundWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        audioSource.Play();
    }
}