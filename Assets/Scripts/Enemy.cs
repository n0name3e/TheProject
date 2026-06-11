/*using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [field: SerializeField] public float MaxHealth { get; private set; } = 4f;
    public float Health { get; private set; } = 4f;
    [SerializeField] private bool isRanged = true;

    [SerializeField] private float movementSpeed = 3f;
    [Space(5)]
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float bulletSpeed = 10f;
    [Space(5)]
    [SerializeField] private Animator weaponAnimator;

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
        agent.speed = movementSpeed;
        if (player == null)
        {
            player = FindAnyObjectByType<PlayerMovement>().transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
            return;
        attackTimer -= Time.deltaTime;
        if (Vector3.Distance(player.position, transform.position) <= attackRange)
        {
            agent.isStopped = true;
            if (attackTimer <= 0)
            {
                Attack();
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
        //hitParticles?.Play();
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }
    public void Attack()
    {
        attackTimer = attackCooldown;
        if (isRanged)
        {
            RangedAttack();
        }
        else
        {
            MeleeAttack();
        }
        
    }
    private void MeleeAttack()
    {
        weaponAnimator.SetTrigger("Attack");
    }
    private void RangedAttack()
    {
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
*/
using UnityEngine;
using UnityEngine.AI;
public class Enemy : MonoBehaviour
{
    [field: SerializeField] public float MaxHealth { get; private set; } = 4f;
    public float Health { get; private set; } = 4f;
    [SerializeField] private bool isRanged = true;

    [SerializeField] private float movementSpeed = 3f;
    [Space(5)]
    //[SerializeField] private float attackRange = 5f;
    [SerializeField] private float attackCooldown = 1f;
    public float attackRange = 5f;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float inaccuracyMultiplier = 1f; // less is more accurate
    [Space(5)]
    [SerializeField] private Animator weaponAnimator;

    [SerializeField] private Transform player;
    [SerializeField] private Transform shootPosition;
    [SerializeField] private GameObject bulletPrefab;

    public float attackTimer { get; private set; } = 0f;

    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private ParticleSystem shootParticles;
    [SerializeField] private ParticleSystem hitParticle;
    private void Awake()
    {
        //agent = GetComponentInParent<NavMeshAgent>();
    }
    void Start()
    {
        Health = MaxHealth;
        agent.speed = movementSpeed;
        if (player == null)
        {
            player = FindAnyObjectByType<PlayerMovement>().transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) // player died
            return;
        attackTimer -= Time.deltaTime;
        /*if (Vector3.Distance(player.position, transform.position) <= attackRange)
        {
            agent.isStopped = true;
            if (attackTimer <= 0)
            {
                Attack();
            }
    }
        else
        {
            if (attackTimer <= 0)
            {
                agent.isStopped = false;
                agent.SetDestination(player.position);
            }
        }*/
    }
    public void Hit(int damage = 1, bool useParticles = false)
    {
        Health -= damage;
        if (useParticles && hitParticle != null)
        {
            hitParticle.transform.position = transform.position;
            hitParticle.Emit(damage * 12);
        }
        if (Health <= 0)
        {
            Destroy(transform.parent.gameObject);
        }
    }
    public void Attack()
    {
        if (attackTimer > 0)
        {
            return;
        }
        attackTimer = attackCooldown;
        if (isRanged)
        {
            RangedAttack();
        }
        else
        {
            MeleeAttack();
        }

    }
    private void MeleeAttack()
    {
        weaponAnimator.SetTrigger("Attack");
    }
    private void RangedAttack()
    {
        shootParticles.Play();

        GameObject bulletObject = Instantiate(bulletPrefab, shootPosition.position, transform.rotation);

        Vector3 direction = (player.position - transform.position).normalized;
        Vector3 inaccuracy = new Vector3(Random.Range(-0.2f, 0.2f) * inaccuracyMultiplier,
            Random.Range(-0.1f, 0.1f) * inaccuracyMultiplier, Random.Range(-0.2f, 0.2f) * inaccuracyMultiplier);
        direction += inaccuracy;
        bulletObject.transform.rotation = Quaternion.LookRotation(direction);
        bulletObject.transform.Rotate(90, 0, 0);
        Bullet bullet = bulletObject.GetComponent<Bullet>();

        bullet.Launch(bulletSpeed, direction.normalized);

        Destroy(bulletObject, 3f);
    }
}