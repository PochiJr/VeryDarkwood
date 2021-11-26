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

    // Receive Damage
    private bool heSidoAtacado = false;
    private float ayuda = 0.0f;
    private float vida = 1.0f;
    public GameObject hitbox;

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
            if (playerInSightRange && !playerInAttackRange) ChasePlayer();
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

        // atacar
        if (!alreadyAttacked)
        {
            // Codigo del ataque
            // Codigo del ataque
            // Cambiar animacion a ataque
            animator.SetBool("isAttacking", true);
            alreadyAttacked = true;
            agent.SetDestination(transform.position);
            transform.LookAt(player);
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }


        // recibir daño
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Atacar") &&
            !animator.GetCurrentAnimatorStateInfo(0).IsName("Arbol Golpeado"))
        {
            agent.SetDestination(player.position);
        }


        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Arbol Golpeado") && heSidoAtacado) {

            ayuda -= Time.deltaTime;
            Debug.Log(ayuda);
          
        }
        if (ayuda <= 0)
        {
            animator.SetBool("isTakingDamage", false);
            heSidoAtacado = false;
            agent.SetDestination(player.position);
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Arbol Golpeado"))
        {
            // Reducir salud
            agent.SetDestination(transform.position);
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
        animator.SetBool("isAttacking", false);

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

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == "ArmaPalo")
        {
            vida -= 0.25f;
            if (vida <= 0)
            {
                animator.SetBool("isDead", true);
                agent.SetDestination(transform.position);

            }else
            {
                animator.SetBool("isTakingDamage", true);
                animator.SetBool("isAttacking", false);
            }
            
            hitbox.GetComponent<BoxCollider>().enabled = false;
            heSidoAtacado = true;
            ayuda = 0.01f;

        }
    }

}
