using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Board : MonoBehaviour
{
    public event System.Action ActionLevelCompleted;

    private List<Card> allCards;
    private Card[,] cards;
    private int winscore = 0;
    private int losescore = 0;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Text timerText;
    [SerializeField] private AudioClip winSound;
    [SerializeField] private GameObject winCardPanel;
    [SerializeField] private GameObject rememberButton;
    [SerializeField] private GameObject timerObject;
    [SerializeField] private Sprite[] allSprites;
    private int winningCardIndex;
    private int pointsPerCat;
    //private int score = 0;
    private Sprite winningSprite;
    private Card winningCard;
    private int size;
    private int currentTopic;
    private int timerDuration;
    private bool gameWin = false;
    private bool gameLose = false;

    private bool isRememberButtonPressed = false;
    private bool areCardsShown = false;

#region basic methods

    void Start()
    {
        currentTopic = Settings.Current_Topic;
    
        // Устанавливаем size и timerDuration в соответствии с Current_Topic
        int[] sizes = { 3, 3, 4, 5 };
        int[] timerDurations = { 10, 15, 20, 25, 30 };
    
        if (currentTopic >= 0 && currentTopic <= 4)
        {
            size = sizes[currentTopic];
        }
        else
        {
            size = Mathf.RoundToInt(currentTopic / 1.2f);
        }
    
        if (currentTopic >= 0 && currentTopic <= 9)
        {
            timerDuration = timerDurations[currentTopic / 3];
        }
        else
        {
            timerDuration = 60;
        }
    
        // Обновляем текст таймера
        timerText.text = string.Format("{0:D2}:{1:D2}", timerDuration / 60, timerDuration % 60);
    
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
        
        if (currentTopic >= 0 && currentTopic <= 4)
        {
            // Выбираем спрайт с индексом 3 из массива allSprites для всех проигрышных карт
            int index = 3;
            Sprite failureSprite = allSprites[index];
        
            // Удаляем выбранный спрайт из массива
            List<Sprite> tempList = new List<Sprite>(allSprites);
            tempList.RemoveAt(index);
            allSprites = tempList.ToArray();
        
            // Устанавливаем спрайт для всех проигрышных карт
            for (int i = winningCardsCount; i < allCards.Count; i++)
            {
                allCards[i].chosenFailureSprite = failureSprite;
            }
        
            // Выбираем случайный спрайт из массива allSprites для всех выигрышных карт
            index = UnityEngine.Random.Range(0, allSprites.Length);
            winningSprite = allSprites[index];
        
            // Удаляем выбранный спрайт из массива
            tempList = new List<Sprite>(allSprites);
            tempList.RemoveAt(index);
            allSprites = tempList.ToArray();
        
            // Устанавливаем первые `winningCardsCount` карт как выигрышные
            for (int i = 0; i < winningCardsCount; i++)
            {
                allCards[i].successSprite = winningSprite;
                allCards[i].HasStar = true;
            }
        }
        else
        {
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
        }
        
        // Скрываем все карты после небольшой задержки
        StartCoroutine(ShowAndHideCards());
    }

    private IEnumerator ShowAndHideCards()
    {
        BlockAllCardsInteraction();

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

        areCardsShown = true; // Устанавливаем флаг, что все карты показаны

        // Ждем, пока пользователь не нажмет кнопку "Я запомнил"
        yield return new WaitUntil(() => isRememberButtonPressed);

        // Скрываем все карты
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                cards[i, j].Flip();
            }
        }

        areCardsShown = false; // Устанавливаем флаг, что все карты скрыты

        // Разблокируем все карты
        UnblockAllCardsInteraction();

        // Запускаем таймер
        StartCoroutine(StartTimerAfterCardsShown());
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

            //score += pointsPerCat;
            card.ShowSuccess();
            card.Flip();

            // Если игрок нашел все выигрышные карты, активируем панель winPanel
            if (winscore == winningCardsCount)
            {
                gameWin = true;
                PlayerPrefs.SetInt("ErrorCount", 0);
                ComonFunctions.Instance.SetNextLevel(60, 30);
                BlockAllCards();
                winPanel.SetActive(true);
                // Загружаем текущий счет из PlayerPrefs
                int currentScore = PlayerPrefs.GetInt("Score", 0);

                ButtonData buttonData = DataLoader.GetLevelData(Settings.Current_ButtonOnMapID);
                int score = buttonData.score;

                // Добавляем к текущему счету очки за эту игру
                currentScore += score;
                // Сохраняем обновленный счет обратно в PlayerPrefs
                PlayerPrefs.SetInt("Score", currentScore);
                PlayerPrefs.SetInt("AddedScore", score);
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
                gameLose = true;
                PlayerPrefs.SetInt("ErrorCount", losescore);
                ComonFunctions.Instance.SetNextLevel(60, 30);
                BlockAllCards();
                StartCoroutine(LoadSceneAfterDelay("BonusSceneLose", 1));
            }
        }
    }

