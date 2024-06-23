using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TopPanel : MonoBehaviour
{
    [SerializeField]
    private TMP_Text courseTitle;
    [SerializeField]
    private TMP_Text pointsText;

    private void Start()
    {
        //SetPoints();
        //SetTitle();
        UpdateTitles();
    }

    private void SetPoints()
    {
        pointsText.text = PlayerPrefs.GetInt("Score", 0).ToString();
    }

    private void SetTitle()
    {
        courseTitle.text = LangAsset.GetValueByKey("PythonBasic");
    }

    public void UpdateTitles()
    {
        SetPoints();
        SetTitle();
    }
}
