using UnityEngine;

public static class Level3ScoreManager
{
    // Session-based score tracking
    private static int sessionScore = 0;
    private static int level1Score = 0;
    private static int level2Score = 0;
    private static int level3Score = 0;

    // Player progression
    private static int level1Completed = 0;
    private static int level2Completed = 0;
    private static int level3Completed = 0;

    public static int SessionScore
    {
        get { return sessionScore; }
        set { sessionScore = value; }
    }

    public static int Level1Score
    {
        get { return GetLevelScore("Level1Score"); }
        set { SetLevelScore("Level1Score", value); level1Score = value; }
    }

    public static int Level2Score
    {
        get { return GetLevelScore("Level2Score"); }
        set { SetLevelScore("Level2Score", value); level2Score = value; }
    }

    public static int Level3Score
    {
        get { return GetLevelScore("Level3Score"); }
        set { SetLevelScore("Level3Score", value); level3Score = value; }
    }

    public static int GetTotalScore()
    {
        return Level1Score + Level2Score + Level3Score;
    }

    public static void SaveProgressToPlayerPrefs()
    {
        PlayerPrefs.SetInt("Level1Score", Level1Score);
        PlayerPrefs.SetInt("Level2Score", Level2Score);
        PlayerPrefs.SetInt("Level3Score", Level3Score);
        PlayerPrefs.SetInt("Level1Completed", level1Completed);
        PlayerPrefs.SetInt("Level2Completed", level2Completed);
        PlayerPrefs.SetInt("Level3Completed", level3Completed);
        PlayerPrefs.Save();
    }

    public static void LoadProgressFromPlayerPrefs()
    {
        level1Score = PlayerPrefs.GetInt("Level1Score", 0);
        level2Score = PlayerPrefs.GetInt("Level2Score", 0);
        level3Score = PlayerPrefs.GetInt("Level3Score", 0);
        level1Completed = PlayerPrefs.GetInt("Level1Completed", 0);
        level2Completed = PlayerPrefs.GetInt("Level2Completed", 0);
        level3Completed = PlayerPrefs.GetInt("Level3Completed", 0);
    }

    public static bool IsLevel1Completed()
    {
        return PlayerPrefs.GetInt("Level1Completed", 0) == 1;
    }

    public static bool IsLevel2Completed()
    {
        return PlayerPrefs.GetInt("Level2Completed", 0) == 1;
    }

    public static bool IsLevel3Completed()
    {
        return PlayerPrefs.GetInt("Level3Completed", 0) == 1;
    }

    public static void MarkLevelAsCompleted(int levelNum)
    {
        switch (levelNum)
        {
            case 1:
                level1Completed = 1;
                PlayerPrefs.SetInt("Level1Completed", 1);
                break;
            case 2:
                level2Completed = 1;
                PlayerPrefs.SetInt("Level2Completed", 1);
                break;
            case 3:
                level3Completed = 1;
                PlayerPrefs.SetInt("Level3Completed", 1);
                break;
        }
        PlayerPrefs.Save();
    }

    public static void ResetAllData()
    {
        sessionScore = 0;
        level1Score = 0;
        level2Score = 0;
        level3Score = 0;
        level1Completed = 0;
        level2Completed = 0;
        level3Completed = 0;

        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }

    private static int GetLevelScore(string key)
    {
        return PlayerPrefs.GetInt(key, 0);
    }

    private static void SetLevelScore(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
        PlayerPrefs.Save();
    }

    public static void PrintDebugInfo()
    {
        Debug.Log($"=== CYBER SYSTEM DEBUG ===");
        Debug.Log($"Level 1 Score: {Level1Score} | Completed: {IsLevel1Completed()}");
        Debug.Log($"Level 2 Score: {Level2Score} | Completed: {IsLevel2Completed()}");
        Debug.Log($"Level 3 Score: {Level3Score} | Completed: {IsLevel3Completed()}");
        Debug.Log($"Total Score: {GetTotalScore()}");
        Debug.Log($"Session Score: {SessionScore}");
    }
}
