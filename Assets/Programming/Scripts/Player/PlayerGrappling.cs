using UnityEngine;

public class PlayerGrappling : MonoBehaviour
{
    [Header("References")]
    public LineRenderer lr;
    public Transform gunTip, cam, player;
    public LayerMask whatIsGrappleable;
    public PlayerMovement pm;

    [Header("Swinging")]
    private float maxSwingDistance = 25f;
    private Vector3 swingPoint;
    private SpringJoint joint;

    [Header("OdmGear")]
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


    private void Update()
    {
        if (Input.GetKeyDown(swingKey)) StartSwing();
        if (Input.GetKeyUp(swingKey)) StopSwing();

        CheckForSwingPoints();

        if (joint != null) OdmGearMovement();
    }

    private void LateUpdate()
    {
        DrawRope();
    }

    private void CheckForSwingPoints()
    {
        if (joint != null) return;

        RaycastHit sphereCastHit;
        Physics.SphereCast(cam.position, predictionSphereCastRadius, cam.forward,
                            out sphereCastHit, maxSwingDistance, whatIsGrappleable);

        RaycastHit raycastHit;
        Physics.Raycast(cam.position, cam.forward,
                            out raycastHit, maxSwingDistance, whatIsGrappleable);

        Vector3 realHitPoint;

        // Option 1 - Direct Hit
        if (raycastHit.point != Vector3.zero)
            realHitPoint = raycastHit.point;

        // Option 2 - Indirect (predicted) Hit
        else if (sphereCastHit.point != Vector3.zero)
            realHitPoint = sphereCastHit.point;

        // Option 3 - Miss
        else
            realHitPoint = Vector3.zero;

        //// realHitPoint found
        //if (realHitPoint != Vector3.zero)
        //{
        //    predictionPoint.gameObject.SetActive(true);
        //    predictionPoint.position = realHitPoint;
        //}
        //// realHitPoint not found
        //else
        //{
        //    predictionPoint.gameObject.SetActive(false);
        //}

        predictionHit = raycastHit.point == Vector3.zero ? sphereCastHit : raycastHit;
    }


    private void StartSwing()
    {
        // return if predictionHit not found
        if (predictionHit.point == Vector3.zero) return;

        // deactivate active grapple
        //if (GetComponent<Grappling>() != null)
        //    GetComponent<Grappling>().StopGrapple();

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

    private void OdmGearMovement()
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

    //[Header("References")]
    //private PlayerMovement pm;
    //public Transform cam;
    //public Transform gunTip;
    //public LayerMask whatIsGrappleable;
    //public LineRenderer lr;

    //[Header("Grappling")]
    //public float maxGrappleDistance;
    //public float grappleDelay;

    //private Vector3 grapplePoint;

    //[Header("Cooldown")]
    //public float grappleCooldown;
    //public float cooldownTimer;

    //[Header("Input")]
    //public KeyCode grappleKey = KeyCode.Mouse0;

    //private bool isGrappling;

    //private void Start()
    //{
    //    pm = GetComponent<PlayerMovement>();
    //}

    //private void Update()
    //{
    //    if (Input.GetKeyDown(grappleKey)) StartGrapple();

    //    if(cooldownTimer > 0) cooldownTimer -= Time.deltaTime;
    //}

    //private void LateUpdate()
    //{
    //    if(isGrappling) lr.SetPosition(0, gunTip.position);
    //}

    //private void StartGrapple()
    //{
    //    //if (cooldownTimer > 0) return;

    //    isGrappling = true;

    //    RaycastHit hit;

    //    if (Physics.Raycast(cam.position, cam.forward, out hit, maxGrappleDistance, whatIsGrappleable))
    //    {
    //        grapplePoint = hit.point;

    //        Invoke(nameof(ExecuteGrapple), grappleDelay);
    //    }
    //    else
    //    {
    //        grapplePoint = cam.position + cam.forward * maxGrappleDistance;

    //        Invoke(nameof(StopGrapple), grappleDelay);
    //    }

    //    lr.enabled = true;
    //    lr.SetPosition(1, grapplePoint);
    //}

    //private void ExecuteGrapple()
    //{

    //}

    //private void StopGrapple()
    //{
    //    isGrappling = false;

    //    cooldownTimer = grappleCooldown;

    //    lr.enabled = false;
    //}
