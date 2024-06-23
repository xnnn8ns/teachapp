using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlSettingsMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject settingsMenu;

    void Start()
    {
        settingsMenu.SetActive(false); // скрываем меню настроек при старте
    }

    public void ToggleSettingsMenu()
    {
        settingsMenu.SetActive(!settingsMenu.activeSelf); // переключаем видимость меню настроек
    }
}