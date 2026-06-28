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
        agent.updateRotation = false;
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
        FacePlayer();
    }
    private void FacePlayer()
    {
        // 1. Get the direction to the player
        Vector3 direction = (player.position - transform.position).normalized;

        // 2. Lock the Y axis so the boss stays flat on the floor
        direction.y = 0;

        // 3. If we are perfectly on top of the boss, avoid math errors
        if (direction == Vector3.zero) return;

        // 4. Calculate the rotation and apply it smoothly
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 8f);
    }
    private void ChasePlayer()
    {
        if (boss.idleTimer <= 0)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
        else
        {
            agent.isStopped = true;
        }
        if (boss.attackTimer <= 0)
        {
            boss.Attack();
        }
        if (boss.CanSeePlayer())
            boss.Attack();

        /*else
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