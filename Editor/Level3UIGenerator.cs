using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class Level3UIGenerator : EditorWindow
{
    // Windows 7 Theme Colors
    private static Color bgDarkColor = new Color(0.1f, 0.4f, 0.8f, 1f); // Classic Blue Desktop
    private static Color panelBgColor = new Color(0.6f, 0.8f, 0.9f, 0.8f); // Aero Glass
    private static Color neonCyan = new Color(1f, 1f, 1f, 1f); // Reused for Window Content
    private static Color neonMagenta = new Color(0.8f, 0.2f, 0.2f, 1f);
    private static Color neonGreen = new Color(0.2f, 0.8f, 0.2f, 1f);
    private static Color neonRed = new Color(0.8f, 0.2f, 0.2f, 1f);

    [MenuItem("Window/Cyber Escape Game/Level 3 UI Generator")]
    public static void ShowWindow()
    {
        GetWindow<Level3UIGenerator>("Level 3 UI Gen");
    }

    private void OnGUI()
    {
        GUILayout.Label("Level 3 Professional UI Generator", EditorStyles.boldLabel);
        GUILayout.Space(10);

        if (GUILayout.Button("Generate Professional UI", GUILayout.Height(50)))
        {
            GenerateLevel3UI();
        }

        GUILayout.Space(10);
        GUILayout.Label("This creates a highly polished, cyberpunk-themed UI:", EditorStyles.miniLabel);
        GUILayout.Label("✓ Dark node-style background with neon accents", EditorStyles.miniLabel);
        GUILayout.Label("✓ System Status, Threat Map, and Log panels", EditorStyles.miniLabel);
        GUILayout.Label("✓ Clean crosshair Item Spawn Area", EditorStyles.miniLabel);
        GUILayout.Label("✓ Fully wired Manager scripts", EditorStyles.miniLabel);
    }

    public static void GenerateLevel3UI()
    {
        Scene activeScene = EditorSceneManager.GetActiveScene();
        if (!activeScene.IsValid())
        {
            EditorUtility.DisplayDialog("Error", "Please open or create a scene first!", "OK");
            return;
        }

        // Clean up previous generated objects to prevent unassigned reference ghosts
        GameObject oldCanvas = GameObject.Find("Canvas");
        if (oldCanvas != null) DestroyImmediate(oldCanvas);
        GameObject oldManagers = GameObject.Find("Managers");
        if (oldManagers != null) DestroyImmediate(oldManagers);
        GameObject oldEventSystem = GameObject.Find("EventSystem");
        if (oldEventSystem != null) DestroyImmediate(oldEventSystem);

        // Base Setup
        GameObject canvas = CreateCanvas();
        GameObject managers = new GameObject("Managers");
        managers.transform.position = Vector3.zero;

        // UI Base Elements
        GameObject desktopPanel = CreateCyberBackground(canvas);
        
        // --- Professional UI Layout ---
        GameObject topBar = CreateTopMenuBar(desktopPanel);
        GameObject sysStatusPanel = CreateSystemStatusPanel(desktopPanel);
        GameObject itemSpawnArea = CreateItemSpawnAreaGrid(desktopPanel);
        GameObject threatMapPanel = CreateThreatMapPanel(desktopPanel);
        GameObject sysLogPanel = CreateSystemLogPanel(desktopPanel);
        GameObject userProfilePanel = CreateUserProfilePanel(desktopPanel);
        
        // Critical Gameplay Elements mapped into the aesthetic
        GameObject healthBar = CreateNeonHealthBar(sysStatusPanel);
        GameObject safeZone = CreateDropZone(desktopPanel, "Systems Drive (Safe)", true, "My Computer", 0);
        GameObject dangerZone = CreateDropZone(desktopPanel, "Recycle Bin (Danger)", false, "Recycle Bin", 300);

        GameObject feedbackPanel = CreateProfessionalFeedbackPanel(desktopPanel);
        GameObject redFlash = CreateFlashOverlay(canvas, "RedFlashOverlay", new Color(1f, 0f, 0f, 0.3f));
        GameObject greenFlash = CreateFlashOverlay(canvas, "GreenFlashOverlay", new Color(0f, 1f, 0f, 0.2f));
        GameObject gameOverPanel = CreateProfessionalGameOverPanel(canvas);
        GameObject completionPanel = CreateProfessionalCompletionPanel(canvas);
        GameObject tutorialPanel = CreateLevel3TutorialPanel(canvas);

        GameObject scoreDisplay = CreateNeonScoreDisplay(topBar);

        // Event System
        if (FindObjectOfType<EventSystem>() == null)
        {
            GameObject evt = new GameObject("EventSystem");
            evt.AddComponent<EventSystem>();
            evt.AddComponent<StandaloneInputModule>();
        }

        // Add manager scripts securely into separated GameObjects for easy user discovery
        VirusShieldLevelManager levelManager = managers.AddComponent<VirusShieldLevelManager>();

        GameObject tutorialSysObj = new GameObject("Level3TutorialSystem");
        tutorialSysObj.transform.SetParent(managers.transform, false);
        Level3TutorialManager tutorialManager = tutorialSysObj.AddComponent<Level3TutorialManager>();

        GameObject healthBarSysObj = new GameObject("HealthBarSystem");
        healthBarSysObj.transform.SetParent(managers.transform, false);
        HealthBarSystem healthBarSystem = healthBarSysObj.AddComponent<HealthBarSystem>();

        GameObject visualFeedbackSysObj = new GameObject("VisualFeedbackSystem");
        visualFeedbackSysObj.transform.SetParent(managers.transform, false);
        VisualFeedbackSystem visualFeedbackSystem = visualFeedbackSysObj.AddComponent<VisualFeedbackSystem>();

        ItemQueueManager itemQueueManager = itemSpawnArea.AddComponent<ItemQueueManager>();
        EducationalFeedbackPanel educationalPanel = feedbackPanel.AddComponent<EducationalFeedbackPanel>();

        // Guaranteed property assignment helper (combines Serialized Object and Reflection fallback)
        System.Action<Object, string, Object> AssignProp = (targetObj, propName, valObj) => {
            if (targetObj == null || valObj == null) return;
            
            bool assigned = false;
            SerializedObject so = new SerializedObject(targetObj);
            SerializedProperty prop = so.FindProperty(propName);
            if (prop != null) {
                prop.objectReferenceValue = valObj;
                so.ApplyModifiedProperties();
                assigned = true;
            }
            
            if (!assigned) {
                var field = targetObj.GetType().GetField(propName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                if (field != null) field.SetValue(targetObj, valObj);
            }
        };

        // Wire VirusShieldLevelManager
        AssignProp(levelManager, "mainCanvas", canvas);
        AssignProp(levelManager, "desktopPanel", desktopPanel);
        AssignProp(levelManager, "gameOverPanel", gameOverPanel);
        AssignProp(levelManager, "completionPanel", completionPanel);
        AssignProp(levelManager, "itemQueueManager", itemQueueManager);
        AssignProp(levelManager, "healthBarSystem", healthBarSystem);
        AssignProp(levelManager, "visualFeedbackSystem", visualFeedbackSystem);
        AssignProp(levelManager, "feedbackPanel", educationalPanel);
        AssignProp(levelManager, "desktopEnvironment", desktopPanel);
        AssignProp(levelManager, "safeZone", safeZone.GetComponent<DropZone>());
        AssignProp(levelManager, "dangerZone", dangerZone.GetComponent<DropZone>());
        AssignProp(levelManager, "scoreText", scoreDisplay.GetComponent<Text>());
        AssignProp(levelManager, "levelCompleteText", completionPanel.transform.Find("LevelCompleteText")?.GetComponent<Text>());
        AssignProp(levelManager, "retryButton", gameOverPanel.transform.Find("RetryButton")?.GetComponent<Button>());
        AssignProp(levelManager, "nextLevelButton", completionPanel.transform.Find("NextButton")?.GetComponent<Button>());

        // Wire Level3TutorialManager
        AssignProp(tutorialManager, "levelManager", levelManager);
        AssignProp(tutorialManager, "tutorialPanel", tutorialPanel);
        AssignProp(tutorialManager, "titleText", tutorialPanel.transform.Find("TitleText")?.GetComponent<Text>());
        AssignProp(tutorialManager, "bodyText", tutorialPanel.transform.Find("BodyText")?.GetComponent<Text>());
        AssignProp(tutorialManager, "nextButton", tutorialPanel.transform.Find("NextButton")?.GetComponent<Button>());

        // Wire ItemQueueManager
        AssignProp(itemQueueManager, "spawnContainer", itemSpawnArea.transform);

        // Wire VisualFeedbackSystem
        AssignProp(visualFeedbackSystem, "redFlashOverlay", redFlash.GetComponent<Image>());
        AssignProp(visualFeedbackSystem, "greenFlashOverlay", greenFlash.GetComponent<Image>());

        // Wire EducationalFeedbackPanel
        AssignProp(educationalPanel, "panel", feedbackPanel);
        AssignProp(educationalPanel, "itemNameText", feedbackPanel.transform.Find("ContentArea/ItemNameText")?.GetComponent<Text>());
        AssignProp(educationalPanel, "feedbackText", feedbackPanel.transform.Find("ContentArea/ExplanationText")?.GetComponent<Text>());
        AssignProp(educationalPanel, "correctnessIndicator", feedbackPanel.transform.Find("ContentArea/CorrectnessIndicator")?.GetComponent<Image>());

        // PERCENTAGE TEXT
        GameObject pctTextObj = new GameObject("PercentageText");
        pctTextObj.transform.SetParent(healthBar.transform, false);
        Text pctText = pctTextObj.AddComponent<Text>();
        pctText.text = "100%";
        pctText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        pctText.fontSize = 18;
        pctText.fontStyle = FontStyle.Bold;
        pctText.color = neonGreen;
        pctText.alignment = TextAnchor.MiddleRight;
        RectTransform pctRect = pctTextObj.GetComponent<RectTransform>();
        pctRect.anchorMin = new Vector2(1,1);
        pctRect.anchorMax = new Vector2(1,1);
        pctRect.sizeDelta = new Vector2(60, 30);
        pctRect.anchoredPosition = new Vector2(0, 30);

        // Wire HealthBarSystem
        AssignProp(healthBarSystem, "healthText", pctText);
        AssignProp(healthBarSystem, "healthBarSlider", healthBar.GetComponent<Slider>());
        AssignProp(healthBarSystem, "healthSlider", healthBar.GetComponent<Slider>());
        AssignProp(healthBarSystem, "slider", healthBar.GetComponent<Slider>());
        
        Transform fillTransform = healthBar.transform.Find("Fill Area/Fill");
        if(fillTransform != null) {
            AssignProp(healthBarSystem, "fillImage", fillTransform.GetComponent<Image>());
            AssignProp(healthBarSystem, "healthBarFill", fillTransform.GetComponent<Image>());
            AssignProp(healthBarSystem, "healthFill", fillTransform.GetComponent<Image>());
        }

        // Draggable Item Prefab Setup
        string prefabDir = "Assets/Prefabs";
        if (!System.IO.Directory.Exists(prefabDir)) System.IO.Directory.CreateDirectory(prefabDir);
        string prefabPath = prefabDir + "/AutoDraggableItem.prefab";
        
        GameObject prefabObj = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        if (prefabObj == null) {
            GameObject draggableObj = new GameObject("DraggableItem");
            RectTransform dRect = draggableObj.AddComponent<RectTransform>();
            dRect.sizeDelta = new Vector2(80, 80);
            Image dImage = draggableObj.AddComponent<Image>();
            dImage.color = new Color(0.8f, 0.8f, 1f, 1f);
            
            draggableObj.AddComponent<DraggableItem>();
            
            GameObject labelObj = new GameObject("Label");
            labelObj.transform.SetParent(draggableObj.transform, false);
            Text dLabel = labelObj.AddComponent<Text>();
            dLabel.text = "File";
            dLabel.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            dLabel.fontSize = 12;
            dLabel.alignment = TextAnchor.MiddleCenter;
            dLabel.color = Color.black;
            RectTransform lRect = labelObj.GetComponent<RectTransform>();
            lRect.sizeDelta = new Vector2(100, 30);
            lRect.anchoredPosition = new Vector2(0, -60);

            prefabObj = PrefabUtility.SaveAsPrefabAsset(draggableObj, prefabPath);
            DestroyImmediate(draggableObj);
        }

        AssignProp(itemQueueManager, "draggableItemPrefab", prefabObj);

        // Attach Level3TerminalTrigger automatically to EmailTerminal if it exists
        GameObject emailTerminalObj = GameObject.Find("EmailTerminal");
        if (emailTerminalObj != null)
        {
            // Specifically search for our interaction prompt based on the user's hierarchy
            GameObject interactionPromptObj = null;
            GameObject controlsUI = GameObject.Find("Controls UI Builder");
            if (controlsUI != null && controlsUI.transform.childCount > 0)
            {
                interactionPromptObj = controlsUI.transform.GetChild(0).gameObject;
            }

            Level3TerminalTrigger trigger = emailTerminalObj.GetComponent<Level3TerminalTrigger>();
            if (trigger == null) trigger = emailTerminalObj.AddComponent<Level3TerminalTrigger>();
            
            AssignProp(trigger, "mainCanvas", canvas);
            AssignProp(trigger, "levelManager", levelManager);
            AssignProp(trigger, "interactionPrompt", interactionPromptObj);
        }

        // Populate database securely
        var dbField = itemQueueManager.GetType().GetField("itemDatabase", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (dbField != null) {
            var list = dbField.GetValue(itemQueueManager) as System.Collections.IList;
            if (list == null) {
                list = new System.Collections.Generic.List<ItemQueueManager.VirusItem>();
                dbField.SetValue(itemQueueManager, list);
            }
            
            // Generate distinctive Sprite assets dynamically
            Sprite txtSprite = CreateDynamicFileSprite("TXT_Icon", new Color(0.1f, 0.4f, 0.8f)); // Blue
            Sprite imgSprite = CreateDynamicFileSprite("IMG_Icon", new Color(0.1f, 0.8f, 0.4f)); // Green
            Sprite exeSprite = CreateDynamicFileSprite("EXE_Icon", new Color(0.8f, 0.1f, 0.1f)); // Red
            Sprite scriptSprite = CreateDynamicFileSprite("SCR_Icon", new Color(0.8f, 0.4f, 0.1f)); // Orange
            Sprite zipSprite = CreateDynamicFileSprite("ZIP_Icon", new Color(0.6f, 0.1f, 0.6f)); // Purple

            list.Clear();
            list.Add(new ItemQueueManager.VirusItem { itemName = "document.txt", isSafe = true, explanation = "Text documents are safe to open. They don't contain executable code.", icon = txtSprite });
            list.Add(new ItemQueueManager.VirusItem { itemName = "image.jpg", isSafe = true, explanation = "Image files are generally safe. Just standard picture files.", icon = imgSprite });
            list.Add(new ItemQueueManager.VirusItem { itemName = "readme.txt", isSafe = true, explanation = "Text files are safe. No risk of malware.", icon = txtSprite });
            list.Add(new ItemQueueManager.VirusItem { itemName = "vacation_photo.png", isSafe = true, explanation = "Photos from your camera are safe to view.", icon = imgSprite });
            list.Add(new ItemQueueManager.VirusItem { itemName = "spreadsheet.xlsx", isSafe = true, explanation = "Office documents from trusted sources are safe.", icon = txtSprite }); 

            list.Add(new ItemQueueManager.VirusItem { itemName = "virus.exe", isSafe = false, explanation = "Executable files (.exe) from unknown sources are DANGEROUS. They can execute malware.", icon = exeSprite });
            list.Add(new ItemQueueManager.VirusItem { itemName = "malware.bat", isSafe = false, explanation = "Batch files can execute harmful system commands. DELETE immediately.", icon = scriptSprite });
            list.Add(new ItemQueueManager.VirusItem { itemName = "trojan.scr", isSafe = false, explanation = "Screen savers from untrusted sources often hide trojans. SCAN or DELETE.", icon = scriptSprite });
            list.Add(new ItemQueueManager.VirusItem { itemName = "ransomware.zip", isSafe = false, explanation = "Archive files from unknown senders could contain encrypted payloads. SCAN first.", icon = zipSprite });
            list.Add(new ItemQueueManager.VirusItem { itemName = "installer.exe", isSafe = false, explanation = "Installers from unknown sources could install unwanted software. Be cautious.", icon = exeSprite });
        }

        EditorSceneManager.SaveScene(activeScene);
        DisplaySuccessMessage();
    }

    private static GameObject CreateCanvas()
    {
        GameObject canvasObj = new GameObject("Canvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        canvasObj.AddComponent<GraphicRaycaster>();
        return canvasObj;
    }

    private static GameObject CreateCyberBackground(GameObject canvas)
    {
        Sprite wpSprite = GenerateUISprite("Win7_Wallpaper_Dummy", bgDarkColor, 128, false);
        GameObject panelObj = new GameObject("DesktopPanel_CyberBackground");
        panelObj.transform.SetParent(canvas.transform, false);
        Image image = panelObj.AddComponent<Image>();
        image.sprite = wpSprite;
        RectTransform rect = panelObj.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        
        return panelObj;
    }

    private static GameObject CreateCyberPanel(Transform parent, string name, Vector2 anchorMin, Vector2 anchorMax, Vector2 sizeDelta, Vector2 anchoredPos, string title)
    {
        Sprite aeroSprite = GenerateUISprite("Win7_WindowAero", panelBgColor, 64, true);
        Sprite closeBtnSprite = GenerateUISprite("Win7_CloseBtn", neonRed, 32, false);

        GameObject panelObj = new GameObject(name);
        panelObj.transform.SetParent(parent, false);
        RectTransform rect = panelObj.AddComponent<RectTransform>();
        rect.anchorMin = anchorMin; rect.anchorMax = anchorMax;
        rect.sizeDelta = sizeDelta; rect.anchoredPosition = anchoredPos;

        Image panelImg = panelObj.AddComponent<Image>();
        panelImg.sprite = aeroSprite;
        panelImg.type = Image.Type.Sliced; // Use 9-slicing if it was a real sprite!

        // Title Bar Area (Virtual)
        GameObject titleObj = new GameObject("TitleText");
        titleObj.transform.SetParent(panelObj.transform, false);
        Text titleText = titleObj.AddComponent<Text>();
        titleText.text = "  " + title;
        titleText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        titleText.fontSize = 16;
        titleText.fontStyle = FontStyle.Bold;
        titleText.color = new Color(0.1f, 0.1f, 0.1f, 1f); // Dark text
        titleText.alignment = TextAnchor.MiddleLeft;
        RectTransform titleRect = titleObj.GetComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0, 1); titleRect.anchorMax = new Vector2(1, 1);
        titleRect.sizeDelta = new Vector2(0, 25); titleRect.anchoredPosition = new Vector2(0, -12.5f);

        // Close Button
        GameObject closeBtn = new GameObject("CloseButton");
        closeBtn.transform.SetParent(panelObj.transform, false);
        Image cbImg = closeBtn.AddComponent<Image>();
        cbImg.sprite = closeBtnSprite;
        RectTransform cbRect = closeBtn.GetComponent<RectTransform>();
        cbRect.anchorMin = new Vector2(1, 1); cbRect.anchorMax = new Vector2(1, 1);
        cbRect.sizeDelta = new Vector2(25, 20); cbRect.anchoredPosition = new Vector2(-15, -12.5f);

        // Content Area (White background)
        GameObject contentObj = new GameObject("ContentArea");
        contentObj.transform.SetParent(panelObj.transform, false);
        Image contentImg = contentObj.AddComponent<Image>();
        contentImg.color = neonCyan; // White
        RectTransform contentRect = contentObj.GetComponent<RectTransform>();
        contentRect.anchorMin = Vector2.zero; contentRect.anchorMax = Vector2.one;
        contentRect.offsetMin = new Vector2(5, 5); contentRect.offsetMax = new Vector2(-5, -25);

        return panelObj;
    }

    private static GameObject CreateTopMenuBar(GameObject parent)
    {
        Sprite tbSprite = GenerateUISprite("Win7_Taskbar", new Color(0.4f, 0.6f, 0.8f, 0.9f), 64, false);
        Sprite startBtnSprite = GenerateUISprite("Win7_StartBtn", new Color(0.2f, 0.8f, 0.2f, 1f), 64, true);

        GameObject taskbar = new GameObject("Taskbar");
        taskbar.transform.SetParent(parent.transform, false);
        Image tbImg = taskbar.AddComponent<Image>();
        tbImg.sprite = tbSprite;
        RectTransform tbRect = taskbar.GetComponent<RectTransform>();
        tbRect.anchorMin = Vector2.zero;
        tbRect.anchorMax = new Vector2(1, 0);
        tbRect.sizeDelta = new Vector2(0, 50);
        tbRect.anchoredPosition = new Vector2(0, 25);

        GameObject startBtn = new GameObject("StartButton");
        startBtn.transform.SetParent(taskbar.transform, false);
        Image startImg = startBtn.AddComponent<Image>();
        startImg.sprite = startBtnSprite;
        RectTransform startRect = startBtn.GetComponent<RectTransform>();
        startRect.anchorMin = new Vector2(0, 0.5f); startRect.anchorMax = new Vector2(0, 0.5f);
        startRect.sizeDelta = new Vector2(40, 40); startRect.anchoredPosition = new Vector2(30, 0);
        
        return taskbar;
    }

    private static GameObject CreateDropZone(GameObject parent, string name, bool isSafe, string labelText, float xOffset)
    {
        Sprite safeZoneSprite = GenerateUISprite("Win7_IconSafe", new Color(0.2f, 0.8f, 0.2f, 0.5f), 64, true);
        Sprite dangerZoneSprite = GenerateUISprite("Win7_IconDanger", new Color(0.8f, 0.2f, 0.2f, 0.5f), 64, true);

        GameObject zoneObj = new GameObject(name);
        zoneObj.transform.SetParent(parent.transform, false);

        RectTransform rect = zoneObj.AddComponent<RectTransform>();
        rect.sizeDelta = new Vector2(64, 64);
        rect.anchorMin = new Vector2(0f, 1f); // Anchor to top-left desktop!
        rect.anchorMax = new Vector2(0f, 1f);
        rect.anchoredPosition = new Vector2(50 - (xOffset / 2.5f), -50); // spacing

        Image image = zoneObj.AddComponent<Image>();
        image.sprite = isSafe ? safeZoneSprite : dangerZoneSprite;

        DropZone dropZone = zoneObj.AddComponent<DropZone>();
        var isSafeField = typeof(DropZone).GetField("isSafeZone", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (isSafeField != null) isSafeField.SetValue(dropZone, isSafe);
        var highlightField = typeof(DropZone).GetField("zoneHighlight", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (highlightField != null) highlightField.SetValue(dropZone, image);

        GameObject textObj = new GameObject("Label");
        textObj.transform.SetParent(zoneObj.transform, false);
        Text text = textObj.AddComponent<Text>();
        text.text = labelText;
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = 12;
        text.color = Color.white;
        text.alignment = TextAnchor.MiddleCenter;
        Outline outline = textObj.AddComponent<Outline>();
        outline.effectColor = Color.black;
        
        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.sizeDelta = new Vector2(120, 40);
        textRect.anchoredPosition = new Vector2(0, -50);

        return zoneObj;
    }

    private static GameObject CreateSystemStatusPanel(GameObject parent)
    {
        GameObject panelObj = CreateCyberPanel(parent.transform, "SystemStatusPanel", 
            new Vector2(0.05f, 0.5f), new Vector2(0.25f, 0.85f), 
            Vector2.zero, Vector2.zero, "SYSTEM STATUS");

        return panelObj;
    }

    private static GameObject CreateNeonHealthBar(GameObject parent)
    {
        GameObject healthBarObj = new GameObject("HealthBar");
        healthBarObj.transform.SetParent(parent.transform, false);
        RectTransform rect = healthBarObj.AddComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(0, -60);
        rect.sizeDelta = new Vector2(0, 30);
        rect.anchorMin = new Vector2(0.1f, 0.5f);
        rect.anchorMax = new Vector2(0.9f, 0.5f);

        Image bgImage = healthBarObj.AddComponent<Image>();
        bgImage.color = new Color(0.1f, 0.1f, 0.15f, 1f);

        Slider slider = healthBarObj.AddComponent<Slider>();
        slider.minValue = 0;
        slider.maxValue = 100;
        slider.value = 100;

        GameObject fillAreaObj = new GameObject("Fill Area");
        fillAreaObj.transform.SetParent(healthBarObj.transform, false);
        RectTransform fillAreaRect = fillAreaObj.AddComponent<RectTransform>();
        fillAreaRect.anchorMin = Vector2.zero;
        fillAreaRect.anchorMax = Vector2.one;
        fillAreaRect.offsetMin = new Vector2(2, 0);
        fillAreaRect.offsetMax = new Vector2(-2, 0);

        GameObject fillObj = new GameObject("Fill");
        fillObj.transform.SetParent(fillAreaObj.transform, false);
        Image fillImage = fillObj.AddComponent<Image>();
        fillImage.color = neonGreen; 
        RectTransform fillRect = fillObj.GetComponent<RectTransform>();
        fillRect.anchorMin = Vector2.zero;
        fillRect.anchorMax = Vector2.one;
        fillRect.offsetMin = Vector2.zero;
        fillRect.offsetMax = Vector2.zero;

        slider.fillRect = fillRect;

        // Add Active text
        GameObject activeTextObj = new GameObject("ActiveText");
        activeTextObj.transform.SetParent(healthBarObj.transform, false);
        Text activeText = activeTextObj.AddComponent<Text>();
        activeText.text = "INTEGRITY:";
        activeText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        activeText.fontSize = 14;
        activeText.fontStyle = FontStyle.Bold;
        activeText.color = neonCyan;
        activeText.alignment = TextAnchor.MiddleLeft;
        RectTransform atRect = activeTextObj.GetComponent<RectTransform>();
        atRect.anchorMin = new Vector2(0,1);
        atRect.anchorMax = new Vector2(0,1);
        atRect.sizeDelta = new Vector2(150, 30);
        atRect.anchoredPosition = new Vector2(0, 30);

        return healthBarObj;
    }

    private static GameObject CreateThreatMapPanel(GameObject parent)
    {
        GameObject panelObj = CreateCyberPanel(parent.transform, "ThreatMapPanel", 
            new Vector2(0.75f, 0.5f), new Vector2(0.95f, 0.85f), 
            Vector2.zero, Vector2.zero, "CURRENT THREAT MAP");
            
        var animator = panelObj.AddComponent<CyberPanelAnimator>();
        animator.panelType = CyberPanelAnimator.PanelType.ThreatMap;
        
        return panelObj;
    }

    private static GameObject CreateSystemLogPanel(GameObject parent)
    {
        GameObject logPanel = CreateCyberPanel(parent.transform, "SystemLogPanel", 
            new Vector2(0.65f, 0.05f), new Vector2(0.95f, 0.25f), 
            Vector2.zero, Vector2.zero, "SYSTEM LOG");
        
        GameObject textObj = new GameObject("LogContent");
        textObj.transform.SetParent(logPanel.transform, false);
        Text text = textObj.AddComponent<Text>();
        text.text = "> SCANNING PATH: C:\\SYSTEM\\BOOT\n> STATUS: SECURE...\n> AWAITING INPUT...";
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = 12;
        text.color = neonGreen;
        text.alignment = TextAnchor.LowerLeft;
        
        RectTransform tRect = textObj.GetComponent<RectTransform>();
        tRect.anchorMin = Vector2.zero;
        tRect.anchorMax = Vector2.one;
        tRect.offsetMin = new Vector2(20, 20);
        tRect.offsetMax = new Vector2(-20, -40);

        var animator = logPanel.AddComponent<CyberPanelAnimator>();
        animator.panelType = CyberPanelAnimator.PanelType.SystemLog;

        return logPanel;
    }

    private static GameObject CreateUserProfilePanel(GameObject parent)
    {
        GameObject panelObj = CreateCyberPanel(parent.transform, "UserProfilePanel", 
            new Vector2(0.05f, 0.05f), new Vector2(0.2f, 0.25f), 
            Vector2.zero, Vector2.zero, "USER PROFILE");
            
        var animator = panelObj.AddComponent<CyberPanelAnimator>();
        animator.panelType = CyberPanelAnimator.PanelType.UserProfile;
        
        return panelObj;
    }

    private static GameObject CreateItemSpawnAreaGrid(GameObject parent)
    {
        GameObject spawnObj = new GameObject("ItemSpawnArea");
        spawnObj.transform.SetParent(parent.transform, false);

        RectTransform rect = spawnObj.AddComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(0, -50);
        rect.sizeDelta = new Vector2(300, 300);
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);

        Image image = spawnObj.AddComponent<Image>();
        image.color = new Color(0f, 0.5f, 1f, 0.05f); // very light center
        
        // Add corner brackets aesthetic
        Outline outline = spawnObj.AddComponent<Outline>();
        outline.effectColor = neonCyan;
        outline.effectDistance = new Vector2(1, -1);
        
        GameObject labelObj = new GameObject("SpawnLabel");
        labelObj.transform.SetParent(spawnObj.transform, false);
        Text labelText = labelObj.AddComponent<Text>();
        labelText.text = "SCAN TARGET AREA";
        labelText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        labelText.fontSize = 14;
        labelText.color = new Color(0, 0.9f, 1f, 0.5f);
        labelText.alignment = TextAnchor.MiddleCenter;
        RectTransform lblRect = labelObj.GetComponent<RectTransform>();
        lblRect.anchorMin = Vector2.zero;
        lblRect.anchorMax = Vector2.one;
        lblRect.offsetMin = Vector2.zero;
        lblRect.offsetMax = Vector2.zero;

        return spawnObj;
    }

    private static GameObject CreateProfessionalFeedbackPanel(GameObject parent)
    {
        GameObject windowObj = CreateCyberPanel(parent.transform, "FeedbackPanel", new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(500, 250), Vector2.zero, "Windows Security - Scan Result");
        CanvasGroup cg = windowObj.AddComponent<CanvasGroup>();
        cg.alpha = 0;
        
        GameObject contentArea = windowObj.transform.Find("ContentArea").gameObject;

        GameObject nameObj = new GameObject("ItemNameText");
        nameObj.transform.SetParent(contentArea.transform, false);
        Text nameText = nameObj.AddComponent<Text>();
        nameText.text = "TARGET: UNKNOWN";
        nameText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        nameText.fontSize = 20;
        nameText.fontStyle = FontStyle.Bold;
        nameText.alignment = TextAnchor.UpperCenter;
        nameText.color = Color.black;
        RectTransform nameRect = nameObj.GetComponent<RectTransform>();
        nameRect.anchorMin = new Vector2(0, 1f);
        nameRect.anchorMax = new Vector2(1, 1f);
        nameRect.sizeDelta = new Vector2(0, 40);
        nameRect.anchoredPosition = new Vector2(0, -20);

        GameObject explainObj = new GameObject("ExplanationText");
        explainObj.transform.SetParent(contentArea.transform, false);
        Text explainText = explainObj.AddComponent<Text>();
        explainText.text = "Processing data...";
        explainText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        explainText.fontSize = 16;
        explainText.alignment = TextAnchor.MiddleCenter;
        explainText.color = Color.black;
        RectTransform explainRect = explainObj.GetComponent<RectTransform>();
        explainRect.anchorMin = Vector2.zero;
        explainRect.anchorMax = Vector2.one;
        explainRect.offsetMin = new Vector2(40, 60);
        explainRect.offsetMax = new Vector2(-20, -50);

        GameObject indicatorObj = new GameObject("CorrectnessIndicator");
        indicatorObj.transform.SetParent(contentArea.transform, false);
        Image indicatorImage = indicatorObj.AddComponent<Image>();
        indicatorImage.color = new Color(0.2f, 0.8f, 0.2f, 1f);
        RectTransform indicatorRect = indicatorObj.GetComponent<RectTransform>();
        indicatorRect.anchorMin = new Vector2(0, 0);
        indicatorRect.anchorMax = new Vector2(1, 0);
        indicatorRect.sizeDelta = new Vector2(0, 30);
        indicatorRect.anchoredPosition = new Vector2(0, 20);

        return windowObj;
    }

    private static GameObject CreateNeonScoreDisplay(GameObject parent)
    {
        GameObject scoreObj = new GameObject("ScoreDisplay");
        scoreObj.transform.SetParent(parent.transform, false);
        RectTransform rect = scoreObj.AddComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(-50, 0);
        rect.sizeDelta = new Vector2(200, 50);
        rect.anchorMin = new Vector2(1f, 0.5f);
        rect.anchorMax = new Vector2(1f, 0.5f);

        Text text = scoreObj.AddComponent<Text>();
        text.text = "SECURE DATABLOCKS: 0";
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = 16;
        text.fontStyle = FontStyle.Bold;
        text.alignment = TextAnchor.MiddleRight;
        text.color = neonCyan;
        return scoreObj;
    }

    private static GameObject CreateFlashOverlay(GameObject canvas, string name, Color color)
    {
        GameObject overlayObj = new GameObject(name);
        overlayObj.transform.SetParent(canvas.transform, false);
        RectTransform rect = overlayObj.AddComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        Image image = overlayObj.AddComponent<Image>();
        image.color = color;
        overlayObj.SetActive(false);
        return overlayObj;
    }

    private static Sprite GenerateUISprite(string name, Color color, int size, bool outline)
    {
        string dir = "Assets/Sprites/Level3_UI";
        if (!System.IO.Directory.Exists(dir)) System.IO.Directory.CreateDirectory(dir);
        string path = dir + "/" + name + ".png";

        if (System.IO.File.Exists(path)) return AssetDatabase.LoadAssetAtPath<Sprite>(path);

        Texture2D tex = new Texture2D(size, size);
        for(int x=0; x<size; x++) {
            for(int y=0; y<size; y++) {
                if (outline && (x < 2 || x > size - 3 || y < 2 || y > size - 3)) tex.SetPixel(x, y, new Color(1, 1, 1, 0.4f)); 
                else tex.SetPixel(x, y, color);
            }
        }
        tex.Apply();
        System.IO.File.WriteAllBytes(path, tex.EncodeToPNG());
        AssetDatabase.ImportAsset(path);

        TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
        if (importer != null) {
            importer.textureType = TextureImporterType.Sprite;
            importer.SaveAndReimport();
        }
        return AssetDatabase.LoadAssetAtPath<Sprite>(path);
    }

    private static Sprite CreateDynamicFileSprite(string name, Color color)
    {
        string dir = "Assets/Sprites/AutoIcons";
        if (!System.IO.Directory.Exists(dir)) System.IO.Directory.CreateDirectory(dir);
        string path = dir + "/" + name + ".png";

        if (System.IO.File.Exists(path))
        {
            return AssetDatabase.LoadAssetAtPath<Sprite>(path);
        }

        Texture2D tex = new Texture2D(128, 128);
        for(int x=0; x<128; x++) {
            for(int y=0; y<128; y++) {
                // simple box with internal border
                if (x < 10 || x > 117 || y < 10 || y > 117) tex.SetPixel(x, y, Color.white);
                else if (y > 90) tex.SetPixel(x, y, new Color(color.r * 1.5f, color.g * 1.5f, color.b * 1.5f)); // file header
                else tex.SetPixel(x, y, color);
            }
        }
        tex.Apply();
        System.IO.File.WriteAllBytes(path, tex.EncodeToPNG());
        AssetDatabase.ImportAsset(path);

        TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
        if (importer != null) {
            importer.textureType = TextureImporterType.Sprite;
            importer.SaveAndReimport();
        }
        
        return AssetDatabase.LoadAssetAtPath<Sprite>(path);
    }

    private static GameObject CreateProfessionalGameOverPanel(GameObject canvas)
    {
        GameObject panelObj = new GameObject("GameOverPanel");
        panelObj.transform.SetParent(canvas.transform, false);
        RectTransform rect = panelObj.AddComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        Image image = panelObj.AddComponent<Image>();
        image.color = new Color(0.1f, 0f, 0f, 0.9f);

        GameObject titleObj = new GameObject("DeviceInfectedText");
        titleObj.transform.SetParent(panelObj.transform, false);
        Text titleText = titleObj.AddComponent<Text>();
        titleText.text = "CRITICAL BREACH DETECTED";
        titleText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        titleText.fontSize = 64;
        titleText.fontStyle = FontStyle.Bold;
        titleText.alignment = TextAnchor.MiddleCenter;
        titleText.color = neonRed;
        Outline outline = titleObj.AddComponent<Outline>();
        outline.effectColor = Color.black;
        RectTransform titleRect = titleObj.GetComponent<RectTransform>();
        titleRect.anchoredPosition = new Vector2(0, 100);
        titleRect.sizeDelta = new Vector2(1200, 150);

        CreateNeonButton(panelObj, "RetryButton", "REINITIALIZE SYSTEM", new Vector2(0, -100));

        panelObj.SetActive(false);
        return panelObj;
    }

    private static GameObject CreateProfessionalCompletionPanel(GameObject canvas)
    {
        GameObject panelObj = new GameObject("CompletionPanel");
        panelObj.transform.SetParent(canvas.transform, false);
        RectTransform rect = panelObj.AddComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        Image image = panelObj.AddComponent<Image>();
        image.color = new Color(0f, 0.1f, 0f, 0.9f);

        GameObject titleObj = new GameObject("LevelCompleteText");
        titleObj.transform.SetParent(panelObj.transform, false);
        Text titleText = titleObj.AddComponent<Text>();
        titleText.text = "SYSTEM SECURED";
        titleText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        titleText.fontSize = 64;
        titleText.fontStyle = FontStyle.Bold;
        titleText.alignment = TextAnchor.MiddleCenter;
        titleText.color = neonGreen;
        RectTransform titleRect = titleObj.GetComponent<RectTransform>();
        titleRect.anchoredPosition = new Vector2(0, 150);
        titleRect.sizeDelta = new Vector2(1000, 150);

        GameObject scoreObj = new GameObject("ScoreSummaryText");
        scoreObj.transform.SetParent(panelObj.transform, false);
        Text scoreText = scoreObj.AddComponent<Text>();
        scoreText.text = "EFFICIENCY RATING: 100%";
        scoreText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        scoreText.fontSize = 32;
        scoreText.alignment = TextAnchor.MiddleCenter;
        scoreText.color = neonCyan;
        RectTransform scoreRect = scoreObj.GetComponent<RectTransform>();
        scoreRect.anchoredPosition = new Vector2(0, 50);
        scoreRect.sizeDelta = new Vector2(800, 60);

        CreateNeonButton(panelObj, "NextButton", "DEPLOY NEXT SYSTEM", new Vector2(0, -100));

        panelObj.SetActive(false);
        return panelObj;
    }

    private static GameObject CreateLevel3TutorialPanel(GameObject canvas)
    {
        GameObject panelObj = new GameObject("TutorialPanel");
        panelObj.transform.SetParent(canvas.transform, false);
        RectTransform rect = panelObj.AddComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        Image image = panelObj.AddComponent<Image>();
        image.color = new Color(0f, 0.05f, 0.1f, 0.95f);

        GameObject titleObj = new GameObject("TitleText");
        titleObj.transform.SetParent(panelObj.transform, false);
        Text titleText = titleObj.AddComponent<Text>();
        titleText.text = "LEVEL 3 TUTORIAL";
        titleText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        titleText.fontSize = 48;
        titleText.alignment = TextAnchor.MiddleCenter;
        titleText.color = neonCyan;
        RectTransform titleRect = titleObj.GetComponent<RectTransform>();
        titleRect.anchoredPosition = new Vector2(0, 200);
        titleRect.sizeDelta = new Vector2(1000, 100);

        GameObject bodyObj = new GameObject("BodyText");
        bodyObj.transform.SetParent(panelObj.transform, false);
        Text bodyText = bodyObj.AddComponent<Text>();
        bodyText.text = "Loading systems...";
        bodyText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        bodyText.fontSize = 24;
        bodyText.alignment = TextAnchor.MiddleCenter;
        bodyText.color = Color.white;
        RectTransform bodyRect = bodyObj.GetComponent<RectTransform>();
        bodyRect.anchoredPosition = new Vector2(0, 0);
        bodyRect.sizeDelta = new Vector2(800, 300);

        CreateNeonButton(panelObj, "NextButton", "CONTINUE", new Vector2(0, -200));

        panelObj.SetActive(false);
        return panelObj;
    }

    private static GameObject CreateNeonButton(GameObject parent, string name, string label, Vector2 position)
    {
        GameObject buttonObj = new GameObject(name);
        buttonObj.transform.SetParent(parent.transform, false);
        RectTransform rect = buttonObj.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = position;
        rect.sizeDelta = new Vector2(300, 60);

        Image image = buttonObj.AddComponent<Image>();
        image.color = panelBgColor;
        Outline outline = buttonObj.AddComponent<Outline>();
        outline.effectColor = neonCyan;

        buttonObj.AddComponent<Button>();

        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(buttonObj.transform, false);
        Text text = textObj.AddComponent<Text>();
        text.text = label;
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = 20;
        text.fontStyle = FontStyle.Bold;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = neonCyan;
        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        return buttonObj;
    }

    private static void DisplaySuccessMessage()
    {
        EditorUtility.DisplayDialog("Success", 
            "Professional Cyberpunk UI generated successfully!\n\n" +
            "✓ Added Dark Node-style Theme\n" +
            "✓ Added System Status Panel\n" +
            "✓ Added Threat Map Panel\n" +
            "✓ Created Top Menu Navigation Bar\n" +
            "✓ Redesigned Spawn Area & Drop Zones\n\n" +
            "Don't forget to wire up the Drag-and-Drop DropZone references!", "OK");
    }
}
