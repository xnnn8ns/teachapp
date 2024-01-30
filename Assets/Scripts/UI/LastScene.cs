using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class LastScene : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject newWindow;
    [SerializeField] private GameObject alreadyRequestedWindow;
    [SerializeField] private Button requestCertButton;
    [SerializeField] private Button certLinkButton;
    [SerializeField] private GameObject errorWindow;
    [SerializeField] private GetCertificateFromServer certificateFromServer;
    [SerializeField] private TextMeshProUGUI buttonName;
    private bool hasRequestedCertificate = false;

    
    private void Start()
    {
        if (PlayerPrefs.GetInt("HasRequestedCertificate", 0) == 1)
        {
            requestCertButton.gameObject.SetActive(false);
            certLinkButton.gameObject.SetActive(true);
            string url = PlayerPrefs.GetString("Certificate1", "");
            if (url.Length > 8)
            {
                buttonName.text = LangAsset.GetValueByKey("RefPDF");
                certLinkButton.onClick.AddListener(() => Application.OpenURL(url));
            }
        }
    }

    public void RequestCertClick()
    {
        if (hasRequestedCertificate)
        {
            alreadyRequestedWindow.SetActive(true);
            StartCoroutine(HideAlreadyRequestedWindow());
        }
        else
        {
            // делаем panel полностью прозрачным
            Color c = panel.GetComponent<Image>().color;
            c.a = 0f;
            panel.GetComponent<Image>().color = c;
            panel.SetActive(true);
            StartCoroutine(ShowPanel());
        }
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
        }
    }

    IEnumerator HideAlreadyRequestedWindow()
    {
        yield return new WaitForSeconds(2f); // задержка перед деактивацией окна
        alreadyRequestedWindow.SetActive(false);
    }

    public void AllCorrectClick()
    {
        Debug.Log("AllCorrectClick");
        if (string.IsNullOrEmpty(UserData.UserFullName))
        {
            errorWindow.SetActive(true); // активируем окно ошибки
            return;
        }

        hasRequestedCertificate = true;
        StartCoroutine(AllCorrectClickCoroutine());
        certificateFromServer.GetHtmlFile(CertificateReady);
        Debug.Log("GetHtmlFile");
        // код для обработки запроса пользователя на сертификат
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

    void OnApplicationQuit()
    {
        if (hasRequestedCertificate)
        {
            PlayerPrefs.SetInt("HasRequestedCertificate", 1);
        }
    }

    public void CloseErrorWindow()
    {
        errorWindow.SetActive(false);
    }

    public void CertificateReady()
    {
        PlayerPrefs.SetInt("HasRequestedCertificate", 1);
        hasRequestedCertificate = true;
        requestCertButton.gameObject.SetActive(false);
        certLinkButton.gameObject.SetActive(true);
        string url = PlayerPrefs.GetString("Certificate1", "");
        if (url.Length > 8)
        {
            buttonName.text = LangAsset.GetValueByKey("RefPDF");
            certLinkButton.onClick.AddListener(() => Application.OpenURL(url));
        }
    }
}