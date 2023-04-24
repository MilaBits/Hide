using FishNet.Object.Synchronizing;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level Data" ,menuName = "HideGame/New Level Data")]
public class LevelData : ScriptableObject
{
    [Header("serialized")]
    public Texture2D floorMap;

    public Tile floorObject;

    [Header("run-time")]
    [HideInInspector]
    public Vector2Int gridSize = Vector2Int.zero;

    public float[,] floorData;

    private Tile[,] tileGrid;

    public void Init()
    {
        gridSize = new Vector2Int(floorMap.width, floorMap.height);
        tileGrid = new Tile[gridSize.x, gridSize.y];

        floorData = new float[gridSize.x, gridSize.y];
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                floorData[x, y] = floorMap.GetPixel(x, y).grayscale;
            }
        }
    }

    //internal void AddTile(Tile tile, int x, int y)
    //{
    //    tileGrid[x, y] = tile;
    //}

    //public bool TryGetTile(Vector2Int pos, out Tile tile)
    //{
    //    tile = null;

    //    if (pos.x >= gridSize.x || pos.y >= gridSize.y
    //     || pos.x < 0 || pos.y < 0) return false;

    //    tile = tileGrid[pos.x, pos.y];

    //    return true;
    //}

    //public bool TryGetTilesInRectangle(Vector2Int origin, Vector2Int size, out List<Tile> tiles)
    //{
    //    tiles = new List<Tile>();
    //    for (int x = 0; x < size.x; x++)
    //    {
    //        for (int y = 0; y < size.y; y++)
    //        {
    //            Vector2Int pos = new Vector2Int(origin.x + x, origin.y + y);
    //            if (pos.x >= gridSize.x || pos.y >= gridSize.y
    //             || pos.x < 0           || pos.y < 0) return false;

    //            tiles.Add(tileGrid[pos.x, pos.y]);
    //        }
    //    }

    //    return true;
    //}
}
