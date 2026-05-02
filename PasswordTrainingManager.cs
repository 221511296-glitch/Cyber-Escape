using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PasswordTrainingManager : MonoBehaviour
{
    public GameObject panel;
    public Text passwordText;
    public Text feedbackText;
    public Text scoreText;
    public Button strongButton;
    public Button weakButton;
    public Button nextButton;

    private List<string> passwordList;
    private int currentIndex = 0;
    private int score 
    {
        get { return CyberSystem.SessionScore; }
        set { CyberSystem.SessionScore = value; }
    }

    void Start()
    {
        if (scoreText != null) scoreText.text = "Score: " + score;
        LoadPasswords();
        DisplayPassword();
        nextButton.gameObject.SetActive(false);
        
        // Add listener to the next button
        if (nextButton != null)
        {
            nextButton.onClick.AddListener(OnNextButtonPressed);
        }
    }

    void LoadPasswords()
    {
        passwordList = new List<string>()
        {
            "123456",
            "password",
            "Admin@123",
            "Qw!7kLp9",
            "3434Adfkd"
        };
    }

    void DisplayPassword()
    {
        passwordText.text = passwordList[currentIndex];
        feedbackText.text = "";
        nextButton.gameObject.SetActive(false);
    }
    public void OnClose()
    {
        panel.SetActive(false);
    }
    public void OnStrongSelected()
    {
        EvaluateAnswer(true);
    }

    public void OnWeakSelected()
    {
        EvaluateAnswer(false);
    }

    void EvaluateAnswer(bool playerChoiceStrong)
    {
        bool actualStrength = CheckPasswordStrength(passwordList[currentIndex]);

        if (playerChoiceStrong == actualStrength)
        {
            feedbackText.text = "Correct! This password is " + (actualStrength ? "strong." : "weak.");
            score += 10;
        }
        else
        {
            feedbackText.text = "Incorrect. This password is actually " + (actualStrength ? "strong." : "weak.");
            score -= 5;
        }

        scoreText.text = "Score: " + score;
        nextButton.gameObject.SetActive(true);

        // Disable buttons after selection
        strongButton.interactable = false;
        weakButton.interactable = false;
    }

    bool CheckPasswordStrength(string password)
    {
        return password.Length >= 8 &&
               System.Text.RegularExpressions.Regex.IsMatch(password, "[A-Z]") &&
               System.Text.RegularExpressions.Regex.IsMatch(password, "[a-z]") &&
               System.Text.RegularExpressions.Regex.IsMatch(password, "[0-9]");
    }

    public void NextPassword()
    {
        currentIndex++;
        strongButton.interactable = true;
        weakButton.interactable = true;

        if (currentIndex < passwordList.Count)
        {
            DisplayPassword();
        }
        else
        {
            // All passwords answered - show completion screen with Continue button
            ShowCompletionScreen();
        }
    }

    void ShowCompletionScreen()
    {
        passwordText.text = "Simulation Complete!";
        feedbackText.text = "Great job! You've completed the password security simulation.\n\nFinal Score: " + score;
        strongButton.interactable = false;
        weakButton.interactable = false;
        nextButton.gameObject.SetActive(true);
        nextButton.GetComponentInChildren<Text>().text = "Continue to Level";
    }

    public void OnNextButtonPressed()
    {
        if (currentIndex >= passwordList.Count)
        {
            // Training complete, close the panel instead of reloading the scene
            panel.SetActive(false);
        }
        else
        {
            // Progress to next password
            NextPassword();
        }
    }
}
