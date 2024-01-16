using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private GameObject transitionPrefab;
    private GameObject transitionInstance;
    private Image transitionImage;
    private Image catImage;
    public Image TransitionImage
    {
        get { return transitionImage; }
    }

    public Image CatImage
    {
        get { return catImage; }
    }

    void Awake()
    {
        StartTransition();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (transitionImage != null && catImage != null)
        {
            Color newColor = new Color(transitionImage.color.r, transitionImage.color.g, transitionImage.color.b, 1f);
            transitionImage.color = newColor;
            catImage.color = newColor;
        }
        StartCoroutine(FadeOut());
    }

    public void StartTransition()
    {
        if (transitionInstance != null)
        {
            Destroy(transitionInstance);
        }
        transitionInstance = Instantiate(transitionPrefab);
        DontDestroyOnLoad(transitionInstance); 
        transitionImage = transitionInstance.transform.Find("Panel").GetComponent<Image>();
        catImage = transitionInstance.transform.Find("Panel/Cat").GetComponent<Image>();
        transitionImage.GetComponentInParent<Canvas>().enabled = true;
    }

    IEnumerator FadeIn()
    {
        for (float t = 0.01f; t < 1f; t += Time.deltaTime)
        {
            if (transitionImage == null || catImage == null)
            {
                yield break;
            }
            Color newColor = new Color(transitionImage.color.r, transitionImage.color.g, transitionImage.color.b, Mathf.Lerp(0f, 1f, t));
            transitionImage.color = newColor;
            catImage.color = newColor;
            yield return null;
        }
    }

    IEnumerator FadeOut()
    {
        for (float t = 0.01f; t < 1f; t += Time.deltaTime)
        {
            if (transitionImage == null || catImage == null)
            {
                yield break;
            }
            Color newColor = new Color(transitionImage.color.r, transitionImage.color.g, transitionImage.color.b, Mathf.Lerp(1f, 0f, t));
            transitionImage.color = newColor;
            catImage.color = newColor;
            yield return null;
        }

        transitionImage.GetComponentInParent<Canvas>().enabled = false;
    }

    public void StartSceneTransition(string sceneName)
    {
        StartTransition();
        StartCoroutine(TransitionToScene(sceneName));
    }

    IEnumerator TransitionToScene(string sceneName)
    {
        StartCoroutine(FadeIn());
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(sceneName);
    }
}