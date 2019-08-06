using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MainMenuScene : MonoBehaviour
{
    public Animator transitionAnimator;

    public GameObject newGameButtonPanel;
    public GameObject continueGameButtonPanel;
    public GameObject optionsPanel;

    public Toggle musicToggle;
    public Toggle sfxToggle;

    public AudioSource backgroundMusic;

    void Start()
    {
        ShowMainMenuPanel();

        InitializeToggleButtons();

        if (PlayerPrefs.GetInt("Music") == 1)
        {
            backgroundMusic.Play();
        }
        else
        {
            backgroundMusic.Stop();
        }
    }

    private void InitializeToggleButtons()
    {
        switch (PlayerPrefs.GetInt("Music"))
        {
            case 0:
                musicToggle.isOn = false;
                break;
            case 1:
                musicToggle.isOn = true;
                break;
            default:
                Debug.LogError("musicToggle switch case defaulted! Setting PlayerPrefs key 'Music' to 1...");
                PlayerPrefs.SetInt("Music", 1);

                musicToggle.isOn = true;
                break;
        }

        switch (PlayerPrefs.GetInt("SFX"))
        {
            case 0:
                sfxToggle.isOn = false;
                break;
            case 1:
                sfxToggle.isOn = true;
                break;
            default:
                Debug.LogError("sfxToggle switch case defaulted! Setting PlayerPrefs key 'SFX' to 1...");
                PlayerPrefs.SetInt("SFX", 1);

                musicToggle.isOn = true;
                break;
        }
    }

    private void ShowMainMenuPanel()
    {
        if (GameData.GameDataExists())
        {
            continueGameButtonPanel.SetActive(true);
            newGameButtonPanel.SetActive(false);
        }
        else
        {
            newGameButtonPanel.SetActive(true);
            continueGameButtonPanel.SetActive(false);
        }

        optionsPanel.SetActive(false);
    }

    private void HideMainMenuPanel()
    {
        newGameButtonPanel.SetActive(false);
        continueGameButtonPanel.SetActive(false);
    }

    public void PlayButtonClicked()
    {
        GameData.DeleteData();
        StartCoroutine(LoadScene("IntroductionCutscene"));
    }

    public void ContinueButtonClicked()
    {
        StartCoroutine(LoadScene("LoadingScreen"));
    }

    public void OptionButtonClicked()
    {
        HideMainMenuPanel();

        optionsPanel.SetActive(true);

    }

    private IEnumerator LoadScene(string scene)
    {
        transitionAnimator.SetBool("SceneEnd", true);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(scene);
    }

    public void ExitButtonClicked()
    {
        Application.Quit();
    }

    public void MusicToggleEnabled()
    {
        if (musicToggle.isOn)
        {
            backgroundMusic.Play();

            PlayerPrefs.SetInt("Music", 1);
        }
        else
        {
            backgroundMusic.Pause();

            PlayerPrefs.SetInt("Music", 0);
        }
    }

    public void SFXToggleEnabled()
    {
        if (sfxToggle.isOn)
        {
            Sounds.sfxEnabled = true;

            PlayerPrefs.SetInt("SFX", 1);
        }
        else
        {
            Sounds.sfxEnabled = false;

            PlayerPrefs.SetInt("SFX", 0);
        }
    }

    public void BackButtonClicked()
    {
        ShowMainMenuPanel();
    }
}