#endregion

#region helper methods
    public void RememberButtonPressed()
    {
        if (!areCardsShown) return; // Если карты еще не показаны, не обрабатываем нажатие кнопки

        isRememberButtonPressed = true;
        timerObject.SetActive(true); // включаем объект таймер
        rememberButton.SetActive(false); // скрываем кнопку "Запомнил"
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

    private IEnumerator LoadSceneAfterDelay(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName,LoadSceneMode.Single);
    }

    private IEnumerator HideCardAfterDelay(float delay)
    {
        // Ждем указанное количество секунд
        yield return new WaitForSeconds(delay);

        // Скрываем winCardPanel
        winCardPanel.SetActive(false);
    }

    private void SetNextLevel()
    {
        //ButtonData buttonData = DataLoader.GetLevelData(Settings.Current_ButtonOnMapID);
        //int score = ComonFunctions.GetScoreForLevel(buttonData.score, buttonData.passCount, (ETypeLevel)buttonData.typeLevel);
        //UserData.SetScore(PlayerPrefs.GetInt("Score", 0));
        //int errorCount = PlayerPrefs.GetInt("ErrorCount", 0);
        //int starCount = 0;
        //starCount = ComonFunctions.GetStarCountAfterLevelPass(120, 60, buttonData.typeLevel);
        //buttonData.passCount++;
        //if (buttonData.typeLevel == (int)ETypeLevel.simple)
        //    buttonData.activeStarsCount += starCount;
        //else
        //    buttonData.activeStarsCount = starCount;
        //if (buttonData.activeStarsCount > 3)
        //    buttonData.activeStarsCount = 3;

        //bool isPassed = false;
        //int currentLevel = Settings.Current_ButtonOnMapID;

        //PlayerPrefs.SetFloat("PassSeconds", 0);
        //PlayerPrefs.SetInt("PassQuestionCount", 1);

        //DataLoader.SaveLevelResults(currentLevel, buttonData.isActive, isPassed, buttonData.activeStarsCount, buttonData.passCount);

        //if (buttonData.passCount >= buttonData.totalForPassCount)
        //{
        //    Settings.Current_ButtonOnMapID++;
        //    DataLoader.SaveCurrentLevel();
        //    isPassed = true;

        //    // Получаем данные следующего уровня
        //    ButtonData nextButtonData = DataLoader.GetLevelData(Settings.Current_ButtonOnMapID);

        //    // Если следующий уровень уже существует, но не активен, делаем его активным
        //    if (nextButtonData != null && !nextButtonData.isActive)
        //    {
        //        nextButtonData.isActive = true;
        //        DataLoader.SaveLevelResults(Settings.Current_ButtonOnMapID, nextButtonData.isActive, nextButtonData.isPassed, nextButtonData.activeStarsCount, nextButtonData.passCount);
        //    }
        //    // Если следующего уровня еще не существует, создаем его и делаем активным
        //    else if (nextButtonData == null)
        //    {
        //        DataLoader.SaveLevelResults(Settings.Current_ButtonOnMapID, true, false, 0, 0);
        //    }
        //}

        //if (isPassed && buttonData.activeStarsCount == 3 && buttonData.passCount == 3) // set next level = true -- isActive, to show on map
        //{
        //    // Проверяем, был ли текущий уровень полностью пройден
        //    DataLoader.SaveLevelResults(Settings.Current_ButtonOnMapID, true, false, 0, 0);
        //}

        //Debug.Log("UpdateUser: " + UserData.Score);
        //if (UserData.UserID != "")
        //    StartCoroutine(ComonFunctions.Instance.UpdateUser(UserData.UserID, UserData.UserName, UserData.UserEmail, UserData.UserPassword, UserData.UserAvatarID, UserData.IsByVK, UserData.VKID, UserData.Score));
        //PlayerPrefs.SetInt("ErrorCount", 0);
        //ActionLevelCompleted.Invoke();

        //ComonFunctions.Instance.SetNextLevel(60, 30);

    }

    private IEnumerator StartTimerAfterCardsShown()
    {
        while (timerDuration >= 0)
        {
            // Если игрок нашел все карты или проиграл, прерываем цикл
            if (gameWin || gameLose)
                yield break;

            // Обновляем текст таймера
            timerText.text = string.Format("{0:D2}:{1:D2}", timerDuration / 60, timerDuration % 60);

            yield return new WaitForSeconds(1);

            if (gameWin || gameLose)
                yield break;

            timerDuration--;
        }

        // Код, который будет выполняться после истечения времени таймера
        if (!gameWin && !gameLose)
        {
            PlayerPrefs.SetInt("ErrorCount", losescore);
            ComonFunctions.Instance.SetNextLevel(60, 30);
            BlockAllCards();
            StartCoroutine(LoadSceneAfterDelay("BonusSceneLose", 1));
        }
    }
}
#endregion