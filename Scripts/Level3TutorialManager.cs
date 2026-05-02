using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Level3TutorialManager : MonoBehaviour
{
    [Header("Tutorial UI Reference")]
    public GameObject tutorialPanel;
    public Text titleText;
    public Text bodyText;
    public Button nextButton;

    [Header("Game Reference")]
    public VirusShieldLevelManager levelManager;

    private int currentStep = 0;
    private List<TutorialStep> steps = new List<TutorialStep>();

    private class TutorialStep
    {
        public string title;
        public string content;
        public TutorialStep(string t, string c) { title = t; content = c; }
    }

    void Start()
    {
        // Add listener to nextButton if it's missing (failsafe)
        if (nextButton != null)
        {
            nextButton.onClick.RemoveListener(OnNextClicked);
            nextButton.onClick.AddListener(OnNextClicked);
        }

        // Initialize steps
        steps.Clear();
        steps.Add(new TutorialStep("LEVEL 3: VIRUS SHIELD", 
            "Welcome, Agent. For your final test, you must protect our network from incoming malicious data blocks.\n\n" +
            "Your custom built workstation is ready. Let's review the new systems."));

        steps.Add(new TutorialStep("DRAG AND DROP", 
            "Data blocks will appear in the center of your screen.\n\n" +
            "DRAG them to the correct drive based on their contents:\n" +
            "- SAFE DRIVE: Clear files and known attachments.\n" +
            "- RECYCLE BIN: Viruses, Phishing attempts, and Ransomware."));

        steps.Add(new TutorialStep("THE USER PROFILE", 
            "Your User Profile shows your current status.\n\n" +
            "The text font has been increased for better visibility. You can click the 'v' button to minimize it to the taskbar if it blocks your view."));

        steps.Add(new TutorialStep("SYSTEM INTEGRITY", 
            "Watch the Integrity Bar at the top. If too many viruses enter the Safe Drive, the network will crash.\n\n" +
            "Good luck, Agent. Click 'CONTINUE' to start the work."));

        // Auto-start tutorial if panel is assigned
        if (tutorialPanel != null)
        {
            StartTutorial();
        }

        // Add listener to nextButton (failsafe)
        if (nextButton != null)
        {
            Debug.Log("TutorialManager: Binding NextButton listener");
            nextButton.onClick.RemoveAllListeners();
            nextButton.onClick.AddListener(() => {
                Debug.Log("TutorialManager: Button Clicked!");
                OnNextClicked();
            });
        }
    }

    public void StartTutorial()
    {
        currentStep = 0;
        tutorialPanel.SetActive(true);
        ShowCurrentStep();
    }

    private void ShowCurrentStep()
    {
        if (currentStep < steps.Count)
        {
            titleText.text = steps[currentStep].title;
            bodyText.text = steps[currentStep].content;
        }
        else
        {
            EndTutorial();
        }
    }

    private void OnNextClicked()
    {
        currentStep++;
        ShowCurrentStep();
    }

    private void EndTutorial()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level3");
    }
}
