using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapManager : MonoBehaviour
{
    private static TileMapManager instance;

    GameController gameController;
    Grid tilemapGrid;

    public GameObject nodePrefab;

    Dictionary<(int, int), GameObject> grid;
    BoundsInt bounds;
    Vector3Int size;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
            Debug.LogError("Tried to instantiate more than one TileMapManager!");
        }
    }

    public void LoadLevel()
    {
        gameController = GameController.GetInstance();

        tilemapGrid = gameController.GetGrid();

        var tilemap = gameController.GetTileMap();
        bounds = tilemap.cellBounds;
        size = tilemap.size;

        //Debug.Log(tilemap);

        InitGrid();
    }

    void InitGrid()
    {
        grid = new Dictionary<(int, int), GameObject>();
        GameObject nodeContainer = GetNodeContainer();

        for (int x = bounds.xMin; x <= bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y <= bounds.yMax; y++)
            {
                var pos = (x, y);
                var node = Instantiate(
                    nodePrefab,
                    nodeContainer.transform.position,
                    Quaternion.identity,
                    nodeContainer.transform
                );

                grid.Add(
                    pos,
                    node
                );
            }
        }
    }

    GameObject GetNodeContainer()
    {
        return GameObject.Find("Nodes").gameObject;
    }

    public Vector3Int GetGridCelPosition(Vector3 position)
    {
        Vector3Int coordinate = tilemapGrid.WorldToCell(position);

        return coordinate;
    }

    public bool CanBuildOnNode(Vector3Int pos)
    {
        try
        {
            return grid[(pos.x, pos.y)].GetComponent<Node>().canBuild;
        }
        catch
        {
            return false;
        }
    }

    public Node GetNode(Vector3Int pos)
    {
        if (grid.ContainsKey((pos.x, pos.y)))
        {
            return grid[(pos.x, pos.y)].GetComponent<Node>();
        }

        Debug.LogWarningFormat("Node at {0}, {1} not found!", pos.x, pos.y);

        return null;
    }

    public Node GetNode(Vector2Int pos)
    {
        if (grid.ContainsKey((pos.x, pos.y))) {
            return grid[(pos.x, pos.y)].GetComponent<Node>();
        }

        Debug.LogWarningFormat("Node at {0}, {1} not found!", pos.x, pos.y);

        return null;
    }

    public Node FindByType(string type)
    {
        foreach (GameObject nodeGameObject in grid.Values)
        {
            Node node = nodeGameObject.GetComponent<Node>();

            if (node.type == type)
            {
                return node;
            }
        }

        return null;
    }

    public static TileMapManager GetInstance()
    {
        return instance;
    }
}
