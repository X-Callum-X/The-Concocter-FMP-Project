using UnityEngine;
using TMPro;

public class EnemyHealth : MonoBehaviour
{
    private float currentHealth = 0;
    private float maxHealth = 100;

    [HideInInspector] public float damage = 10;

    public TMP_Text healthUI;

    public PlayerHealth player;

    private void Start()
    {
        currentHealth = maxHealth;

        healthUI.text = "Enemy Health: " + currentHealth.ToString();
    }

    private void Update()
    {
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void TakeDamage()
    {
        // Called whenever the player takes any damage

        currentHealth -= player.damage;

        healthUI.text = "Enemy Health: " + currentHealth.ToString();
    }

    private void Die()
    {
        // Trigger what happens when the player dies

        Debug.Log("Player has died");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            TakeDamage();
        }
    }
}
