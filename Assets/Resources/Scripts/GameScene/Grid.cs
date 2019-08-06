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

    public GameObject potionTile;

    public Sprite defaultSprite;

    private List<GameObject> selectedTiles;

    private List<GameObject> characterTiles;

    public GameScene gameScene;

    public Sounds sounds;

    public GameObject ghostTile;

    public GameObject gridParentObject; // The parent of all instantiated tile objects. 

    public GameObject defaultTile; 

    void Start()
    {
        selectedTiles = new List<GameObject>();
    }

    public List<Tile> SetupGameTiles(int rows, int columns)
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
            sounds.PlayHealButtonActivatedSound();

            int randomIndex = Random.Range(0, healingTiles.Length);
            for (int counter = 0; counter < 4; counter++)
            {
                tileList.Add(healingTiles[randomIndex].GetComponent<Tile>());
            }
        }

        int totalGridCount = rows * columns;

        Debug.Log("Row: " + rows + "\nColumns: " + columns);
        Debug.Log("Total Grid Count: " + totalGridCount);

        List<Tile> tiles = GenerateGridContents(totalGridCount, tileList);

        tiles = Extensions.ShuffleTiles(tiles);

        return tiles;
    }

    private List<Tile> GenerateGridContents(int numberOfTiles, List<Tile> tileList)
    {
        List<Tile> tiles = new List<Tile>();

        int timesPotionTileHasBeenSpawned = 0;

        for (int counter = 0; counter < numberOfTiles/2; counter++)
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

        return tiles;
    }

    public void FillTileBoard(List<Tile> tile, int rows, int columns)
    {
        if (gridParentObject.transform.childCount > 0)
        {
            foreach (Transform child in gridParentObject.transform)
            {
                Destroy(child.gameObject);
            }
        }

        int totalCounter = 0;

        gridParentObject.GetComponent<GridLayoutGroup>().constraintCount = columns;

        for (int rowCounter = 0; rowCounter < rows; rowCounter++)
        {
            for (int columnCounter = 0; columnCounter < columns; columnCounter++)
            {
                GameObject generatedTile = Instantiate(defaultTile) as GameObject;

                generatedTile.GetComponent<Tile>().tileId = totalCounter;
                generatedTile.GetComponent<Tile>().tile = tile[totalCounter].tile;
                generatedTile.GetComponent<Tile>().tileType = tile[totalCounter].tileType;
                generatedTile.GetComponent<Tile>().character = tile[totalCounter].character;
                generatedTile.GetComponent<Tile>().effectId = tile[totalCounter].effectId;

                generatedTile.transform.SetParent(gridParentObject.transform);

                totalCounter++;

            }
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
        foreach (Transform children in gridParentObject.transform)
        {
            children.gameObject.GetComponent<Image>().sprite = children.gameObject.GetComponent<Tile>().tile;
        }
    }

    public void HideTileBoardContents()
    {
        foreach (Transform children in gridParentObject.transform)
        {
            children.gameObject.GetComponent<Image>().sprite = defaultSprite;
        }
    }

    private GameObject chosenTile1;
    private GameObject chosenTile2;

    private bool doneFlipping = true; //This is set to true by default so it doesn't trigger the tile flip code prematurely.

    void Update()
    {
        // TODO: Tile flip code could be written better! Refine this hacky mess! 
        if (!doneFlipping)
        {
            GameObject tileToFlip = EventSystem.current.currentSelectedGameObject;

            if (tileToFlip != null)
            {
                flipXValue += 0.2f;

                if (flipXValue >= 1.05)  // Prevents the tile from being slightly bigger in width than unselected tiles aside from stopping the animation.
                {                        // 1.05 is apparently the most convincing localScale.x value that looks close to the width of unselected tiles. 
                    doneFlipping = true; // 1.0f is way too thin compared to unselected tiles and 1.2f is way too wide. 
                }

                tileToFlip.transform.localScale = new Vector3(flipXValue, tileToFlip.transform.localScale.y, tileToFlip.transform.localScale.z);
            }
            else
            {
                doneFlipping = true;
            }
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
                var selectedTile = EventSystem.current.currentSelectedGameObject;
                selectedTile.GetComponent<Image>().sprite = EventSystem.current.currentSelectedGameObject.GetComponent<Tile>().tile;

                if (Input.touches.Length < 2)
                {
                    flipXValue = 0.2f; // Initial localscale.x value of the selected tile when it begins to flip.

                    doneFlipping = false; // Flip card.
                }

                selectedTiles.Add(EventSystem.current.currentSelectedGameObject);

                if (selectedTiles.Count > 1)
                {
                    chosenTile1 = selectedTiles[0];
                    chosenTile2 = selectedTiles[1];

                    if (chosenTile1.GetComponent<Tile>().effectId == chosenTile2.GetComponent<Tile>().effectId && chosenTile1.GetComponent<Tile>().tileType == chosenTile2.GetComponent<Tile>().tileType)
                    {
                        Debug.Log("Tiles are similar!");

                        Tile tile = chosenTile1.GetComponent<Tile>();

                        GameScene.pairedTiles.Add(tile);

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

    private IEnumerator HideSelectedTiles()
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

        ghostTileCopy.transform.SetParent(gameObject.transform);
    }
}
