using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour
{
    public AudioSource backgroundMusic;

    [SerializeField]
    public AudioClip enchantedForest;
    public AudioClip crystalCaverns;
    public AudioClip templeOfXartha;

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

    public AudioSource wrongMatch;
    public AudioSource correctMatch;
    public AudioSource spellAttack;
    public AudioSource normalAttack;
    public AudioSource enemyAttack;
    public AudioSource critical;
    public AudioSource healUp;
    public AudioSource chargeFull;

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

    public void PlayEnemyAttackSound()
    {
        enemyAttack.pitch = Random.Range(0.85f, 1f);
        enemyAttack.Play();
    }

    public void PlayHeroAttackSound(Player player)
    {
        spellAttack.pitch = Random.Range(0.85f, 90f);

        if (player.name.Equals("Rikko"))
        {
            spellAttack.Play();
        }
        else
        {
            normalAttack.Play();
        }
    }

    public void PlayCorrectMatchSound()
    {
        correctMatch.pitch = Random.Range(0.85f, 1f);
        correctMatch.Play();
    }

    public void PlayWrongMatchSound()
    {
        wrongMatch.pitch = Random.Range(0.85f, 1f);
        wrongMatch.Play();
    }
}
