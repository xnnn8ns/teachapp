using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class CodeLine
{
    public string FullLine { get; set; }
    public List<string> Rectangles { get; set; }
    public int[] BugIndex { get; set; }
}

public class GameRound
{
    public CodeLine CurrentLine { get; set; }
    public int SelectedRectangle { get; set; }
}

public class BugHunterGame : MonoBehaviour
{
    public event System.Action ActionLevelCompleted;
    private List<CodeLine> codeLines;
    private GameRound currentRound;
    private int linesWithoutBugsCount = 0;
    private List<GameObject> selectedRectangles = new List<GameObject>();
    private List<GameObject> createdRectangles = new List<GameObject>();
    [SerializeField] private GameObject rectanglePrefab;
    [SerializeField] private GameObject allBugsSelectedButton;
    [SerializeField] private AudioClip winSound;
    [SerializeField] private AudioClip winSound2;
    [SerializeField] private AudioClip loseSound;
    [SerializeField] private AudioClip loseGameSound;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Animator animator;
    private AudioSource audioSource;
    private int loseScore;
    private int maxRoundCount;
    private int roundCount = 0;
    private int lastLineIndex = -1;

#region basic methods
    void Start()
    {
        int currentTopic = Settings.Current_Topic;

        if (currentTopic <= 4) {
            maxRoundCount = 3;
        } 
        else if (currentTopic <= 7) {
            maxRoundCount = 4;
        } else {
            maxRoundCount = 5;
        }

        // Загрузка данных из JSON-файла
        string jsonFilePath = Path.Combine(Application.dataPath, "Scripts/MiniGame/BugHunterGame/listStringForGame.json");
        string jsonData = File.ReadAllText(jsonFilePath);
        codeLines = JsonConvert.DeserializeObject<List<CodeLine>>(jsonData);
        
        audioSource = GetComponent<AudioSource>();

        // Начало нового раунда
        StartNewRound();
    }

