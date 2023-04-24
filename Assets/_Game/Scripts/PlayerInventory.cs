using FishNet.Object;
using FishNet.Object.Synchronizing;

public class PlayerInventory : NetworkBehaviour
{
    [SyncObject]
    private readonly SyncDictionary<int, int> currencies = new SyncDictionary<int, int>();

    private void Awake()
    {
        currencies.OnChange += OnChangeCurrencies;
    }

    private void OnChangeCurrencies(SyncDictionaryOperation op, int key, int value, bool asServer)
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
                if (IsOwner) ViewManager.Instance.ShowAdditive<CurrencyView>(new CurrencyInfo(key, value));
                break;
            case SyncDictionaryOperation.Complete:
                break;
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!IsOwner) return;

        Currency[] managedCurrencies = CurrencyManager.Instance.Getcurrencies();
        for (int i = 0; i < managedCurrencies.Length; i++)
        {
            AddBallance(i, 0);
        }
        ViewManager.Instance.ShowAdditive<CurrencyView>(currencies.Values);
    }

    public void AddBallance(int currency, int amount)
    {
        if (!currencies.ContainsKey(currency))
        {
            RpcAddCurrency(currency, amount);
            //currencies.Add(currency, amount);
        }
        else
        {
            currencies[currency] += amount;
        }
    }

    [ServerRpc]
    public void RpcAddCurrency(int currency, int amount)
    {
        currencies.Add(currency, amount);
    }

    public void AddBalance(Currency currency, int amount)
    {
        if (!CurrencyManager.Instance.TryGetCurrencyIdx(currency, out int currencyIdx)) return;
        AddBallance(currencyIdx, amount);
    }

    public bool CanAfford(int currencyIdx, int amount)
    {
        if (currencies.ContainsKey(currencyIdx)) return currencies[currencyIdx] - amount >= 0;
        return false;
    }

    public bool CanAfford(Currency currency, int amount)
    {
        if (!CurrencyManager.Instance.TryGetCurrencyIdx(currency, out int currencyIdx)) return false;
        return CanAfford(currencyIdx, amount);
    }

    public bool TryRemoveBalance(int currencyIdx, int amount)
    {
        if (currencies.ContainsKey(currencyIdx) && currencies[currencyIdx] - amount >= 0)
        {
            currencies[currencyIdx] -= amount;
            return true;
        }
        return false;
    }

    public bool TryRemoveBalance(Currency currency, int amount)
    {
        if (!CurrencyManager.Instance.TryGetCurrencyIdx(currency, out int currencyIdx)) return false;
        return TryRemoveBalance(currencyIdx, amount);
    }
}