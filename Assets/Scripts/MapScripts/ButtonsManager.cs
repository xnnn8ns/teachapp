using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class ButtonData
{
    public int id;
    public int score;
    public bool isActive;
    public int activeStarsCount;
    public bool isPassed;
}

public class ButtonDataList
{
    public List<ButtonData> buttons = new List<ButtonData>();
}
public class ButtonsManager : MonoBehaviour
{
    string json;

    List<ButtonData> buttonDataList = new List<ButtonData>(); // ??????? ?????? ??? ???????? ?????? ? ???????

    public int rx = 2;
    void CreateDefaultButtons(int id)
    {
        ButtonData buttonData = new ButtonData()
        {
            id = id,
            score = 0,
            isActive = false,
            activeStarsCount=0,
            isPassed = false
        };
        buttonDataList.Add(buttonData);

    }

    public void CreateAllButtons(int id)
    {
        CreateDefaultButtons(id);
        json = JsonConvert.SerializeObject(buttonDataList, Formatting.Indented);
        File.WriteAllText(Application.dataPath + "/Map/Resources/buttonData.json", json);
    }

   
}

