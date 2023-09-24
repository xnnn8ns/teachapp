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
    public int typeLevel;
}

public class ButtonDataList
{
    public List<ButtonData> buttons = new List<ButtonData>();
}
public class ButtonsManager : MonoBehaviour
{
    string json;

    List<ButtonData> buttonDataList = new List<ButtonData>(); 

    public int rx = 2;
    void CreateDefaultButtons(int id)
    {
        ButtonData buttonData = new ButtonData()
        {
            id = id,
            score = 0,
            isActive = false,
            activeStarsCount=0,
            isPassed = false,
            typeLevel = 0
        };
        buttonDataList.Add(buttonData);

    }

    public void CreateAllButtons()
    {
        //CreateDefaultButtons(id);
        //json = JsonConvert.SerializeObject(buttonDataList, Formatting.Indented);
        //File.WriteAllText(Application.persistentDataPath + Settings.jsonButtonFilePath, json);

        if (File.Exists(Application.persistentDataPath + Settings.jsonButtonFilePath))
            json = File.ReadAllText(Application.persistentDataPath + Settings.jsonButtonFilePath);
        else
        {
            FileStream fs = File.Create(Application.persistentDataPath + Settings.jsonButtonFilePath);
            fs.Dispose();
            TextAsset txt = (TextAsset)Resources.Load("buttonData", typeof(TextAsset));
            json = txt.text;
            //Debug.Log(json);
            File.WriteAllText(Application.persistentDataPath + Settings.jsonButtonFilePath, json);
        }
    }

   
}

