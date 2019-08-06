using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameOverScene : MonoBehaviour
{
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

    public void ContinueButtonClicked()
    {
        StartCoroutine(LoadScene("LoadingScreen"));
    }

    public void ExitButtonClicked()
    {
        StartCoroutine(LoadScene("MainMenu"));
    }

    private IEnumerator LoadScene(string name)
    {
        transitionAnimator.SetBool("SceneEnd", true);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(name);
    }
}
