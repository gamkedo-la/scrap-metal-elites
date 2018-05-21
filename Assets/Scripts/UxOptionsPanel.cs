using UnityEngine;
using UnityEngine.UI;

public class UxOptionsPanel: UxPanel {
    [Header("UI Reference")]
    public Button backButton;
    public Slider sfxVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider voiceVolumeSlider;

    [Header("State Variables")]
    public UnitFloatVariable sfxMasterVolume;
    public UnitFloatVariable musicMasterVolume;
    public UnitFloatVariable voiceMasterVolume;

    void Start() {
        sfxVolumeSlider.minValue = 0f;
        sfxVolumeSlider.maxValue = 1f;
        sfxVolumeSlider.value = sfxMasterVolume.Value;

        musicVolumeSlider.minValue = 0f;
        musicVolumeSlider.maxValue = 1f;
        musicVolumeSlider.value = musicMasterVolume.Value;

        voiceVolumeSlider.minValue = 0f;
        voiceVolumeSlider.maxValue = 1f;
        voiceVolumeSlider.value = voiceMasterVolume.Value;

        // setup callbacks
        backButton.onClick.AddListener(OnBackClick);
        sfxVolumeSlider.onValueChanged.AddListener((val) => OnValueChanged(sfxVolumeSlider, sfxMasterVolume));
        musicVolumeSlider.onValueChanged.AddListener((val) => OnValueChanged(musicVolumeSlider, musicMasterVolume));
        voiceVolumeSlider.onValueChanged.AddListener((val) => OnValueChanged(voiceVolumeSlider, voiceMasterVolume));
    }

    public void OnValueChanged(Slider slider, UnitFloatVariable volume) {
        volume.Value = slider.normalizedValue;
    }

    public void OnBackClick() {
        Destroy(gameObject);
    }

}
