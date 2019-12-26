using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    float cooldown;

    public float weaponCooldown = 0.5f;
    public float weaponDamage = 1.0f;
    public float weaponRange = 2.0f;
    public float weaponCost = 50f;

    public GameObject bulletObjectPrefab;

    void Start()
    {
        cooldown = weaponCooldown;
    }

    void Update()
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

    void SpawnBullet(GameObject target) {
        GameObject newGameObject =
            Instantiate(
                bulletObjectPrefab,
                transform.position,
                Quaternion.identity
            );

        Bullet bullet = newGameObject.GetComponent<Bullet>();
        bullet.SetTarget(target);
    }
}
