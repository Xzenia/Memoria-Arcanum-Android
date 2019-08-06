using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class IntroductionCutscene : MonoBehaviour
{
    public Sprite[] cutscenes;
    private int cutsceneIndex;

    public Animator transitionAnimator;

    public AudioSource backgroundMusic;

    void Start()
    {
        cutsceneIndex = 0;
        gameObject.GetComponent<Image>().sprite = cutscenes[cutsceneIndex];

        if (PlayerPrefs.GetInt("Music") == 1)
        {
            backgroundMusic.Play();
        }
        else
        {
            backgroundMusic.Stop();
        }
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

    private IEnumerator LoadScene()
    {
        transitionAnimator.SetBool("SceneEnd", true);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("CharacterSelection");
    }
}
