using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VisualFeedbackSystem : MonoBehaviour
{
    [Header("Red Flash Settings")]
    [SerializeField] private Image redFlashOverlay;
    [SerializeField] private float flashDuration = 1.5f;
    [SerializeField] private float flashFadeDuration = 0.3f;

    [Header("Danger Icon Settings")]
    [SerializeField] private GameObject dangerIcon;
    [SerializeField] private float dangerIconDuration = 1f;

    [Header("Success Feedback Settings")]
    [SerializeField] private Image greenFlashOverlay;
    [SerializeField] private float successFlashDuration = 0.8f;

    [Header("Virus Animation Settings")]
    [SerializeField] private GameObject virusAnimationPrefab;
    [SerializeField] private Transform virusSpawnPoint;
    // [SerializeField] private float virusAnimationDuration = 2f;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip errorSound;
    [SerializeField] private AudioClip successSound;
    [SerializeField] private AudioClip virusAlertSound;
    [SerializeField] private float volume = 1f;

    private AudioSource audioSource;
    // private bool isInitialized = false;

    public void Initialize()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }

        // Ensure overlays are inactive
        if (redFlashOverlay != null) redFlashOverlay.gameObject.SetActive(false);
        if (greenFlashOverlay != null) greenFlashOverlay.gameObject.SetActive(false);
        if (dangerIcon != null) dangerIcon.SetActive(false);

        // isInitialized = true;
    }

    public void ShowErrorFeedback()
    {
        StartCoroutine(RedFlashSequence());
        ShowDangerIcon();
        PlayErrorSound();
    }

    public void ShowSuccessFeedback()
    {
        StartCoroutine(GreenFlashSequence());
        PlaySuccessSound();
    }

    private IEnumerator RedFlashSequence()
    {
        if (redFlashOverlay == null) yield break;

        redFlashOverlay.gameObject.SetActive(true);
        Color color = redFlashOverlay.color;
        color.a = 1f;
        redFlashOverlay.color = color;

        // Hold red flash
        yield return new WaitForSeconds(flashDuration);

        // Fade out
        float elapsedTime = 0f;
        while (elapsedTime < flashFadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, elapsedTime / flashFadeDuration);
            redFlashOverlay.color = color;
            yield return null;
        }

        redFlashOverlay.gameObject.SetActive(false);
    }

    private IEnumerator GreenFlashSequence()
    {
        if (greenFlashOverlay == null) yield break;

        greenFlashOverlay.gameObject.SetActive(true);
        Color color = greenFlashOverlay.color;
        color.a = 0.8f;
        greenFlashOverlay.color = color;

        yield return new WaitForSeconds(successFlashDuration);

        float elapsedTime = 0f;
        while (elapsedTime < flashFadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(0.8f, 0f, elapsedTime / flashFadeDuration);
            greenFlashOverlay.color = color;
            yield return null;
        }

        greenFlashOverlay.gameObject.SetActive(false);
    }

    private void ShowDangerIcon()
    {
        if (dangerIcon != null)
        {
            StartCoroutine(ShowDangerIconSequence());
        }
    }

    private IEnumerator ShowDangerIconSequence()
    {
        dangerIcon.SetActive(true);
        yield return new WaitForSeconds(dangerIconDuration);
        dangerIcon.SetActive(false);
    }

    public void ShowVirusAnimation()
    {
        if (virusAnimationPrefab != null && virusSpawnPoint != null)
        {
            PlayVirusAlertSound();
            Instantiate(virusAnimationPrefab, virusSpawnPoint.position, Quaternion.identity);
        }
    }

    private void PlayErrorSound()
    {
        if (audioSource != null && errorSound != null)
        {
            audioSource.PlayOneShot(errorSound, volume);
        }
    }

    private void PlaySuccessSound()
    {
        if (audioSource != null && successSound != null)
        {
            audioSource.PlayOneShot(successSound, volume);
        }
    }

    private void PlayVirusAlertSound()
    {
        if (audioSource != null && virusAlertSound != null)
        {
            audioSource.PlayOneShot(virusAlertSound, volume);
        }
    }

    public void SetVolume(float newVolume)
    {
        volume = Mathf.Clamp01(newVolume);
        if (audioSource != null)
        {
            audioSource.volume = volume;
        }
    }
}
