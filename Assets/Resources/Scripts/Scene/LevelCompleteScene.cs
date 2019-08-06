using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompleteScene : MonoBehaviour
{
    public TMPro.TextMeshProUGUI levelName;
    public TMPro.TextMeshProUGUI matchesCounter;

    public Animator transitionAnimator;

    public AudioSource backgroundMusic;

    private int matches;
    private int totalTileCount;
    private float grade;
    private string rank;

    void Start()
    {
        GameScene.level++;

        SaveGameData();

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
                levelName.text = "Home...";
                break;
            default:
                Debug.LogError("(LevelCompleteScene) Start(): Switch case defaulted! Level number is invalid.");
                levelName.text = "The Enchanted Forest";

                GameScene.level = 1;
                break;
        }

        if (PlayerPrefs.GetInt("Music") == 1)
        {
            backgroundMusic.Play();
        }
        else
        {
            backgroundMusic.Stop();
        }
    }

    public void ProceedToNextLevel()
    {
        StartCoroutine(LoadScene());
    }

    public void SaveGameData()
    {
        var gameData = new GameData();

        if (GameScene.player == null)
        {
            Debug.LogError("SaveGameData(): GameScene.player is null! Defaulting to Shou...");

            GameScene.player = new Player();
            GameScene.player.name = "Shou";
        }

        gameData.playerCharacterName = GameScene.player.name;

        gameData.level = GameScene.level;

        GameData.SaveData(gameData);
    }

    private IEnumerator LoadScene()
    {
        transitionAnimator.SetBool("SceneEnd", true);
        yield return new WaitForSeconds(1f);

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
