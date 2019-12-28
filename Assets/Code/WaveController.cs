using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    GameController gameController;
    Source source;

    bool finished;
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
        if (gameController.GetGameState() != GameState.IN_GAME)
        {
            return;
        }

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
        if (finished)
        {
            return;
        }

        currentWaveIndex++;

        Debug.LogFormat("Finished wave {0}", currentWaveIndex);
        
        if (currentWaveIndex >= waves.Length)
        {
            FinishLevel();
            return;
        }

        currentWave = waves[currentWaveIndex];

        timer = currentWave.timeBeforeWave;
    }

    void FinishLevel()
    {
        finished = true;
        Debug.Log("Finished all waves for current level");

        //gameController.FinishedLevel();
    }

    void FindSource()
    {
        if (source == null)
        {
            source = GameObject.FindGameObjectWithTag("Source").GetComponent<Source>();
        }
        Debug.Assert(source != null, "All maps should have a source!");
    }

    public bool LevelFinished()
    {
        return finished;
    }
}