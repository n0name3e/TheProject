using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public static UI Instance { get; private set; }
    [SerializeField] private Image health;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    
    public void SetHealth(float current, float max)
    {
        health.material.SetFloat("_health", current / max);
    }
}
