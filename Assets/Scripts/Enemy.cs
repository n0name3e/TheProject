using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class Enemy : MonoBehaviour
{
    private static readonly int AttackHash = Animator.StringToHash("Attack");

    [field: SerializeField] public float MaxHealth { get; private set; } = 4f;
    public float Health { get; private set; } = 4f;
    [SerializeField] private bool isRanged = true;

    [SerializeField] private float movementSpeed = 3f;
    [Space(5)]
    //[SerializeField] private float attackRange = 5f;
    [SerializeField] private float attackIdleCooldown = 1f;
    [SerializeField] private float hardIdleCooldown = 0.75f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float hardAttackCooldown = 0.75f;
    public float attackRange = 5f;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float inaccuracyMultiplier = 1f; // less is more accurate
    [Space(5)]
    [SerializeField] private Animator weaponAnimator;
    [SerializeField] private Sledgehammer weapon;

    [SerializeField] private Transform player;
    [SerializeField] private Transform shootPosition;

    public float idleTimer { get; private set; } = 0f;
    public float attackTimer { get; private set; } = 0f;

    [SerializeField] private NavMeshAgent agent;
    private EnemyAI ai;
    [SerializeField] private ParticleSystem shootParticles;
    [SerializeField] private ParticleSystem hitParticle;

    private BulletPoolManager bulletPoolManager;


    [Header("Aduio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip hitSound;

    [SerializeField] private AudioClip[] footstepClips;
    [SerializeField] private float footstepInterval = 0.5f;
    private float footstepTimer = 0f;

    private void Awake()
    {
        //agent = GetComponentInParent<NavMeshAgent>();
        if (audioSource == null)
        {
            audioSource = GetComponentInChildren<AudioSource>();
        }
        ai = GetComponentInParent<EnemyAI>();
    }
    
    void Start()
    {
        if (GameDifficulty.difficulty == DifficultyLevel.Easy)
        {
            MaxHealth--;
        }
        if (GameDifficulty.difficulty == DifficultyLevel.Hard)
        {
            attackCooldown = hardAttackCooldown;
            attackIdleCooldown = hardIdleCooldown;
        }
        Health = MaxHealth;
        agent.speed = movementSpeed;
        if (player == null)
        {
            player = FindAnyObjectByType<PlayerMovement>().transform;
        }
        bulletPoolManager = BulletPoolManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) // player died
            return;
        attackTimer -= Time.deltaTime;
        idleTimer -= Time.deltaTime;
        HandleStepSounds();
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
        audioSource.PlayOneShot(hitSound);
        if (Health <= 0)
        {
            if (TryGetComponent(out Drop drop))
            {
                drop.DropItem();
            }
            StatsManager.Instance.kills++;
            audioSource.transform.SetParent(null, true);
            Destroy(audioSource.gameObject, 2f);
            Destroy(transform.parent.gameObject);
            return;
        }
        if (!ai.isChasing)
        {
            ai.ActivateChasing();
        }
    }
    public void Attack()
    {
        if (attackTimer > 0)
        {
            return;
        }
        attackTimer = attackCooldown;
        idleTimer = attackIdleCooldown;
        if (isRanged)
        {
            RangedAttack();
        }
        else
        {
            MeleeAttack();
        }

    }
    private void HandleStepSounds()
    {
        footstepTimer -= Time.deltaTime;
        if (footstepTimer > 0f || footstepClips.Length == 0) // for snipers
        {
            return;
        }
        float movingSpeed = agent.velocity.sqrMagnitude;
        if (movingSpeed <= 0.1f)
        {
            return;
        }
        int index = Random.Range(0, footstepClips.Length);
        audioSource.pitch = Random.Range(0.85f, 1.15f);
        audioSource.PlayOneShot(footstepClips[index]);
        footstepTimer = footstepInterval;
    }
    private void MeleeAttack()
    {
        weaponAnimator.SetTrigger(AttackHash);
        if (shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }
    }
    private void RangedAttack()
    {
        shootParticles.Play();
        audioSource.PlayOneShot(shootSound);
        //GameObject bulletObject = Instantiate(bulletPrefab, shootPosition.position, transform.rotation);
        Bullet bulletObject = bulletPoolManager.GetBullet();

        Vector3 direction = (player.position - transform.position).normalized;
        Vector3 inaccuracy = new Vector3(Random.Range(-0.2f, 0.2f) * inaccuracyMultiplier,
            Random.Range(-0.1f, 0.1f) * inaccuracyMultiplier, Random.Range(-0.2f, 0.2f) * inaccuracyMultiplier);
        direction += inaccuracy;

        bulletObject.transform.position = shootPosition.position;
        bulletObject.transform.rotation = Quaternion.LookRotation(direction);
        bulletObject.transform.Rotate(90, 0, 0);
        bulletObject.Launch(bulletSpeed, direction.normalized);

        //Destroy(bulletObject, 3f);
    }
    public void HitWithMeleeWeapon()
    {
        if (weapon == null)
        {
            return;
        }
        if (shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }
        weapon.Hit();
    }
}