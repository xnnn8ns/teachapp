using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BottomMenuScript : MonoBehaviour
{
    [SerializeField] private GameObject[] _backButtons;
    [SerializeField] private GameObject[] _scenes;

    private int _currentButtons;

    public void Buttons(int number)
    {
        _backButtons[_currentButtons].SetActive(false);

        ReloadScenes(number, _currentButtons);

        _currentButtons = number;
        
        _backButtons[_currentButtons].SetActive(true);        
    }

    private void ReloadScenes(int numberScenes, int previousScene) 
    {
        if(numberScenes == 0)
        {
            SceneManager.LoadScene("LoginForm", LoadSceneMode.Single);
            return;
        }
        _scenes[previousScene].SetActive(false);

        _scenes[numberScenes].SetActive(true);
    }
}
