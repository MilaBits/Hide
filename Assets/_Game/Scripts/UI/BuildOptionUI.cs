using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildOptionUI : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI keyText;
    public TextMeshProUGUI costText;

    public void Init(BuildOption bo)
    {
        image.type = Image.Type.Filled;
        image.sprite = bo.icon;
        image.color = bo.tier.ToColor();

        keyText.text = (transform.GetSiblingIndex() + 1).ToString();
        costText.text = bo.cost.ToString();
    }
}
