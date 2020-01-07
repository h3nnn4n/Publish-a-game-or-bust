using System;
using UnityEngine;
using UnityEngine.UI;

public class TowerUpgradeUi
{
    public bool active;

    GameController gameController;
    UiController uiController;

    GameObject gameUi;
    GameObject inGameUi;
    GameObject towerUpgradeUi;

    Text weaponSpeedValue;
    Text weaponDamageValue;
    Text weaponRangeValue;
    Text weaponDPSValue;

    Node towerNode;

    WeaponModifier weaponModifier;

    public TowerUpgradeUi()
    {
        gameController = GameController.GetInstance();
        uiController = gameController.GetUiController();

        gameUi = GameObject.Find("GameUi");
        inGameUi = gameUi.transform.Find("InGameUi").gameObject;
        towerUpgradeUi = inGameUi.transform.Find("TowerUpgradeUi").gameObject;
        weaponSpeedValue = towerUpgradeUi.transform.Find("WeaponSpeedValue").gameObject.GetComponent<Text>();
        weaponDamageValue = towerUpgradeUi.transform.Find("WeaponDamageValue").gameObject.GetComponent<Text>();
        weaponRangeValue = towerUpgradeUi.transform.Find("WeaponRangeValue").gameObject.GetComponent<Text>();
        weaponDPSValue = towerUpgradeUi.transform.Find("WeaponDPSValue").gameObject.GetComponent<Text>();

        Disable();
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
        weaponDamageValue.text = tower.GetWeaponDamage().ToString();
        weaponRangeValue.text = tower.GetWeaponRange().ToString();
        weaponDPSValue.text = tower.DPS().ToString();
    }

    public void Enable()
    {
        towerUpgradeUi.SetActive(true);
        active = true;
    }

    public void Disable()
    {
        towerUpgradeUi.SetActive(false);
        active = false;
    }

    public void ConfirmTowerUpgrade()
    {
        Tower tower = GetTower();

        // Check if can upgrade
        tower.AddModifier(weaponModifier); // Upgrade towers
        Disable(); // Close Upgrade UI
    }

    public void SetTowerNode(Node node)
    {
        towerNode = node;
    }

    public Node GetTowerNode()
    {
        return towerNode;
    }

    public void SetWeaponUpgradeType(WeaponModifier modifier)
    {
        Debug.Log(modifier.ToString());
        weaponModifier = modifier;
    }
}
