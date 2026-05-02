using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Text))]
public class GameStartInstruction : MonoBehaviour
{
    [Header("Instruction Settings")]
    [TextArea(2, 3)]
    public string message = "Go near the mentor and press 'E' to start the game.";
    public float typeSpeed = 0.05f;           // Speed of the typewriter effect
    public float displayDuration = 4f;        // How long to keep the text on screen after it finishes typing

    [Header("UI Elements")]
    public Image backgroundBox;               // Drag your background image here

    private Text instructionText;

    void Start()
    {
        instructionText = GetComponent<Text>();
        
        if (instructionText != null)
        {
            instructionText.text = "";
            if (backgroundBox != null) backgroundBox.gameObject.SetActive(true);
            StartCoroutine(AnimateAndHideInstruction());
        }
    }

    IEnumerator AnimateAndHideInstruction()
    {
        // 1. Typewriter effect
        instructionText.text = "";
        instructionText.gameObject.SetActive(true);

        foreach (char c in message)
        {
            instructionText.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }

        // 2. Wait for the player to read the message
        yield return new WaitForSeconds(displayDuration);

        // 3. Fade out effect (Optional but looks nicer)
        Color startColor = instructionText.color;
        Color boxStartColor = backgroundBox != null ? backgroundBox.color : Color.white;
        float fadeDuration = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startColor.a, 0f, elapsedTime / fadeDuration);
            instructionText.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            
            if (backgroundBox != null)
            {
                float boxAlpha = Mathf.Lerp(boxStartColor.a, 0f, elapsedTime / fadeDuration);
                backgroundBox.color = new Color(boxStartColor.r, boxStartColor.g, boxStartColor.b, boxAlpha);
            }
            yield return null;
        }

        // 4. Finally disable the text completely
        instructionText.gameObject.SetActive(false);
        if (backgroundBox != null) backgroundBox.gameObject.SetActive(false);
    }
}