    public void AllBugsSelectedButtonPressed()
    {
        bool isWin = false;
        bool isLose = false;
        bool alreadyLost = false;

        if (selectedRectangles.Count > 0)
        {
            foreach (GameObject rectangle in selectedRectangles)
            {
                int rectangleIndex = rectangle.transform.GetSiblingIndex();
                if (!currentRound.CurrentLine.BugIndex.Contains(rectangleIndex))
                {
                    isLose = true;
                    if (!alreadyLost)
                    {
                        loseScore++;
                        alreadyLost = true;
                    }
                    rectangle.GetComponent<Image>().color = Color.red;
                    rectangle.GetComponent<RectTransform>().anchoredPosition -= new Vector2(0, 10);
                    break;
                }
            }

            if (!isLose)
            {
                foreach (GameObject rectangle in selectedRectangles)
                {
                    rectangle.GetComponent<Image>().color = Color.green;
                    rectangle.GetComponent<RectTransform>().anchoredPosition += new Vector2(0, 5);
                }
                isWin = true;
            }

            // Подсветка желтым цветом прямоугольников с багами, которые игрок не выбрал
            foreach (int bugIndex in currentRound.CurrentLine.BugIndex)
            {
                if (bugIndex < createdRectangles.Count && !selectedRectangles.Contains(createdRectangles[bugIndex]))
                {
                    isLose = true; // Если игрок не нашел все баги, это считается ошибкой
                    if (!alreadyLost)
                    {
                        loseScore++;
                        alreadyLost = true;
                    }
                    createdRectangles[bugIndex].GetComponent<Image>().color = Color.yellow;
                    createdRectangles[bugIndex].GetComponent<RectTransform>().anchoredPosition -= new Vector2(0, 10);
                }
            }
        }
        else
        {
            if (currentRound.CurrentLine.BugIndex.Length == 0)
            {
                isWin = true;
                foreach (GameObject rectangle in createdRectangles)
                {
                    rectangle.GetComponent<Image>().color = Color.green;
                    rectangle.GetComponent<RectTransform>().anchoredPosition += new Vector2(0, 5);
                }
            }
            else 
            {
                isLose = true;
                loseScore++;
                foreach (int bugIndex in currentRound.CurrentLine.BugIndex) 
                {
                    if (bugIndex < createdRectangles.Count)
                    {
                        createdRectangles[bugIndex].GetComponent<Image>().color = Color.yellow;
                        createdRectangles[bugIndex].GetComponent<RectTransform>().anchoredPosition -= new Vector2(0, 10);
                    }
                }
            }
        }
    
        if (isWin)
        {
            audioSource.volume = 0.1f;
            audioSource.PlayOneShot(winSound);
        }
        else if (isLose)
        {
            audioSource.volume = 0.33f;
            audioSource.PlayOneShot(loseSound);
        }
    
        currentRound.CurrentLine.BugIndex = new int[0];
        allBugsSelectedButton.GetComponent<Button>().interactable = false;
    
        roundCount++;
        
        if (loseScore >= 2 && maxRoundCount == 3)
        {
            PlayerPrefs.SetInt("ErrorCount", loseScore);
            // ComonFunctions.Instance.SetNextLevel(60, 30);
        
            // Воспроизводим звук проигрыша
            audioSource.clip = loseGameSound;
            audioSource.Play();
        
            StartCoroutine(LoadSceneAfterDelay("BonusSceneLose", 1.5f));
        }
        else if(loseScore >= 3 && maxRoundCount == 4)
        {
            PlayerPrefs.SetInt("ErrorCount", loseScore);
            // ComonFunctions.Instance.SetNextLevel(60, 30);
        
            // Воспроизводим звук проигрыша
            audioSource.clip = loseGameSound;
            audioSource.Play();
        
            StartCoroutine(LoadSceneAfterDelay("BonusSceneLose", 1.5f));
        }
        else if(loseScore >= 4 && maxRoundCount == 5)
        {
            PlayerPrefs.SetInt("ErrorCount", loseScore);
            // ComonFunctions.Instance.SetNextLevel(60, 30);
        
            // Воспроизводим звук проигрыша
            audioSource.clip = loseGameSound;
            audioSource.Play();
        
            StartCoroutine(LoadSceneAfterDelay("BonusSceneLose", 1.5f));
        }
        else 
        {
            StartCoroutine(EnableButtonAfterDelay(1.7f));
        
            if (roundCount < maxRoundCount)
            {
                StartCoroutine(StartNewRoundWithDelay(1.6f));
            } 
            else 
            {
                StartCoroutine(EndGameWithDelay(1.6f));
            }
        }
    }

    private void EndGame()
    {
        PlayerPrefs.SetInt("ErrorCount", 0);
        // ComonFunctions.Instance.SetNextLevel(60, 30);

        StartCoroutine(WinCoroutine());
    }

#endregion
#region helper methods

