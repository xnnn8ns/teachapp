using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LastLevel : MonoBehaviour
{
    public static bool IsLastLevelCompleted = false;

    void Awake()
    {
        // Получить все дочерние объекты
        List<GameObject> levelBlocks = new List<GameObject>();
        foreach (Transform child in transform)
        {
            if (child.name.Contains("Lvl-"))
            {
                levelBlocks.Add(child.gameObject);
            }
        }

        // Отсортировать их по положению по оси Y
        levelBlocks.Sort((a, b) => (int)(b.transform.position.y - a.transform.position.y));

        // Получить последний объект
        GameObject lastLevelBlock = levelBlocks[0];

        // Найти все дочерние объекты с именем "LevelButtons(Clone)"
        List<GameObject> levels = new List<GameObject>();
        foreach (Transform child in lastLevelBlock.transform)
        {
            if (child.name.Contains("LevelButtons(Clone)"))
            {
                levels.Add(child.gameObject);
            }
        }

        // Отсортировать их по положению по оси Y
        levels.Sort((a, b) => (int)(a.transform.position.y - b.transform.position.y));

        // Получить последний уровень
        GameObject lastLevel = levels[0];

        // глобальная булевая переменная для хранения данных о прохождение последнего уровня
        IsLastLevelCompleted = IsLevelCompleted(lastLevel);
    }

    bool IsLevelCompleted(GameObject level)
    {
        // Проверить, активен ли компонент "StarLeft"
        Transform child = level.transform.Find("StarLeft");
        if (child != null)
        {
            return child.gameObject.activeSelf;
        }

        return false;
    }
}