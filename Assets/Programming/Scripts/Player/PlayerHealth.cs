using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IDamagable
{
    [Header("References")]
    public Slider healthSlider;
    public TMP_Text healthText;
    public EnemyHealth enemy;

    public DebuffUIController debuffUI;

    public GameObject gameOverScreen;

    [Header("Variables")]
    private float currentHealth = 100;
    private float maxHealth = 100;
    
    [HideInInspector] public float damage = 5;

    private bool Invincible = false;

    private float healTimer = 0;

    private bool pauseHealing = false;

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
            currentHealth = 0;

            UpdateHealthUI();

            Die();
        }

        if (currentHealth > maxHealth) currentHealth = maxHealth;

        if (currentHealth < maxHealth && !pauseHealing)
        {
            healTimer += Time.deltaTime;

            if (healTimer >= 0.5f)
            {
                currentHealth += 1;
                healTimer = 0;

                UpdateHealthUI();
            }
        }
    }

    public void TakeDamage(float damage)
    {
        // Called whenever the player takes any damage

        currentHealth -= damage;

        UpdateHealthUI();

        StartCoroutine(StopAndStartHealing());
    }

    public void Die()
    {
        // Trigger what happens when the player dies

        currentHealth = 0;

        UpdateHealthUI();

        Time.timeScale = 0;

        gameOverScreen.gameObject.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void UpdateHealthUI()
    {
        healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();

        healthSlider.value = currentHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DeathTrigger"))
        {
            Die();
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(10);
        }

        if (other.gameObject.CompareTag("Poison"))
        {
            TakeDamage(5);

            StartCoroutine(PoisonDamage(10, 5));
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("FireTrigger"))
        {
            StartCoroutine(FireDamage(15, 3));
        }
    }

    private IEnumerator StopAndStartHealing()
    {
        pauseHealing = true;

        yield return new WaitForSeconds(5f);

        pauseHealing = false;
    }

    private IEnumerator PoisonDamage(float damageAmount, float duration)
    {
        float amountDamaged = 0;
        float damagePerLoop = damageAmount / duration;

        while (amountDamaged < damageAmount)
        {
            debuffUI.isPoisoned = true;

            currentHealth -= damagePerLoop;

            amountDamaged += damagePerLoop;

            UpdateHealthUI();

            yield return new WaitForSeconds(1f);
        }

        if (amountDamaged == damageAmount)
        {
            debuffUI.isPoisoned = false;

            StartCoroutine(StopAndStartHealing());
        }
    }

    private IEnumerator FireDamage(float damageAmount, float duration)
    {
        float amountDamaged = 0;
        float damagePerLoop = damageAmount / duration;

        while (amountDamaged < damageAmount && !Invincible)
        {
            Invincible = true;
            debuffUI.isOnFire = true;

            currentHealth -= damagePerLoop;

            amountDamaged += damagePerLoop;

            UpdateHealthUI();

            yield return new WaitForSeconds(0.25f);
            Invincible = false;
        }

        if (amountDamaged == damageAmount)
        {
            debuffUI.isOnFire = false;

            StartCoroutine(StopAndStartHealing());
        }
    }
}