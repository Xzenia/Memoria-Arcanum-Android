using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuScene : MonoBehaviour
{
    public Animator transitionAnimator;
    public void PlayButtonClicked()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        transitionAnimator.SetBool("SceneEnd", true);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("IntroductionCutscene");
    }

    public void ExitButtonClicked()
    {
        Application.Quit();
    }
}
