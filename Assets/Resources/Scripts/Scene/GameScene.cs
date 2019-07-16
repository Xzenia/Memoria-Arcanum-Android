using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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

    public GameObject matchCounter;
    public GameObject potionCounter;

    public GameObject playerTurnIndicator;
    public GameObject enemyTurnIndicator;

    public GameObject playerSprite;
    public GameObject shou;
    public GameObject rikko;
    public GameObject emily;

    public GameObject enemySprite;
    private GameObject[] enemies;

    public Image playerHealthBarAmount;
    public Image enemyHealthBarAmount;
    public Image timeAmount;

    public GameObject playerDamageCounter;
    public GameObject enemyDamageCounter;

    public static Player player;

    private List<GameObject> enemyEncounterList; //Contains the final list of enemies. 

    private static GameObject currentEnemyObject;

    public static bool playerTurnHasStarted;

    public static bool enemyHasDied;

    public static bool playerHasDied;

    public GameObject memorizePopup;

    public Animator gridBackground;

    private bool demoMode = false;

    public Image chargeBarAmount;

    public Animator chargeBarAnimation;

    public Sounds sounds;

    public static int gridResetCount;

    public Animator enemyAttackAnimation;

    public GameObject tapEffect;

    public static bool specialAttackModeActivated;

    public GameObject pauseMenuCanvas;

    void Awake()
    {
        if (!PlayerPrefs.HasKey("level"))
        {
            PlayerPrefs.SetInt("level", 1);
        }
    }

    void Start()
    {
        InitializeValues();

        if (player == null)
        {
            Debug.LogError("Start(): player variable is null! Defaulting to Shou.");
            player = shou.GetComponent<Player>();
        }
        player.ResetPlayerValues();

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
    }

    public void PotionButtonClicked()
    {
        //Prevents the button from activating twice (!healthPotionActivated) and in a time when it would be
        //unlikely to make a pair (time > 3). 
        if (potionCount > 0 && !healthPotionActivated && time > 3)
        {
            potionCount--;
            healthPotionActivated = true;

            playerTurnHasStarted = false;

            SetupGameBoard();
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
                if (!healthPotionActivated)
                { 
                    StartCoroutine(PlayerTurnHasEnded());
                }

                playerTurnHasStarted = false;

                if (specialAttackModeActivated)
                {
                    specialAttackModeActivated = false;
                }
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

        //Popups
        if (playerTurnHasStarted)
        {
            enemyTurnIndicator.SetActive(false);
            memorizePopup.SetActive(false);

            playerTurnIndicator.SetActive(true);
        }
        else
        {
            memorizePopup.SetActive(true);
        }
    }

    void FixedUpdate()
    {
        //Player health bar
        //Value correction measures. 
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

        //Enemy health bar 
        var enemy = currentEnemyObject.GetComponent<Enemy>();

        //Value correction measures. 
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

        //Player charge bar
        chargeBarAmount.transform.localScale = new Vector3(chargeBarAmount.transform.localScale.x, (player.charge / player.maxCharge), chargeBarAmount.transform.localScale.z);

        if (player.charge == player.maxCharge)
        {
            //sounds.chargeFull.Play();

            chargeBarAnimation.SetBool("IsFull", true);
        }
        else
        {
            //sounds.chargeFull.Stop();

            chargeBarAnimation.SetBool("IsFull", false);
        }
    }

    IEnumerator PlayerTurnHasEnded()
    {
        playerTurnIndicator.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        if (enemyHasDied)
        {
            enemySprite.GetComponent<Animator>().SetBool("IsDead", true);
            yield return new WaitForSeconds(1.5f);

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

            enemyHasDied = false;
        }
        else
        {
            BeginEnemyTurn();
        }

        grid.GetComponent<Grid>().HideTileBoardContents();

        yield return new WaitForSeconds(2f);

        SetupGameBoard();
    }

    public void BeginEnemyTurn()
    {
        Enemy enemy = currentEnemyObject.GetComponent<Enemy>();

        enemyTurnIndicator.SetActive(true);

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

    IEnumerator ShowDamageCounter(GameObject damageCounter, float damage)
    {
        damageCounter.GetComponent<TMPro.TextMeshProUGUI>().text = ""+(int)damage;
        damageCounter.SetActive(true);

        yield return new WaitForSeconds(1f);

        damageCounter.SetActive(false);
    }

    private void SetupGameBoard()
    {
        List <Tile> tileList = grid.GetComponent<Grid>().GenerateTile();
        grid.GetComponent<Grid>().FillTileBoard(tileList);
        grid.GetComponent<Grid>().StartCoroutine(grid.GetComponent<Grid>().ShowAndHideTileContents());

        if (!healthPotionActivated)
        {
            gridResetCount++;
        }
    }

    private void LoadNewEnemy()
    {
        currentEnemyObject = enemyEncounterList[0];
        enemySprite.GetComponent<Image>().sprite = currentEnemyObject.GetComponent<Image>().sprite;

        var currentEnemyObjectRectTransform = currentEnemyObject.transform as RectTransform;
        var enemySpriteRectTransform = enemySprite.transform as RectTransform;

        float currentEnemyObjectHeight = currentEnemyObjectRectTransform.rect.height;
        float currentEnemyObjectWidth = currentEnemyObjectRectTransform.rect.width;

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
