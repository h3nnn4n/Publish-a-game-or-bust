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

    Text weaponSpeedValue;
    Text weaponDamageValue;
    Text weaponRangeValue;
    Text weaponDPSValue;
    Text damageDealtValue;

    Node towerNode;

    public TowerUi()
    {
        gameController = GameController.GetInstance();
        uiController = gameController.GetUiController();

        gameUi = GameObject.Find("GameUi");
        inGameUi = gameUi.transform.Find("InGameUi").gameObject;
        towerUi = inGameUi.transform.Find("TowerUi").gameObject;
        weaponSpeedValue = towerUi.transform.Find("WeaponSpeedValue").gameObject.GetComponent<Text>();
        weaponDamageValue = towerUi.transform.Find("WeaponDamageValue").gameObject.GetComponent<Text>();
        weaponRangeValue = towerUi.transform.Find("WeaponRangeValue").gameObject.GetComponent<Text>();
        weaponDPSValue = towerUi.transform.Find("WeaponDPSValue").gameObject.GetComponent<Text>();
        damageDealtValue = towerUi.transform.Find("DamageDealtValue").gameObject.GetComponent<Text>();

        towerUi.SetActive(false);
    }

    Tower GetTower()
    {
        GameObject nodeGameObject = towerNode.gameObject;
        GameObject towerGameObject = nodeGameObject.transform.Find("tower").gameObject;

        Tower tower = towerGameObject.GetComponent<Tower>();

        Debug.AssertFormat(tower != null, "Found no Tower!");

        return tower;
    }

    public void Update()
    {
        Tower tower = GetTower();

        weaponSpeedValue.text = tower.ShootingSpeed().ToString();
        weaponDamageValue.text = tower.weaponDamage.ToString();
        weaponRangeValue.text = tower.weaponRange.ToString();
        weaponDPSValue.text = tower.DPS().ToString();
        damageDealtValue.text = GetTower().DamageDealt().ToString();
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
