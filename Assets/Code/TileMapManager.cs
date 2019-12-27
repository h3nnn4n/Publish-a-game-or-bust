using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMapManager : MonoBehaviour
{
    private static TileMapManager instance;

    public GameObject nodePrefab;

    Dictionary<(int, int), GameObject> grid;
    GameController gameController;
    BoundsInt bounds;
    Vector3Int size;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else
        {
            Destroy(this);
            Debug.LogError("Tried to instantiate more than one TileMapManager!");
        }
    }

    void Start()
    {
        gameController = GameController.GetInstance();
        var tilemap = gameController.GetTileMap();
        bounds = tilemap.cellBounds;
        size = tilemap.size;

        Debug.Log(tilemap);

        InitGrid();
    }

    void InitGrid()
    {
        grid = new Dictionary<(int, int), GameObject>();

        for (int x = bounds.xMin; x <= bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y <= bounds.yMax; y++)
            {
                var pos = (x, y);
                var node = Instantiate(
                    nodePrefab,
                    GetNodeContainer().transform.position,
                    Quaternion.identity,
                    GetNodeContainer().transform
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
        return transform.Find("Nodes").gameObject;
    }

    public bool CanBuild(Vector3Int pos)
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
        return grid[(pos.x, pos.y)].GetComponent<Node>();
    }

    public Node GetNode(Vector2Int pos)
    {
        if (grid.ContainsKey((pos.x, pos.y))) {
            return grid[(pos.x, pos.y)].GetComponent<Node>();
        }

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
