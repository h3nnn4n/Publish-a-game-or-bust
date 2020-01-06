using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveConfig : MonoBehaviour
{
    public int waveIndex;
    public float timeBeforeWave = 2;
    public List<EnemyWave> enemies;
    public List<EnemyModifier> enemyModifiers;

    int spawnIndex;

    public bool IsFinished()
    {
        return spawnIndex >= enemies.Count;
    }

    public NextEnemySpawn GetNextEnemySpawn()
    {
        EnemyWave enemyWave = enemies[spawnIndex];

        var nextEnemySpawn = new NextEnemySpawn(
            enemyWave.interval,
            enemyWave.enemyType,
            enemyModifiers);

        if (enemyWave.Finished())
        {
            spawnIndex++;
        }
        else
        {
            enemyWave.BumpIndex();
        }

        return nextEnemySpawn;
    }
}
