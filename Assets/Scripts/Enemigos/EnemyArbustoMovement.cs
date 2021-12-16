using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyArbustoMovement : MonoBehaviour
{
    public Animator animator;
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    // Patroling
    public Vector3 walkPoint;
    private bool walkPointSet;
    public float walkPointRange;

    public float timeBetweenAttacks;
    private bool alreadyAttacked;

    // States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    // Return home
    public bool isPlayerAtHome = false;
    private Vector3 spawnPoint;

    // Receive Damage
    private bool heSidoAtacado = false;
    private float ayuda = 0.0f;
    private float vida = 0.5f;
    private float ttl = 4f;
    public GameObject hitbox;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        
    }

    private void Start()
    {
        spawnPoint = transform.position;
        agent.SetDestination(transform.position);
        agent.speed = 2.3f;
    }

    private void Update()
    {
        // Check for ranges
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
            if (!playerInSightRange && !playerInAttackRange) Camuflarse();
        }
        if(vida <= 0)
        {
            agent.SetDestination(transform.position);
            ttl -= Time.deltaTime;
            if(ttl <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }

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


        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Arbol Golpeado") && heSidoAtacado)
        {

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
    private void ResetAttack()
    {
        alreadyAttacked = false;
        animator.SetBool("isAttacking", false);

    }

    private void Camuflarse()
    {
        agent.SetDestination(spawnPoint);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
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

            }
            else
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
