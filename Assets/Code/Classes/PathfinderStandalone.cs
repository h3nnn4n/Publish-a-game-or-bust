using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfinderStandalone
{
    public PathfinderStandalone()
    {
        gameController = GameController.GetInstance();
        tileMapManager = TileMapManager.GetInstance();
    }

    GameController gameController;
    TileMapManager tileMapManager;

    public Vector2Int startPoint;
    public Vector2Int endPoint;

    bool hasPath;

    List<Vector2Int> path = new List<Vector2Int>();

    Queue<Vector2Int> queue = new Queue<Vector2Int>();
    HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
    Dictionary<Vector2Int, Vector2Int> pathGraph = new Dictionary<Vector2Int, Vector2Int>();

    public List<Vector2Int> GetPath()
    {
        return new List<Vector2Int>(path);
    }

    public bool HasPath()
    {
        return hasPath;
    }

    public void CalculatePath()
    {
        ResetEnvironment();

        EnqueueNeighbors(startPoint);

        while (queue.Count > 0)
        {
            Vector2Int position = queue.Dequeue();

            if (position == endPoint)
            {
                BuildPath();
                break;
            }

            Node node = tileMapManager.GetNode(position);

            if (node != null && node.canBuild)
            {
                EnqueueNeighbors(position);
            }
        }
    }

    void EnqueueNeighbors(Vector2Int position)
    {
        EnqueueNode(position + new Vector2Int(1, 0), position);
        EnqueueNode(position + new Vector2Int(0, 1), position);
        EnqueueNode(position + new Vector2Int(-1, 0), position);
        EnqueueNode(position + new Vector2Int(0, -1), position);
    }

    void EnqueueNode(Vector2Int node, Vector2Int parent)
    {
        if (visited.Contains(node))
        {
            return;
        }

        visited.Add(node);

        pathGraph.Add(
            node,
            parent
        );

        queue.Enqueue(node);
    }

    void BuildPath()
    {
        Vector2Int node = endPoint;
        path.Add(node);

        while (node != startPoint)
        {
            node = pathGraph[node];
            path.Add(node);
        }

        Debug.Log("Found a valid path!");
        hasPath = true;

        path.Reverse();
    }

    void ResetEnvironment()
    {
        hasPath = false;

        path.Clear();
        pathGraph.Clear();
        queue.Clear();
        visited.Clear();

        if (gameController == null)
        {
            gameController = GameController.GetInstance();
        }

        if (tileMapManager == null)
        {
            tileMapManager = TileMapManager.GetInstance();
        }
    }
}
