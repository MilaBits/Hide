using UnityEngine;

[CreateAssetMenu(fileName = "New Currency", menuName = "HideGame/New Currency")]
public class Currency : ScriptableObject
{
    public static Currency Instance { get; private set; }

    public Sprite icon;
    public Color color;

    public CurrencyInfo CurrencyInfo => new CurrencyInfo(this, 0);

}

public struct CurrencyInfo {

    private int currencyIdx;
    private int balance;
    public int CurrencyIdx => currencyIdx;
    public int Balance => balance;

    public CurrencyInfo(Currency currency, int balance = 0)
    {
        CurrencyManager.Instance.TryGetCurrencyIdx(currency, out currencyIdx);
        this.balance = balance;
    }
    public CurrencyInfo(int currencyIdx, int balance = 0)
    {
        this.currencyIdx = currencyIdx;
        this.balance = balance;
    }

    //public Currency GetCurrency() => currency;

    public void AddBalance(int amount) => balance += amount;
    public bool TryRemoveBalance(int amount)
    {
        if (!CanAfford(amount)) return false;
        balance -= amount;
        return true;
    }

    public bool CanAfford(int amount) => balance - amount >= 0;
    //public int GetBalance() => balance;
}