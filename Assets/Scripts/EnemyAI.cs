using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private LayerMask validLayers;
    private float attackRange = 5f;

    [SerializeField] private Transform player;
    // before chasing player, enemy will go here if not empty
    [SerializeField] private Transform startingDestination;
    [SerializeField] private Transform eyePosition;

    public bool isChasing = false;
    private bool hasGotToStartingDestionation = false;

    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Enemy enemy;

    [SerializeField] private float visionCheckInterval = 0.25f;
    private float visionTimer = 0f;
    private float moveTimer = 0f;

    public static List<EnemyAI> AllActiveEnemies = new List<EnemyAI>();

    void OnEnable()
    {
        AllActiveEnemies.Add(this);
    }
    void OnDisable()
    {
        AllActiveEnemies.Remove(this);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();
        if (enemy == null)
            enemy = GetComponentInChildren<Enemy>();

        if (player == null)
        {
            player = FindAnyObjectByType<PlayerMovement>().transform;
        }
        if (startingDestination == null)
        {
            hasGotToStartingDestionation = true;
        }
        attackRange = enemy.attackRange;
    }

    // Update is called once per frame
    void Update()
    {
        // player is dead
        if (player == null)
            return;
        if (isChasing)
        {
            if (!hasGotToStartingDestionation)
            {
                if (Vector3.Distance(transform.position, startingDestination.position) > 1.4f)
                {
                    agent.SetDestination(startingDestination.position);
                }
                else
                {
                    hasGotToStartingDestionation = true;
                }
            }
            else
            {
                //print(gameObject.name + " chasing");
                ChasePlayer();
            }

        }
        else
        {
            CheckPlayer();
        }
    }
    private void CheckPlayer()
    {
        visionTimer -= Time.deltaTime;
        if (visionTimer <= 0f)
        {
            visionTimer = visionCheckInterval; 

            if (CanSeePlayer())
            {
                isChasing = true;
                hasGotToStartingDestionation = true;
            }
        }
    }
    private bool CanSeePlayer()
    {
        if (Vector3.Distance(transform.position, player.position) > detectionRange)
        {
            return false;
        }
        RaycastHit hit;
        Debug.DrawRay(eyePosition.position, ((player.position - new Vector3(0, 0, 0)) - transform.position).normalized * 75, Color.red, 0.25f);
        if (Physics.Raycast(eyePosition.position, (player.position - new Vector3(0, 0, 0)) - transform.position, out hit, detectionRange, validLayers))
        {
            //print(gameObject.name + ": " + hit.transform.name);
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                return true;
            }
        }
        return false;
    }
    private void ChasePlayer()
    {

        if (Vector3.Distance(player.position + new Vector3(0, 1, 0), transform.position) <= attackRange)
        {
            if (CanSeePlayer())
            {
                agent.isStopped = true;

                enemy.Attack();
            }
        }
        else
        {
            if (moveTimer > 0)
            {
                moveTimer -= Time.deltaTime;
                return;
            }
            if (enemy.idleTimer <= 0)
            {
                agent.isStopped = false;
                agent.SetDestination(player.position);
            }
        }
    }
    // can be called from some events
    public void ActivateChasing()
    {
        isChasing = true;
    }
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStatic()
    {
        
        AllActiveEnemies.Clear();
    }
}