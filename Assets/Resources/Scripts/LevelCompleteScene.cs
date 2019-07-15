using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompleteScene : MonoBehaviour
{
    public TMPro.TextMeshProUGUI levelName;
    public TMPro.TextMeshProUGUI matchesCounter;

    public Animator transitionAnimator;

    private int level;
    private int matches;
    private int totalTileCount;
    private float grade;
    private string rank;

    void Start()
    {
        level = PlayerPrefs.GetInt("level") + 1;
        PlayerPrefs.SetInt("level", level);

        matches = GameScene.matches;
        totalTileCount = GameScene.gridResetCount * 12;

        try
        {
            grade = (matches / totalTileCount) * 100;

            if (grade > 95)
            {
                rank = "SS";
            }
            else if (grade > 90)
            {
                rank = "S";
            }
            else if (grade > 80)
            {
                rank = "A";
            }
            else if (grade > 70)
            {
                rank = "B";
            }
            else if (grade > 60)
            {
                rank = "C";
            }
            else if (grade > 50)
            {
                rank = "D";
            }
            else
            {
                rank = "E";
            }
        }
        catch (DivideByZeroException)
        {
            Debug.LogError("LevelCompleteScene: DivideByZeroException caught!");
            grade = 0f;
            rank = "N/A";
        }

        matchesCounter.text = "Matches: " + matches.ToString() +" / " +totalTileCount.ToString() +"\n Rank: " +rank;

        switch (level)
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
                levelName.text = "Home...";
                break;
            default:
                Debug.LogError("(LevelCompleteScene) Start(): Switch case defaulted! Level number is invalid.");
                levelName.text = "The Enchanted Forest";

                PlayerPrefs.SetInt("level", 1);
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

        if (level >= 4)
        {
            SceneManager.LoadScene("EndingScene");
        }
        else
        {
            SceneManager.LoadScene("GameScene");
        }
    }
}
