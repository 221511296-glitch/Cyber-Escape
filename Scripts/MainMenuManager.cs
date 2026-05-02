using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenuManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject settingsPanel;
    public Toggle showInstructionsToggle;
    private const string SOUND_KEY = "SoundEnabled";
    private const string SCORE_KEY = "PlayerScore";
    private const string LEVEL_KEY = "CurrentLevel";
    private const string INSTRUCTIONS_KEY = "ShowInstructions";
    
    private void Start()
    {
        settingsPanel.SetActive(false);
        
        if (showInstructionsToggle != null)
        {
            bool isChecked = PlayerPrefs.GetInt(INSTRUCTIONS_KEY, 1) == 1;
            showInstructionsToggle.isOn = isChecked;
            showInstructionsToggle.onValueChanged.AddListener(OnInstructionsToggleChanged);
        }
    }
    
    private void OnInstructionsToggleChanged(bool isOn)
    {
        PlayerPrefs.SetInt(INSTRUCTIONS_KEY, isOn ? 1 : 0);
        PlayerPrefs.Save();
    }
    // --------------------
    // MAIN MENU
    // --------------------
    public void StartGame()
    {
        PlayerPrefs.SetInt(SCORE_KEY, 0);
        PlayerPrefs.SetInt(LEVEL_KEY, 1);
        PlayerPrefs.Save();
        
        CyberSystem.SessionScore = 0; // Ensure it starts at 0

        //scene name
        SceneManager.LoadScene("Level1_Office 1");
    }
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    // --------------------
    // SETTINGS
    // --------------------
    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }
    // --------------------
    // RESET
    // --------------------
    public void ResetGameProgress()
    {
        PlayerPrefs.DeleteKey(SCORE_KEY);
        PlayerPrefs.DeleteKey(LEVEL_KEY);
        PlayerPrefs.Save();
        
        // Reset static session variables
        CyberSystem.SessionScore = 0;
        
        Debug.Log("Game progress reset!");
    }
}