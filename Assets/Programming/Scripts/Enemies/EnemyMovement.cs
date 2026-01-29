using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform player;
    public float speed;
    public float minDistFromPlayer = 0.5f;

    private void Update()
    {
        if (Vector3.Distance(transform.position, player.position) > minDistFromPlayer)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }

        LookAtPlayer(player.position);
    }

    private void LookAtPlayer(Vector3 lookAt)
    {
        Quaternion lookRotation = Quaternion.LookRotation((lookAt - transform.position).normalized);

        transform.rotation = lookRotation;
    }
}
