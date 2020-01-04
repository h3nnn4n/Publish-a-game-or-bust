using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    LineRenderer lineRenderer;

    public Vector2Int startPoint;
    public Vector2Int endPoint;

    bool hasPath;

    List<Vector2Int> path = new List<Vector2Int>();

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
        //Debug.Log("Recalculating shortest path");

        SetupEnvironment();

        var pathfinder = new PathfinderStandalone();
        pathfinder.startPoint = startPoint;
        pathfinder.endPoint = endPoint;
        pathfinder.CalculatePath();
        path = pathfinder.GetPath();
        hasPath = pathfinder.HasPath();

        DrawPath();
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
