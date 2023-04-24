using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

internal class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    [SerializeField]
    private List<Currency> currencies = new List<Currency>();

    private void Awake()
    {
        Instance = this;
    }

    public Currency[] Getcurrencies()
    {
        return currencies.ToArray();
    }

    public Currency GetCurrency(int index)
    {
        return currencies[index];
    }

    public Currency GetCurrency(string name)
    {
        return currencies.FirstOrDefault(c => c.name == name);
    }

    public bool TryGetCurrencyIdx(Currency currency, out int idx)
    {
        idx = -1;
        for (int i = 0; i < currencies.Count; i++)
        {
            if (currencies[i] == currency)
            {
                idx = i;
                return true;
            }
        }
        Debug.Log(currency.name + " not in CurrencyManager");
        return false;
    }
}
