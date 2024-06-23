using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlAnimForBar : MonoBehaviour
{
    private GameObject[] childObjects;
    public QuestionInitializer otherScript; // ссылка на другой скрипт
    private bool isAnimationPlaying = false; // флаг, указывающий, играет ли анимация

    void Start()
    {
        childObjects = new GameObject[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            childObjects[i] = transform.GetChild(i).gameObject;
        }
    }

    // проверка для запуска анимации (проверяется при нажатии на кнопку)
    public void CheckAndPlayAnimation()
    {
        if (otherScript.NeedPlayAnimBar && !isAnimationPlaying)
        {
            StartCoroutine(PlayAnimation());
        }
    }

    IEnumerator PlayAnimation()
    {
        isAnimationPlaying = true;
        GameObject selectedObject = childObjects[Random.Range(0, childObjects.Length)];
        selectedObject.SetActive(true);
        yield return new WaitForSeconds(1.52f);
        selectedObject.SetActive(false);
        isAnimationPlaying = false;

        // Установить NeedPlayAnimBar в false после завершения анимации
        otherScript.NeedPlayAnimBar = false;
    }
}