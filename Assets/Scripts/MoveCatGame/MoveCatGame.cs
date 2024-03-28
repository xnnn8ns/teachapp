using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoveCatGame : MonoBehaviour
{
    public GameObject catPrefab;
    public GameObject targetPrefab;
    public GameObject winPanel;
    [SerializeField] private TextMeshProUGUI scoreText;
    public AudioClip winSound;
    public int fieldSize = 7;
    private GameObject[,] cells;
    private GameObject cat;
    private GameObject target;
    private Vector2 catPosition;
    private Vector2 targetPosition;
    private int moveCount = 0;
    private int score = 0;

    void Start()
    {
        cells = new GameObject[fieldSize, fieldSize];
        CreateField();
        PlaceCat();
        PlaceTarget();
    }

    void CreateField()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        float cellWidth = rectTransform.rect.width / fieldSize;
        float cellHeight = rectTransform.rect.height / fieldSize;

        for (int i = 0; i < fieldSize; i++)
        {
            for (int j = 0; j < fieldSize; j++)
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
                cells[i, j] = cell;

                // Add obstacles randomly
                if (Random.value < 0.3f && // 30% chance
                    Mathf.Abs(i - catPosition.x) > 1 && Mathf.Abs(j - catPosition.y) > 1 && // not around the cat
                    Mathf.Abs(i - targetPosition.x) > 1 && Mathf.Abs(j - targetPosition.y) > 1) // not around the target
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
            }
        }

        int minObstacles = fieldSize / 2; // minimum number of obstacles
        int currentObstacles = 0;

        // Count the current number of obstacles
        for (int i = 0; i < fieldSize; i++)
        {
            for (int j = 0; j < fieldSize; j++)
            {
                if (cells[i, j].transform.Find("Obstacle") != null)
                {
                    currentObstacles++;
                }
            }
        }

        // Add more obstacles if necessary
        while (currentObstacles < minObstacles)
        {
            int x, y;
            do
            {
                x = Random.Range(0, fieldSize);
                y = Random.Range(0, fieldSize);
            } while (Mathf.Abs(x - catPosition.x) <= 1 && Mathf.Abs(y - catPosition.y) <= 1 || // not around the cat
                    Mathf.Abs(x - targetPosition.x) <= 1 && Mathf.Abs(y - targetPosition.y) <= 1 || // not around the target
                    cells[x, y].transform.Find("Obstacle") != null); // not on a cell that already has an obstacle

            GameObject obstacle = new GameObject("Obstacle");
            obstacle.transform.SetParent(cells[x, y].transform);
            RectTransform obstacleRectTransform = obstacle.AddComponent<RectTransform>();
            obstacleRectTransform.anchorMin = new Vector2(0, 1); // top left corner
            obstacleRectTransform.anchorMax = new Vector2(0, 1); // top left corner
            obstacleRectTransform.pivot = new Vector2(0, 1); // top left corner
            obstacleRectTransform.sizeDelta = new Vector2(cellWidth, cellHeight);
            obstacleRectTransform.anchoredPosition = Vector2.zero;
            obstacle.AddComponent<Image>().color = new Color(0, 0, 0, 0.5f); // semi-transparent black

            currentObstacles++;
        }
    }

    void PlaceCat()
    {
        int x, y;
        do
        {
            x = Random.Range(0, fieldSize);
            y = Random.Range(0, fieldSize);
        } while (cells[x, y].transform.Find("Obstacle") != null);

        cat = Instantiate(catPrefab, cells[x, y].transform);
        cat.GetComponent<RectTransform>().sizeDelta = cells[x, y].GetComponent<RectTransform>().sizeDelta;
        catPosition = new Vector2(x, y);
    }

    void PlaceTarget()
    {
        int x, y;
        do
        {
            x = Random.Range(0, fieldSize);
            y = Random.Range(0, fieldSize);
        } while (Mathf.Abs(x - catPosition.x) < 5 && Mathf.Abs(y - catPosition.y) < 5 || cells[x, y].transform.Find("Obstacle") != null);

        target = Instantiate(targetPrefab, cells[x, y].transform);
        target.GetComponent<RectTransform>().sizeDelta = cells[x, y].GetComponent<RectTransform>().sizeDelta;
        targetPosition = new Vector2(x, y);
    }

    public void MoveCat(int dx, int dy)
    {
        int newX = (int)catPosition.x + dx;
        int newY = (int)catPosition.y + dy;

        if (newX >= 0 && newX < fieldSize && newY >= 0 && newY < fieldSize)
        {
            // Check for obstacle
            if (cells[newX, newY].transform.Find("Obstacle") == null)
            {
                cat.transform.SetParent(cells[newX, newY].transform, false);
                catPosition = new Vector2(newX, newY);

                moveCount++;

                // Если кот достиг цели
                if (catPosition == targetPosition)
                {
                    // Рассчитываем очки
                    if (moveCount <= 7)
                    {
                        score = Random.Range(100, 151);
                    }
                    else if (moveCount <= 10)
                    {
                        score = Random.Range(75, 101);
                    }
                    else if (moveCount <= 15)
                    {
                        score = Random.Range(40, 76);
                    }
                    else
                    {
                        score = Random.Range(5, 41);
                    }

                    // Загружаем текущий счет из PlayerPrefs
                    int currentScore = PlayerPrefs.GetInt("Score", 0);

                    // Добавляем к текущему счету очки за эту игру
                    currentScore += score;

                    // Сохраняем обновленный счет обратно в PlayerPrefs
                    PlayerPrefs.SetInt("Score", currentScore);
                    scoreText.text = score.ToString();

                    // Активируем панель победы
                    winPanel.SetActive(true);

                    // Проигрываем звук победы
                    AudioSource audioSource = FindObjectOfType<AudioSource>();
                    if (audioSource != null)
                    {
                        audioSource.clip = winSound;
                        audioSource.Play();
                    }
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
#endregion
}