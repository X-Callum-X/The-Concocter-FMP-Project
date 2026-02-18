using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    public Transform player;

    private void Update()
    {
        LookAt(player.position);
    }

    private void LookAt(Vector3 lookAt)
    {
        Quaternion lookRotation = Quaternion.LookRotation((lookAt - transform.position).normalized);

        transform.rotation = lookRotation;
    }
}
