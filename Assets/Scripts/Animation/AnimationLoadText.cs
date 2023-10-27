using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AnimationLoadText : MonoBehaviour
{
    public TextMeshProUGUI textComponent; // Ссылка на компонент текста
    public string[] textArray; // Массив текстов
    public float typingSpeed = 0.05f; // Скорость печати
    public float delayBetweenTexts = 2.0f; // Задержка между текстами

    private bool isTyping = false; // Флаг, указывающий, идет ли процесс печати
    private int currentTextIndex = -1; // Индекс текущего текста

    private void Start()
    {
        // Начните анимацию загрузки
        TypeRandomText();
    }



    private void TypeRandomText()
    {
        
            int randomIndex;
            do
            {
                randomIndex = Random.Range(0, textArray.Length);
            } while (randomIndex == currentTextIndex); // Убедитесь, что новый текст не совпадает с предыдущим

            currentTextIndex = randomIndex;

            // Начните печать случайного текста
            StartCoroutine(TypeText(textArray[currentTextIndex]));
        
    }

    IEnumerator TypeText(string text)
    {
        isTyping = true;
        textComponent.text = "";

        for (int charIndex = 0; charIndex < text.Length; charIndex++)
        {
            textComponent.text += text[charIndex];
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }
}
