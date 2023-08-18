using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LangAsset : MonoBehaviour
{
    private static List<LangItem> instance;
    public static LangLocation CurrentLangLocation = LangLocation.Ru;

    private LangAsset()
    { }

    public static List<LangItem> getInstance()
    {
        if (instance == null)
        {
            instance = new List<LangItem>();
            ImportLang();
        }
        return instance;
    }

    private static void ImportLang()
    {
        getInstance().Clear();
        var dataset = Resources.Load<TextAsset>("lang_assets");
        var splitDataset = dataset.text.Split(new char[] { '\n' });
        for (var i = 0; i < splitDataset.Length; i++)
        {
            string[] row = splitDataset[i].Split(new char[] { ';' });
            LangItem lang = new LangItem();
            for (int k = 0; k < row.Length; k++)
            {
                switch (k)
                {
                    case 0:
                        lang.Key = row[k];
                        break;
                    case 1:
                        lang.Ru = row[k];
                        break;
                    case 2:
                        lang.En = row[k];
                        break;
                    default:
                        break;
                }
            }

            getInstance().Add(lang);
        }
    }

    public static string GetValueByKey(string key)
    {
        List<LangItem> langList = getInstance();
        foreach (var item in langList)
        {
            if (item.Key == key)
            {
                return item.GetType().GetField(CurrentLangLocation.ToString()).GetValue(item).ToString();
            }
        }
        return "";
    }
}

public class LangItem
{
    public string Key = "";
    public string Ru = "";
    public string En = "";
}

public enum LangLocation
{
    Ru = 1,
    En = 2
}