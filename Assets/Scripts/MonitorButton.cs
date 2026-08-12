using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class MonitorButton : MonoBehaviour
{
    private Color normalColor;
    private Color emissionColor;
    [SerializeField] private Renderer buttonRenderer;
    [SerializeField] private bool isSign = false;
    
    [ColorUsage(true, true)] [SerializeField] private Color hoverColor;
    private Material material;

    public UnityEvent onClick;

    private void Start()
    {
        material = buttonRenderer.material;
        material.EnableKeyword("_EMISSION");
        normalColor = material.color;
        emissionColor = material.GetColor("_EmissionColor");
    }
    private void OnMouseEnter()
    {
        material.color = hoverColor;
        material.SetColor("_EmissionColor", hoverColor);
        if (isSign)
        {
            MenuAudioManager.Instance.PlaySignHoverSound();
        }
        else
        {
            MenuAudioManager.Instance.PlayMonitorHoverSound();
        }
    }
    private void OnMouseExit()
    {
        material.color = normalColor;
        material.SetColor("_EmissionColor", emissionColor);
    }
    private void OnMouseDown()
    {
        if (isSign)
        {
            MenuAudioManager.Instance.PlaySignClickSound();
        }
        else
        {
            MenuAudioManager.Instance.PlayMonitorClickSound();
        }
        onClick?.Invoke();
    }
}
