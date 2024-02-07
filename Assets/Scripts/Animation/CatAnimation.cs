using Mkey;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatAnimation : MonoBehaviour
{
    [SerializeField] private LastLevel lastLevel;

    private void OnEnable()
    {
        //CheckIsNeedAnimate();
        StartCoroutine(StartCheckIsNeedAnimate());
    }

    private IEnumerator StartCheckIsNeedAnimate()
    {
        yield return new WaitForSeconds(0.1f);
        CheckIsNeedAnimate();
        yield break;
    }

    private void CheckIsNeedAnimate()
    {
        List<LevelButton> firstLevels = lastLevel.GetFirstLevelsOfEachBlock();

        MapController mapController = MapController.Instance;
        List<Biome> biomes = new List<Biome>(FindObjectsOfType<Biome>());
        biomes.Sort((a, b) => (int)(b.transform.position.y - a.transform.position.y));

        for (int i = 0; i < biomes.Count; i++)
        {
            Biome biome = biomes[i];
            LevelButton firstLevel = biome.levelButtons[0];
            ButtonData buttonData = DataLoader.GetLevelData(firstLevel.ID);
            bool isFirstLevelCompleted = i == 0 || (firstLevels.Contains(firstLevel) && firstLevel.Interactable && buttonData != null && buttonData.passCount > 0);

            Animator[] anims = biome.GetComponentsInChildren<Animator>();

            if (isFirstLevelCompleted)
            {
                foreach (Animator anim in anims)
                {
                    anim.speed = 1;
                    anim.GetComponent<Image>().color = Color.white;
                }
            }
            else
            {
                foreach (Animator anim in anims)
                {
                    anim.speed = 0;
                    anim.Play(0, 0, 0);
                    anim.GetComponent<Image>().color = Color.gray;
                }
            }
        }
    }
}