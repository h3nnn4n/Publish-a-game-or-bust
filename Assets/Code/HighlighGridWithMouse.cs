using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HighlighGridWithMouse : MonoBehaviour
{
    Grid grid;
    Camera mainCamera;
    Tilemap tileMap;
    public Tile highlightedTile;

    Tile lastTile;

    Vector3Int lastCoordinate;
    Vector3Int currentCoordinate;
    bool coordinateChanged;

    void Start()
    {
        grid = GetGrid();
        mainCamera = Camera.main;
        tileMap = GetTileMap();
    }

    void Update()
    {
        UpdateCoordinates();
        HighlightCurrentTile();
    }

    void HighlightCurrentTile()
    {
        if (coordinateChanged) {
            if (lastTile != null)
            {
                tileMap.SetTile(lastCoordinate, lastTile);
            }

            lastTile = tileMap.GetTile<Tile>(currentCoordinate);

            tileMap.SetTile(currentCoordinate, highlightedTile);
        }
    }

    void UpdateCoordinates()
    {
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int coordinate = grid.WorldToCell(mouseWorldPos);
        //transform.localPosition = tilemap.GetCellCenterLocal(coordinate);

        if (coordinate != currentCoordinate)
        {
            lastCoordinate = currentCoordinate;
            currentCoordinate = coordinate;
            coordinateChanged = true;
        }
        else
        {
            coordinateChanged = false;
        }

    }

    Tilemap GetTileMap()
    {
        GameObject tileMapObject = GameObject.FindGameObjectWithTag("Tilemap");

        return tileMapObject.GetComponent<Tilemap>();
    }

    Grid GetGrid()
    {
        GameObject gridObject = GameObject.FindGameObjectWithTag("Grid");

        return gridObject.GetComponent<Grid>();
    }
}
