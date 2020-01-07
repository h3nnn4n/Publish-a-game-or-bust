using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    GameObject sink;
    GameController gameController;
    Pathfinder globalPathfinder;
    TileMapManager tileMapManager;

    List<Vector2Int> path;
    Vector2Int currentNode;

    HealthBar healthBar;

    public EnemyType enemyType;
    public float health = 10;
    public float speed = 1f;
    public float credits = 10f;
    public float nodeDistanceThreshold = 0.2f;
    public float sinkDistanceThreshold = 0.5f;

    float currentHealth;
    List<EnemyModifier> modifiers;

    public GameObject deathParticlePrefab;

    void Start()
    {
        FindAndCacheSink();
        gameController = GameController.GetInstance();
        globalPathfinder = gameController.GetPathfinder();
        tileMapManager = TileMapManager.GetInstance();

        currentHealth = health;

        path = globalPathfinder.GetPath();
        GetNextNodeFromPathfinder();

        healthBar = new HealthBar(gameObject);
    }

    void Update()
    {
        for (int i = 0; i < gameController.GetGameSpeed(); i++)
        {
            EnemyUpdate();
        }
    }

    void EnemyUpdate()
    {
        FindAndCacheSink();
        Despawn();
        MoveTowardsSink();
        healthBar.Update();
    }

    void Despawn()
    {
        Vector3 sinkPosition = sink.transform.position;
        Vector3 selfPosition = transform.position;
        Vector3 distance = sinkPosition - selfPosition;

        if (distance.magnitude < sinkDistanceThreshold)
        {
            gameController.LoseLife();
            Destroy(gameObject);

            Instantiate(
                deathParticlePrefab,
                transform.position,
                Quaternion.identity);
        }

        if (currentHealth <= 0)
        {
            gameController.AddCredits(credits);
            Destroy(gameObject);

            Instantiate(
                deathParticlePrefab,
                transform.position,
                Quaternion.identity);
        }
    }

    void MoveTowardsSink()
    {
        Vector2 nodePosition = currentNode + new Vector2(-1.5f, 0f);
        Vector2 selfPosition = transform.position;
        Vector2 moveDirection = nodePosition - selfPosition;

        if(moveDirection.magnitude < nodeDistanceThreshold)
        {
            GetNextNodeFromPathfinder();
            return;
        }

        Vector3 step = moveDirection.normalized * speed * Time.deltaTime;

        transform.position += step;
    }

    void GetNextNodeFromPathfinder()
    {
        currentNode = path[0];
        path.RemoveAt(0);
    }

    void FindAndCacheSink()
    {
        if (sink == null)
        {
            sink = GameObject.FindGameObjectWithTag("Sink");
        }
    }

    public void DealDamage(float damage)
    {
        currentHealth -= damage;
    }

    public void RecalculatePath()
    {
        Vector3Int gridPosition = tileMapManager.GetGridCelPosition(transform.position);

        var pathfinder = new PathfinderStandalone();
        pathfinder.startPoint = (Vector2Int)gridPosition;
        pathfinder.endPoint = globalPathfinder.endPoint;

        pathfinder.CalculatePath();

        path = pathfinder.GetPath();
        Debug.AssertFormat(pathfinder.HasPath(), "Pathfinder could not find a path!");

        GetNextNodeFromPathfinder();
    }

    void OnDrawGizmos()
    {
        Vector2 enemyPosition = transform.position;

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(enemyPosition, 0.2f);

        Gizmos.color = Color.red;

        if (path != null && path.Count > 0)
        {
            foreach (var node in path)
            {
                Gizmos.DrawSphere(new Vector3(node.x - 1.5f, node.y, 0f), 0.2f);
            }
        }

        Gizmos.color = Color.magenta;

        Gizmos.DrawLine(
            transform.position,
            new Vector3(currentNode.x - 1.5f, currentNode.y, 0f));

        if (path.Count > 0)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(
                transform.position,
                new Vector3(path[0].x - 1.5f, path[0].y, 0f));
        }
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public void ApplyModifiers(List<EnemyModifier> enemyModifiers)
    {
        modifiers = new List<EnemyModifier>(enemyModifiers);

        float baseHealth = health;
        float baseCredits = credits;
        float baseSpeed = speed;
        float baseNodeDistanceThreshold = nodeDistanceThreshold;

        foreach (var modifier in modifiers)
        {
            switch(modifier)
            {
                case EnemyModifier.DOUBLE_BASE_CREDITS:
                    credits += baseCredits;
                    IncreaseDangerTint();
                    break;
                case EnemyModifier.DOUBLE_BASE_HEALTH:
                    currentHealth += baseHealth;
                    IncreaseDangerTint();
                    break;
                case EnemyModifier.DOUBLE_BASE_SPEED:
                    speed += baseSpeed;
                    nodeDistanceThreshold += baseNodeDistanceThreshold;
                    IncreaseDangerTint();
                    break;
            }
        }
    }

    void IncreaseDangerTint()
    {
        SpriteRenderer spriteRenderer = GetComponentsInChildren<SpriteRenderer>().FirstOrDefault(r => r.CompareTag("EnemySprite"));
        Color color = spriteRenderer.color;
        float multiplier = 0.125f;

        switch(enemyType)
        {
            case EnemyType.Sphere:
                color = Color.Lerp(
                    color,
                    Color.green,
                    multiplier);
                break;
            case EnemyType.Square:
                color = Color.Lerp(
                    color,
                    Color.red,
                    multiplier);
                break;
        }

        spriteRenderer.color = color;
    }
}
