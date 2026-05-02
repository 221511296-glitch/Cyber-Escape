using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject tutorialPanel;

    [Header("UI Elements")]
    public Text titleText;
    public Text bodyText;
    public Button nextButton;

    private int tutorialStep = 0;

    void Start()
    {
        if (tutorialPanel == null)
        {
            GameObject[] allObs = Resources.FindObjectsOfTypeAll<GameObject>();
            foreach (GameObject obj in allObs)
            {
                if ((obj.name.Contains("TutorialPanel") || obj.name.Contains("Tutorial Panel")) && obj.scene.isLoaded)
                {
                    tutorialPanel = obj;
                    break;
                }
            }
        }

        if (tutorialPanel != null)
            tutorialPanel.SetActive(false);
            
        if (nextButton != null)
            nextButton.onClick.AddListener(NextStep);
    }

    // Call this when security panel closes
    public void StartTutorial()
    {
        if (tutorialPanel != null)
            tutorialPanel.SetActive(true);
        tutorialStep = 0;
        ShowStep();
    }
    // // In SecurityIntro.cs
    // public void ClosePanel()
    // {
    //     if (flashCoroutine != null)
    //         StopCoroutine(flashCoroutine);

    //     messageText.color = Color.white;
    //     panel.SetActive(false);

    //     // Load the Office level after the training intro
    //     UnityEngine.SceneManagement.SceneManager.LoadScene("Level1_Office");

    //     if (tutorialManager != null)
    //         tutorialManager.StartTutorial();
    // }
    void ShowStep()
    {
        if (tutorialStep == 0)
        {
            titleText.text = "Tutorial: Strong vs Weak Passwords";

            bodyText.text =
            "AI Mentor:\n\n" +
            "A weak password is easy to guess.\n\n" +
            "It may be too short, use common words, names,\n" +
            "or simple patterns like '123'.\n\n" +
            "Examples:\n" +
            "- password\n" +
            "- ali123\n" +
            "- qwerty";

            nextButton.gameObject.SetActive(true);
        }
        else if (tutorialStep == 1)
        {
            titleText.text = "How to Play & Controls";

            bodyText.text =
            "Welcome to Level 1!\n\n" +
            "Your objective is to find and fix weak passwords.\n" +
            "Explore the office to locate the terminals.\n\n" +
            "Controls:\n" +
            "- W A S D or Arrow Keys to Move\n" +
            "- E to Interact\n" +
            "- Space/Enter to Confirm\n";
            
            nextButton.gameObject.SetActive(true);
        }
        else
        {
            if (tutorialPanel != null)
                tutorialPanel.SetActive(false);
        }
    }

    void NextStep()
    {
        tutorialStep++;
        ShowStep();
    }
}
