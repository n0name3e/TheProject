using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private bool isRanged = true;
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private LayerMask validLayers;
    private float attackRange = 5f;

    [SerializeField] private Transform player;
    // before chasing player, enemy will go here if not empty
    [SerializeField] private Transform startingDestination;
    [SerializeField] private Transform eyePosition;

    [SerializeField] private bool isChasing = false;
    private bool hasGotToStartingDestionation = false;

    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Enemy enemy;

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
        //print(gameObject.name + " Checking");
        if (CanSeePlayer())
        {
            isChasing = true;
            hasGotToStartingDestionation = true; // if enemy already can see player there are no need for scripted things
        }
    }
    private bool CanSeePlayer()
    {
        RaycastHit hit;
        Debug.DrawRay(eyePosition.position, ((player.position - new Vector3(0, 0, 0)) - transform.position) * 75, Color.red, 0.25f);
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
            if (enemy.attackTimer <= 0)
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
}