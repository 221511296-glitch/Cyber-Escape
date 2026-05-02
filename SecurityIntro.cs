using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SecurityIntro : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject panel;
    [SerializeField] private Text messageText;   // Legacy Text
    [SerializeField] private Button closeButton;

    [Header("Settings")]
    [SerializeField] private float flashSpeed = 0.5f;

    [Header("Managers")]
    [SerializeField] private TutorialManager tutorialManager;

    private Coroutine flashCoroutine;

    private void Start()
    {
        if (panel == null) 
        {
            Transform t = transform.Find("Panel");
            panel = t != null ? t.gameObject : this.gameObject;
        }
        if (messageText == null) messageText = GetComponentInChildren<Text>(true);
        if (closeButton == null) closeButton = GetComponentInChildren<Button>(true);

        if (panel == null || messageText == null || closeButton == null)
        {
            Debug.LogWarning("SecurityIntro: Missing UI references even after auto-find! Disabling script.");
            enabled = false;
            return;
        }

        panel.SetActive(true);

        messageText.text =
            "⚠ SECURITY BREACH DETECTED ⚠\n\n" +
            "Weak passwords have caused a security breach.\n\n" +
            "Let’s fix it!\n\n" +
            "Your task:\nIdentify weak passwords and create strong ones.";

        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(ClosePanel);

        flashCoroutine = StartCoroutine(FlashEffect());
    }

    private IEnumerator FlashEffect()
    {
        while (true)
        {
            messageText.color = Color.red;
            yield return new WaitForSeconds(flashSpeed);

            messageText.color = Color.white;
            yield return new WaitForSeconds(flashSpeed);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClosePanel();
        }
    }

    public void ClosePanel()
    {
        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);

        messageText.color = Color.white;
        panel.SetActive(false);
        
        // Removed automatic StartTutorial() call - this is now handled sequentially
        // by MentorInteractionManager.cs orchestrating the flow.
    }
}