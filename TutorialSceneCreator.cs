using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class TutorialSceneCreator : EditorWindow
{
    [MenuItem("Window/Cyber Escape Game/Create Level 3 Tutorial Scene")]
    public static void CreateTutorialScene()
    {
        // 1. Create a new scene
        Scene tutorialScene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
        tutorialScene.name = "Level3Tutorial";

        // 2. Create Canvas
        GameObject canvasObj = new GameObject("Canvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();

        // 3. Create Background
        GameObject bgObj = new GameObject("Background");
        bgObj.transform.SetParent(canvasObj.transform, false);
        Image bgImg = bgObj.AddComponent<Image>();
        bgImg.color = new Color(0f, 0.05f, 0.1f, 1f);
        RectTransform bgRect = bgObj.GetComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero; bgRect.anchorMax = Vector2.one;
        bgRect.offsetMin = Vector2.zero; bgRect.offsetMax = Vector2.zero;

        // 4. Create Tutorial Panel
        GameObject panelObj = new GameObject("TutorialPanel");
        panelObj.transform.SetParent(canvasObj.transform, false);
        RectTransform panelRect = panelObj.AddComponent<RectTransform>();
        panelRect.anchorMin = Vector2.zero; panelRect.anchorMax = Vector2.one;
        panelRect.offsetMin = Vector2.zero; panelRect.offsetMax = Vector2.zero;

        // 5. Create Title Text
        GameObject titleObj = new GameObject("TitleText");
        titleObj.transform.SetParent(panelObj.transform, false);
        Text titleText = titleObj.AddComponent<Text>();
        titleText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        titleText.fontSize = 50;
        titleText.alignment = TextAnchor.MiddleCenter;
        titleText.color = new Color(0f, 0.9f, 1f); // Cyan
        RectTransform titleRect = titleObj.GetComponent<RectTransform>();
        titleRect.anchoredPosition = new Vector2(0, 200);
        titleRect.sizeDelta = new Vector2(800, 100);

        // 6. Create Body Text
        GameObject bodyObj = new GameObject("BodyText");
        bodyObj.transform.SetParent(panelObj.transform, false);
        Text bodyText = bodyObj.AddComponent<Text>();
        bodyText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        bodyText.fontSize = 28;
        bodyText.alignment = TextAnchor.MiddleCenter;
        bodyText.color = Color.white;
        RectTransform bodyRect = bodyObj.GetComponent<RectTransform>();
        bodyRect.anchoredPosition = new Vector2(0, 0);
        bodyRect.sizeDelta = new Vector2(800, 400);

        // 7. Create Next Button
        GameObject buttonObj = new GameObject("NextButton");
        buttonObj.transform.SetParent(panelObj.transform, false);
        Image btnImg = buttonObj.AddComponent<Image>();
        btnImg.color = new Color(0.1f, 0.3f, 0.5f, 1f);
        Button btn = buttonObj.AddComponent<Button>();
        RectTransform btnRect = buttonObj.GetComponent<RectTransform>();
        btnRect.anchoredPosition = new Vector2(0, -250);
        btnRect.sizeDelta = new Vector2(250, 60);

        GameObject btnTextObj = new GameObject("Text");
        btnTextObj.transform.SetParent(buttonObj.transform, false);
        Text btnText = btnTextObj.AddComponent<Text>();
        btnText.text = "CONTINUE";
        btnText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        btnText.fontSize = 24;
        btnText.alignment = TextAnchor.MiddleCenter;
        btnText.color = Color.white;
        RectTransform btRect = btnTextObj.GetComponent<RectTransform>();
        btRect.anchorMin = Vector2.zero; btRect.anchorMax = Vector2.one;
        btRect.sizeDelta = Vector2.zero;

        // 8. Create Manager
        GameObject managerObj = new GameObject("TutorialManager");
        Level3TutorialManager manager = managerObj.AddComponent<Level3TutorialManager>();
        manager.tutorialPanel = panelObj;
        manager.titleText = titleText;
        manager.bodyText = bodyText;
        manager.nextButton = btn;

        // EXTRA FIX: Find any EventSystem in the scene, or create one if missing
        if (UnityEngine.Object.FindObjectOfType<EventSystem>() == null)
        {
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
        }

        // 9. Save Scene
        string scenePath = "Assets/Scenes/Level3Tutorial.unity";
        EditorSceneManager.SaveScene(tutorialScene, scenePath);

        EditorUtility.DisplayDialog("Success", "Level 3 Tutorial Scene created at " + scenePath, "OK");
    }

    private static void AddSceneToBuildSettings(string scenePath)
    {
        EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
        foreach (var scene in scenes)
        {
            if (scene.path == scenePath) return;
        }

        EditorBuildSettingsScene[] newScenes = new EditorBuildSettingsScene[scenes.Length + 1];
        System.Array.Copy(scenes, newScenes, scenes.Length);
        newScenes[scenes.Length] = new EditorBuildSettingsScene(scenePath, true);
        EditorBuildSettings.scenes = newScenes;
    }
}
