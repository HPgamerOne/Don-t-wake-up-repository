using UnityEngine;
using UnityEngine.InputSystem;

/*
Hantera spelare-kamera
*/

public class CameraController : MonoBehaviour
{
    [SerializeField] private InputActionReference lookAction;

    [Header("Camera")]
    [SerializeField] private float sensitivity = 0.2f;
    [SerializeField] private float mouseX;
    [SerializeField] private float mouseY;
    [SerializeField] private float xRotation;
    [SerializeField] private Vector2 mouseInput;

    [Header("GizmosRay")]
    [SerializeField] private float gizmosRayLength = 2f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleLooking();
    }

    private void HandleLooking()
    {
        mouseInput = lookAction.action.ReadValue<Vector2>();
        mouseX = mouseInput.x * sensitivity;
        mouseY = mouseInput.y * sensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.parent.Rotate(0, mouseX, 0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawRay(transform.position, transform.forward * gizmosRayLength);
    }
}