using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class ButtonClickSound : MonoBehaviour
{
    public AudioClip clickSound; // Звук клика
    public Button[] buttons; // Массив кнопок
    private AudioSource audioSource;
    private Dictionary<Button, bool> isSoundAdded = new Dictionary<Button, bool>(); // новый словарь для отслеживания, был ли добавлен звук для каждой кнопки

    void Awake()
    {
        // Проверяем, есть ли уже AudioListener в сцене
        if (FindObjectOfType<AudioListener>() == null)
        {
            // Если нет, то добавляем его к главной камере
            Camera.main.gameObject.AddComponent<AudioListener>();
        }
        
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
            audioSource.volume = UserData.SoundEnabled ? 0.5f : 0;
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

    void AddSoundToButton(Button button)
    {
        if (!isSoundAdded.ContainsKey(button) || !isSoundAdded[button])
        {
            button.onClick.AddListener(() => OnButtonClick(button));
            isSoundAdded[button] = true; // обновляем словарь, указывая, что звук был добавлен
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // при загрузке сцены снова добавляем обработчик событий для каждой кнопки
        buttons = FindObjectsOfType<Button>();
        foreach (Button button in buttons)
        {
            AddSoundToButton(button);
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
            AddSoundToButton(button);
        }
    }

    IEnumerator PlaySoundWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (UserData.SoundEnabled)
        {
            audioSource.PlayOneShot(clickSound);
        }
        if (UserData.VibrationEnabled)
        {
            Vibration.VibratePop();
        }
    }
}