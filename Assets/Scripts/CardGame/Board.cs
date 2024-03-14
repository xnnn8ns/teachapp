using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Board : MonoBehaviour
{
    private List<Card> allCards;
    private Card[,] cards;
    private int winscore = 0;
    private int losescore = 0;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private AudioClip winSound;
    [SerializeField] private GameObject winCardPanel;
    [SerializeField] private Sprite[] allSprites;
    private int winningCardIndex;
    private int pointsPerCat;
    private int score = 0;
    private Sprite winningSprite;
    private Card winningCard;
    public int size;

#region basic methods

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

                card.Initialize(false); // Инициализируем карты в закрытом состоянии
                card.OnCardSelected += CardSelected;

                cards[i, j] = card;
                allCards.Add(card);
            }
        }

        // Перемешиваем список карт
        allCards = allCards.OrderBy(x => Random.value).ToList();

        // Генерируем случайное число от 2 до size включительно
        int winningCardsCount = Random.Range(2, size + 1);

        // Выбираем случайный спрайт из массива allSprites для всех выигрышных карт
        int index = UnityEngine.Random.Range(0, allSprites.Length);
        winningSprite = allSprites[index];

        // Удаляем выбранный спрайт из массива
        List<Sprite> tempList = new List<Sprite>(allSprites);
        tempList.RemoveAt(index);
        allSprites = tempList.ToArray();

        // Устанавливаем первые `winningCardsCount` карт как выигрышные
        for (int i = 0; i < winningCardsCount; i++)
        {
            allCards[i].successSprite = winningSprite;
            allCards[i].HasStar = true;
        }

        // Выбираем спрайты для остальных карт
        for (int i = winningCardsCount; i < allCards.Count; i++)
        {
            index = UnityEngine.Random.Range(0, allSprites.Length);
            allCards[i].chosenFailureSprite = allSprites[index];
        }

        // Скрываем все карты после небольшой задержки
        StartCoroutine(ShowAndHideCards());
    }

    private IEnumerator ShowAndHideCards()
    {
        BlockAllCardsInteraction();
        yield return new WaitForSeconds(1);

        // Отображаем выигрышную карту на панели
        DisplayCardOnPanel(winningCard);
        yield return new WaitForSeconds(4.25f);

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

    private void DisplayCardOnPanel(Card card)
    {
        // Проверяем, был ли инициализирован winCardPanel
        if (winCardPanel != null)
        {
            // Активируем winCardPanel
            winCardPanel.SetActive(true);

            // Находим дочерний объект с именем "Image"
            Transform imageTransform = winCardPanel.transform.Find("Image");

            // Проверяем, нашли ли мы объект
            if (imageTransform != null)
            {
                // Получаем компонент Image этого объекта
                Image panelImage = imageTransform.GetComponent<Image>();
                // Используем спрайт выигрышной карты
                panelImage.sprite = winningSprite;

            // Запускаем Coroutine для скрытия карты после задержки
            StartCoroutine(HideCardAfterDelay(3));
            }
        }
    }

    private void CardSelected(Card card)
    {
        if (card.HasStar)
        {
            // игрок угадал, начисляем очки победы
            winscore++;

            // Если общее количество выигрышных карт меньше или равно 3, то даем от 20 до 50 баллов, если больше - от 15 до 40
            int winningCardsCount = allCards.Count(c => c.HasStar);
            pointsPerCat = winningCardsCount <= 3 ? Random.Range(20, 51) : Random.Range(15, 41);

            score += pointsPerCat;
            card.ShowSuccess();
            card.Flip();

            // Если игрок нашел все выигрышные карты, активируем панель winPanel
            if (winscore == winningCardsCount)
            {
                BlockAllCards();
                winPanel.SetActive(true);
                // Загружаем текущий счет из PlayerPrefs
                int currentScore = PlayerPrefs.GetInt("Score", 0);
                // Добавляем к текущему счету очки за эту игру
                currentScore += score;
                // Сохраняем обновленный счет обратно в PlayerPrefs
                PlayerPrefs.SetInt("Score", currentScore);
                scoreText.text = score.ToString();
                AudioSource audioSource = FindObjectOfType<AudioSource>();
                if (audioSource != null)
                {
                    audioSource.clip = winSound;
                    audioSource.Play();
                }
            }
        }
        else
        {
            // игрок не угадал, -1 попытка
            losescore--;
            card.ShowFailure();
            card.Flip();

            // Если очков проигрыша равняются размеру поля или больше, переходим на сцену BonusSceneLose
            if (losescore <= -size)
            {
                BlockAllCards();
                StartCoroutine(LoadSceneAfterDelay("BonusSceneLose", 1));
            }
        }
    }

#endregion
#region helper methods
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

    private IEnumerator LoadSceneAfterDelay(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator HideCardAfterDelay(float delay)
    {
        // Ждем указанное количество секунд
        yield return new WaitForSeconds(delay);

        // Скрываем winCardPanel
        winCardPanel.SetActive(false);
    }
}
#endregion