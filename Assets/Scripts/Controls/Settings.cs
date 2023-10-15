using Newtonsoft.Json;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public const float SCALE_CHAR = 0.2f;// 0.166f;
    public const float SCALE_SURFACE_MAX_SHELF = 4.0f;// 1.25f;
    public const float SCALE_SURFACE_MAX = 1.75f;// 1.25f;
    public const float SCALE_SURFACE_MIN = 0.4f;// 0.25f;

    public static bool IsNeedDragDropTouchDetector = false;
    public static bool IsTurnOnClickPointerListener = false;
    public static int Current_ButtonOnMapID = 1;
    public static bool IsMisionClicked = false;
    public static int Current_Topic = 1;
    //public const string jsonFilePath = "/Map/Resources/buttonData.json";
    public const string jsonButtonFilePath = "/buttonData.json";
    public const string jsonTestFilePath = "/testRawData.json";
    public const string jsonTaskFilePath = "/taskData.json";
    public const string theoryFilePath = "/theoryTask-";

    public static bool IsModalWindowOpened = false;

    public static void SetClickQuestionSettings()
    {
        IsNeedDragDropTouchDetector = false;
        IsTurnOnClickPointerListener = true;
    }

    public static void SetDragDropQuestionSettings()
    {
        IsNeedDragDropTouchDetector = true;
        IsTurnOnClickPointerListener = false;
    }

    public static JsonSerializerSettings JsonSettings = new JsonSerializerSettings
    {
        NullValueHandling = NullValueHandling.Ignore,
        MissingMemberHandling = MissingMemberHandling.Ignore
    };

    public static int GetTopicFromButtonOnMapID(int buttonOnMapID)
    {
        return ((buttonOnMapID - 1)  / 10) + 1;
    }

    public static int GetLevelFromButtonOnMapID(int buttonOnMapID)
    {
        int levelCount = buttonOnMapID / 10;
        return buttonOnMapID - levelCount * 10;
    }
}
