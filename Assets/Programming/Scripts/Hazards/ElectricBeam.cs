using System.Collections;
using UnityEngine;

public class ElectricBeam : MonoBehaviour
{
    public Transform player;

    public GameObject warningBeam;
    public GameObject laserBeam;

    public float attackDuration;
    public float timeBetweenAttacks;

    private bool hasAttacked;

    public LayerMask whatIsPlayer;

    private void Update()
    {
        if (!hasAttacked)
        {
            StartCoroutine(Shoot(player.position));
        }
    }

    private IEnumerator Shoot(Vector3 lookAt)
    {
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
}
