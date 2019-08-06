using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused;

    public Toggle musicToggle;
    public Toggle sfxToggle;

    void Start()
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

    public void PauseGame()
    {
        Time.timeScale = 0f;

        AudioListener.pause = true;

        gameObject.SetActive(true);

        isPaused = true;
    }

    public void PlayButtonClicked()
    {
        Time.timeScale = 1f;

        AudioListener.pause = false;

        gameObject.SetActive(false);

        isPaused = false;
    }

    public void QuitButtonClicked()
    {
        Time.timeScale = 1f;

        AudioListener.pause = false;

        SceneManager.LoadScene("MainMenu");        
    }

    public void DisablePauseMenu()
    {
        gameObject.SetActive(false);

        PlayButtonClicked();
    }

    public void MusicToggleEnabled()
    {
        if (musicToggle.isOn)
        {
            PlayerPrefs.SetInt("Music", 1);
        }
        else
        {
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
}
