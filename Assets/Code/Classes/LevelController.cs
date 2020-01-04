using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController
{
    int currentLevel = 1;

    void FinishedLoadingScene(Scene scene, LoadSceneMode mode)
    {
        EventProxy.FinishLoadingLevel();
        SceneManager.sceneLoaded -= FinishedLoadingScene;

        Debug.Log("Finished async scene load");

        var animationController = AnimationController.GetInstance();
        animationController.TriggerFadeIn();
    }

    public void SetCurrentLevel(int level)
    {
        currentLevel = level;
    }

    public void LoadCurrentLevelScene()
    {
        SceneManager.sceneLoaded += FinishedLoadingScene;

        SceneManager.LoadSceneAsync(
            string.Format("Level {0}", currentLevel),
            LoadSceneMode.Additive);

        Debug.Log("Trigged async scene load");
    }

    public void UnloadCurrentLevelScene()
    {
        Debug.LogFormat("Unloading scene {0}", currentLevel);

        SceneManager.UnloadSceneAsync(
            string.Format("Level {0}", currentLevel));
    }
}
