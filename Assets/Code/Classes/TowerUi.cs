using UnityEngine;
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

        weaponDamageValue.text = FormatAttribute(
            tower.baseWeaponDamage,
            tower.GetModifierBonus(WeaponModifier.DAMAGE)
        );

        weaponRangeValue.text = FormatAttribute(
            tower.baseWeaponRange,
            tower.GetModifierBonus(WeaponModifier.RANGE)
        );

        weaponSpeedValue.text = FormatAttribute(
            tower.baseWeaponCooldown,
            tower.GetModifierBonus(WeaponModifier.SPEED)
        );

        weaponDPSValue.text = FormatAttribute(tower.BaseDPS(), tower.BonusDPS());
        damageDealtValue.text = FormatAttribute(GetTower().DamageDealt(), 0f);
    }

    string FormatAttribute(float baseValue, float modifierValue)
    {
        string formated;

        if (modifierValue == 0)
        {
            formated = string.Format("{0:n2}", baseValue);
        }
        else
        {
            formated = string.Format("{0:n2} (+{1:n2})", baseValue, modifierValue);
        }

        return formated;
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
