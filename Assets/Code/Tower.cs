using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public float baseWeaponCooldown = 0.5f;
    public float baseWeaponDamage = 1.0f;
    public float baseWeaponRange = 2.0f;
    public float towerCost = 50f;

    public float cooldownUpgradeMultiplier = 0.8f;
    public float damageUpgradeMultiplier = 1.2f;
    public float rangeUpgradeMultiplier = 1.2f;

    float damageDealt;

    float weaponCooldown;
    float weaponDamage;
    float weaponRange;
    float cooldown;

    public GameObject bulletObjectPrefab;
 
    GameObject bulletsContainer;
    GameController gameController;
    List<WeaponModifier> weaponModifiers = new List<WeaponModifier>();

    void Start()
    {
        bulletsContainer = GameObject.Find("Bullets");
        gameController = GameController.GetInstance();

        weaponCooldown = baseWeaponCooldown;
        weaponDamage = baseWeaponDamage;
        weaponRange = baseWeaponRange;

        cooldown = weaponCooldown;
    }

    void Update()
    {
        for (int i = 0; i < gameController.GetGameSpeed(); i++)
        {
            WeaponUpdate();
        }
    }

    void WeaponUpdate()
    {
        Aim();
        TickWeaponCooldown();
    }

    void Aim()
    {
        GameObject target = FindNearestTarget();

        if (target == null)
        {
            return;
        }

        Vector3 targetPos = target.transform.position;
        Vector3 objectPos = transform.position; // Camera.main.WorldToScreenPoint(transform.position);
        Vector3 diff = targetPos - objectPos;

        float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    void TickWeaponCooldown()
    {
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }
        else
        {
            cooldown = weaponCooldown;
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject target = FindNearestTarget();

        if (target == null)
        {
            return;
        }

        InflictDamage(target);
        SpawnBullet(target);
    }

    void InflictDamage(GameObject target)
    {
        Enemy enemy = target.GetComponent<Enemy>();

        enemy.DealDamage(weaponDamage);

        damageDealt += weaponDamage;
    }

    GameObject FindNearestTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Enemy");
        float nearestDistance = float.PositiveInfinity;
        GameObject nearest = null;
        Vector3 position = transform.position;

        foreach (var target in targets)
        {
            float distance = (position - target.transform.position).magnitude;

            if (distance < nearestDistance && distance <= weaponRange)
            {
                nearestDistance = distance;
                nearest = target;
            }
        }

        return nearest;
    }

    void IncreaseDangerTint()
    {
        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        Color color = spriteRenderer.color;
        int weaponModifierCount = weaponModifiers.Count;
        float multiplier = 0.2f;

        if (weaponModifierCount <= 1)
        {
            color = Color.Lerp(
                color,
                Color.green,
                multiplier);
        }
        else if (weaponModifierCount <= 3)
        {
            color = Color.Lerp(
                color,
                Color.yellow,
                multiplier);
        }
        else if (weaponModifierCount <= 5)
        {
            color = Color.Lerp(
                color,
                Color.red,
                multiplier);
        }

        color = Color.Lerp(
            color,
            Color.red,
            multiplier);

        spriteRenderer.color = color;
    }

    public float DPS()
    {
        return ShootingSpeed() * weaponDamage;
    }

    public float ShootingSpeed()
    {
        return 1.0f / weaponCooldown;
    }

    void SpawnBullet(GameObject target) {
        GameObject newGameObject =
            Instantiate(
                bulletObjectPrefab,
                transform.position,
                Quaternion.identity,
                bulletsContainer.transform
            );

        Bullet bullet = newGameObject.GetComponent<Bullet>();
        bullet.SetTarget(target);
    }

    public float DamageDealt()
    {
        return damageDealt;
    }

    public float GetWeaponDamage()
    {
        return weaponDamage;
    }

    public float GetWeaponRange()
    {
        return weaponRange;
    }

    public void AddModifier(WeaponModifier modifier)
    {
        switch (modifier)
        {
            case WeaponModifier.DAMAGE:
                weaponDamage *= damageUpgradeMultiplier;
                break;
            case WeaponModifier.SPEED:
                weaponCooldown *= cooldownUpgradeMultiplier;
                break;
            case WeaponModifier.RANGE:
                weaponRange *= rangeUpgradeMultiplier;
                break;
        }

        IncreaseDangerTint();
        weaponModifiers.Add(modifier);
    }

    public int GetNumberOfModifiers()
    {
        return weaponModifiers.Count;
    }
}
