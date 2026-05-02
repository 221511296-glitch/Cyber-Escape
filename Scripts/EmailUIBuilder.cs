using UnityEngine;
using UnityEngine.UI;

public class EmailUIBuilder : MonoBehaviour
{
    [Header("Assign these before generating")]
    public Canvas targetCanvas;
    public Font uiFont;
    public Sprite panelBackgroundSprite;
    public Sprite buttonSprite;
    public Sprite headerSprite;
    public Sprite emailIconSprite;
    
    [Header("Email Manager reference (optional)")]
    public EmailPhishingLevelManager emailManager;

    [ContextMenu("Generate Email Inbox UI")]
    public void GenerateUI()
    {
        if (targetCanvas == null)
        {
            Debug.LogError("EmailUIBuilder: Please assign a Target Canvas.");
            return;
        }
        if (uiFont == null) uiFont = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        // 1. MainWindow (Background with light shadows, size 1000x700)
        GameObject mainWindow = CreateUIPanel("EmailWindow", targetCanvas.transform, new Vector2(1000, 700), panelBackgroundSprite, Color.white);
        
        // 2. Sidebar (Left panel - Outlook style)
        Color sidebarColor; ColorUtility.TryParseHtmlString("#F4F5F7FF", out sidebarColor);
        GameObject sidebar = CreateUIPanel("Sidebar", mainWindow.transform, new Vector2(250, 700), panelBackgroundSprite, sidebarColor);
        SetRectAnchor(sidebar.GetComponent<RectTransform>(), new Vector2(0, 0.5f), new Vector2(0, 0.5f), new Vector2(0, 0.5f), new Vector2(125, 0));
        
        // Sidebar Texts
        Text folderText = CreateUIText("FolderText", sidebar.transform, "All    Unread", 16, Color.black, TextAnchor.MiddleLeft, FontStyle.Bold);
        SetRectAnchor(folderText.GetComponent<RectTransform>(), new Vector2(0, 1), new Vector2(1, 1), new Vector2(0.5f, 1), new Vector2(0, -30), new Vector2(-40, 30));
        
        Color activeColor; ColorUtility.TryParseHtmlString("#E0E0E0FF", out activeColor);
        GameObject activeEmailCell = CreateUIPanel("ActiveEmailCell", sidebar.transform, new Vector2(250, 80), panelBackgroundSprite, activeColor);
        SetRectAnchor(activeEmailCell.GetComponent<RectTransform>(), new Vector2(0.5f, 1), new Vector2(0.5f, 1), new Vector2(0.5f, 1), new Vector2(0, -90));
        
        Color activeLineColor; ColorUtility.TryParseHtmlString("#0078D4FF", out activeLineColor);
        GameObject activeLine = CreateUIPanel("ActiveLine", activeEmailCell.transform, new Vector2(5, 70), panelBackgroundSprite, activeLineColor);
        SetRectAnchor(activeLine.GetComponent<RectTransform>(), new Vector2(0, 0.5f), new Vector2(0, 0.5f), new Vector2(0, 0.5f), new Vector2(2.5f, 0));
        
        Text sidebarSender = CreateUIText("SidebarSender", activeEmailCell.transform, "Current Email", 16, Color.black, TextAnchor.MiddleLeft, FontStyle.Bold);
        SetRectAnchor(sidebarSender.GetComponent<RectTransform>(), new Vector2(0, 1), new Vector2(1, 1), new Vector2(0.5f, 1), new Vector2(0, -20), new Vector2(-30, 20));
        
        // 3. Right Main Area
        GameObject rightArea = CreateGameObject("RightArea", mainWindow.transform);
        SetRectAnchor(rightArea.GetComponent<RectTransform>(), new Vector2(0, 0.5f), new Vector2(0, 0.5f), new Vector2(0, 0.5f), new Vector2(250 + 375, 0), new Vector2(750, 700));
        
        // Outer structure for Subject, Sender, Body.
        Text subjectText = CreateUIText("SubjectText", rightArea.transform, "[Subject here]", 28, Color.black, TextAnchor.MiddleLeft, FontStyle.Bold);
        SetRectAnchor(subjectText.GetComponent<RectTransform>(), new Vector2(0, 1), new Vector2(1, 1), new Vector2(0.5f, 1), new Vector2(0, -50), new Vector2(-60, 50));
        
        // Sender Block
        GameObject senderBlock = CreateGameObject("SenderBlock", rightArea.transform);
        SetRectAnchor(senderBlock.GetComponent<RectTransform>(), new Vector2(0.5f, 1), new Vector2(0.5f, 1), new Vector2(0.5f, 1), new Vector2(0, -120), new Vector2(710, 60));
        
        // Profile Initials Icon
        GameObject profileIcon = CreateUIImage("ProfileIcon", senderBlock.transform, new Vector2(50, 50), emailIconSprite, activeLineColor);
        SetRectAnchor(profileIcon.GetComponent<RectTransform>(), new Vector2(0, 0.5f), new Vector2(0, 0.5f), new Vector2(0, 0.5f), new Vector2(25, 0));
        Text profileInitials = CreateUIText("ProfileInitials", profileIcon.transform, "SF", 18, Color.white, TextAnchor.MiddleCenter, FontStyle.Bold);
        SetRectAnchor(profileInitials.GetComponent<RectTransform>(), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(50, 50));
        
        Text senderText = CreateUIText("SenderText", senderBlock.transform, "[Sender here]", 18, Color.black, TextAnchor.MiddleLeft, FontStyle.Bold);
        SetRectAnchor(senderText.GetComponent<RectTransform>(), new Vector2(0, 1), new Vector2(1, 1), new Vector2(0, 1), new Vector2(70, -15), new Vector2(-80, 30));
        Text toText = CreateUIText("ToText", senderBlock.transform, "To: You", 14, Color.gray, TextAnchor.MiddleLeft, FontStyle.Normal);
        SetRectAnchor(toText.GetComponent<RectTransform>(), new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, 0), new Vector2(70, 15), new Vector2(-80, 30));
        
