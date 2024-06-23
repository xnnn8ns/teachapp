using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class MoveCatGame : MonoBehaviour
{
    public event System.Action ActionLevelCompleted;
    [SerializeField] private GameObject catPrefab;
    [SerializeField] private GameObject targetPrefab;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject whatDoPanel;
    [SerializeField] private CanvasGroup controlPanel;
    [SerializeField] private AudioClip winSound;
    [SerializeField] private AudioClip winSound2;
    [SerializeField] private Animator animator;
    [SerializeField] private int fieldSize = 7;
    private GameObject[,] cells;
    private GameObject cat;
    private GameObject target;
    private Vector2 catPosition;
    private Vector2 targetPosition;
    // private int score = 0;
    private bool[,] obstacles;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = FindObjectOfType<AudioSource>();
        cells = new GameObject[fieldSize, fieldSize];
        obstacles = new bool[fieldSize, fieldSize];
        StartCoroutine(StartGameAfterDelay(2));  
    }

    IEnumerator StartGameAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        whatDoPanel.SetActive(false);
        CreateField(); // Создаем поле
        PlaceCat(); // Размещаем кота
        PlaceTarget(); // Размещаем цель
    }

    void CreateField()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        float cellWidth = rectTransform.rect.width / fieldSize;
        float cellHeight = rectTransform.rect.height / fieldSize;
        
        RectTransform cellRectTransform;
        GameObject cell;
        
        for (int i = 0; i < fieldSize; i++)
        {
            for (int j = 0; j < fieldSize; j++)
            {
                cell = CreateCell(i, j, cellWidth, cellHeight);
                cellRectTransform = cell.GetComponent<RectTransform>();
                cells[i, j] = cell;
        
                // Add obstacles randomly
                if (ShouldPlaceObstacle(i, j))
                {
                    CreateObstacle(cell, cellWidth, cellHeight);
                    obstacles[i, j] = true;
                }
            }
        }
        
        int minObstacles = fieldSize / 2; // minimum number of obstacles
        
        // Add more obstacles if necessary
        while (CountObstacles() < minObstacles)
        {
            PlaceAdditionalObstacle(cellWidth, cellHeight);
        }
    }
    
    void PlaceCat()
    {
        int x, y;
        do
        {
            x = Random.Range(0, fieldSize);
            y = Random.Range(0, fieldSize);
        } while (obstacles[x, y]);
    
        PlaceGameObject(catPrefab, ref cat, x, y);
        catPosition = new Vector2(x, y); // Инициализация позиции кота
    }
    
    void PlaceTarget()
    {
        int x, y;
        do
        {
            do
            {
                x = Random.Range(0, fieldSize);
                y = Random.Range(0, fieldSize);
            } while (Mathf.Abs(x - catPosition.x) < 5 && Mathf.Abs(y - catPosition.y) < 5 || obstacles[x, y]);
    
            PlaceGameObject(targetPrefab, ref target, x, y);
            targetPosition = new Vector2(x, y); // Инициализация позиции цели
        } while (!IsPathAvailable((int)catPosition.x, (int)catPosition.y, x, y));
    }
    
    void PlaceGameObject(GameObject prefab, ref GameObject gameObject, int x, int y)
    {
        gameObject = Instantiate(prefab, cells[x, y].transform);
        gameObject.GetComponent<RectTransform>().sizeDelta = cells[x, y].GetComponent<RectTransform>().sizeDelta;
    }

    public void MoveCat(int dx, int dy)
    {
        int newX = (int)catPosition.x + dx;
        int newY = (int)catPosition.y + dy;

        if (newX >= 0 && newX < fieldSize && newY >= 0 && newY < fieldSize)
        {
            // Check for obstacle
            if (cells != null && cells[newX, newY] != null && !obstacles[newX, newY])
            {
                cat.transform.SetParent(cells[newX, newY].transform, false);
                catPosition = new Vector2(newX, newY);

                // Если кот достиг цели
                if (catPosition == targetPosition)
                {
                    controlPanel.interactable = false; // Отключаем интерактивность панели управления

                    foreach (CanvasGroup childCanvasGroup in controlPanel.GetComponentsInChildren<CanvasGroup>())
                    {
                        childCanvasGroup.interactable = false;
                    }

                    PlayerPrefs.SetInt("ErrorCount", 0);
                    ComonFunctions.Instance.SetNextLevel(60, 30);

                    // Воспроизводим звук победы и активируем систему частиц
                    audioSource.clip = winSound;
                    audioSource.Play();

                    StartCoroutine(ShowWinPanelAfterDelay(2f));
                }
            }
        }
    }

    

