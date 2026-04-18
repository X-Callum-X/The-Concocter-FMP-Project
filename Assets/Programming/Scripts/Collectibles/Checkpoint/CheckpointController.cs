using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    public float rotationSpeed;

    public ParticleSystem collectEffect;

    private void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime, 0, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Instantiate(collectEffect, transform.position, Quaternion.identity);

            Destroy(this.gameObject);
        }
    }
}