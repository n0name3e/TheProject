using UnityEngine;

public class PlayerShotting : MonoBehaviour
{
    [SerializeField] private LayerMask shootableLayer;
    [SerializeField] private ParticleSystem shootParticle; // wish it was Visual Effect // actually particle system is fine
    [SerializeField] private WeaponManager weaponManager;
    [SerializeField] private ParticleSystem hitParticles; // when enemy is hit
    [SerializeField] private ParticleSystem environmentHitParticles; // when world object is hit
    [SerializeField] private ParticleSystem explosionParticles;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (UI.Instance.isCutscene)
            return;
        if (Input.GetMouseButton(0))
        {
            Shoot();
        }
    }
    private void Shoot()
    {
        if (!weaponManager.CanShoot() || Time.timeScale == 0)
            return;
        //shootParticle.Play();
        weaponManager.Shoot();
        RaycastHit hit;
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, 100f, shootableLayer))
        {
            Transform hitObject = hit.transform;
            if (hitObject.TryGetComponent(out Enemy enemy))
            {
                enemy.Hit();
                hitParticles.transform.position = hit.point;
                hitParticles.Emit(15);
                StatsManager.Instance.hits++;
                return;
            }
            if (hitObject.GetComponentInChildren<Enemy>())
            {
                enemy = hitObject.GetComponentInChildren<Enemy>();
                enemy.Hit();
                hitParticles.transform.position = hit.point;
                hitParticles.Emit(15);
                StatsManager.Instance.hits++;
            }
            if (hitObject.TryGetComponent(out Boss boss))
            {
                boss.Hit();
                hitParticles.transform.position = hit.point;
                hitParticles.Emit(15);
                StatsManager.Instance.hits++;
                return;
            }
            if (hitObject.TryGetComponent(out Barrel barrel))
            {
                barrel.Explode();

                Destroy(hitObject.gameObject);
            }
            if (hitObject.TryGetComponent(out BreakablePallet pallet))
            {
                pallet.Hit(3);
            }
            if (hitObject.TryGetComponent(out MonitorObject monitor))
            {
                monitor.TakeDamage();
            }
            environmentHitParticles.transform.position = hit.point;
            environmentHitParticles.Emit(15);
        }
    }
}
