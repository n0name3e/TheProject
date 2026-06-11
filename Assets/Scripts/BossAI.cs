using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossAI : MonoBehaviour
{
    [SerializeField] private Transform player;

    private bool isActive = false;

    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Boss boss;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();
        if (boss == null)
            boss = GetComponentInChildren<Boss>();

        if (player == null)
        {
            player = FindAnyObjectByType<PlayerMovement>().transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // player is dead
        if (player == null)
            return;
        if (isActive)
        {
            ChasePlayer();
        }
    }

    private void ChasePlayer()
    {
        if (boss.idleTimer <= 0)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
        if (boss.attackTimer <= 0)
        {
            boss.Attack();
        }
        /*if (Vector3.Distance(player.position, transform.position) <= attackRange)
        {
            agent.isStopped = true;

            if (CanSeePlayer())
                boss.Attack();
        }
        else
        {
            if (boss.attackTimer <= 0)
            {
                agent.isStopped = false;
                agent.SetDestination(player.position);
            }
        }*/
    }
    // can be called from some events
    public void ActivateBoss()
    {
        isActive = true;
    }
}