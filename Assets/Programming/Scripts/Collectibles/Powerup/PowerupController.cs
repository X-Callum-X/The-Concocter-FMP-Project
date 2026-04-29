using UnityEngine;

public class PowerupController : MonoBehaviour
{
    private PlayerHealth playerHealth;
    private PlayerMovement playerMovement;
    private PlayerGrappling playerGrappling;

    public ParticleSystem collectEffect;

    [SerializeField] private float rotationSpeed;
    [SerializeField] private PowerupSO powerup;

    public AudioSource source;
    public AudioClip collect;

    private void Start()
    {
        playerHealth = FindFirstObjectByType<PlayerHealth>();
        playerMovement = FindFirstObjectByType<PlayerMovement>();
        playerGrappling = FindFirstObjectByType<PlayerGrappling>();
    }

    private void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime, 0, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Instantiate(collectEffect, transform.position, Quaternion.identity);

            playerHealth.currentHealth += powerup.healthBoost;
            playerMovement.moveSpeed += powerup.speedBoost;
            playerGrappling.maxNoOfGrapples += powerup.grappleCount;

            playerHealth.UpdateHealthUI();

            source.PlayOneShot(collect);

            Destroy(this.gameObject);
        }
    }
}
