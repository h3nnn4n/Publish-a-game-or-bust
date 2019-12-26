using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class HighlighGridWithMouse : MonoBehaviour
{
    Grid grid;
    Camera mainCamera;
    Tilemap tileMap;

    public GameObject highlightEffect;

    GameObject effectObject;

    GameController gameControler;
    TileMapManager tileMapManager;

    Tile lastTile;

    Vector3Int lastCoordinate;
    Vector3Int currentCoordinate;
    Vector3 currentWorldCoordinate;
    bool coordinateChanged;

    void Start()
    {
        gameControler = GameController.GetInstance();
        tileMapManager = TileMapManager.GetInstance();
        grid = gameControler.GetGrid();
        mainCamera = Camera.main;
        tileMap = gameControler.GetTileMap();

        effectObject = Instantiate(
            highlightEffect
        );
    }

    void Update()
    {
        UpdateCoordinates();
        HighlightCurrentTile();
    }

    void UpdateCoordinates()
    {
        currentWorldCoordinate = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int coordinate = grid.WorldToCell(currentWorldCoordinate);

        if (coordinate != currentCoordinate)
        {
            lastCoordinate = currentCoordinate;
            currentCoordinate = coordinate;
            coordinateChanged = true;
        }
        else
        {
            coordinateChanged = false;
        }
    }

    void HighlightCurrentTile()
    {
        if (coordinateChanged) {
            if(tileMapManager.CanBuild(currentCoordinate))
            {
                effectObject.SetActive(true);
            } else
            {
                effectObject.SetActive(false);
            }

            effectObject.transform.position = tileMap.GetCellCenterWorld(currentCoordinate);
        }
    }
}
