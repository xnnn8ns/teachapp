using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextManager : MonoBehaviour
{
    public List<TextMeshProUGUI> TextObjects; // Список текстовых объектов
    public List<string> Keys; // Список ключей

    public void UpdateTexts()
    {
        List<LangItem> langItems = LangAsset.getInstance();
        for (int i = 0; i < TextObjects.Count; i++) // Используем цикл for, чтобы иметь доступ к индексу
        {
            TextMeshProUGUI textObject = TextObjects[i];
            string key = Keys[i]; // Получаем ключ для этого текстового объекта

            // Находим соответствующий LangItem для этого ключа
            LangItem item = langItems.Find(langItem => langItem.Key == key);
            if (item != null)
            {
                // Обновляем текст на основе текущего языка
                switch (LangAsset.CurrentLangLocation)
                {
                    case LangLocation.En:
                        textObject.text = item.En;
                        break;
                    case LangLocation.Ru:
                        textObject.text = item.Ru;
                        break;
                    case LangLocation.Ge:
                        textObject.text = item.Ge;
                        break;
                    case LangLocation.It:
                        textObject.text = item.It;
                        break;
                }
            }
        }
    }
}