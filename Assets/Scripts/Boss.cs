using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Boss : MonoBehaviour
{
    private static readonly int MinigunBashHash = Animator.StringToHash("MinigunBash");
    private static readonly int MinigunThrustHash = Animator.StringToHash("MinigunThrust");
    private static readonly int SummonSkeletonHash = Animator.StringToHash("SummonSkeleton");

    [field: SerializeField] public float MaxHealth { get; private set; } = 60f;
    public float Health { get; private set; } = 60f;

    [SerializeField] private float movementSpeed = 3.75f;
    [Space(5)]
    //[SerializeField] private float attackRange = 5f;
    [SerializeField] private float attackCooldown = 4f;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float inaccuracyMultiplier = 1f; // less is more accurate
    [Space(5)]
    [SerializeField] private Animator animator;
    [SerializeField] private Sledgehammer weapon;

    [SerializeField] private Transform player;
    [SerializeField] private Transform shootPosition;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject grenadePrefab;
    [SerializeField] private LayerMask validLayers;
    [SerializeField] private Transform eyePosition;
    [SerializeField] private List<EnemyAI> skeletons = new List<EnemyAI>();
    private List<GameObject> bulletPool = new List<GameObject>();

    public float attackTimer { get; private set; } = 0f;
    public float idleTimer { get; private set; } = 0f; // when boss does'nt attack but move

    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private ParticleSystem shootParticles;
    [SerializeField] private ParticleSystem hitParticle;
    private void Awake()
    {
        //agent = GetComponentInParent<NavMeshAgent>();
        if (animator == null)
            animator = GetComponent<Animator>();
    }
    void Start()
    {
        Health = MaxHealth;
        agent.speed = movementSpeed;
        if (player == null)
        {
            player = FindAnyObjectByType<PlayerMovement>().transform;
        }
        for (int i = 0; i < 30; i++)
        {
            GameObject b = Instantiate(bulletPrefab);
            b.SetActive(false);
            bulletPool.Add(b);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) // player died
            return;
        idleTimer -= Time.deltaTime;
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
            Destroy(gameObject);
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
        if (Vector3.Distance(player.position, transform.position) <= 3f && random >= 10)
        {
            MeleeAttack();
            return;
        }

        if (random < 20 && skeletons.Count > 0)
        {
            print("skeleon");
            SummonSkeleton();
        }
        else if (random < 60 && CanSeePlayer())
        {
            StartCoroutine(MinigunBarrage());
        }
        else if (random < 80)
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
        idleTimer = 2f;
        attackTimer = 3f;
    }
    private IEnumerator MinigunBarrage()
    {
        print("minigun barrage");
        attackTimer = 5f; // just in case some attack would like to interfere
        idleTimer = 5f;
        for (int i = 0; i < 20; i++)
        {
            if (Health <= 0)
                break;
            RangedAttack();
            yield return new WaitForSeconds(0.1f);
        }
        idleTimer = 2f;
        attackTimer = 4f; // actual no-attack time
    }
    private GameObject GetBulletFromPool()
    {
        foreach (GameObject b in bulletPool)
        {
            if (!b.activeInHierarchy)
            {
                return b;
            }
        }
        return null; 
    }
    private void GrenadeThrow()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction += Vector3.up;

        GameObject grenade = Instantiate(grenadePrefab, shootPosition.position, Quaternion.identity);
        grenade.GetComponent<Rigidbody>().AddForce(20 * direction, ForceMode.Impulse);

        idleTimer = 1f;
        attackTimer = 3f;
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
            skeletons.Remove(currentSkeleton);
            idleTimer = 1f;
            attackTimer = 3f;
        }
        else
        {
            StartCoroutine(MinigunBarrage());
        }

    }
    private void RangedAttack()
    {
        shootParticles.Play();
        GameObject bulletObject = GetBulletFromPool();
        if (bulletObject == null)
            return;
        //GameObject bulletObject = Instantiate(bulletPrefab, shootPosition.position, transform.rotation);
        bulletObject.transform.position = shootPosition.position;

        Vector3 direction = (player.position - transform.position).normalized;
        Vector3 inaccuracy = new Vector3(Random.Range(-0.2f, 0.2f) * inaccuracyMultiplier,
            Random.Range(-0.1f, 0.1f) * inaccuracyMultiplier, Random.Range(-0.2f, 0.2f) * inaccuracyMultiplier);
        direction += inaccuracy;
        bulletObject.transform.rotation = Quaternion.LookRotation(direction);
        bulletObject.transform.Rotate(90, 0, 0);
        Bullet bullet = bulletObject.GetComponent<Bullet>();
        bullet.destroyOnHit = false;
        bulletObject.SetActive(true);
        bullet.Launch(bulletSpeed, direction.normalized);

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