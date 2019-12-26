using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameController : MonoBehaviour
{
    public GameObject sourcePrefab;
    public GameObject sinkPrefab;

    UiController uiController;

    float credits;

    Tilemap tileMap;
    Grid grid;

    void Start()
    {
        Debug.Log("GameController started");

        tileMap = GetTileMap();
        grid = GetGrid();

        SpawnGameObjects();

        credits = 0;
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

        switch (spriteName)
        {
            case "source":
                tile.gameObject = Instantiate(
                    sourcePrefab,
                    cellWorldPosition,
                    Quaternion.identity
               );
                break;
            case "sink":
                tile.gameObject = Instantiate(
                    sinkPrefab,
                    cellWorldPosition,
                    Quaternion.identity
               );
                break;
            default:
                break;
        }
    }

    public void AddCredits(float income)
    {
        credits += income;
    }

    Tilemap GetTileMap()
    {
        return GameObject.FindGameObjectWithTag("Tilemap").GetComponent<Tilemap>();
    }

    Grid GetGrid()
    {
        return GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>();
    }

    UiController GetUiController()
    {
        if (uiController == null)
        {
            uiController = GameObject.FindGameObjectWithTag("UI").GetComponent<UiController>();
        }

        return uiController;
    }
}
