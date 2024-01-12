using System.Collections;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip[] audioClips; // Массив для хранения аудиоклипов

    void Awake()
    {
        // Добавляем новый компонент AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.volume = 0.02f;
        StartCoroutine(PlayMusic());

        // Сохраняем объект при переходе между сценами
        DontDestroyOnLoad(gameObject);
    }

    IEnumerator PlayMusic()
    {
        while (true)
        {
            // Выбираем случайный аудиоклип из массива и устанавливаем его в AudioSource
            audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];

            // Начинаем воспроизведение музыки
            audioSource.Play();

            // Ждем окончания воспроизведения музыки
            yield return new WaitForSeconds(audioSource.clip.length);

            // Добавляем задержку перед началом воспроизведения следующего трека
            yield return new WaitForSeconds(2);
        }
    }
}