using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    private PlayerGrappling grappling;
    public DebuffUIController debuffUI;
    public GameObject winScreen;

    [Header("Movement")]
    public float moveSpeed;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool canJump;

    [HideInInspector] public float walkSpeed;
    [HideInInspector] public float sprintSpeed;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public LayerMask whatIsIce;

    [HideInInspector] public bool grounded;

    [HideInInspector] public bool onIce;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    public Transform _camera;

    private void Start()
    {
        grappling = GetComponent<PlayerGrappling>();

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        canJump = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f, whatIsGround);

        onIce = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f, whatIsIce);

        PlayerInput();
        SpeedControl();

        // handle drag
        if (grounded)
        {
            rb.linearDamping = groundDrag;
            grappling.currentGrappleNo = grappling.maxNoOfGrapples;

            grappling.grappleNoUI.text = "Number Of Grapples: " + grappling.currentGrappleNo;
        }

        else
        {
            rb.linearDamping = 0;
        }

        if (onIce)
        {
            grappling.currentGrappleNo = grappling.maxNoOfGrapples;

            grappling.grappleNoUI.text = "Number Of Grapples: " + grappling.currentGrappleNo;

            debuffUI.isOnIce = true;
        }

        else
        {
            debuffUI.isOnIce = false;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();

        if (!canJump)
        {
            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void PlayerInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        if (Input.GetKeyDown(jumpKey) && canJump && grounded)
        {
            canJump = false;
        }

        else if (Input.GetKeyDown(jumpKey) && canJump && onIce)
        {
            canJump = false;
        }
    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on ground
        if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // in air
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 3f, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        // limit velocity if needed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        // reset y velocity
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        canJump = true;
    }

    private void Win()
    {
        Time.timeScale = 0;

        winScreen.gameObject.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PoisonTrigger"))
        {
            jumpForce = jumpForce / 2;
            moveSpeed = moveSpeed / 2;
        }

        if (other.gameObject.CompareTag("WinTrigger"))
        {
            Win();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PoisonTrigger"))
        {
            jumpForce = jumpForce * 2;
            moveSpeed = moveSpeed * 2;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position - Vector3.up, 1);
    }
}
