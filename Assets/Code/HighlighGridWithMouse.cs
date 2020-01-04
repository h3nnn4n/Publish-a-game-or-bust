using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class HighlighGridWithMouse : MonoBehaviour
{
    Grid grid;
    Camera mainCamera;
    Tilemap tileMap;

    public GameObject highlightEffectPrefab;
    public Color blockedTint;
    public Color emptyBlockTint;
    public Color canBuildTint;
    public Color cannotAffordTint;

    GameObject effectObject;

    GameController gameControler;
    TileMapManager tileMapManager;
    BuildController buildController;

    Tile lastTile;

    Vector3Int lastCoordinate;
    Vector3Int currentCoordinate;
    Vector3 currentWorldCoordinate;
    bool coordinateChanged;
    bool willTowerBlockPath;

    SpriteRenderer spriteRenderer;

    void Start()
    {
        gameControler = GameController.GetInstance();
        tileMapManager = TileMapManager.GetInstance();
        buildController = BuildController.GetInstance();
        grid = gameControler.GetGrid();
        mainCamera = Camera.main;
        tileMap = gameControler.GetTileMap();

        effectObject = Instantiate(
            highlightEffectPrefab,
            grid.gameObject.transform
        );

        spriteRenderer = effectObject.GetComponentInChildren<SpriteRenderer>();
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
        Node node = tileMapManager.GetNode(currentCoordinate);

        if (node == null)
            return;

        if (node.type == "empty")
        {
            spriteRenderer.color = emptyBlockTint;
        }
        else if (node.canBuild)
        {
            if (WillTowerBlockPath())
            {
                spriteRenderer.color = blockedTint;
            }
            else if (CannotAffordTower())
            {
                spriteRenderer.color = cannotAffordTint;
            }
            else
            {
                spriteRenderer.color = canBuildTint;
            }
        }
        else
        {
            spriteRenderer.color = blockedTint;
        }

        if (coordinateChanged)
        {
            effectObject.transform.position = tileMap.GetCellCenterWorld(currentCoordinate);
        }
    }

    bool WillTowerBlockPath()
    {
        if (coordinateChanged)
        {
            buildController = BuildController.GetInstance();

            willTowerBlockPath = buildController.WillTowerBlockThePath(currentCoordinate);
        }

        return willTowerBlockPath;
    }

    bool CannotAffordTower()
    {
        return buildController.HasTowerSelected() && buildController.CannotAffordTower();
    }
}
