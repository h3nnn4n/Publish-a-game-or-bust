using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Source : MonoBehaviour
{
    float spawnTimer;

    public float spawnFrequency = 5;
    public GameObject enemyPrefab;

    void Start()
    {
        spawnTimer = spawnFrequency;
    }

    void Update()
    {
        if (spawnTimer < 0)
        {
            spawnTimer = spawnFrequency;
            Spawn();
        } else
        {
            spawnTimer -= Time.deltaTime;
        }
    }

    void Spawn()
    {
        Instantiate(
            enemyPrefab,
            transform.position,
            Quaternion.identity
        );
    }
}
