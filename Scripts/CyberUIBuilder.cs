// Cyber theme UI generator
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CyberUIBuilder : MonoBehaviour
{
    [Header("Target UI Elements")]
    [Tooltip("List of Canvases or Panels you want to apply the Cyber Theme to")]
    public List<RectTransform> targetPanels = new List<RectTransform>();

    [Header("Theme Settings")]
    public Font cyberFont;
    public Color primaryBackgroundColor = new Color(0.04f, 0.07f, 0.16f, 0.95f); // Dark deep blue #0A1128
    public Color secondaryBackgroundColor = new Color(0.08f, 0.14f, 0.30f, 0.9f); // Lighter blue for inner panels
    public Color accentColor = new Color(0f, 0.93f, 1f, 1f); // Neon Cyan #00EDFF
    public Color textColor = Color.white;
    public Color headerTextColor = new Color(0f, 0.93f, 1f, 1f); // Neon Cyan

    [Header("Optional Sprites")]
    public Sprite panelBackgroundSprite; // A sliced sprite for borders
    public Sprite buttonBackgroundSprite;

    [ContextMenu("Apply Cyber Theme to Targets")]
    public void ApplyCyberTheme()
    {
        if (targetPanels == null || targetPanels.Count == 0)
        {
            Debug.LogError("CyberUIBuilder: Please assign at least one Target Panel or Canvas.");
            return;
        }

        if (cyberFont == null)
        {
            cyberFont = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        }

        int count = 0;
        foreach (var targetPanel in targetPanels)
        {
            if (targetPanel != null)
            {
                ApplyThemeRecursive(targetPanel);
                count++;
            }
        }
        
        Debug.Log($"Cyber Theme applied successfully to {count} panels.");
    }

    [ContextMenu("Generate New Cyber Panels")]
    public void GenerateNewCyberPanel()
    {
        if (targetPanels == null || targetPanels.Count == 0)
        {
            Debug.LogError("CyberUIBuilder: Please assign at least one Target Canvas to generate the panel under.");
            return;
        }

        int count = 0;
        foreach (var targetPanel in targetPanels)
        {
            if (targetPanel != null)
            {
                GeneratePanelForTarget(targetPanel);
                count++;
            }
        }

        Debug.Log($"Generated {count} new Cyber Panels.");
    }

    private void GeneratePanelForTarget(RectTransform targetParent)
    {
        // Create Main Panel
        GameObject newPanel = new GameObject("CyberPanel", typeof(RectTransform), typeof(Image));
        newPanel.transform.SetParent(targetParent, false);
        RectTransform rt = newPanel.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.1f, 0.1f);
        rt.anchorMax = new Vector2(0.9f, 0.9f);
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        Image img = newPanel.GetComponent<Image>();
        img.color = primaryBackgroundColor;
        if (panelBackgroundSprite != null)
        {
            img.sprite = panelBackgroundSprite;
            img.type = Image.Type.Sliced;
        }

        // Create Header Background
        GameObject header = new GameObject("Header", typeof(RectTransform), typeof(Image));
        header.transform.SetParent(newPanel.transform, false);
        RectTransform headerRt = header.GetComponent<RectTransform>();
        headerRt.anchorMin = new Vector2(0f, 1f);
        headerRt.anchorMax = new Vector2(1f, 1f);
        headerRt.offsetMin = new Vector2(0, -60);
        headerRt.offsetMax = new Vector2(0, 0);

        Image headerImg = header.GetComponent<Image>();
        headerImg.color = secondaryBackgroundColor;

        // Bottom Accent Line
        GameObject accentLine = new GameObject("AccentLine", typeof(RectTransform), typeof(Image));
        accentLine.transform.SetParent(header.transform, false);
        RectTransform accentRt = accentLine.GetComponent<RectTransform>();
        accentRt.anchorMin = new Vector2(0, 0);
        accentRt.anchorMax = new Vector2(1, 0);
        accentRt.offsetMin = new Vector2(0, 0);
        accentRt.offsetMax = new Vector2(0, 4); // 4px thick line
        
        Image accentImg = accentLine.GetComponent<Image>();
        accentImg.color = accentColor;

        // Header Title
        GameObject titleTextObj = new GameObject("TitleText", typeof(RectTransform), typeof(Text));
        titleTextObj.transform.SetParent(header.transform, false);
        RectTransform titleRt = titleTextObj.GetComponent<RectTransform>();
        titleRt.anchorMin = Vector2.zero;
        titleRt.anchorMax = Vector2.one;
        titleRt.offsetMin = new Vector2(20, 0);
        titleRt.offsetMax = new Vector2(-20, 0);

        Text titleText = titleTextObj.GetComponent<Text>();
        titleText.text = "CYBER ONE";
        titleText.font = cyberFont;
        titleText.fontSize = 28;
        titleText.fontStyle = FontStyle.Bold;
        titleText.color = headerTextColor;
        titleText.alignment = TextAnchor.MiddleLeft;

        // Content Area
        GameObject content = new GameObject("ContentArea", typeof(RectTransform), typeof(Image));
        content.transform.SetParent(newPanel.transform, false);
        RectTransform contentRt = content.GetComponent<RectTransform>();
        contentRt.anchorMin = new Vector2(0, 0);
        contentRt.anchorMax = new Vector2(1, 1);
        contentRt.offsetMin = new Vector2(20, 20);
        contentRt.offsetMax = new Vector2(-20, -80); // Leave room for header

        Image contentImg = content.GetComponent<Image>();
        contentImg.color = new Color(0, 0, 0, 0.5f); // Darker semi-transparent backing for content

        // Close Button
        GameObject closeBtnObj = new GameObject("CloseButton", typeof(RectTransform), typeof(Image), typeof(Button));
        closeBtnObj.transform.SetParent(header.transform, false);
        RectTransform closeRt = closeBtnObj.GetComponent<RectTransform>();
        closeRt.anchorMin = new Vector2(1, 0.5f);
        closeRt.anchorMax = new Vector2(1, 0.5f);
        closeRt.sizeDelta = new Vector2(40, 40);
        closeRt.anchoredPosition = new Vector2(-30, 0);

        Image closeImg = closeBtnObj.GetComponent<Image>();
        closeImg.color = secondaryBackgroundColor;
        if (buttonBackgroundSprite != null)
        {
            closeImg.sprite = buttonBackgroundSprite;
            closeImg.type = Image.Type.Sliced;
        }

        GameObject closeTextObj = new GameObject("Text", typeof(RectTransform), typeof(Text));
        closeTextObj.transform.SetParent(closeBtnObj.transform, false);
        RectTransform closeTextRt = closeTextObj.GetComponent<RectTransform>();
        closeTextRt.anchorMin = Vector2.zero;
        closeTextRt.anchorMax = Vector2.one;
        closeTextRt.offsetMin = Vector2.zero;
        closeTextRt.offsetMax = Vector2.zero;

        Text closeText = closeTextObj.GetComponent<Text>();
        closeText.text = "X";
        closeText.font = cyberFont;
        closeText.fontSize = 20;
        closeText.fontStyle = FontStyle.Bold;
        closeText.color = accentColor;
        closeText.alignment = TextAnchor.MiddleCenter;
    }

    private void ApplyThemeRecursive(Transform trans)
    {
        // Setup Panels
        Image img = trans.GetComponent<Image>();
        if (img != null)
        {
            if (trans.name.ToLower().Contains("panel") || trans.name.ToLower().Contains("background"))
            {
                img.color = primaryBackgroundColor;
                if (panelBackgroundSprite != null) img.sprite = panelBackgroundSprite;
            }
            else if (trans.name.ToLower().Contains("header"))
            {
                img.color = secondaryBackgroundColor;
            }
        }

        // Setup Buttons
        Button btn = trans.GetComponent<Button>();
        if (btn != null && img != null)
        {
            img.color = secondaryBackgroundColor;
            if (buttonBackgroundSprite != null) img.sprite = buttonBackgroundSprite;
            
            // Highlight colors block for button
            ColorBlock cb = btn.colors;
            cb.normalColor = secondaryBackgroundColor;
            cb.highlightedColor = accentColor;
            cb.pressedColor = new Color(accentColor.r * 0.8f, accentColor.g * 0.8f, accentColor.b * 0.8f);
            cb.selectedColor = secondaryBackgroundColor;
            btn.colors = cb;
        }

        // Setup Text
        Text txt = trans.GetComponent<Text>();
        if (txt != null)
        {
            txt.font = cyberFont;
            
            if (trans.name.ToLower().Contains("title") || trans.name.ToLower().Contains("header"))
            {
                txt.color = headerTextColor;
            }
            else if (trans.GetComponentInParent<Button>() != null)
            {
                txt.color = accentColor;
            }
            else
            {
                txt.color = textColor;
            }
        }

        // Recursively apply to children
        foreach (Transform child in trans)
        {
            ApplyThemeRecursive(child);
        }
    }
}
