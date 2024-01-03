using UnityEngine;
using UnityEngine.UI;

public class FlagDisplay : MonoBehaviour
{
    public Sprite[] flagSprites; // Массив спрайтов флагов
    public string[] languages; // Массив языков

    private Image flagImage; // Компонент изображения флага

    void Start()
    {
        flagImage = GetComponent<Image>();

        if (PlayerPrefs.HasKey("language"))
        {
            string language = PlayerPrefs.GetString("language");
            int index = System.Array.IndexOf(languages, language); // Находим индекс текущего языка

            if (index >= 0 && index < flagSprites.Length)
            {
                flagImage.sprite = flagSprites[index]; // Устанавливаем соответствующий спрайт флага
            }
        }
    }
}
