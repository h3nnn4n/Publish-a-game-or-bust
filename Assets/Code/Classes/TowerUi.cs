using UnityEngine;
using System.Collections;
using System;

public class TowerUi
{
    GameController gameController;
    UiController uiController;

    GameObject gameUi;
    GameObject inGameUi;
    GameObject towerUi;

    Node towerNode;

    public TowerUi()
    {
        gameController = GameController.GetInstance();
        uiController = gameController.GetUiController();

        gameUi = GameObject.Find("GameUi");
        inGameUi = gameUi.transform.Find("InGameUi").gameObject;
        towerUi = inGameUi.transform.Find("TowerUi").gameObject;

        towerUi.SetActive(false);
    }

    public void Enable()
    {
        towerUi.SetActive(true);
    }

    public void SetTowerNode(Node node)
    {
        towerNode = node;
    }

    public Node GetTowerNode()
    {
        return towerNode;
    }
}
