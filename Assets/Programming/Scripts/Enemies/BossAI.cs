using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Collections;

public class BossAI : MonoBehaviour, IDamagable
{
    [Header("References")]
    private NavMeshAgent agent;

    public LayerMask whatIsPlayer;

    public GameObject winScreen;

    [Header("Variables")]
    public Slider healthSlider;

    public float currentHealth;
    public float maxHealth;

    private float attackCooldown;
    public float timeBetweenAttacks;

    public Transform player;

    private bool canAttack;
    private bool isDead;

    public GameObject flame;
    public GameObject shootingPoint;

    public Animator animator;

    private void Start()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();

        currentHealth = maxHealth;

        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;
    }

    private void Update()
    {
        if (currentHealth <= 0)
        {
            currentHealth = 0;

            Die();
        }

        if (!canAttack && !isDead)
        {
            attackCooldown += Time.deltaTime;

            if (attackCooldown > timeBetweenAttacks)
            {
                attackCooldown = 0;
                canAttack = true;
            }

            Chase();
        }
        else if (canAttack && !isDead)
        {
            Attack();
        }
    }

    private void Chase()
    {
        agent.SetDestination(player.position);

        animator.Play("Walk");
    }

    private void Attack()
    {
        agent.SetDestination(transform.position);

        animator.Play("Attack");
    }

    private void TakeDamage(float damageTaken)
    {
        currentHealth -= damageTaken;

        healthSlider.value = currentHealth;
    }

    private void Die()
    {
        isDead = true;
        animator.Play("Death");

        StartCoroutine(DelayWin());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BossDamageTrigger"))
        {
            Debug.Log("damage");
            TakeDamage(1);
        }
    }

    private void ThrowFlame()
    {
        Rigidbody rb = Instantiate(flame, shootingPoint.transform.position, Quaternion.identity).GetComponent<Rigidbody>();

        rb.AddForce(transform.forward * 15f, ForceMode.Impulse);

        Destroy(rb.gameObject, 3);
    }

    private void ResetAttack()
    {
        canAttack = false;
    }

    private IEnumerator DelayWin()
    {
        Debug.Log("You Win");

        yield return new WaitForSeconds(1.5f);

        winScreen.gameObject.SetActive(true);

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
