using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SceneSound
{
    public string sceneName;
    public AudioClip clip;
    public bool loop;
    public bool randomize;
    public AudioClip[] additionalClips;
}

public class PlaySoundWhenStartingScene : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField]
    private SceneSound[] sceneSounds;
    private Dictionary<string, SceneSound> sceneSoundDictionary;
    private static string currentSceneName;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        sceneSoundDictionary = new Dictionary<string, SceneSound>();
        foreach (SceneSound sceneSound in sceneSounds)
        {
            sceneSoundDictionary[sceneSound.sceneName] = sceneSound;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentSceneName = scene.name;
        StartCoroutine(PlaySoundAfterDelay(0.25f));
    }

    IEnumerator PlaySoundAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (sceneSoundDictionary.TryGetValue(currentSceneName, out SceneSound sceneSound))
        {
            audioSource.Stop();
            if (sceneSound.randomize && sceneSound.additionalClips.Length > 0)
            {
                audioSource.clip = sceneSound.additionalClips[Random.Range(0, sceneSound.additionalClips.Length)];
            }
            else
            {
                audioSource.clip = sceneSound.clip;
            }
            audioSource.pitch = 1.1f;
            audioSource.volume = UserData.SoundEnabled ? 0.8f : 0; // Используем настройки звука (вкл или выкл из userData)
            audioSource.loop = sceneSound.loop;
            audioSource.Play();
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}