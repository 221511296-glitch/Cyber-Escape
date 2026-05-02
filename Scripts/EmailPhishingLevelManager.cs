using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class EmailPhishingLevelManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject emailCanvas;
    [SerializeField] private GameObject inboxPanel;
    [SerializeField] private GameObject completionPanel;

    [Header("Email UI")]
    [SerializeField] private Text subjectText;
    [SerializeField] private Text previewText;
    [SerializeField] private Text senderText;
    [SerializeField] private Text urlText;
    [SerializeField] private Text feedbackText;
    [SerializeField] private Text mentorText;
    [SerializeField] private Text scoreText;
    [SerializeField] private Slider progressBar;

    [Header("Decision Buttons")]
    [SerializeField] private Button phishingButton;
    [SerializeField] private Button notPhishingButton;
    [SerializeField] private Button reportButton;
    [SerializeField] private Button ignoreButton;
    [SerializeField] private Button openLinkButton;
    [SerializeField] private Button nextButton;

    [Header("Completion UI")]
    [SerializeField] private Text completionText;

    [Header("Inbox Data")]
    [SerializeField] private List<EmailScenario> emailInbox = new List<EmailScenario>
    {
        new EmailScenario
        {
            subject = "Bank Server - CLAIM NOW!",
            senderAddress = "security-update@bank-secure-help.net",
            preview = "Dear Customer,\n\nWe have detected unusual login activity on your account from an unrecognized device. As a security precaution, your online banking access has been temporarily suspended.\n\nTo restore your account access, you must immediately confirm your identity by clicking the link provided below. Failure to complete this verification within 24 hours will result in permanent account suspension and a potential freeze on your funds.\n\nWe take your security seriously.\n\nSincerely,\nThe Security Team",
            embeddedUrl = "http://bank-secure-help.net/verify",
            isPhishing = true,
            explanation = "The sender and link are suspicious and use urgency to trick you."
        },
        new EmailScenario
        {
            subject = "HR Policy Update",
            senderAddress = "hr@company.com",
            preview = "Dear Team,\n\nAs part of our commitment to fostering a safe and productive environment, we have recently updated our workplace policy document.\n\nThese changes reflect recent regulatory updates and internal guidelines for remote and hybrid work environments. Please take a moment to review the full document via the designated company intranet portal below.\n\nAll employees are required to acknowledge they have read and understood the new policies by the end of the week. If you have any questions, feel free to reach out to your manager or your designated HR representative.\n\nBest regards,\nHuman Resources Department",
            embeddedUrl = "https://intranet.company.com/hr/policy",
            isPhishing = false,
            explanation = "Legitimate sender and trusted company domain."
        },
        new EmailScenario
        {
            subject = "URGENT: Salary Correction",
            senderAddress = "payroll-alert@company-payroll-secure.co",
            preview = "URGENT ATTENTION REQUIRED,\n\nDuring a recent audit of our accounting systems, we discovered a critical discrepancy in your payroll profile. Your salary disbursement for the upcoming pay period is currently on administrative hold due to missing tax verification forms and unverified routing numbers in our system.\n\nTo avoid any delays in your next paycheck, it is critical that you log in to the payroll portal and update your details immediately.\n\nPlease follow the link below to access your secure portal and resolve this block immediately.\n\nThank you,\nGlobal Payroll Services",
            embeddedUrl = "http://company-payroll-secure.co/login",
            isPhishing = true,
            explanation = "The domain is not your trusted organization domain and pushes panic."
        }
    };

    private int currentIndex;
    private int score
    {
        get { return CyberSystem.SessionScore; }
        set { CyberSystem.SessionScore = value; }
    }
    private bool? selectedPhishingDecision;

    private void Start()
    {
        // If SessionScore is 0 (e.g., loaded directly to Level 2), load from PlayerPrefs
        if (CyberSystem.SessionScore == 0)
        {
            CyberSystem.SessionScore = PlayerPrefs.GetInt("PlayerScore", 0);
        }

        UpdateScoreUI();

        if (emailCanvas != null)
        {
            emailCanvas.SetActive(false); // Hidden until player interacts
        }

        if (inboxPanel != null)
        {
            inboxPanel.SetActive(false); // Hidden until player interacts
        }

        if (completionPanel != null)
        {
            completionPanel.SetActive(false);
        }

        RegisterButtonEvents();
    }

    public void StartEmailSession()
    {
        if (emailCanvas != null)
        {
            emailCanvas.SetActive(true);
        }

        if (inboxPanel != null)
        {
            inboxPanel.SetActive(true);
        }
        LoadCurrentEmail();
    }

    private void Update()
    {
        // ToggleInspect is removed because details are always shown now

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TrySubmitAction(EmailAction.Report);
        }
    }

    private void RegisterButtonEvents()
    {
        phishingButton.onClick.AddListener(() => SelectDecision(true));
        notPhishingButton.onClick.AddListener(() => SelectDecision(false));

        reportButton.onClick.AddListener(() => TrySubmitAction(EmailAction.Report));
        ignoreButton.onClick.AddListener(() => TrySubmitAction(EmailAction.Ignore));
        openLinkButton.onClick.AddListener(() => TrySubmitAction(EmailAction.OpenLink));

        nextButton.onClick.AddListener(NextScenario);
    }

    private void LoadCurrentEmail()
    {
        if (emailInbox.Count == 0)
        {
            CompleteLevel();
            return;
        }

        EmailScenario email = emailInbox[currentIndex];

        subjectText.text = email.subject;
        previewText.text = email.preview;

        selectedPhishingDecision = null;
        senderText.gameObject.SetActive(true);
        urlText.gameObject.SetActive(true);
        senderText.text = "Sender: " + email.senderAddress;
        urlText.text = "URL: " + email.embeddedUrl;

        feedbackText.text = "Choose Phishing/Not Phishing and your action.";
        mentorText.text = "AI Mentor: Investigate before deciding.";

        phishingButton.gameObject.SetActive(true);
        notPhishingButton.gameObject.SetActive(true);
        phishingButton.gameObject.SetActive(true);
        notPhishingButton.gameObject.SetActive(true);
        phishingButton.interactable = true;
        notPhishingButton.interactable = true;
        
        reportButton.gameObject.SetActive(false);
        ignoreButton.gameObject.SetActive(false);
        openLinkButton.gameObject.SetActive(false);
        
        nextButton.gameObject.SetActive(false);

        UpdateScoreUI();
        UpdateProgressUI();
    }

    // ToggleInspect removed because they are always visible now.

    private void SelectDecision(bool chosePhishing)
    {
        EmailScenario email = emailInbox[currentIndex];
        
        if (chosePhishing == email.isPhishing)
        {
            selectedPhishingDecision = chosePhishing;
            mentorText.text = chosePhishing
                ? "AI Mentor: Correct! This is phishing. Now choose a safe action."
                : "AI Mentor: Correct! This is not phishing. Choose your action.";
            
            feedbackText.text = "✅ Great! Now select an action.";
            
            // Show action buttons
            reportButton.gameObject.SetActive(true);
            ignoreButton.gameObject.SetActive(true);
            openLinkButton.gameObject.SetActive(true);
            
            reportButton.interactable = true;
            ignoreButton.interactable = true;
            openLinkButton.interactable = true;
            
            // Hide phishing decision buttons
            phishingButton.gameObject.SetActive(false);
            notPhishingButton.gameObject.SetActive(false);
        }
        else
        {
            score = Mathf.Max(0, score - 5);
            feedbackText.text = "❌ Incorrect. Take a closer look at the sender and URL.";
            mentorText.text = "AI Mentor: That classification is incorrect. Try again.";
            UpdateScoreUI();
        }
    }

    private void TrySubmitAction(EmailAction action)
    {
        if (!selectedPhishingDecision.HasValue)
        {
            feedbackText.text = "Choose 'Phishing' or 'Not Phishing' first.";
            return;
        }

        EmailScenario email = emailInbox[currentIndex];
        bool isCorrect = CyberSystem.IsCorrectPhishingChoice(email.isPhishing, selectedPhishingDecision.Value, action);

        if (isCorrect)
        {
            score += 15;
            feedbackText.text = "✅ " + AIMentor.PhishingCorrect();
            mentorText.text = "AI Mentor: Inbox secured for this message.";

            LockDecisionButtons();
            nextButton.gameObject.SetActive(true);
        }
        else
        {
            score = Mathf.Max(0, score - 5);
            feedbackText.text = "❌ " + AIMentor.PhishingIncorrect(email.explanation);
            mentorText.text = "AI Mentor: Re-check sender and URL, then try again.";
            
            // Allow them to reselect actions since they got the phishing type right already
            reportButton.interactable = true;
            ignoreButton.interactable = true;
            openLinkButton.interactable = true;
        }

        UpdateScoreUI();
    }

    private void LockDecisionButtons()
    {
        phishingButton.interactable = false;
        notPhishingButton.interactable = false;
        reportButton.interactable = false;
        ignoreButton.interactable = false;
        openLinkButton.interactable = false;
    }

    private void NextScenario()
    {
        currentIndex++;
        if (currentIndex >= emailInbox.Count)
        {
            CompleteLevel();
            return;
        }

        LoadCurrentEmail();
    }

    private void CompleteLevel()
    {
        if (inboxPanel != null)
        {
            inboxPanel.SetActive(false);
        }

        if (emailCanvas != null)
        {
            emailCanvas.SetActive(false);
        }

        // Disable the email terminal object completely in the scene
        EmailTerminalInteractor terminal = FindObjectOfType<EmailTerminalInteractor>();
        if (terminal != null)
        {
            terminal.gameObject.SetActive(false);
        }

        // Ensure player cannot move at the end of the level
        PlayerController playerMovement = FindObjectOfType<PlayerController>();
        if (playerMovement != null)
        {
            playerMovement.StopMovement();
        }

        // Start mentor interaction for Level 2
        MentorInteractionManager mentorManager = FindObjectsOfType<MentorInteractionManager>(true).FirstOrDefault();
        if (mentorManager != null)
        {
            mentorManager.gameObject.SetActive(true); // Must be active to run coroutines!
            mentorManager.StartLevel2EndingSequence();
        }

        PlayerPrefs.SetInt("PlayerScore", score);
        PlayerPrefs.SetInt("CurrentLevel", 3);
        PlayerPrefs.Save();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }

        // Specifically find the global HUD Score text that might not be directly referenced
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.name.Contains("Score") && obj.scene.isLoaded && obj.activeInHierarchy)
            {
                Text t = obj.GetComponent<Text>();
                if (t == null) t = obj.GetComponentInChildren<Text>();
                
                if (t != null && t.text.StartsWith("Score:"))
                {
                    // Don't override progress scores with max "Score: 0 / 100" if we can help it, just pure "Score: "
                    if (!t.text.Contains("/"))
                    {
                        t.text = "Score: " + score;
                    }
                }
            }
        }
    }

    private void UpdateProgressUI()
    {
        if (progressBar == null || emailInbox.Count == 0)
        {
            return;
        }

        progressBar.maxValue = emailInbox.Count;
        progressBar.value = currentIndex;
    }
}
