using Newtonsoft.Json;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public const float SCALE_CHAR = 0.175f;
    public const float SCALE_SURFACE_MAX = 1.5f;
    public const float SCALE_SURFACE_MIN = 0.25f;

    public static bool IsNeedDragDropTouchDetector = false;
    public static bool IsTurnOnClickPointerListener = false;
    public static int Current_Level = 1;
    public static int Current_Theme = 0;
    public const string jsonFilePath = "/Map/Resources/buttonData.json";

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

}
