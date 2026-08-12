using Unity.VisualScripting;
using UnityEngine;

public enum WeaponType
{
    Pistol,
    Rifle
}
public class WeaponManager : MonoBehaviour
{
    private static readonly int RifleReloadHash = Animator.StringToHash("rifleReload");
    private static readonly int UnequipRifleHash = Animator.StringToHash("unequipRifle");
    private static readonly int UnequipPistolHash = Animator.StringToHash("unequipPistol");
    private static readonly int EquipRifleHash = Animator.StringToHash("equipRifle");
    private static readonly int EquipPistolHash = Animator.StringToHash("equipPistol");
    private static readonly int RifleReloadAltHash = Animator.StringToHash("rifle_reload_alt");
    private Animator animator;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private GameObject pistolHands;
    [SerializeField] private GameObject rifleHands;

    [SerializeField] private ParticleSystem pistolShootEffect;
    [SerializeField] private ParticleSystem rifleShootEffect;

    // this could have possible be done with dictionary<enum, int> but for 2 weapons whatever
    // or with scriptable objects
    // magazine size
    [SerializeField] private int rifleMaxAmmo = 15;
    [SerializeField] private int pistolMaxAmmo = 8;
    // current magazine amount
    private int rifleCurrentAmmo = 0;
    private int pistolCurrentAmmo = 8;
    // bullets that player carries (pistol is unlimited)
    public int rifleAvailableAmmo { get; private set; } = 0;

    [SerializeField] private float pistolCooldown = 0.3f;
    [SerializeField] private float rifleCooldown = 0.12f;
    [SerializeField] private float pistolRecoilMultiplier = 2f;
    [SerializeField] private float rifleRecoilMultiplier = 1f;
    private bool hasRifle = false;

    [Header("Aduio")]
    [SerializeField] private AudioSource shootAudio;
    [SerializeField] private AudioClip rifleShoot;
    [SerializeField] private AudioClip pistolShoot;
    [SerializeField] private AudioClip rifleReloadStart;
    [SerializeField] private AudioClip rifleReloadEnd;
    [SerializeField] private AudioClip pistolReloadStart;
    [SerializeField] private AudioClip pistolReloadEnd;
    [SerializeField] private AudioClip emptyFire;
    [SerializeField] private AudioClip rifleEquip;
    [SerializeField] private AudioClip pistolEquip;

