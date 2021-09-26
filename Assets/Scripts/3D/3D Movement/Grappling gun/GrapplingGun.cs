using UnityEngine;

public class GrapplingGun : MonoBehaviour
{
    // Components
    private LineRenderer lr;
    private SpringJoint joint;

    [Header("Transforms")]
    [SerializeField] private Transform gunTip, player;
    [SerializeField] private LayerMask whatIsGrappleable;
    private Vector3 grapplePoint;
    private Transform cam;

    [Header("Variables")]
    [SerializeField] private float maxGrappleDistance = 100f;
    [SerializeField] private float springForce = 4.5f, springDamp = 7f, massScale = 4.5f;
    [SerializeField] private Transform dot;
    private Vector3 dotScale;


    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        cam = Camera.main.transform;
        dotScale = dot.localScale;

    }

    void Update()
    {
        if (dot != null && Physics.Raycast(cam.position, cam.forward, maxGrappleDistance, whatIsGrappleable))
        {
            dot.localScale = 1.2f * Vector3.one;
        }
        else dot.localScale = dotScale;
        if (Input.GetMouseButtonDown(0))
        {
            StartGrapple();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopGrapple();
        }
    }

    //Called after Update
    void LateUpdate()
    {
        DrawRope();
    }

    /// <summary>
    /// Call whenever we want to start a grapple
    /// </summary>
    void StartGrapple()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, maxGrappleDistance, whatIsGrappleable))
        {
            grapplePoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

            //The distance grapple will try to keep from grapple point. 
            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

            //Adjust these values to fit your game.
            joint.spring = springForce;
            joint.damper = springDamp;
            joint.massScale = massScale;

            lr.positionCount = 2;
            currentGrapplePosition = gunTip.position;
        }
    }


    /// <summary>
    /// Call whenever we want to stop a grapple
    /// </summary>
    void StopGrapple()
    {
        lr.positionCount = 0;
        Destroy(joint);
    }

    private Vector3 currentGrapplePosition;

    void DrawRope()
    {
        //If not grappling, don't draw rope
        if (!joint) return;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * 8f);

        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, currentGrapplePosition);
    }

    public bool IsGrappling()
    {
        return joint != null;
    }

    public Vector3 GetGrapplePoint()
    {
        return grapplePoint;
    }
}