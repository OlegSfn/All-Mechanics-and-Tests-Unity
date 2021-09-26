using UnityEngine;

public class GravityGun : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private float lerp;
    [SerializeField] private float throwForce;
    [SerializeField] private float maxTouchDistance;
    [SerializeField] private LayerMask whatIsTouchable;

    [Header("Transforms")]
    [SerializeField] private Transform objectPoint;
    [SerializeField] private Transform cam;
    [SerializeField] private Transform dot;

    private Rigidbody objRB;
    private CollisionDetectionMode colDetectMode;
    void Update()
    {
        if (objRB)
		{
            objRB.MovePosition(Vector3.Lerp(objRB.transform.position, objectPoint.position, lerp * Time.deltaTime));

            if (Input.GetMouseButtonDown(0))
            {
                objRB.isKinematic = false;
                objRB.collisionDetectionMode = colDetectMode;
                objRB.AddForce(cam.transform.forward * throwForce, ForceMode.VelocityChange);
                objRB = null;
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
		{
            if (objRB)
			{
                objRB.isKinematic = false;
                objRB.collisionDetectionMode = colDetectMode;
                objRB = null;
            }
			else
			{
                if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hit, maxTouchDistance, whatIsTouchable))
				{
                    objRB = hit.collider.GetComponent<Rigidbody>();
                    if (objRB)
					{
                        colDetectMode = objRB.collisionDetectionMode;
                        objRB.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
                        objRB.isKinematic = true;
                    }
				}
			}
		}

        
        // Dot
        if (Physics.Raycast(cam.position, cam.forward, maxTouchDistance, whatIsTouchable) && !objRB)
            dot.localScale = Vector3.one * 1.2f;
        else
            dot.localScale = Vector3.one;
        
    }
}
