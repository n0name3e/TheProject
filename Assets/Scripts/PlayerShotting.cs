using UnityEngine;

public class PlayerShotting : MonoBehaviour
{
    [SerializeField] private LayerMask shootableLayer;
    [SerializeField] private ParticleSystem shootParticle; // wish it was Visual Effect
    [SerializeField] private Animator weaponAnimator;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }
    private void Shoot()
    {
        shootParticle.Play();
        weaponAnimator.SetTrigger("Shoot");

        RaycastHit hit;
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, 100f, shootableLayer))
        {
            Transform hitObject = hit.transform;
            print(hitObject.name);
            if (hitObject.TryGetComponent(out Enemy enemy))
            {
                print("hit");
                enemy.Hit();
            }
        }
    }
}
