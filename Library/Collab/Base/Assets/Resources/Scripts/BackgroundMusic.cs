using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public AudioSource backgroundMusic;

    public AudioClip enchantedForest;
    public AudioClip crystalCaverns;
    public AudioClip templeOfXartha;

    // Start is called before the first frame update
    void Start()
    {
        int level = PlayerPrefs.GetInt("level");
        
        switch (level)
        {
            case 1:
                backgroundMusic.clip = enchantedForest;
                break;
            case 2:
                backgroundMusic.clip = crystalCaverns;
                break;
            case 3:
                backgroundMusic.clip = templeOfXartha;
                break;
            default:
                Debug.LogError("BackgroundMusic: Switch case defaulted! Switching to enchantedForest.");
                backgroundMusic.clip = enchantedForest;
                break;
        }

        backgroundMusic.Play();
    }
}
