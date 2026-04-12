using UnityEngine;
using UnityEngine.AI;
using UnityEngine.ProBuilder;
using UnityEngine.Rendering;

public class BossAI : MonoBehaviour, IDamagable
{
    [Header("References")]
    private NavMeshAgent agent;

    public LayerMask whatIsPlayer;

    [Header("Variables")]
    public float health;
    public float maxHealth;

    public float damage;
    public float sightRange;

    public int phaseCount;

    private float attackCooldown;
    public int timeBetweenAttacks;

    public int numberOfEnemiesToSpawn;

    public Transform player;

    private bool playerInAttackRange;
    private bool canAttack;

    public GameObject flame;
    public GameObject shootingPoint;

    public Animator animator;

    private void Start()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();

        health = maxHealth;
    }

    private void Update()
    {
        playerInAttackRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);

        if (!canAttack)
        {
            attackCooldown += Time.deltaTime;

            if (attackCooldown > timeBetweenAttacks)
            {
                attackCooldown = 0;
                canAttack = true;
            }

            Chase();
        }
        else
        {
            Attack();
        }

        if (health > maxHealth / 2)
        {
            phaseCount = 1;
        }
        else
        {
            phaseCount = 2;
        }
    }

    private void Chase()
    {
        Debug.Log("chase");
        agent.SetDestination(player.position);

        animator.Play("Walk");
    }

    private void Attack()
    {
        Debug.Log("attack");
        agent.SetDestination(transform.position);

        animator.Play("Attack");
    }

    private void ThrowFlame()
    {
        Rigidbody rb = Instantiate(flame, shootingPoint.transform.position, Quaternion.identity).GetComponent<Rigidbody>();

        rb.AddForce(transform.forward * 15f, ForceMode.Impulse);

        Destroy(rb.gameObject, 3);
    }

    private void ResetAttack()
    {
        canAttack = false;
    }

    private void TakeDamage(float damageTaken)
    {
        health -= damageTaken;
    }

    private void Die()
    {
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