        // Email Body
        Text previewText = CreateUIText("PreviewText", rightArea.transform, "Email body preview goes here...", 18, Color.black, TextAnchor.UpperLeft, FontStyle.Normal);
        SetRectAnchor(previewText.GetComponent<RectTransform>(), new Vector2(0, 1), new Vector2(1, 1), new Vector2(0.5f, 1), new Vector2(0, -220), new Vector2(-60, 100));
        
        // Highlighted URL
        GameObject urlBg = CreateUIPanel("UrlBg", rightArea.transform, new Vector2(710, 30), panelBackgroundSprite, new Color(1f, 1f, 0.4f, 0.8f)); // Yellow highlight
        SetRectAnchor(urlBg.GetComponent<RectTransform>(), new Vector2(0.5f, 1), new Vector2(0.5f, 1), new Vector2(0.5f, 1), new Vector2(0, -320), new Vector2(710, 30));
        
        Color linkColor; ColorUtility.TryParseHtmlString("#0000FFFF", out linkColor);
        Text urlText = CreateUIText("UrlText", urlBg.transform, "[URL hidden]", 18, linkColor, TextAnchor.MiddleLeft, FontStyle.Bold);
        SetRectAnchor(urlText.GetComponent<RectTransform>(), new Vector2(0, 0.5f), new Vector2(1, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0, 0), new Vector2(-10, 30));
        
        // Dummy Outlook Toolbar
        GameObject dummyToolbar = CreateGameObject("DummyToolbar", senderBlock.transform);
        SetRectAnchor(dummyToolbar.GetComponent<RectTransform>(), new Vector2(1, 0.5f), new Vector2(1, 0.5f), new Vector2(1, 0.5f), new Vector2(0, 0), new Vector2(300, 40));
        Text dummyToolbarText = CreateUIText("MockToolbar", dummyToolbar.transform, "[ Reply ]  [ Reply All ]  [ Forward ]", 14, Color.gray, TextAnchor.MiddleRight, FontStyle.Normal);
        SetRectAnchor(dummyToolbarText.GetComponent<RectTransform>(), new Vector2(1, 0.5f), new Vector2(1, 0.5f), new Vector2(1, 0.5f), new Vector2(-20, 0), new Vector2(300, 40));

        // --- Game Overlay UI Overlay at the Bottom ---
        GameObject gameOverlay = CreateUIPanel("GameOverlay", rightArea.transform, new Vector2(750, 250), panelBackgroundSprite, new Color(0.9f, 0.95f, 1f, 0.8f)); // Slight transparent blue tint overlay for game area
        SetRectAnchor(gameOverlay.GetComponent<RectTransform>(), new Vector2(0.5f, 0), new Vector2(0.5f, 0), new Vector2(0.5f, 0), new Vector2(0, 125));
        
