using System.Collections;
using TMPro;
using UnityEngine;

public class Game_Controller : MonoBehaviour {

    [SerializeField] private GameObject winPanel;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private AudioClip winSound;
    [SerializeField] private Animator animator;
    public AudioSource audioSource;
    private GameObject Player;

    private float Max_Height = 0;
    public TextMeshProUGUI Txt_Score;

    private int Score;

    private Vector3 Top_Left;
    private Vector3 Camera_Pos;

    private bool Game_Over = false;

    public TextMeshProUGUI Txt_GameOverScore;

	void Awake () 
    {
        Player = GameObject.Find("Cat");

        // инициализируем границы
        Camera_Pos = Camera.main.transform.position;
        Top_Left = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
	}
	
	void FixedUpdate () 
    {
        if(!Game_Over)
        {
            // Высчитываем максимальную высоту
            if (Player.transform.position.y > Max_Height)
            {
                Max_Height = Player.transform.position.y;
            }

            // проверяем проигрыш (путем высоты)
            if (Player.transform.position.y - Camera.main.transform.position.y < Get_DestroyDistance())
            {
                // проигрываем звук падения
                GetComponent<AudioSource>().Play();
                
                // Запускаем game_over
                StartCoroutine(Set_GameOver());
                Game_Over = true;
            }
        }
	}

    void OnGUI()
    {
        // Устанавливаем счет
        Score = (int)(Max_Height * 50);
        Txt_Score.text = Score.ToString();
    }

    public bool Get_GameOver()
    {
        return Game_Over;
    }

    public float Get_DestroyDistance()
    {
        return Camera_Pos.y + Top_Left.y;
    }

    IEnumerator Set_GameOver()
    {
        GameObject Background_Canvas = GameObject.Find("Background_Canvas");
        yield return new WaitForSeconds(1.5f);

        // Включаем все объекты в Background_Canvas
        foreach (Transform child in Background_Canvas.transform)
        {
            child.gameObject.SetActive(true);
        }

        Txt_GameOverScore.text = Score.ToString();
        Background_Canvas.GetComponent<Animator>().enabled = true;
    }

    public void StartWin() {
        StartCoroutine(WinCoroutine());
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
