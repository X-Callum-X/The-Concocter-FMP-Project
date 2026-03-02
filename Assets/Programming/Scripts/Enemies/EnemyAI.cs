using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float speed;
    public LayerMask whatIsPlayer;

    public float attackRange;
    public bool playerInAttackRange;

    public GameObject damageArea;
    public GameObject projectile;

    public float timeBetweenAttacks;

    public bool hasAttacked;

    public bool isMelee;
    public bool isRanged;

    [Header("Animation")]

    public Animator chaseAnim;
    public Animator attackAnim;

    private void Update()
    {
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInAttackRange && !hasAttacked)
        {
            ChasePlayer();
        }

        else
        {
            if (isMelee)
            {
                MeleeAttack();
            }

            else if (isRanged)
            {
                RangedAttack();
            }
        }
    }

    private void ChasePlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }
     
    private void MeleeAttack()
    {
        if (!hasAttacked)
        {
            StartCoroutine(TriggerDamageArea());

            hasAttacked = true;

            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void RangedAttack()
    {
        if (!hasAttacked)
        {
            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();

            rb.AddForce(transform.forward * 15f, ForceMode.Impulse);

            Destroy(rb.gameObject, 3);

            hasAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        hasAttacked = false;
    }

    private IEnumerator TriggerDamageArea()
    {
        damageArea.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.1f);

        damageArea.gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
