using UnityEngine;
using UnityEngine.UI;

public class ControlsUIBuilder : MonoBehaviour
{
    public Canvas targetCanvas;
    
    [ContextMenu("Generate Controls UI")]
    public void GenerateControlsUI()
    {
        if (targetCanvas == null)
        {
            Debug.LogError("Please assign a Target Canvas.");
            return;
        }

        // Auto-fix: Ensure the Canvas has a Graphic Raycaster so buttons actually work
        if (targetCanvas.GetComponent<GraphicRaycaster>() == null)
        {
            targetCanvas.gameObject.AddComponent<GraphicRaycaster>();
            Debug.Log("Added missing GraphicRaycaster to Canvas.");
        }

        // Auto-fix: Bump up the sort order so no other transparent screens (like dialogue) block the clicks
        targetCanvas.sortingOrder = 100;

        // Check if there is already a Controls UI
        if (targetCanvas.transform.Find("Controls_UI") != null)
        {
            Debug.LogWarning("Controls UI already exists in canvas!");
            return;
        }

        // 1. Create Main Holder (Bottom Left)
        GameObject holderObj = new GameObject("Controls_UI", typeof(RectTransform));
        holderObj.transform.SetParent(targetCanvas.transform, false);
        RectTransform holderRect = holderObj.GetComponent<RectTransform>();
        SetRectAnchor(holderRect, new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(10, 10), new Vector2(250, 150)); // Bottom Left Padding

        // 2. Add ControlsManager Script
        ControlsManager manager = holderObj.AddComponent<ControlsManager>();

        // 3. Create Container
        GameObject containerObj = new GameObject("InstructionsContainer", typeof(RectTransform), typeof(Image));
        containerObj.transform.SetParent(holderObj.transform, false);
        Image containerImg = containerObj.GetComponent<Image>();
        ColorUtility.TryParseHtmlString("#060A1FF2", out Color bgCol);
        containerImg.color = bgCol;
        RectTransform containerRect = containerObj.GetComponent<RectTransform>();
        SetRectAnchor(containerRect, new Vector2(0, 0), new Vector2(1, 1), new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero);
        
        manager.instructionsContainer = containerObj;

        // 4. Create Header Line
        GameObject headerObj = new GameObject("ControlsHeader", typeof(RectTransform), typeof(Image));
        headerObj.transform.SetParent(containerObj.transform, false);
        Image headImg = headerObj.GetComponent<Image>();
        ColorUtility.TryParseHtmlString("#1E50A6FF", out Color headCol);
        headImg.color = headCol;
        RectTransform headerRect = headerObj.GetComponent<RectTransform>();
        SetRectAnchor(headerRect, new Vector2(0, 1), new Vector2(1, 1), new Vector2(0.5f, 1f), new Vector2(0, 0), new Vector2(0, 30));

        ColorUtility.TryParseHtmlString("#00EDFFFF", out Color cyanTxt);
        Text headerText = CreateUIText("TitleText", headerObj.transform, "Controls", 16, cyanTxt, TextAnchor.MiddleLeft, FontStyle.Bold);
        SetRectAnchor(headerText.GetComponent<RectTransform>(), new Vector2(0, 0), new Vector2(1, 1), new Vector2(0.5f, 0.5f), new Vector2(10, 0), Vector2.zero);

        // 5. Create Text list
        string controlsText = "WASD/Arrows - Move\nE - Interact\nSpace - Confirm/Check\nR-Click - Inspect Details\nEsc - Menu";
        Text infoText = CreateUIText("ControlsList", containerObj.transform, controlsText, 14, Color.white, TextAnchor.UpperLeft, FontStyle.Normal);
        SetRectAnchor(infoText.GetComponent<RectTransform>(), new Vector2(0, 0), new Vector2(1, 1), new Vector2(0.5f, 0.5f), new Vector2(10, -40), new Vector2(-10, -40));

        // 6. Create Toggle Button
        GameObject toggleBtnObj = new GameObject("ToggleButton", typeof(RectTransform), typeof(Image), typeof(Button));
        toggleBtnObj.transform.SetParent(holderObj.transform, false);
        RectTransform toggleBtnRect = toggleBtnObj.GetComponent<RectTransform>();
        // Anchor to the right edge, center vertically
        SetRectAnchor(toggleBtnRect, new Vector2(1, 0.5f), new Vector2(1, 0.5f), new Vector2(0, 0.5f), new Vector2(0, 0), new Vector2(25, 60));
        
        Image btnImg = toggleBtnObj.GetComponent<Image>();
        btnImg.color = headCol;
        
        Button btn = toggleBtnObj.GetComponent<Button>();
        btn.targetGraphic = btnImg;
        // The click listener is now added automatically by ControlsManager.Start()

        Text btnText = CreateUIText("ToggleText", toggleBtnObj.transform, "<", 14, cyanTxt, TextAnchor.MiddleCenter, FontStyle.Bold);
        SetRectAnchor(btnText.GetComponent<RectTransform>(), new Vector2(0, 0), new Vector2(1, 1), new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero);
        
        Debug.Log("Controls UI Generated Successfully in Canvas.");
    }

    private Text CreateUIText(string name, Transform parent, string label, int fontSize, Color color, TextAnchor alignment, FontStyle style)
    {
        GameObject textObj = new GameObject(name, typeof(RectTransform), typeof(Text));
        textObj.transform.SetParent(parent, false);
        Text textComp = textObj.GetComponent<Text>();
        textComp.text = label;
        textComp.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        textComp.fontSize = fontSize;
        textComp.color = color;
        textComp.alignment = alignment;
        textComp.fontStyle = style;
        return textComp;
    }

    private void SetRectAnchor(RectTransform rect, Vector2 min, Vector2 max, Vector2 pivot, Vector2 anchoredPosition, Vector2 sizeDelta)
    {
        rect.anchorMin = min;
        rect.anchorMax = max;
        rect.pivot = pivot;
        rect.anchoredPosition = anchoredPosition;
        rect.sizeDelta = sizeDelta;
    }
}
