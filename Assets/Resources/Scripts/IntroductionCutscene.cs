using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class IntroductionCutscene : MonoBehaviour
{
    public Sprite[] cutscenes;
    private int cutsceneIndex;

    public Animator transitionAnimator;

    void Start()
    {
        cutsceneIndex = 0;
        this.gameObject.GetComponent<Image>().sprite = cutscenes[cutsceneIndex];
    }

    public void NextCutscene()
    {
        cutsceneIndex++;
        if (cutsceneIndex >= cutscenes.Length)
        {
            StartCoroutine(LoadScene());
        }
        else
        {
            this.gameObject.GetComponent<Image>().sprite = cutscenes[cutsceneIndex];
        }

    }

    IEnumerator LoadScene()
    {
        transitionAnimator.SetBool("SceneEnd", true);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("CharacterSelection");
    }
}
