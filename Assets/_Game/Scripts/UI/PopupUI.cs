using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupUI : MonoBehaviour
{
    private Image popupImage;
    private TextMeshProUGUI popupText;
    private CanvasGroup cg;

    public AnimationCurve fadeCurve;
    public AnimationCurve moveCurve;

    private Vector3 startPos;
    private float timePassed;
    private float endTime;

    private void Awake()
    {
        popupImage = GetComponentInChildren<Image>();
        popupText = GetComponentInChildren<TextMeshProUGUI>();
        cg = GetComponentInChildren<CanvasGroup>();
        cg.alpha = 0;
    }

    public void ShowPopup(Vector3 position, string text, Color textColor, Sprite icon, Color iconColor)
    {
        popupImage.sprite = icon;
        popupImage.color = iconColor;

        popupText.color = textColor;
        popupText.text = text;

        transform.position = position + Vector3.up;

        timePassed = 0;
        startPos = transform.position;
        if (fadeCurve.length == 0 || moveCurve.length == 0)
        {
            Debug.Log("curve has no length");
            return;
        }
        endTime = Mathf.Max(fadeCurve[fadeCurve.length-1].time, moveCurve[moveCurve.length - 1].time);
    }

    // Update is called once per frame
    void Update()
    {
        if (timePassed > endTime) return;

        Vector3 lookDir = (transform.position - Camera.main.transform.position).normalized;
        transform.LookAt(transform.position + lookDir);

        cg.alpha = fadeCurve.Evaluate(timePassed);
        transform.position = startPos + (Vector3.up * moveCurve.Evaluate(timePassed));

        timePassed += Time.deltaTime;
    }
}