        // Mentor Text
        Text mentorText = CreateUIText("MentorText", gameOverlay.transform, "AI Mentor: Let's analyze this email...", 18, activeLineColor, TextAnchor.MiddleCenter, FontStyle.Italic);
        SetRectAnchor(mentorText.GetComponent<RectTransform>(), new Vector2(0.5f, 1), new Vector2(0.5f, 1), new Vector2(0.5f, 1), new Vector2(0, -30), new Vector2(700, 40));
        
        // Feedback Text
        Text feedbackText = CreateUIText("FeedbackText", gameOverlay.transform, "", 20, Color.black, TextAnchor.MiddleCenter, FontStyle.Bold);
        SetRectAnchor(feedbackText.GetComponent<RectTransform>(), new Vector2(0.5f, 1), new Vector2(0.5f, 1), new Vector2(0.5f, 1), new Vector2(0, -60), new Vector2(700, 40));
        
        // Buttons Area (Phishing/Not Phishing)
        GameObject decisionArea = CreateGameObject("DecisionButtonsArea", gameOverlay.transform);
        SetRectAnchor(decisionArea.GetComponent<RectTransform>(), new Vector2(0.5f, 0), new Vector2(0.5f, 0), new Vector2(0.5f, 0), new Vector2(0, 120), new Vector2(450, 50));
        HorizontalLayoutGroup decLayout = decisionArea.AddComponent<HorizontalLayoutGroup>();
        decLayout.spacing = 20; decLayout.childAlignment = TextAnchor.MiddleCenter; decLayout.childControlWidth = false;
        
        Button phishingBtn = CreateUIButton("PhishingButton", decisionArea.transform, "⚠️ Warning (Phishing)", new Vector2(200, 50), new Color(0.85f, 0.3f, 0.3f));
        Button notPhishingBtn = CreateUIButton("NotPhishingButton", decisionArea.transform, "✅ Safe (Not Phishing)", new Vector2(200, 50), new Color(0.3f, 0.7f, 0.3f));
        
        // Buttons Area (Actions)
        GameObject actionArea = CreateGameObject("ActionButtonsArea", gameOverlay.transform);
        SetRectAnchor(actionArea.GetComponent<RectTransform>(), new Vector2(0.5f, 0), new Vector2(0.5f, 0), new Vector2(0.5f, 0), new Vector2(0, 50), new Vector2(600, 50));
        HorizontalLayoutGroup actLayout = actionArea.AddComponent<HorizontalLayoutGroup>();
        actLayout.spacing = 15; actLayout.childAlignment = TextAnchor.MiddleCenter; actLayout.childControlWidth = false;
        
        Button reportBtn = CreateUIButton("ReportButton", actionArea.transform, "Report", new Vector2(150, 45), new Color(0.9f, 0.5f, 0.1f));
        Button ignoreBtn = CreateUIButton("IgnoreButton", actionArea.transform, "Ignore", new Vector2(150, 45), new Color(0.5f, 0.5f, 0.5f));
        Button openLinkBtn = CreateUIButton("OpenLinkButton", actionArea.transform, "Open Link", new Vector2(150, 45), new Color(0.3f, 0.6f, 0.9f));
        
        // Score & Progress
        Text scoreText = CreateUIText("ScoreText", mainWindow.transform, "Score: 0", 20, Color.black, TextAnchor.MiddleRight, FontStyle.Bold);
        SetRectAnchor(scoreText.GetComponent<RectTransform>(), new Vector2(1, 0), new Vector2(1, 0), new Vector2(1, 0), new Vector2(-20, 20), new Vector2(200, 40));
        
        Slider progressBar = CreateGameObject("ProgressBar", mainWindow.transform).AddComponent<Slider>();
        SetRectAnchor(progressBar.GetComponent<RectTransform>(), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(150, 20), new Vector2(250, 20));
        
