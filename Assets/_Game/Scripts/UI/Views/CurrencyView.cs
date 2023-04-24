using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class CurrencyView : View
{
    [SerializeField]
    private RectTransform container;

    [SerializeField]
    private CurrencyElement currencyElementPrefab;

    private List<CurrencyElement> currencyElements = new List<CurrencyElement>();

    public override void Initialize()
    {
        Currency[] currencies = CurrencyManager.Instance.Getcurrencies();
        for (int i = 0; i < currencies.Length; i++)
        {
            AddCurrencyElement(new CurrencyInfo(currencies[i], 0));
        }

        base.Initialize();
    }

    private void AddCurrencyElement(CurrencyInfo info)
    {
        CurrencyElement element = Instantiate(currencyElementPrefab, container);
        currencyElements.Add(element);
        element.SetCurrency(info);
    }

    public override void Show(object args = null)
    {
        if (args is CurrencyInfo info)
        {
            currencyElements[info.CurrencyIdx].SetCurrency(info);
        }
        else if (args is List<CurrencyInfo>)
        {
            List<CurrencyInfo> currencies = (List<CurrencyInfo>)args;
            Debug.Log("show " + currencies.Count + " currencies");

            for (int i = 0; i < currencies.Count; i++)
            {
                if (currencyElements.Count <= i) currencyElements.Add(Instantiate(currencyElementPrefab, container));
                CurrencyElement currentElement = currencyElements[i];
                currentElement.SetCurrency(currencies[i]);
            }
        }

        base.Show(args);
    }
}
