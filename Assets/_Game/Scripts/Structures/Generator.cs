using FishNet.Object;
using UnityEngine;

public class Generator : NetworkBehaviour, IStructure
{
    public int Health = 150;
    public Tier tier = Tier.Crude;

    public Currency currency;
    public int income;
    public float incomeInterval;

    private float incomeTimer = 0;
    public PopupUI popupUI;

    PlayerInventory inventory;

    public int GetHealth() => Health;

    public Tier GetTier() => tier;

    private void Awake()
    {
        popupUI = Instantiate(popupUI);
    }

    public void Init(GameObject player)
    {
        if (player != null) inventory = player.GetComponentInChildren<PlayerInventory>();
    }

    private void Update()
    {
        if (!IsOwner) return;

        if (incomeTimer > incomeInterval)
        {
            RpcGenerate();
            incomeTimer = 0;
        }

        incomeTimer += Time.deltaTime;
    }

    [ServerRpc]
    private void RpcGenerate()
    {
        if (inventory != null) inventory.AddBalance(currency, income);
        RpcShowPopup();
    }

    [ObserversRpc]
    private void RpcShowPopup()
    {
        popupUI.ShowPopup(transform.position, "+ " + income, (Color.white + tier.ToColor()) / 2, currency.icon, currency.color);
    }

    private void Start()
    {
        Material[] mats = GetComponentInChildren<Renderer>().materials;
        mats[1].color = tier.ToColor();

        GetComponentInChildren<Renderer>().materials = mats;
    }
}
