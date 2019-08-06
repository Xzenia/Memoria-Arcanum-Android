using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class CharacterSelectionScene : MonoBehaviour
{
    public Player shou;
    public Player rikko;
    public Player emily;

    public Animator transitionAnimator;

    public AudioSource backgroundMusic;

    void Start()
    {
        if (PlayerPrefs.GetInt("Music") == 1)
        {
            backgroundMusic.Play();
        }
        else
        {
            backgroundMusic.Stop();
        }
    }

    public void CharacterSelected()
    {
        if (EventSystem.current.currentSelectedGameObject.name.Equals("Shou"))
        {
            GameScene.player = shou;
        }
        else if (EventSystem.current.currentSelectedGameObject.name.Equals("Rikko"))
        {
            GameScene.player = rikko;
        }
        else if (EventSystem.current.currentSelectedGameObject.name.Equals("Emily"))
        {
            GameScene.player = emily;
        }
        else
        {
            Debug.LogError("CharacterSelected(): Button name is invalid! Defaulting to Shou...");
            GameScene.player = shou;
        }

        StartCoroutine(LoadScene());
    }

    private IEnumerator LoadScene()
    {
        transitionAnimator.SetBool("SceneEnd", true);

        if (backgroundMusic.isPlaying)
        {
            backgroundMusic.Stop();
        }

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene("LoadingScreen");
    }
}
