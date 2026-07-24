using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] private ParticleSystem explosionParticles;
    [SerializeField] private float radius = 7f;
    private ServerRoomBreaker roomBreaker;
    private float timeLeft = 4f;
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

        if (timeLeft <= 0)
        {
            Explode();
        }
    }
    public void Explode()
    {
        explosionParticles.transform.position = transform.position;
        explosionParticles.Emit(60);

        Vector3 maxPos = transform.position + Vector3.up;
        GetComponent<Collider>().enabled = false;

        PlayerHealth player = FindAnyObjectByType<PlayerHealth>();
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
        if (Vector3.Distance(transform.position, roomBreaker.transform.position) <= 4f)
        {
            roomBreaker.Break();
        }


        Destroy(gameObject);
    }
    }
