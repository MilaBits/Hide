using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyUI : MonoBehaviour
{
    private GridLayoutGroup container;
    public CurrencyElement currencyElement;
    private Dictionary<Currency, CurrencyElement> currencies = new Dictionary<Currency, CurrencyElement>();

    private void Awake()
    {
        container = GetComponentInChildren<GridLayoutGroup>();
    }

    internal void Init(List<Currency> initCurrencies)
    {
        initCurrencies.ForEach(currency =>
        {
            CurrencyElement currencyUI = Instantiate(currencyElement, container.transform);
            //currencyUI.Init(currency);
            currencies.Add(currency, currencyUI);
        });
    }

    public void OnCurrencyChanged(Currency currency, int amount)
    {
        //currencies[currency].SetAmount(amount);
    }
}
