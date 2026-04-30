using System;
using Unity.VisualScripting;
using UnityEngine;

public enum WeaponType
{
    Pistol,
    Rifle
}
public class WeaponManager : MonoBehaviour
{
    private static readonly int UnequipRifleHash = Animator.StringToHash("unequipRifle");
    private static readonly int UnequipPistolHash = Animator.StringToHash("unequipPistol");
    private static readonly int EquipRifleHash = Animator.StringToHash("equipRifle");
    private static readonly int EquipPistolHash = Animator.StringToHash("equipPistol");
    private Animator animator;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private GameObject pistolHands;
    [SerializeField] private GameObject rifleHands;

    [SerializeField] private ParticleSystem pistolShootEffect;
    [SerializeField] private ParticleSystem rifleShootEffect;

    // this could have possible be done with dictionary<enum, int> but for 2 weapons whatever
    // magazine size
    [SerializeField] private int rifleMaxAmmo = 20;
    [SerializeField] private int pistolMaxAmmo = 8;
    // current magazine amount
    private int rifleCurrentAmmo = 20;
    private int pistolCurrentAmmo = 8;
    // bullets that player carries (pistol is unlimited)
    public int rifleAvailableAmmo { get; private set; } = 40;

    [SerializeField] private float pistolCooldown = 0.3f;
    [SerializeField] private float rifleCooldown = 0.12f;
    [SerializeField] private float pistolRecoilMultiplier = 2f;
    [SerializeField] private float rifleRecoilMultiplier = 1f;
    private float currentCooldown = 0f;
    public WeaponType currentWeapon { get; private set; } = WeaponType.Rifle;
    private bool isInteracting;
    private bool isUnequiping = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        isInteracting = animator.GetCurrentAnimatorStateInfo(0).IsTag("interact");
    
        if (Input.GetKeyDown(KeyCode.Alpha1))
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
        return false;
    }
    public void Reload()
    {
        if (isInteracting)
            return;
        if (currentWeapon == WeaponType.Pistol && pistolCurrentAmmo < pistolMaxAmmo)
        {
            animator.CrossFade("pistolReload", 0.2f);
            pistolCurrentAmmo = pistolMaxAmmo;
        }
        if (currentWeapon == WeaponType.Rifle && rifleCurrentAmmo < rifleMaxAmmo)
        {
            if (rifleAvailableAmmo > 0)
            {
                animator.CrossFade("rifleReload", 0.2f);
                int beforeAmmo = rifleCurrentAmmo;
                int targetAmmo = Mathf.Min((rifleCurrentAmmo + rifleAvailableAmmo), rifleMaxAmmo);
                rifleCurrentAmmo = targetAmmo;
                rifleAvailableAmmo -= targetAmmo - beforeAmmo;
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
        if (currentWeapon == WeaponType.Pistol)
        {
            //animator.Play(PistolShootHash);
            animator.SetTrigger("pistolShoot");
            pistolCurrentAmmo--;
            UI.Instance.SetAmmoText(pistolCurrentAmmo, pistolMaxAmmo, -1);
            currentCooldown = pistolCooldown;
            cameraController.TriggerRecoil(pistolRecoilMultiplier);
            pistolShootEffect.Play();
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
}
