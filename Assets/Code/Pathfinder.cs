using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    GameController gameController;
    TileMapManager tileMapManager;

    LineRenderer lineRenderer;

    public Vector2Int startPoint;
    public Vector2Int endPoint;

    bool hasPath;

    List<Vector2Int> path = new List<Vector2Int>();

    Queue<Vector2Int> queue = new Queue<Vector2Int>();
    HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
    Dictionary<Vector2Int, Vector2Int> pathGraph = new Dictionary<Vector2Int, Vector2Int>();

    void Start()
    {
        gameController = GameController.GetInstance();
        tileMapManager = TileMapManager.GetInstance();

        lineRenderer = GetComponent<LineRenderer>();
    }

    public List<Vector2Int> GetPath()
    {
        return new List<Vector2Int>(path);
    }

    public bool HasPath()
    {
        return hasPath;
    }

    public void RecalculatePath()
    {
        Debug.Log("Recalculating shortest path");

        SetupEnvironment();

        EnqueueNeighbors(startPoint);

        while (queue.Count > 0)
        {
            Vector2Int position = queue.Dequeue();

            if (position == endPoint)
            {
                BuildPath();
                break; // ?
            }
            
            Node node = tileMapManager.GetNode(position);

            if(node.canBuild)
            {
                EnqueueNeighbors(position);
            }
        }

        DrawPath();
    }

    void EnqueueNeighbors(Vector2Int position)
    {
        EnqueueNode(position + new Vector2Int( 1,  0), position);
        EnqueueNode(position + new Vector2Int( 0,  1), position);
        EnqueueNode(position + new Vector2Int(-1,  0), position);
        EnqueueNode(position + new Vector2Int( 0, -1), position);
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

        while(node != startPoint)
        {
            node = pathGraph[node];
            path.Add(node);
        }

        Debug.Log("Found a valid path!");
        hasPath = true;

        path.Reverse();
    }

    void DrawPath()
    {
        if (path.Count == 0)
        {
            return;
        }

        List<Vector3> linePath = new List<Vector3>();

        foreach (var node in path)
        {
            linePath.Add(
                new Vector3(node.x - 1.5f, node.y)
            );
        }

        lineRenderer.positionCount = path.Count;
        lineRenderer.SetPositions(linePath.ToArray());
    }

    void SetupEnvironment()
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

        if (tileMapManager ==  null)
        {
            tileMapManager = TileMapManager.GetInstance();
        }

        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }
    }

    public void Reset()
    {
        SetupEnvironment();
        lineRenderer.positionCount = 0;
    }
}
