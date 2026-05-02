using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class VirusShieldLevelManager : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject mainCanvas;
    [SerializeField] private GameObject desktopPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject completionPanel;
    [SerializeField] private GameObject quitConfirmationPanel; // Added this

    [Header("UI Elements")]
    [SerializeField] private Text scoreText;
    [SerializeField] private Text levelCompleteText;
    [SerializeField] private Button retryButton;
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private Button menuButton;

    [Header("Game Systems")]
    [SerializeField] private ItemQueueManager itemQueueManager;
    [SerializeField] private HealthBarSystem healthBarSystem;
    [SerializeField] private VisualFeedbackSystem visualFeedbackSystem;
    [SerializeField] private EducationalFeedbackPanel feedbackPanel;
    [SerializeField] private GameObject desktopEnvironment;

    [Header("Drop Zones")]
    [SerializeField] private DropZone safeZone;
    [SerializeField] private DropZone dangerZone;

    [Header("Game Configuration")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int healthDecreasePerMistake = 20;
    [SerializeField] private int itemsPerLevel = 10;
    [SerializeField] private float itemSpawnDelay = 1f;

    private int currentScore;
    private bool isGameActive = true;
    private int correctAnswers = 0;
    private int totalItems = 0;

    private void Start()
    {
        // Set up panels to be hidden initially until interact
        if (mainCanvas != null) mainCanvas.SetActive(false);
        if (desktopPanel != null) desktopPanel.SetActive(false);

        InitializeGame();
        LoadPreviousScore();
        UpdateScoreUI();
    }

    private void InitializeGame()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (completionPanel != null) completionPanel.SetActive(false);
        if (quitConfirmationPanel != null) quitConfirmationPanel.SetActive(false);

        // Initialize systems
        if (healthBarSystem != null)
        {
            healthBarSystem.Initialize(maxHealth, healthDecreasePerMistake);
        }

        if (visualFeedbackSystem != null)
        {
            visualFeedbackSystem.Initialize();
        }

        if (feedbackPanel != null)
        {
            feedbackPanel.Initialize();
        }

        if (itemQueueManager != null)
        {
            itemQueueManager.Initialize(itemsPerLevel, safeZone, dangerZone, OnItemDropped);
        }

        // Register button events
        if (retryButton != null) retryButton.onClick.AddListener(RetryLevel);
        if (nextLevelButton != null) nextLevelButton.onClick.AddListener(GoToNextLevel);
        if (menuButton != null) menuButton.onClick.AddListener(ShowQuitConfirmation);

    }

    public void StartLevelTask()
    {
        if (mainCanvas != null) mainCanvas.SetActive(true);
        if (desktopPanel != null) desktopPanel.SetActive(true);
        
        // Start spawning items
        StartCoroutine(SpawnItemsSequence());
    }

    private void LoadPreviousScore()
    {
        currentScore = PlayerPrefs.GetInt("PlayerScore", 0);
    }

    private IEnumerator SpawnItemsSequence()
    {
        yield return new WaitForSeconds(1f);
        if (itemQueueManager != null)
        {
            itemQueueManager.StartSpawningItems(itemSpawnDelay);
        }
    }

    public void OnItemDropped(bool isCorrect, string itemName, string explanation)
    {
        if (!isGameActive) return;

        totalItems++;

        if (isCorrect)
        {
            correctAnswers++;
            currentScore += 10;
            visualFeedbackSystem?.ShowSuccessFeedback();
        }
        else
        {
            currentScore = Mathf.Max(0, currentScore - 5);
            visualFeedbackSystem?.ShowErrorFeedback();
            healthBarSystem?.DecreaseHealth();

            // Check if game over
            if (healthBarSystem?.IsGameOver() == true)
            {
                EndGame(false);
                return;
            }
        }

        feedbackPanel?.ShowFeedback(itemName, explanation, isCorrect);
        UpdateScoreUI();
    }

    private void EndGame(bool isVictory)
    {
        isGameActive = false;

        if (isVictory)
        {
            ShowCompletionScreen();
        }
        else
        {
            ShowGameOverScreen();
        }
    }

    private void ShowGameOverScreen()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            visualFeedbackSystem?.ShowVirusAnimation();
        }
    }

    private void ShowCompletionScreen()
    {
        if (completionPanel != null)
        {
            completionPanel.SetActive(true);
            if (levelCompleteText != null)
            {
                levelCompleteText.text = $"Level Complete!\nFinal Score: {currentScore}\nCorrect: {correctAnswers}/{totalItems}";
            }
        }

        // Save score
        Level3ScoreManager.SessionScore = currentScore;
        PlayerPrefs.SetInt("PlayerScore", currentScore);
        PlayerPrefs.SetInt("Level3Completed", 1);
        PlayerPrefs.Save();
    }

    private void RetryLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level3");
    }

    private void GoToNextLevel()
    {
        // Go to main menu or next level
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void ShowQuitConfirmation()
    {
        if (quitConfirmationPanel != null)
        {
            quitConfirmationPanel.SetActive(true);
        }
        else
        {
            // Fallback if they didn't assign the panel: just go to the menu
            GoToMenu();
        }
    }

    public void HideQuitConfirmation()
    {
        if (quitConfirmationPanel != null)
        {
            quitConfirmationPanel.SetActive(false);
        }
    }

    public void ConfirmQuitToMenu()
    {
        GoToMenu();
    }

    private void GoToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {currentScore}";
        }
    }

    public int GetCurrentScore() => currentScore;
    public bool IsGameActive() => isGameActive;

    public void LevelComplete()
    {
        EndGame(true);
    }
}
