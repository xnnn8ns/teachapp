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
    string jsonFilePath = "/Map/Resources/buttonData.json";

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

        //Debug.Log("Button data saved to buttonData.json");
        //Debug.Log(Application.dataPath + "/Map/Resources/buttonData.json");

    }

    public void SetData(int id, int _score, bool _isActive, bool _isPassed, int _activeStarsCount)
    {
        
        string json = File.ReadAllText(Application.dataPath + jsonFilePath);

        
        buttonDataList = JsonConvert.DeserializeObject<List<ButtonData>>(json);

        
        ButtonData buttonData = buttonDataList.Find(item => item.id == id);

        if (buttonData != null)
        {
            
            buttonData.score = _score;
            buttonData.isActive = _isActive;
            buttonData.isPassed = _isPassed;
            buttonData.activeStarsCount = _activeStarsCount;
            
            

            
            json = JsonConvert.SerializeObject(buttonDataList, Formatting.Indented);

            
            File.WriteAllText(Application.dataPath + jsonFilePath, json);

            //Debug.Log("Button data updated and saved to buttonData.json");
        }
        else
        {
            Debug.LogError("Button data with id " + id + " not found!");
        }
    }

    public ButtonData GetData(int id)
    {
        
        string json = File.ReadAllText(Application.dataPath + jsonFilePath);

        
        buttonDataList = JsonConvert.DeserializeObject<List<ButtonData>>(json);

        
        ButtonData buttonData = buttonDataList.Find(item => item.id == id);

        if (buttonData != null)
        {
            
            return buttonData;
        }
        else
        {
            Debug.LogError("Button data with id " + id + " not found!");
            return null;
        }
    }
}

