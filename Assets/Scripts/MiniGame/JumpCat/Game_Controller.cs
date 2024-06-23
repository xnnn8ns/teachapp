using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class Game_Controller : MonoBehaviour {

    [SerializeField] private GameObject winPanel;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private AudioClip winSound;
    [SerializeField] private AudioClip winGameSound;
    [SerializeField] private AudioClip loseSound;
    [SerializeField] private Sprite YouWinSprite;
    [SerializeField] private Animator animator;
    public AudioSource audioSource;
    public AudioSource audioSourceWin;
    private GameObject Player;

    private float Max_Height = 0;
    private const int MaxScore = 20000;
    public TextMeshProUGUI Txt_Score;

    private int Score;

    private Vector3 Top_Left;
    private Vector3 Camera_Pos;

    private bool Game_Over = false;
    private bool platformsSpawned = true;
    private bool gameEnding = false;
    private bool controlEnabled = false;

    public TextMeshProUGUI Txt_GameOverScore;
    private Platform_Generator platformGenerator;

    void Awake () 
    {
        Player = GameObject.Find("Cat");
        if (Player == null)
        {
            Debug.LogError("Player not found");
            return;
        }

        platformGenerator = GetComponent<Platform_Generator>();
        if (platformGenerator == null)
        {
            Debug.LogError("Platform_Generator not found");
            return;
        }

        Camera_Pos = Camera.main.transform.position;
        Top_Left = Camera.main.ScreenToWorldPoint(Vector3.zero);

        StartCoroutine(EnableControlAfterDelay(1.0f));
    }
	
    void FixedUpdate () 
    {
        if(!Game_Over && controlEnabled)
        {
            // Высчитываем максимальную высоту
            if (Player.transform.position.y > Max_Height)
            {
                Max_Height = Player.transform.position.y;
            }

            // Обновляем счет
            Score = (int)(Max_Height * 50);
            Txt_Score.text = Score.ToString();

            // проверяем проигрыш (путем высоты)
            if (Player.transform.position.y - Camera.main.transform.position.y < Get_DestroyDistance())
            {
                // проигрываем звук падения
                GetComponent<AudioSource>().Play();
                
                // Запускаем game_over
                StartCoroutine(Set_GameOver());
                Game_Over = true;
            }

            // Проверяем, достиг ли игрок максимального количества очков
            if (Score >= MaxScore && platformsSpawned)
            {
                // Останавливаем спавн платформ
                platformGenerator.Generate_Platform(1);
                platformGenerator.ShouldSpawnPlatforms = false;
                platformsSpawned = false;
            }

            if (TopPlatform.gameNeedEnd && !gameEnding)
            {
                gameEnding = true;
                StartCoroutine(DelayedGameOver());
            }
        }
    }

    public bool Get_GameOver()
    {
        return Game_Over;
    }

    public float Get_DestroyDistance()
    {
        return Camera_Pos.y + Top_Left.y;
    }

    IEnumerator DelayedGameOver()
    {
        Rigidbody2D playerRigidbody = Player.GetComponent<Rigidbody2D>();
        playerRigidbody.velocity = Vector2.zero;
        playerRigidbody.gravityScale = 0;
        playerRigidbody.isKinematic = true;
    
        // Проигрываем звук победы
        audioSourceWin.PlayOneShot(winGameSound);
    
        yield return new WaitForSeconds(1.5f);
        
        Game_Over = true;
        StartCoroutine(Set_GameOver());
    }

    IEnumerator Set_GameOver()
    {
        GameObject Background_Canvas = GameObject.Find("Background_Canvas");
        yield return new WaitForSeconds(1.5f);

        if (Score < MaxScore * 0.3f) // Если игрок набрал меньше 30% от максимального счета
        {
            PlayerPrefs.SetInt("ErrorCount", Convert.ToInt32(Score / 1000));
            ComonFunctions.Instance.SetNextLevel(60, 30);

            // Воспроизводим звук проигрыша
            audioSourceWin.PlayOneShot(loseSound);

            StartCoroutine(LoadSceneAfterDelay("BonusSceneLose", 2f));

            yield break;
        }

        // Включаем все объекты в Background_Canvas
        foreach (Transform child in Background_Canvas.transform)
        {
            child.gameObject.SetActive(true);
        }

        // Если игрок достиг самого высокого счета, меняем спрайт объекта Game_Over
        if (gameEnding)
        {
            GameObject Game_Over = GameObject.Find("Game_Over");
            Game_Over.GetComponent<Image>().sprite = YouWinSprite;
        }

        Txt_GameOverScore.text = Score.ToString();
        Background_Canvas.GetComponent<Animator>().enabled = true;
    }

    public void StartWin() {
        StartCoroutine(WinCoroutine());
    }

    private IEnumerator LoadSceneAfterDelay(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName,LoadSceneMode.Single);
    }

    IEnumerator WinCoroutine()
    {
        PlayerPrefs.SetInt("ErrorCount", 0);
        ComonFunctions.Instance.SetNextLevel(60, 30);

        // Ждем 0.5 секунды
        yield return new WaitForSeconds(0.5f);

        winPanel.SetActive(true);

        audioSource.clip = winSound;
        audioSource.Play();

        // Запускаем анимацию
        animator.speed = 0.85f;
        animator.Play("Anim6");

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

    IEnumerator EnableControlAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        controlEnabled = true;
    }

    public int GetScore()
    {
        return Score;
    }

    public int GetMaxScore()
    {
        return MaxScore;
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
