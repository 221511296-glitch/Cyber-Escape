using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIPanelController : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject panel;          // The panel to open/close
    public Toggle soundToggle;        // Toggle for sound on/off
    public Toggle instructionsToggle; // Toggle for showing/hiding instructions in game

    [Header("Audio Settings")]
    public AudioSource backgroundMusic; // Optional: your background music source

    private void Start()
    {
        if (panel == null)
        {
            Transform t = transform.Find("Panel");
            if (t != null) panel = t.gameObject;
            else panel = gameObject;
        }

        // Ensure panel is closed at start
        if (panel != null && panel != gameObject)
            panel.SetActive(false);

        // Load saved sound preference if exists
        if (soundToggle != null)
        {
            if (PlayerPrefs.HasKey("SoundOn"))
            {
                bool isSoundOn = PlayerPrefs.GetInt("SoundOn") == 1;
                soundToggle.isOn = isSoundOn;
                SetSound(isSoundOn);
            }

            // Add listener for toggle changes
            soundToggle.onValueChanged.AddListener(SetSound);
        }

        if (instructionsToggle != null)
        {
            bool showInstructions = PlayerPrefs.GetInt("ShowInstructions", 1) == 1;
            instructionsToggle.isOn = showInstructions;
            instructionsToggle.onValueChanged.AddListener(ToggleInstructions);
        }
    }

    public void ToggleInstructions(bool isOn)
    {
        PlayerPrefs.SetInt("ShowInstructions", isOn ? 1 : 0);
        PlayerPrefs.Save();
        
        ControlsManager[] cms = Resources.FindObjectsOfTypeAll<ControlsManager>();
        foreach (var cm in cms)
        {
            if (cm.gameObject.scene.isLoaded) // ensure it's in the scene
            {
                cm.UpdateVisibilityFromSettings();
            }
        }
    }

    // Open the panel
    public void OpenPanel()
    {
        if (panel != null)
        {
            panel.SetActive(true);
            Time.timeScale = 0f; // Pause the game
        }
    }

    // Close the panel
    public void ClosePanel()
    {
        if (panel != null)
        {
            panel.SetActive(false);
            Time.timeScale = 1f; // Resume the game
        }
    }

    // Toggle sound on/off
    public void SetSound(bool isOn)
    {
        if (backgroundMusic != null)
        {
            backgroundMusic.mute = !isOn;
        }
        // Save preference
        PlayerPrefs.SetInt("SoundOn", isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    // Go to Main Menu Scene
    public void GoToMainMenu()
    {
        Time.timeScale = 1f; // Ensure time is unpaused before loading
        SceneManager.LoadScene("MainMenu"); // Make sure scene name matches
    }

    // Quit the game
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stop in editor
#else
        Application.Quit(); // Quit in build
#endif
    }
}
