using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LevelCompleteScene : MonoBehaviour
{
    public TMPro.TextMeshProUGUI levelName;
    public TMPro.TextMeshProUGUI matchesCounter;

    public Animator transitionAnimator;
    void Start()
    {
        GameScene.level++;
        PlayerPrefs.SetInt("level", GameScene.level);
        matchesCounter.text = "Matches: " + GameScene.matches;

        switch (GameScene.level)
        {
            case 1:
                levelName.text = "The Enchanted Forest";
                break;
            case 2:
                levelName.text = "The Crystal Caverns";
                break;
            case 3:
                levelName.text = "Temple of Xartha";
                break;
            case 4:
                levelName.text = "...";
                break;
            default:
                Debug.LogError("(LevelCompleteScene) Start(): Switch case defaulted! Level number is invalid. Defaulting to 1...");
                levelName.text = "The Enchanted Forest";

                GameScene.level = 1;
                PlayerPrefs.SetInt("level", GameScene.level);

                break;
        }
    }

    public void ProceedToNextLevel()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        transitionAnimator.SetBool("SceneEnd", true);
        yield return new WaitForSeconds(1.5f);

        if (GameScene.level >= 4)
        {
            SceneManager.LoadScene("EndingScene");
        }
        else
        {
            SceneManager.LoadScene("GameScene");
        }
    }
}
