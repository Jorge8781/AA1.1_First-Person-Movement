using UnityEngine;

public class ObjectPickup : MonoBehaviour
{
    public float pickupRange = 2f;
    public float holdDistance = 2f;
    public float moveSpeed = 10f;

    public float minHoldDistance = 1f;
    public float maxHoldDistance = 5f;
    public float scrollSensitivity = 1f;

    public LayerMask pickupLayer;

    private Rigidbody heldObject;
    private Transform cam;

    void Start()
    {
        cam = Camera.main.transform;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (heldObject == null)
            {
                TryPickupObject();
            }
            else
            {
                AdjustHoldDistance();
                HoldObject();
            }
        }
        else if (heldObject != null)
        {
            DropObject();
        }
    }

    void TryPickupObject()
    {
        Ray ray = new Ray(cam.position, cam.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickupRange, pickupLayer))
        {
            Rigidbody rb = hit.collider.GetComponent<Rigidbody>();

            if (rb != null)
            {
                heldObject = rb;
                heldObject.useGravity = false;
                heldObject.linearDamping = 10f;
            }
        }
    }

    void AdjustHoldDistance()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0)
        {
            holdDistance += scroll * scrollSensitivity;
            holdDistance = Mathf.Clamp(
                holdDistance,
                minHoldDistance,
                maxHoldDistance
            );
        }
    }

    void HoldObject()
    {
        Vector3 targetPos = cam.position + cam.forward * holdDistance;
        Vector3 direction = targetPos - heldObject.position;

        heldObject.linearVelocity = direction * moveSpeed;
    }

    void DropObject()
    {
        heldObject.useGravity = true;
        heldObject.linearDamping = 0f;
        heldObject = null;
    }
}