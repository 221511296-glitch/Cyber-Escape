using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

[Serializable]
public class PlayerProfile
{
    public string playerName = "Player";
    public int totalScore;
}

[Serializable]
public class ScoreData
{
    public int value;

    public void Add(int points)
    {
        value += points;
    }
}

[Serializable]
public class PasswordData
{
    public string accountName;
    public string password;
}

[Serializable]
public class EmailScenario
{
    public string subject;
    public string senderAddress;
    [TextArea(5, 15)]
    public string preview;
    public string embeddedUrl;
    public bool isPhishing;
    [TextArea(3, 5)]
    public string explanation;
}

public enum EmailAction
{
    Report,
    Ignore,
    OpenLink
}

public static class CyberSystem
{
    public static int SessionScore { get; set; } = 0;

    private static readonly HashSet<string> CommonWeakWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "password",
        "qwerty",
        "admin",
        "welcome",
        "letmein",
        "123456",
        "12345678"
    };

    public static bool IsWeakPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
        {
            return true;
        }

        if (CommonWeakWords.Contains(password))
        {
            return true;
        }

        string lowered = password.ToLowerInvariant();
        if (lowered.Contains("123") || lowered.Contains("abc") || lowered.Contains("qwerty"))
        {
            return true;
        }

        bool hasUpper = Regex.IsMatch(password, "[A-Z]");
        bool hasLower = Regex.IsMatch(password, "[a-z]");
        bool hasNumber = Regex.IsMatch(password, "[0-9]");
        bool hasSymbol = Regex.IsMatch(password, "[^a-zA-Z0-9]");

        return !(hasUpper && hasLower && hasNumber && hasSymbol);
    }

    public static bool IsStrongPassword(string password, out string hint)
    {
        hint = string.Empty;

        if (string.IsNullOrWhiteSpace(password))
        {
            hint = "Enter a password first.";
            return false;
        }

        List<string> missingRequirements = new List<string>();

        if (password.Length < 8)
        {
            missingRequirements.Add("at least 8 characters");
        }

        if (!Regex.IsMatch(password, "[A-Z]"))
        {
            missingRequirements.Add("an uppercase letter");
        }

        if (!Regex.IsMatch(password, "[a-z]"))
        {
            missingRequirements.Add("a lowercase letter");
        }

        if (!Regex.IsMatch(password, "[0-9]"))
        {
            missingRequirements.Add("a number");
        }

        if (!Regex.IsMatch(password, "[^a-zA-Z0-9]"))
        {
            missingRequirements.Add("a symbol (!, @, #, etc.)");
        }

        if (missingRequirements.Count > 0)
        {
            hint = "Your password is weak. Please add: " + string.Join(", ", missingRequirements) + ".";
            return false;
        }

        hint = "Great work. That new password is secure.";
        return true;
    }

    public static bool IsCorrectPhishingChoice(bool isPhishing, bool chosePhishing, EmailAction action)
    {
        if (isPhishing)
        {
            return chosePhishing && action == EmailAction.Report;
        }

        return !chosePhishing && (action == EmailAction.Ignore || action == EmailAction.OpenLink);
    }
}

public static class AIMentor
{
    public static string PasswordIdentifyCorrect(bool weak)
    {
        return weak
            ? "Yes! That's weak. Let's replace it with a stronger password."
            : "Correct. That password is strong enough and can stay.";
    }

    public static string PasswordIdentifyIncorrect(bool weak)
    {
        return weak
            ? "Not quite. This one is weak because it breaks password safety rules."
            : "Hmm, that's actually strong enough. Check length, case, numbers, and symbols.";
    }

    public static string PasswordCreationSuccess()
    {
        return "Great work. That new password is secure.";
    }

    public static string PasswordCreationFailure(string hint)
    {
        return "Still weak. " + hint;
    }

    public static string PhishingCorrect()
    {
        return "Good decision. Threat handled safely.";
    }

    public static string PhishingIncorrect(string explanation)
    {
        return "Unsafe choice. " + explanation + " Retry this email.";
    }
}
