using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EducationalFeedbackPanel : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Text feedbackText;
    [SerializeField] private Text itemNameText;
    [SerializeField] private Image correctnessIndicator;
    [SerializeField] private Color correctColor = Color.green;
    [SerializeField] private Color incorrectColor = Color.red;
    [SerializeField] private float displayDuration = 3f;
    [SerializeField] private float fadeInDuration = 0.3f;
    [SerializeField] private float fadeOutDuration = 0.5f;

    private CanvasGroup canvasGroup;

    public void Initialize()
    {
        if (canvasGroup == null)
        {
            canvasGroup = panel.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = panel.AddComponent<CanvasGroup>();
            }
        }

        panel.SetActive(false);
    }

    public void ShowFeedback(string itemName, string explanation, bool isCorrect)
    {
        if (panel != null) panel.SetActive(true);
        gameObject.SetActive(true); 
        StartCoroutine(ShowFeedbackSequence(itemName, explanation, isCorrect));
    }

    private IEnumerator ShowFeedbackSequence(string itemName, string explanation, bool isCorrect)
    {
        // Set content
        if (itemNameText != null)
        {
            itemNameText.text = itemName;
        }

        if (feedbackText != null)
        {
            feedbackText.text = explanation;
        }

        // Set color indicator
        if (correctnessIndicator != null)
        {
            correctnessIndicator.color = isCorrect ? correctColor : incorrectColor;
        }

        // Show panel
        panel.SetActive(true);
        canvasGroup.alpha = 0f;

        // Fade in
        float elapsedTime = 0f;
        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeInDuration);
            yield return null;
        }
        canvasGroup.alpha = 1f;

        // Display
        yield return new WaitForSeconds(displayDuration);

        // Fade out
        elapsedTime = 0f;
        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeOutDuration);
            yield return null;
        }

        canvasGroup.alpha = 0f;
        panel.SetActive(false);
    }

    public void HideFeedback()
    {
        StopAllCoroutines();
        panel.SetActive(false);
    }
}
