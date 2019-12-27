using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Source : MonoBehaviour
{
    public GameObject enemyPrefab;

    public void Spawn()
    {
        Instantiate(
            enemyPrefab,
            transform.position,
            Quaternion.identity
        );
    }

    public void Spawn(EnemyType enemyType)
    {
        switch(enemyType)
        {
            case EnemyType.Sphere:
                Instantiate(
                    enemyPrefab,
                    transform.position,
                    Quaternion.identity
                );
                break;
            case EnemyType.None:
                break;
            default:
                Debug.LogError("Found a new type of EnemyType! Please FIXME");
                break;
        }
    }
}
