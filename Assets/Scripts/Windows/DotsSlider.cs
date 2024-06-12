using UnityEngine;
using UnityEngine.UI;

public class DotsSlider : MonoBehaviour
{
    [SerializeField]
    private Image[] _dots; // Массив изображений, представляющих собой точки
    [SerializeField]
    private Color _notCompletedColor; // Цвет для непройденных уровней
    [SerializeField]
    private Sprite _completedSprite; // Спрайт для пройденных уровней

    private Color _defaultColor; // Цвет изображения по умолчанию
    private Sprite _defaultSprite; // Спрайт изображения по умолчанию

    private void Awake()
    {
        // Сохраняем цвет и спрайт изображения по умолчанию
        _defaultColor = _dots[0].color;
        _defaultSprite = _dots[0].sprite;
    }

    public void UpdateDots(int passCount)
    {
        // Заполняем точки
        for (int i = 0; i < _dots.Length; i++)
        {
            if (i < passCount)
            {
                // Если уровень пройден, устанавливаем спрайт пройденного уровня
                _dots[i].color = _defaultColor;
                _dots[i].sprite = _completedSprite;
            }
            else
            {
                // Если уровень не пройден, устанавливаем цвет и спрайт изображения по умолчанию
                _dots[i].color = _notCompletedColor;
                _dots[i].sprite = _defaultSprite;
            }
        }
    }
}