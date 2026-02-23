using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private float currentHealth = 0;
    private float maxHealth = 100;

    [HideInInspector] public float damage = 5;

    public TMP_Text healthUI;

    public EnemyHealth enemy;

    private void Start()
    {
        currentHealth = maxHealth;

        if (healthUI != null)
        {
            healthUI.text = "Player Health: " + currentHealth.ToString();
        }
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

        currentHealth -= enemy.damage;

        healthUI.text = "Player Health: " + currentHealth.ToString();
    }

    private void Die()
    {
        // Trigger what happens when the player dies

        Debug.Log("Player has died");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage();
        }
    }
}
