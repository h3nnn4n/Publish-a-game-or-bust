﻿using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;
using System;

public class TowerSelectionController : MonoBehaviour
{
    private static TowerSelectionController instance;

    GameController gameController;
    TileMapManager tileMapManager;
    Grid grid;
    Camera mainCamera;
    Vector3Int currentCoordinate;

    UiController uiController;
    TowerUi towerUi;
    TowerUpgradeUi towerUpgradeUi;

    public float resellValue = 0.75f;

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
        gameController = GameController.GetInstance();
        uiController = gameController.GetUiController();
        tileMapManager = TileMapManager.GetInstance();

        mainCamera = Camera.main;
    }

    void Update()
    {
        if (!UpdateActiveGrid())
        {
            return;
        }

        UpdateFocusedTitle();

        if (Input.GetMouseButtonDown(0))
        {
            if (HasTower())
            {
                OpenTowerUi();
            }
        }
    }

    void OpenTowerUi()
    {
        towerUi = uiController.GetTowerUi();
        towerUpgradeUi = uiController.GetTowerUpgradeUi();

        Node towerNode = tileMapManager.GetNode(currentCoordinate);

        towerUi.Enable();
        towerUi.SetTowerNode(towerNode);
        towerUpgradeUi.SetTowerNode(towerNode);
    }

    bool HasTower()
    {
        Node node = tileMapManager.GetNode(currentCoordinate);

        return node.HasTower() && !node.IsTowerNew();
    }

    public void Load()
    {
        gameController = GameController.GetInstance();
        tileMapManager = TileMapManager.GetInstance();

        grid = gameController.GetGrid();
    }

    bool UpdateActiveGrid()
    {
        grid = gameController.GetGrid();

        return grid != null;
    }

    void UpdateFocusedTitle()
    {
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int coordinate = grid.WorldToCell(mouseWorldPos);

        currentCoordinate = coordinate;
    }

    TowerUi GetTowerUi()
    {
        gameController = GameController.GetInstance();
        uiController = gameController.GetUiController();
        towerUi = uiController.towerUi;

        return towerUi;
    }

    TowerUpgradeUi GetTowerUpgradeUi()
    {
        gameController = GameController.GetInstance();
        uiController = gameController.GetUiController();
        towerUpgradeUi = uiController.towerUpgradeUi;

        return towerUpgradeUi;
    }

    public void SellTower()
    {
        Debug.Log("Selling Tower");

        towerUi = GetTowerUi();

        Node towerNode = towerUi.GetTowerNode();
        GameObject nodeGameObject = towerNode.gameObject;
        GameObject towerGameObject = nodeGameObject.transform.Find("tower").gameObject;
        Tower tower = towerGameObject.GetComponent<Tower>();

        towerNode.SetBuildable();

        gameController.AddCredits(tower.towerCost * resellValue);

        Destroy(towerGameObject);

        gameController.BroadcastRecalculatePathToAllEnemies();

        towerUi.Disable();
    }

    public void OpenTowerUpgradeUi()
    {
        GetTowerUpgradeUi().Enable();
    }

    public void ConfirmTowerUpgrade()
    {
        GetTowerUpgradeUi().ConfirmTowerUpgrade();
    }

    public void CloseTowerUpgradeUi()
    {
        GetTowerUpgradeUi().Disable();
    }

    public void SetWeaponUpgradeType(string modifier)
    {
        switch(modifier)
        {
            case "speed":
                GetTowerUpgradeUi().SetWeaponUpgradeType(WeaponModifier.SPEED);
                break;
            case "damage":
                GetTowerUpgradeUi().SetWeaponUpgradeType(WeaponModifier.DAMAGE);
                break;
            case "range":
                GetTowerUpgradeUi().SetWeaponUpgradeType(WeaponModifier.RANGE);
                break;
        }
    }

    public void CloseUi()
    {
        GetTowerUi().Disable();
        GetTowerUpgradeUi().Disable();
    }

    public static TowerSelectionController GetInstance()
    {
        return instance;
    }
}
