using System;
using UnityEditor;
using UnityEngine;

[Serializable]
public struct TileInfo
{
    public bool buildable;
    public bool occupied;
    public Vector2Int gridPosition;
    public bool Occupied => occupied;
    public bool IsAvailable => buildable && !occupied;

    internal Vector3 GetWorldPosition()
    {
        return new Vector3(gridPosition.x, 0, gridPosition.y);
    }
}

public class Tile : MonoBehaviour
{
    public TileInfo tileInfo = new TileInfo();

    private void Start()
    {
        Renderer renderer = GetComponentInChildren<Renderer>();
        renderer.material = tileInfo.buildable ? LevelBuilder.Instance.baseMaterial : LevelBuilder.Instance.corridorMaterial;
    }

    public Vector2Int GridPos => new(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z));

    //public bool buildable = true;
    //public bool occupied = false;
    //public Vector2Int gridPosition;

    //public bool Occupied => occupied;
    //public bool IsAvailable => buildable && !occupied;
}
