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
    private float mouseX;
    private float mouseY;
    private float xRotation;
    private Vector2 mouseInput;

    public bool active = true;

    /*
    [Header("GizmosRay")]
    [SerializeField] private float gizmosRayLength = 2f;
    */

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
        if (!active)
        {
            xRotation = 90f;
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            return;
        }

        mouseInput = lookAction.action.ReadValue<Vector2>();
        mouseX = mouseInput.x * sensitivity;
        mouseY = mouseInput.y * sensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.parent.Rotate(0, mouseX, 0);
    }

    /*
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawRay(transform.position, transform.forward * gizmosRayLength);
    }
    */
}