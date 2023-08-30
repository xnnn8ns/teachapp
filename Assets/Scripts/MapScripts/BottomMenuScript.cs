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
        if(numberScenes == 2)
        {
            if (UserData.IsHaveLoginUserData())
                SceneManager.LoadScene("UserForm", LoadSceneMode.Additive);
            else
                SceneManager.LoadScene("LoginForm", LoadSceneMode.Additive);

            return;
        }
        foreach (var item in SceneManager.GetAllScenes())
        {
            if(item.name != "MapScene")
                SceneManager.UnloadSceneAsync(item);
        }
        _scenes[0].SetActive(false);
        _scenes[1].SetActive(false);
        _scenes[2].SetActive(false);

        //_scenes[previousScene].SetActive(false);

        _scenes[numberScenes].SetActive(true);
    }

    public void JoinTeamButtonClick()
    {
        SceneManager.LoadScene("SelectTeam", LoadSceneMode.Additive);
    }

}
