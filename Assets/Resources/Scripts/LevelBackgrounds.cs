using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelBackgrounds : MonoBehaviour
{
    public Image levelTop;
    public Image levelBottom;

    public Sprite enchantedForestTop;
    public Sprite enchantedForestBottom;

    public Sprite crystalCavernsTop;
    public Sprite crystalCavernsBottom;

    public Sprite templeOfXarthaTop;
    public Sprite templeOfXarthaBottom;

    void Start()
    {
        int level = PlayerPrefs.GetInt("level");

        switch (level)
        {
            case 1:
                levelTop.sprite = enchantedForestTop;
                levelBottom.sprite = enchantedForestBottom;
                break;
            case 2:
                levelTop.sprite = crystalCavernsTop;
                levelBottom.sprite = crystalCavernsBottom;
                break;
            case 3:
                levelTop.sprite = templeOfXarthaTop;
                levelBottom.sprite = templeOfXarthaBottom;
                break;
            default:
                Debug.LogError("LevelBackgrounds: Switch case defaulted! Reverting to Enchanted Forest.");
                levelTop.sprite = enchantedForestTop;
                levelBottom.sprite = enchantedForestBottom;
                break;
        }   
    }
}
