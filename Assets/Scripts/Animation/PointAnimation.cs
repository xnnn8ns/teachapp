using UnityEngine;
using TMPro;
using System.Collections;

public class PointAnimation : MonoBehaviour
{
    public TextMeshProUGUI textComponent; // Ссылка на компонент текста
    public string loadingText ; // Текст загрузки
    public float animationSpeed = 0.5f; // Скорость анимации
    public int maxDots = 3; // Максимальное количество точек

    private int dotCount = 0; // Текущее количество точек
    private float lastUpdateTime;

    private void Start()
    {
        // Начните анимацию загрузки
        lastUpdateTime = Time.time;
    }

    private void Update()
    {
        // Обновите текст с заданной скоростью анимации
        if (Time.time - lastUpdateTime > animationSpeed)
        {
            dotCount = (dotCount + 1) % (maxDots + 1); // Переключаем количество точек с 0 до maxDots
            UpdateLoadingText();
            lastUpdateTime = Time.time;
        }
    }

    private void UpdateLoadingText()
    {
        textComponent.text = loadingText;
        for (int i = 0; i < dotCount; i++)
        {
            textComponent.text += ".";
        }
    }
}

