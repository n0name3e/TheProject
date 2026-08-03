using TMPro;
using Unity.AppUI.UI;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class SettingsMenuManager : MonoBehaviour
{
    [Header("UI Sliders")]
    public Slider musicSlider;
    public Slider soundSlider;
    public Slider sensitivitySlider;
    public Slider fovSlider;
    public Slider brightnessSlider;
    public UnityEngine.UI.Toggle deathCameraCheckbox;
    public UnityEngine.UI.Toggle invertYCheckbox;

    [Header("UI Texts (The numbers next to sliders)")]
    public TMP_Text musicText;
    public TMP_Text soundText;
    public TMP_Text sensitivityText;
    public TMP_Text fovText;
    public TMP_Text brightnessText;

    [Header("Systems")]
    public AudioMixer mainMixer; // Drag your game's AudioMixer here!
    public Volume globalVolume;  // Drag your URP Global Volume here!
    private ColorAdjustments colorAdjustments;

    private CameraController cameraController;

    private void OnEnable()
    {
        cameraController = FindAnyObjectByType<CameraController>();
        Settings.LoadSettings();
        // 1. Hook into URP Brightness safely
        if (globalVolume != null && globalVolume.profile.TryGet(out colorAdjustments))
        {
            if (colorAdjustments != null) colorAdjustments.postExposure.value = Settings.Brightness;
        }

        // 2. Make the UI match the hard drive data on boot
        InitializeUI();
    }

    private void InitializeUI()
    {
        // SetValueWithoutNotify is a AAA trick. It changes the slider visually 
        // WITHOUT accidentally triggering the save functions below during boot up!
        if (musicSlider != null) musicSlider.SetValueWithoutNotify(Settings.MusicVolume);
        if (soundSlider != null) soundSlider.SetValueWithoutNotify(Settings.SoundVolume);
        if (sensitivitySlider != null) sensitivitySlider.SetValueWithoutNotify(Settings.Sensitivity);
        if (fovSlider != null) fovSlider.SetValueWithoutNotify(Settings.FOV);
        if (brightnessSlider != null) brightnessSlider.SetValueWithoutNotify(Settings.Brightness);
        if (deathCameraCheckbox != null) deathCameraCheckbox.SetIsOnWithoutNotify(Settings.DeathCamera);
        if (invertYCheckbox != null) invertYCheckbox.SetIsOnWithoutNotify(Settings.InvertY);

        // Update the text numbers immediately
        UpdateMusicText(Settings.MusicVolume);
        UpdateSoundText(Settings.SoundVolume);
        UpdateSensitivityText(Settings.Sensitivity);
        UpdateFOVText(Settings.FOV);
        UpdateBrightnessText(Settings.Brightness);
    }

    // ==========================================
    // LINK THESE TO YOUR SLIDERS 'OnValueChanged'
    // ==========================================

    public void OnMusicSliderChanged(float value)
    {
        Settings.MusicVolume = value;
        UpdateMusicText(value);

        // AAA Audio Math: AudioMixers use Logarithmic math (-80db to 0db), not linear (0 to 1).
        // This converts a 0.001 -> 1 slider into perfect audio mixer math!
        if (mainMixer != null)
            mainMixer.SetFloat("MyExposedParam", Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f);
    }

    public void OnSoundSliderChanged(float value)
    {
        Settings.SoundVolume = value;
        UpdateSoundText(value);

        if (mainMixer != null)
            mainMixer.SetFloat("MyExposedParam 1", Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f);
    }

    public void OnSensitivitySliderChanged(float value)
    {
        Settings.Sensitivity = value;
        UpdateSensitivityText(value);

        if (cameraController != null)
        {
            cameraController.sensitivity = value;
        }
    }

    public void OnFOVSliderChanged(float value)
    {
        Settings.FOV = (int)value; // FOV must be a whole integer!
        UpdateFOVText(value);

        if (cameraController != null)
        {
            Camera.main.fieldOfView = Settings.FOV;
        }
        // Update the camera instantly so the player can see the change!
        //if (Camera.main != null) Camera.main.fieldOfView = Settings.FOV;
    }

    public void OnBrightnessSliderChanged(float value)
    {
        Settings.Brightness = value;
        UpdateBrightnessText(value);

        if (colorAdjustments != null) colorAdjustments.postExposure.value = value;
    }
    public void OnDeathCameraCheckboxChanged(bool isOn)
    {
        Settings.DeathCamera = isOn;
    }
    public void OnInvertYCheckboxChanged(bool isOn)
    {
        Settings.InvertY = isOn;

        if (cameraController != null)
        {
            cameraController.InvertY(isOn);
        }
    }

    // ==========================================
    // TEXT FORMATTING (The Secret Sauce)
    // ==========================================

    private void UpdateMusicText(float val) => musicText.text = val.ToString("F2"); // e.g. "1.0"
    private void UpdateSoundText(float val) => soundText.text = val.ToString("F2");
    private void UpdateSensitivityText(float val) => sensitivityText.text = val.ToString("F2");
    private void UpdateFOVText(float val) => fovText.text = Mathf.RoundToInt(val).ToString(); // e.g. "90"
    private void UpdateBrightnessText(float val) => brightnessText.text = val.ToString("F2"); // e.g. "0.2"
}