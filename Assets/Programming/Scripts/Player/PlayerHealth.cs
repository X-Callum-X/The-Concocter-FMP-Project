using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("References")]
    public Slider healthSlider;
    public TMP_Text healthUI;
    public EnemyHealth enemy;

    public DebuffUIController debuffUI;

    [Header("Variables")]
    private float currentHealth = 100;
    private float maxHealth = 100;

    [HideInInspector] public float damage = 5;

    private void Start()
    {
        currentHealth = maxHealth;

        healthSlider.maxValue = maxHealth;

        UpdateHealthUI();
    }

    private void Update()
    {
        if (currentHealth <= 0)
        {
            Die();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            currentHealth += 10;

            UpdateHealthUI();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            currentHealth -= 10;

            UpdateHealthUI();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            StopAllCoroutines();
            StartCoroutine(DamageOverTime(5, 5));
        }
    }

    private void TakeDamage()
    {
        // Called whenever the player takes any damage

        currentHealth -= enemy.damage;

        UpdateHealthUI();
    }

    private void Die()
    {
        // Trigger what happens when the player dies

        Debug.Log("Player has died");
    }

    private void UpdateHealthUI()
    {
        healthUI.text = currentHealth.ToString() + " / " + maxHealth.ToString();

        healthSlider.value = currentHealth;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage();
        }
    }

    private IEnumerator DamageOverTime(float damageAmount, float duration)
    {
        float amountDamaged = 0;
        float damagePerLoop = damageAmount / duration;

        while (amountDamaged < damageAmount)
        {
            debuffUI.isPoisoned = true;

            currentHealth -= damagePerLoop;
            Debug.Log(currentHealth.ToString());
            amountDamaged += damagePerLoop;

            UpdateHealthUI();

            yield return new WaitForSeconds(1f);
        }

        if (amountDamaged == damageAmount)
        {
            debuffUI.isPoisoned = false;
        }
    }
}