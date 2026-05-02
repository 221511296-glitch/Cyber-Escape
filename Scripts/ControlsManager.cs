using UnityEngine;
using UnityEngine.UI;

public class ControlsManager : MonoBehaviour
{
    public GameObject instructionsContainer;
    private bool isManuallyHidden = false;

    private const string INSTRUCTIONS_KEY = "ShowInstructions";
    private Text toggleButtonText;
    private RectTransform toggleBtnRect;

    private Vector2 originalAnchorMin;
    private Vector2 originalAnchorMax;
    private Vector2 originalAnchoredPosition;
    private Vector2 originalSizeDelta;
    private Vector3 originalPosition;

    void Start()
    {
        // Find the toggle button and bind the click event at runtime
        Button toggleBtn = GetComponentInChildren<Button>();
        if (toggleBtn != null)
        {
            toggleBtnRect = toggleBtn.GetComponent<RectTransform>();
            toggleBtn.onClick.AddListener(ToggleManualVisibility);
            toggleButtonText = toggleBtn.GetComponentInChildren<Text>();

            originalAnchorMin = toggleBtnRect.anchorMin;
            originalAnchorMax = toggleBtnRect.anchorMax;
            originalAnchoredPosition = toggleBtnRect.anchoredPosition;
            originalSizeDelta = toggleBtnRect.sizeDelta;
            originalPosition = toggleBtnRect.position;
        }
        
        UpdateVisibilityFromSettings();
    }

    // Called when the manual toggle button is clicked on the UI
    public void ToggleManualVisibility()
    {
        isManuallyHidden = !isManuallyHidden;
        Debug.Log("Controls UI Toggle Clicked! isManuallyHidden is now: " + isManuallyHidden);
        UpdateContainerVisibility();
    }

    // Called when the global settings checkbox is changed
    public void UpdateVisibilityFromSettings()
    {
        bool settingsEnabled = PlayerPrefs.GetInt(INSTRUCTIONS_KEY, 1) == 1;

        // Rather than hiding the entire GameObject (which could accidentally hide the Main Menu if attached wrong),
        // we just hide the instructions container and the toggle button themselves.
        if (instructionsContainer != null)
        {
            instructionsContainer.SetActive(settingsEnabled && !isManuallyHidden);
        }

        if (toggleBtnRect != null && toggleBtnRect.gameObject != null)
        {
            toggleBtnRect.gameObject.SetActive(settingsEnabled);
        }
    }

    private void UpdateContainerVisibility()
    {
        bool settingsEnabled = PlayerPrefs.GetInt(INSTRUCTIONS_KEY, 1) == 1;

        if (instructionsContainer != null)
        {
            instructionsContainer.SetActive(settingsEnabled && !isManuallyHidden);
        }

        // Move the button to the edge of the screen when hidden, return to original otherwise
        if (toggleBtnRect != null)
        {
            if (isManuallyHidden)
            {
                Canvas canvas = GetComponentInParent<Canvas>();
                if (canvas != null)
                {
                    RectTransform canvasRect = canvas.GetComponent<RectTransform>();
                    Vector3[] canvasCorners = new Vector3[4];
                    canvasRect.GetWorldCorners(canvasCorners);
                    
                    Vector3[] btnCorners = new Vector3[4];
                    toggleBtnRect.GetWorldCorners(btnCorners);
                    Vector3 btnCenter = (btnCorners[0] + btnCorners[2]) / 2f;
                    
                    float btnWidth = Vector3.Distance(btnCorners[0], btnCorners[3]);
                    float btnHeight = Vector3.Distance(btnCorners[0], btnCorners[1]);

                    Vector3 canvasCenter = (canvasCorners[0] + canvasCorners[2]) / 2f;
                    Vector3 dir = btnCenter - canvasCenter;

                    float canvasWidth = Vector3.Distance(canvasCorners[0], canvasCorners[3]);
                    float canvasHeight = Vector3.Distance(canvasCorners[0], canvasCorners[1]);
                    
                    // Normalize to find whether it's closer horizontally or vertically to the edge
                    float normalizedX = dir.x / canvasWidth;
                    float normalizedY = dir.y / canvasHeight;

                    Vector3 targetPos = originalPosition;
                    string arrowText = "";

                    // If it's placed further horizontally than vertically (e.g. near the right edge)
                    if (Mathf.Abs(normalizedX) > Mathf.Abs(normalizedY))
                    {
                        // Horizontal snap
                        if (normalizedX > 0)
                        {
                            targetPos.x = canvasCorners[3].x - (btnWidth / 2f); // Snap Right (Index 3 is Bottom-Right)
                            arrowText = "<"; // Point left to open
                        }
                        else
                        {
                            targetPos.x = canvasCorners[0].x + (btnWidth / 2f); // Snap Left (Index 0 is Bottom-Left)
                            arrowText = ">"; // Point right to open
                        }
                    }
                    else
                    {
                        // Vertical snap
                        if (normalizedY > 0)
                        {
                            targetPos.y = canvasCorners[1].y - (btnHeight / 2f); // Snap Top (Index 1 is Top-Left)
                            arrowText = "v"; // Point down to open
                        }
                        else
                        {
                            targetPos.y = canvasCorners[0].y + (btnHeight / 2f); // Snap Bottom (Index 0 is Bottom-Left)
                            arrowText = "^"; // Point up to open
                        }
                    }

                    // Move object
                    toggleBtnRect.position = targetPos;

                    if (toggleButtonText != null)
                    {
                        toggleButtonText.text = arrowText;
                    }
                }
            }
            else
            {
                // Restore original position
                toggleBtnRect.anchorMin = originalAnchorMin;
                toggleBtnRect.anchorMax = originalAnchorMax;
                toggleBtnRect.sizeDelta = originalSizeDelta;
                toggleBtnRect.anchoredPosition = originalAnchoredPosition;
                toggleBtnRect.position = originalPosition;

                // Restore dynamic text
                if (toggleButtonText != null)
                {
                    Canvas canvas = GetComponentInParent<Canvas>();
                    if (canvas != null)
                    {
                        Vector3[] canvasCorners = new Vector3[4];
                        canvas.GetComponent<RectTransform>().GetWorldCorners(canvasCorners);
                        Vector3 canvasCenter = (canvasCorners[0] + canvasCorners[2]) / 2f;
                        Vector3 dir = originalPosition - canvasCenter;

                        float canvasWidth = Vector3.Distance(canvasCorners[0], canvasCorners[3]);
                        float canvasHeight = Vector3.Distance(canvasCorners[0], canvasCorners[1]);
                        
                        float normalizedX = dir.x / canvasWidth;
                        float normalizedY = dir.y / canvasHeight;

                        if (Mathf.Abs(normalizedX) > Mathf.Abs(normalizedY))
                        {
                            toggleButtonText.text = normalizedX > 0 ? ">" : "<";
                        }
                        else
                        {
                            toggleButtonText.text = normalizedY > 0 ? "^" : "v";
                        }
                    }
                }
            }
        }
    }
}
