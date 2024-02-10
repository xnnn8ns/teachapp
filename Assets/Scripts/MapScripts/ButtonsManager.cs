using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class ButtonData
{
    public int id;
    public int level;
    public int score;
    public bool isActive;
    public int activeStarsCount;
    public int passCount;
    public bool isPassed;
    public int typeLevel;
    public int topic;

    public int totalForPassCount;
}

public class ButtonDataList
{
    public List<ButtonData> buttons = new List<ButtonData>();
}
public class ButtonsManager : MonoBehaviour
{
    //string json;

    //List<ButtonData> buttonDataList = new List<ButtonData>(); 

    public void CreateAllButtons()
    {
        DataLoader.GetDataAllButtons();
        //Debug.Log("-----CreateAllButtons");
        //if (File.Exists(Application.persistentDataPath + Settings.jsonButtonFilePath))
        //    json = File.ReadAllText(Application.persistentDataPath + Settings.jsonButtonFilePath);
        //else
        //{
        //    FileStream fs = File.Create(Application.persistentDataPath + Settings.jsonButtonFilePath);
        //    fs.Dispose();
        //    //TextAsset txt = (TextAsset)Resources.Load("buttonData", typeof(TextAsset));
        //    //json = txt.text;
        //    //buttonDataList = JsonConvert.DeserializeObject<List<ButtonData>>(json);
        //    int topicID = 1;
        //    int levelID = 1;
        //    int scoreCurrent;
        //    int scoreLevel = 20;
        //    int scoreFinal = 80;
        //    int totalForPassCount = 3;
        //    for (int i = 1; i <= 120; i++)
        //    {
        //        bool isActiveButton = false;
        //        if (i == 1)
        //            isActiveButton = true;
        //        int typeLevelButton;
        //        if (levelID == 10)
        //        {
        //            typeLevelButton = (int)ETypeLevel.final;
        //            scoreCurrent = scoreFinal + scoreLevel;
        //            totalForPassCount = 1;
        //        }
        //        else if (levelID == 11)
        //        {
        //            typeLevelButton = (int)ETypeLevel.mission1;
        //            scoreCurrent = 2 * scoreLevel;
        //            totalForPassCount = 1;
        //        }
        //        else if (levelID == 12)
        //        {
        //            typeLevelButton = (int)ETypeLevel.mission2;
        //            scoreCurrent = 2 * scoreLevel;
        //            totalForPassCount = 1;
        //        }
        //        else if (levelID == 4 || levelID == 7)
        //        {
        //            typeLevelButton = (int)ETypeLevel.additional;
        //            scoreCurrent = scoreLevel;
        //            totalForPassCount = 1;
        //        }
        //        else
        //        {
        //            typeLevelButton = (int)ETypeLevel.simple;
        //            scoreCurrent = scoreLevel;
        //            totalForPassCount = 3;
        //        }

        //        Debug.Log("CreateAllButtons" + i.ToString() + "-" + typeLevelButton.ToString()+"-"+ topicID.ToString()+"-"+ scoreCurrent.ToString());
        //        ButtonData buttonData = new ButtonData()
        //        {
        //            id = i,
        //            score = scoreCurrent,
        //            isActive = isActiveButton,
        //            activeStarsCount = 0,
        //            passCount = 0,
        //            totalForPassCount = totalForPassCount,
        //            isPassed = false,
        //            typeLevel = typeLevelButton,
        //            topic = topicID,
        //            level = levelID
        //        };
        //        buttonDataList.Add(buttonData);
        //        if (i % 12 == 0)
        //        {
        //            topicID++;
        //            levelID = 1;
        //            scoreLevel += 10;
        //            scoreFinal += 20;
        //        }else
        //            levelID++;

        //    }
        //    //Debug.Log(json);
        //    json = JsonConvert.SerializeObject(buttonDataList, Formatting.Indented);
        //    File.WriteAllText(Application.persistentDataPath + Settings.jsonButtonFilePath, json);
        //}
    }

   
}

