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
    }

    public void SetCurrentLevel(int level)
    {
        currentLevel = level;
    }

    public void ReloadLevel()
    {
        UnloadCurrentLevelScene();
        LoadCurrentLevelScene();
    }

    public void LoadNextLevel()
    {
        UnloadCurrentLevelScene();
        AdvanceLevel();
        LoadCurrentLevelScene();
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
        SceneManager.UnloadSceneAsync(
            string.Format("Level {0}", currentLevel));
    }

    void AdvanceLevel()
    {
        currentLevel++;
    }
}
