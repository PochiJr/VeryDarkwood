using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public Animator animator;
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    // Patroling
    public Vector3 walkPoint;
    private bool walkPointSet;
    public float walkPointRange;

    // Turning into bichoplanta
    public GameObject arbolMalvado;
    public float timeBetweenAttacks;
    private bool alreadyAttacked;

    // States
    public float sightRange, transformRange, attackRange;
    public bool playerInSightRange, playerInTransformRange, playerInAttackRange;

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
        playerInTransformRange = Physics.CheckSphere(transform.position, transformRange, whatIsPlayer);
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);


        if (isPlayerAtHome && playerInSightRange)
        {
            agent.SetDestination(spawnPoint);
        }
        else
        {
            if (playerInSightRange && !playerInAttackRange)ChasePlayer();
            else if (playerInAttackRange) AttackPlayer();
            if (!playerInSightRange && !playerInTransformRange && !playerInAttackRange) TurnIntoTree();
        }

    }

    /*private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();
        if (walkPointSet)
            agent.SetDestination(walkPoint);

        // Calculamos la distancia al walkPoint para ver si ya hemos llegado a él
        Vector3 distanceToWalkpoint = transform.position - walkPoint;
        if (distanceToWalkpoint.magnitude < 1f)
            walkPointSet = false;
    }*/

    /* private void SearchWalkPoint()
     {
         // Calculate random point in range
         float randomZ = Random.Range(-walkPointRange, walkPointRange);
         float randomX = Random.Range(-walkPointRange, walkPointRange);

         walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
         // Comprobamos que este walkPoint esté en el mapa que si no se va a suicidar el enemigo
         if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)) walkPointSet = true;
     }*/

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }
    private void AttackPlayer()
    {
        // Nos aseguramos de que el enemigo no se mueve mientras ataque
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            // Codigo del ataque
            // Codigo del ataque
            // Cambiar animacion a ataque
            Debug.Log("Te estoy atacando");

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void TurnIntoTree()
    {

        // Nos aseguramos de que el enemigo no se mueve mientras se transforma
        agent.SetDestination(transform.position);

        // Destruimos este objeto y nos convertimos en arbol
        Instantiate(arbolMalvado, transform.position, transform.rotation);
        Destroy(this.gameObject);

        
    }
    private void ResetAttack()
    {
         alreadyAttacked = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, transformRange);
    }
}
