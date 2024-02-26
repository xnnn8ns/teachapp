using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;

public class Board : MonoBehaviour
{
    private List<Card> allCards;
    private Card[,] cards;
    private int winscore = 0;
    private int losescore = 0;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Button continueButton;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    public int size;

    void Start()
    {
        Initialize(size);
    }

    public void Initialize(int difficulty)
    {
        size = difficulty;
        cards = new Card[size, size];

        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector2 position = rectTransform.anchoredPosition;
        Vector2 scale = rectTransform.sizeDelta;

        // прямоугольный вариант сторон
        // float cardWidth = rectTransform.rect.width / size;
        // float cardHeight = rectTransform.rect.height / size;

        // квадрат
        float cardSize = Mathf.Min(rectTransform.rect.width, rectTransform.rect.height) / size;
        float cardWidth = cardSize;
        float cardHeight = cardSize;
        cardPrefab.GetComponent<RectTransform>().sizeDelta = new Vector2(cardWidth, cardHeight);

        // Вычисляем расстояние между картами
        float paddingX = (rectTransform.rect.width - cardWidth * size) / (size - 1) + 2;
        float paddingY = (rectTransform.rect.height - cardHeight * size) / (size - 1) + 2;

        // Создаем список всех карт
        allCards = new List<Card>();

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                Vector2 pivot = rectTransform.pivot;
                float x = (i * (cardWidth + paddingX)) - rectTransform.rect.width / 2 + cardWidth / 2;
                float y = (j * (cardHeight + paddingY)) - rectTransform.rect.height / 2 + cardHeight / 2;
                GameObject cardObject = Instantiate(cardPrefab, rectTransform);
                cardObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
                Card card = cardObject.GetComponent<Card>();

                card.Initialize(false); // Инициализируем карты в закрытом состоянии и без звезды
                card.OnCardSelected += CardSelected;

                cards[i, j] = card;
                allCards.Add(card);
            }
        }

        // Перемешиваем список карт
        allCards = allCards.OrderBy(x => Random.value).ToList();

        // Генерируем случайное число от 2 до size включительно
        int winningCardsCount = Random.Range(2, size + 1);

        // Устанавливаем первые `winningCardsCount` карт как выигрышные
        for (int i = 0; i < winningCardsCount; i++)
        {
            allCards[i].HasStar = true;
        }

        // Скрываем все карты после небольшой задержки
        StartCoroutine(ShowAndHideCards());
    }

    private IEnumerator ShowAndHideCards()
    {
        BlockAllCardsInteraction();
        yield return new WaitForSeconds(1);

        // Показываем все карты
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                cards[i, j].Flip();
            }
        }

        yield return new WaitForSeconds(3);

        // Скрываем все карты
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                cards[i, j].Flip();
            }
        }

        // Разблокируем все карты
        UnblockAllCardsInteraction();
    }

    public void ShowCards()
    {
        foreach (var card in cards)
        {
            card.Flip();
        }
    }

    public void HideCards()
    {
        foreach (var card in cards)
        {
            card.Flip();
        }
    }

    private void CardSelected(Card card)
    {
        if (card.HasStar)
        {
            // игрок угадал, начисляем очки
            winscore++;
            card.ShowSuccess();
            card.Flip();

            // Если игрок нашел все выигрышные карты, отображаем панель winPanel
            if (winscore == allCards.Count(c => c.HasStar))
            {
                BlockAllCards();
                continueButton.gameObject.SetActive(true);
                winPanel.SetActive(true);
            }
        }
        else
        {
            // игрок не угадал, можно уменьшить счет или ничего не делать
            losescore--;
            card.ShowFailure();
            card.Flip();

            // Если очков проигрыша равняются размеру поля или больше, блокируем все карты и показываем сообщение
            if (losescore <= -size)
            {
                BlockAllCards();
                continueButton.gameObject.SetActive(true);
                losePanel.SetActive(true);
            }
        }
    }

    private void BlockAllCards()
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                cards[i, j].Block();
            }
        }
    }

    private void BlockAllCardsInteraction()
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                cards[i, j].BlockCardInteraction();
            }
        }
    }

    private void UnblockAllCardsInteraction()
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                cards[i, j].UnblockCardInteraction();
            }
        }
    }
}