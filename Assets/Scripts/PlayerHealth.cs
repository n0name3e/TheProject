using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float health { get; private set; } = 5f;
    public float immunityTime = 2f;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private GameObject normalHands;
    [SerializeField] private GameObject physicalHands; // used on death

    [field: SerializeField] public float maxHealth { get; private set; } = 5f;

    private bool isImmune = false;
    private float immunityTimer = 0f;
    private float flickeringTimer = 0f;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip hitSound;
    private void Awake()
    {
        if (cameraController == null)
        {
            cameraController = Camera.main.GetComponent<CameraController>();
        }
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }
    private void Start()
    {
        health = maxHealth;
        UI.Instance.SetHealth(health, maxHealth);
    }
    private void Update()
    {
        if (immunityTimer <= 0f)
        {
            if (isImmune)
            {
                isImmune = false;
                UI.Instance.DisableWhiteHeart();
                return;
            }
            return;
        }
        immunityTimer -= Time.deltaTime;
        if (flickeringTimer <= 0f)
        {
            UI.Instance.ToggleWhiteHeart();
            flickeringTimer += 0.1f;
            return;
        }
        flickeringTimer -= Time.deltaTime;
    }

    public void Hit(Transform hitter)
    {
        if (immunityTimer > 0f)
        {
            print("immune");
            return;
        }
        health--;
        UI.Instance.SetHealth(health, maxHealth);
        UI.Instance.ActivateDamageEffect();
        if (UI.Instance.isBoss)
        {
            cameraController.ApplyHitFlinch(27f);
        }
        else
        {
            cameraController.ApplyHitFlinch(17.5f);
        }
        audioSource.PlayOneShot(hitSound);

        if (health <= 0)
        {
            normalHands.SetActive(false);
            Camera.main.transform.SetParent(null);
            Camera.main.GetComponent<AudioSource>().PlayOneShot(hitSound);
            cameraController.TriggerDeath((transform.position - hitter.position).normalized);
            
            WeaponManager weaponManager = GetComponent<WeaponManager>();
            GameObject hands = Instantiate(physicalHands, normalHands.transform.position, normalHands.transform.rotation);
            if (weaponManager.currentWeapon == WeaponType.Pistol)
            {
                hands.transform.GetChild(0).gameObject.SetActive(true);
                hands.transform.GetChild(1).gameObject.SetActive(false);
            }
            else
            {
                hands.transform.GetChild(1).gameObject.SetActive(true);
                hands.transform.GetChild(0).gameObject.SetActive(false);
            }
            Destroy(gameObject);
            //SceneManager.LoadScene(0);
            return;
        }
        UI.Instance.EnableWhiteHeart();
        flickeringTimer = 0.1f;
        isImmune = true;
        immunityTimer = immunityTime;
    }
    public void Heal()
    {
        if (health >= maxHealth)
        {
            maxHealth++;
            health = maxHealth;
        }
        else
        {
            health++;
        }
        UI.Instance.SetHealth(health, maxHealth);
    }
}
