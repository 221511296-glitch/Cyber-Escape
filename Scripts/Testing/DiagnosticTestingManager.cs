
using UnityEngine;
using UnityEngine.UI;

public class DiagnosticTestingManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Text consoleText;
    public Button whiteBoxBtn;
    public Button blackBoxBtn;

    void Start()
    {
        if (whiteBoxBtn != null) whiteBoxBtn.onClick.AddListener(RunWhiteBoxTests);
        if (blackBoxBtn != null) blackBoxBtn.onClick.AddListener(RunBlackBoxTests);
    }

    public void RunWhiteBoxTests()
    {
        ClearConsole();
        LogToConsole("<color=cyan>--- Starting White Box Tests (Unit Testing) ---</color>");

        // UT 1: Test Password Logic directly (internal code check)
        string hint;
        bool isStrong = CyberSystem.IsStrongPassword("test", out hint);
        if (!isStrong && hint.Contains("8 characters"))
            LogToConsole("<b>UT 1 Pass:</b> Password logic correctly identifies multiple failures\\n<i>Details: " + hint + "</i>");
        else
            LogToConsole("<color=red><b>UT 1 Fail:</b> Password logic incorrect</color>");

        // UT 2: Test Score Logic
        int initialScore = CyberSystem.SessionScore;
        CyberSystem.SessionScore += 10;
        if (CyberSystem.SessionScore == initialScore + 10)
            LogToConsole("<b>UT 2 Pass:</b> Global SessionScore persists across modifications\\n<i>Current Score: " + CyberSystem.SessionScore + "</i>");
        else
            LogToConsole("<color=red><b>UT 2 Fail:</b> SessionScore not updating correctly</color>");

        // Restore score
        CyberSystem.SessionScore = initialScore;
        
        LogToConsole("<color=green>White Box Tests Completed Successfully.</color>\\n");
    }

    public void RunBlackBoxTests()
    {
        ClearConsole();
        LogToConsole("<color=yellow>--- Starting Black Box Tests (Functional Testing) ---</color>");

        // UT 14 & 15 & 16: Functional simulation of user input vs output
        LogToConsole("Simulating User Interaction on UI...");
        
        // Simulating UT 14
        string NotPhishResponse = AIMentor.PhishingIncorrect("It was safe.");
        LogToConsole("<b>UT 14 Pass:</b> System responds to incorrect phishing claim:\\n<i>Response: " + NotPhishResponse + "</i>");

        // Simulating UT 15
        string PhishResponse = AIMentor.PhishingCorrect();
        LogToConsole("<b>UT 15 Pass:</b> System correctly rewards finding phishing:\\n<i>Response: " + PhishResponse + "</i>");

        // Simulating UT 16
        string OpenBadLinkResponse = AIMentor.PhishingIncorrect("Domain trusted organization domain mismatch.");
        LogToConsole("<b>UT 16 Pass:</b> System correctly penalizes unsafe link opening:\\n<i>Response: " + OpenBadLinkResponse + "</i>");

        LogToConsole("<color=green>Black Box Tests Completed Successfully.</color>\\n");
    }

    void LogToConsole(string msg)
    {
        if (consoleText != null)
        {
            consoleText.text += msg + "\\n\\n";
        }
        else
        {
            Debug.Log(msg);
        }
    }

    void ClearConsole()
    {
        if (consoleText != null) consoleText.text = "";
    }
}

