using FishNet.Object;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Builder : NetworkBehaviour
{
    public BuilderUI builderUI;
    public List<BuildOption> options;

    private List<InputAction> hotkeys;
    private BuildOption selectedOption = null;

    private Tile currentTile;
    private TileInfo currentTileInfo;
    private Tile lastTile;

    private List<TileInfo> requiredTiles;
    private Controls controls;

    private PlayerInventory inventory;

    private void Awake()
    {
        inventory = GetComponent<PlayerInventory>();

        options = options.Select(option => Instantiate(option)).ToList(); // make copies so we don't alter the ScriptableObject assets

        builderUI = Instantiate(builderUI);
        builderUI.Init(options);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!IsOwner) return;
            
        controls = new Controls();
        controls.Gameplay.Enable();

        hotkeys = new List<InputAction>() { controls.Gameplay.Hotbar1, controls.Gameplay.Hotbar2, controls.Gameplay.Hotbar3, controls.Gameplay.Hotbar4, controls.Gameplay.Hotbar5, controls.Gameplay.Hotbar6, controls.Gameplay.Hotbar7, controls.Gameplay.Hotbar8, controls.Gameplay.Hotbar9 };
        hotkeys.ForEach(x => x.performed += OnHotbarButton);

        controls.Gameplay.Interact.performed += OnInteract;
        controls.Gameplay.Back.performed += OnBack;

        for (int i = 0; i < options.Count; i++) options[i].binding = hotkeys[i]; // assign hotkeys to our options
    }

    private void OnEnable()
    {
        if (IsOwner) controls.Gameplay.Enable();
    }

    private void OnDisable()
    {
        if (IsOwner) controls.Gameplay.Disable();
    }

    private void OnBack(InputAction.CallbackContext obj)
    {
        DeselectOption();
    }

    private void OnInteract(InputAction.CallbackContext obj)
    {
        if (currentTile == null) return;
        if (selectedOption == null) return;
        ServerRequestBuild(currentTileInfo.gridPosition, options.IndexOf(selectedOption));
    }

    private void OnHotbarButton(InputAction.CallbackContext obj)
    {
        BuildOption option = options.FirstOrDefault(option => option.binding == obj.action);
        if (option != null) SelectOption(option);
    }

    void Update()
    {
        if (!IsOwner) return;

        currentTile = GetCursorTile();

        if (currentTile == null) return;
        LevelBuilder.Instance.TryGetTile(GetCursorTile().GridPos, out currentTileInfo);
        
        if (selectedOption == null) return; // no option selected

        if (currentTile != lastTile)
        {
            int currencyIdx;
            CurrencyManager.Instance.TryGetCurrencyIdx(selectedOption.currency, out currencyIdx);

            PreviewState state = PreviewState.Invalid;
            if (LevelBuilder.Instance.TryGetTilesInRectangle(currentTileInfo.gridPosition, selectedOption.Size, out requiredTiles)
                && !requiredTiles.Any(info => !info.IsAvailable)
                && inventory.CanAfford(currencyIdx, selectedOption.cost))
            {
                state = PreviewState.Valid;
            }
            else
            {
                state = PreviewState.Invalid;
            }

            ViewManager.Instance.ShowAdditive<BuildPreviewView>(new PreviewInfo(selectedOption, currentTileInfo.GetWorldPosition(), state));
        }

        lastTile = currentTile;
    }

    [ServerRpc]
    private void ServerRequestBuild(Vector2Int targetPos, int targetOptionIndex)
    {
        BuildOption targetOption = options[targetOptionIndex];

        if (!LevelBuilder.Instance.TryGetTile(targetPos, out TileInfo targetTile)) return;
        if (!LevelBuilder.Instance.TryGetTilesInRectangle(targetTile.gridPosition, targetOption.Size, out List<TileInfo> requiredTiles)) return;
        if (requiredTiles.Any(info => !info.IsAvailable)) return;
        if (!inventory.TryRemoveBalance(targetOption.currency, targetOption.cost)) return;
        
        GameObject structuregameObject = Instantiate(targetOption.structurePrefab.gameObject, targetTile.GetWorldPosition(), Quaternion.identity);
        base.Spawn(structuregameObject, base.Owner);
        IStructure structure = structuregameObject.GetComponent(typeof(IStructure)) as IStructure;
        structure.Init(gameObject);

        requiredTiles.ForEach(tile => tile.occupied = true);

        DeselectOption();
    }

    private void DeselectOption()
    {
        ViewManager.Instance.Hide<BuildPreviewView>();
        selectedOption = null;
    }

    private void SelectOption(BuildOption option)
    {
        selectedOption = option;
    }

    private Tile GetCursorTile()
    {
        Tile tile = null;
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit, LayerMask.GetMask("Ground")))
        {
            if (hit.transform.parent == null) return null;
            tile = hit.transform.parent.GetComponent<Tile>();
        }
        return tile;
    }
}
