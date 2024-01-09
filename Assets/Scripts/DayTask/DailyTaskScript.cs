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
    private List<UserData> _data = new List<UserData>();
    private DateTime _date;

    private void Start()
    {
        _date = DateTime.Today;
        _timeSlider.maxValue = 600;
        _scoreSlider.maxValue = 30;
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
        var dailyScore = UserData.Score - UserData.InitialScore;
        bool isLevelCompleted = UserData.InitialLevel < DataLoader.GetCurrentLevel();
        _timeSlider.value = UserData.ElapsedTime;
        _levelSlider.value = isLevelCompleted ? 1f : 0f;
        _scoreSlider.value = dailyScore;

        UpdateSliderFillVisibility(_levelSlider);
        UpdateSliderFillVisibility(_scoreSlider);
        UpdateSliderFillVisibility(_timeSlider);
    }

    private void UpdateSliderFillVisibility(Slider slider)
    {
        var fillImage = slider.transform.Find("Fill Area/Fill").GetComponent<Image>();
        fillImage.enabled = slider.value != 0;
        
    }
}