    private float currentCooldown = 0f;
    public WeaponType currentWeapon { get; private set; } = WeaponType.Pistol;
    private bool isInteracting;
    private bool isUnequiping = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        UI.Instance.SetAmmoText(pistolCurrentAmmo, pistolMaxAmmo, -1);
        //UI.Instance.SetAmmoText(rifleCurrentAmmo, rifleMaxAmmo, rifleAvailableAmmo);
    }
    private void Update()
    {
        isInteracting = animator.GetCurrentAnimatorStateInfo(0).IsTag("interact");
    
        if (Time.timeScale <= 0)
            return;
        if (Input.GetKeyDown(KeyCode.Alpha1) && hasRifle)
        {
            UnequipPistol(); // rifle
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            UnequipRifle(); // pistol
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
        currentCooldown -= Time.deltaTime;
    }

    public bool CanShoot()
    {
        if (isInteracting || currentCooldown > 0)
        {
            return false;
        }
        if (currentWeapon == WeaponType.Pistol)
        {
            if (pistolCurrentAmmo > 0)
            {
                return true;
            }
        }
        if (currentWeapon == WeaponType.Rifle)
        {
            if (rifleCurrentAmmo > 0)
            {
                return true;
            }
        }
        shootAudio.PlayOneShot(emptyFire);
        currentCooldown = pistolCooldown;
        return false;
    }
    public void CollectRifle()
    {
        if (!hasRifle)
        {
            rifleCurrentAmmo = rifleMaxAmmo;
            hasRifle = true;
            UnequipPistol();
        }
        if (currentWeapon == WeaponType.Rifle)
        {
            rifleAvailableAmmo += 15;
            UI.Instance.SetAmmoText(rifleCurrentAmmo, rifleMaxAmmo, rifleAvailableAmmo);
        }
    }
    public void Reload()
    {
        if (isInteracting)
            return;
        if (currentWeapon == WeaponType.Pistol && pistolCurrentAmmo < pistolMaxAmmo)
        {
            animator.CrossFade("pistolReload", 0.2f);
            //pistolCurrentAmmo = pistolMaxAmmo;
        }
        if (currentWeapon == WeaponType.Rifle && rifleCurrentAmmo < rifleMaxAmmo)
        {
            if (rifleAvailableAmmo > 0)
            {
                int r = Random.Range(0, 100);
                if (r < 20 || (rifleCurrentAmmo == 0 && r < 60))
                {
                    animator.Play(RifleReloadAltHash);
                }
                else
                    animator.Play(RifleReloadHash);

                /*int beforeAmmo = rifleCurrentAmmo;
                int targetAmmo = Mathf.Min((rifleCurrentAmmo + rifleAvailableAmmo), rifleMaxAmmo);
                rifleCurrentAmmo = targetAmmo;
                rifleAvailableAmmo -= targetAmmo - beforeAmmo;*/
            }
        }
    }
    // this is called from animator
    public void ReplenishAmmo()
    {
        if (currentWeapon == WeaponType.Pistol)
        {
            pistolCurrentAmmo = pistolMaxAmmo;
            UI.Instance.SetAmmoText(pistolCurrentAmmo, pistolMaxAmmo, -1);
        }
        if (currentWeapon == WeaponType.Rifle)
        {
            int beforeAmmo = rifleCurrentAmmo;
            int targetAmmo = Mathf.Min((rifleCurrentAmmo + rifleAvailableAmmo), rifleMaxAmmo);
            rifleCurrentAmmo = targetAmmo;
            rifleAvailableAmmo -= targetAmmo - beforeAmmo;
            UI.Instance.SetAmmoText(rifleCurrentAmmo, rifleMaxAmmo, rifleAvailableAmmo);
        }
    }
    // those are called when player presses the button
    public void UnequipRifle()
    {
        if (isInteracting || currentWeapon == WeaponType.Pistol)
            return;
        isUnequiping = true;
        animator.CrossFade(UnequipRifleHash, 0.2f);
    }
    public void UnequipPistol()
    {
        if (isInteracting || currentWeapon == WeaponType.Rifle)
            return;
        isUnequiping = true;
        animator.CrossFade(UnequipPistolHash, 0.2f);
    }
    // those are called from animator when unequip is finished
    public void EquipPistol()
    {
        if (!isUnequiping)
            return;
        isUnequiping = false;
        rifleHands.SetActive(false);
        //pistolHands.SetActive(true);
        currentWeapon = WeaponType.Pistol;
        UI.Instance.SetAmmoText(pistolCurrentAmmo, pistolMaxAmmo, -1);
        animator.Play(EquipPistolHash);
    }
    public void EquipRifle()
    {
        if (!isUnequiping)
            return;
        isUnequiping = false;
        //rifleHands.SetActive(true);
        pistolHands.SetActive(false);
        currentWeapon = WeaponType.Rifle;
        UI.Instance.SetAmmoText(rifleCurrentAmmo, rifleMaxAmmo, rifleAvailableAmmo);
        animator.Play(EquipRifleHash);
    }
    public void DisableRifle()
    {
        rifleHands.SetActive(false);
    }
    public void DisablePistol()
    {
        pistolHands.SetActive(false);
    }
    public void EnableRifle()
    {
        rifleHands.SetActive(true);
    }
    public void EnablePistol()
    {
        pistolHands.SetActive(true);
    }

    /// <summary>
    /// invokes animations and eats ammo
    /// </summary>
    public void Shoot()
    { 
        // can't shoot while swapping weapons or reloading!
        if (isInteracting)
            return;
        shootAudio.pitch = Random.Range(0.9f, 1.1f);
        if (currentWeapon == WeaponType.Pistol)
        {
            //animator.Play(PistolShootHash);
            animator.SetTrigger("pistolShoot");
            pistolCurrentAmmo--;
            UI.Instance.SetAmmoText(pistolCurrentAmmo, pistolMaxAmmo, -1);
            currentCooldown = pistolCooldown;
            cameraController.TriggerRecoil(pistolRecoilMultiplier);
            pistolShootEffect.Play();
            shootAudio.pitch = Random.Range(0.8f, 0.95f);
            shootAudio.PlayOneShot(pistolShoot);
        }
        else
        {
            //animator.Play(RifleShootHash);
            animator.SetTrigger("rifleShoot");
            rifleCurrentAmmo--;
            UI.Instance.SetAmmoText(rifleCurrentAmmo, rifleMaxAmmo, rifleAvailableAmmo);
            currentCooldown = rifleCooldown;
            cameraController.TriggerRecoil(rifleRecoilMultiplier);
            rifleShootEffect.Play();
            shootAudio.PlayOneShot(rifleShoot);
        }
    }
    public bool CanSprint()
    {
        if (currentCooldown > 0 || isInteracting)
        {
            return false;
        }
        return true;
    }
    public void CollectAmmo(int amount)
    {
        rifleAvailableAmmo += amount;
        if (currentWeapon == WeaponType.Rifle)
        {
            UI.Instance.SetAmmoText(rifleCurrentAmmo, rifleMaxAmmo, rifleAvailableAmmo);
        }
    }
    public void IncreaseRifleAmmo(int amount)
    {
        rifleMaxAmmo += amount;
        rifleCurrentAmmo += amount;
        if (currentWeapon == WeaponType.Rifle)
        {
            UI.Instance.SetAmmoText(rifleCurrentAmmo, rifleMaxAmmo, rifleAvailableAmmo);
        }
    }
    public void PlayRifleReloadStart()
    {
        shootAudio.pitch = Random.Range(0.9f, 1.1f);
        shootAudio.PlayOneShot(rifleReloadStart);
    }
    public void PlayRifleReloadEnd()
    {
        shootAudio.pitch = Random.Range(0.9f, 1.1f);
        shootAudio.PlayOneShot(rifleReloadEnd);
    }
    public void PlayPistolReloadStart()
    {
        shootAudio.pitch = Random.Range(0.9f, 1.1f);
        shootAudio.PlayOneShot(pistolReloadStart);
    }
    public void PlayPistolReloadEnd()
    {
        shootAudio.pitch = Random.Range(0.9f, 1.1f);
        shootAudio.PlayOneShot(pistolReloadEnd);
    }
    public void PlayEquipRifle()
    {
        shootAudio.pitch = 1f;
        shootAudio.PlayOneShot(rifleEquip);
    }
    public void PlayEquipPistol()
    {
        shootAudio.pitch = 1f;
        shootAudio.PlayOneShot(pistolEquip);
    }
}
