using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameController : MonoBehaviour
{
    public GameObject sourcePrefab;
    public GameObject sinkPrefab;
    public GameObject nodePrefab;
    public GameObject emptyCellPrefab;
    public GameObject blockCellPrefab;

    UiController uiController;
    TileMapManager tileMapManager;
    Pathfinder pathfinder;
    BuildController buildController;

    readonly LevelController levelController = new LevelController();

    float credits;

    GameState gameState = GameState.MAIN_MENU;

    int currentLives;
    bool gameOver;
    bool levelFinished;

    Tilemap tileMap;
    Grid grid;

    private static GameController instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
            Debug.LogError("Warning, GameController tryed to spawn more than once");
        }
    }

    void Start()
    {
        Debug.Assert(instance == this, "Singleton constraint was violated! WTF");

        Debug.Log("GameController started");

        tileMapManager = TileMapManager.GetInstance();
        buildController = GetComponent<BuildController>();

        GetUiController();

        SetGameStateToMainMenu();
    }

    void Update()
    {
        Debug.Assert(instance == this, "Singleton constraint was violated! WTF");

        switch (gameState)
        {
            case GameState.MAIN_MENU:
                MainMenuLoop();
                break;
            case GameState.LEVEL_SELECT_MENU:
                LevelSelectMenuLoop();
                break;
            case GameState.IN_GAME:
                InGameLoop();
                break;
            default:
                Debug.LogErrorFormat("Warning, found a untreated state: {0}", gameState);
                break;
        }

        uiController.SetGameState(gameState.ToString());
    }

    void MainMenuLoop()
    {
        uiController.SetDebugHeartBeat("mainloop");
    }

    void LevelSelectMenuLoop()
    {
        uiController.SetDebugHeartBeat("select menu");
    }

    void InGameLoop()
    {
        uiController.SetDebugHeartBeat("ingame");

        CheckGameOver();
        CheckForLevelCompleted();

        GetUiController().SetCredits(credits);
        GetUiController().SetLives(currentLives);
    }

    void LoadLevel()
    {
        Debug.LogFormat("LoadLevel triggered with gameState: {0}", gameState);

        Debug.Assert(instance == this, "Singleton constraint was violated! WTF");
        //Debug.AssertFormat(gameState == GameState.IN_GAME,
        //    "LoadLevel should only be called when GameState is IN_GAME. It was {0}",
        //    gameState);
        SetGameStateToInGame();
        // FIXME This really smells. There is somethign wrong here but I dont know what
        // There is no reason to have to call this here.

        tileMap = GetTileMap();
        grid = GetGrid();

        tileMapManager.LoadLevel();

        SpawnGameObjects();
        HideTiles();

        GetPathfinder();
        pathfinder.RecalculatePath();

        GameObject gridGameObject = GameObject.FindGameObjectWithTag("Grid");
        WaveCredits waveCredits = gridGameObject.GetComponent<WaveCredits>();

        credits = waveCredits.startingCredits;
        currentLives = waveCredits.startingLives;

        buildController.enabled = true;
        buildController.Load();

        WaveController waveController = grid.GetComponent<WaveController>();
        waveController.enabled = true;

        gameOver = false;
        levelFinished = false;
    }

    void SpawnGameObjects()
    {
        var allCellPositions = tileMap.cellBounds.allPositionsWithin;

        foreach (var cellPosition in allCellPositions)
        {
            SpawnGameObject(cellPosition);
        }
    }

    void SpawnGameObject(Vector3Int cellPosition)
    {
        GetPathfinder();

        Debug.Assert(pathfinder != null, "pathFinder is null!");

        Vector3 cellWorldPosition = tileMap.GetCellCenterWorld(cellPosition);
        Tile tile = tileMap.GetTile<Tile>(cellPosition);

        if (tile == null)
        {
            Debug.LogFormat("Found no tile at {0}", cellPosition);
            return;
        }

        string spriteName = tile.sprite.name;
        var node = tileMapManager.GetNode(cellPosition);
        node.type = spriteName;
        node.position = (Vector2Int)cellPosition;

        switch (spriteName)
        {
            case "source":
                Debug.Log(string.Format("Found source at: {0}", cellPosition));

                Instantiate(
                    sourcePrefab,
                    cellWorldPosition,
                    Quaternion.identity,
                    node.transform
                );

                node.SetEmpty();
                pathfinder.startPoint = (Vector2Int)cellPosition;

                break;
            case "sink":
                Debug.Log(string.Format("Found sink at: {0}", cellPosition));

                Instantiate(
                    sinkPrefab,
                    cellWorldPosition,
                    Quaternion.identity,
                    node.transform
                );

                node.SetEmpty();
                pathfinder.endPoint = (Vector2Int)cellPosition;

                break;
            case "empty":
                Instantiate(
                    emptyCellPrefab,
                    cellWorldPosition,
                    Quaternion.identity,
                    node.transform
                );

                node.SetEmpty();
                break;
            case "block":
                Instantiate(
                    blockCellPrefab,
                    cellWorldPosition,
                    Quaternion.identity,
                    node.transform
                );

                node.SetBuildable();
                break;
            default:
                break;
        }
    }

    void HideTiles()
    {
        tileMap = GetTileMap();
        grid = GetGrid();

        tileMap.color = new Color(0, 0, 0, 0);
    }

    void CheckGameOver()
    {
        if (currentLives <= 0 && !gameOver)
        {
            DoGameOver();
        }
    }

    void DoGameOver()
    {
        Debug.Log("GameOver triggered");

        UnloadLevel();
        gameOver = true;
    }

    void SetGameState(GameState newGameState)
    {
        gameState = newGameState;
    }

    public int EnemyAliveCount()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        return enemies.Length;
    }

    public void UnloadLevel()
    {
        StartCoroutine(UnloadLevelTransition());
    }

    IEnumerator UnloadLevelTransition()
    {
        var animationController = AnimationController.GetInstance();
        animationController.TriggerFadeOut();

        yield return new WaitForSeconds(0.26f);

        pathfinder.Reset();
        buildController.enabled = false;
        levelController.UnloadCurrentLevelScene();

        SetGameStateToLevelSelectMenu();

        animationController.TriggerFadeIn();

        yield return null;
    }

    public void SetLevelAndTriggerLoad(int level)
    {
        StartCoroutine(SetLevelAndTriggerLoadTransition(level));
    }

    IEnumerator SetLevelAndTriggerLoadTransition(int level)
    {
        var animationController = AnimationController.GetInstance();
        animationController.TriggerFadeOut();

        yield return new WaitForSeconds(0.26f);

        SetGameStateToInGame();

        levelController.SetCurrentLevel(level);
        levelController.LoadCurrentLevelScene();
    }

    public void FinishLoadingLevel()
    {
        LoadLevel();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetGameStateToMainMenu()
    {
        SetGameState(GameState.MAIN_MENU);
        GetUiController().SetUiMode(gameState);
        Debug.LogFormat("SetGameStateToMainMenu called. gameState is {0}", gameState.ToString());

        uiController.SetDebugHeartBeat(gameState.ToString());
    }

    public void SetGameStateToInGame()
    {
        currentLives = 1;

        SetGameState(GameState.IN_GAME);
        GetUiController().SetUiMode(gameState);
        Debug.LogFormat("SetGameStateToInGame called. gameState is {0}", gameState.ToString());

        uiController.SetDebugHeartBeat(gameState.ToString());
    }

    public void SetGameStateToLevelSelectMenu()
    {
        SetGameState(GameState.LEVEL_SELECT_MENU);
        GetUiController().SetUiMode(gameState);
        Debug.LogFormat("SetGameStateToLevelSelectMenu called. gameState is {0}", gameState.ToString());

        uiController.SetDebugHeartBeat(gameState.ToString());
    }

    public GameState GetGameState()
    {
        return gameState;
    }

    public void CheckForLevelCompleted()
    {
        if(grid == null)
        {
            return;
        }

        WaveController waveController = grid.gameObject.GetComponent<WaveController>();

        bool waveFinished = waveController.LevelFinished();
        int enemiesAlive = EnemyAliveCount();

        if (waveFinished && enemiesAlive <= 0 && levelFinished == false)
        {
            levelFinished = true;
            UnloadLevel();
        }
    }

    public void LoseLife()
    {
        currentLives--;
    }

    public void BroadcastRecalculatePathToAllEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (var enemyGameObject in enemies)
        {
            Enemy enemy = enemyGameObject.GetComponent<Enemy>();
            enemy.RecalculatePath();
        }

        Node sourceNode = tileMapManager.FindByType("source");
        pathfinder.startPoint = sourceNode.position;
        pathfinder.RecalculatePath();
    }

    public Node GetNodeForTile(Tile tile)
    {
        return tile.gameObject.GetComponent<Node>();
    }

    public float GetCredits()
    {
        return credits;
    }

    public void SpendCredits(float spent)
    {
        credits -= spent;
    }

    public static GameController GetInstance()
    {
        return instance;
    }

    public void AddCredits(float income)
    {
        credits += income;
    }

    public Tilemap GetTileMap()
    {
        return GameObject.FindGameObjectWithTag("Tilemap").GetComponent<Tilemap>();
    }

    public Grid GetGrid()
    {
        GameObject gridGameObject = GameObject.FindGameObjectWithTag("Grid");

        if (gridGameObject == null)
        {
            return null;
        }

        return gridGameObject.GetComponent<Grid>();
    }

    public UiController GetUiController()
    {
        if (uiController == null)
        {
            uiController = GameObject.FindGameObjectWithTag("UI").GetComponent<UiController>();
        }

        return uiController;
    }

    public Pathfinder GetPathfinder()
    {
        if (pathfinder == null )
        {
            pathfinder = GameObject.Find("PathfinderObject").GetComponent<Pathfinder>();
        }

        return pathfinder;
    }
}
