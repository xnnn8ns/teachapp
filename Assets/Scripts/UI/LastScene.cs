using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LastScene : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject newWindow;

    public void RequestCertClick()
    {
        // делаем panel полностью прозрачным
        Color c = panel.GetComponent<Image>().color;
        c.a = 0f;
        panel.GetComponent<Image>().color = c;
        panel.SetActive(true);
        StartCoroutine(ShowPanel());
    }

    IEnumerator ShowPanel()
    {
        yield return new WaitForSeconds(0.25f);
        // плавное появление панели
        for (float f = 0f; f <= 1f; f += 0.1f)
        {
            Color c = panel.GetComponent<Image>().color;
            c.a = f;
            panel.GetComponent<Image>().color = c;
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0.2f);
        foreach (Transform child in panel.transform)
        {
            // подгрузка текста из Data файла
            child.gameObject.SetActive(true);
            if (child.name == "FIO")
            {
                var textComponent = child.GetComponentInChildren<TMPro.TextMeshProUGUI>();
                if (textComponent != null)
                {
                    textComponent.text = UserData.UserFullName;
                }
            }
            else if (child.name == "Email")
            {
                var textComponent = child.GetComponentInChildren<TMPro.TextMeshProUGUI>();
                if (textComponent != null)
                {
                    textComponent.text = UserData.UserEmail;
                }
            }
        }
    }

    public void AllCorrectClick()
    {
        StartCoroutine(AllCorrectClickCoroutine());
        // код для сохранения данных о пользователе, который создал запрос на сертификат
        // и обработка этого запроса
    }

    IEnumerator AllCorrectClickCoroutine()
    {
        StartCoroutine(HidePanel());

        yield return new WaitForSeconds(2f); // задержка перед деактивацией панели и активацией newWindow

        // делаем newWindow полностью прозрачным
        Color c = newWindow.GetComponent<Image>().color;
        c.a = 0f;
        newWindow.GetComponent<Image>().color = c;

        newWindow.SetActive(true); // активируем newWindow
        StartCoroutine(ShowAndHideWindow());
    }

    IEnumerator HidePanel()
    {
        // делаем неактивными все дочерние объекты панели
        foreach (Transform child in panel.transform)
        {
            child.gameObject.SetActive(false);
        }
        yield return new WaitForSeconds(0.1f);

        // плавное исчезновение панели
        for (float f = 1f; f >= 0; f -= 0.1f)
        {
            Color c = panel.GetComponent<Image>().color;
            c.a = f;
            panel.GetComponent<Image>().color = c;
            yield return new WaitForSeconds(0.1f);
        }
        panel.SetActive(false); // деактивируем объект panel
    }

    IEnumerator ShowAndHideWindow()
    {
        // плавное появление окна
        for (float f = 0f; f <= 1; f += 0.1f)
        {
            Color c = newWindow.GetComponent<Image>().color;
            c.a = f;
            newWindow.GetComponent<Image>().color = c;
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(0.2f);

        // делаем активными все дочерние объекты нового окна
        foreach (Transform child in newWindow.transform)
        {
            child.gameObject.SetActive(true);
        }

        // ждем 2 секунды
        yield return new WaitForSeconds(2);

        // делаем неактивными все дочерние объекты нового окна
        foreach (Transform child in newWindow.transform)
        {
            child.gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(0.1f);

        // плавное исчезновение окна
        for (float f = 1f; f >= 0; f -= 0.1f)
        {
            Color c = newWindow.GetComponent<Image>().color;
            c.a = f;
            newWindow.GetComponent<Image>().color = c;
            yield return new WaitForSeconds(0.1f);
        }

        newWindow.SetActive(false); // деактивируем объект newWindow
    }
}