    private IEnumerator StartNewRoundWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        foreach (GameObject rectangle in selectedRectangles)
        {
            Destroy(rectangle);
        }
        selectedRectangles.Clear();
        StartNewRound();
    }

    private IEnumerator EndGameWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        selectedRectangles.Clear();
        EndGame();
    }

    public void SelectRectangle(GameObject selectedRectangle)
    {
        // Проверка, был ли прямоугольник уже выбран
        if (selectedRectangles.Contains(selectedRectangle))
        {
            // Если прямоугольник уже был выбран, отменяем его выбор
            selectedRectangle.GetComponent<Image>().color = Color.white; // Изменение цвета на исходный
            selectedRectangles.Remove(selectedRectangle); // Удаление из списка выбранных
        }
        else
        {
            // Если прямоугольник еще не был выбран, выбираем его
            selectedRectangle.GetComponent<Image>().color = Color.gray; // Изменение цвета на серый
            selectedRectangles.Add(selectedRectangle); // Добавление в список выбранных
        }
    }

    private void StartNewRound()
    {
        // Удаление старых прямоугольников
        foreach (GameObject rectangle in createdRectangles)
        {
            Destroy(rectangle);
        }
        createdRectangles.Clear(); // Очистка списка

        // Выбор случайной строки кода и установка ее как текущей
        int randomIndex;
        do
        {
            randomIndex = Random.Range(0, codeLines.Count);
        } while ((randomIndex == lastLineIndex) || ((linesWithoutBugsCount >= (maxRoundCount <= 3 ? 1 : 2)) && codeLines[randomIndex].BugIndex.Length == 0));

        if (codeLines[randomIndex].BugIndex.Length == 0)
        {
            linesWithoutBugsCount++;
        }

        currentRound = new GameRound { CurrentLine = codeLines[randomIndex] };
        lastLineIndex = randomIndex;
    
        // Создание прямоугольников для текущей строки кода
        float offset = rectanglePrefab.GetComponent<RectTransform>().sizeDelta.x / 2; // Отступ между прямоугольниками
        float tabOffset = 0; // Отступ для табуляции
        float verticalOffset = 0; // Вертикальное смещение для новых строк
    
        foreach (string rect in currentRound.CurrentLine.Rectangles)
        {
            string rectangle = rect; // Создание временной переменной
            GameObject newRectangle = Instantiate(rectanglePrefab, this.transform);
        
            // Установка позиции прямоугольника с учетом отступа
            RectTransform rectTransform = newRectangle.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0, 1); // Левый верхний угол
            rectTransform.anchorMax = new Vector2(0, 1); // Левый верхний угол
        
            // Проверка, начинается ли текст прямоугольника с новой строки
            if (rectangle.StartsWith("\n"))
            {
                offset = rectanglePrefab.GetComponent<RectTransform>().sizeDelta.x / 2; // Сброс отступа
                tabOffset = 0; // Сброс отступа для табуляции
                verticalOffset += rectTransform.sizeDelta.y + 10; // Увеличение вертикального смещения
                rectangle = rectangle.TrimStart('\n'); // Удаление символа новой строки из начала строки
            }
        
            // Проверка, начинается ли текст прямоугольника с отступа
            if (rectangle.StartsWith("\t") || rectangle.StartsWith(" "))
            {
                tabOffset += rectTransform.sizeDelta.x + 20; // Увеличение отступа для табуляции на ширину прямоугольника
                rectangle = rectangle.TrimStart('\t', ' '); // Удаление символов табуляции и пробела из начала строки
            }
        
            rectTransform.anchoredPosition = new Vector2(offset + tabOffset, -rectTransform.sizeDelta.y / 2 - verticalOffset);
        
            // Получение компонента TextMeshPro и установка текста
            TextMeshProUGUI textComponent = newRectangle.transform.Find("TextValue").GetComponent<TextMeshProUGUI>();
            textComponent.text = rectangle; // Теперь символы новой строки и отступа уже удалены
        
            // Добавление обработчика события нажатия
            Button button = newRectangle.GetComponent<Button>();
            button.onClick.AddListener(() => SelectRectangle(newRectangle));
        
            // Увеличение отступа для следующего прямоугольника
            float nextOffset = 20f; // Установка фиксированного отступа после каждого прямоугольника
            offset += rectTransform.sizeDelta.x + nextOffset;
        
            // Добавление нового прямоугольника в список
            createdRectangles.Add(newRectangle);
        }
    }

    IEnumerator WinCoroutine()
    {
        yield return new WaitForSeconds(1.4f);

        winPanel.SetActive(true);

        audioSource.clip = winSound2;
        audioSource.Play();

        // Запускаем анимацию
        animator.speed = 0.85f;
        animator.Play("Anim2");

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
    }
    
    private IEnumerator EnableButtonAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        allBugsSelectedButton.GetComponent<Button>().interactable = true;
    }

    private IEnumerator LoadSceneAfterDelay(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName,LoadSceneMode.Single);
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
}

#endregion