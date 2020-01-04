using UnityEngine;

public class EventProxy : MonoBehaviour
{
    public void QuitGame()
    {
        GameController gameController = GameController.GetInstance();
        gameController.QuitGame();
    }

    public void SetGameStateToLevelSelectMenu()
    {
        GameController gameController = GameController.GetInstance();
        gameController.SetGameStateToLevelSelectMenu();
    }

    public void SetGameStateToInGame()
    {
        GameController gameController = GameController.GetInstance();
        gameController.SetGameStateToInGame();
    }

    public void SetGameStateToMainMenu()
    {
        GameController gameController = GameController.GetInstance();
        gameController.SetGameStateToMainMenu();
    }

    public void SetLevelAndTriggerLoad(int level)
    {
        GameController gameController = GameController.GetInstance();
        gameController.SetLevelAndTriggerLoad(level);
        Debug.Log("SetLevelAndTriggerLoad");
    }

    public void LeaveGameAndGoToLevelSelectMenu()
    {
        GameController gameController = GameController.GetInstance();
        gameController.UnloadLevel();
        //gameController.SetGameStateToLevelSelectMenu();
        Debug.Log("LeaveGameAndGoToLevelSelectMenu");
    }

    public static void FinishLoadingLevel()
    {
        GameController gameController = GameController.GetInstance();
        gameController.FinishLoadingLevel();
    }
}
