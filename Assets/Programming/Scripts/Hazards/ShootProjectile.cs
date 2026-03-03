using UnityEngine;
using UnityEngine.ProBuilder;

public class ShootProjectile : MonoBehaviour
{
    private bool hasAttacked;

    public float timeBetweenAttacks;

    public float attackRange;

    public GameObject projectile;
    public GameObject shootingPoint;

    public Transform player;

    public bool playerInAttackRange;

    public LayerMask whatIsPlayer;

    private void Update()
    {
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (playerInAttackRange)
        {
            LookAt(player.position);

            Shoot();
        }
    }

    private void Shoot()
    {
        if (!hasAttacked)
        {
            Rigidbody rb = Instantiate(projectile, shootingPoint.transform.position, Quaternion.identity).GetComponent<Rigidbody>();

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

    private void LookAt(Vector3 lookAt)
    {
        Quaternion lookRotation = Quaternion.LookRotation((lookAt - transform.position).normalized);

        transform.rotation = lookRotation;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
