using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class EndingScene : MonoBehaviour
{
    public Animator transitionAnimator;

    public void EndingPanelTapped()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        transitionAnimator.SetBool("SceneEnd", true);
        yield return new WaitForSeconds(1.5f);

        PlayerPrefs.DeleteKey("Level");
        SceneManager.LoadScene("MainMenu");
    }
}
