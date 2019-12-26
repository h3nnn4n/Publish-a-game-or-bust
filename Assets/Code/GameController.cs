using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class GameController : MonoBehaviour
{
    public GameObject sourcePrefab;
    public GameObject sinkPrefab;
    public GameObject nodePrefab;

    UiController uiController;
    TileMapManager tileMapManager;

    float credits;

    Tilemap tileMap;
    Grid grid;

    private static GameController instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        } else
        {
            Destroy(this);
            Debug.LogError("Warning, GameController tryed to spawn more than once");
        }
    }

    void Start()
    {
        Debug.Log("GameController started");

        tileMapManager = TileMapManager.GetInstance();

        tileMap = GetTileMap();
        grid = GetGrid();

        SpawnGameObjects();

        credits = 100;
    }

    void Update()
    {
        GetUiController().SetCredits(credits);
    }

    void SpawnGameObjects()
    {
        var allCellPositions = tileMap.cellBounds.allPositionsWithin;

        foreach (var cellPosition in allCellPositions)
        {
            SpawnGameObject(cellPosition);
        }
    }

    void SpawnGameObject(Vector3Int cellPosition)
    {
        Vector3 cellWorldPosition = tileMap.GetCellCenterWorld(cellPosition);
        Tile tile = tileMap.GetTile<Tile>(cellPosition);

        if (tile == null)
        {
            return;
        }

        string spriteName = tile.sprite.name;
        var node = tileMapManager.GetNode(cellPosition);

        switch (spriteName)
        {
            case "source":
                Debug.Log(string.Format("Found source at: {0}", cellPosition));

                //node.gameObject =
                Instantiate(
                    sourcePrefab,
                    cellWorldPosition,
                    Quaternion.identity,
                    node.transform
                );

                node.canBuild = false;

                break;
            case "sink":
                Debug.Log(string.Format("Found sink at: {0}", cellPosition));

                //node.gameObject =
                Instantiate(
                    sinkPrefab,
                    cellWorldPosition,
                    Quaternion.identity,
                    node.transform
                );

                node.canBuild = false;

                break;
            case "empty":
                node.canBuild = false;
                break;
            case "block":
                node.canBuild = true;
                break;
            default:
                break;
        }
    }

    public Node GetNodeForTile(Tile tile)
    {
        return tile.gameObject.GetComponent<Node>();
    }

    public float GetCredits()
    {
        return credits;
    }

    public void SpendCredits(float spent)
    {
        credits -= spent;
    }

    public static GameController GetInstance()
    {
        return instance;
    }

    public void AddCredits(float income)
    {
        credits += income;
    }

    public Tilemap GetTileMap()
    {
        return GameObject.FindGameObjectWithTag("Tilemap").GetComponent<Tilemap>();
    }

    public Grid GetGrid()
    {
        return GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>();
    }

    public UiController GetUiController()
    {
        if (uiController == null)
        {
            uiController = GameObject.FindGameObjectWithTag("UI").GetComponent<UiController>();
        }

        return uiController;
    }
}
