using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatAnimation : MonoBehaviour
{
    [SerializeField]
    private GameObject[] levelButtons;

    [SerializeField]
    private Animator[] animators;

    [SerializeField]
    private GameObject[] cats;

    private bool isLevelCompleted; // Булевое значение, true - если все первые звезды активны

    void Start()
    {
        // Инициализируем массивы
        animators = new Animator[cats.Length];

        // Для каждого кота
        for (int i = 0; i < cats.Length; i++)
        {
            // Получаем компонент Animator и добавляем его в массив
            animators[i] = cats[i].GetComponent<Animator>();
        }
    }

    void Update()
    {
        // Проверяем, активны ли все первые звезды
        isLevelCompleted = true; // Предполагаем, что все уровни пройдены
        foreach (GameObject levelButton in levelButtons)
        {
            GameObject star = levelButton.transform.Find("StarLeft").gameObject; // Находим объект StarLeft
            if (star != null && !star.activeSelf) // Если звезда существует и не активна
            {
                isLevelCompleted = false; // Уровень не пройден
                break; // Выходим из цикла, так как уже нашли неактивную звезду
            }
        }

        for (int i = 0; i < cats.Length; i++) // Проходим по всем котам
        {
            if (cats[i] != null && animators[i] != null) // Если кот и его аниматор существуют
            {
                Image catImage = cats[i].GetComponent<Image>(); // Получаем компонент Image
                if (!isLevelCompleted) // Если уровень не пройден
                {
                    animators[i].enabled = false; // Останавливаем анимацию
                    if (catImage != null) // Если компонент Image существует
                    {
                        catImage.color = Color.gray; // Красим кота в серый цвет
                    }
                }
                else
                {
                    animators[i].enabled = true; // Включаем анимацию
                    if (catImage != null) // Если компонент Image существует
                    {
                        catImage.color = Color.white; // Возвращаем коту исходный цвет
                    }
                }
            }
        }
    }
}