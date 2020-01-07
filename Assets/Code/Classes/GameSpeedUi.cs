using UnityEngine;
using UnityEngine.UI;

public class GameSpeedUi
{
    GameController gameController;
    GameObject gameUi;
    GameObject inGameUi;
    GameObject gameSpeedUi;
    Text gameSpeedText;

    public GameSpeedUi()
    {
        gameController = GameController.GetInstance();

        gameUi = GameObject.Find("GameUi");
        inGameUi = gameUi.transform.Find("InGameUi").gameObject;
        gameSpeedUi = inGameUi.transform.Find("SpeedUi").gameObject;
        gameSpeedText = gameSpeedUi.transform.Find("GameSpeedText").gameObject.GetComponent<Text>();
    }

    public void Update()
    {
        gameSpeedText.text = gameController.GetGameSpeed().ToString();
    }
}
