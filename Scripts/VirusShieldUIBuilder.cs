using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class VirusShieldUIBuilder : MonoBehaviour
{
    [System.Serializable]
    public class ItemData
    {
        public string itemName = "file.exe";
        public bool isSafe = false;
        public string explanation = "This file is suspicious.";
        public Sprite icon;
    }

    [Header("Canvas References")]
    [SerializeField] private Canvas mainCanvas;
    [SerializeField] private Transform uiContainer;

    [Header("Item Database")]
    [SerializeField] private List<ItemData> itemDatabase = new List<ItemData>();

    [Header("Prefab References")]
    [SerializeField] private GameObject draggableItemPrefab;
    [SerializeField] private GameObject virusAnimationPrefab;

    [Header("Panel Settings")]
    [SerializeField] private RectOffset canvasPadding = new RectOffset(20, 20, 20, 20);

    private Dictionary<string, GameObject> uiElements = new Dictionary<string, GameObject>();

    public void BuildLevel3UI()
    {
        if (mainCanvas == null) return;

        // Create main desktop panel
        GameObject desktopPanel = CreatePanel("DesktopPanel", Vector2.zero, Vector2.zero);
        
        // Create health bar
        GameObject healthBarObj = CreateHealthBar();
        healthBarObj.transform.SetParent(desktopPanel.transform);

        // Create score display
        GameObject scoreObj = CreateScoreDisplay();
        scoreObj.transform.SetParent(desktopPanel.transform);

        // Create drop zones
        GameObject safeZoneObj = CreateDropZone("SafeZone", true);
        safeZoneObj.transform.SetParent(desktopPanel.transform);

        GameObject dangerZoneObj = CreateDropZone("DangerZone", false);
        dangerZoneObj.transform.SetParent(desktopPanel.transform);

        // Create item spawn area
        GameObject itemSpawnArea = CreatePanel("ItemSpawnArea", new Vector2(0, 100), new Vector2(200, 100));
        itemSpawnArea.transform.SetParent(desktopPanel.transform);

        // Create feedback panel
        GameObject feedbackPanel = CreateFeedbackPanel();
        feedbackPanel.transform.SetParent(desktopPanel.transform);

        // Create game over panel
        GameObject gameOverPanel = CreateGameOverPanel();
        gameOverPanel.SetActive(false);
        gameOverPanel.transform.SetParent(mainCanvas.transform);

        // Create completion panel
        GameObject completionPanel = CreateCompletionPanel();
        completionPanel.SetActive(false);
        completionPanel.transform.SetParent(mainCanvas.transform);

        Debug.Log("Level 3 UI built successfully!");
    }

    private GameObject CreatePanel(string name, Vector2 position, Vector2 size)
    {
        GameObject panelObj = new GameObject(name);
        RectTransform rect = panelObj.AddComponent<RectTransform>();
        rect.anchoredPosition = position;
        rect.sizeDelta = size;
        
        Image image = panelObj.AddComponent<Image>();
        image.color = Color.white;

        uiElements[name] = panelObj;
        return panelObj;
    }

    private GameObject CreateHealthBar()
    {
        GameObject healthBarObj = new GameObject("HealthBar");
        RectTransform rect = healthBarObj.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 1);
        rect.anchorMax = new Vector2(0.5f, 1);
        rect.anchoredPosition = new Vector2(0, -20);
        rect.sizeDelta = new Vector2(400, 40);

        Image bgImage = healthBarObj.AddComponent<Image>();
        bgImage.color = new Color(0.2f, 0.2f, 0.2f);

        // Fill object
        GameObject fillObj = new GameObject("Fill");
        fillObj.transform.SetParent(healthBarObj.transform);
        RectTransform fillRect = fillObj.AddComponent<RectTransform>();
        fillRect.anchorMin = Vector2.zero;
        fillRect.anchorMax = Vector2.zero;
        fillRect.offsetMin = Vector2.zero;
        fillRect.sizeDelta = new Vector2(400, 40);

        Image fillImage = fillObj.AddComponent<Image>();
        fillImage.color = Color.green;
        fillImage.type = Image.Type.Filled;
        fillImage.fillMethod = Image.FillMethod.Horizontal;
        fillImage.fillOrigin = (int)Image.OriginHorizontal.Left;

        // Health text
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(healthBarObj.transform);
        RectTransform textRect = textObj.AddComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;

        Text text = textObj.AddComponent<Text>();
        text.text = "100%";
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.alignment = TextAnchor.MiddleCenter;
        text.fontSize = 20;

        uiElements["HealthBar"] = healthBarObj;
        return healthBarObj;
    }

    private GameObject CreateScoreDisplay()
    {
        GameObject scoreObj = new GameObject("ScoreDisplay");
        RectTransform rect = scoreObj.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(1, 1);
        rect.anchorMax = new Vector2(1, 1);
        rect.anchoredPosition = new Vector2(-50, -50);
        rect.sizeDelta = new Vector2(200, 60);

        Image bgImage = scoreObj.AddComponent<Image>();
        bgImage.color = new Color(0.1f, 0.1f, 0.1f, 0.8f);

        // Score text
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(scoreObj.transform);
        RectTransform textRect = textObj.AddComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;

        Text text = textObj.AddComponent<Text>();
        text.text = "Score: 0";
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.alignment = TextAnchor.MiddleCenter;
        text.fontSize = 18;
        text.color = Color.white;

        uiElements["ScoreDisplay"] = scoreObj;
        return scoreObj;
    }

    private GameObject CreateDropZone(string name, bool isSafe)
    {
        GameObject zoneObj = new GameObject(name);
        RectTransform rect = zoneObj.AddComponent<RectTransform>();
        
        Vector2 position = isSafe ? new Vector2(-200, -200) : new Vector2(200, -200);
        rect.anchoredPosition = position;
        rect.sizeDelta = new Vector2(150, 150);

        Image image = zoneObj.AddComponent<Image>();
        image.color = isSafe ? Color.green : new Color(1, 0.5f, 0);

        DropZone dropZone = zoneObj.AddComponent<DropZone>();
        
        // Add text label
        GameObject labelObj = new GameObject("Label");
        labelObj.transform.SetParent(zoneObj.transform);
        RectTransform labelRect = labelObj.AddComponent<RectTransform>();
        labelRect.anchorMin = Vector2.zero;
        labelRect.anchorMax = Vector2.one;

        Text labelText = labelObj.AddComponent<Text>();
        labelText.text = isSafe ? "SAFE\nTO OPEN" : "SCAN OR\nDELETE";
        labelText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        labelText.alignment = TextAnchor.MiddleCenter;
        labelText.fontSize = 16;
        labelText.color = Color.white;

        uiElements[name] = zoneObj;
        return zoneObj;
    }

    private GameObject CreateFeedbackPanel()
    {
        GameObject panelObj = new GameObject("FeedbackPanel");
        RectTransform rect = panelObj.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0);
        rect.anchorMax = new Vector2(0.5f, 0);
        rect.anchoredPosition = new Vector2(0, 50);
        rect.sizeDelta = new Vector2(400, 150);

        Image bgImage = panelObj.AddComponent<Image>();
        bgImage.color = new Color(0.1f, 0.1f, 0.1f, 0.9f);

        CanvasGroup canvasGroup = panelObj.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 0;

        // Item name
        GameObject nameObj = new GameObject("ItemName");
        nameObj.transform.SetParent(panelObj.transform);
        RectTransform nameRect = nameObj.AddComponent<RectTransform>();
        nameRect.anchorMin = new Vector2(0, 0.7f);
        nameRect.anchorMax = new Vector2(1, 1);
        nameRect.offsetMin = Vector2.zero;
        nameRect.offsetMax = Vector2.zero;

        Text nameText = nameObj.AddComponent<Text>();
        nameText.text = "Item Name";
        nameText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        nameText.alignment = TextAnchor.MiddleCenter;
        nameText.fontSize = 18;
        nameText.color = Color.white;

        // Feedback text
        GameObject feedbackObj = new GameObject("Feedback");
        feedbackObj.transform.SetParent(panelObj.transform);
        RectTransform feedbackRect = feedbackObj.AddComponent<RectTransform>();
        feedbackRect.anchorMin = new Vector2(0, 0);
        feedbackRect.anchorMax = new Vector2(1, 0.7f);
        feedbackRect.offsetMin = Vector2.zero;
        feedbackRect.offsetMax = Vector2.zero;

        Text feedbackText = feedbackObj.AddComponent<Text>();
        feedbackText.text = "Explanation text here.";
        feedbackText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        feedbackText.alignment = TextAnchor.MiddleCenter;
        feedbackText.fontSize = 14;
        feedbackText.color = Color.white;

        uiElements["FeedbackPanel"] = panelObj;
        return panelObj;
    }

    private GameObject CreateGameOverPanel()
    {
        GameObject panelObj = new GameObject("GameOverPanel");
        RectTransform rect = panelObj.AddComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        Image bgImage = panelObj.AddComponent<Image>();
        bgImage.color = new Color(1, 0, 0, 0.7f);

        // Title
        GameObject titleObj = new GameObject("Title");
        titleObj.transform.SetParent(panelObj.transform);
        RectTransform titleRect = titleObj.AddComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0.5f, 0.7f);
        titleRect.anchorMax = new Vector2(0.5f, 0.7f);
        titleRect.sizeDelta = new Vector2(400, 100);

        Text titleText = titleObj.AddComponent<Text>();
        titleText.text = "DEVICE INFECTED!";
        titleText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        titleText.alignment = TextAnchor.MiddleCenter;
        titleText.fontSize = 40;
        titleText.color = Color.red;

        // Retry button
        GameObject retryObj = new GameObject("RetryButton");
        retryObj.transform.SetParent(panelObj.transform);
        RectTransform retryRect = retryObj.AddComponent<RectTransform>();
        retryRect.anchorMin = new Vector2(0.5f, 0.4f);
        retryRect.anchorMax = new Vector2(0.5f, 0.4f);
        retryRect.sizeDelta = new Vector2(200, 80);

        Image retryImage = retryObj.AddComponent<Image>();
        retryImage.color = Color.green;

        Button retryButton = retryObj.AddComponent<Button>();

        Text retryText = new GameObject("Text").AddComponent<Text>();
        retryText.transform.SetParent(retryObj.transform);
        retryText.text = "RETRY";
        retryText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        retryText.alignment = TextAnchor.MiddleCenter;
        retryText.fontSize = 24;
        retryText.color = Color.white;

        uiElements["GameOverPanel"] = panelObj;
        return panelObj;
    }

    private GameObject CreateCompletionPanel()
    {
        GameObject panelObj = new GameObject("CompletionPanel");
        RectTransform rect = panelObj.AddComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        Image bgImage = panelObj.AddComponent<Image>();
        bgImage.color = new Color(0, 1, 0, 0.7f);

        // Title
        GameObject titleObj = new GameObject("Title");
        titleObj.transform.SetParent(panelObj.transform);
        RectTransform titleRect = titleObj.AddComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0.5f, 0.7f);
        titleRect.anchorMax = new Vector2(0.5f, 0.7f);
        titleRect.sizeDelta = new Vector2(400, 100);

        Text titleText = titleObj.AddComponent<Text>();
        titleText.text = "LEVEL COMPLETE!";
        titleText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        titleText.alignment = TextAnchor.MiddleCenter;
        titleText.fontSize = 40;
        titleText.color = Color.green;

        // Next button
        GameObject nextObj = new GameObject("NextButton");
        nextObj.transform.SetParent(panelObj.transform);
        RectTransform nextRect = nextObj.AddComponent<RectTransform>();
        nextRect.anchorMin = new Vector2(0.5f, 0.3f);
        nextRect.anchorMax = new Vector2(0.5f, 0.3f);
        nextRect.sizeDelta = new Vector2(200, 80);

        Image nextImage = nextObj.AddComponent<Image>();
        nextImage.color = Color.blue;

        Button nextButton = nextObj.AddComponent<Button>();

        Text nextText = new GameObject("Text").AddComponent<Text>();
        nextText.transform.SetParent(nextObj.transform);
        nextText.text = "NEXT";
        nextText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        nextText.alignment = TextAnchor.MiddleCenter;
        nextText.fontSize = 24;
        nextText.color = Color.white;

        uiElements["CompletionPanel"] = panelObj;
        return panelObj;
    }

    public GameObject GetUIElement(string name)
    {
        return uiElements.ContainsKey(name) ? uiElements[name] : null;
    }

    public void PopulateItemDatabase(ItemQueueManager itemManager)
    {
        if (itemManager == null) return;

        itemManager.ClearDatabase();
        foreach (var item in itemDatabase)
        {
            itemManager.AddItemToDatabase(new ItemQueueManager.VirusItem
            {
                itemName = item.itemName,
                isSafe = item.isSafe,
                explanation = item.explanation,
                icon = item.icon
            });
        }
    }
}
