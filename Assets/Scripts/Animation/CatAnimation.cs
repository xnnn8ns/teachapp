using Mkey;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatAnimation : MonoBehaviour
{
    void Update()
    {
        MapController mapController = MapController.Instance;
        List<Biome> biomes = new List<Biome>(FindObjectsOfType<Biome>());
        biomes.Sort((a, b) => (int)(b.transform.position.y - a.transform.position.y));

        bool isPreviousBlockCompleted = true;

        foreach (Biome biome in biomes)
        {
            bool isBlockCompleted = true;
            foreach (LevelButton button in biome.levelButtons)
            {
                bool isLevelActive = button.Interactable;
                if (!isLevelActive)
                {
                    isBlockCompleted = false;
                    break;
                }
            }

            Animator[] anims = biome.GetComponentsInChildren<Animator>();

            if (isPreviousBlockCompleted)
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

            isPreviousBlockCompleted = isBlockCompleted;
        }
    }
}