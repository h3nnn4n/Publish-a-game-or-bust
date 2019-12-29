using System;
using System.Collections;
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

    public float health = 10;
    public float speed = 1f;
    public float credits = 10f;
    public float nodeDistanceThreshold = 0.2f;
    public float sinkDistanceThreshold = 0.5f;

    readonly float speedScale = 0.1f;

    void Start()
    {
        FindAndCacheSink();
        gameController = GameController.GetInstance();
        globalPathfinder = gameController.GetPathfinder();
        tileMapManager = TileMapManager.GetInstance();

        path = globalPathfinder.GetPath();
        GetNextNodeFromPathfinder();
    }

    void Update()
    {
        FindAndCacheSink();
        Despawn();
        MoveTowardsSink();
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
        }

        if (health <= 0)
        {
            gameController.AddCredits(credits);
            Destroy(gameObject);
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

        Vector3 step = moveDirection.normalized * speed * speedScale;

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
        health -= damage;
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

        foreach (var node in path)
        {
            Gizmos.DrawSphere(new Vector3(node.x - 1.5f, node.y, 0f), 0.2f);
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
}
