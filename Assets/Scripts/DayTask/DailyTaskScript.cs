using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using TMPro;
using System.Collections;

public class DailyTaskScript : MonoBehaviour
{
    [SerializeField]
    private Slider _levelSlider;
    [SerializeField]
    private Slider _scoreSlider;
    [SerializeField]
    private Slider _timeSlider;
    [SerializeField]
    private Button rewardButton;
    [SerializeField]
    private GameObject _allTasksCompleteWindow;
    [SerializeField]
    private ParticleSystem _rewardParticleSystem;
    private List<UserData> _data = new List<UserData>();
    private DateTime _date;
    private int _initialScore;
    private int _initialLevel;

    private void Start()
    {
        UserData.LoadUserData();

        _date = DateTime.Today;
        _timeSlider.maxValue = 600;
        _scoreSlider.maxValue = 30;

        // Загрузка начального счета и уровня из PlayerPrefs
        _initialScore = PlayerPrefs.GetInt("InitialScore", UserData.Score);
        _initialLevel = PlayerPrefs.GetInt("InitialLevel", UserData.InitialLevel);

        if (ComonFunctions.UnixTimeStampToDateTime(UserData.LastUpdateDate).Date < DateTime.Today)
        {
            UserData.UpdateUserForNewDay();
            _allTasksCompleteWindow.SetActive(false);
        }

        SetElapsedTime();
        CheckConditions();
    }

    private void SetElapsedTime()
    {
        UserData.LoadUserData();
        _timeSlider.value = UserData.ElapsedTime;
        Debug.Log(UserData.ElapsedTime);
        UpdateSliderFillVisibility(_timeSlider);
    }

    public static void SaveElapsedTime(int secondsFromSceneStart)
    {
        UserData.ElapsedTime += secondsFromSceneStart;
        UserData.SetLastUpdateDate();
        UserData.SetElapsedTime();
    }

    private void CheckConditions()
    {
        Debug.Log(UserData.Score);
        Debug.Log(UserData.InitialScore);
        var dailyScore = UserData.Score - _initialScore;
        _initialLevel = DataLoader.GetCurrentLevel();
        bool isLevelCompleted = _initialLevel < UserData.InitialLevel;
        _timeSlider.value = UserData.ElapsedTime;
        _levelSlider.value = isLevelCompleted ? 1f : 0f;
        _scoreSlider.value = dailyScore;

        UpdateSliderFillVisibility(_levelSlider);
        UpdateSliderFillVisibility(_scoreSlider);
        UpdateSliderFillVisibility(_timeSlider);

        if (_levelSlider.value == 1f && _scoreSlider.value == 30 && _timeSlider.value == 600 && !UserData.HasReceivedReward)
        {
            if (!UserData.HasReceivedReward)
            {
                rewardButton.gameObject.SetActive(true);
            }
            else
            {
                rewardButton.gameObject.SetActive(false);
            }
        }
    }

    private void UpdateSliderFillVisibility(Slider slider)
    {
        var fillImage = slider.transform.Find("Fill Area/Fill").GetComponent<Image>();
        fillImage.enabled = slider.value != 0;
    }

    public void OnRewardButtonClicked()
    {
        if (!UserData.HasReceivedReward)
        {
            // Игрок еще не получил награду в этот день, дать ему награду
            GiveReward();
            UserData.HasReceivedReward = true;
            
        }
        else
        {
            // Игрок уже получил награду в этот день
            Debug.Log("Вы уже получили награду за сегодня!");
        }
    }

    private void GiveReward()
    {
        // Генерируем случайное число от 20 до 50
        int reward = UnityEngine.Random.Range(20, 51);
        UserData.Score += reward;
        UserData.SetScore(UserData.Score);

        // Помечаем, что игрок получил награду
        UserData.HasReceivedReward = true;
        UserData.SetHasReceivedReward(UserData.HasReceivedReward);

        // Деактивируем кнопку получения награды
        rewardButton.gameObject.SetActive(false);
        
        _allTasksCompleteWindow.SetActive(true);
    }
}