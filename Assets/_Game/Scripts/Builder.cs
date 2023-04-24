using FishNet.Object;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Builder : NetworkBehaviour
{
    [SerializeField] private BuilderUI builderUI;
    [SerializeField] private List<BuildOption> options;

    private List<InputAction> hotkeys;
    //private BuildOption selectedOption = null;

    private Tile currentTile;
    private TileInfo currentTileInfo;
    private Tile lastTile;

    private List<TileInfo> requiredTiles;
    private Controls controls;

    private PlayerInventory inventory;

    [SerializeField] private List<BuildCategory> buildCategories;

    private Dictionary<InputAction, int> HotkeyToIndex;

    private Vector2Int selectedOption = new Vector2Int(-1, -1);
    private int menuDepth;

    private void Awake()
    {
        inventory = GetComponent<PlayerInventory>();

        options = options.Select(option => Instantiate(option)).ToList(); // make copies so we don't alter the ScriptableObject assets

        //builderUI = Instantiate(builderUI);
        //builderUI.Init(options);
    }

    private BuildOption GetSelectedOption()
    {
        return buildCategories[selectedOption.x].options[selectedOption.y];
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!IsOwner) return;

        controls = new Controls();
        controls.Gameplay.Enable();

        hotkeys = new List<InputAction>() { controls.Gameplay.Hotbar1, controls.Gameplay.Hotbar2, controls.Gameplay.Hotbar3, controls.Gameplay.Hotbar4, controls.Gameplay.Hotbar5, controls.Gameplay.Hotbar6, controls.Gameplay.Hotbar7, controls.Gameplay.Hotbar8, controls.Gameplay.Hotbar9 };
        hotkeys.ForEach(x => x.performed += OnHotbarButton);

        HotkeyToIndex = new Dictionary<InputAction, int>
        {
            { controls.Gameplay.Hotbar1, 0},
            { controls.Gameplay.Hotbar2, 1},
            { controls.Gameplay.Hotbar3, 2},
            { controls.Gameplay.Hotbar4, 3},
            { controls.Gameplay.Hotbar5, 4},
            { controls.Gameplay.Hotbar6, 5},
            { controls.Gameplay.Hotbar7, 6},
            { controls.Gameplay.Hotbar8, 7},
            { controls.Gameplay.Hotbar9, 8},
        };

        controls.Gameplay.Interact.performed += OnInteract;
        controls.Gameplay.Back.performed += OnBack;

        for (int i = 0; i < options.Count; i++) options[i].binding = hotkeys[i]; // assign hotkeys to our options

        ViewManager.Instance.ShowAdditive<HotbarView>(buildCategories);
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
        switch (menuDepth)
        {
            case 0:
                selectedOption.x = -1;
                break;
            case 1:
                menuDepth--;
                selectedOption.y = -1; 
                ViewManager.Instance.Hide<BuildCategoryView>();
                break;
        }

        ViewManager.Instance.Hide<BuildPreviewView>();
    }

    private void OnInteract(InputAction.CallbackContext obj)
    {
        if (currentTile == null) return;
        if (selectedOption.x < 0 && selectedOption.y < 0) return;
        ServerRequestBuild(currentTileInfo.gridPosition, selectedOption);
        //ServerRequestBuild(currentTileInfo.gridPosition, options.IndexOf(selectedOption));
    }

    private void OnHotbarButton(InputAction.CallbackContext obj)
    {
        switch (menuDepth)
        {
            case 0:
                menuDepth++;
                selectedOption.x = HotkeyToIndex[obj.action];
                ViewManager.Instance.ShowAdditive<BuildCategoryView>(buildCategories[selectedOption.x]);
                break;
            case 1:
                selectedOption.y = HotkeyToIndex[obj.action];
                break;
        }
        //BuildOption option = options.FirstOrDefault(option => option.binding == obj.action);
        //if (option != null) SelectOption(option);
    }

    void Update()
    {
        if (!IsOwner) return;

        currentTile = GetCursorTile();

        if (currentTile == null) return;
        LevelBuilder.Instance.TryGetTile(GetCursorTile().GridPos, out currentTileInfo);

        if (selectedOption.x < 0 || selectedOption.y < 0) return; // no option selected
        BuildOption option = buildCategories[selectedOption.x].options[selectedOption.y];

        if (currentTile != lastTile)
        {
            int currencyIdx;
            CurrencyManager.Instance.TryGetCurrencyIdx(option.currency, out currencyIdx);

            PreviewState state = PreviewState.Invalid;
            if (LevelBuilder.Instance.TryGetTilesInRectangle(currentTileInfo.gridPosition, option.Size, out requiredTiles)
                && !requiredTiles.Any(info => !info.IsAvailable)
                && inventory.CanAfford(currencyIdx, option.cost))
            {
                state = PreviewState.Valid;
            }
            else
            {
                state = PreviewState.Invalid;
            }

            ViewManager.Instance.ShowAdditive<BuildPreviewView>(new PreviewInfo(option, currentTileInfo.GetWorldPosition(), state));
        }

        lastTile = currentTile;
    }

    [ServerRpc]
    private void ServerRequestBuild(Vector2Int targetPos, Vector2Int targetOptionIndex)
    {
        BuildOption targetOption = buildCategories[targetOptionIndex.x].options[targetOptionIndex.y]; //options[targetOptionIndex];

        if (!LevelBuilder.Instance.TryGetTile(targetPos, out TileInfo targetTile)) return;
        if (!LevelBuilder.Instance.TryGetTilesInRectangle(targetTile.gridPosition, targetOption.Size, out List<TileInfo> requiredTiles)) return;
        if (requiredTiles.Any(info => !info.IsAvailable)) return;
        if (!inventory.TryRemoveBalance(targetOption.currency, targetOption.cost)) return;

        GameObject structuregameObject = Instantiate(targetOption.structurePrefab.gameObject, targetTile.GetWorldPosition(), Quaternion.identity);
        base.Spawn(structuregameObject, base.Owner);
        IStructure structure = structuregameObject.GetComponent(typeof(IStructure)) as IStructure;
        structure.Init(gameObject);

        requiredTiles.ForEach(tile => tile.occupied = true);

        ViewManager.Instance.Hide<BuildPreviewView>();
    }

    //private void DeselectOption()
    //{
    //    ViewManager.Instance.Hide<BuildPreviewView>();
    //    selectedOption = null;
    //}

    //private void SelectOption(BuildOption option)
    //{
    //    selectedOption = option;
    //}

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
