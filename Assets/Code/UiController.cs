using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
    Text textCreditsAmount;
    Text textLivesAmount;

    Text debugText2;
    Text debugText1;

    GameController gameController;

    GameObject gameUi;
    GameObject inGameUi;
    GameObject mainMenuUi;
    GameObject levelSelectUi;

    public TowerUi towerUi;
    public TowerUpgradeUi towerUpgradeUi;
    public GameSpeedUi gameSpeedUi;

    float credits;
    int nLives;

    void Start()
    {
        gameController = GameController.GetInstance();

        CacheUiObjects();

        towerUi = new TowerUi();
        towerUpgradeUi = new TowerUpgradeUi();
        gameSpeedUi = new GameSpeedUi();
    }

    private void Update()
    {
        UpdateTowerUi();
        UpdateTowerUpgradeUi();
        UpdateGameSpeedUI();
    }

    void UpdateTowerUi()
    {
        if (towerUi != null && towerUi.active)
        {
            towerUi.Update();
        }
    }

    void UpdateTowerUpgradeUi()
    {
        if (towerUpgradeUi != null && towerUpgradeUi.active)
        {
            towerUpgradeUi.Update();
        }
    }

    void UpdateGameSpeedUI()
    {
        gameSpeedUi.Update();
    }

    void CacheUiObjects()
    {
        gameUi = GameObject.Find("GameUi");

        inGameUi = gameUi.transform.Find("InGameUi").gameObject;
        mainMenuUi = gameUi.transform.Find("MainMenuUi").gameObject;
        levelSelectUi = gameUi.transform.Find("LevelSelectUi").gameObject;
    }

    void LoadInGameUI()
    {
        inGameUi.SetActive(true);
        mainMenuUi.SetActive(false);
        levelSelectUi.SetActive(false);

        textCreditsAmount = GameObject.Find("TextCreditsAmount").GetComponent<Text>();
        textLivesAmount = GameObject.Find("TextLivesAmount").GetComponent<Text>();
    }

    void LoadLevelSelectUI()
    {
        inGameUi.SetActive(false);
        mainMenuUi.SetActive(false);
        levelSelectUi.SetActive(true);

        towerUi.Disable();
    }

    void LoadMainMenuUI()
    {
        inGameUi.SetActive(false);
        mainMenuUi.SetActive(true);
        levelSelectUi.SetActive(false);
    }

    public void SetGameState(string text)
    {
        CacheUiObjects();

        if (debugText1 == null)
        {
            debugText1 = gameUi.transform.Find("DebugGameState").gameObject.GetComponent<Text>();
        }

        debugText1.text = text;
    }

    public void SetDebugHeartBeat(string text)
    {
        CacheUiObjects();

        if (debugText2 == null)
        {
            debugText2 = gameUi.transform.Find("DebugHeartBeat").gameObject.GetComponent<Text>();
        }

        debugText2.text = text;
    }

    public void SetUiMode(GameState gameState)
    {
        CacheUiObjects();

        switch (gameState)
        {
            case GameState.IN_GAME:
                LoadInGameUI();
                break;
            case GameState.LEVEL_SELECT_MENU:
                LoadLevelSelectUI();
                break;
            case GameState.MAIN_MENU:
                LoadMainMenuUI();
                break;
            default:
                Debug.LogErrorFormat("Found unknow gamestate: {0}", gameState);
                break;
        }
    }

    public void SetLives(int nLives)
    {
        if (this.nLives != nLives)
        {
            this.nLives = nLives;
            textLivesAmount.text = this.nLives.ToString();
        }
    }

    public void SetCredits(float newCredits)
    {
        if (System.Math.Abs(newCredits - credits) > 1e-04)
        {
            credits = newCredits;
            textCreditsAmount.text = credits.ToString();
        }
    }

    public TowerUi GetTowerUi()
    {
        return towerUi;
    }

    public TowerUpgradeUi GetTowerUpgradeUi()
    {
        return towerUpgradeUi;
    }
}
