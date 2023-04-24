using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyElement : MonoBehaviour
{
    private Image Icon;
    private TextMeshProUGUI Text;

    private void Awake()
    {
        Icon = GetComponentInChildren<Image>();
        Text = GetComponentInChildren<TextMeshProUGUI>();
    }

    internal void SetCurrency(CurrencyInfo info)
    {
        Currency currency = CurrencyManager.Instance.GetCurrency(info.CurrencyIdx);

        Icon.sprite = currency.icon;
        Icon.color = currency.color;

        Text.text = info.Balance.ToString();
    }
}
