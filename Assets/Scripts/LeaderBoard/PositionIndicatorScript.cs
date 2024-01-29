using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PositionIndicatorScript : MonoBehaviour
{
    [SerializeField] private Sprite upArrow;
    [SerializeField] private Sprite downArrow;
    [SerializeField] private Sprite equalSign;

    void Start()
    {
        // Отложить выполнение кода
        Invoke("InitializePositionIndicator", 0.25f);
    }

    void InitializePositionIndicator()
    {
        // Получить ссылку на скрипт LeaderBoardScript
        LeaderBoardScript leaderboard = FindObjectOfType<LeaderBoardScript>();

        // Получить текущую позицию пользователя из PlayerPrefs
        int userPosition = PlayerPrefs.GetInt("LastUserPosition");

        // Если позиция пользователя - первая или последняя, не делать ничего
        if (userPosition == 1 || userPosition == transform.childCount)
        {
            return;
        }

        // Получить объект игрока по позиции
        GameObject player = transform.GetChild(userPosition - 1).gameObject;

        // Найти текстовую строку Position
        TextMeshProUGUI positionText = player.transform.Find("Position").GetComponent<TextMeshProUGUI>();

        // Создать новый объект Image для отображения стрелки
        GameObject arrow = new GameObject("Arrow");
        Image arrowImage = arrow.AddComponent<Image>();
        arrow.transform.SetParent(player.transform);

        // Получить компонент RectTransform из positionText
        RectTransform positionRect = positionText.GetComponent<RectTransform>();

        // Отключить positionText
        positionText.enabled = false;

        // Установить позицию и размеры изображения
        RectTransform arrowRect = arrow.GetComponent<RectTransform>();
        arrowRect.position = positionRect.position;

        // Установить изображение в зависимости от значений userMovedUp и userMovedDown
        if (leaderboard.userMovedUp)
        {
            arrowRect.sizeDelta = new Vector2(100, 100);
            arrowImage.sprite = upArrow;
        }
        else if (leaderboard.userMovedDown)
        {
            arrowRect.sizeDelta = new Vector2(125, 125);
            arrowImage.sprite = downArrow;
        }
        else
        {
            arrowRect.sizeDelta = new Vector2(85, 85);
            arrowImage.sprite = equalSign;
        }
    }
}