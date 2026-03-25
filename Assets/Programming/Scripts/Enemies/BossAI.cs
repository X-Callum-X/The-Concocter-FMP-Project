using UnityEngine;
using UnityEngine.AI;

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

    public int numberOfEnemiesToSpawn;

    public Transform player;

    private bool playerInAttackRange;

    private void Start()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();

        health = maxHealth;
    }

    private void Update()
    {
        playerInAttackRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);

        if (!playerInAttackRange)
        {
            Chase();
        }
        else
        {
            AttackPhase();
        }

        if (health > maxHealth)
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
        agent.SetDestination(player.position);
    }

    private void AttackPhase()
    {
        agent.SetDestination(transform.position);

        if (phaseCount == 1)
        {

        }
        else
        {

        }
    }

    private void PerformAttack()
    {

    }

    private void SpawnEnemies()
    {

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
