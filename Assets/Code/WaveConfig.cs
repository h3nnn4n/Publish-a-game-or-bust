using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveConfig : MonoBehaviour
{
    public int waveIndex;

    public float timeBeforeWave = 2;

    public List<float> timeIntervals;
    public List<EnemyType> enemies;

    int spawnIndex;

    void Start()
    {
        Debug.Assert(
            timeIntervals.Count == enemies.Count,
            "The number of intervals should equal the number of enemies");
    }

    public bool IsFinished()
    {
        return spawnIndex >= timeIntervals.Count;
    }

    public NextEnemySpawn GetNextEnemySpawn()
    {
        var nextEnemySpawn = new NextEnemySpawn(
            timeIntervals[spawnIndex],
            enemies[spawnIndex]);

        spawnIndex++;

        return nextEnemySpawn;
    }
}
