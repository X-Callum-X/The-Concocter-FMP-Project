using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.ProBuilder;

public class ShootLaser : MonoBehaviour
{
    public Transform player;

    public GameObject warningBeam;
    public GameObject laserBeam;

    public float attackDuration;
    public float timeBetweenAttacks;
    public float attackRange;

    private bool hasAttacked;

    private bool playerInAttackRange;

    public LayerMask whatIsPlayer;

    private void Update()
    {
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!hasAttacked && playerInAttackRange)
        {
            StartCoroutine(Shoot(player.position));
        }
    }

    private IEnumerator Shoot(Vector3 lookAt)
    {
        Quaternion lookRotation = Quaternion.LookRotation((lookAt - transform.position).normalized);

        transform.rotation = lookRotation;

        hasAttacked = true;

        warningBeam.SetActive(true);

        yield return new WaitForSeconds(1f);

        warningBeam.SetActive(false);
        laserBeam.SetActive(true);

        yield return new WaitForSeconds(attackDuration);

        laserBeam.SetActive(false);

        Invoke(nameof(ResetAttack), timeBetweenAttacks);
    }

    private void ResetAttack()
    {
        hasAttacked = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
