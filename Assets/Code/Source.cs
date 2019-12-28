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

    public void Spawn(EnemyType enemyType)
    {
        switch(enemyType)
        {
            case EnemyType.Sphere:
                Instantiate(
                    enemyCirclePrefab,
                    transform.position,
                    Quaternion.identity,
                    GetEnemiesContainer().transform
                );
                break;
            case EnemyType.Square:
                Instantiate(
                    enemySquarePrefab,
                    transform.position,
                    Quaternion.identity,
                    GetEnemiesContainer().transform
                );
                break;
            case EnemyType.None:
                break;
            default:
                Debug.LogError("Found a new type of EnemyType! Please FIXME");
                break;
        }
    }

    GameObject GetEnemiesContainer()
    {
        return GameObject.Find("Enemies").gameObject;
    }
}
