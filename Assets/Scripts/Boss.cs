using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Boss : MonoBehaviour
{
    private static readonly int MinigunBashHash = Animator.StringToHash("MinigunBash");
    private static readonly int MinigunThrustHash = Animator.StringToHash("MinigunThrust");
    private static readonly int SummonSkeletonHash = Animator.StringToHash("SummonSkeleton");

    [field: SerializeField] public int MaxHealth { get; private set; } = 60;
    public int Health { get; private set; } = 60;

    [SerializeField] private float movementSpeed = 3.75f;
    [Space(5)]
    //[SerializeField] private float attackRange = 5f;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float inaccuracyMultiplier = 1f; // less is more accurate
    [Space(5)]
    [SerializeField] private Animator animator;
    [SerializeField] private Sledgehammer weapon;

    [SerializeField] private Transform player;
    private WeaponManager playerWeaponManager;
    [SerializeField] private Transform shootPosition;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject grenadePrefab;
    [SerializeField] private LayerMask validLayers;
    [SerializeField] private Transform eyePosition;
    [SerializeField] private GameObject keyHatch;
    [SerializeField] private List<EnemyAI> skeletons = new List<EnemyAI>();
    private List<EnemyAI> summonedSkeletons = new List<EnemyAI>();
    private BulletPoolManager bulletPoolManager;
    public float attackTimer { get; private set; } = 0f;
    public float idleTimer { get; private set; } = 0f; // when boss does'nt attack but move

    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private ParticleSystem shootParticles;
    [SerializeField] private ParticleSystem hitParticle;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip summonSkeletonSound;
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip grenadeThrowSound;
    [SerializeField] private AudioClip meleeAttackSound;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private GameObject bossRagdoll;

    [SerializeField] private AudioClip[] footstepClips;
    [SerializeField] private float footstepInterval = 0.8f;
    private float footstepTimer = 0f;
    private int minigunBullets = 20;
    private float distance = 0f;
    private void Awake()
    {
        //agent = GetComponentInParent<NavMeshAgent>();
        if (animator == null)
            animator = GetComponent<Animator>();
    }
    void Start()
    {
        if (GameDifficulty.difficulty == DifficultyLevel.Easy)
        {
            MaxHealth = 45;
            minigunBullets = 15;
        }
        else if (GameDifficulty.difficulty == DifficultyLevel.Medium)
        {
            MaxHealth = 60;
            minigunBullets = 20;
        }
        else if (GameDifficulty.difficulty == DifficultyLevel.Hard)
        {
            MaxHealth = 61;
            minigunBullets = 30;
        }
        Health = MaxHealth;
        agent.speed = movementSpeed;
        if (player == null)
        {
            player = FindAnyObjectByType<PlayerMovement>().transform;
        }
        playerWeaponManager = player.GetComponent<WeaponManager>();
        bulletPoolManager = BulletPoolManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) // player died
            return;
        distance = Vector3.Distance(player.position, transform.position);
        idleTimer -= Time.deltaTime;
        attackTimer -= Time.deltaTime;
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
    public void Hit(int damage = 1, bool useParticles = false)
    {
        Health -= damage;
        UI.Instance.SetBossHealth(Health, MaxHealth);
        if (hitSound != null)
        {
            audioSource.PlayOneShot(hitSound);
        }
        if (useParticles && hitParticle != null)
        {
            hitParticle.transform.position = transform.position;
            hitParticle.Emit(damage * 12);
        }
        if (Health <= 0)
        {
            keyHatch.transform.SetParent(null, true);
            keyHatch.SetActive(true);
            StatsManager.Instance.kills++;
            if (damage >= 2)
            {
                StatsManager.Instance.barrelKills++;
            }
            else if (playerWeaponManager.currentWeapon == WeaponType.Rifle)
            {
                StatsManager.Instance.rifleKills++;
            }
            else
            {
                StatsManager.Instance.pistolKills++;
            }
            if (summonedSkeletons.Count > 0)
            {
                foreach (EnemyAI skeleton in summonedSkeletons)
                {
                    if (skeleton != null)
                    {
                        skeleton.GetComponentInChildren<Enemy>().Hit(6, true);
                    }
                }
            }
            bossRagdoll.transform.SetParent(null, true);
            bossRagdoll.SetActive(true);
            Destroy(transform.parent.gameObject);
        }
    }
    public void Attack()
    {
        if (attackTimer > 0)
        {
            return;
        }
        //attackTimer = attackCooldown;
        // attacks
        float random = Random.Range(0, 100);
        if (distance <= 3f && random >= 10)
        {
            MeleeAttack();
            return;
        }

        if (random < 20 && skeletons.Count > 0)
        {
            SummonSkeleton();
        }
        else if (random < 60 && (CanSeePlayer() || random < 30))
        {
            StartCoroutine(MinigunBarrage());
        }
        else if (random < 80 || (distance <= 5f && random < 40))
        {
            GrenadeThrow();
        }
        else
        {
            attackTimer = 0.5f;
        }
    }
    private void MeleeAttack()
    {
        if (Random.Range(0, 100) < 60)
        {
            animator.Play(MinigunBashHash);
        }
        else
        {
            animator.Play(MinigunThrustHash);
        }
        audioSource.PlayOneShot(meleeAttackSound);
        if (GameDifficulty.difficulty == DifficultyLevel.Hard)
        {
            idleTimer = 1.5f;
            attackTimer = 2f;
        }
        else { 
        idleTimer = 2f;
        attackTimer = 3f;
    }
    }
    private IEnumerator MinigunBarrage()
    {
        attackTimer = 5f; // just in case some attack would like to interfere
        idleTimer = 5f;
        for (int i = 0; i < minigunBullets; i++)
        {
            if (Health <= 0)
                break;
            RangedAttack();
            yield return new WaitForSeconds(0.1f);
        }
        if (GameDifficulty.difficulty == DifficultyLevel.Hard)
        {
            idleTimer = 1f;
            attackTimer = 1.25f;
        }
        else { 
        idleTimer = 2f;
        attackTimer = 4f; // actual no-attack time
    }
    }
    private void GrenadeThrow()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        if (distance <= 5f)
        {
            direction += Vector3.up / 2;
        }
        else
        {
            direction += Vector3.up;
        }

        GameObject grenade = Instantiate(grenadePrefab, shootPosition.position, Quaternion.identity);
        grenade.GetComponent<Rigidbody>().AddForce(20 * direction, ForceMode.Impulse);

        audioSource.PlayOneShot(grenadeThrowSound);
        if (GameDifficulty.difficulty == DifficultyLevel.Hard)
        {
            idleTimer = 1f;
            if (distance <= 5f)
                attackTimer = 1.5f;
            else
                attackTimer = 2.5f;

        }
        else
        {
            idleTimer = 1f;
            attackTimer = 3f;
        }
    }
    private void SummonSkeleton()
    {
        animator.Play(SummonSkeletonHash);
        float currentMinDistance = Mathf.Infinity;
        EnemyAI currentSkeleton = null;
        foreach (EnemyAI skeleton in skeletons)
        {
            if (skeleton.gameObject.activeInHierarchy)
                continue;
            float dist = Vector3.Distance(transform.position, skeleton.transform.position);
            if (dist < currentMinDistance)
            {
                currentSkeleton = skeleton;
                currentMinDistance = dist;
            }
        }
        if (currentSkeleton != null)
        {
            currentSkeleton.gameObject.SetActive(true);
            currentSkeleton.ActivateChasing();
            summonedSkeletons.Add(currentSkeleton);
            skeletons.Remove(currentSkeleton);
            audioSource.PlayOneShot(summonSkeletonSound);
            if (GameDifficulty.difficulty == DifficultyLevel.Hard)
            {
                idleTimer = 1f;
                attackTimer = 2f;
            }
            else
            {
                idleTimer = 1f;
                attackTimer = 3f;
            }
        }
        else
        {
            StartCoroutine(MinigunBarrage());
        }

    }
    private void RangedAttack()
    {
        shootParticles.Play();
        //GameObject bulletObject = GetBulletFromPool();
        //if (bulletObject == null)
        //    return;
        //GameObject bulletObject = Instantiate(bulletPrefab, shootPosition.position, transform.rotation);
        Bullet bulletObject = bulletPoolManager.GetBullet();
        bulletObject.transform.position = shootPosition.position;

        Vector3 direction = (player.position - transform.position).normalized;
        Vector3 inaccuracy = new Vector3(Random.Range(-0.2f, 0.2f) * inaccuracyMultiplier,
            Random.Range(-0.1f, 0.1f) * inaccuracyMultiplier, Random.Range(-0.2f, 0.2f) * inaccuracyMultiplier);
        direction += inaccuracy;
        bulletObject.transform.rotation = Quaternion.LookRotation(direction);
        bulletObject.transform.Rotate(90, 0, 0);
        Bullet bullet = bulletObject.GetComponent<Bullet>();
        bullet.Launch(bulletSpeed, direction.normalized);

        audioSource.PlayOneShot(shootSound);

        //Destroy(bulletObject, 3f);
    }
    public bool CanSeePlayer()
    {
        RaycastHit hit;
        if (Physics.Raycast(eyePosition.position, (player.position - new Vector3(0, 1.2f, 0)) - transform.position, out hit, 100f, validLayers))
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                return true;
            }
        }
        return false;
    }
    public void MinigunBash()
    {
        weapon.Hit();
    }
}