#region MoveCatMethods
    public void MoveCatUp()
    {
        MoveCat(0, -1);
    }

    public void MoveCatDown()
    {
        MoveCat(0, 1);
    }

    public void MoveCatLeft()
    {
        MoveCat(-1, 0);
    }

    public void MoveCatRight()
    {
        MoveCat(1, 0);
    }

    IEnumerator ShowWinPanelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Ждем указанное количество секунд

        winPanel.SetActive(true);

        audioSource.clip = winSound2;
        audioSource.Play();

        // Запускаем анимацию
        animator.speed = 0.85f;
        animator.Play("Anim5");

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

    GameObject CreateCell(int i, int j, float cellWidth, float cellHeight)
    {
        GameObject cell = new GameObject("Cell");
        cell.transform.SetParent(transform);
        RectTransform cellRectTransform = cell.AddComponent<RectTransform>();
        cellRectTransform.anchorMin = new Vector2(0, 1); // top left corner
        cellRectTransform.anchorMax = new Vector2(0, 1); // top left corner
        cellRectTransform.pivot = new Vector2(0, 1); // top left corner
        cellRectTransform.sizeDelta = new Vector2(cellWidth, cellHeight);
        cellRectTransform.anchoredPosition = new Vector2(i * cellWidth, -j * cellHeight);
        cell.AddComponent<Image>().color = new Color(1, 1, 1, 0.5f); // semi-transparent white
        return cell;
    }
    
    bool ShouldPlaceObstacle(int i, int j)
    {
        return Random.value < 0.3f && // 30% chance
            Mathf.Abs(i - catPosition.x) > 1 && Mathf.Abs(j - catPosition.y) > 1 && // not around the cat
            Mathf.Abs(i - targetPosition.x) > 1 && Mathf.Abs(j - targetPosition.y) > 1; // not around the target
    }
    
    void CreateObstacle(GameObject cell, float cellWidth, float cellHeight)
    {
        GameObject obstacle = new GameObject("Obstacle");
        obstacle.transform.SetParent(cell.transform);
        RectTransform obstacleRectTransform = obstacle.AddComponent<RectTransform>();
        obstacleRectTransform.anchorMin = new Vector2(0, 1); // top left corner
        obstacleRectTransform.anchorMax = new Vector2(0, 1); // top left corner
        obstacleRectTransform.pivot = new Vector2(0, 1); // top left corner
        obstacleRectTransform.sizeDelta = new Vector2(cellWidth, cellHeight);
        obstacleRectTransform.anchoredPosition = Vector2.zero;
        obstacle.AddComponent<Image>().color = new Color(0, 0, 0, 0.5f); // semi-transparent black
    }
    
    int CountObstacles()
    {
        int count = 0;
        for (int i = 0; i < fieldSize; i++)
        {
            for (int j = 0; j < fieldSize; j++)
            {
                if (obstacles[i, j])
                {
                    count++;
                }
            }
        }
        return count;
    }
    
    void PlaceAdditionalObstacle(float cellWidth, float cellHeight)
    {
        int x, y;
        do
        {
            x = Random.Range(0, fieldSize);
            y = Random.Range(0, fieldSize);
        } while (IsInvalidObstaclePosition(x, y));
    
        CreateObstacle(cells[x, y], cellWidth, cellHeight);
        obstacles[x, y] = true;
    }
    
    bool IsInvalidObstaclePosition(int x, int y)
    {
        return Mathf.Abs(x - catPosition.x) <= 1 && Mathf.Abs(y - catPosition.y) <= 1 || // not around the cat
            Mathf.Abs(x - targetPosition.x) <= 1 && Mathf.Abs(y - targetPosition.y) <= 1 || // not around the target
            obstacles[x, y]; // not on a cell that already has an obstacle
    }

    bool IsPathAvailable(int startX, int startY, int endX, int endY)
    {
        bool[,] visited = new bool[fieldSize, fieldSize];
        Queue<Vector2> queue = new Queue<Vector2>();
        Vector2[] directions = new Vector2[] { new Vector2(0, 1), new Vector2(0, -1), new Vector2(1, 0), new Vector2(-1, 0) };
    
        visited[startX, startY] = true;
        queue.Enqueue(new Vector2(startX, startY));
    
        while (queue.Count > 0)
        {
            Vector2 current = queue.Dequeue();
            if (current.x == endX && current.y == endY)
            {
                return true;
            }
    
            foreach (Vector2 direction in directions)
            {
                int newX = (int)(current.x + direction.x);
                int newY = (int)(current.y + direction.y);
    
                if (newX >= 0 && newX < fieldSize && newY >= 0 && newY < fieldSize && !visited[newX, newY] && !obstacles[newX, newY])
                {
                    visited[newX, newY] = true;
                    queue.Enqueue(new Vector2(newX, newY));
                }
            }
        }
    
        return false;
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

#endregion
}