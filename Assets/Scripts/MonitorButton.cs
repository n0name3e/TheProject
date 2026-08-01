using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class MonitorButton : MonoBehaviour
{
    private Color normalColor;
    private Color emissionColor;
    [SerializeField] private Renderer buttonRenderer;
    
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
        print("enter");
    }
    private void OnMouseExit()
    {
        material.color = normalColor;
        material.SetColor("_EmissionColor", emissionColor);
        print("exit");
    }
    private void OnMouseDown()
    {
        print("Click");
        onClick?.Invoke();
    }
}
