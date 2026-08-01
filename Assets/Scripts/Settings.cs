using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal; // Required for URP Post-Processing

public class Settings : MonoBehaviour
{
    public Volume globalVolume;

    private ColorAdjustments colorAdjustments;

    public static float MusicVolume = 1f;
    public static float SoundVolume = 1f;
    public static float Sensitivity = 1f;
    public static int FOV = 90;
    public static float Brightness = 0.2f;
    public static bool InvertY = false;
    public static bool DeathCamera = true;


    void Start()
    {
        if (globalVolume == null)
        {
            Debug.LogError("No Global Volume assigned to BrightnessManager!");
            return;
        }
        if (globalVolume.profile.TryGet(out colorAdjustments))
        {
            //Debug.Log("Color Adjustments found and linked.");
        }
        else
        {
            Debug.LogWarning("No Color Adjustments override found on this volume.");
        }
    }
    public static void LoadSettings()
    {
        MusicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        SoundVolume = PlayerPrefs.GetFloat("SoundVolume", 1f);
        Sensitivity = PlayerPrefs.GetFloat("Sensitivity", 1f);
        FOV = PlayerPrefs.GetInt("FOV", 90);
        Brightness = PlayerPrefs.GetFloat("Brightness", 0.2f);

        InvertY = PlayerPrefs.GetInt("InvertY", 0) == 1;
        DeathCamera = PlayerPrefs.GetInt("DeathCamera", 1) == 1;
    }
    public static void SaveSettings()
    {
        PlayerPrefs.SetFloat("MusicVolume", MusicVolume);
        PlayerPrefs.SetFloat("SoundVolume", SoundVolume);
        PlayerPrefs.SetFloat("Sensitivity", Sensitivity);
        PlayerPrefs.SetInt("FOV", FOV);
        PlayerPrefs.SetFloat("Brightness", Brightness);

        PlayerPrefs.SetInt("InvertY", InvertY ? 1 : 0);
        PlayerPrefs.SetInt("DeathCamera", DeathCamera ? 1 : 0);

        PlayerPrefs.Save();
    }
    /// <summary>
    /// Call this function from your UI Slider's "OnValueChanged" event
    /// </summary>
    /// <param name="sliderValue">A float between -1f and 1f</param>
    public void SetPostExposure(float sliderValue)
    {
        if (colorAdjustments != null)
        {
            colorAdjustments.postExposure.value = sliderValue;
        }
    }
}