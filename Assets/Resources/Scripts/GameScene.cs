using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameScene : MonoBehaviour
{
    public GameObject grid;

    public static int matches;
    public static int potionCount;
    public static bool healthPotionActivated;

    public static float time; //Added timer variables
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

    public static bool gameHasStarted;

    public static bool enemyHasDied;

    public static bool playerHasDied;

    public static int level;

    public GameObject memorizePopup;

    void Start()
    {
        matches = 0;
        potionCount = 3;
        gameHasStarted = false;
        enemyHasDied = false;
        playerHasDied = false;

        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        maxTime = 100f;
        time = maxTime;
        timeDecrementValue = 0.1f;

        healthPotionActivated = false;
        pairedTiles = new List<Tile>();

        if (player == null)
        {
            Debug.LogError("Start(): player is null! Defaulting to Shou.");
            player = shou.GetComponent<Player>();
        }

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

        enemyEncounterList = SetupEnemyList(enemies);
        LoadNewEnemy();
    }

    public List<GameObject> SetupEnemyList(GameObject[] enemies)
    {
        List<GameObject> enemyList = new List<GameObject>(enemies);
        List<GameObject> finalEnemyList = new List<GameObject>();

        if (enemyList.Count > 0)
        {
            enemyList = Extensions.Shuffle(enemyList);

            int numberOfEnemies = Random.Range(3, enemyList.Count - 1);

            for (int counter = 0; counter < numberOfEnemies; counter++)
            {
                finalEnemyList.Add(enemyList[counter]);
            }
        }
        else
        {
            Debug.LogError("SetupEnemyList(): enemyList is empty!");
        }

        finalEnemyList = Extensions.SortEnemies(finalEnemyList);
        return finalEnemyList;
    }

    public void PotionButtonClicked()
    {
        if (potionCount > 0 && !healthPotionActivated)
        {
            potionCount--;
            healthPotionActivated = true;
        }
    }

    public IEnumerator ExecuteMove(Move move)
    {
        playerSprite.GetComponent<Animator>().SetTrigger("IsAttacking");
        yield return new WaitForSeconds(1);

        StartCoroutine(ShowDamageCounter(enemyDamageCounter, move.attack));

        int damage = (int)(player.attackStat - (player.attackStat * (currentEnemyObject.GetComponent<Enemy>().defenseStat / 100)));
        currentEnemyObject.GetComponent<Enemy>().DecreaseHealth(damage);
    }

    void Update()
    {
        matchCounter.GetComponent<TMPro.TextMeshProUGUI>().text = matches.ToString();
        potionCounter.GetComponent<TMPro.TextMeshProUGUI>().text = potionCount.ToString();

        if (gameHasStarted)
        {
            enemyTurnIndicator.SetActive(false);
            memorizePopup.SetActive(false);
            if (time >= 0 && !enemyHasDied)
            {
                playerTurnIndicator.SetActive(true);

                time -= timeDecrementValue;
                timeAmount.transform.localScale = new Vector3(time / maxTime, timeAmount.transform.localScale.y, timeAmount.transform.localScale.z);
            }
            else
            {
                time = maxTime;
                timeAmount.transform.localScale = new Vector3(time / maxTime, timeAmount.transform.localScale.y, timeAmount.transform.localScale.z);

                StartCoroutine(PlayerTurnHasEnded());

                gameHasStarted = false;
            }
        }
        else
        {
            memorizePopup.SetActive(true);
        }

        if (player.health > 0)
        {
            if (player.health > player.maxHealth)
            {
                playerHealthBarAmount.transform.localScale = new Vector3((player.maxHealth / player.maxHealth), playerHealthBarAmount.transform.localScale.y, playerHealthBarAmount.transform.localScale.z);
            }
            else
            {
                playerHealthBarAmount.transform.localScale = new Vector3((player.health / player.maxHealth), playerHealthBarAmount.transform.localScale.y, playerHealthBarAmount.transform.localScale.z);
            }
        }
        else if (player.health <= 0)
        {
            playerHealthBarAmount.transform.localScale = new Vector3((0 / player.maxHealth), playerHealthBarAmount.transform.localScale.y, playerHealthBarAmount.transform.localScale.z);
        }

        if (currentEnemyObject.GetComponent<Enemy>().health > 0)
        {
            var enemy = currentEnemyObject.GetComponent<Enemy>();
            enemyHealthBarAmount.transform.localScale = new Vector3(enemy.health / enemy.maxHealth, enemyHealthBarAmount.transform.localScale.y, enemyHealthBarAmount.transform.localScale.z);
        }
        else
        {
            var enemy = currentEnemyObject.GetComponent<Enemy>();
            enemyHealthBarAmount.transform.localScale = new Vector3(0 / enemy.maxHealth, enemyHealthBarAmount.transform.localScale.y, enemyHealthBarAmount.transform.localScale.z);

            enemyHasDied = true;
        }
    }

    IEnumerator PlayerTurnHasEnded()
    {
        playerTurnIndicator.SetActive(false);

        yield return new WaitForSeconds((float)0.5);

        if (enemyHasDied)
        {
            enemySprite.GetComponent<Animator>().SetBool("IsDead", true);
            yield return new WaitForSeconds((float)1.5);

            enemyEncounterList.RemoveAt(0);
            LoadNewEnemy();

            enemyHasDied = false;
        }
        else
        {
            BeginEnemyTurn();
        }

        SetupGameBoard();
    }

    public void BeginEnemyTurn()
    {
        Enemy enemy = currentEnemyObject.GetComponent<Enemy>();

        enemyTurnIndicator.SetActive(true);

        StartCoroutine(ShowDamageCounter(playerDamageCounter, (int)enemy.attackStat));

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

        yield return new WaitForSeconds((float)1);

        damageCounter.SetActive(false);
    }

    private void SetupGameBoard()
    {
        List<Tile> tileList = grid.GetComponent<Grid>().GenerateTile();
        grid.GetComponent<Grid>().FillTileBoard(tileList);
        grid.GetComponent<Grid>().ShowTileBoardContents();
    }

    private void LoadNewEnemy()
    {
        currentEnemyObject = enemyEncounterList[0];
        enemySprite.GetComponent<Image>().sprite = currentEnemyObject.GetComponent<Image>().sprite;
        enemySprite.GetComponent<Animator>().runtimeAnimatorController = currentEnemyObject.GetComponent<Animator>().runtimeAnimatorController;
    }
}
