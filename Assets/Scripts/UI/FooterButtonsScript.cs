using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FooterButtonsScript : MonoBehaviour
{
    public void ClickCallLeaderBoard()
    {
        SceneManager.LoadSceneAsync("LeaderBoard", LoadSceneMode.Single);
    }
}
