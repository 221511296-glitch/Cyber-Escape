using UnityEngine;
using UnityEngine.UI;

public class TestingUIBuilder : MonoBehaviour
{
    [Header("Assign before clicking Generate")]
    public Canvas targetCanvas;
    public Font uiFont;
    public DiagnosticTestingManager testingManager;

    [ContextMenu("Generate Testing Console UI")]
    public void GenerateUI()
    {
        if (targetCanvas == null) { Debug.LogError("Assign Target Canvas!"); return; }
        if (uiFont == null) uiFont = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        // 1. Main Background Panel
        Color darkBgColor; ColorUtility.TryParseHtmlString("#060A1FF2", out darkBgColor);
        GameObject bgObj = CreatePanel("TestingBackground", targetCanvas.transform, new Vector2(800, 600), darkBgColor);

        // 2. Header
        Color headerColor; ColorUtility.TryParseHtmlString("#1E50A6FF", out headerColor);
        GameObject headerObj = CreatePanel("TestingHeader", bgObj.transform, new Vector2(800, 80), headerColor);
        SetAnchorPosition(headerObj.GetComponent<RectTransform>(), new Vector2(0.5f, 1f), new Vector2(0, -40));

        Color cyanColor; ColorUtility.TryParseHtmlString("#00EDFFFF", out cyanColor);
        Text title = CreateText("TitleText", headerObj.transform, "System Diagnostics & Testing (BB/WB)", 28, cyanColor, TextAnchor.MiddleCenter, FontStyle.Bold);
        SetAnchorPosition(title.GetComponent<RectTransform>(), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(500, 50));

        // 3. Console Area
        Color contentBgColor; ColorUtility.TryParseHtmlString("#09112AFF", out contentBgColor);
        GameObject consolePanel = CreatePanel("ConsolePanel", bgObj.transform, new Vector2(760, 350), contentBgColor);
        SetAnchorPosition(consolePanel.GetComponent<RectTransform>(), new Vector2(0.5f, 0.5f), new Vector2(0, 20));

        Text consoleText = CreateText("ConsoleText", consolePanel.transform, "Select a test type to begin...", 18, Color.white, TextAnchor.UpperLeft, FontStyle.Normal);
        SetAnchorPosition(consoleText.GetComponent<RectTransform>(), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(740, 330));

        // 4. Buttons area
        GameObject buttonArea = new GameObject("ButtonArea");
        buttonArea.transform.SetParent(bgObj.transform, false);
        SetAnchorPosition(buttonArea.AddComponent<RectTransform>(), new Vector2(0.5f, 0), new Vector2(0, 80), new Vector2(600, 60));
        
        HorizontalLayoutGroup layout = buttonArea.AddComponent<HorizontalLayoutGroup>();
        layout.spacing = 20; layout.childAlignment = TextAnchor.MiddleCenter; layout.childControlWidth = false;

        Button wbBtn = CreateButton("WhiteBoxBtn", buttonArea.transform, "Run White Box Tests", cyanColor);
        Button bbBtn = CreateButton("BlackBoxBtn", buttonArea.transform, "Run Black Box Tests", new Color(0.2f, 0.8f, 0.2f));

        // assign to manager if not null
        if (testingManager != null)
        {
            var testObj = new UnityEditor.SerializedObject(testingManager);
            testObj.FindProperty("consoleText").objectReferenceValue = consoleText;
            testObj.FindProperty("whiteBoxBtn").objectReferenceValue = wbBtn;
            testObj.FindProperty("blackBoxBtn").objectReferenceValue = bbBtn;
            testObj.ApplyModifiedProperties();
            Debug.Log("Testing UI generated and linked to manager!");
        }
    }

    GameObject CreatePanel(string name, Transform parent, Vector2 size, Color color)
    {
        GameObject go = new GameObject(name);
        go.transform.SetParent(parent, false);
        RectTransform rt = go.AddComponent<RectTransform>();
        rt.sizeDelta = size;
        Image img = go.AddComponent<Image>();
        img.color = color;
        return go;
    }

    Text CreateText(string name, Transform parent, string text, int fontSize, Color color, TextAnchor alignment, FontStyle style)
    {
        GameObject go = new GameObject(name);
        go.transform.SetParent(parent, false);
        Text t = go.AddComponent<Text>();
        t.font = uiFont;
        t.text = text;
        t.fontSize = fontSize;
        t.color = color;
        t.alignment = alignment;
        t.fontStyle = style;
        return t;
    }

    Button CreateButton(string name, Transform parent, string text, Color color)
    {
        GameObject go = CreatePanel(name, parent, new Vector2(250, 50), color);
        Button btn = go.AddComponent<Button>();
        Text t = CreateText("BtnText", go.transform, text, 20, Color.black, TextAnchor.MiddleCenter, FontStyle.Bold);
        SetAnchorPosition(t.GetComponent<RectTransform>(), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(250, 50));
        return btn;
    }

    void SetAnchorPosition(RectTransform rt, Vector2 anchors, Vector2 anchoredPosition, Vector2 sizeDelta = default)
    {
        rt.anchorMin = anchors; rt.anchorMax = anchors; rt.pivot = anchors;
        rt.anchoredPosition = anchoredPosition;
        if (sizeDelta != default) rt.sizeDelta = sizeDelta;
    }
}