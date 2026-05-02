using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class CyberPanelAnimator : MonoBehaviour, IPointerClickHandler
{
    public enum PanelType { ThreatMap, SystemLog, UserProfile }
    public PanelType panelType;

    public GameObject profileDetailPanel; // Assign via Inspector or script

    // For Profile Toggling
    private RectTransform panelRect;
    private Vector2 originalPosition;
    private bool isProfileMinimized = false;
    private GameObject contentArea;
    private GameObject toggleButton;

    // For System Log
    private Text logText;
    private string[] fakeLogs = new string[] {
        "> ROUTING PACKETS...",
        "> FIREWALL ONLINE",
        "> SCANNING SECTOR 7G",
        "> NO THREATS DETECTED",
        "> UPDATING DEFINITIONS...",
        "> DECRYPTING PACKET...",
        "> ANOMALY FOUND!",
        "> INTRUSION COUNTERMEASURES ACTIVE"
    };

    // For Threat Map
    private Image mapImage;
    private Color normalColor = new Color(0.05f, 0.2f, 0.2f, 0.8f);
    private Color alertColor = new Color(0.3f, 0.05f, 0.05f, 0.8f);

    void Start()
    {
        if (panelType == PanelType.SystemLog)
        {
            // Find text component down hierarchy
            Text[] texts = GetComponentsInChildren<Text>();
            if (texts.Length > 1) { logText = texts[1]; } // 0 is title, 1 is content
            if (logText != null) StartCoroutine(AnimateSystemLog());
        }
        else if (panelType == PanelType.ThreatMap)
        {
            mapImage = GetComponent<Image>();
            StartCoroutine(AnimateThreatMap());
        }
        else if (panelType == PanelType.UserProfile)
        {
            panelRect = GetComponent<RectTransform>();
            originalPosition = panelRect.anchoredPosition;
            
            // Find reference objects
            Transform contentTrans = transform.Find("ContentArea");
            if (contentTrans != null) contentArea = contentTrans.gameObject;
            
            StartCoroutine(AnimateUserProfile());
            
            // Auto-setup button listener for the close button
            Button[] buttons = GetComponentsInChildren<Button>(true);
            foreach (Button btn in buttons)
            {
                if (btn.name == "CloseButton")
                {
                    btn.onClick.AddListener(ToggleProfile);
                }
                else if (btn.name == "ToggleButton (1)")
                {
                    toggleButton = btn.gameObject;
                    btn.onClick.AddListener(ToggleMinimize);
                }
            }
        }
    }

    public void ToggleMinimize()
    {
        isProfileMinimized = !isProfileMinimized;

        // Find the background image component to hide only the window body
        Image panelBg = GetComponent<Image>();

        if (isProfileMinimized)
        {
            // Hide content and panel background, keep title bar/buttons
            if (contentArea != null) contentArea.SetActive(false);
            if (panelBg != null) panelBg.enabled = false;
            
            // Move to taskbar area
            // Use 0,0 anchors (bottom left of parent) to ensure it's relative to the bottom of the screen
            panelRect.anchorMin = Vector2.zero;
            panelRect.anchorMax = Vector2.zero;
            panelRect.pivot = new Vector2(0, 0); // Pivot to bottom-left
            
            // 200px from left (near start button), 10px from bottom (inside taskbar)
            panelRect.anchoredPosition = new Vector2(250f, 10f); 
            
            Text btnTxt = toggleButton.GetComponentInChildren<Text>();
            if (btnTxt != null) btnTxt.text = "^";
        }
        else
        {
            // Restore original state (which was centered)
            panelRect.anchorMin = new Vector2(0.5f, 0.5f);
            panelRect.anchorMax = new Vector2(0.5f, 0.5f);
            panelRect.pivot = new Vector2(0.5f, 0.5f);
            
            if (contentArea != null) contentArea.SetActive(true);
            if (panelBg != null) panelBg.enabled = true;
            panelRect.anchoredPosition = originalPosition;

            Text btnTxt = toggleButton.GetComponentInChildren<Text>();
            if (btnTxt != null) btnTxt.text = "v";
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Only trigger detail panel if we didn't click a toggle button
        if (panelType == PanelType.UserProfile)
        {
            if (eventData.pointerEnter != null && 
                (eventData.pointerEnter.name == "CloseButton" || eventData.pointerEnter.name == "ToggleButton (1)"))
                return;

            if (isProfileMinimized) return; // Don't open details if minimized

            if (profileDetailPanel != null)
            {
                profileDetailPanel.SetActive(true);
            }
            else
            {
                Debug.Log("User Profile Clicked! (Detail Panel not assigned)");
            }
        }
    }

    IEnumerator AnimateSystemLog()
    {
        if (logText == null) yield break;
        
        List<string> currentLogs = new List<string>();
        currentLogs.Add("> SYSTEM BOOT SEQ...");

        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0.5f, 2.5f));
            
            currentLogs.Add(fakeLogs[Random.Range(0, fakeLogs.Length)]);
            if (currentLogs.Count > 6) currentLogs.RemoveAt(0); // keep last 6 lines

            logText.text = string.Join("\n", currentLogs);
        }
    }

    IEnumerator AnimateThreatMap()
    {
        if (mapImage == null) yield break;

        while (true)
        {
            yield return new WaitForSeconds(Random.Range(2f, 5f));
            
            // Simulating a radar sweep or threat pulse
            mapImage.color = alertColor;
            yield return new WaitForSeconds(0.2f);
            mapImage.color = normalColor;
            yield return new WaitForSeconds(0.2f);
            mapImage.color = alertColor;
            yield return new WaitForSeconds(0.2f);
            mapImage.color = normalColor;
        }
    }

    IEnumerator AnimateUserProfile()
    {
        // Simple scanning effect or data change
        GameObject txtObj = new GameObject("ProfileData");
        txtObj.transform.SetParent(transform, false);
        Text profText = txtObj.AddComponent<Text>();
        profText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        profText.fontSize = 32; // Increased further from 24
        profText.color = new Color(0f, 0.8f, 1f); // Neon Cyan
        RectTransform rt = profText.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.1f, 0.1f);
        rt.anchorMax = new Vector2(0.9f, 0.9f);
        rt.sizeDelta = Vector2.zero;
        rt.anchoredPosition = new Vector2(0, -20); // space below title

        while (true)
        {
            if (gameObject.activeInHierarchy)
            {
                profText.text = "USER: ADMIN\nAUTH: SECURE\nSESSION ID: " + Random.Range(1000, 9999) + "\nUPLINK: ACTIVE\n" +
                                "SEC LEVEL: " + (Random.Range(0, 100) > 85 ? "<color=red>CRITICAL</color>" : "OK");
            }
            yield return new WaitForSeconds(1.5f);
        }
    }

    // Task: Toggle visibility
    public void ToggleProfile()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
