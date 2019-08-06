using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
public class EndingScene : MonoBehaviour
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

    public void EndingPanelTapped()
    {
        StartCoroutine(LoadScene());
    }

    private IEnumerator LoadScene()
    {
        transitionAnimator.SetBool("SceneEnd", true);
        yield return new WaitForSeconds(1f);

        GameData.DeleteData();

        SceneManager.LoadScene("MainMenu");
    }
}
