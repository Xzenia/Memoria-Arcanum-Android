using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public AudioSource backgroundMusic;

    [SerializeField]
    public AudioClip mainMenuMusic;
    public AudioClip enchantedForest;
    public AudioClip crystalCaverns;
    public AudioClip templeOfXartha;
    public AudioClip gameOverMusic;

    public AudioClip RikkouBasicAttack;
    public AudioClip RikkouPrimaryAttack;
    public AudioClip RikkouAlternateAttack;
    public AudioClip RikkouSpecialAttack;

    public AudioClip EmilyBasicAttack;
    public AudioClip EmilyPrimaryAttack;
    public AudioClip EmilyAlternateAttack;
    public AudioClip EmilySpecialAttack;

    public AudioClip ShouBasicAttack;
    public AudioClip ShouPrimaryAttack;
    public AudioClip ShouAlternateAttack;
    public AudioClip ShouSpecialAttack;

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
