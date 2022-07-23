using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class ToolTip : MonoBehaviour
{
    private static ToolTip instance;
    private TextMeshProUGUI toolTipText;
    private RectTransform backgroundRectTransform;
    private readonly float textPaddingSize = 4f;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        backgroundRectTransform = transform.Find("background").GetComponent<RectTransform>();
        toolTipText = transform.Find("text").GetComponent<TextMeshProUGUI>();
        HideToolTip();
    }

    private void Update()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), Mouse.current.position.ReadValue(), null, out Vector2 localPoint);
        transform.localPosition = localPoint;
    }

    void ShowToolTip_NonStatic(string newText)
    {
        gameObject.SetActive(true);
        toolTipText.text = newText;
        Vector2 newBackgroundSize = new(toolTipText.preferredWidth + textPaddingSize * 2f, toolTipText.preferredHeight + textPaddingSize * 2f);
        backgroundRectTransform.sizeDelta = newBackgroundSize;
    }

    void UpdateToolTip_NonStatic(string newText)
    {
        toolTipText.text = newText;
        Vector2 newBackgroundSize = new(toolTipText.preferredWidth + textPaddingSize * 2f, toolTipText.preferredHeight + textPaddingSize * 2f);
        backgroundRectTransform.sizeDelta = newBackgroundSize;
    }

    void HideToolTip_NonStatic()
    {
        gameObject.SetActive(false);
    }

    public static void UpdateToolTip(string newText)
    {
        instance.UpdateToolTip_NonStatic(newText);
    }

    public static void ShowToolTip(string newText)
    {
        instance.ShowToolTip_NonStatic(newText);
    }

    public static void HideToolTip()
    {
        instance.HideToolTip_NonStatic();
    }
}