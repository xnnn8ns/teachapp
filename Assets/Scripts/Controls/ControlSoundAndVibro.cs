using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlSoundAndVibro : MonoBehaviour
{
    [SerializeField]
    private Image soundIcon;
    [SerializeField]
    private Sprite soundOnSprite;
    [SerializeField]
    private Sprite soundOffSprite;

    [SerializeField]
    private Image vibrationIcon;
    [SerializeField]
    private Sprite vibrationOnSprite;
    [SerializeField]
    private Sprite vibrationOffSprite;

    void Start()
    {
        UpdateIcons();
    }

    public void ToggleSound()
    {
        UserData.SoundEnabled = !UserData.SoundEnabled;
        AudioListener.volume = UserData.SoundEnabled ? 1 : 0;
        UserData.SaveSoundAndVibrationSettings(); // сохраняем изменения
        UpdateIcons();
    }

    public void ToggleVibration()
    {
        UserData.VibrationEnabled = !UserData.VibrationEnabled;
        UserData.SaveSoundAndVibrationSettings(); // сохраняем изменения
        UpdateIcons();
    }

    private void UpdateIcons()
    {
        soundIcon.sprite = UserData.SoundEnabled ? soundOnSprite : soundOffSprite;
        vibrationIcon.sprite = UserData.VibrationEnabled ? vibrationOnSprite : vibrationOffSprite;
    }
}