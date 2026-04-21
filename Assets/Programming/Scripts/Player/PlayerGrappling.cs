using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class PlayerGrappling : MonoBehaviour
{
    [Header("References")]
    public LineRenderer lr;
    public Transform gunTip, cam, player;
    public LayerMask whatIsGrappleable;
    public PlayerMovement pm;
    public TMP_Text grappleNoUI;

    private PlayerHealth playerHealth;

    public Image reticle;

    [Header("Swinging")]
    private float maxSwingDistance = 25f;
    private Vector3 swingPoint;
    private SpringJoint joint;

    public float maxNoOfGrapples = 1;
    public float currentGrappleNo = 0;

    private bool noSwing;

    [Header("Movement")]
    public Transform orientation;
    public Rigidbody rb;
    public float horizontalThrustForce;
    public float forwardThrustForce;
    public float extendCableSpeed;

    [Header("Prediction")]
    public RaycastHit predictionHit;
    public float predictionSphereCastRadius;

    [Header("Input")]
    public KeyCode swingKey = KeyCode.Mouse0;

    private void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();

        currentGrappleNo = maxNoOfGrapples;

        grappleNoUI.text = "Number Of Grapples: " + currentGrappleNo;
    }

    private void Update()
    {
        if (Input.GetKeyDown(swingKey) && currentGrappleNo > 0 && !playerHealth.isDead) StartSwing();
        if (Input.GetKeyUp(swingKey)) StopSwing();

        CheckForSwingPoints();

        if (joint != null) Movement();
    }

    private void LateUpdate()
    {
        DrawRope();
    }

    private void CheckForSwingPoints()
    {
        if (joint != null)
        {
            reticle.GetComponent<Image>().color = new Color32(150, 150, 150, 100);
            return;
        }

        RaycastHit sphereCastHit;
        Physics.SphereCast(cam.position, predictionSphereCastRadius, cam.forward,
                            out sphereCastHit, maxSwingDistance, whatIsGrappleable);

        RaycastHit raycastHit;
        Physics.Raycast(cam.position, cam.forward,
                            out raycastHit, maxSwingDistance, whatIsGrappleable);

        Vector3 realHitPoint;

        // Direct Hit
        if (raycastHit.point != Vector3.zero)
        {
            reticle.GetComponent<Image>().color = new Color32(255, 255, 255, 100);
            realHitPoint = raycastHit.point;
            noSwing = false;
        }

        // Indirect (predicted) Hit
        else if (sphereCastHit.point != Vector3.zero)
        {
            reticle.GetComponent<Image>().color = new Color32(150, 150, 150, 100);
            realHitPoint = sphereCastHit.point;
            noSwing = false;
        }

        // Miss
        else
        {
            realHitPoint = Vector3.zero;
            noSwing = true;
        }

        predictionHit = raycastHit.point == Vector3.zero ? sphereCastHit : raycastHit;
    }


    private void StartSwing()
    {
        if (!noSwing) currentGrappleNo -= 1;

        if (!pm.grounded)
        {
            grappleNoUI.text = "Number Of Grapples: " + currentGrappleNo;
        }

        // return if predictionHit not found
        if (predictionHit.point == Vector3.zero) return;

        swingPoint = predictionHit.point;
        joint = player.gameObject.AddComponent<SpringJoint>();
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = swingPoint;

        float distanceFromPoint = Vector3.Distance(player.position, swingPoint);

        // the distance grapple will try to keep from grapple point. 
        joint.maxDistance = distanceFromPoint * 0.8f;
        joint.minDistance = distanceFromPoint * 0.25f;

        // customize values as you like
        joint.spring = 4.5f;
        joint.damper = 7f;
        joint.massScale = 4.5f;

        lr.positionCount = 2;
        currentGrapplePosition = gunTip.position;
    }

    public void StopSwing()
    {
        lr.positionCount = 0;

        Destroy(joint);
    }

    private void Movement()
    {
        // right
        if (Input.GetKey(KeyCode.D)) rb.AddForce(orientation.right * horizontalThrustForce * Time.deltaTime);
        // left
        if (Input.GetKey(KeyCode.A)) rb.AddForce(-orientation.right * horizontalThrustForce * Time.deltaTime);

        // forward
        if (Input.GetKey(KeyCode.W)) rb.AddForce(orientation.forward * horizontalThrustForce * Time.deltaTime);

        // shorten cable
        if (Input.GetKey(KeyCode.Space))
        {
            Vector3 directionToPoint = swingPoint - transform.position;
            rb.AddForce(directionToPoint.normalized * forwardThrustForce * Time.deltaTime);

            float distanceFromPoint = Vector3.Distance(transform.position, swingPoint);

            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;
        }
        // extend cable
        if (Input.GetKey(KeyCode.S))
        {
            float extendedDistanceFromPoint = Vector3.Distance(transform.position, swingPoint) + extendCableSpeed;

            joint.maxDistance = extendedDistanceFromPoint * 0.8f;
            joint.minDistance = extendedDistanceFromPoint * 0.25f;
        }
    }

    private Vector3 currentGrapplePosition;

    private void DrawRope()
    {
        // if not grappling, don't draw rope
        if (!joint) return;

        currentGrapplePosition =
            Vector3.Lerp(currentGrapplePosition, swingPoint, Time.deltaTime * 8f);

        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, currentGrapplePosition);
    }
}