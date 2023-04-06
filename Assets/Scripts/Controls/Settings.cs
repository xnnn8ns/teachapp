using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public static bool IsNeedDragDropTouchDetector = false;
    public static bool IsTurnOnClickPointerListener = false;

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
}
