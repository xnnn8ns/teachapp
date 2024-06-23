using System.Collections;
using UnityEngine;
using TMPro;

public class LoadCourseList : MonoBehaviour
{
    [SerializeField] private GameObject prefab; // Префаб, который будет создаваться

    void Start()
    {
        StartCoroutine(LoadAndSetCourseList());
    }

    IEnumerator LoadAndSetCourseList()
    {
        // Загрузка файла courseList
        TextAsset courseListData = Resources.Load<TextAsset>("courseList");

        // Разделение данных на строки
        var lines = courseListData.text.Split('\n');
        // Создание экземпляров префаба для каждой темы
        for (int i = 0; i < lines.Length; i++)
        {
            // Создание экземпляра префаба
            var instance = Instantiate(prefab, transform);

            // Установка позиции экземпляра
            float prefabHeight = ((RectTransform)prefab.transform).sizeDelta.y;
            instance.transform.localPosition = new Vector2(0, -i * prefabHeight);

            // Ожидание следующего кадра, чтобы дать префабу время на инициализацию
            yield return null;

            // Получение всех компонентов TextMeshProUGUI в дочерних объектах
            var textMeshPros = instance.GetComponentsInChildren<TextMeshProUGUI>(true);

            // Установка текста в каждом компоненте TextMeshProUGUI
            foreach (var textMeshPro in textMeshPros)
            {
                textMeshPro.text = lines[i];
            }
        }
    }
}