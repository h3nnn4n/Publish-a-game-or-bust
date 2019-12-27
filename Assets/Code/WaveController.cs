using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    GameController gameController;
    Source source;

    int currentWaveIndex;
    WaveConfig currentWave;
    WaveConfig[] waves;

    float timer;

    void Start()
    {
        gameController = GameController.GetInstance();
        Debug.Assert(gameController != null, "Could not find gameController!");


        var sourceGameObject = GameObject.FindGameObjectWithTag("Source");
        if (sourceGameObject != null)
        {
            source = sourceGameObject.GetComponent<Source>();
        }

        GameObject wavesContainer = transform.Find("Waves").gameObject;
        waves = wavesContainer.GetComponents<WaveConfig>();
        currentWave = waves[currentWaveIndex];
        timer = currentWave.timeBeforeWave;

        Debug.LogFormat("Found {0} WaveConfig objects", waves.Length);
    }

    void Update()
    {
        FindSource();

        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            if (currentWave.IsFinished())
            {
                AdvanceToNextWave();
            }
            else
            {
                AdvanceWave();
            }
        }
    }

    void AdvanceWave()
    {
        var nextEnemySpawn = currentWave.GetNextEnemySpawn();

        timer = nextEnemySpawn.countdown;

        source.Spawn(nextEnemySpawn.enemyType);
    }

    void AdvanceToNextWave()
    {
        if (currentWaveIndex + 1 >= waves.Length)
        {
            return;
        }

        currentWaveIndex++;

        Debug.LogFormat("Finished wave {0}", currentWaveIndex);

        currentWave = waves[currentWaveIndex];

        timer = currentWave.timeBeforeWave;
    }

    void FindSource()
    {
        if (source == null)
        {
            source = GameObject.FindGameObjectWithTag("Source").GetComponent<Source>();
        }
        Debug.Assert(source != null, "All maps should have a source!");
    }
}