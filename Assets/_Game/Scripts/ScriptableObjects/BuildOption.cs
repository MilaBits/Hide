using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "New Build Option", menuName = "HideGame/New Build Option")]
public class BuildOption : ScriptableObject
{
    [Header("serialized")]
    public string Name;
    public GameObject structurePrefab;

    public Currency currency;
    public Sprite icon;
    public Tier tier;
    
    public Vector2Int Size = Vector2Int.one;
    public int cost = 15;

    [Header("run-time")]
    public InputAction binding;
}
