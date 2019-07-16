using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DemoCompleteScene : MonoBehaviour
{
    public TMPro.TextMeshProUGUI matchesCounter;

    public Animator transitionAnimator;

    void Start()
    {
        matchesCounter.text = "Matches: " + GameScene.matches;
    }

    public void DemoPanelTapped()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        transitionAnimator.SetBool("SceneEnd", true);
        yield return new WaitForSeconds(1.5f);

        PlayerPrefs.DeleteKey("level");
        SceneManager.LoadScene("MainMenu");
    }
}
