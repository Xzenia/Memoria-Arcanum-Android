using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Grid : MonoBehaviour
{
    private float flipXValue = 1f;
    public GameObject[] shouTiles;
    public GameObject[] rikkoTiles;
    public GameObject[] emilyTiles;

    public GameObject shouSpecialAttackTile;
    public GameObject rikkoSpecialAttackTile;
    public GameObject emilySpecialAttackTile;

    public GameObject[] healingTiles;

    public GameObject[] gameTiles;

    public GameObject potionTile;

    public Sprite defaultSprite;

    private List<GameObject> selectedTiles;

    private List<GameObject> characterTiles;

    public GameScene gameScene;

    public Sounds sounds;

    public GameObject ghostTile;

    void Start()
    {
        selectedTiles = new List<GameObject>();
    }

    public List<Tile> GenerateTile()
    {
        if (GameScene.player.name.Equals("Shou"))
        {
            if (GameScene.specialAttackModeActivated)
            {
                characterTiles = new List<GameObject>();
                characterTiles.Add(shouSpecialAttackTile);
            }
            else
            {
                characterTiles = new List<GameObject>(shouTiles);
            }

        }
        else if (GameScene.player.name.Equals("Rikko"))
        {
            if (GameScene.specialAttackModeActivated)
            {
                characterTiles = new List<GameObject>();
                characterTiles.Add(rikkoSpecialAttackTile);
            }
            else
            {
                characterTiles = new List<GameObject>(rikkoTiles);
            }
        }
        else if (GameScene.player.name.Equals("Emily"))
        {
            if (GameScene.specialAttackModeActivated)
            {
                characterTiles = new List<GameObject>();
                characterTiles.Add(emilySpecialAttackTile);
            }
            else
            {
                characterTiles = new List<GameObject>(emilyTiles);
            }
        }
        else
        {
            Debug.LogError("Input in GameScene.selectedCharacterName is invalid!");
            characterTiles = new List<GameObject>(shouTiles);
        }

        List<Tile> tileList = new List<Tile>();
        foreach (GameObject gameObject in characterTiles)
        {
            tileList.Add(gameObject.GetComponent<Tile>());
        }

        if (GameScene.healthPotionActivated)
        {
            sounds.healUp.Play();

            int randomIndex = Random.Range(0, healingTiles.Length);
            for (int counter = 0; counter < 4; counter++)
            {
                tileList.Add(healingTiles[randomIndex].GetComponent<Tile>());
            }
        }

        //TODO: Rewrite tile generation code into something much simpler.
        int timesPotionTileHasBeenSpawned = 0;

        List<Tile> tiles = new List<Tile>();
        for (int counter = 0; counter < 13; counter++)
        {
            int randomIndex = Random.Range(0, tileList.Count);

            if (Random.Range(0, 100) <= 25 && !GameScene.healthPotionActivated)
            {
                if (timesPotionTileHasBeenSpawned < 2)
                {
                    tiles.Add(potionTile.GetComponent<Tile>());
                    tiles.Add(potionTile.GetComponent<Tile>());

                    timesPotionTileHasBeenSpawned++;
                }
                else
                {
                    tiles.Add(tileList[randomIndex]);
                    tiles.Add(tileList[randomIndex]);
                }
            }
            else
            {
                tiles.Add(tileList[randomIndex]);
                tiles.Add(tileList[randomIndex]);
            }
        }

        Extensions.ShuffleTiles(tiles);

        GameScene.healthPotionActivated = false;

        return tiles;
    }

    public void FillTileBoard(List<Tile> tile)
    {
        for (int counter = 0; counter < 25; counter++)
        {
            gameTiles[counter].GetComponent<Tile>().tileId = counter;
            gameTiles[counter].GetComponent<Tile>().tile = tile[counter].tile;
            gameTiles[counter].GetComponent<Tile>().tileType = tile[counter].tileType;
            gameTiles[counter].GetComponent<Tile>().character = tile[counter].character;
            gameTiles[counter].GetComponent<Tile>().effectId = tile[counter].effectId;
        }
    }
    public IEnumerator ShowAndHideTileContents()
    {
        ShowTileBoardContents();

        yield return new WaitForSeconds(5);

        HideTileBoardContents();

        GameScene.playerTurnHasStarted = true;

        yield return new WaitForSeconds(1);
    }

    public void ShowTileBoardContents()
    {
        foreach (GameObject gameObject in gameTiles)
        {
            gameObject.GetComponent<Image>().sprite = gameObject.GetComponent<Tile>().tile;
        }
    }

    public void HideTileBoardContents()
    {
        foreach (GameObject gameObject in gameTiles)
        {
            gameObject.GetComponent<Image>().sprite = defaultSprite;
        }
    }

    private GameObject chosenTile1;
    private GameObject chosenTile2;

    void FixedUpdate()
    {
        if (flipXValue < 1 && EventSystem.current.currentSelectedGameObject != null)
        {
            GameObject tileToFlip = EventSystem.current.currentSelectedGameObject;
            flipXValue += 0.2f;
            tileToFlip.transform.localScale = new Vector3(flipXValue, tileToFlip.transform.localScale.y, tileToFlip.transform.localScale.z);
        }

        if (!GameScene.playerTurnHasStarted)
        {
            selectedTiles.Clear();
        }
    }

    public void TileIsClicked()
    {
        if (GameScene.playerTurnHasStarted)
        {
            if (EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite.Equals(defaultSprite))
            {
                EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite = EventSystem.current.currentSelectedGameObject.GetComponent<Tile>().tile;
                flipXValue = 0.2f; //Flip card.

                selectedTiles.Add(EventSystem.current.currentSelectedGameObject);

                if (selectedTiles.Count > 1)
                {
                    chosenTile1 = selectedTiles[0];
                    chosenTile2 = selectedTiles[1];

                    if (chosenTile1.GetComponent<Tile>().effectId == chosenTile2.GetComponent<Tile>().effectId && chosenTile1.GetComponent<Tile>().tileType == chosenTile2.GetComponent<Tile>().tileType)
                    {
                        Debug.Log("Tiles are similar!");

                        Tile tile = chosenTile1.GetComponent<Tile>();
                        if (tile.tileType == TileType.Item)
                        {
                            switch (tile.effectId)
                            {
                                case 1:
                                    GameScene.player.IncreaseHealth(20);
                                    break;
                                case 2:
                                    GameScene.player.IncreaseHealth(30);
                                    break;
                                case 3:
                                    GameScene.potionCount++;
                                    break;
                                default:
                                    Debug.LogError("PlayerTurnEnded(): Healing tiles switch case defaulted!");
                                    GameScene.player.IncreaseHealth(30);
                                    break;
                            }
                        }
                        else if (tile.tileType == TileType.Move)
                        {
                            Move move = MoveSets.RetrieveMove(tile);
                            Debug.Log("Selected Tile: " + move.name + "\nDamage: " + move.attack);
                            
                            if (GameScene.player.charge < GameScene.player.maxCharge && !GameScene.specialAttackModeActivated)
                            {
                                GameScene.player.IncreaseCharge(10);
                            }

                            StartCoroutine(gameScene.ExecuteMove(move));
                        }

                        sounds.PlayCorrectMatchSound();

                        CreateGhostTile(selectedTiles[0]);
                        CreateGhostTile(selectedTiles[1]);

                        GameScene.matches++;
                    }
                    else
                    {
                        sounds.PlayWrongMatchSound();

                        Debug.Log("Tiles are different!");
                        StartCoroutine(HideSelectedTiles());
                    }

                    selectedTiles.Clear();
                }
            }
        }
    }

    IEnumerator HideSelectedTiles()
    {
        yield return new WaitForSeconds((float)0.5);

        chosenTile1.GetComponent<Image>().sprite = defaultSprite;
        chosenTile2.GetComponent<Image>().sprite = defaultSprite;
    }

    private void CreateGhostTile(GameObject tile)
    {
        GameObject ghostTileCopy = Instantiate(ghostTile, tile.transform.position, Quaternion.identity);

        ghostTileCopy.GetComponent<GhostTile>().destroyable = true;
        ghostTileCopy.GetComponent<Image>().sprite = tile.GetComponent<Tile>().tile;
        ghostTileCopy.GetComponent<GhostTile>().tileY = tile.transform.position.y;

        ghostTileCopy.transform.SetParent(this.gameObject.transform);
    }
}
