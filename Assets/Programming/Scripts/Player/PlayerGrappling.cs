using UnityEngine;

public class PlayerGrappling : MonoBehaviour
{
    [Header("References")]
    private PlayerMovement pm;
    public Transform cam;
    public Transform areaOfSpawn;
    public LayerMask whatIsGrappleable;
    public LineRenderer lr;

    [Header("Grappling")]
    public float maxGrappleDistance;
    public float grappleDelay;

    private Vector3 grapplePoint;

    [Header("Cooldown")]
    public float grappleCooldown;
    public float cooldownTimer;

    [Header("Input")]
    public KeyCode grappleKey = KeyCode.Mouse1;

    private bool isGrappling;

    private void Start()
    {
        pm = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(grappleKey)) StartGrapple();

        if(cooldownTimer > 0) cooldownTimer -= Time.deltaTime;
    }

    private void LateUpdate()
    {
        if(isGrappling) lr.SetPosition(0, areaOfSpawn.position);
    }

    private void StartGrapple()
    {
        if (cooldownTimer > 0) return;

        isGrappling = true;

        RaycastHit hit;

        if (Physics.Raycast(cam.position, cam.forward, out hit, maxGrappleDistance, whatIsGrappleable))
        {
            grapplePoint = hit.point;

            Invoke(nameof(ExecuteGrapple), grappleDelay);
        }
        else
        {
            grapplePoint = cam.position + cam.forward * maxGrappleDistance;

            Invoke(nameof(StopGrapple), grappleDelay);
        }

        lr.enabled = true;
        lr.SetPosition(1, grapplePoint);
    }

    private void ExecuteGrapple()
    {

    }

    private void StopGrapple()
    {
        isGrappling = false;

        cooldownTimer = grappleCooldown;

        lr.enabled = false;
    }
}
