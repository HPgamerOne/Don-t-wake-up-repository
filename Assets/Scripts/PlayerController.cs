using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private InputActionReference moveAction, jumpAction, sprintAction;

    [Header("Movement")]
    [SerializeField] private bool isSprinting = false;
    [SerializeField] private float baseMoveSpeed = 10;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float sprintMultiplier = 1.5f;
    [SerializeField] private float jumpVelocity = 10f;
    [SerializeField] private float currentHorizontalSpeed;
    [SerializeField] private Vector2 moveInput;
    [SerializeField] private Vector3 velocity;
    [SerializeField] private Vector3 horizontalVelocity;

    [Header("Gravity")]
    [SerializeField] private float gravity = -9.82f;
    [SerializeField] private float gravityMultiplier = 3f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        moveSpeed = baseMoveSpeed;
    }

    void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        // Sprint
        if (sprintAction.action.ReadValue<float>() != 0)
        {
            moveSpeed = baseMoveSpeed * sprintMultiplier;
            isSprinting = true;
        }
        else
        {
            moveSpeed = baseMoveSpeed;
            isSprinting = false;
        }

        // Movement Input
        moveInput = moveAction.action.ReadValue<Vector2>();
        horizontalVelocity = new Vector3(moveInput.x * moveSpeed, 0, moveInput.y * moveSpeed);
        horizontalVelocity = transform.rotation * horizontalVelocity;

        // Gravity
        if (characterController.isGrounded)
        {
            velocity.y = -1;

            // Jump
            if (jumpAction.action.ReadValue<float>() != 0)
            {
                velocity.y = jumpVelocity;
            }
        }
        else
        {
            velocity.y += gravity * gravityMultiplier * Time.deltaTime;
        }

        velocity.x = horizontalVelocity.x;
        velocity.z = horizontalVelocity.z;

        currentHorizontalSpeed = horizontalVelocity.magnitude;

        characterController.Move(velocity * Time.deltaTime);
    }

    public float CurrentHorizontalSpeed()
    {
        return currentHorizontalSpeed;
    }

    public bool IsSprinting()
    {
        return isSprinting;
    }
}