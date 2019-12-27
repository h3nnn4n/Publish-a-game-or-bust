using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class BuildController : MonoBehaviour
{
    public GameObject basicTower;

    GameObject selectedTower;

    GameController gameControler;
    TileMapManager tileMapManager;
    Pathfinder pathfinder;
    Grid grid;
    Camera mainCamera;
    Tilemap tileMap;
    Vector3Int currentCoordinate;
    Tile currentTile;
    bool coordinateChanged;

    void Start()
    {
        gameControler = GameController.GetInstance();
        tileMapManager = TileMapManager.GetInstance();
        pathfinder = gameControler.GetPathfinder();

        grid = gameControler.GetGrid();
        mainCamera = Camera.main;
        tileMap = gameControler.GetTileMap();

        selectedTower = basicTower;
    }

    void Update()
    {
        UpdateFocusedTitle();

        if (Input.GetMouseButtonDown(0))
        {
            if (CanBuildTower())
            {
                BuildTower();
            }
        }
    }

    bool CanBuildTower()
    {
        bool canBuild = true;

        if (gameControler.GetCredits() < SelectedWeaponCost())
        {
            Debug.Log("Not enough credits!");
            canBuild = false;
        }

        if (!tileMapManager.GetNode(currentCoordinate).canBuild)
        {
            Debug.Log(string.Format("Position {0} is not buildable", currentCoordinate));
            canBuild = false;
        }

        if (canBuild && WillTowerBlockThePath())
        {
            Debug.Log(string.Format("Building at {0} would block all possible paths", currentCoordinate));
            canBuild = false;
        }

        return canBuild;
    }

    void BuildTower()
    {
        Node node = tileMapManager.GetNode(currentCoordinate);
        Vector3 cellWorldPosition = tileMap.GetCellCenterWorld(currentCoordinate);

        Instantiate(
            selectedTower,
            cellWorldPosition,
            Quaternion.identity,
            node.gameObject.transform
        );

        gameControler.SpendCredits(SelectedWeaponCost());

        node.canBuild = false;

        gameControler.BroadcastRecalculatePathToAllEnemies();

        Debug.Log("Tower Built");
    }

    float SelectedWeaponCost()
    {
        return selectedTower.GetComponent<Tower>().weaponCost;
    }

    void UpdateFocusedTitle()
    {
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int coordinate = grid.WorldToCell(mouseWorldPos);
        //transform.localPosition = tilemap.GetCellCenterLocal(coordinate);

        if (coordinate != currentCoordinate)
        {
            currentCoordinate = coordinate;
            currentTile = tileMap.GetTile<Tile>(currentCoordinate);
            coordinateChanged = true;
        } else
        {
            coordinateChanged = false;
        }
    }

    bool WillTowerBlockThePath()
    {
        Node node = tileMapManager.GetNode(currentCoordinate);
        node.canBuild = false;

        Debug.Assert(pathfinder.HasPath(), "Expected a valid path to exist when WillTowerBlockThePath() was called");

        pathfinder.RecalculatePath();

        bool willPathBeBlocked = !pathfinder.HasPath();
  
        node.canBuild = true;

        pathfinder.RecalculatePath();

        Debug.Assert(pathfinder.HasPath(), "Expected a valid path to exist after WillTowerBlockThePath() was called");

        return willPathBeBlocked;
    }
}
