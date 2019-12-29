using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class TowerUi
{
    public bool active;

    GameController gameController;
    UiController uiController;

    GameObject gameUi;
    GameObject inGameUi;
    GameObject towerUi;
    Text damageDealtLabel;

    Node towerNode;

    public TowerUi()
    {
        gameController = GameController.GetInstance();
        uiController = gameController.GetUiController();

        gameUi = GameObject.Find("GameUi");
        inGameUi = gameUi.transform.Find("InGameUi").gameObject;
        towerUi = inGameUi.transform.Find("TowerUi").gameObject;
        damageDealtLabel = towerUi.transform.Find("DamageDealtLabel").gameObject.GetComponent<Text>();

        towerUi.SetActive(false);
    }

    Tower GetTower()
    {
        GameObject nodeGameObject = towerNode.gameObject;
        GameObject towerGameObject = nodeGameObject.transform.GetChild(0).gameObject;
        Tower tower = towerGameObject.GetComponent<Tower>();

        return tower;
    }

    public void Update()
    {
        damageDealtLabel.text = GetTower().DamageDealt().ToString();
    }

    public void Enable()
    {
        towerUi.SetActive(true);
        active = true;
    }

    public void Disable()
    {
        towerUi.SetActive(false);
        active = false;
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
