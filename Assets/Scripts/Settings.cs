using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal; // Required for URP Post-Processing

public class Settings : MonoBehaviour
{
    public Volume globalVolume;

    private ColorAdjustments colorAdjustments;

    void Start()
    {
        if (globalVolume == null)
        {
            Debug.LogError("No Global Volume assigned to BrightnessManager!");
            return;
        }
        if (globalVolume.profile.TryGet(out colorAdjustments))
        {
            Debug.Log("Color Adjustments found and linked.");
        }
        else
        {
            Debug.LogWarning("No Color Adjustments override found on this volume.");
        }
    }

    /// <summary>
    /// Call this function from your UI Slider's "OnValueChanged" event
    /// </summary>
    /// <param name="sliderValue">A float between -1f and 1f</param>
    public void SetPostExposure(float sliderValue)
    {
        // If we successfully found the override in Start(), change the value
        if (colorAdjustments != null)
        {
            colorAdjustments.postExposure.value = sliderValue;
        }
    }
}