using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
{
    public static UI Instance { get; private set; }
    [SerializeField] private Image health;
    [SerializeField] private GameObject PC;
    [SerializeField] private GameObject interactableStuff;

    [SerializeField] private TMP_Text currentAmmoText;
    [SerializeField] private TMP_Text availableAmmoText;
    private int whiteHeartEnabled = 0;

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
    private void Start()
    {
        DisableWhiteHeart();
    }

    public void SetHealth(float current, float max)
    {
        health.material.SetFloat("_health", current / max);
    }
    public void EnablePC()
    {
        PC.SetActive(true);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
    }
    public void DisablePC()
    {
        PC.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void ToggleWhiteHeart()
    {
        health.material.SetFloat("_whiteHeart", 1 - whiteHeartEnabled);
        whiteHeartEnabled = 1 - whiteHeartEnabled;
    }
    public void DisableWhiteHeart()
    {
        health.material.SetFloat("_whiteHeart", 0);
        whiteHeartEnabled = 0;
    }
    public void EnableWhiteHeart()
    {
        health.material.SetFloat("_whiteHeart", 1);
        whiteHeartEnabled = 1;
    }
    public void ToggleInteractableStuff(bool enable)
    {
        interactableStuff.SetActive(enable);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="current"></param>
    /// <param name="max"></param>
    /// <param name="available">total ammo left in player's inventory. Put -1 here if pistol with unlimited ammo</param>
    public void SetAmmoText(int current, int max, int available)
    {
        currentAmmoText.text = $"{current} / {max}";
        
        if (available == -1)
        {
            availableAmmoText.text = "";
        }
        else
        {
            availableAmmoText.text = available.ToString();
        }
    }
}
