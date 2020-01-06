using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Source : MonoBehaviour
{
    public GameObject enemyCirclePrefab;
    public GameObject enemySquarePrefab;

    public void Spawn()
    {
        Instantiate(
            enemyCirclePrefab,
            transform.position,
            Quaternion.identity,
            GetEnemiesContainer().transform
        );
    }

    public void Spawn(NextEnemySpawn enemySpawn)
    {
        GameObject enemyGameObject;
        Enemy enemy;

        switch(enemySpawn.enemyType)
        {
            case EnemyType.Sphere:
                enemyGameObject = Instantiate(
                    enemyCirclePrefab,
                    transform.position,
                    Quaternion.identity,
                    GetEnemiesContainer().transform
                );
                break;
            case EnemyType.Square:
                enemyGameObject = Instantiate(
                    enemySquarePrefab,
                    transform.position,
                    Quaternion.identity,
                    GetEnemiesContainer().transform
                );
                break;
            default:
                throw new Exception("Found a new type of EnemyType! Please FIXME");
        }

        enemy = enemyGameObject.GetComponent<Enemy>();
        enemy.ApplyModifiers(enemySpawn.enemyModifiers);
    }

    GameObject GetEnemiesContainer()
    {
        return GameObject.Find("Enemies").gameObject;
    }
}
