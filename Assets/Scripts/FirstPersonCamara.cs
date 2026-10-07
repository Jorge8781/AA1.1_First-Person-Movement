using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    public Camera playerCamera;
    public float sensitivity = 200f;

    public float minVerticalAngle = -75f;
    public float maxVerticalAngle = 75f;

    private float verticalRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);

        verticalRotation -= mouseY;

        verticalRotation = Mathf.Clamp(
            verticalRotation,
            minVerticalAngle,
            maxVerticalAngle
        );

        playerCamera.transform.localRotation =
            Quaternion.Euler(verticalRotation, 0f, 0f);
    }
}
