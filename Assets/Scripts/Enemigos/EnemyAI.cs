using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    // Patroling
    public Vector3 walkPoint;
    private bool walkPointSet;
    public float walkPointRange;

    // Turning into bichoplanta
    public GameObject bichoPlanta;
    public float timeBetweenAttacks;
    private bool alreadyAttacked;

    // States
    public float sightRange, stealthRange, transformRange;
    public bool playerInSightRange, playerInStealthRange, playerInTransformRange;

    // Return home
    public bool isPlayerAtHome = false;
    private Vector3 spawnPoint;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        spawnPoint = transform.position;
    }
    
    private void Update()
    {
        // Check for ranges
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInStealthRange = Physics.CheckSphere(transform.position, stealthRange, whatIsPlayer);
        playerInTransformRange = Physics.CheckSphere(transform.position, transformRange, whatIsPlayer);
        

        if (isPlayerAtHome && playerInSightRange)
        {
            agent.SetDestination(spawnPoint);
        } else {
            if (playerInStealthRange && !playerInTransformRange) StealthPlayer();
            else if (playerInSightRange && !playerInTransformRange) ChasePlayer();
            if (playerInStealthRange && playerInTransformRange) TurnIntoMonster();
            if (!playerInSightRange && !playerInStealthRange && !playerInTransformRange) agent.SetDestination(transform.position);
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }
    private void StealthPlayer()
    {
        agent.SetDestination(transform.position);
    }
    private void TurnIntoMonster()
    {
       
        // Nos aseguramos de que el enemigo no se mueve mientras ataque
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        // Destruimos este objeto y nos convertimos en mocho 
        Instantiate(bichoPlanta, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, transformRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, stealthRange);
    }
}
