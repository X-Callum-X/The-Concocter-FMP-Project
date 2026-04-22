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

    private bool hasAttacked;
    private bool performedAttack;

    public GameObject shootingPoint;

    [Header("Animation")]

    public Animator animator;

    private void Update()
    {
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInAttackRange && !hasAttacked && !performedAttack)
        {
            ChasePlayer();
        }

        else
        {
            PerformAttack();
        }
    }

    private void ChasePlayer()
    {
        animator.Play("Chase");
        transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }
     
    private void PerformAttack()
    {
        performedAttack = true;
        animator.Play("Attack");
    }

    public void MeleeAttack()
    {
        if (!hasAttacked)
        {
            StartCoroutine(TriggerDamageArea());

            hasAttacked = true;
        }
    }

    public void RangedAttack()
    {
        if (!hasAttacked)
        {
            Rigidbody rb = Instantiate(projectile, shootingPoint.transform.position, Quaternion.identity).GetComponent<Rigidbody>();

            rb.AddForce(transform.forward * 15f, ForceMode.Impulse);

            Destroy(rb.gameObject, 3);

            hasAttacked = true;
        }
    }

    public void ResetAttack()
    {
        hasAttacked = false;

        performedAttack = false;
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
