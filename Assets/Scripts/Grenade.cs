using UnityEngine;
using UnityEngine.Audio;

public class Grenade : MonoBehaviour
{
    [SerializeField] private ParticleSystem explosionParticles;
    [SerializeField] private float radius = 7f;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip explosionSound;
    private ServerRoomBreaker roomBreaker;
    private float timeLeft = 4f;
    private float hitTimer = 0f; // used so that hitSound won't spam
    private void Start()
    {
        if (explosionParticles == null)
        {
            explosionParticles = UI.Instance.explosionParticles;
        }
        roomBreaker = FindAnyObjectByType<ServerRoomBreaker>();
    }
    void Update()
    {
        timeLeft -= Time.deltaTime;
        hitTimer -= Time.deltaTime;
        if (timeLeft <= 0)
        {
            Explode();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (hitTimer > 0f)
            return;
        Vector3 speed = collision.relativeVelocity;
        audioSource.PlayOneShot(hitSound, Mathf.Clamp01(speed.magnitude / 10f));
        hitTimer = 0.35f;
    }
    public void Explode()
    {
        explosionParticles.transform.position = transform.position;
        explosionParticles.Emit(60);

        Vector3 maxPos = transform.position + Vector3.up;
        GetComponent<Collider>().enabled = false;
        if (Vector3.Distance(transform.position, roomBreaker.transform.position) <= 4f)
        {
            roomBreaker.Break();
        }
        audioSource.minDistance = 30f;
        audioSource.transform.SetParent(null, true);
        audioSource.PlayOneShot(explosionSound);
        Destroy(audioSource.gameObject, 2f);
        PlayerHealth player = FindAnyObjectByType<PlayerHealth>();
        if (player != null)
        {
            if (Vector3.Distance(transform.position, player.transform.position) <= radius)
            {
                RaycastHit hit;
                if (Physics.Raycast(maxPos, player.transform.position - maxPos, out hit, radius))
                {
                    if (hit.transform == player.transform)
                    {
                        player.Hit(transform);
                    }
                }
            }
        }
        Destroy(gameObject);
    }
    }
