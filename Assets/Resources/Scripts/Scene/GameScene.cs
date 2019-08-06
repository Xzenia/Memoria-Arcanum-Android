using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameScene : MonoBehaviour
{
    public GameObject grid;

    public static int matches;
    public static int potionCount;
    public static bool healthPotionActivated;

    public static float time; 
    public static float maxTime;
    public static float timeDecrementValue;

    public static List<Tile> pairedTiles;

    public GameObject playerSprite;
    public GameObject shou;
    public GameObject rikko;
    public GameObject emily;

    public GameObject enemySprite;

    private GameObject[] enemies;

    public static Player player;

    private List<GameObject> enemyEncounterList; //Contains the final list of enemies. 

    private static GameObject currentEnemyObject;

    public static bool playerTurnHasStarted;

    public static bool enemyHasDied;

    public static bool playerHasDied;

    private bool demoMode = false;

    public static int gridResetCount;

    public static bool specialAttackModeActivated;

    public static int level;

    private List<Tile> tileList;

    private List<Tuple<int, int>> rowsAndColumnCountList; //TODO: Needs a better name. 

    //UI 
    public GameObject matchCounter;
    public GameObject potionCounter;

    public GameObject playerDamageCounter;
    public GameObject enemyDamageCounter;

    public GameObject memorizePopup;

    public Animator gridBackground;

    public GameObject pauseMenuCanvas;

    public GameObject tapEffect;

    public Animator enemyAttackAnimation;

    public Image chargeBarAmount;

    public Animator chargeBarAnimation;

    public Sounds sounds;

    public Image playerHealthBarAmount;
    public Image enemyHealthBarAmount;
    public Image timeAmount;

    void Awake()
    {
        var gameData = new GameData();

        var loadedData = GameData.LoadData();

        if (loadedData != null)
        {
            gameData = loadedData;

            player = new Player();

            player.name = gameData.playerCharacterName;

            switch (player.name)
            {
                case "Shou":
                    player = shou.GetComponent<Player>();
                    break;
                case "Emily":
                    player = emily.GetComponent<Player>();
                    break;
                case "Rikko":
                    player = rikko.GetComponent<Player>();
                    break;
                default:
                    Debug.LogError("GameScene(): playerCharacterName switch case defaulted! There is GameData but is has an invalid character name! Reverting to Shou...");
                    player = shou.GetComponent<Player>();
                    break;
            }
        }
        else
        {
            if (player == null)
            {
                Debug.LogError("Start(): player variable is null! Defaulting to Shou.");
                player = shou.GetComponent<Player>();
            }

            player.ResetPlayerValues();
            gameData.level = 1;
        }

        level = gameData.level;

        //Pause menu initialization
        PauseMenu.isPaused = false;
    }

    void Start()
    {
        InitializeValues();

        SetupGameBoard();

        potionCounter.GetComponent<TMPro.TextMeshProUGUI>().text = potionCount.ToString();

        if (player.name.Equals("Shou"))
        { 
            playerSprite.GetComponent<Image>().sprite = shou.GetComponent<Image>().sprite;
            playerSprite.GetComponent<Animator>().runtimeAnimatorController = shou.GetComponent<Animator>().runtimeAnimatorController;
        }
        else if (player.name.Equals("Rikko"))
        {
            playerSprite.GetComponent<Image>().sprite = rikko.GetComponent<Image>().sprite;
            playerSprite.GetComponent<Animator>().runtimeAnimatorController = rikko.GetComponent<Animator>().runtimeAnimatorController;
        }
        else if (player.name.Equals("Emily"))
        {
            playerSprite.GetComponent<Image>().sprite = emily.GetComponent<Image>().sprite;
            playerSprite.GetComponent<Animator>().runtimeAnimatorController = emily.GetComponent<Animator>().runtimeAnimatorController;
        }
        else
        {
            Debug.LogError("selectedCharacterName input is invalid!");
            playerSprite.GetComponent<Image>().sprite = shou.GetComponent<Image>().sprite;
            playerSprite.GetComponent<Animator>().runtimeAnimatorController = shou.GetComponent<Animator>().runtimeAnimatorController;
        }

        enemyEncounterList = Enemy.SetupEnemyList(enemies);
        LoadNewEnemy();
    }


    private void InitializeValues()
    {
        matches = 0;
        potionCount = 3;
        playerTurnHasStarted = false;
        enemyHasDied = false;
        playerHasDied = false;

        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        maxTime = 100f;
        time = maxTime;
        timeDecrementValue = 0.1f;

        healthPotionActivated = false;
        pairedTiles = new List<Tile>();

        gridResetCount = 0;

        specialAttackModeActivated = false;

        tileList = new List<Tile>();
        // This tuple represents row and column. Tuple<row, column>. 
        // When adding new combinations, make sure it is even when multiplied. Otherwise, you will have tiles with no pairs. 
        // At the moment, the maximum amount of rows and columns that can be placed in-game without overflowing is 5. This might be fixed soon. 
        rowsAndColumnCountList = new List<Tuple<int, int>>();
        rowsAndColumnCountList.Add(new Tuple<int,int> (4, 3));
        rowsAndColumnCountList.Add(new Tuple<int, int>(3, 4));
        rowsAndColumnCountList.Add(new Tuple<int, int>(5, 4));
        rowsAndColumnCountList.Add(new Tuple<int, int>(4, 5));
    }

    public void PotionButtonClicked()
    {
        // Prevents the button from activating twice (!healthPotionActivated) and in a time when it would be
        // unlikely to make a pair (time > 3). It also does not activate when the player is at max health. 
        if (potionCount > 0 && !healthPotionActivated && player.health != player.maxHealth)
        {
            if (playerTurnHasStarted && time > 3)
            {
                potionCount--;

                healthPotionActivated = true;

                playerTurnHasStarted = false;

                SetupGameBoard();
            }
        }
    }

    public IEnumerator ExecuteMove(Move move)
    {
        if (specialAttackModeActivated)
        {
            playerSprite.GetComponent<Animator>().SetTrigger("SpecialAttack");
        }
        else
        {
            playerSprite.GetComponent<Animator>().SetTrigger("IsAttacking");
        }

        yield return new WaitForSeconds(0.5f);

        StartCoroutine(ShowDamageCounter(enemyDamageCounter, move.attack));

        sounds.PlayHeroAttackSound(player);

        int damage = (int)(player.attackStat - (player.attackStat * (currentEnemyObject.GetComponent<Enemy>().defenseStat / 100)));
        currentEnemyObject.GetComponent<Enemy>().DecreaseHealth(damage);
    }

    void Update()
    {
        matchCounter.GetComponent<TMPro.TextMeshProUGUI>().text = matches.ToString();
        potionCounter.GetComponent<TMPro.TextMeshProUGUI>().text = potionCount.ToString();

        if (playerTurnHasStarted)
        {
            if (time <= 0 || enemyHasDied)
            {
                StartCoroutine(PlayerTurnHasEnded());

                playerTurnHasStarted = false;

                if (specialAttackModeActivated)
                {
                    specialAttackModeActivated = false;
                }
            }
            else if (pairedTiles.Count == (tileList.Count / 2))
            {
                playerTurnHasStarted = false;
                healthPotionActivated = false;
                specialAttackModeActivated = false;

                SetupGameBoard();
            }
            else if (PauseMenu.isPaused)
            {
                timeAmount.transform.localScale = new Vector3(time / maxTime, timeAmount.transform.localScale.y, timeAmount.transform.localScale.z);
            }
            else
            {
                time -= timeDecrementValue;
                timeAmount.transform.localScale = new Vector3(time / maxTime, timeAmount.transform.localScale.y, timeAmount.transform.localScale.z);
            }
        }
        else
        {
            time = maxTime;
            timeAmount.transform.localScale = new Vector3(time / maxTime, timeAmount.transform.localScale.y, timeAmount.transform.localScale.z);
        }

        // Popups
        if (playerTurnHasStarted)
        {
            memorizePopup.SetActive(false);
        }
        else
        {
            memorizePopup.SetActive(true);
        }
    }

    void FixedUpdate()
    {
        // Player health bar
        // Value correction measures. 
        if (player.health > player.maxHealth)
        {
            player.health = player.maxHealth;
        }
        else if (player.health < 0)
        {
            player.health = 0;
        }

        playerHealthBarAmount.transform.localScale = new Vector3((player.health / player.maxHealth), playerHealthBarAmount.transform.localScale.y, playerHealthBarAmount.transform.localScale.z);

        if (player.health <= 15)
        {
            sounds.critical.Play();

            gridBackground.SetBool("Player Health Critical", true);
        }
        else
        {
            sounds.critical.Stop();

            gridBackground.SetBool("Player Health Critical", false);
        }

        // Enemy health bar 
        var enemy = currentEnemyObject.GetComponent<Enemy>();

        // Value correction measures. 
        if (enemy.health > enemy.maxHealth)
        {
            enemy.health = enemy.maxHealth;
        }

        if (enemy.health <= 0)
        {
            enemy.health = 0;

            enemyHasDied = true;
        }

        enemyHealthBarAmount.transform.localScale = new Vector3(enemy.health / enemy.maxHealth, enemyHealthBarAmount.transform.localScale.y, enemyHealthBarAmount.transform.localScale.z);

        // Player charge bar
        chargeBarAmount.transform.localScale = new Vector3(chargeBarAmount.transform.localScale.x, (player.charge / player.maxCharge), chargeBarAmount.transform.localScale.z);

        if (player.charge == player.maxCharge)
        {
            chargeBarAnimation.SetBool("IsFull", true);
        }
        else
        {
            chargeBarAnimation.SetBool("IsFull", false);
        }
    }

    private IEnumerator PlayerTurnHasEnded()
    {
        if (enemyHasDied)
        {
            enemySprite.GetComponent<Animator>().SetBool("IsDead", true);
            yield return new WaitForSeconds(1f);

            enemyEncounterList.RemoveAt(0);

            if (enemyEncounterList.Count <= 0)
            {
                if (demoMode)
                {
                    SceneManager.LoadScene("DemoComplete");
                }
                else
                {
                    SceneManager.LoadScene("LevelComplete");
                }
            }
            else
            {
                LoadNewEnemy();
            }
        }
        else
        {

            BeginEnemyTurn();
        }

        if (!enemyHasDied)
        {
            grid.GetComponent<Grid>().HideTileBoardContents();

            yield return new WaitForSeconds(2f);
        }

        enemyHasDied = false;
        healthPotionActivated = false;

        SetupGameBoard();
    }

    public void BeginEnemyTurn()
    {
        Enemy enemy = currentEnemyObject.GetComponent<Enemy>();

        switch (enemy.attackType)
        {
            case AttackType.Teeth:
                enemyAttackAnimation.SetTrigger("Bite");
                break;
            case AttackType.Claws:
                enemyAttackAnimation.SetTrigger("Scratch");
                break;
            case AttackType.Crystal:
                enemyAttackAnimation.SetTrigger("Crystal");
                break;
            case AttackType.Projectile:
                enemyAttackAnimation.SetTrigger("Projectile");
                break;
            default:
                Debug.LogError("BeginEnemyTurn(): AttackType switch case defaulted! Switching to teeth...");
                enemyAttackAnimation.SetTrigger("Teeth");
                break;
        }

        StartCoroutine(ShowDamageCounter(playerDamageCounter, (int)enemy.attackStat));

        sounds.PlayEnemyAttackSound();

        gridBackground.SetTrigger("Player Hit");

        int damage = (int)(enemy.attackStat - (enemy.attackStat * (player.defenseStat / 100)));

        player.DecreaseHealth(damage);

        if (player.health <= 0)
        {
            SceneManager.LoadScene("GameOver");
        }
    }

    private IEnumerator ShowDamageCounter(GameObject damageCounter, float damage)
    {
        damageCounter.GetComponent<TMPro.TextMeshProUGUI>().text = ""+(int)damage;
        damageCounter.SetActive(true);

        yield return new WaitForSeconds(1f);

        damageCounter.SetActive(false);
    }

    private void SetupGameBoard()
    {
        int rowAndColumnCountListSelectedIndex = UnityEngine.Random.Range(0, rowsAndColumnCountList.Count);
        Tuple<int, int> rowAndColumnCount = rowsAndColumnCountList[rowAndColumnCountListSelectedIndex];
        tileList = grid.GetComponent<Grid>().SetupGameTiles(rowAndColumnCount.Item1, rowAndColumnCount.Item2);
        grid.GetComponent<Grid>().FillTileBoard(tileList, rowAndColumnCount.Item1, rowAndColumnCount.Item2);
        grid.GetComponent<Grid>().StartCoroutine(grid.GetComponent<Grid>().ShowAndHideTileContents());

        if (!healthPotionActivated)
        {
            gridResetCount++;
        }

        pairedTiles.Clear(); // TODO: pairedTiles.Clear() is best put somewhere else other than SetupGameBoard().
    }

    private void LoadNewEnemy()
    {
        currentEnemyObject = enemyEncounterList[0];
        enemySprite.GetComponent<Image>().sprite = currentEnemyObject.GetComponent<Image>().sprite;

        var currentEnemyObjectRectTransform = currentEnemyObject.transform as RectTransform;
        var enemySpriteRectTransform = enemySprite.transform as RectTransform;

        float currentEnemyObjectHeight = currentEnemyObjectRectTransform.rect.height;
        float currentEnemyObjectWidth = currentEnemyObjectRectTransform.rect.width;

        float currentEnemyYPivot = currentEnemyObjectRectTransform.pivot.y;

        enemySpriteRectTransform.pivot.Set(enemySpriteRectTransform.pivot.x, currentEnemyYPivot);
        enemySpriteRectTransform.sizeDelta = new Vector2(currentEnemyObjectWidth, currentEnemyObjectHeight);

        enemySprite.GetComponent<Animator>().runtimeAnimatorController = currentEnemyObject.GetComponent<Animator>().runtimeAnimatorController;
    }

    public void SpecialAttackButtonClicked()
    {
        if (player.charge >= player.maxCharge && playerTurnHasStarted)
        {
            specialAttackModeActivated = true;
            playerTurnHasStarted = false;
            SetupGameBoard();

            player.RevertToDefaultChargeValue();
        }
    }

    public void PauseButtonClicked()
    {
        if (!PauseMenu.isPaused)
        {
            pauseMenuCanvas.GetComponent<PauseMenu>().PauseGame();
        }
    }
}
