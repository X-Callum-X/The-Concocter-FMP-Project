using UnityEngine;
using TMPro;

public class EnemyHealth : MonoBehaviour, IDamagable
{
    private float currentHealth = 0;
    private float maxHealth = 100;

    [HideInInspector] public float damage = 10;

    public PlayerHealth player;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void TakeDamage(float damage)
    {
        // Called whenever the player takes any damage

        currentHealth -= damage;
    }

    private void Die()
    {
        // Trigger what happens when the player dies

        Debug.Log("Player has died");
    }

    private void OnCollisionEnter(Collision collision)
    {
        IDamagable target = collision.gameObject.GetComponentInParent<IDamagable>();
        
        if (target != null)
        {
            target.TakeDamage(damage);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        IDamagable target = other.gameObject.GetComponentInParent<IDamagable>();

        if (target != null)
        {
            target.TakeDamage(damage);
        }
    }
}