        // Next Button
        Button nextBtn = CreateUIButton("NextButton", gameOverlay.transform, "Next Email >", new Vector2(160, 50), activeLineColor);
        SetRectAnchor(nextBtn.GetComponent<RectTransform>(), new Vector2(1, 0), new Vector2(1, 0), new Vector2(1, 0), new Vector2(-90, 40));
        nextBtn.gameObject.SetActive(false);

        // Auto Assign to Level Manager
        if (emailManager != null)
        {
            var serializedObj = new UnityEditor.SerializedObject(emailManager);
            serializedObj.FindProperty("inboxPanel").objectReferenceValue = mainWindow;
            serializedObj.FindProperty("subjectText").objectReferenceValue = subjectText;
            serializedObj.FindProperty("previewText").objectReferenceValue = previewText;
            serializedObj.FindProperty("senderText").objectReferenceValue = senderText;
            serializedObj.FindProperty("urlText").objectReferenceValue = urlText;
            serializedObj.FindProperty("feedbackText").objectReferenceValue = feedbackText;
            serializedObj.FindProperty("mentorText").objectReferenceValue = mentorText;
            serializedObj.FindProperty("scoreText").objectReferenceValue = scoreText;
            serializedObj.FindProperty("progressBar").objectReferenceValue = progressBar;
            
            serializedObj.FindProperty("phishingButton").objectReferenceValue = phishingBtn;
            serializedObj.FindProperty("notPhishingButton").objectReferenceValue = notPhishingBtn;
            serializedObj.FindProperty("reportButton").objectReferenceValue = reportBtn;
            serializedObj.FindProperty("ignoreButton").objectReferenceValue = ignoreBtn;
            serializedObj.FindProperty("openLinkButton").objectReferenceValue = openLinkBtn;
            serializedObj.FindProperty("nextButton").objectReferenceValue = nextBtn;
            
            serializedObj.ApplyModifiedProperties();
            Debug.Log("UI Generated to closely match the reference image and assigned structure!");
        }
    }

    // Helper Methods
    private GameObject CreateGameObject(string name, Transform parent)
    {
        GameObject go = new GameObject(name);
        go.transform.SetParent(parent, false);
        go.AddComponent<RectTransform>();
        return go;
    }

    private GameObject CreateUIPanel(string name, Transform parent, Vector2 size, Sprite sprite, Color color)
    {
        GameObject panelObj = CreateGameObject(name, parent);
        RectTransform rt = panelObj.GetComponent<RectTransform>();
        rt.sizeDelta = size;
        Image img = panelObj.AddComponent<Image>();
        if (sprite != null) img.sprite = sprite;
        img.color = color;
        img.type = Image.Type.Sliced;
        return panelObj;
    }

    private GameObject CreateUIImage(string name, Transform parent, Vector2 size, Sprite sprite, Color? color = null)
    {
        GameObject imgObj = CreateGameObject(name, parent);
        RectTransform rt = imgObj.GetComponent<RectTransform>();
        rt.sizeDelta = size;
        Image img = imgObj.AddComponent<Image>();
        if (sprite != null) img.sprite = sprite;
        if (color.HasValue) img.color = color.Value;
        return imgObj;
    }

    private Text CreateUIText(string name, Transform parent, string textContent, int fontSize, Color color, TextAnchor alignment, FontStyle style)
    {
        GameObject textObj = CreateGameObject(name, parent);
        Text txt = textObj.AddComponent<Text>();
        txt.font = uiFont;
        txt.text = textContent;
        txt.fontSize = fontSize;
        txt.color = color;
        txt.alignment = alignment;
        txt.fontStyle = style;
        return txt;
    }

    private Button CreateUIButton(string name, Transform parent, string label, Vector2 size, Color color)
    {
        GameObject btnObj = CreateUIPanel(name, parent, size, buttonSprite, color);
        Button btn = btnObj.AddComponent<Button>();
        Text btnText = CreateUIText("Text", btnObj.transform, label, 18, Color.white, TextAnchor.MiddleCenter, FontStyle.Bold);
        SetRectAnchor(btnText.GetComponent<RectTransform>(), Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero);
        return btn;
    }

    private void SetRectAnchor(RectTransform rt, Vector2 min, Vector2 max, Vector2 pivot, Vector2 pos, Vector2 size = default)
    {
        rt.anchorMin = min; rt.anchorMax = max; rt.pivot = pivot;
        rt.anchoredPosition = pos; if (size != default) rt.sizeDelta = size;
    }
}
