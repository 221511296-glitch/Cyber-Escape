using UnityEngine;
using UnityEngine.UI;

public class BackgroundMusicController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private Toggle soundToggle;

    private const string SOUND_KEY = "SoundEnabled";

    private void Start()
    {
        // Load saved state
        bool soundEnabled = PlayerPrefs.GetInt(SOUND_KEY, 1) == 1;
        soundToggle.isOn = soundEnabled;

        ApplyMusicState(soundEnabled);
    }

    private void Update()
    {
        ApplyMusicState(soundToggle.isOn);
    }

    private void ApplyMusicState(bool isOn)
    {
        if (musicSource == null) return;

        if (isOn)
        {
            if (!musicSource.isPlaying)
                musicSource.Play();
        }
        else
        {
            if (musicSource.isPlaying)
                musicSource.Stop();
        }

        PlayerPrefs.SetInt(SOUND_KEY, isOn ? 1 : 0);
    }
}
