using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildController : MonoBehaviour
{
    private static BuildController instance;

    public GameObject basicTowerPrefab;
    public GameObject cannonTowerPrefab;

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

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
            Debug.Log("Tried to build more than one BuildController");
        }
    }

    void Start()
    {
        gameControler = GameController.GetInstance();
        pathfinder = gameControler.GetPathfinder();
        mainCamera = Camera.main;
    }

    public void Load()
    {
        gameControler = GameController.GetInstance();

        tileMapManager = TileMapManager.GetInstance();

        grid = gameControler.GetGrid();
        tileMap = gameControler.GetTileMap();

        selectedTower = null;
    }

    void Update()
    {
        if (gameControler.GetGameState() != GameState.IN_GAME)
        {
            return;
        }

        UpdateActiveGrid();
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

        if (!HasTowerSelected())
        {
            Debug.Log("Not tower selected");
            return false;
        }

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
        if (HasTowerSelected())
        {
            Debug.Log("No tower selected");
        }

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

    bool HasTowerSelected()
    {
        return selectedTower != null;
    }

    float SelectedWeaponCost()
    {
        return selectedTower.GetComponent<Tower>().towerCost;
    }

    void UpdateFocusedTitle()
    {
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int coordinate = grid.WorldToCell(mouseWorldPos);

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

    void UpdateActiveGrid()
    {
        grid = gameControler.GetGrid();
        tileMap = gameControler.GetTileMap();
    }

    public void SelectGunTower()
    {
        Debug.Log("Selected Gun Tower");
        GetInstance().selectedTower = basicTowerPrefab;
    }

    public void SelectCannonTower()
    {
        Debug.Log("Selected Cannon Tower");
        GetInstance().selectedTower = cannonTowerPrefab;
    }

    public static BuildController GetInstance()
    {
        return instance;
    }
}
