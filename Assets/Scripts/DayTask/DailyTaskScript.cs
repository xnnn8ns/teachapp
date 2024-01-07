using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using TMPro;
using System.Collections;

public class DailyTaskScript : MonoBehaviour
{
    [Serializable]
    public class UserData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int LevelsCompleted { get; set; }
        public int Score { get; set; }
        public int Rank { get; set; }
        public string StartTime { get; set; }
        public int InitialLevelsCompleted { get; set; }
        public int InitialScore { get; set; }
        public string LastUpdateDate { get; set; }
        public string ElapsedTime { get; set; }
    }

    [SerializeField]
    private Slider _levelSlider;
    [SerializeField]
    private Slider _scoreSlider;
    [SerializeField]
    private Slider _timeSlider;
    private List<UserData> _data = new List<UserData>();
    private Stopwatch _stopwatch;
    private DateTime _date;

    private async void Start()
    {
        LoadQuitTime();
        _stopwatch = new Stopwatch();
        _date = DateTime.Today;
        _timeSlider.maxValue = 600;
        await LoadData();
        var currentUserId = PlayerPrefs.GetInt("id");
        var user = GetUserById(currentUserId);
        if (user != null)
        {
            UpdateUserForNewDay(user);
            LoadElapsedTime(user);
            CheckConditions();
        }
        StartCoroutine(UpdateElapsedTime());
    }

    // Этот метод загружает время выхода из приложения из PlayerPrefs
    private void LoadQuitTime()
    {
        // Проверяем, есть ли сохраненное значение времени выхода в PlayerPrefs
        if (PlayerPrefs.HasKey("quitTime"))
        {
            // Получаем сохраненное значение времени выхода
            var quitTimeStr = PlayerPrefs.GetString("quitTime");
            // Преобразуем строку в DateTime
            if (DateTime.TryParse(quitTimeStr, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var quitTime))
            {
                // Вычисляем количество секунд, прошедших с момента выхода из приложения до текущего момента
                var elapsedSeconds = (DateTime.UtcNow - quitTime).TotalSeconds;
            }
        }
    }
    // метод сброса прогресса ежедневно
    private void UpdateUserForNewDay(UserData user)
    {
        if (DateTime.TryParse(user.LastUpdateDate, out var lastUpdateDate))
        {
            if (lastUpdateDate.Date < DateTime.Today)
            {
                user.InitialScore = user.Score;
                user.InitialLevelsCompleted = user.LevelsCompleted;
                user.ElapsedTime = "0";
                user.LastUpdateDate = _date.ToString("yyyy-MM-dd");
                SaveDataWhenReady();
            }
        }
        else
        {
            UnityEngine.Debug.LogError($"Invalid {nameof(user.LastUpdateDate)} value: {user.LastUpdateDate}");
        }
    }

    // Этот метод загружает затраченное время из PlayerPrefs и устанавливает его для пользователя.
    private void LoadElapsedTime(UserData user)
    {
        // Проверяем, есть ли сохраненное значение времени в PlayerPrefs
        if (PlayerPrefs.HasKey("elapsedTime"))
        {
            // Получаем сохраненное значение времени
            var elapsedTimeFloat = PlayerPrefs.GetFloat("elapsedTime");
            // Создаем новый экземпляр Stopwatch и добавляем к нему сохраненное время
            _stopwatch = Stopwatch.StartNew();
            _stopwatch.Elapsed.Add(TimeSpan.FromSeconds(elapsedTimeFloat));
            // Преобразуем сохраненное время в TimeSpan
            var storedElapsedTime = TimeSpan.FromSeconds(elapsedTimeFloat);
            // Устанавливаем затраченное время для пользователя
            user.ElapsedTime = storedElapsedTime.ToString();
        }
    }
    private async Task LoadData()
    {
        var path = Path.Combine(Application.persistentDataPath, "userData.json");
        if (File.Exists(path))
        {
            var jsonData = await File.ReadAllTextAsync(path);
            _data = JsonConvert.DeserializeObject<List<UserData>>(jsonData) ?? new List<UserData>();
        }
    }

    private UserData GetUserById(int id)
    {
        return _data.FirstOrDefault(user => user.Id == id);
    }
    // обновляем слайдеры
    private void CheckConditions()
    {
        foreach (var user in _data)
        {
            var dailyScore = user.Score - user.InitialScore;
            var dailyLevels = user.LevelsCompleted - user.InitialLevelsCompleted;
            if (float.TryParse(user.ElapsedTime, out var dailyTime))
            {
                _timeSlider.value = dailyTime;
            }

            _levelSlider.value = dailyLevels;
            _scoreSlider.value = dailyScore;
            _timeSlider.value = dailyTime;

            UpdateSliderFillVisibility(_levelSlider);
            UpdateSliderFillVisibility(_scoreSlider);
            UpdateSliderFillVisibility(_timeSlider);
        }
    }
    // Этот метод сохраняет данные в файл
    private async void SaveDataWhenReady()
    {
        // Ждем 5 секунд перед сохранением данных, чтобы избежать частых операций записи
        await Task.Delay(5000);
        var jsonData = JsonConvert.SerializeObject(_data);
        var path = Path.Combine(Application.persistentDataPath, "userData.json");
        await File.WriteAllTextAsync(path, jsonData);
    }
    // Этот метод должен вызываться при завершении уровня
    public void OnLevelCompleted()
    {
        var currentUserId = PlayerPrefs.GetInt("id");
        var user = GetUserById(currentUserId);
        if (user != null)
        {
             // Увеличиваем количество завершенных уровней
            user.LevelsCompleted++;
            // Обновляем данные пользователя для нового дня
            UpdateUserForNewDay(user);
            // Сохраняем данные
            SaveDataWhenReady();
        }
    }
    // Этот метод должен вызываться при получении очков
    public void OnScoreReceived(int score)
    {
        // Получаем текущего пользователя
        var currentUserId = PlayerPrefs.GetInt("id");
        var user = GetUserById(currentUserId);
        if (user != null)
        {
            // Увеличиваем счет
            user.Score += score;
            // Обновляем данные пользователя для нового дня
            UpdateUserForNewDay(user);
            SaveDataWhenReady();
        }
    }
    private void UpdateSliderFillVisibility(Slider slider)
    {
        var fillImage = slider.transform.Find("Fill Area/Fill").GetComponent<Image>();
        fillImage.enabled = slider.value != 0;
    }
    private IEnumerator UpdateElapsedTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            var currentUserId = PlayerPrefs.GetInt("id");
            var user = GetUserById(currentUserId);
            if (user != null)
            {
                user.ElapsedTime = _stopwatch.Elapsed.TotalSeconds.ToString();
            }
        }
    }
    private void Update()
    {
        var currentUserId = PlayerPrefs.GetInt("id");
        var user = GetUserById(currentUserId);
        if (user != null)
        {
            if (float.TryParse(user.ElapsedTime, out var dailyTime))
            {
                _timeSlider.value = dailyTime;
            }
            UpdateSliderFillVisibility(_timeSlider);
        }
    }
    private void OnApplicationQuit()
    {
        SaveElapsedTime();
        SaveDataWhenReady();
    }

    private void SaveElapsedTime()
    {
        var currentUserId = PlayerPrefs.GetInt("id");
        var user = GetUserById(currentUserId);
        if (user != null)
        {
            PlayerPrefs.SetString("elapsedTime", user.ElapsedTime);
        }
    }
}