using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : NetworkBehaviour
{
    public static LevelBuilder Instance { get; private set; }

    public LevelData levelData;

    public Material baseMaterial;
    public Material corridorMaterial;

    public Vector2Int gridSize;

    [SyncObject]
    private readonly SyncDictionary<Vector2Int, TileInfo> tileGrid = new SyncDictionary<Vector2Int, TileInfo>();

    private void OnChangeTileGrid(SyncDictionaryOperation op, Vector2Int key, TileInfo value, bool asServer)
    {
        switch (op)
        {
            case SyncDictionaryOperation.Add:
                break;
            case SyncDictionaryOperation.Clear:
                break;
            case SyncDictionaryOperation.Remove:
                break;
            case SyncDictionaryOperation.Set:
                break;
            case SyncDictionaryOperation.Complete:
                break;
        }
    }

    internal void AddTile(Vector2Int position, TileInfo tile)
    {
        tileGrid[position] = tile;
        //tileGrid[x, y] = tile;
    }

    public bool TryGetTile(Vector2Int position, out TileInfo info)
    {
        info = new TileInfo();

        if (tileGrid.TryGetValue(position, out info))
        {
            return true;
        }

        return false;

        //if (position.x >= gridSize.x || position.y >= gridSize.y
        // || position.x < 0 || position.y < 0) return false;
        //tile = tileGrid[position];
        //return true;
    }

    public bool TryGetTilesInRectangle(Vector2Int origin, Vector2Int size, out List<TileInfo> tiles)
    {
        tiles = new List<TileInfo>();
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                Vector2Int pos = new Vector2Int(origin.x + x, origin.y + y);
                if (TryGetTile(pos, out TileInfo tile))
                {
                    tiles.Add(tile);
                    continue;
                }
                return false;

                //if (pos.x >= gridSize.x || pos.y >= gridSize.y
                // || pos.x < 0 || pos.y < 0) return false;
                //tiles.Add(tileGrid[pos.x, pos.y]);
            }
        }

        return true;
    }

    private void Build()
    {
        levelData.Init();

        for (int x = 0; x < levelData.gridSize.x; x++)
        {
            for (int y = 0; y < levelData.gridSize.y; y++)
            {
                float height = levelData.floorData[x, y];
                if (height >= .25f) continue;

                Tile tile = levelData.floorObject;

                tile = Instantiate(levelData.floorObject, new Vector3(x, 0, y), Quaternion.identity);
                base.Spawn(tile.gameObject, base.Owner);
                tile.tileInfo.gridPosition = new Vector2Int(x, y);
                

                if (height == 0)
                {
                    tile.tileInfo.buildable = false;
                }
                else if (height < .25f)
                {
                    tile.tileInfo.buildable = true;
                }

                AddTile(tile.tileInfo.gridPosition, tile.tileInfo);
            }
        }
    }

    public override void OnStartNetwork()
    {
        base.OnStartNetwork();

        tileGrid.OnChange += OnChangeTileGrid;

        Instance = this;
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        Build();
    }
}
