using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChangeLanguage : MonoBehaviour
{
    public GameObject[] flags; // Массив флагов
    public string[] languages; // Массив языков
    public TextManager textManager; // Ссылка на TextManager

    private Button[] flagButtons; // Кнопки флагов
    private int currentLanguageIndex; // Индекс текущего языка
    private Vector3[] initialPositions; // Начальные позиции кнопок

    void Start()
    {
        // Заполняем массив кнопок и сохраняем начальные позиции
        flagButtons = new Button[flags.Length];
        initialPositions = new Vector3[flags.Length];
        for (int i = 0; i < flags.Length; i++)
        {
            flagButtons[i] = flags[i].GetComponent<Button>();
            initialPositions[i] = flags[i].transform.localPosition;
        }

        if (PlayerPrefs.HasKey("language"))
        {
            string language = PlayerPrefs.GetString("language");
            LangAsset.CurrentLangLocation = (LangLocation)System.Enum.Parse(typeof(LangLocation), language);
            currentLanguageIndex = System.Array.IndexOf(languages, language);
        }
        else
        {
            string language = "En";
            PlayerPrefs.SetString("language", language);
            LangAsset.CurrentLangLocation = LangLocation.En;
            currentLanguageIndex = 0;
        }

        // Если главный флаг не русский, меняем местами позиции главного и русского флагов
        if (currentLanguageIndex != 0)
        {
            Vector3 tempPosition = flags[0].transform.localPosition;
            flags[0].transform.localPosition = flags[currentLanguageIndex].transform.localPosition;
            flags[currentLanguageIndex].transform.localPosition = tempPosition;
        }

        textManager.UpdateTexts(); // Обновляем тексты
    }

    public void OnClick(int index)
    {
        if (index == currentLanguageIndex) 
        {
            SceneManager.LoadScene("DefaultPersonalPage");
            return; // Если выбран уже текущий язык, загружаем сцену и выходим из метода
        }

        // Проверяем, что индекс не выходит за границы массива
        if (currentLanguageIndex >= 0 && currentLanguageIndex < initialPositions.Length)
        {
            // Возвращаем текущий главный флаг на его исходную позицию
            flags[currentLanguageIndex].transform.localPosition = initialPositions[currentLanguageIndex];
        }

        // Если текущий главный флаг - русский, перемещаем его на позицию выбранного флага
        if (currentLanguageIndex == 0)
        {
            flags[0].transform.localPosition = initialPositions[index];
        }
        else if (index != 0) // Если выбранный флаг не русский, перемещаем русский флаг на позицию выбранного флага
        {
            flags[0].transform.localPosition = initialPositions[index];
        }

        // Меняем позиции выбранного флага и главного флага
        flags[index].transform.localPosition = initialPositions[0];

        // Обновляем текущий язык
        currentLanguageIndex = index;
        string language = languages[index];
        PlayerPrefs.SetString("language", language);
        LangLocation newLangLocation = (LangLocation)System.Enum.Parse(typeof(LangLocation), language);
        LangAsset.ChangeLanguage(newLangLocation); // Здесь вызывается событие OnLanguageChanged
    }
}