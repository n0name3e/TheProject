using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [field: SerializeField] public float MaxHealth { get; private set; } = 4f;
    public float Health { get; private set; } = 4f;

    [SerializeField] private float attackRange = 5f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private Transform player;
    [SerializeField] private Transform shootPosition;
    [SerializeField] private GameObject bulletPrefab;

    private float attackTimer;

    private NavMeshAgent agent;
    [SerializeField] private ParticleSystem shootParticles;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    void Start()
    {
        Health = MaxHealth;
        if (player == null)
        {
            player = FindAnyObjectByType<PlayerMovement>().transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        attackTimer -= Time.deltaTime;
        if (Vector3.Distance(player.position, transform.position) <= attackRange)
        {
            agent.isStopped = true;
            if (attackTimer <= 0)
            {
                Shoot();
            }
        }
        else
        {
            if (attackTimer <= 0)
            {
                agent.isStopped = false;
                agent.SetDestination(player.position);
            }
        }
    }
    public void Hit()
    {
        Health--;
        shootParticles.Play();
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }
    public void Shoot()
    {
        attackTimer = attackCooldown;
        shootParticles.Play();

        GameObject bulletObject = Instantiate(bulletPrefab, shootPosition.position, transform.rotation);

        Vector3 direction = (player.position - transform.position).normalized;
        direction += new Vector3(direction.x + Random.Range(-0.5f, 0.5f), 
            direction.y + Random.Range(-0.2f, 0.2f), direction.z + Random.Range(-0.5f, 0.5f));
        bulletObject.transform.rotation = Quaternion.LookRotation(direction);
        bulletObject.transform.Rotate(90, 0, 0);
        Bullet bullet = bulletObject.GetComponent<Bullet>();

        bullet.Launch(bulletSpeed, direction.normalized);

        Destroy(bulletObject, 3f);
    }
}
