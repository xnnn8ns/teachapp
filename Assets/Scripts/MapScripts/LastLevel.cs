using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using Mkey;

public class LastLevel : MonoBehaviour
{
    public static bool IsLastLevelCompleted = false;
    public static int lastLevelIndex = -1;
    public Sprite certificateSprite;
    private LevelButton previousLevel;

    void Start()
    {
        StartCoroutine(InitializeLastLevel());
    }

    public IEnumerator InitializeLastLevel()
    {
        yield return new WaitForSeconds(0.1f);
        List<Biome> biomes = new List<Biome>(FindObjectsOfType<Biome>());
        biomes.Sort((a, b) => (int)(b.transform.position.y - a.transform.position.y));

        Biome lastBiome = biomes[biomes.Count - 1]; // последний биом
        LevelButton lastLevel = lastBiome.levelButtons[lastBiome.levelButtons.Count - 1]; // последний уровень

        // Обновляем lastLevelIndex
        lastLevelIndex = lastBiome.levelButtons.Count - 1;

        IsLastLevelCompleted = lastLevel.Interactable;
        if (certificateSprite != null)
        {
            // Получаем дочерний объект ImageButton
            Image imageButton = lastLevel.transform.Find("ImageButton").GetComponent<Image>();
            imageButton.sprite = certificateSprite;
            // Если предыдущий уровень не пройден, делаем текущий уровень серым
            if (previousLevel != null && !previousLevel.Interactable)
            {
                imageButton.color = Color.gray;
            }
            // Если предыдущий уровень пройден, возвращаем текущему уровню его исходный цвет
            else
            {
                imageButton.color = Color.white;
            }
        }
    }
}