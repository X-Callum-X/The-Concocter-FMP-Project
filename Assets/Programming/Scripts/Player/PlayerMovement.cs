using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]

    [SerializeField] private Camera followCamera;

    private CharacterController playerController;

    [Header("Variables")]

    public int health;

    private float healTimer = 0;

    public float currentMoveSpeed = 5f;
    public float originalMoveSpeed = 5f;
    public float jumpHeight = 1.0f;

    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float gravity = -9.81f;

    private Vector3 moveDirection;
    private bool isGrounded;
    private bool isRunning = false;

    [Header("Respawning")]
    public Vector3 placeToRespawn;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Movement();

        if (transform.position.y <= -10 || health <= 0)
        {
            Die();
        }

        if (health < 5)
        {
            healTimer += Time.deltaTime;

            if (healTimer >= 5)
            {
                health += 1;
                healTimer = 0;
            }
        }
    }

    void Movement()
    {
        isGrounded = playerController.isGrounded;

        if (isGrounded && moveDirection.y < 0)
        {
            moveDirection.y = 0f;
        }

        bool pressingShift = Input.GetKey(KeyCode.LeftShift);
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 movementInput = Quaternion.Euler(0, followCamera.transform.eulerAngles.y, 0) * new Vector3(horizontalInput, 0, verticalInput);
        Vector3 movementDirection = movementInput;

        playerController.Move(movementDirection.normalized * currentMoveSpeed * Time.deltaTime);

        if (pressingShift && !isRunning)
        {
            currentMoveSpeed *= 1.5f;
            isRunning = true;
        }
        else if (!pressingShift)
        {
            currentMoveSpeed = originalMoveSpeed;
            isRunning = false;
        }

        if (movementDirection != Vector3.zero)
        {
            Quaternion desiredRotation = Quaternion.LookRotation(movementDirection, Vector3.up);

            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);
        }

        if (Input.GetButton("Jump") && isGrounded)
        {
            moveDirection.y += Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }

        if (!isGrounded)
        {
            moveDirection.y += gravity * Time.deltaTime;
        }

        playerController.Move(moveDirection * Time.deltaTime);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        healTimer = 0;

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        playerController.enabled = false;
        transform.position = placeToRespawn;
        playerController.enabled = true;

        health = 5;
    }
}
