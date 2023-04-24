using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyElement : MonoBehaviour
{
    private Image Icon;
    private TextMeshProUGUI Text;

    //private CurrencyInfo currency;

    private void Awake()
    {
        Icon = GetComponentInChildren<Image>();
        Text = GetComponentInChildren<TextMeshProUGUI>();
    }

    //internal void Init(Currency currency)
    //{
    //    Icon.sprite = currency.icon;
    //    Icon.color = currency.color;
    //    Text.text = "0";
    //}

    //internal void SetAmount(int amount) => Text.text = amount.ToString();

    internal void SetCurrency(CurrencyInfo info)
    {
        Currency currency = CurrencyManager.Instance.GetCurrency(info.CurrencyIdx);
        //Currency curr = CurrencyManager.Instance.GetCurrency(currencyInfo.Name);

        //if (currencyInfo.Name != currency.Name)
        //{
        Icon.sprite = currency.icon;
        Icon.color = currency.color;
        //}

        Text.text = info.Balance.ToString();
    }
}
