using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class UI : MonoBehaviour
{
    public static UI Instance { get; private set; }
    [SerializeField] private Image health;
    [SerializeField] private GameObject PC;
    [SerializeField] private GameObject interactableStuff;
    [SerializeField] private TMP_Text interactText;
    [SerializeField] private Image interactImage;

    [SerializeField] private Image damageOverlay;
    [SerializeField] private TMP_Text currentAmmoText;
    [SerializeField] private TMP_Text availableAmmoText;
    [SerializeField] private GameObject HUD;
    [SerializeField] private GameObject victoryScreen;
    [SerializeField] private GameObject bossHealth;
    [SerializeField] private Image bossHealthBar;
    [SerializeField] private Image yellowBossBar;
    [SerializeField] private AudioSource bossMusic;
    [SerializeField] private Transform deathScreen;
    private float yellowBossBarTimer = 0f;
    private float yellowBossBarValue = 1f;
    private float bossHealthValue = 1f;
    private float deathBossTimer = 3f;
    private float deathTimer = 0f;
    private bool isDead = false;
    public bool isBoss = false;
    public bool hasWon = false;
    public ParticleSystem explosionParticles; // why not
    public bool isCutscene = false;
    private int whiteHeartEnabled = 0;
    public AudioSource monitorAudio;
    [SerializeField] private AudioSource eventSource;
    [SerializeField] private AudioClip victorySound;
    [SerializeField] private AudioClip defeatSound;
    [SerializeField] private Image blackBackground;
    public int emptyDropsInARow = 0;

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
        StartCoroutine(BlackImageFade());
    }
    private void Update()
    {
        if (Cursor.lockState == CursorLockMode.None)
        {
            if (Time.timeScale >= 1f && Input.GetMouseButtonDown(0))
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
        if (damageOverlay.color.a > 0 && !isDead) {
            Color c = damageOverlay.color;
            c.a = Mathf.MoveTowards(c.a, 0f, Time.deltaTime / 2f);
            damageOverlay.color = c;
        }
        if (yellowBossBarTimer > 0)
        {
            yellowBossBarTimer -= Time.unscaledDeltaTime;
        }
        else
        {
            SetYellowBossBar(bossHealthValue);
        }
        if (bossHealthValue <= 0f && isBoss)
        {
            deathBossTimer -= Time.unscaledDeltaTime;
            if (deathBossTimer <= 0f)
            {
                ActivateBossHealth(false);
                isBoss = false;
                StartCoroutine(FadeOutMusic());
            }
        }
        if (isDead)
        {
            deathTimer -= Time.unscaledDeltaTime;
            if (deathTimer <= 0f)
            {
                ShowDeathScreen();
                isDead = false;
            }
        }
    }
    public void SetHealth(float current, float max)
    {
        health.material.SetFloat("_hearts", max);
        health.material.SetFloat("_health", current / max);
    }
    public void EnableHUD()
    {
        HUD.SetActive(true);
    }
    public void DisableHUD()
    {
        HUD.SetActive(false);
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
    public void ActivateDamageEffect()
    {
        Color c = damageOverlay.color;
        c.a = 0.2f;
        damageOverlay.color = c;
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
    public void ToggleInteractableStuff(IInteractable interactable)
    {
        if (interactable == null)
        {
            interactableStuff.SetActive(false);
            return;
        }
        interactableStuff.SetActive(true);
        if (interactable.isInteractable)
        {
            if (interactable.interactText == "")
            {
                interactText.text = "Interact";

            }
            else
            {
                interactText.text = interactable.interactText;
            }
            interactImage.color = new Color(1, 1, 1, 1);
        }
        else
        {
            if (interactable.nonInteractableText.Trim() == "")
            {
                interactableStuff.SetActive(false);
            }
            else
            {
                interactText.text = interactable.nonInteractableText;
                interactImage.color = new Color(1, 1, 1, 0.3f);
            }

        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="current"></param>
    /// <param name="max"></param>
    /// <param name="available">total ammo left in player's inventory. Put -1 here if pistol with unlimited ammo</param>
    public void SetAmmoText(int current, int max, int available)
    {
        currentAmmoText.text = $"{current.ToString()} / {max.ToString()}";
        
        if (available == -1)
        {
            availableAmmoText.text = "";
        }
        else
        {
            availableAmmoText.text = available.ToString();
        }
    }
    public void ShowVictoryScreen()
    {
        victoryScreen.SetActive(true);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        eventSource.PlayOneShot(victorySound);
        hasWon = true;
        DisableHUD();
    }
    public void ActivateBossHealth(bool activate)
    {
        bossHealth.SetActive(activate);
    }
    public void SetBossHealth(float current, float max)
    {
        bossHealthBar.fillAmount = current / max;
        yellowBossBarTimer = 1f;
        bossHealthValue = current / max;
    }
    private void SetYellowBossBar(float value)
    {
        if (yellowBossBarTimer <= 0)
        {
            yellowBossBarValue = Mathf.Max(yellowBossBarValue - (0.75f * Time.deltaTime), value);
            yellowBossBar.fillAmount = yellowBossBarValue;
        }
    }
    private IEnumerator FadeOutMusic()
    {
        while (bossMusic.volume > 0)
        {
            bossMusic.volume -= Time.unscaledDeltaTime / 3f;
            yield return null;
        }
        bossMusic.Stop();
    }
    public void Death()
    {
        if (!Settings.DeathCamera)
        {
            deathTimer = 0.1f;
        }
        else
        {
            deathTimer = 5f;
        }
        isDead = true;
        StartCoroutine(FadeOutMusic());
    }
    private void ShowDeathScreen()
    {
        deathScreen.gameObject.SetActive(true);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        eventSource.PlayOneShot(defeatSound);
        DisableHUD();
    }
    public void Restart()
    {
        StartCoroutine(LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex));
        //UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
    public void MainMenu()
    {
        StartCoroutine(LoadScene(0));
        //UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    private IEnumerator LoadScene(int index)
    {
        yield return null;
        yield return BlackImageAppear();
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(index);  
    }
    private IEnumerator BlackImageAppear()
    {
        blackBackground.gameObject.SetActive(true);
        while (blackBackground.color.a < 1f)
        {
            Color c = blackBackground.color;
            c.a = Mathf.MoveTowards(blackBackground.color.a, 1f, Mathf.Min(Time.unscaledDeltaTime * 1f, 1f / 3f));
            blackBackground.color = c;
            yield return null;
        }
    }
    private IEnumerator BlackImageFade()
    {
        blackBackground.gameObject.SetActive(true);
        Color cc = blackBackground.color;
        cc.a = 1f;
        blackBackground.color = cc;
        while (blackBackground.color.a > 0f)
        {
            Color c = blackBackground.color;
            c.a = Mathf.MoveTowards(blackBackground.color.a, 0f, Mathf.Min(Time.unscaledDeltaTime * 1f, 1f / 3f));
            blackBackground.color = c;
            yield return null;
        }
        blackBackground.gameObject.SetActive(false);
    }
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStatic()
    {
        Instance = null;
    }
}
